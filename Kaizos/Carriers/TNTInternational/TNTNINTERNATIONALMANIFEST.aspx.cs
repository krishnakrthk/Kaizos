using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace TNTINTERNATIONAL.carrier
{
    public partial class TNTNINTERNATIONALMANIFEST : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string Manifest = Session["ManifetstTNTINT"].ToString();
            Response.Clear();

            if (Manifest.Contains("http://iconnection.tnt.com:81/Shipper/NewStyleSheets/manifest.xsl"))
            {

                XmlReader xmlReader = XmlReader.Create(new System.IO.StringReader(Manifest));
                XslCompiledTransform XSLTransform = new XslCompiledTransform();
                XSLTransform.Load("http://iconnection.tnt.com:81/Shipper/NewStyleSheets/manifest.xsl");
                XSLTransform.Transform(xmlReader, null, Response.Output);
            }
            else if (Manifest.Contains("http://iconnection.tnt.com:81/Shipper/NewStyleSheets/summary-manifest.xsl"))
            {
                XmlReader xmlReader = XmlReader.Create(new System.IO.StringReader(Manifest));
                XslCompiledTransform XSLTransform = new XslCompiledTransform();
                XSLTransform.Load("http://iconnection.tnt.com:81/Shipper/NewStyleSheets/summary-manifest.xsl");
                XSLTransform.Transform(xmlReader, null, Response.Output);
            }
            

        }
    }
}