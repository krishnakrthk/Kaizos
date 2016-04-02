using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.ServiceModel;
using System.ServiceModel.Channels;
using KaizosServiceInvokers.KaizosServiceReference;

using OnBarcode.Barcode;

namespace GLSFinal.Carrier
{
    public partial class frmManifest : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            display a = new display();
            if (a.value == "Normal")
            {

               // ReportViewer2.LocalReport.ReportPath = @"Carrier\GLS\ManifestReport.rdlc";
                ObjectDataSource1.TypeName = "GLSFinal.Carrier.display";
                ObjectDataSource1.SelectMethod = "NormalLbl";
                ReportViewer2.Visible = true;

            }
            else if (a.value == "Emergency")
            {

                //ReportViewer2.LocalReport.ReportPath = @"Carrier\GLS\ManifestReport.rdlc";
                ObjectDataSource1.TypeName = "GLSFinal.Carrier.display";
                ObjectDataSource1.SelectMethod = "EmergencyLbl";
                ReportViewer2.Visible = true;

            }
            else if (a.value == "Zipcode Error")
            {
                Response.Write("Error in Zipcode");
                ReportViewer2.Visible = false;

            }
            else if (a.value == "Countrycode Error")
            {

                Response.Write("Error in Country Code");
                ReportViewer2.Visible = false;

            }
            
        }
    }

    

   
}

            
      