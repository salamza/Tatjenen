<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Profile</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <br />
        Welcome <%=user%> :
        <br />
        <br />
    
        <asp:Button ID="Article" runat="server" onclick="Article_Click" 
            Text="Add An Article" />
        <br />
        <br />
        <br />
        <br />
    
    </div>
    <p>
        <asp:Button ID="ViewArticles" runat="server" onclick="ViewArticles_Click" 
            Text="View Articles" />
    </p>
    </form>
</body>
</html>
