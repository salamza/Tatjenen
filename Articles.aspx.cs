using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;

public partial class _Default : System.Web.UI.Page
{
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string Name = TextBox1.Text; 
        lblDisplay.Visible = true;
        pnlEditor.Visible = false;
        lblDisplay.Text = RichTextBox.Text;
        btnSave.Visible = false;
        btnCancel.Visible = true;
        String Article = RichTextBox.Text;
        String user = HttpContext.Current.User.Identity.Name ;
        Int32 UserID = Int32.Parse(user);
        SqlConnection data = new SqlConnection();
        data.ConnectionString = WebConfigurationManager.ConnectionStrings["UserDB"].ConnectionString;
        SqlCommand c = new SqlCommand();
        c.Connection = data;
        if (Name!="")
        c.CommandText = "INSERT INTO Articles (UserID,ArticleTitle,ArticleText) VALUES (@UserID,@Name,@Article)";
        else
            c.CommandText = "INSERT INTO Articles (UserID,ArticleText) VALUES (@UserID,@Article)";
        c.Parameters.AddWithValue("@UserID", UserID);
        c.Parameters.AddWithValue("@Article", Article);
        c.Parameters.AddWithValue("@Name", Name);
        data.Open();
        c.ExecuteNonQuery();
        data.Close();

        Response.Redirect("~/Default.aspx");



    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        lblDisplay.Visible = false;
        pnlEditor.Visible = true;
        lblDisplay.Text = "";
        RichTextBox.Text = "";
        btnSave.Visible = true;
        btnCancel.Visible = false;
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            string FileName = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
            string FilePath = "images/" + FileName;
            FileUpload1.SaveAs(Server.MapPath(FilePath));
            RichTextBox.Text += string.Format("<img src = '{0}' alt = '{1}' />", FilePath, FileName);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
