<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowArticles.aspx.cs" Inherits="ShowArticles" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <%=user%> Articles :
    </div>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:DatabaseConnectionString1 %>" 
        DeleteCommand="DELETE FROM [Articles] WHERE [ArticleID] = @ArticleID" 
        InsertCommand="INSERT INTO [Articles] ([UserID], [ArticleTitle], [ArticleText]) VALUES (@UserID, @AtricleTitle, @ArticleText)" 
        ProviderName="<%$ ConnectionStrings:DatabaseConnectionString1.ProviderName %>" 
        SelectCommand="SELECT [ArticleID], [UserID], [ArticleTitle], [ArticleText] FROM [Articles] WHERE ([UserID] = @UserID)" 
        UpdateCommand="UPDATE [Articles] SET [UserID] = @UserID, [ArticleTitle] = @ArticleTitle, [ArticleText] = @ArticleText WHERE [ArticleID] = @ArticleID">
        <SelectParameters>
            <asp:SessionParameter Name="UserID" SessionField="user" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="ArticleID" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="UserID" Type="Int32" />
            <asp:Parameter Name="ArticleTitle" Type="String" />
            <asp:Parameter Name="ArticleText" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="UserID" Type="Int32" />
            <asp:Parameter Name="ArticleTitle" Type="String" />
            <asp:Parameter Name="ArticleText" Type="String" />
            <asp:Parameter Name="ArticleID" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:DropDownList ID="DropDownList1" runat="server" AppendDataBoundItems="True" 
        AutoPostBack="True" CausesValidation="True" DataSourceID="SqlDataSource2" 
        DataTextField="ArticleTitle" DataValueField="ArticleID" 
        onselectedindexchanged="DropDownList1_SelectedIndexChanged" 
        OnPreRender="DropDownList1_SelectedIndexChanged" style="width: 88px">
    </asp:DropDownList>
    <asp:Panel ID="Label1" runat="server" Text="Label" BorderStyle="Groove" 
          Width="800px" Height="500px" Scrollbars="Both" >
          <%=ArticleText%>
    </asp:Panel>
    
    <br />
    <br />
    <br />
    &nbsp;<br />
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
        ConnectionString="<%$ ConnectionStrings:DatabaseConnectionString1 %>" 
        ProviderName="<%$ ConnectionStrings:DatabaseConnectionString1.ProviderName %>" 
        SelectCommand="SELECT DISTINCT ArticleText, ArticleID FROM Articles WHERE (ArticleID = @ArticleID)">
        <SelectParameters>
            <asp:ControlParameter ControlID="DropDownList1" Name="ArticleID" 
                PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <p>
        &nbsp;</p>
    </form>
</body>
</html>
