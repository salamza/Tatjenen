using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Configuration;
using System.Data.SqlClient;

public partial class _Default : System.Web.UI.Page
{
    public string user;
    protected void Page_Load(object sender, EventArgs e)
    {
        string fname;
        string lname;
       

        int UserID = Int32.Parse(HttpContext.Current.User.Identity.Name);
        Session["user"] = UserID;
        SqlConnection con = new SqlConnection();
        con.ConnectionString = WebConfigurationManager.ConnectionStrings["UserDB"].ConnectionString;
        SqlCommand cmd = new SqlCommand("SELECT FirstName, LastName FROM Users WHERE UserID=@UserID", con);
        cmd.Parameters.AddWithValue("@UserID", UserID);
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataSet dataSet = new DataSet();
        con.Open();
        adapter.Fill(dataSet, "Users");
        con.Close();

        fname = (string)dataSet.Tables["Users"].Rows[0]["FirstName"];
        lname = (string)dataSet.Tables["Users"].Rows[0]["LastName"];
        user = fname + " " + lname;

    }
    protected void Article_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Articles.aspx");
    }
    protected void ViewArticles_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/ShowArticles.aspx");
    }
}
