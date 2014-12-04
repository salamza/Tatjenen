<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
   <link href="LoginCss.css" rel="Stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    
    
        <table id="LayoutTable" class="ContainerTable" runat="server">
        <tr>

        <td>
        <asp:Panel ID="RegisterPanel" runat="server">
        <div id="RegisterDiv" class="FormDiv" runat="server">
        <asp:Label ID="RegisterStatus" runat="server" Visible="false" CssClass="Error" EnableViewState="False"></asp:Label>
        <table id="RegisterTable" class="FormTable" runat="server">
            <tr>
            <td><asp:Label Text="Username:" runat="server"></asp:Label></td>
            <td><asp:TextBox ID="NewUsername" runat="server"  ValidationGroup="RegisterGroup"></asp:TextBox><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ErrorMessage="You must enter a username." ControlToValidate="NewUsername" 
                    Display="Dynamic" ValidationGroup="RegisterGroup" CssClass="Error"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    ControlToValidate="NewUsername" CssClass="Error" Display="Dynamic" 
                    ErrorMessage="Username must be from 1 to 30 characters, and contain only letters, numbers or underscores." 
                    ValidationExpression="\w{1,30}" 
                    ValidationGroup="RegisterGroup"></asp:RegularExpressionValidator>
                </td>
            </tr>

            <tr>
            <td><asp:Label ID="Label3" Text="Password:" runat="server"></asp:Label></td>
            <td><asp:TextBox ID="NewPassword" runat="server" TextMode="Password"  ValidationGroup="RegisterGroup"></asp:TextBox><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ErrorMessage="You must enter a password." Display="Dynamic" 
                    ValidationGroup="RegisterGroup" CssClass="Error" 
                    ControlToValidate="NewPassword"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                    CssClass="Error" Display="Dynamic" 
                    ErrorMessage="Password must be from 1 to 30 characters, and contain only letters, numbers or underscores." 
                    ValidationExpression="\w{1,30}" ValidationGroup="RegisterGroup" 
                    ControlToValidate="NewPassword"></asp:RegularExpressionValidator>
                </td>
            </tr>

            <tr>
            <td><asp:Label ID="Label4" Text="Confirm password:" runat="server"></asp:Label></td>
            <td><asp:TextBox ID="PasswordConfirm" runat="server" TextMode="Password"  ValidationGroup="RegisterGroup"></asp:TextBox><br />
                
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="PasswordConfirm" CssClass="Error" Display="Dynamic" 
                    ErrorMessage="You must confirm your password." 
                    ValidationGroup="RegisterGroup"></asp:RequiredFieldValidator>

                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ErrorMessage="Passwords do not match." ControlToCompare="NewPassword" 
                    ControlToValidate="PasswordConfirm" CssClass="Error" Display="Dynamic" 
                    ValidationGroup="RegisterGroup"></asp:CompareValidator>
                </td>
            </tr>

            <tr>
            <td><asp:Label ID="Label5" Text="First Name:" runat="server"></asp:Label></td>
            <td><asp:TextBox ID="FirstName" runat="server"  ValidationGroup="RegisterGroup"></asp:TextBox></td>
            </tr>

            <tr>
            <td><asp:Label ID="Label6" Text="Last Name:" runat="server"></asp:Label></td>
            <td><asp:TextBox ID="LastName" runat="server"  ValidationGroup="RegisterGroup"></asp:TextBox></td>
            </tr>

            <tr>
            <td><asp:Label ID="Label7" Text="Email:" runat="server"></asp:Label></td>
            <td><asp:TextBox ID="Email" runat="server"  ValidationGroup="RegisterGroup"></asp:TextBox></td>
            </tr>

            <tr>
            <td><asp:Label ID="Label8" Text="Upload Avatar:" runat="server"></asp:Label></td>
            <td>
            <asp:FileUpload ID="Avatar" runat="server" /> <br />
            </td>
            </tr>
            
        </table>
        </div>
        <asp:Button ID="RegisterButton" Text="Register" runat="server" OnClick="RegisterClick" ValidationGroup="RegisterGroup" />
        </asp:Panel>
        </td>


        <td>
        <asp:Panel ID="LoginPanel" runat="server">
        <div id="LoginDiv" class="FormDiv" runat="server">
        <asp:Label ID="LoginStatus" runat="server" Visible="false" CssClass="Error" EnableViewState="False"></asp:Label>
        <table id="LoginTable" class="FormTable" runat="server">
            <tr>
            <td><asp:Label ID="Label1" runat="server" Text="Username: "></asp:Label></td>
            <td> <asp:TextBox ID="Username" runat="server" ValidationGroup="LoginGroup"></asp:TextBox><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                    ErrorMessage="You must enter a username." ControlToValidate="Username" 
                    CssClass="Error" Display="Dynamic" ValidationGroup="LoginGroup"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                    ControlToValidate="Username" CssClass="Error" Display="Dynamic" 
                    ErrorMessage="Username must be from 1 to 30 characters, and contain only letters, numbers or underscores." 
                    ValidationExpression="\w{1,30}" ValidationGroup="Login"></asp:RegularExpressionValidator>
                </td>
            </tr>

            <tr>
            <td><asp:Label ID="Label2" runat="server" Text="Password: "></asp:Label></td>
            <td><asp:TextBox ID="Password" runat="server" TextMode="Password" 
                    ValidationGroup="LoginGroup"></asp:TextBox><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                    ErrorMessage="You must enter a password." ControlToValidate="Password" 
                    CssClass="Error" Display="Dynamic" ValidationGroup="LoginGroup"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" 
                    ControlToValidate="Password" CssClass="Error" Display="Dynamic" 
                    ErrorMessage="Password must be from 1 to 30 characters, and contain only letters, numbers or underscores." 
                    ValidationExpression="\w{1,30}" ValidationGroup="LoginGroup"></asp:RegularExpressionValidator>
                </td>
            </tr>
        </table>
        <asp:CheckBox ID="KeepLoggedIn" runat="server" Text="Stay logged in" />
        </div>
        <asp:Button ID="LoginButton" Text="Login" runat="server" OnClick="LoginClick" 
                ValidationGroup="LoginGroup" />
        </asp:Panel>

        
        <br /><br />
        <asp:Panel ID="OtherLoginsPanel" runat="server">
        <div runat="server" id="OtherLoginsDiv" class="FormDiv">
            <asp:Label ID="SocialLoginStatus" runat="server" Visible="False" 
                CssClass="Error"></asp:Label>
            <asp:CheckBox ID="SocialKeepLoggedIn" runat="server" Text="Stay logged in" /><br />
            <asp:TextBox ID="TwitterUsername" runat="server" ValidationGroup="TwitterLogin" ></asp:TextBox>

            <asp:Button ID="TwitterLoginButton" runat="server" OnClick="TwitterLoginClick" 
                Text="Login with Twitter" ValidationGroup="TwitterLogin" />
            

            <br />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                ControlToValidate="TwitterUsername" CssClass="Error" Display="Dynamic" 
                ErrorMessage="Enter your twitter username.<br/>" ValidationGroup="TwitterLogin"></asp:RequiredFieldValidator>
            

            <asp:Button ID="FacebookLoginButton" OnClick="FacebookLoginClick" 
                runat="server" Text="Login with Facebook" Width="279px" />
        </div>

        </asp:Panel>
        </td>

        </tr>
        </table>

        
    </div>
    </form>
</body>
</html>
