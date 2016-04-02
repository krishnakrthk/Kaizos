using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration; 
namespace Kaizos
{
    public class BasePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebConfigurationManager.AppSettings["Emergency"].ToString().Equals("Off"))
            {
                Server.Transfer("frmResultLogin.aspx", false);
            }

        }

        protected override void InitializeCulture()
        {
            if (KaizosSession.Current.UserResourceFile !=null) UICulture = KaizosSession.Current.UserResourceFile;
            base.InitializeCulture();
        }
    }
}