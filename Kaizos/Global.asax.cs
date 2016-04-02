using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using log4net;
using System.Web.Configuration; 

namespace Kaizos
{
  public class Global : System.Web.HttpApplication
  {

    protected void Application_Start(object sender, EventArgs e)
    {
      log4net.Config.XmlConfigurator.Configure();
     
     

    }

    protected void Session_Start(object sender, EventArgs e)
    {

        string strRequestedPage = HttpContext.Current.Request.Url.LocalPath;
        if (WebConfigurationManager.AppSettings["Emergency"].ToString().Equals("Off"))
        {
            Server.Transfer("frmResultLogin.aspx", false);
        }
        if (!strRequestedPage.Trim().Equals("/frmLogin.aspx"))
        {
            if (strRequestedPage.Trim().Contains("/frmActivate.aspx"))
            {
                Server.Transfer("frmActivate.aspx", false);
            }
            if (strRequestedPage.Trim().Contains("/frmConfirmPassword.aspx"))
            {
                Server.Transfer("frmConfirmPassword.aspx", false);
            }
            if (!strRequestedPage.Trim().Contains("/frmActivate.aspx") || !strRequestedPage.Trim().Contains("/frmConfirmPassword.aspx"))
            {
                Server.Transfer("frmLogin.aspx", false);
            }
        }
        
    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {

    }

    protected void Application_AuthenticateRequest(object sender, EventArgs e)
    {

    }

    protected void Application_Error(object sender, EventArgs e)
    {

    }

    protected void Session_End(object sender, EventArgs e)
    {

    }

    protected void Application_End(object sender, EventArgs e)
    {

    }
  }
}