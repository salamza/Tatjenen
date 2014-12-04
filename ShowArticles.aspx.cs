using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;


public partial class ShowArticles : System.Web.UI.Page
{
    public string ArticleText;
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
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {

    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        int ArticleID;
        if (DropDownList1.SelectedValue != "")
        {
            ArticleID = Int32.Parse(DropDownList1.SelectedValue);
            //  ArticleID = 2;
            SqlConnection con1 = new SqlConnection();
            con1.ConnectionString = WebConfigurationManager.ConnectionStrings["UserDB"].ConnectionString;
            SqlCommand cmd1 = new SqlCommand("SELECT ArticleText FROM Articles WHERE ArticleID=@ArticleID", con1);
            cmd1.Parameters.AddWithValue("@ArticleID", ArticleID);
            SqlDataAdapter adapter1 = new SqlDataAdapter(cmd1);
            DataSet dataSet1 = new DataSet();
            con1.Open();
            adapter1.Fill(dataSet1, "Articles");
            con1.Close();
            ArticleText = (string)dataSet1.Tables["Articles"].Rows[0]["ArticleText"];
         
        }
    }
   
}
