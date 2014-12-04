using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

public partial class FacebookCallback : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string tokenEndpoint = "https://graph.facebook.com/oauth/access_token?";
        string graphAPI = "https://graph.facebook.com/me?access_token=";

        if (Request.QueryString["code"] == null)
        {
            Status.Text += "\"code\" (value expected in query string) is null. Login failed.";
        }

        else
        {
            string code = Request.QueryString["code"];
            Status.Text += "<br/>The code: " + code;
            string tokenEndPointURL = tokenEndpoint
                + "&client_id=" + System.Configuration.ConfigurationManager.AppSettings["FacebookAppID"]
                + "&redirect_uri=" + "http://localhost:" + Logging.Port + "/"
                + Logging.WebsiteName + "/FacebookCallback.aspx"
                + "&client_secret=" + System.Configuration.ConfigurationManager.AppSettings["FacebookAppSecret"]
                + "&code=" + code;

            System.Net.WebClient wc = new System.Net.WebClient();
            byte[] accessTokenBuffer = wc.DownloadData(tokenEndPointURL);
            string tokenEndPointResponse = System.Text.Encoding.ASCII.GetString(accessTokenBuffer);
            Status.Text += "<br/>Response #1: " + tokenEndPointResponse;

            Regex regEx1 = new Regex("(\\w+)=([^&]+)", RegexOptions.Compiled);
            Dictionary<string, string> dict1 = new Dictionary<string, string>();
            foreach (Match m in regEx1.Matches(tokenEndPointResponse))
            {
                Status.Text += "<br/>" + m.Groups[1].Value + ": " + m.Groups[2].Value;
                dict1.Add(m.Groups[1].Value, m.Groups[2].Value);
            }

            try
            {
                string graphURL = graphAPI + dict1["access_token"];
                byte[] graphBuffer = wc.DownloadData(graphURL);
                string graphResponse = System.Text.Encoding.ASCII.GetString(graphBuffer);
                graphResponse = graphResponse.Substring(1, graphResponse.Length - 2);

                Regex re = new Regex("\"([\\w\\s]+)\":\"([\\w\\s,\\\\\\/\\.:]+)\"", RegexOptions.Compiled);
                Dictionary<string, string> dict2 = new Dictionary<string, string>();
                foreach (Match m in re.Matches(graphResponse))
                {
                    Status.Text += "<br/>" + m.Groups[1].Value + ": " + m.Groups[2].Value;
                    if (!dict2.ContainsKey(m.Groups[1].Value))
                    {
                        dict2.Add(m.Groups[1].Value, m.Groups[2].Value);
                    }
                }
                AddUserToDatabase(dict2);
            }
            catch (Exception err)
            {
                Status.Text += "<br/>Error: " + err.Message;
            }
        }
    }

    protected void AddUserToDatabase(Dictionary<string, string> dict)
    {
        bool keepLoggedIn = false;
        if (Request.QueryString["KeepLoggedIn"] != null)
        {
            keepLoggedIn = Convert.ToBoolean(Request.QueryString["KeepLoggedIn"]);
        }

        SqlConnection con = new SqlConnection();
        con.ConnectionString = WebConfigurationManager.ConnectionStrings["UserDB"].ConnectionString;
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();

        int facebookID = Int32.Parse(dict["id"]);
        string checkExistingCmd = "SELECT UserID FROM FacebookUsers WHERE FacebookID=@FacebookID";
        cmd.CommandText = checkExistingCmd;
        cmd.Parameters.AddWithValue("@FacebookID", facebookID);

        try
        {
            con.Open();
            adapter.Fill(ds, "FacebookUsers");
            if (ds.Tables["FacebookUsers"].Rows.Count != 0)
            {
                Logging.LoginWithUserID(this, ds.Tables["FacebookUsers"].Rows[0]["UserID"].ToString(), keepLoggedIn);
            }
            else 
            {
                string fields = "Username, Password, JoinDate";
                string parameters = "@Username, @Password, @JoinDate";
                string username = "";
                if (dict.ContainsKey("link"))
                {
                    string link = dict["link"];
                    username = "%" + link.Substring(link.LastIndexOf("/") + 1);
                }
                else
                {
                    Status.Text += "Error: \"link\" not found.";
                }
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", "SomeSecretMessage" + facebookID.ToString());
                cmd.Parameters.AddWithValue("@JoinDate", DateTime.Now);

                if (dict.ContainsKey("first_name"))
                {
                    fields += ", FirstName";
                    parameters += ", @FirstName";
                    cmd.Parameters.AddWithValue("@FirstName", dict["first_name"]);
                }
                if (dict.ContainsKey("last_name"))
                {
                    fields += ", LastName";
                    parameters += ", @LastName";
                    cmd.Parameters.AddWithValue("@LastName", dict["last_name"]);
                }
                string insertUserCmd = string.Format("INSERT INTO Users ({0}) VALUES ({1})", fields, parameters);
                cmd.CommandText = insertUserCmd;
                int success = cmd.ExecuteNonQuery();
                if (success != 1)
                {
                    Status.Text += "<br/>Error: Not added to user database";
                }
                else
                {
                    Status.Text += "<br/>Successfully added to user database";

                    cmd.CommandText = "SELECT UserID FROM Users WHERE Username=@Username";
                    adapter.Fill(ds, "Users");
                    int UserID = (int)ds.Tables["Users"].Rows[0]["UserID"];

                    Status.Text += "<br/>UserID: " + UserID.ToString();

                    cmd.CommandText = "INSERT INTO FacebookUsers (FacebookID, UserID) VALUES (@FacebookID, @UserID)";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    success = cmd.ExecuteNonQuery();
                    if (success != 1)
                    {
                        Status.Text += "<br/>Error: Not added to facebook database";
                    }
                    else
                    {
                        Status.Text += "<br/>Successfully added to facebook database.";
                        Logging.LoginWithUserID(this, UserID.ToString(), keepLoggedIn);
                    }
                }
            }
        }
        catch (Exception err)
        {
            Status.Text += "Error: " + err.Message;
        }
        finally
        {
            con.Close();
        }
    }
}