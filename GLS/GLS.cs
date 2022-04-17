using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Net;

using System.IO;
using System.Text;
using System.Text.RegularExpressions;


using System.Data.Objects;

using Kaizos.Entities.Business;
using KaizosEntities;
using Kaizos.Components.GlobalLibrary;


namespace Kaizos.Components.Carriers
{

    public class GLS
    {
        public bool ok;
        public string input, trackingResult, service, isoccode;
        public string strResponse, strOutput, nc, pc;
        public static string s1;
        public string[] str;

        public BCarriercredentials Getcarrierdetails()
        {
            BCarriercredentials CD = new BCarriercredentials();

            #region Reteriving Carrier Crdentials from Database

            string sCarrierCode = "GLS";

            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

            var result = context.uSP_GET_CARRIER_PARAMETERS(sCarrierCode).ToList();
            foreach (var rec in result)
            {

                if (rec.KEY_CODE.Equals("NContactID"))
                {
                    CD.NContactID = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("EContactID"))
                {
                    CD.EContactID = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("CustomerID"))
                {
                    CD.CustomerID = rec.KEY_VALUE;
                }

                if (rec.KEY_CODE.Equals("T852"))
                {
                    CD.T852 = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("GLSoutboundDepot"))
                {
                    CD.GLSoutboundDepot = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("ConstantZero"))
                {
                    CD.ConstantZero = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("Service"))
                {
                    CD.Service = rec.KEY_VALUE;
                }

            }

            #endregion


            return CD;
        }

        public BShipmentOrder GetFeasability(BShipmentOrder order)
        {
            BShipmentResult bshipresult = new BShipmentResult();
            bshipresult.FeasibilityError = "";
            bshipresult.isFeasibility = BEnumFlag.No;
            bshipresult.isLabelGenerated = BEnumFlag.No;
            bshipresult.isManifestGenerated = BEnumFlag.No;
            bshipresult.isOther = BEnumFlag.No;
            bshipresult.LabelError = "";
            bshipresult.ManifestError = "";
            bshipresult.OtherError = "";
            
            if (order.UODCount >= 9)
            {
                ok = false;

                bshipresult.isFeasibility = BEnumFlag.No;
                bshipresult.FeasibilityError = "Given Number Of UODs Could not Ship ";
                order.ShipmentResult = bshipresult;
            }

            else
            {
                foreach (BShipmentDetails bo in order.ShipDetail)
                {
                    if (bo.Weight <= 30 && bo.Length <= 200 && bo.Height <= 80 && bo.Width <= 60)
                    {

                        ok = true;

                    }

                    else
                    {
                        ok = false;
                        break;
                    }


                }

                if (ok == true)
                {
                    bshipresult.isFeasibility = BEnumFlag.Yes;
                    // order.ShipmentResult = bshipresult;

                }
                else
                {
                    bshipresult.isFeasibility = BEnumFlag.No;
                    bshipresult.FeasibilityError = "Shipment Failed";

                }
                order.ShipmentResult = bshipresult;
            }

            return order;
        }

