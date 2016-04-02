using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kaizos
{
    public class BasePage : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            if (KaizosSession.Current.UserResourceFile !=null) UICulture = KaizosSession.Current.UserResourceFile;
            base.InitializeCulture();
        }
    }
}