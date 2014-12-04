using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Twitterizer;
using Twitterizer.Core;
using Twitterizer.Entities;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

public partial class TwitterCallback : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string consumerKey = System.Configuration.ConfigurationManager.AppSettings["TwitterConsumerKey"];
        string consumerSecret = System.Configuration.ConfigurationManager.AppSettings["TwitterConsumerSecret"];

        OAuthTokenResponse accessTokenResponse = OAuthUtility.GetAccessTokenDuringCallback(consumerKey, consumerSecret);
        string accessToken = accessTokenResponse.Token;
        string accessSecret = accessTokenResponse.TokenSecret;
        string username = "@" + accessTokenResponse.ScreenName;
        int id = (int)accessTokenResponse.UserId;

        
        Status.Text += "Access Token: " + accessToken;
        Status.Text += "Access Secret: " + accessSecret;
        Status.Text += "Username: " + username;
        Status.Text += "ID: " + id.ToString();

        SqlConnection con = new SqlConnection();
        con.ConnectionString = WebConfigurationManager.ConnectionStrings["UserDB"].ConnectionString;
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;

        cmd.Parameters.AddWithValue("@Username", username);
        cmd.Parameters.AddWithValue("@Password", accessToken + id.ToString());
        cmd.Parameters.AddWithValue("@JoinDate", DateTime.Now);
        cmd.Parameters.AddWithValue("@TwitterUsername", accessTokenResponse.ScreenName);
        cmd.Parameters.AddWithValue("@TwitterID", id);
        cmd.Parameters.AddWithValue("@AccessToken", accessToken);
        cmd.Parameters.AddWithValue("@AccessSecret", accessSecret);

        cmd.CommandText = "INSERT INTO Users (Username, Password, JoinDate) VALUES (@Username, @Password, @JoinDate)";

        string getIDCmd = "SELECT UserID FROM Users WHERE Username=@Username";
        string insertTwitterCmd = "INSERT INTO TwitterUser (UserID, TwitterID, TwitterUsername, AccessToken, AccessSecret)" +
            "VALUES (@UserID, @TwitterID, @TwitterUsername, @AccessToken, @AccessSecret)";

        try
        {
            con.Open();
            int success = cmd.ExecuteNonQuery();
            if (success == 1)
            {
                Status.Text += "@" + accessTokenResponse.ScreenName + " inserted into Users table.";
            }

            cmd.CommandText = getIDCmd;
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            reader.Read();
            int UserID = (int)reader["UserID"];
            //int UserIDInt = Int32.Parse(UserID);
            reader.Close();

            Status.Text += "<br/>Got UserID: " + UserID.ToString();

            cmd.CommandText = insertTwitterCmd;
            cmd.Parameters.AddWithValue("@UserID", UserID);
            success = cmd.ExecuteNonQuery();
            if (success == 1)
            {
                Status.Text += "<br/>@" + accessTokenResponse.ScreenName + " inserted into TwitterUser table.";
                if (Request.QueryString["StayLoggedIn"] != null)
                {
                    Logging.LoginWithUserID(this, UserID.ToString(), Convert.ToBoolean(Request.QueryString["StayLoggedIn"]));
                }
                else 
                {
                    Logging.LoginWithUserID(this, UserID.ToString(), false);
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