        public BShipmentOrder GetLabel(BShipmentOrder bs, out BCarrierProcessResult bCarrierProcessResult)
        {
            Library lb = new Library();
            BCarriercredentials CD = Getcarrierdetails();
            BShipmentResult bshipresult = new BShipmentResult();
            bshipresult.FeasibilityError = "";
            bshipresult.isFeasibility = BEnumFlag.No;
            bshipresult.isLabelGenerated = BEnumFlag.No;
            bshipresult.isManifestGenerated = BEnumFlag.No;
            bshipresult.isOther = BEnumFlag.No;
            bshipresult.LabelError = "";
            bshipresult.ManifestError = "";
            bshipresult.OtherError = "";
            
            int limit = bs.UODCount;
            if (limit > 10)
            {
                ok = false;
            }
            else
            {
                KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
                string servicecode = context.uSP_CARRIER_SERVICE_CODE(bs.CarrierService).DefaultIfEmpty().First();
                if (!(servicecode == null))
                {
                    service = servicecode;//.CARRIER_SERVICE_CODE;
                    string[] Sp = service.Split(new Char[] { '!' });
                    for (int h = 0; h <= Sp.Length - 1; h++)
                    {
                        if (h == 0)
                        {
                            nc = Sp[h];
                        }
                        else if (h == 1)
                        {
                            pc = Sp[h];
                        }
                    }

                }

                string ccode = context.uSP_GET_ISOCOUNTRYCODE(bs.RecipientCountry).DefaultIfEmpty().First();
                if (!(ccode == null))
                {
                    isoccode = ccode;//.THREE_DIGIT_CODE.ToString();
                }

                int i = 1;
                string cid = "";
                foreach (BShipmentDetails bo in bs.ShipDetail)
                {
                    string scity = lb.FrtoEn(bs.SenderCity);
                    string rcity = lb.FrtoEn(bs.RecipientCity);
                    string lc;

                    if (bs.SenderCountry == "FR")
                    {
                        lc = "UNIQUENO";

                    }
                    else
                    {
                        lc = " ";

                    }

                    if (bs.RecipientCountry == "FR")
                    {
                        cid = CD.NContactID;
                    }
                    else
                    {
                        cid = CD.EContactID;
                    }

                    string input = @"\\\\\GLS\\\\\T082:" + lc;
                    input = input + "|T090:NOSAVE";//Constant
                    input = input + "|T100:" + bs.RecipientCountry;
                    input = input + "|T330:" + bs.RecipientZipCode;
                    input = input + "|T530:" + bo.Weight;
                    //input = input + "|T540:201009019090";
                    string year = bs.ShipDateTime.Year.ToString();
                    string month = (bs.ShipDateTime.Month < 10) ? "0" + bs.ShipDateTime.Month.ToString() : bs.ShipDateTime.Month.ToString();
                    string day = (bs.ShipDateTime.Day < 10) ? "0" + bs.ShipDateTime.Day.ToString() : bs.ShipDateTime.Day.ToString();
                    input = input + "|T540:" + year + month + day;
                    string sref = bs.ShipReference;
                    //bs.ShipDateTime;//bs.ShipDateTime="201009019090"
                    input = input + "|T810:" + bs.SenderName;
                    input = input + "|T821:" + bs.SenderCountry;
                    input = input + "|T822:" + bs.SenderZipCode;
                    input = input + "|T823:" + scity;
                    input = input + "|T852:" + CD.T852;
                    input = input + "|T860:" + bs.RecipientName;
                    input = input + "|T861:" + bs.RecipientAddress2;
                    input = input + "|T862:" + bs.RecipientAddress3;
                    input = input + "|T863:" + bs.RecipientAddress1;
                    input = input + "|T864:" + rcity;
                    input = input + "|T8906:" + bs.SenderComments;
                    input = input + "|T8700:"+CD.GLSoutboundDepot; //     FR0031";//Gls Outbound Depot
                    input = input + "|T8914:" + cid;//2503883901";//ContactID 
                    input = input + "|T8915:" + CD.CustomerID; ;//Customer ID unique for each customer - >wheteher it is a carrier sub account
                    input = input + "|T8702:00" + i.ToString(); //current Uod count
                    input = input + "|T8973:00" + bs.UODCount.ToString(); //Total Uod 
                    string pno = sref + i.ToString() + bs.UODCount.ToString() + "0000";
                    if (pno.Length > 10)
                    {
                        pno = pno.Substring(0, 10);
                    }
                    input = input + "|T8975:" + nc.ToString() + pno + CD.ConstantZero.ToString() + bs.RecipientCountry; //   0200009993080000FR"; //ProductCode+parcelnumber+constant=0000+Receiver country code
                    input = input + "|T871:" + bs.RecipientPhone; //Receiver phone number
                    input = input + "/////GLS/////";
                    string url = "http://www.gls-france.com/cgi-bin/glsboxGITest.cgi";
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                    request.Method = "Post";
                    byte[] byteArray = Encoding.UTF8.GetBytes(input);
                    request.ContentType = "text/html";
                    request.ContentLength = byteArray.Length;
                    Stream wri = request.GetRequestStream();
                    wri.Write(byteArray, 0, byteArray.Length);
                    StreamReader stIn = new StreamReader(request.GetResponse().GetResponseStream());
                    strResponse = stIn.ReadToEnd();
                    if (strResponse.Contains("RESULT:E002:T330"))
                    {
                        ok = false;
                        bshipresult.isLabelGenerated = BEnumFlag.No;
                        bshipresult.LabelError = "ERROR IN ZIP CODE";
                        bs.ShipmentResult = bshipresult;
                        break;
                    }

                    else if (strResponse.Contains("RESULT:E002:T100"))
                    {
                        ok = false;
                        bshipresult.isLabelGenerated = BEnumFlag.No;
                        bshipresult.LabelError = "ERROR IN COUNTRY CODE ";
                        bs.ShipmentResult = bshipresult;
                        break;

                    }
                    else if (strResponse.Contains("RESULT:E000"))
                    {
                        ok = true;
                        str = new string[limit];
                        strOutput = strOutput + strResponse + "|len:" + bo.Length + "|wid:" + bo.Width + "|hgt:" + bo.Height + "|senadd1:" + bs.SenderAddress1.ToString() + "|senadd2:" + bs.SenderAddress2.ToString() + "|sencom:" + bs.SenderCompany.ToString() + "|orderno:" + bo.ShippingReference.ToString() + "|desc:" + bo.ContentType.ToString() + "|curr:" + bs.CurrencyType.ToString() + "|insur:" + bs.Insured.ToString() + "|reccom:" + bs.RecipientCompany.ToString() + "|totwgt:" + bs.TotalWeight.ToString() + "|" + ";";
                        s1 = strOutput;
                        string[] Split = strResponse.Split(new Char[] { '|' });
                        for (int le = 1; le <= Split.Length - 1; le++)
                        {
                            if (Split[le].Contains("T8913:"))
                            {
                                string t8913 = Split[le].Replace("T8913:", "");
                                bo.TrackingNo = t8913;
                            }
                        }

                    }

                    else
                    {
                        ok = true;
                        str = new string[limit];
                        strOutput = strOutput + strResponse + "|len:" + bo.Length + "|wid:" + bo.Width + "|hgt:" + bo.Height + "|senadd1:" + bs.SenderAddress1.ToString() + "|senadd2:" + bs.SenderAddress2.ToString() + "|sencom:" + bs.SenderCompany.ToString() + "|orderno:" + bo.ShippingReference.ToString() + "|desc:" + bo.ContentType.ToString() + "|curr:" + bs.CurrencyType.ToString() + "|insur:" + bs.Insured.ToString() + "|reccom:" + bs.RecipientCompany.ToString() + "|totwgt:" + bs.TotalWeight.ToString() + "|nc:" + nc.ToString() + "|pc:" + pc.ToString() + "|isocode:" + isoccode.ToString() + "|custref:" + pno.ToString() + "|conref:" + sref.ToString() + "|serv:" + CD.Service.ToString() + "|" + ";";
                        s1 = strOutput;
                        bo.TrackingNo = sref;

                    }

                    i = i + 1;
                }


            }

            bCarrierProcessResult = new BCarrierProcessResult();

            if (ok == true)
            {
                bCarrierProcessResult.Carrier = "GLS";
                bCarrierProcessResult.Output = strOutput;
                bCarrierProcessResult.Result = new byte[] { 1, 2, 3 };
                //label should be stored in the database
                //manifest should be stored in the database
                KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
                System.Nullable<int> result = context.uSP_CARRIER_DOCUMENTS_STORE(bs.ShipReference, "GLS", null, null, strOutput, null, null, DateTime.Today, null, null).First();
                result = context.uSP_CARRIER_DOCUMENTS_STORE(bs.ShipReference, "GLS", null, null, null, strOutput, null, null, DateTime.Today, null).First();
                //ShowLabel(s);


                bshipresult.isLabelGenerated = BEnumFlag.Yes;
                bs.ShipmentResult = bshipresult;



            }

            return bs;


        }

