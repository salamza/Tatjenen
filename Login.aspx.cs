using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Security;
using System.Data.SqlTypes;
using Twitterizer;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void RegisterClick(object sender, EventArgs e)
    {
        string username = NewUsername.Text;
        string firstName = FirstName.Text;
        string lastName = LastName.Text;
        string email = Email.Text;
        string password = NewPassword.Text;
        int Score = 0;

        SqlConnection con = new SqlConnection();
        con.ConnectionString = WebConfigurationManager.ConnectionStrings["UserDB"].ConnectionString;
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;

        string fields = "Username, Password, JoinDate ,Score";
        string parameters = "@Username, @Password, @JoinDate , @Score";
        cmd.Parameters.AddWithValue("@Username", username);
        cmd.Parameters.AddWithValue("@Password", password);
        cmd.Parameters.AddWithValue("@JoinDate", DateTime.Now);
        cmd.Parameters.AddWithValue("@Score", Score);

        if (Avatar.PostedFile != null && Avatar.PostedFile.FileName != "")
        {
            byte[] buffer = new byte[Avatar.PostedFile.ContentLength];
            HttpPostedFile uploadedImage = Avatar.PostedFile;
            uploadedImage.InputStream.Read(buffer, 0, Avatar.PostedFile.ContentLength);
            fields += ", Avatar";
            parameters += ", @Avatar";

            SqlParameter avatarParam = new SqlParameter("@Avatar", SqlDbType.Image, buffer.Length);
            avatarParam.Value = buffer;
            cmd.Parameters.Add(avatarParam);
        }

        if (firstName != "")
        {
            fields += ", FirstName";
            parameters += ", @FirstName";
            cmd.Parameters.AddWithValue("@FirstName", firstName);
        }
        if (lastName != "")
        {
            fields += ", LastName";
            parameters += ", @LastName";
            cmd.Parameters.AddWithValue("@LastName", lastName);
        }
        if (email != "")
        {
            fields += ", Email";
            parameters += ", @Email";
            cmd.Parameters.AddWithValue("@Email", email);
        }

        string cmdString = "INSERT INTO Users (" + fields + ") VALUES (" + parameters + ")";
        cmd.CommandText = cmdString;

        try
        {
            con.Open();
            int success = cmd.ExecuteNonQuery();
            if (success == 1)
            {
                RegisterStatus.Text = "Registration successful.";
                //RegisterStatus.Text += "<br />Username: " + username;
                //RegisterStatus.Text += "<br />Password: " + password;
                RegisterStatus.Visible = true;
            }
        }
        catch (Exception err)
        {
            RegisterStatus.Text = "Error: " + err.Message;
            //RegisterStatus.Text += "<br />Username: " + username;
            //RegisterStatus.Text += "<br />Password: " + password;
            RegisterStatus.Visible = true;
        }
        finally
        {
            con.Close();
        }
    }

    protected void LoginClick(object sender, EventArgs e)
    {
        string password = Password.Text;
        string username = Username.Text;

        SqlConnection con = new SqlConnection();
        con.ConnectionString = WebConfigurationManager.ConnectionStrings["UserDB"].ConnectionString;
        SqlCommand cmd = new SqlCommand("SELECT Password, UserID FROM Users WHERE Username=@Username", con);
        cmd.Parameters.AddWithValue("@Username", username);
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataSet dataSet = new DataSet();

        try
        {
            con.Open();
            adapter.Fill(dataSet, "Users");
        }
        catch (Exception err)
        {
            LoginStatus.Text = "Error: " + err.Message;
            LoginStatus.Visible = true;
        }
        finally
        {
            con.Close();
        }

        if (dataSet.Tables["Users"].Rows.Count == 0)
        {
            LoginStatus.Text = "The username doesn't exist.";
            LoginStatus.Visible = true;
        }

        else if ((string)dataSet.Tables["Users"].Rows[0]["Password"] != password)
        {
            LoginStatus.Text = "Incorrect password and/or username.";
            LoginStatus.Visible = true;
        }

        else
        {
            Logging.LoginWithUserID(this, dataSet.Tables["Users"].Rows[0]["UserID"].ToString(), KeepLoggedIn.Checked);
            LoginStatus.Text = "Login successful.";
            LoginStatus.Visible = true;
        }
    }

    protected void TwitterLoginClick(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = WebConfigurationManager.ConnectionStrings["UserDB"].ConnectionString;
        SqlCommand cmd = new SqlCommand("SELECT UserID, TwitterUsername FROM TwitterUser WHERE TwitterUsername=@TwitterUsername", con);
        cmd.Parameters.AddWithValue("@TwitterUsername", TwitterUsername.Text);
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataSet dataSet = new DataSet();

        try
        {
            con.Open();
            adapter.Fill(dataSet, "TwitterUser");
        }
        catch (Exception err)
        {
            LoginStatus.Text = "Error: " + err.Message;
            LoginStatus.Visible = true;
        }
        finally
        {
            con.Close();
        }

        if (dataSet.Tables["TwitterUser"].Rows.Count == 0)
        {
            string consumerKey = System.Configuration.ConfigurationManager.AppSettings["TwitterConsumerKey"];
            string consumerSecret = System.Configuration.ConfigurationManager.AppSettings["TwitterConsumerSecret"];

            string callbackUrl = "http://localhost:" + Logging.Port + "/" + Logging.WebsiteName + "/TwitterCallback.aspx?StayLoggedIn=" + SocialKeepLoggedIn.Checked.ToString();
            string requestToken = OAuthUtility.GetRequestToken(consumerKey, consumerSecret, callbackUrl).Token;
            Uri authenticationUri = OAuthUtility.BuildAuthorizationUri(requestToken);
            Response.Redirect(authenticationUri.AbsoluteUri);
        }

        else
        {
            Logging.LoginWithUserID(this, dataSet.Tables["TwitterUser"].Rows[0]["UserID"].ToString(), SocialKeepLoggedIn.Checked);
            //SocialLoginStatus.Text = "Welcome, @" + dataSet.Tables["TwitterUser"].Rows[0]["TwitterUsername"].ToString();
            //SocialLoginStatus.Visible = true;
        }
    }


    

    protected void FacebookLoginClick(object sender, EventArgs e)
    {
        string OAuthDialog = "https://www.facebook.com/dialog/oauth?";
        string responseURL = OAuthDialog + "client_id="
            + System.Configuration.ConfigurationManager.AppSettings["FacebookAppID"]
            + "&redirect_uri=" + "http://localhost:" + Logging.Port + "/"
            + Logging.WebsiteName + "/" + "FacebookCallback.aspx";
        Response.Redirect(responseURL);
        
    }
}