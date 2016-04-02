using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kaizos
{
    public partial class frmLogout : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Page.Title = GetGlobalResourceObject("LocalString", "frmLogOut").ToString();
        }
    }
}