        public void ShowLabel(string label)
        {
            System.Web.HttpContext.Current.Session["one"] = label;
            Page page = (Page)HttpContext.Current.Handler;

            string url = page.ResolveUrl(@"~\Carrier\GLSCarrier.aspx");
            string url1 = page.Request.Url.AbsolutePath.ToString();
            string url2 = page.Request.Url.AbsoluteUri.Substring(0, page.Request.Url.AbsoluteUri.IndexOf(page.Request.Url.LocalPath));
            //System.Web.HttpContext.Current.Response.Redirect(@"Carr\About.aspx");
            //Page page = (Page)HttpContext.Current.Handler;
            //string url = page.ResolveClientUrl(@"Carr/About.aspx");
            System.Web.HttpContext.Current.Response.Write("<script>");
            System.Web.HttpContext.Current.Response.Write(@"window.open('" + url2 + url + "','_blank')");
            System.Web.HttpContext.Current.Response.Write("</script>");
        }

        public BShipmentOrder ShowLabel(BShipmentOrder order, out BCarrierProcessResult bCarrierProcessResult)
        {
            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            var result = context.uSP_GET_CARRIER_LABEL(order.ShipReference).First();
            bCarrierProcessResult = new BCarrierProcessResult();
            bCarrierProcessResult.Carrier = "GLS";
            bCarrierProcessResult.Output = result.LABEL;
            bCarrierProcessResult.Result = new byte[] { 1, 2, 3 };
            // ShowLabel(result.LABEL);            
            return order;
        }

