using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kaizos
{
    public partial class WebForm1 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!KaizosSession.Current.ReturnURL.Trim().Equals("") &&
                    !KaizosSession.Current.ErrorMessage.Trim().Equals(""))
                {
                    lbReturnUrl.PostBackUrl = KaizosSession.Current.ReturnURL;
                    lblDisplayMsg.Text = KaizosSession.Current.ErrorMessage;
                }
                else
                {
                    lbReturnUrl.PostBackUrl = Request.Path;
                    lblDisplayMsg.Text = String.Format(GetGlobalResourceObject("LocalString", "ResultErrorMessage").ToString(), Request.Path);
                }
            }
        }
    }
}