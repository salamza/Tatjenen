using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

/// <summary>
/// Summary description for Logging
/// </summary>
public class Logging
{
    protected static string port = "49680";
    protected static string websiteName = "LoginPages";
    public static string Port
    {
        get
        {
            return port;
        }
        set
        {
            port = value;
        }
    }
    public static string WebsiteName
    {
        get
        {
            return websiteName;
        }
        set
        {
            websiteName = value;
        }
    }
	public Logging()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static void LoginWithUserID(System.Web.UI.Page sender, string UserID, bool KeepLoggedIn)
    {
        if (!KeepLoggedIn)
        {
            FormsAuthentication.RedirectFromLoginPage(UserID, false);
        }

        else
        {
            int timeOut = (int)TimeSpan.FromDays(30).TotalMinutes;
            FormsAuthenticationTicket ticket = new
                FormsAuthenticationTicket(UserID, true, timeOut);
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.Expires = ticket.Expiration;
            HttpContext.Current.Response.Cookies.Set(cookie);
            string requestedPage =
                FormsAuthentication.GetRedirectUrl(UserID, false);
            sender.Response.Redirect(requestedPage, true);
        }
    }
}