        public void GetManifest()
        {
            string mani = s1;
            System.Web.HttpContext.Current.Session["one"] = mani;
            Page page = (Page)HttpContext.Current.Handler;

            string url = page.ResolveUrl(@"~\Carrier\frmManifest.aspx");
            string url1 = page.Request.Url.AbsolutePath.ToString();
            string url2 = page.Request.Url.AbsoluteUri.Substring(0, page.Request.Url.AbsoluteUri.IndexOf(page.Request.Url.LocalPath));
            //System.Web.HttpContext.Current.Response.Redirect(@"Carr\About.aspx");
            //Page page = (Page)HttpContext.Current.Handler;
            //string url = page.ResolveClientUrl(@"Carr/About.aspx");
            System.Web.HttpContext.Current.Response.Write("<script>");
            System.Web.HttpContext.Current.Response.Write(@"window.open('" + url2 + url + "','_blank')");
            System.Web.HttpContext.Current.Response.Write("</script>");

        }

        public BShipmentOrder ReteriveLabel(BShipmentOrder order, out BCarrierProcessResult bCarrierProcessResult)
        {
            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            var result = context.uSP_GET_CARRIER_LABEL(order.ShipReference).First();
            string LabelXML;
            bCarrierProcessResult = new BCarrierProcessResult();
            bCarrierProcessResult.Carrier = "GLS";
            bCarrierProcessResult.Result = new byte[] { 1, 2, 3 };
            bCarrierProcessResult.Output = result.LABEL;
            return order;
        }

        public BShipmentOrder ReterievbeManifest(BShipmentOrder order, out BCarrierProcessResult bCarrierProcessResult)
        {
            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            var result = context.uSP_GET_CARRIER_MANIFEST(order.ShipReference).First();
            string ManifestXML;
            bCarrierProcessResult = new BCarrierProcessResult();

            bCarrierProcessResult.Carrier = "TNTINTERNATIONAL";
            bCarrierProcessResult.Output = result.MANIFEST;




            return order;
        }

        public string GetTracking(string trackNo)
        {
            string[] track = trackNo.Split(new Char[] { ',' });
            string language = "EN";
            string refNumber;
            for (int ine = 0; ine <= track.Count() - 1; ine++)
            {
                refNumber = track[ine];
                string url = @"http://www.gls-group.eu/276-I-PORTAL-WEB/content/GLS/FR01/" + language + "/5004.htm?txtRefNo=" + refNumber + "&txtAction=71000";
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "Post";
                StreamReader stIn = new StreamReader(request.GetResponse().GetResponseStream());

                string splitby = "</table>";
                int count = 0;

                string strResponse = stIn.ReadToEnd();

                string[] lines = Regex.Split(strResponse, splitby);
                for (int i = 0; i < lines.Count(); i++)
                {
                    if (lines[i].Contains("resultlist"))
                    {
                        count = i;
                        break;
                    }

                }

                string newtable = "<html><body>" + lines[count] + "</table></body></html>";

                trackingResult += "<html><head></head><body><br>" + newtable + "</br></body></html>";
            }

            return trackingResult;

        }
    }

}
