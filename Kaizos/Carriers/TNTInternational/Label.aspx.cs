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
    public partial class Label : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string LabelXML = Session["Label"].ToString();
            // string[] Split = LabelXML.Split(new Char[] { '!' });
            Response.Clear();
            //for (int i = 1; i<=Split.Length-1; i++)
            //{
            XmlReader xmlReader = XmlReader.Create(new System.IO.StringReader(LabelXML));
            XslCompiledTransform XSLTransform = new XslCompiledTransform();
            XSLTransform.Load("http://iconnection.tnt.com:81/Shipper/NewStyleSheets/label.xsl");
            XSLTransform.Transform(xmlReader, null, Response.Output);
            //}
        }
    }
}