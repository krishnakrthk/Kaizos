﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kaizos
{
    public partial class WebForm2 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Page.Title = GetGlobalResourceObject("LocalString", "frmMain").ToString();
        }
    }
}