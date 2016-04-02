using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace TNTNATIONAL.carrier
{
    public partial class LabelTNTnat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            byte[] LabelXML = (byte[])Session["PDfLabel"];
            //Response.ContentType = "image/gif";
            //Response.BinaryWrite(LabelXML);
            Response.Clear();
            MemoryStream ms = new MemoryStream(LabelXML);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=TNTNationalLabel.pdf");
            Response.Buffer = true;
            ms.WriteTo(Response.OutputStream);
            HyperLink hh = new HyperLink();
            Page.Controls.Add(hh);
            hh.Text = "Return";
            hh.NavigateUrl = "frmcarrierLabelDisplay.aspx";
            Response.End();
            



        }
    }
}