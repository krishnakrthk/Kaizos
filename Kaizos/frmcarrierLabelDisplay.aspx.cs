using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;


namespace Kaizos
{
    public partial class frmcarrierLabelDisplay : System.Web.UI.Page
    {
      
        public string ship;
        public  List<SCarrierOutput> ss;
        public List<hh> collec;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmcarrierLabelDisplay").ToString();

                if (!(Session["shipresult"]==null))
                {
                    if (Session["shipresult"].ToString() != null)
                    {
                        string sp = Session["shipresult"].ToString();
                        //Session["shipresult"] = null;
                        string ShipDetail = sp.Trim();
                        int len = ShipDetail.Length - 1;
                        ShipDetail = ShipDetail.Substring(0, ShipDetail.Length - 1);
                        // ShipDetail="1189!1190!1195";
                        KaizosServiceAgent proxy = new KaizosServiceAgent();
                        ss = proxy.GetCarrierOutput(ShipDetail);
                        Session["ss"] = ss;
                        collec = new List<hh>();
                        foreach (SCarrierOutput aa in ss)
                        {
                            hh nn = new hh();
                            nn.Carrier = aa.Carrier;
                            nn.ShippingReference = aa.ShippingReference;
                            collec.Add(nn);
                        }
                        GridView1.DataSource = collec;
                        GridView1.DataBind();
                    }
                }
                else
                    Response.Redirect("frmTariffDelayInterrogation.aspx");
            }
        }

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    //if (!IsPostBack)
        //    //{
        //    string sp = Session["shipresult"].ToString();
        //   // Session["shipresult"] = null;
        //    string ShipDetail = sp.Trim();
        //    int len = ShipDetail.Length - 1; 
        //    ShipDetail = ShipDetail.Substring(0,ShipDetail.Length - 1);
        //    // ShipDetail="1189!1190!1195";
        //        KaizosServiceAgent proxy = new KaizosServiceAgent();
        //        ss = proxy.GetCarrierOutput(ShipDetail);
        //        Session["ss"] = ss;
        //         collec = new List<hh>();
        //   foreach (SCarrierOutput aa in ss )
        //   {
        //       hh nn = new hh();
        //       nn.Carrier = aa.Carrier;
        //       nn.ShippingReference = aa.ShippingReference;
        //       collec.Add(nn);
        //   }
        //   GridView1.DataSource = collec;
        //   GridView1.DataBind();
            
        //    //}
        //}

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region carrierlabel
            if (e.CommandName == "GoEdit")
            {
                //Server.Transfer("frmCarrier.aspx");
                try
                {
                    GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                    ship = GridView1.DataKeys[row.RowIndex].Value.ToString();
                       string url = string.Empty;
                       ss=(List<SCarrierOutput>)Session["ss"];
                       foreach (SCarrierOutput carrier in ss )
                       {
                           if (carrier.ShippingReference == ship)
                           {
                               if (carrier.Carrier == "GLS")
                               {
                                   //url = ResolveUrl(@"~\Carriers\GLS\GLSCarrier.aspx");
                                   System.Web.HttpContext.Current.Session["Label"] = carrier.Label;
                                   //string url1 = Request.Url.AbsolutePath.ToString();
                                   //string url2 = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.IndexOf(Request.Url.LocalPath));
                                   //System.Web.HttpContext.Current.Response.Write("<script>");
                                   //System.Web.HttpContext.Current.Response.Write(@"window.open('" + url2 + url + "','_blank')");
                                   //System.Web.HttpContext.Current.Response.Write("</script>");
                                   Server.Transfer(@"~\Carriers\GLS\GLSCarrier.aspx");
                               }
                               else if (carrier.Carrier == "TNTINTERNATIONAL")
                               {
                                   //url = ResolveUrl(@"~\Carriers\TNTInternational\Label.aspx");
                                   System.Web.HttpContext.Current.Session["Label"] = carrier.Label;
                                   //string url1 = Request.Url.AbsolutePath.ToString();
                                   //string url2 = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.IndexOf(Request.Url.LocalPath));
                                   //System.Web.HttpContext.Current.Response.Write("<script>");
                                   //System.Web.HttpContext.Current.Response.Write(@"window.open('" + url2 + url + "','_blank')");
                                   //System.Web.HttpContext.Current.Response.Write("</script>");
                                   Server.Transfer(@"~\Carriers\TNTInternational\Label.aspx");
                               }
                               else if (carrier.Carrier == "TNTNATIONAL")
                               {
                                   //url = ResolveUrl(@"~\Carriers\TNTNational\LabelTNTnat.aspx");
                                   Session["PDfLabel"] = carrier.LabelByte;
                                   //string url1 = Request.Url.AbsolutePath.ToString();
                                   //string url2 = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.IndexOf(Request.Url.LocalPath));
                                   ////System.Web.HttpContext.Current.Response.Redirect(@"Carr\About.aspx");
                                   ////Page page = (Page)HttpContext.Current.Handler;
                                   ////string url = page.ResolveClientUrl(@"Carr/About.aspx");
                                   //System.Web.HttpContext.Current.Response.Write("<script>");
                                   //System.Web.HttpContext.Current.Response.Write(@"window.open('" + url2 + url + "','_blank')");
                                   //System.Web.HttpContext.Current.Response.Write("</script>");
                                   Server.Transfer(@"~\Carriers\TNTNational\LabelTNTnat.aspx");
                               }
                           }
                       }
               }

                catch (Exception ee)
                {
                    string message = ee.Message;
                }
            }
 #endregion 

            #region Manifest
            if (e.CommandName == "GoEdit2")
            {
                GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                ship = GridView1.DataKeys[row.RowIndex].Value.ToString();
                string url = string.Empty;
                ss = (List<SCarrierOutput>)Session["ss"];
                foreach (SCarrierOutput carrier in ss)
                {
                    if (carrier.ShippingReference == ship)
                    {
                        if (carrier.Carrier == "GLS")
                        {
                            //url = ResolveUrl(@"~\Carriers\GLS\frmManifest.aspx");
                            System.Web.HttpContext.Current.Session["Label"] = carrier.Label;
                            Server.Transfer(@"~\Carriers\GLS\frmManifest.aspx");
                            //string url1 = Request.Url.AbsolutePath.ToString();
                            //string url2 = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.IndexOf(Request.Url.LocalPath));
                            //System.Web.HttpContext.Current.Response.Write("<script>");
                            //System.Web.HttpContext.Current.Response.Write(@"window.open('" + url2 + url + "','_blank')");
                            //System.Web.HttpContext.Current.Response.Write("</script>");
                        }
                        else if (carrier.Carrier == "TNTINTERNATIONAL")
                        {
                            //url = ResolveUrl(@"~\Carriers\TNTInternational\TNTNINTERNATIONALMANIFEST.aspx");
                            System.Web.HttpContext.Current.Session["ManifetstTNTINT"] = carrier.Manifest;
                            Server.Transfer(@"~\Carriers\TNTInternational\TNTNINTERNATIONALMANIFEST.aspx");
                            //string url1 = Request.Url.AbsolutePath.ToString();
                            //string url2 = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.IndexOf(Request.Url.LocalPath));
                            //System.Web.HttpContext.Current.Response.Write("<script>");
                            //System.Web.HttpContext.Current.Response.Write(@"window.open('" + url2 + url + "','_blank')");
                            //System.Web.HttpContext.Current.Response.Write("</script>");
                        }
                        else if (carrier.Carrier == "TNTNATIONAL")
                        {
                            //url = ResolveUrl(@"~\Carriers\TNTNational\ManifestTNTNat.aspx");
                            System.Web.HttpContext.Current.Session["ManifetstTNTNAT"] = carrier.Manifest;
                            Server.Transfer(@"~\Carriers\TNTNational\ManifestTNTNat.aspx");
                            //string url1 = Request.Url.AbsolutePath.ToString();
                            //string url2 = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.IndexOf(Request.Url.LocalPath));
                            //System.Web.HttpContext.Current.Response.Write("<script>");
                            //System.Web.HttpContext.Current.Response.Write(@"window.open('" + url2 + url + "','_blank')");
                            //System.Web.HttpContext.Current.Response.Write("</script>");
                        }
                    }
                }

            }
            #endregion

        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Session["shipresult"]= null;
            LinkButton1.PostBackUrl = "~/frmTariffDelayInterrogation.aspx";
        }

    }

    public class hh
    {
        public string ShippingReference { get; set; }
        public string Carrier { get; set; }
    }
}