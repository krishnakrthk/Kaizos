using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Net;

using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

using System.Data.Entity;
using System.Data.Objects;
using System.Data;

using Kaizos.Entities.Business;
using Kaizos.Components.GlobalLibrary;


namespace Kaizos.Components.Carriers
{

    public class TNTINTERNATIONAL
    {
        public  int te1 = 0;
        public string ManifestXML;
        public string InvoiceXML;
        public string LabelXML;
        public string ConNoteXML;
        public string Number;
        public string ResultXML;
        public static string[] ka;
        public static int i = 0;
        public string groupcode;

        public BCarriercredentials Getcarrierdetails()
        {
            BCarriercredentials CD = new BCarriercredentials();

            #region Reteriving Carrier Crdentials from Database

            string sCarrierCode = "TNTINTERNATIONAL";

            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

            var result = context.uSP_GET_CARRIER_PARAMETERS(sCarrierCode).ToList();
            foreach (var rec in result)
            {
                if (rec.KEY_CODE.Equals("UserName"))
                {
                    CD.UserName = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("PassWord"))
                {
                    CD.PassWord = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("AppIdFeasability"))
                {
                    CD.AppIdFeasability = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("CountryVersion"))
                {
                    CD.CountryVersion = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("CurrencyVersion"))
                {
                    CD.CurrencyVersion = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("PostcodeMaskVersion"))
                {
                    CD.PostcodeMaskVersion = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("TownGroupVersion"))
                {
                    CD.TownGroupVersion = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("ServiceVersion"))
                {
                    CD.ServiceVersion = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("OptionVersion"))
                {
                    CD.OptionVersion = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("RateID"))
                {
                    CD.RateID = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("OriginalTownGroup"))
                {
                    CD.OriginalTownGroup = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("DestTownGroup"))
                {
                    CD.DestTownGroup = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("AccountNO"))
                {
                    CD.AccountNO = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("ServiceType"))
                {
                    CD.ServiceType = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("AppIdLabel"))
                {
                    CD.AppIdLabel = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("Appverersion"))
                {
                    CD.Appverersion = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("GroupCode"))
                {
                    CD.GroupCode = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("Providance"))
                {
                    CD.Providance = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("VATSender"))
                {
                    CD.VATSender = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("VATReciver"))
                {
                    CD.VATReciver = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("PaymentId"))
                {
                    CD.PaymentId = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("Deliveryinstruction"))
                {
                    CD.Deliveryinstruction = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("Description"))
                {
                    CD.Description = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("TrackingType"))
                {
                    CD.TrackingType = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("ExtratrackingDetail1"))
                {
                    CD.ExtratrackingDetail1 = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("ExtratrackingDetail2"))
                {
                    CD.ExtratrackingDetail2 = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("ExtratrackingDetail3"))
                {
                    CD.ExtratrackingDetail3 = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("ExtratrackingDetail4"))
                {
                    CD.ExtratrackingDetail4 = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("ExtratrackingDetail5"))
                {
                    CD.ExtratrackingDetail5 = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("ExtratrackingDetail6"))
                {
                    CD.ExtratrackingDetail6 = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("NContactID"))
                {
                    CD.ContactID = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("EContactID"))
                {
                    CD.ContactID = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("CustomerID"))
                {
                    CD.CustomerID = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("CountryISOCode"))
                {
                    CD.CountryISOCode = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("ProductCode"))
                {
                    CD.ProductCode = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("ConsigneeRef"))
                {
                    CD.ConsigneeRef = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("CustomerRefNumber"))
                {
                    CD.CustomerRefNumber = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("ServiceandAdditional"))
                {
                    CD.ServiceandAdditional = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("ServiceInformation"))
                {
                    CD.ServiceInformation = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("T852"))
                {
                    CD.T852 = rec.KEY_VALUE;
                }
                if (rec.KEY_CODE.Equals("GLSoutboundDepot"))
                {
                    CD.GLSoutboundDepot = rec.KEY_VALUE;
                }



            }
            #endregion
            #region cd
            //    CD = new BCarriercredentials();
            ///* TNT International Feasibility ExtraDetails  */
            //CD.AccountNO = "001000072";
            //CD.UserName = "navilib";
            //CD.PassWord = "navilib123";
            //CD.AppIdFeasability = "PC";
            //CD.CountryVersion = "1.0";
            //CD.CurrencyVersion = "1.0";
            //CD.PostcodeMaskVersion = "1.0";
            //CD.ServiceVersion = "1.0";
            //CD.TownGroupVersion = "1.0";
            //CD.OptionVersion = "1.0";
            //CD.RateID = "21";
            //CD.OriginalTownGroup = "";
            //CD.DestTownGroup = "";

            ///*TNT International Label ExtraDetails   */
            //CD.AppIdLabel = "IN";
            //CD.Appverersion = "2.1";
            //CD.VATSender = "";
            //CD.VATReciver = "";
            #endregion
            return CD;
        }

        public DataTable gettable(BShipmentOrder   bsorder)
        {
            DataTable db = new DataTable();
            db.Columns.Add("Weight", typeof(string));
            db.Columns.Add("Height", typeof(string));
            db.Columns.Add("Lenght", typeof(string));
            db.Columns.Add("width", typeof(string));
            

            foreach (BShipmentDetails bo in bsorder.ShipDetail)
            {
                db.Rows.Add(bo.Weight.ToString(),bo.Height.ToString(), bo.Length.ToString(),bo.Width.ToString()  );
            }
            return db;

        }

        public List<temp> getlist(BShipmentOrder bshhdgs)
        {
            List<temp> collect = new List<temp>();
            
            foreach (BShipmentDetails bo in bshhdgs.ShipDetail)
            {
                temp tt = new temp();
                tt.Weight = bo.Weight;
                tt.Height = bo.Height;
                tt.Width = bo.Width;
                tt.Length = bo.Length;
                tt.container = bo.ContentType;
                collect.Add(tt);

            }


            IEnumerable<temp> noduplicates =collect.Distinct(new ProductComparer());
            List<temp> try1 = new List<temp>();
            foreach (var product in noduplicates)
            {
                temp tt1 = new temp();
                tt1.Weight = product.Weight;
                tt1.Height = product.Height;
                tt1.Width = product.Width;
                tt1.Length = product.Length;
                try1.Add(tt1);
            }
                


           
            return try1;
        }

        public BShipmentOrder GetFeasibility(BShipmentOrder bShipmentOrder)
        {
            Library lb = new Library();
            BShipmentResult bShipmentResult = new BShipmentResult();
            bShipmentResult.FeasibilityError = "";
            bShipmentResult.isFeasibility = BEnumFlag.No;
            bShipmentResult.isLabelGenerated = BEnumFlag.No;
            bShipmentResult.isManifestGenerated = BEnumFlag.No;
            bShipmentResult.isOther = BEnumFlag.No;
            bShipmentResult.LabelError = "";
            bShipmentResult.ManifestError = "";
            bShipmentResult.OtherError = "";
           
           
            List<temp> te = new List<temp>();
                       te = getlist(bShipmentOrder);
                      
                      
                           for (int k = 0; k <= te.Count - 1; k++)
                           {
                               te1 = 0;
                             
                               for (int i = 0; i <= bShipmentOrder.ShipDetail.Count - 1; i++)
                               {
                                  
                                   if ((te[k].Weight == bShipmentOrder.ShipDetail[i].Weight) && (te[k].Height == bShipmentOrder.ShipDetail[i].Height) && (te[k].Width == bShipmentOrder.ShipDetail[i].Width) && (te[k].Length == bShipmentOrder.ShipDetail[i].Length))
                                       {
                                           te1++;
                                           te[k].container = bShipmentOrder.ShipDetail[i].ContentType;
                                          te[k].count=te1;
                                       }
                               }
                           }
                           if (te.Count <= 3)
                           {
                               #region KB
                               //}
                               //foreach (BShipmentDetails bsh in bShipmentOrder.ShipDetail)
                               //{
                               //    if ((bsh.Weight == te[te1].Weight) && (bsh.Width == te[te1].Width))
                               //    {
                               //        te[te1].count = 1;

                               //        te1++;
                               //    }
                               //    else
                               //    {
                               //        te[te1].count++;
                               //    }
                               //}
                        
                               ////            //DataTable db = gettable(bShipmentOrder);
                               ////            int newuod = bShipmentOrder.UODCount;
                               ////            ////for (int bship = 0; bship <= bShipmentOrder.ShipDetail.Count-1; bship++)
                               ////            ////{
                               ////            ////    int tt=bship+1;
                               ////            ////    for(int bshipnew=tt;bshipnew<=bShipmentOrder.ShipDetail.Count-1;bshipnew++)
                               ////            ////    {
                               ////            ////        if (bship != bshipnew)
                               ////            ////        {
                               ////            ////            if ((bShipmentOrder.ShipDetail[bship].Weight == bShipmentOrder.ShipDetail[bshipnew].Weight))
                               ////            ////            {
                               ////            ////                newuod = newuod - 1;
                               ////            ////            }
                               ////            ////        }
                               ////            ////    }
                               ////            ////}
                               ////            int totl = bShipmentOrder.ShipDetail.Count;
                               ////            totl = totl - 1;
                               ////            for (int bship = 0; bship <= totl; bship++)
                               ////            {
                               ////                int tt = bship + 1;
                               ////                int hhhh = 0;
                               ////                for (int bshipnew = tt; bshipnew <= totl; bshipnew++)
                               ////                {
                               ////                    if (bship != bshipnew)
                               ////                    {
                               ////                        if ((bShipmentOrder.ShipDetail[bship].Weight == bShipmentOrder.ShipDetail[bshipnew].Weight))
                               ////                        {
                               ////                            if (te.Count == bShipmentOrder.ShipDetail.Count)
                               ////                            {
                               ////                                if (te[bshipnew].Weight == bShipmentOrder.ShipDetail[bshipnew].Weight)
                               ////                                    te.RemoveAt(bshipnew);
                               ////                            }
                               ////                            else
                               ////                            {
                               ////                                hhhh = bShipmentOrder.ShipDetail.Count - te.Count;
                               ////                                if (bshipnew - hhhh >= 0)
                               ////                                {
                               ////                                    if (te[bshipnew - hhhh].Weight == bShipmentOrder.ShipDetail[bshipnew].Weight)
                               ////                                        te.RemoveAt(bshipnew - hhhh);
                               ////                                }
                               ////                            }
                               ////                        }
                               ////                    }
                               ////                }
                               ////            }
                               ////            int kkkk = te.Count
                               #endregion

                               string nc = "15N";
                               string pc = "";
                               string service = "";
                               KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
                               string servicecode = context.uSP_CARRIER_SERVICE_CODE(bShipmentOrder.Carrier).DefaultIfEmpty().First();
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


                               BCarriercredentials cd;
                               cd = Getcarrierdetails();
                               string scity = lb.FrtoEn(bShipmentOrder.SenderCity);
                               string rcity = lb.FrtoEn(bShipmentOrder.RecipientCity);

                               Uri targetUri = new Uri("http://iconnection.tnt.com:81/priceGate.asp");
                               HttpWebRequest request = (HttpWebRequest)WebRequest.Create(targetUri);
                               string formParameterName = "xml_in";
                               string xmlData = @"<?xml version=""1.0"" standalone=""no""?>";
                               xmlData = xmlData + "<!DOCTYPE PRICEREQUEST SYSTEM 'http://164.39.41.88:81/PriceCheckerDTD1.0/PriceRequestIN.dtd'><PRICEREQUEST>";
                               xmlData = xmlData + "<LOGIN><COMPANY>" + cd.UserName + "</COMPANY>";
                               xmlData = xmlData + "<PASSWORD>" + cd.PassWord + "</PASSWORD>";
                               xmlData = xmlData + "<APPID>" + cd.AppIdFeasability + "</APPID></LOGIN>";
                               xmlData = xmlData + "<DATASETS><COUNTRY>" + cd.CountryVersion + "</COUNTRY>";
                               xmlData = xmlData + "<CURRENCY>" + cd.CurrencyVersion + "</CURRENCY><POSTCODEMASK>" + cd.PostcodeMaskVersion + "</POSTCODEMASK>";
                               xmlData = xmlData + "<TOWNGROUP>" + cd.TownGroupVersion + "</TOWNGROUP><SERVICE>" + cd.ServiceVersion + "</SERVICE>";
                               xmlData = xmlData + "<OPTION>" + cd.OptionVersion + "</OPTION></DATASETS>";
                               xmlData = xmlData + "<PRICECHECK><RATEID>" + cd.RateID + "</RATEID>";
                               xmlData = xmlData + "<ORIGINCOUNTRY>" + bShipmentOrder.SenderCountry + "</ORIGINCOUNTRY>";
                               xmlData = xmlData + "<ORIGINTOWNNAME>" + scity + "</ORIGINTOWNNAME>";
                               xmlData = xmlData + "<ORIGINPOSTCODE>" + bShipmentOrder.SenderZipCode + "</ORIGINPOSTCODE>";
                               xmlData = xmlData + "<ORIGINTOWNGROUP></ORIGINTOWNGROUP>";
                               xmlData = xmlData + "<DESTCOUNTRY>" + bShipmentOrder.RecipientCountry + "</DESTCOUNTRY>";
                               xmlData = xmlData + "<DESTTOWNNAME>" + rcity + "</DESTTOWNNAME>";
                               xmlData = xmlData + "<DESTPOSTCODE>" + bShipmentOrder.RecipientZipCode + "</DESTPOSTCODE>";
                               xmlData = xmlData + "<DESTTOWNGROUP></DESTTOWNGROUP>";
                                   string cc;
                                   if (bShipmentOrder.ContainerType == "Letter")
                                       cc = "D";
                                   else
                                       cc = "N";
                               xmlData = xmlData + "<CONTYPE>" + cc + "</CONTYPE>";
                               xmlData = xmlData + "<CURRENCY>GBP</CURRENCY>";
                               xmlData = xmlData + "<WEIGHT>" + bShipmentOrder.TotalWeight+ "</WEIGHT>";
                               xmlData = xmlData + "<VOLUME>0</VOLUME>";
                               xmlData = xmlData + "<ACCOUNT>" + cd.AccountNO + "</ACCOUNT>";/*+cd.AccountNO+*/
                               xmlData = xmlData + "<ITEMS>" + bShipmentOrder.UODCount + "</ITEMS>";
                               xmlData = xmlData + "<SERVICE>" + nc + "</SERVICE></PRICECHECK>";
                               xmlData = xmlData + "</PRICEREQUEST>";
                              
                               string sendString = formParameterName + "=" + HttpUtility.UrlEncode(xmlData);
                               byte[] byteStream;
                               byteStream = System.Text.Encoding.UTF8.GetBytes(sendString);
                               request.Method = "POST";
                               request.ContentType = "application/x-www-form-urlencoded";
                               request.ContentLength = byteStream.LongLength;

                               using (Stream writer = request.GetRequestStream())
                               {
                                   writer.Write(byteStream, 0, (int)request.ContentLength);
                                   writer.Flush();
                               }

                               HttpWebResponse resp = (HttpWebResponse)request.GetResponse();

                               StreamReader sr = new StreamReader(resp.GetResponseStream());
                               string hh = sr.ReadToEnd();
                             
                               bool Result;
                               
                               if (hh.Contains("<RESULT>Y</RESULT>")) //&& (i == bShipmentOrder.UODCount))
                               {
                                   Result = true;
                                   bShipmentResult.isFeasibility = BEnumFlag.Yes;
                               }
                               else
                               {
                                   Result = false;
                                   bShipmentResult.isFeasibility = BEnumFlag.No;
                                   bShipmentResult.FeasibilityError = "check";
                               }
                               bShipmentOrder.ShipmentResult = bShipmentResult;
                              
                           }
                           else
                           {
                               bShipmentResult.isFeasibility = BEnumFlag.No;
                               bShipmentResult.FeasibilityError = "More Then Three Different Package won't be taken";
                           }
            return bShipmentOrder;
        }

        public string Generation(string xml_in)
        {
            Uri targetUri = new Uri("http://iconnection.tnt.com:81/ShipperGate2.asp");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(targetUri);

            string formParameterName = "xml_in";

            //string xmlData = @"?xml version=""1.0"" standalone=""no""?><!DOCTYPE ESHIPPER SYSTEM 'http://164.39.41.88:81/ShipperDTD2.0/EShipperIN2.dtd'><ESHIPPER><LOGIN><COMPANY>navilib</COMPANY><PASSWORD>navilib123</PASSWORD><APPID>IN</APPID><APPVERSION>2.1</APPVERSION></LOGIN><CONSIGNMENTBATCH><SENDER><COMPANYNAME>TNT</COMPANYNAME><STREETADDRESS1>43, avenue des droits de l'Homme</STREETADDRESS1><STREETADDRESS2>Holly Lane</STREETADDRESS2><STREETADDRESS3></STREETADDRESS3><CITY>Orléans</CITY><PROVINCE></PROVINCE><POSTCODE>45000</POSTCODE><COUNTRY>FR</COUNTRY><ACCOUNT>001000072</ACCOUNT><VAT>GB78687</VAT><CONTACTNAME>Mr Smith</CONTACTNAME><CONTACTDIALCODE>44 121</CONTACTDIALCODE><CONTACTTELEPHONE>717733</CONTACTTELEPHONE><CONTACTEMAIL>MrSmith@mail.CON</CONTACTEMAIL><COLLECTION><COLLECTIONADDRESS><COMPANYNAME>TNT Express</COMPANYNAME><STREETADDRESS1>TNT Lloyd House</STREETADDRESS1><STREETADDRESS2>Manor Road</STREETADDRESS2><STREETADDRESS3></STREETADDRESS3><CITY>Mancetter</CITY><PROVINCE>Warwickshire</PROVINCE><POSTCODE>CV9 1TT</POSTCODE><COUNTRY>GB</COUNTRY><VAT>65354</VAT><CONTACTNAME>Micheal Wood</CONTACTNAME><CONTACTDIALCODE>01827</CONTACTDIALCODE><CONTACTTELEPHONE>712345</CONTACTTELEPHONE><CONTACTEMAIL>contact@TNT.CON</CONTACTEMAIL></COLLECTIONADDRESS><SHIPDATE>10/01/2012</SHIPDATE><PREFCOLLECTTIME><FROM>09:00</FROM><TO>10:00</TO></PREFCOLLECTTIME><ALTCOLLECTTIME><FROM>11:00</FROM><TO>12:00</TO></ALTCOLLECTTIME><COLLINSTRUCTIONS>use rear gate </COLLINSTRUCTIONS></COLLECTION></SENDER><CONSIGNMENT><CONREF>ref1</CONREF><DETAILS><RECEIVER><COMPANYNAME>Receiver Co.Ltd</COMPANYNAME><STREETADDRESS1>Head Office</STREETADDRESS1><STREETADDRESS2>Hoo fddorp</STREETADDRESS2><STREETADDRESS3></STREETADDRESS3><CITY>Amsterdam</CITY><PROVINCE></PROVINCE><POSTCODE>1100 kg</POSTCODE><COUNTRY>NL</COUNTRY><VAT>7668880</VAT><CONTACTNAME>Mr Frank</CONTACTNAME><CONTACTDIALCODE>1672</CONTACTDIALCODE><CONTACTTELEPHONE>987432</CONTACTTELEPHONE><CONTACTEMAIL>CITTEST@TEST.CON</CONTACTEMAIL></RECEIVER><DELIVERY><COMPANYNAME>TNT International</COMPANYNAME><STREETADDRESS1>Grovester 19</STREETADDRESS1><STREETADDRESS2></STREETADDRESS2><STREETADDRESS3></STREETADDRESS3><CITY>Hannover</CITY><PROVINCE></PROVINCE><POSTCODE>30853</POSTCODE><COUNTRY>DE</COUNTRY><VAT>7668880</VAT><CONTACTNAME>Mr Jones</CONTACTNAME><CONTACTDIALCODE>1672</CONTACTDIALCODE><CONTACTTELEPHONE>987432</CONTACTTELEPHONE><CONTACTEMAIL>CITTEST@TEST.CON</CONTACTEMAIL></DELIVERY><CUSTOMERREF>Reciver09</CUSTOMERREF><CONTYPE>N</CONTYPE><PAYMENTIND>S</PAYMENTIND><ITEMS>2</ITEMS><TOTALWEIGHT>1.4</TOTALWEIGHT><TOTALVOLUME>1.0</TOTALVOLUME><CURRENCY>GBP</CURRENCY><GOODSVALUE>180.00</GOODSVALUE><INSURANCEVALUE>150.00</INSURANCEVALUE><INSURANCECURRENCY>GBP</INSURANCECURRENCY><SERVICE>15N</SERVICE><OPTION>PR</OPTION><DESCRIPTION>assorted office accessories</DESCRIPTION><DELIVERYINST>ggg</DELIVERYINST><PACKAGE><ITEMS>2</ITEMS><DESCRIPTION>box</DESCRIPTION><LENGTH>0.5</LENGTH><HEIGHT>0.9</HEIGHT><WIDTH>0.6</WIDTH><WEIGHT>0.8</WEIGHT><ARTICLE><ITEMS>1</ITEMS><DESCRIPTION>paperclips</DESCRIPTION><WEIGHT>.03</WEIGHT><INVOICEVALUE>2.30</INVOICEVALUE><INVOICEDESC>metal paperclips</INVOICEDESC><HTS>6345</HTS><COUNTRY>GB</COUNTRY></ARTICLE></PACKAGE></DETAILS></CONSIGNMENT></CONSIGNMENTBATCH><ACTIVITY><CREATE><CONREF>ref1</CONREF></CREATE><RATE><CONREF>ref1</CONREF></RATE><BOOK><CONREF>ref1</CONREF></BOOK><PRINT><CONNOTE><CONREF>ref2</CONREF><CONNUMBER>GE012062707</CONNUMBER></CONNOTE><LABEL><CONREF>ref1</CONREF></LABEL><MANIFEST><CONREF>ref1</CONREF></MANIFEST><INVOICE><CONREF>ref1</CONREF></INVOICE></PRINT></ACTIVITY></ESHIPPER>";
            string xmlData = xml_in;

            string sendString = formParameterName + "=" + HttpUtility.UrlEncode(xmlData);

            byte[] byteStream;
            byteStream = System.Text.Encoding.UTF8.GetBytes(sendString);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteStream.Length;

            using (Stream writer = request.GetRequestStream())
            {
                writer.Write(byteStream, 0, (int)request.ContentLength);
                writer.Flush();
            }

            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string final = sr.ReadToEnd();

            return final;
        }

        public BShipmentOrder GetLabel(BShipmentOrder bShipmentOrder, out BCarrierProcessResult bCarrierProcessResult)
        {
            Library lb = new Library();
            BCarriercredentials cd = Getcarrierdetails();
            BShipmentResult bShipmentResult = new BShipmentResult();
            bShipmentResult.FeasibilityError = "";
            bShipmentResult.isFeasibility = BEnumFlag.No;
            bShipmentResult.isLabelGenerated = BEnumFlag.No;
            bShipmentResult.isManifestGenerated = BEnumFlag.No;
            bShipmentResult.isOther = BEnumFlag.No;
            bShipmentResult.LabelError = "";
            bShipmentResult.ManifestError = "";
            bShipmentResult.OtherError = "";
            string nc = "";
            string pc = "";
            string service = "";

            #region BUGFIXID=112
            List<temp> te = new List<temp>();
            te = getlist(bShipmentOrder);
            for (int k = 0; k <= te.Count - 1; k++)
            {
                te1 = 0;

                for (int i = 0; i <= bShipmentOrder.ShipDetail.Count - 1; i++)
                {

                    if ((te[k].Weight == bShipmentOrder.ShipDetail[i].Weight) && (te[k].Height == bShipmentOrder.ShipDetail[i].Height) && (te[k].Width == bShipmentOrder.ShipDetail[i].Width) && (te[k].Length == bShipmentOrder.ShipDetail[i].Length))
                    {
                        te1++;
                        te[k].container = bShipmentOrder.ShipDetail[i].ContentType;
                        te[k].count = te1;
                    }
                }
            }
            #endregion

            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            // string servicecode = context.uSP_CARRIER_SERVICE_CODE(bShipmentOrder.Carrier).DefaultIfEmpty().First();
            string servicecode = context.uSP_CARRIER_SERVICE_CODE(bShipmentOrder.CarrierService).DefaultIfEmpty().First();
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
            bool Result;
            #region ss
            //string xmlData = @"<?xml version=""1.0"" standalone=""no""?><!DOCTYPE ESHIPPER SYSTEM 'http://164.39.41.88:81/ShipperDTD2.0/EShipperIN2.dtd'><ESHIPPER>";
            //xmlData = xmlData + "<LOGIN><COMPANY>navilib</COMPANY>";
            //xmlData = xmlData + "<PASSWORD>navilib123</PASSWORD>";
            //xmlData = xmlData + "<APPID>IN</APPID><APPVERSION>2.1</APPVERSION></LOGIN>";
            //xmlData = xmlData + "<CONSIGNMENTBATCH><SENDER><COMPANYNAME>" + bShipmentOrder.SenderCompany + "</COMPANYNAME>";
            //xmlData = xmlData + "<STREETADDRESS1>" + bShipmentOrder.SenderAddress1 + "</STREETADDRESS1>";
            //xmlData = xmlData + "<STREETADDRESS2>" + bShipmentOrder.SenderAddress2 + "</STREETADDRESS2>";
            //xmlData = xmlData + "<STREETADDRESS3>" + bShipmentOrder.SenderAddress3 + "</STREETADDRESS3>";
            //xmlData = xmlData + "<CITY>" + bShipmentOrder.SenderCity + "</CITY>";
            //xmlData = xmlData + "<PROVINCE></PROVINCE>";
            //xmlData = xmlData + "<POSTCODE>" + bShipmentOrder.SenderZipCode + "</POSTCODE>";
            //xmlData = xmlData + "<COUNTRY>" + bShipmentOrder.SenderCountry + "</COUNTRY>";
            //xmlData = xmlData + "<ACCOUNT>" + bShipmentOrder.AccountNo + "</ACCOUNT>";
            //xmlData = xmlData + "<VAT>GB78687</VAT>";
            //xmlData = xmlData + "<CONTACTNAME>" + bShipmentOrder.SenderName + "</CONTACTNAME>";
            //xmlData = xmlData + "<CONTACTDIALCODE>44 121</CONTACTDIALCODE>";
            //xmlData = xmlData + "<CONTACTTELEPHONE>717733</CONTACTTELEPHONE>";
            //xmlData = xmlData + "<CONTACTEMAIL>" + bShipmentOrder.SenderEmail + "</CONTACTEMAIL>";
            //xmlData = xmlData + "<COLLECTION><COLLECTIONADDRESS><COMPANYNAME>" + bShipmentOrder.SenderCompany + "</COMPANYNAME>";
            //xmlData = xmlData + "<STREETADDRESS1>" + bShipmentOrder.SenderAddress1 + "</STREETADDRESS1>";
            //xmlData = xmlData + "<STREETADDRESS2>" + bShipmentOrder.SenderAddress2 + "</STREETADDRESS2>";
            //xmlData = xmlData + "<STREETADDRESS3>" + bShipmentOrder.SenderAddress3 + "</STREETADDRESS3>";
            //xmlData = xmlData + "<CITY>" + bShipmentOrder.SenderCity + "</CITY>";
            //xmlData = xmlData + "<PROVINCE></PROVINCE>";
            //xmlData = xmlData + "<POSTCODE>" + bShipmentOrder.SenderZipCode + "</POSTCODE>";
            //xmlData = xmlData + "<COUNTRY>" + bShipmentOrder.SenderCountry + "</COUNTRY>";
            //xmlData = xmlData + "<ACCOUNT>" + bShipmentOrder.AccountNo + "</ACCOUNT>";
            //xmlData = xmlData + "<VAT>GB78687</VAT>";
            //xmlData = xmlData + "<CONTACTNAME>" + bShipmentOrder.SenderName + "</CONTACTNAME>";
            //xmlData = xmlData + "<CONTACTDIALCODE>44 121</CONTACTDIALCODE>";
            //xmlData = xmlData + "<CONTACTTELEPHONE>717733</CONTACTTELEPHONE>";
            //xmlData = xmlData + "<CONTACTEMAIL>" + bShipmentOrder.SenderEmail + "</CONTACTEMAIL>";
            ////xmlData = xmlData + "<COLLECTION><COLLECTIONADDRESS><COMPANYNAME>IPS EUROPE</COMPANYNAME>";
            ////xmlData = xmlData + "<STREETADDRESS1>43, avenue des droits de l'Homme</STREETADDRESS1>";
            ////xmlData = xmlData + "<STREETADDRESS2>Holly Lane</STREETADDRESS2>";
            ////xmlData = xmlData + "<STREETADDRESS3></STREETADDRESS3>";
            ////xmlData = xmlData + "<CITY>Orléans</CITY>";
            ////xmlData = xmlData + "<PROVINCE>Warwickshire</PROVINCE>";
            ////xmlData = xmlData + "<POSTCODE>45000</POSTCODE>";
            ////xmlData = xmlData + "<COUNTRY>FR</COUNTRY>";
            ////xmlData = xmlData + "<VAT>65354</VAT>";
            ////xmlData = xmlData + "<CONTACTNAME>Yohann</CONTACTNAME>";
            ////xmlData = xmlData + "<CONTACTDIALCODE>01827</CONTACTDIALCODE>";
            ////xmlData = xmlData + "<CONTACTTELEPHONE>712345</CONTACTTELEPHONE>";
            ////xmlData = xmlData + "<CONTACTEMAIL>Yohann@IPS.CON</CONTACTEMAIL>";
            ////string date1 = string.Format("dd/mm/yyyy", bShipmentOrder.WishedShipDate);
            ////string date = bShipmentOrder.WishedShipDate.Day.ToString() + "/" + bShipmentOrder.WishedShipDate.Month.ToString() + "/" + bShipmentOrder.WishedShipDate.Year.ToString();
            //string year  = bShipmentOrder.ShipDateTime.Year.ToString();
            //string month = (bShipmentOrder.ShipDateTime.Month < 10) ? "0" + bShipmentOrder.ShipDateTime.Month.ToString() : bShipmentOrder.ShipDateTime.Month.ToString();
            //string day = (bShipmentOrder.ShipDateTime.Day < 10) ? "0" + bShipmentOrder.ShipDateTime.Day.ToString() : bShipmentOrder.ShipDateTime.Day.ToString();
            //string  date = day + "/" + month + "/" +year;

            //xmlData = xmlData + "</COLLECTIONADDRESS><SHIPDATE>" + date + "</SHIPDATE>";
            //xmlData = xmlData + "<PREFCOLLECTTIME><FROM>09:00</FROM><TO>10:00</TO></PREFCOLLECTTIME>";
            //xmlData = xmlData + "<ALTCOLLECTTIME><FROM>11:00</FROM><TO>12:00</TO></ALTCOLLECTTIME>";
            //xmlData = xmlData + "<COLLINSTRUCTIONS>use rear gate </COLLINSTRUCTIONS></COLLECTION></SENDER>";
            //xmlData = xmlData + "<CONSIGNMENT><CONREF>ref1</CONREF>";
            //xmlData = xmlData + "<DETAILS><RECEIVER><COMPANYNAME>" + bShipmentOrder.RecipientCompany + "</COMPANYNAME>";
            //xmlData = xmlData + "<STREETADDRESS1>" + bShipmentOrder.RecipientAddress1 + "</STREETADDRESS1>";
            //xmlData = xmlData + "<STREETADDRESS2>" + bShipmentOrder.RecipientAddress2 + "</STREETADDRESS2>";
            //xmlData = xmlData + "<STREETADDRESS3>" + bShipmentOrder.RecipientAddress3 + "</STREETADDRESS3>";
            //xmlData = xmlData + "<CITY>" + bShipmentOrder.RecipientCity + "</CITY>";
            //xmlData = xmlData + "<PROVINCE></PROVINCE>";
            //xmlData = xmlData + "<POSTCODE>" + bShipmentOrder.RecipientZipCode + "</POSTCODE>";
            //xmlData = xmlData + "<COUNTRY>" + bShipmentOrder.RecipientCountry + "</COUNTRY>";
            //xmlData = xmlData + "<VAT>GB78687</VAT>";
            //xmlData = xmlData + "<CONTACTNAME>" + bShipmentOrder.RecipientName + "</CONTACTNAME>";
            //xmlData = xmlData + "<CONTACTDIALCODE>44 121</CONTACTDIALCODE>";
            //xmlData = xmlData + "<CONTACTTELEPHONE>717733</CONTACTTELEPHONE>";
            //xmlData = xmlData + "<CONTACTEMAIL>" + bShipmentOrder.RecipientEmail + "</CONTACTEMAIL></RECEIVER>";

            ////xmlData = xmlData + "<DETAILS><RECEIVER><COMPANYNAME>Receiver Co.Ltd</COMPANYNAME>";
            ////xmlData = xmlData + "<STREETADDRESS1>Head Office</STREETADDRESS1>";
            ////xmlData = xmlData + "<STREETADDRESS2>Hoo fddorp</STREETADDRESS2>";
            ////xmlData = xmlData + "<STREETADDRESS3></STREETADDRESS3>";
            ////xmlData = xmlData + "<CITY>Amsterdam</CITY>";
            ////xmlData = xmlData + "<PROVINCE></PROVINCE>";
            ////xmlData = xmlData + "<POSTCODE>1100 kg</POSTCODE>";
            ////xmlData = xmlData + "<COUNTRY>NL</COUNTRY>";
            ////xmlData = xmlData + "<VAT>7668880</VAT>";
            ////xmlData = xmlData + "<CONTACTNAME>Mr Frank</CONTACTNAME>";
            ////xmlData = xmlData + "<CONTACTDIALCODE>1672</CONTACTDIALCODE>";
            ////xmlData = xmlData + "<CONTACTTELEPHONE>987432</CONTACTTELEPHONE>";
            ////xmlData = xmlData + "<CONTACTEMAIL>CITTEST@TEST.CON</CONTACTEMAIL></RECEIVER>";

            //xmlData = xmlData + "<DELIVERY><COMPANYNAME>" + bShipmentOrder.RecipientCompany + "</COMPANYNAME>";
            //xmlData = xmlData + "<STREETADDRESS1>" + bShipmentOrder.RecipientAddress1 + "</STREETADDRESS1>";
            //xmlData = xmlData + "<STREETADDRESS2>" + bShipmentOrder.RecipientAddress2 + "</STREETADDRESS2>";
            //xmlData = xmlData + "<STREETADDRESS3>" + bShipmentOrder.RecipientAddress3 + "</STREETADDRESS3>";
            //xmlData = xmlData + "<CITY>" + bShipmentOrder.RecipientCity + "</CITY>";
            //xmlData = xmlData + "<PROVINCE></PROVINCE>";
            //xmlData = xmlData + "<POSTCODE>" + bShipmentOrder.RecipientZipCode + "</POSTCODE>";
            //xmlData = xmlData + "<COUNTRY>" + bShipmentOrder.RecipientCountry + "</COUNTRY>";
            //xmlData = xmlData + "<VAT>GB78687</VAT>";
            //xmlData = xmlData + "<CONTACTNAME>" + bShipmentOrder.RecipientName + "</CONTACTNAME>";
            //xmlData = xmlData + "<CONTACTDIALCODE>44 121</CONTACTDIALCODE>";
            //xmlData = xmlData + "<CONTACTTELEPHONE>717733</CONTACTTELEPHONE>";
            //xmlData = xmlData + "<CONTACTEMAIL>" + bShipmentOrder.RecipientEmail + "</CONTACTEMAIL></DELIVERY>";



            ////xmlData = xmlData + "<DELIVERY><COMPANYNAME>TNT International</COMPANYNAME>";
            ////xmlData = xmlData + "<STREETADDRESS1>Grovester 19</STREETADDRESS1>";
            ////xmlData = xmlData + "<STREETADDRESS2></STREETADDRESS2>";
            ////xmlData = xmlData + "<STREETADDRESS3></STREETADDRESS3>";
            ////xmlData = xmlData + "<CITY>Hannover</CITY>";
            ////xmlData = xmlData + "<PROVINCE></PROVINCE>";
            ////xmlData = xmlData + "<POSTCODE>30853</POSTCODE>";
            ////xmlData = xmlData + "<COUNTRY>DE</COUNTRY>";
            ////xmlData = xmlData + "<VAT>7668880</VAT>";
            ////xmlData = xmlData + "<CONTACTNAME>Mr Jones</CONTACTNAME>";
            ////xmlData = xmlData + "<CONTACTDIALCODE>1672</CONTACTDIALCODE>";
            ////xmlData = xmlData + "<CONTACTTELEPHONE>987432</CONTACTTELEPHONE>";
            ////xmlData = xmlData + "<CONTACTEMAIL>CITTEST@TEST.CON</CONTACTEMAIL></DELIVERY>";



            //xmlData = xmlData + "<CUSTOMERREF>Reciver09</CUSTOMERREF>";
            //xmlData = xmlData + "<CONTYPE>N</CONTYPE>";
            //xmlData = xmlData + "<PAYMENTIND>S</PAYMENTIND>";
            //xmlData = xmlData + "<ITEMS>2</ITEMS>";
            //xmlData = xmlData + "<TOTALWEIGHT>1.4</TOTALWEIGHT>";
            //xmlData = xmlData + "<TOTALVOLUME>1.0</TOTALVOLUME>";
            //xmlData = xmlData + "<CURRENCY>GBP</CURRENCY>";
            //xmlData = xmlData + "<GOODSVALUE>180.00</GOODSVALUE>";
            //xmlData = xmlData + "<INSURANCEVALUE>150.00</INSURANCEVALUE>";
            //xmlData = xmlData + "<INSURANCECURRENCY>GBP</INSURANCECURRENCY>";
            //xmlData = xmlData + "<SERVICE>15N</SERVICE>";
            //xmlData = xmlData + "<OPTION>PR</OPTION>";
            //xmlData = xmlData + "<DESCRIPTION>assorted office accessories</DESCRIPTION>";
            //xmlData = xmlData + "<DELIVERYINST>ggg</DELIVERYINST>";
            //for (int i = 0; i <= 1; i++)
            //{
            //    xmlData = xmlData + "<PACKAGE><ITEMS>1</ITEMS>";
            //    xmlData = xmlData + "<DESCRIPTION>box" + i + "</DESCRIPTION>";
            //    xmlData = xmlData + "<LENGTH>0.8</LENGTH><HEIGHT>0.9" + i + "</HEIGHT><WIDTH>0.6" + i + "</WIDTH>";
            //    xmlData = xmlData + "<WEIGHT>1" + i + "</WEIGHT>";
            //    xmlData = xmlData + "<ARTICLE><ITEMS>1</ITEMS>";
            //    xmlData = xmlData + "<DESCRIPTION>paperclips</DESCRIPTION>";
            //    xmlData = xmlData + "<WEIGHT>.03</WEIGHT>";
            //    xmlData = xmlData + "<INVOICEVALUE>2.30</INVOICEVALUE>";
            //    xmlData = xmlData + "<INVOICEDESC>metal paperclips</INVOICEDESC>";
            //    xmlData = xmlData + "<HTS>6345</HTS><COUNTRY>GB</COUNTRY>";
            //    xmlData = xmlData + "</ARTICLE></PACKAGE>";
            //}
            //xmlData = xmlData + "</DETAILS></CONSIGNMENT></CONSIGNMENTBATCH>";
            //xmlData = xmlData + "<ACTIVITY><CREATE><CONREF>ref1</CONREF></CREATE>";
            //xmlData = xmlData + "<RATE><CONREF>ref1</CONREF></RATE>";
            //xmlData = xmlData + "<BOOK><CONREF>ref1</CONREF></BOOK>";
            //xmlData = xmlData + "<PRINT><CONNOTE><CONREF>ref1</CONREF>";
            //xmlData = xmlData + "<CONNUMBER>GE012062707</CONNUMBER></CONNOTE>";
            //xmlData = xmlData + "<LABEL><CONREF>ref1</CONREF></LABEL>";
            //xmlData = xmlData + "<MANIFEST><CONREF>ref1</CONREF></MANIFEST>";
            //xmlData = xmlData + "<INVOICE><CONREF>ref1</CONREF>";
            //xmlData = xmlData + "</INVOICE></PRINT><SHOW_GROUPCODE/></ACTIVITY></ESHIPPER>";
            # endregion
            #region need to check
            //string xmlData = @"<?xml version=""1.0"" standalone=""no""?><!DOCTYPE ESHIPPER SYSTEM 'http://164.39.41.88:81/ShipperDTD2.0/EShipperIN2.dtd'><ESHIPPER>";
            //xmlData = xmlData + "<LOGIN><COMPANY>"+cd.UserName+"</COMPANY>";
            //xmlData = xmlData + "<PASSWORD>"+cd.PassWord+"</PASSWORD>";
            //xmlData = xmlData + "<APPID>"+cd.AppIdLabel+"</APPID><APPVERSION>"+cd.Appverersion+"</APPVERSION></LOGIN>";
            //xmlData = xmlData + "<CONSIGNMENTBATCH><SENDER><COMPANYNAME>" + bShipmentOrder.SenderCompany + "</COMPANYNAME>";
            //xmlData = xmlData + "<STREETADDRESS1>" + bShipmentOrder.SenderAddress1 + "</STREETADDRESS1>";
            //xmlData = xmlData + "<STREETADDRESS2>" + bShipmentOrder.SenderAddress2 + "</STREETADDRESS2>";
            //xmlData = xmlData + "<STREETADDRESS3>" + bShipmentOrder.SenderAddress3 + "</STREETADDRESS3>";
            //xmlData = xmlData + "<CITY>" + bShipmentOrder.SenderCity + "</CITY>";
            //xmlData = xmlData + "<PROVINCE>"+cd.Providance+"</PROVINCE>";
            //xmlData = xmlData + "<POSTCODE>" + bShipmentOrder.SenderZipCode + "</POSTCODE>";
            //xmlData = xmlData + "<COUNTRY>" + bShipmentOrder.SenderCountry + "</COUNTRY>";
            //xmlData = xmlData + "<ACCOUNT>"+cd.AccountNO+"</ACCOUNT>";
            //xmlData = xmlData + "<VAT>"+cd.VATSender+"</VAT>";
            //xmlData = xmlData + "<CONTACTNAME>" + bShipmentOrder.SenderName + "</CONTACTNAME>";
            //xmlData = xmlData + "<CONTACTDIALCODE>0</CONTACTDIALCODE>";
            //xmlData = xmlData + "<CONTACTTELEPHONE>"+bShipmentOrder.SenderPhone+"</CONTACTTELEPHONE>";
            //xmlData = xmlData + "<CONTACTEMAIL>" + bShipmentOrder.SenderEmail + "</CONTACTEMAIL><COLLECTION>";
            //#region collectionaddress // can add when we need in application 
            ////xmlData = xmlData + "<COLLECTIONADDRESS><COMPANYNAME>" + bShipmentOrder.SenderCompany + "</COMPANYNAME>";
            ////xmlData = xmlData + "<STREETADDRESS1>" + bShipmentOrder.SenderAddress1 + "</STREETADDRESS1>";
            ////xmlData = xmlData + "<STREETADDRESS2>" + bShipmentOrder.SenderAddress2 + "</STREETADDRESS2>";
            ////xmlData = xmlData + "<STREETADDRESS3>" + bShipmentOrder.SenderAddress3 + "</STREETADDRESS3>";
            ////xmlData = xmlData + "<CITY>" + bShipmentOrder.SenderCity + "</CITY>";
            ////xmlData = xmlData + "<PROVINCE>"+cd.Providance+"</PROVINCE>"; //correct province
            ////xmlData = xmlData + "<POSTCODE>" + bShipmentOrder.SenderZipCode + "</POSTCODE>";
            ////xmlData = xmlData + "<COUNTRY>" + bShipmentOrder.SenderCountry + "</COUNTRY>";
            ////xmlData = xmlData + "<VAT>"+cd.VATReciver+"</VAT>";
            ////xmlData = xmlData + "<CONTACTNAME>" + bShipmentOrder.SenderName + "</CONTACTNAME>";
            ////xmlData = xmlData + "<CONTACTDIALCODE>01827</CONTACTDIALCODE>";
            ////xmlData = xmlData + "<CONTACTTELEPHONE>2341562</CONTACTTELEPHONE>";
            ////xmlData = xmlData + "<CONTACTEMAIL>" + bShipmentOrder.SenderEmail + "</CONTACTEMAIL></COLLECTIONADDRESS>";
            //#endregion
            //string date1 = string.Format("dd/mm/yyyy", bShipmentOrder.WishedShipDate);
            //string date = bShipmentOrder.WishedShipDate.Day.ToString() + "/" + bShipmentOrder.WishedShipDate.Month.ToString() + "/" + bShipmentOrder.WishedShipDate.Year.ToString();
            //xmlData = xmlData + "<SHIPDATE>17/02/2012</SHIPDATE>";
            //xmlData = xmlData + "<PREFCOLLECTTIME><FROM>09:00</FROM><TO>10:00</TO></PREFCOLLECTTIME>";
            //xmlData = xmlData + "<ALTCOLLECTTIME><FROM>11:00</FROM><TO>12:00</TO></ALTCOLLECTTIME>";
            //xmlData = xmlData + "<COLLINSTRUCTIONS>use rear gate </COLLINSTRUCTIONS></COLLECTION></SENDER>";
            //xmlData = xmlData + "<CONSIGNMENT><CONREF>ref1</CONREF>";
            //xmlData = xmlData + "<DETAILS><RECEIVER><COMPANYNAME>" + bShipmentOrder.RecipientCompany + "</COMPANYNAME>";
            //xmlData = xmlData + "<STREETADDRESS1>" + bShipmentOrder.RecipientAddress1 + "</STREETADDRESS1>";
            //xmlData = xmlData + "<STREETADDRESS2>" + bShipmentOrder.RecipientAddress2 + "</STREETADDRESS2>";
            //xmlData = xmlData + "<STREETADDRESS3>" + bShipmentOrder.RecipientAddress3 + "</STREETADDRESS3>";
            //xmlData = xmlData + "<CITY>" + bShipmentOrder.RecipientCity + "</CITY>";
            //xmlData = xmlData + "<PROVINCE></PROVINCE>";
            //xmlData = xmlData + "<POSTCODE>" + bShipmentOrder.RecipientZipCode + "</POSTCODE>";
            //xmlData = xmlData + "<COUNTRY>" + bShipmentOrder.RecipientCountry + "</COUNTRY>";
            //xmlData = xmlData + "<VAT></VAT>";
            //xmlData = xmlData + "<CONTACTNAME>" + bShipmentOrder.RecipientName + "</CONTACTNAME>";
            //xmlData = xmlData + "<CONTACTDIALCODE>44 121</CONTACTDIALCODE>";
            //xmlData = xmlData + "<CONTACTTELEPHONE>717733</CONTACTTELEPHONE>";
            //xmlData = xmlData + "<CONTACTEMAIL>" + bShipmentOrder.RecipientEmail + "</CONTACTEMAIL></RECEIVER>";
            //#region delivary
            //xmlData = xmlData + "<DELIVERY><COMPANYNAME>" + bShipmentOrder.RecipientCompany + "</COMPANYNAME>";
            //xmlData = xmlData + "<STREETADDRESS1>" + bShipmentOrder.RecipientAddress1 + "</STREETADDRESS1>";
            //xmlData = xmlData + "<STREETADDRESS2>" + bShipmentOrder.RecipientAddress2 + "</STREETADDRESS2>";
            //xmlData = xmlData + "<STREETADDRESS3>" + bShipmentOrder.RecipientAddress3 + "</STREETADDRESS3>";
            //xmlData = xmlData + "<CITY>" + bShipmentOrder.RecipientCity + "</CITY>";
            //xmlData = xmlData + "<PROVINCE></PROVINCE>";
            //xmlData = xmlData + "<POSTCODE>" + bShipmentOrder.RecipientZipCode + "</POSTCODE>";
            //xmlData = xmlData + "<COUNTRY>" + bShipmentOrder.RecipientCountry + "</COUNTRY>";
            //xmlData = xmlData + "<VAT></VAT>";
            //xmlData = xmlData + "<CONTACTNAME>" + bShipmentOrder.RecipientName + "</CONTACTNAME>";
            //xmlData = xmlData + "<CONTACTDIALCODE>0</CONTACTDIALCODE>";
            //xmlData = xmlData + "<CONTACTTELEPHONE>"+bShipmentOrder.RecipientPhone+"</CONTACTTELEPHONE>";
            //xmlData = xmlData + "<CONTACTEMAIL>" + bShipmentOrder.RecipientEmail  + "</CONTACTEMAIL></DELIVERY>";
            //#endregion
            //xmlData = xmlData + "<CUSTOMERREF>Reciver09</CUSTOMERREF>";
            //xmlData = xmlData + "<CONTYPE>N</CONTYPE>";
            //xmlData = xmlData + "<PAYMENTIND>S</PAYMENTIND>";
            //xmlData = xmlData + "<ITEMS>4</ITEMS>";
            //xmlData = xmlData + "<TOTALWEIGHT>1.4</TOTALWEIGHT>";
            //xmlData = xmlData + "<TOTALVOLUME>1.0</TOTALVOLUME>";
            //xmlData = xmlData + "<CURRENCY>GBP</CURRENCY>";
            //xmlData = xmlData + "<GOODSVALUE>180.00</GOODSVALUE>";
            //xmlData = xmlData + "<INSURANCEVALUE>150.00</INSURANCEVALUE>";
            //xmlData = xmlData + "<INSURANCECURRENCY>GBP</INSURANCECURRENCY>";
            //xmlData = xmlData + "<SERVICE>15N</SERVICE>";
            //xmlData = xmlData + "<OPTION>PR</OPTION>";
            //xmlData = xmlData + "<DESCRIPTION>assorted office accessories</DESCRIPTION>";
            //xmlData = xmlData + "<DELIVERYINST>ggg</DELIVERYINST>";
            //for (int i = 0; i <= 1; i++)
            //{
            //    xmlData = xmlData + "<PACKAGE><ITEMS>1</ITEMS>";
            //    xmlData = xmlData + "<DESCRIPTION>box" + i + "</DESCRIPTION>";
            //    xmlData = xmlData + "<LENGTH>0.8</LENGTH><HEIGHT>0.9" + i + "</HEIGHT><WIDTH>0.6" + i + "</WIDTH>";
            //    xmlData = xmlData + "<WEIGHT>1" + i + "</WEIGHT>";
            //    xmlData = xmlData + "<ARTICLE><ITEMS>1</ITEMS>";
            //    xmlData = xmlData + "<DESCRIPTION>paperclips</DESCRIPTION>";
            //    xmlData = xmlData + "<WEIGHT>.03</WEIGHT>";
            //    xmlData = xmlData + "<INVOICEVALUE>2.30</INVOICEVALUE>";
            //    xmlData = xmlData + "<INVOICEDESC>metal paperclips</INVOICEDESC>";
            //    xmlData = xmlData + "<HTS>6345</HTS><COUNTRY>GB</COUNTRY>";
            //    xmlData = xmlData + "</ARTICLE></PACKAGE>";
            //}
            //xmlData = xmlData + "</DETAILS></CONSIGNMENT></CONSIGNMENTBATCH>";
            //xmlData = xmlData + "<ACTIVITY><CREATE><CONREF>ref1</CONREF></CREATE>";
            //xmlData = xmlData + "<RATE><CONREF>ref1</CONREF></RATE>";
            //xmlData = xmlData + "<BOOK><CONREF>ref1</CONREF></BOOK>";
            //xmlData = xmlData + "<PRINT><CONNOTE><CONREF>ref1</CONREF>";
            //xmlData = xmlData + "</CONNOTE>";
            //xmlData = xmlData + "<LABEL><CONREF>ref1</CONREF></LABEL>";
            //xmlData = xmlData + "<MANIFEST><CONREF>ref1</CONREF></MANIFEST>";
            //xmlData = xmlData + "<INVOICE><CONREF>ref1</CONREF>";
            //xmlData = xmlData + "</INVOICE></PRINT></ACTIVITY></ESHIPPER>";
            #endregion

            string scity = lb.FrtoEn(bShipmentOrder.SenderCity);
            string rcity = lb.FrtoEn(bShipmentOrder.RecipientCity);
            string xmlData = @"<?xml version=""1.0"" standalone=""no""?><!DOCTYPE ESHIPPER SYSTEM 'http://164.39.41.88:81/ShipperDTD2.0/EShipperIN2.dtd'><ESHIPPER>";
            xmlData = xmlData + "<LOGIN><COMPANY>" + cd.UserName + "</COMPANY>";
            xmlData = xmlData + "<PASSWORD>" + cd.PassWord + "</PASSWORD>";
            xmlData = xmlData + "<APPID>" + cd.AppIdLabel + "</APPID><APPVERSION>" + cd.Appverersion + "</APPVERSION></LOGIN>";
            xmlData = xmlData + "<CONSIGNMENTBATCH><SENDER><COMPANYNAME>" + bShipmentOrder.SenderCompany + "</COMPANYNAME>";
            xmlData = xmlData + "<STREETADDRESS1>" + bShipmentOrder.SenderAddress1 + "</STREETADDRESS1>";
            xmlData = xmlData + "<STREETADDRESS2>" + bShipmentOrder.SenderAddress2 + "</STREETADDRESS2>";
            xmlData = xmlData + "<STREETADDRESS3>" + bShipmentOrder.SenderAddress3 + "</STREETADDRESS3>";
            xmlData = xmlData + "<CITY>" + scity + "</CITY>";
            xmlData = xmlData + "<PROVINCE></PROVINCE>";
            xmlData = xmlData + "<POSTCODE>" + bShipmentOrder.SenderZipCode + "</POSTCODE>";
            xmlData = xmlData + "<COUNTRY>" + bShipmentOrder.SenderCountry + "</COUNTRY>";
            xmlData = xmlData + "<ACCOUNT>" + cd.AccountNO + "</ACCOUNT>";
            xmlData = xmlData + "<VAT></VAT>";
            xmlData = xmlData + "<CONTACTNAME>" + bShipmentOrder.SenderName + "</CONTACTNAME>";
            char Sphonecode = bShipmentOrder.SenderPhone[0];
            string Sphone = bShipmentOrder.SenderPhone.Substring(1);
            if (Sphone == "")
                Sphone = bShipmentOrder.SenderPhone;
            xmlData = xmlData + "<CONTACTDIALCODE>"+Sphonecode+"</CONTACTDIALCODE>";
            xmlData = xmlData + "<CONTACTTELEPHONE>" + Sphone + "</CONTACTTELEPHONE>";
            xmlData = xmlData + "<CONTACTEMAIL>" + bShipmentOrder.SenderEmail + "</CONTACTEMAIL><COLLECTION>";
            //xmlData = xmlData + "<COLLECTIONADDRESS><COMPANYNAME>" + bShipmentOrder.SenderCompany + "</COMPANYNAME>";
            //xmlData = xmlData + "<STREETADDRESS1>" + bShipmentOrder.SenderAddress1 + "</STREETADDRESS1>";
            //xmlData = xmlData + "<STREETADDRESS2>" + bShipmentOrder.SenderAddress2 + "</STREETADDRESS2>";
            //xmlData = xmlData + "<STREETADDRESS3>" + bShipmentOrder.SenderAddress3 + "</STREETADDRESS3>";
            //xmlData = xmlData + "<CITY>" + bShipmentOrder.SenderCity + "</CITY>";
            //xmlData = xmlData + "<PROVINCE></PROVINCE>"; //correct province
            //xmlData = xmlData + "<POSTCODE>" + bShipmentOrder.SenderZipCode + "</POSTCODE>";
            //xmlData = xmlData + "<COUNTRY>" + bShipmentOrder.SenderCountry + "</COUNTRY>";
            //xmlData = xmlData + "<VAT></VAT>";
            //xmlData = xmlData + "<CONTACTNAME>" + bShipmentOrder.SenderName + "</CONTACTNAME>";
            //xmlData = xmlData + "<CONTACTDIALCODE>01827</CONTACTDIALCODE>";
            //xmlData = xmlData + "<CONTACTTELEPHONE>2341562</CONTACTTELEPHONE>";
            //xmlData = xmlData + "<CONTACTEMAIL>" + bShipmentOrder.SenderEmail + "</CONTACTEMAIL></COLLECTIONADDRESS>";
            string date1 = string.Format("dd/mm/yyyy", bShipmentOrder.WishedShipDate);
            //string date = bShipmentOrder.WishedShipDate.Day.ToString() + "/" + bShipmentOrder.WishedShipDate.Month.ToString() + "/" + bShipmentOrder.WishedShipDate.Year.ToString();
            string year = bShipmentOrder.ShipDateTime.Year.ToString();
            string month = (bShipmentOrder.ShipDateTime.Month < 10) ? "0" + bShipmentOrder.ShipDateTime.Month.ToString() : bShipmentOrder.ShipDateTime.Month.ToString();
            string day = (bShipmentOrder.ShipDateTime.Day < 10) ? "0" + bShipmentOrder.ShipDateTime.Day.ToString() : bShipmentOrder.ShipDateTime.Day.ToString();
            string date = day + "/" + month + "/" + year;
            xmlData = xmlData + "<SHIPDATE>" + date + "</SHIPDATE>";
            xmlData = xmlData + "<PREFCOLLECTTIME><FROM>09:00</FROM><TO>10:00</TO></PREFCOLLECTTIME>";
         //   xmlData = xmlData + "<ALTCOLLECTTIME><FROM>11:00</FROM><TO>12:00</TO></ALTCOLLECTTIME>";
            xmlData = xmlData + "<COLLINSTRUCTIONS>" + bShipmentOrder.SenderComments + "</COLLINSTRUCTIONS></COLLECTION></SENDER>";
            xmlData = xmlData + "<CONSIGNMENT><CONREF>ref1</CONREF>";
            xmlData = xmlData + "<DETAILS><RECEIVER><COMPANYNAME>" + bShipmentOrder.RecipientCompany + "</COMPANYNAME>";
            xmlData = xmlData + "<STREETADDRESS1>" + bShipmentOrder.RecipientAddress1 + "</STREETADDRESS1>";
            xmlData = xmlData + "<STREETADDRESS2>" + bShipmentOrder.RecipientAddress2 + "</STREETADDRESS2>";
            xmlData = xmlData + "<STREETADDRESS3>" + bShipmentOrder.RecipientAddress3 + "</STREETADDRESS3>";
            xmlData = xmlData + "<CITY>" + rcity + "</CITY>";
            xmlData = xmlData + "<PROVINCE></PROVINCE>";
            xmlData = xmlData + "<POSTCODE>" + bShipmentOrder.RecipientZipCode + "</POSTCODE>";
            xmlData = xmlData + "<COUNTRY>" + bShipmentOrder.RecipientCountry + "</COUNTRY>";
            xmlData = xmlData + "<VAT></VAT>";
            xmlData = xmlData + "<CONTACTNAME>" + bShipmentOrder.RecipientName + "</CONTACTNAME>";
            char Rphonecode = bShipmentOrder.RecipientPhone[0];
            string Rphone = bShipmentOrder.RecipientPhone.Substring(1);
            if (Rphone == "")
                Rphone = bShipmentOrder.RecipientPhone;
            xmlData = xmlData + "<CONTACTDIALCODE>"+Rphonecode+"</CONTACTDIALCODE>";
            xmlData = xmlData + "<CONTACTTELEPHONE>" + Rphone + "</CONTACTTELEPHONE>";
            xmlData = xmlData + "<CONTACTEMAIL>" + bShipmentOrder.RecipientEmail + "</CONTACTEMAIL></RECEIVER>";
            //xmlData = xmlData + "<DELIVERY><COMPANYNAME>TNT International</COMPANYNAME>";
            //xmlData = xmlData + "<STREETADDRESS1>Grovester 19</STREETADDRESS1>";
            //xmlData = xmlData + "<STREETADDRESS2></STREETADDRESS2>";
            //xmlData = xmlData + "<STREETADDRESS3></STREETADDRESS3>";
            //xmlData = xmlData + "<CITY>Hannover</CITY>";
            //xmlData = xmlData + "<PROVINCE></PROVINCE>";
            //xmlData = xmlData + "<POSTCODE>30853</POSTCODE>";
            //xmlData = xmlData + "<COUNTRY>DE</COUNTRY>";
            //xmlData = xmlData + "<VAT>7668880</VAT>";
            //xmlData = xmlData + "<CONTACTNAME>Mr Jones</CONTACTNAME>";
            //xmlData = xmlData + "<CONTACTDIALCODE>1672</CONTACTDIALCODE>";
            //xmlData = xmlData + "<CONTACTTELEPHONE>987432</CONTACTTELEPHONE>";
            //xmlData = xmlData + "<CONTACTEMAIL>CITTEST@TEST.CON</CONTACTEMAIL></DELIVERY>";
            xmlData = xmlData + "<CUSTOMERREF>Reciver09</CUSTOMERREF>";
            xmlData = xmlData + "<CONTYPE>N</CONTYPE>";
            xmlData = xmlData + "<PAYMENTIND>S</PAYMENTIND>";
            xmlData = xmlData + "<ITEMS>" + bShipmentOrder.UODCount + "</ITEMS>";
            string tweight;
            tweight = bShipmentOrder.TotalWeight.ToString().Replace(",", ".");
            xmlData = xmlData + "<TOTALWEIGHT>" + tweight + "</TOTALWEIGHT>";
            xmlData = xmlData + "<TOTALVOLUME>1.0</TOTALVOLUME>";
            xmlData = xmlData + "<CURRENCY>GBP</CURRENCY>";
            xmlData = xmlData + "<GOODSVALUE>180.00</GOODSVALUE>";
            xmlData = xmlData + "<INSURANCEVALUE>150.00</INSURANCEVALUE>";
            xmlData = xmlData + "<INSURANCECURRENCY>GBP</INSURANCECURRENCY>";
            xmlData = xmlData + "<SERVICE>" + nc + "</SERVICE>";
            xmlData = xmlData + "<OPTION>" + pc + "</OPTION>";
            xmlData = xmlData + "<DESCRIPTION>a</DESCRIPTION>";
            xmlData = xmlData + "<DELIVERYINST>" + bShipmentOrder.RecipientComments + "</DELIVERYINST>";
            // for (int i = 0; i <= 1; i++)
            foreach (temp tttt in te)
            {
                string len;
                string width;
                string height;
                string weight;
                weight = tttt.Weight.ToString().Replace(",", ".");
                len = (tttt.Length / 100).ToString().Replace(",", ".");
                width = (tttt.Width / 100).ToString().Replace(",", ".");
                height = (tttt.Height / 100).ToString().Replace(",", ".");

                xmlData = xmlData + "<PACKAGE><ITEMS>"+tttt.count +"</ITEMS>";
                xmlData = xmlData + "<DESCRIPTION>" + tttt.container + "</DESCRIPTION>";
                xmlData = xmlData + "<LENGTH>" +len+"</LENGTH><HEIGHT>" + height  + "</HEIGHT><WIDTH>" +width+ "</WIDTH>";
                xmlData = xmlData + "<WEIGHT>" + weight + "</WEIGHT>";
                //xmlData = xmlData + "<ARTICLE><ITEMS>1</ITEMS>";
                //xmlData = xmlData + "<DESCRIPTION>paperclips</DESCRIPTION>";
                //xmlData = xmlData + "<WEIGHT>.03</WEIGHT>";
                //xmlData = xmlData + "<INVOICEVALUE>2.30</INVOICEVALUE>";
                //xmlData = xmlData + "<INVOICEDESC>metal paperclips</INVOICEDESC>";
                //xmlData = xmlData + "<HTS>6345</HTS><COUNTRY>GB</COUNTRY></ARTICLE>";
                xmlData = xmlData + "</PACKAGE>";
            }
            xmlData = xmlData + "</DETAILS></CONSIGNMENT></CONSIGNMENTBATCH>";
            xmlData = xmlData + "<ACTIVITY><CREATE><CONREF>ref1</CONREF></CREATE>";
            xmlData = xmlData + "<RATE><CONREF>ref1</CONREF></RATE>";
            xmlData = xmlData + "<BOOK><CONREF>ref1</CONREF></BOOK>";
            xmlData = xmlData + "<PRINT><CONNOTE><CONREF>ref1</CONREF>";
            xmlData = xmlData + "</CONNOTE>";
            xmlData = xmlData + "<LABEL><CONREF>ref1</CONREF></LABEL>";
            xmlData = xmlData + "<MANIFEST><CONREF>ref1</CONREF></MANIFEST>";
            xmlData = xmlData + "<INVOICE><CONREF>ref1</CONREF>";
            xmlData = xmlData + "</INVOICE></PRINT></ACTIVITY></ESHIPPER>";
            string result = Generation(xmlData);
            string[] Split = result.Split(new Char[] { ':' });
            Number = Split[1];
            Number = Number.Trim();
            bCarrierProcessResult = new BCarrierProcessResult();
            string error = "";
            if (Number.Length >= 1)
            {
                xmlData = "GET_RESULT:" + Number;
                ResultXML = Generation(xmlData);
                string Trackingnumber = string.Empty;
                if (ResultXML.Contains("<LABEL>CREATED</LABEL>"))
                {
                    string[] Trackingcontent = Regex.Split(ResultXML, "<CONNUMBER>");
                    foreach (string TT in Trackingcontent)
                    {
                        if (TT.Contains("</CONNUMBER>"))
                        {
                            string[] Trckingno = Regex.Split(TT, "</CONNUMBER>");
                            Trackingnumber = Trckingno[0];
                        }
                    }
                    foreach (BShipmentDetails bship in bShipmentOrder.ShipDetail)                      // (int k = 0; k <= 9; k++)
                    {

                        bship.TrackingNo = Trackingnumber;

                    }
                    Result = true;

                    //label should be stored in the database
                    //manifest should be stored in the database
                    // KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
                   // System.Nullable<int> resultStoreNumber = context.uSP_CARRIER_DOCUMENTS_STORE(bShipmentOrder.ShipReference, "TNTINTERNATIONAL", Number, null, null, null, null, DateTime.Today, null, null).First();

                    string xmlLabelData = "GET_LABEL:" + Number;
                    LabelXML = Generation(xmlLabelData);
                    string xmlmanifestData = "GET_MANIFEST:" + Number;
                    ManifestXML = Generation(xmlmanifestData);
                    bCarrierProcessResult.Carrier = "TNTINTERNATIONAL";
                    bCarrierProcessResult.Result = new byte[] { 1, 2, 3 };
                    bCarrierProcessResult.Output = LabelXML;
                    // Showlabel(Number);
                    bShipmentResult.isLabelGenerated = BEnumFlag.Yes;

                }
                else
                {

                    string[] lines = Regex.Split(ResultXML, "<DESCRIPTION>");
                    foreach (string line in lines)
                    {
                        if (line.Contains("</DESCRIPTION>"))
                        {
                            string[] linesnew = Regex.Split(line, "</DESCRIPTION>");
                            error = error + "<br/>" + linesnew[0];
                        }
                    }
                    Result = false;
                    bShipmentResult.isLabelGenerated = BEnumFlag.No;
                    bShipmentResult.LabelError = error;
                }


            }
            else
            {
                Result = false;
                bShipmentResult.isLabelGenerated = BEnumFlag.No;
                bShipmentResult.LabelError = "Server error";
            }
            if (Result.ToString() == "True")
            {
                System.Nullable<int> resultStoreNumber = context.uSP_CARRIER_DOCUMENTS_STORE(bShipmentOrder.ShipReference, "TNTINTERNATIONAL", Number, null, null, null, null, DateTime.Today, null, null).First();

                resultStoreNumber = context.uSP_CARRIER_DOCUMENTS_STORE(bShipmentOrder.ShipReference, "TNTINTERNATIONAL", null, null, LabelXML, null, null, DateTime.Today, null, null).First();
                bCarrierProcessResult.Carrier = "TNTINTERNATIONAL";
                bCarrierProcessResult.Result = new byte[] { 1, 2, 3 };
                bCarrierProcessResult.Output = LabelXML;
                // Showlabel(Number);
                bShipmentResult.isLabelGenerated = BEnumFlag.Yes;

                resultStoreNumber = context.uSP_CARRIER_DOCUMENTS_STORE(bShipmentOrder.ShipReference, "TNTINTERNATIONAL", null, null, null, ManifestXML, null, null, DateTime.Today, null).First();
            }
            //  Showlabel(Number);
            bShipmentOrder.ShipmentResult = bShipmentResult;
            return bShipmentOrder;
        }

        public void Showlabel(string number)
        {
            //string xmlData = "GET_LABEL:" + number;
            //LabelXML = Generation(xmlData);
            //System.Web.HttpContext.Current.Session["one"] = LabelXML;
            Page page = (Page)HttpContext.Current.Handler;

            string url = page.ResolveUrl(@"~\carrier\Label.aspx");
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
            var result = context.uSP_GET_CARRIER_COMMUNIVATIONNUMBER(order.ShipReference).First();

            bCarrierProcessResult = new BCarrierProcessResult();
            string xmlLabelData = "GET_LABEL:" + result.COMMUNICATIONNUMBER;
            LabelXML = Generation(xmlLabelData);
            bCarrierProcessResult.Carrier = "TNTINTERNATIONAL";
            bCarrierProcessResult.Result = new byte[] { 1, 2, 3 };
            bCarrierProcessResult.Output = LabelXML;
            return order;
        }

        public BShipmentOrder ReterievbeManifest(BShipmentOrder order, out BCarrierProcessResult bCarrierProcessResult)
        {
            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            var result = context.uSP_GET_CARRIER_COMMUNIVATIONNUMBER(order.ShipReference).First();

            bCarrierProcessResult = new BCarrierProcessResult();
            string xmlManifestData = "GET_MANIFEST:" + result.COMMUNICATIONNUMBER;
            ManifestXML = Generation(xmlManifestData);
            bCarrierProcessResult.Carrier = "TNTINTERNATIONAL";
            bCarrierProcessResult.Output = ManifestXML;


            //bool Result;
            //if (ResultXML.Contains("<MANIFEST>CREATED</MANIFEST>"))
            //{
            //    Result = true;
            //    ShowManifest(Number);
            //}
            //else
            //    Result = false;

            return order;
        }

        public string GetTracking(string trackingno)
        {
            string[] Split = trackingno.Split(new Char[] { ',' });

            Uri targetUri = new Uri("http://iConnection.tnt.com:81/VBGateway.asp");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(targetUri);

            string formParameterName = "xml_in";

            //string xmlData = @"<?xml version=""1.0"" standalone=""no""?><!DOCTYPE document SYSTEM 'http://164.39.41.88:81/trackerDTD1.0/trackerIN.dtd'><document><APPLICATION>Tr</APPLICATION><VERSION>1.0</VERSION><USER_NAME>navilib</USER_NAME><PASSWORD>navilib123</PASSWORD><TRACK_TYPE>Consignment</TRACK_TYPE><TRACK_DATA><ExtraDetails>SenderAddress</ExtraDetails><ExtraDetails>ReceiverAddress</ExtraDetails><ExtraDetails>shipment</ExtraDetails><ExtraDetails>package</ExtraDetails><ExtraDetails>Statcode</ExtraDetails><ExtraDetails>statdesc</ExtraDetails><Tdata>402776739</Tdata></TRACK_DATA></document>";
            string xmlData = @"<?xml version=""1.0"" standalone=""no""?><!DOCTYPE document SYSTEM 'http://164.39.41.88:81/trackerDTD1.0/trackerIN.dtd'><document><APPLICATION>Tr</APPLICATION><VERSION>1.0</VERSION>";
            xmlData = xmlData + "<USER_NAME>navilib</USER_NAME>";
            xmlData = xmlData + "<PASSWORD>navilib123</PASSWORD>";
            xmlData = xmlData + "<TRACK_TYPE>Consignment</TRACK_TYPE>";
            xmlData = xmlData + " <TRACK_DATA><ExtraDetails>SenderAddress</ExtraDetails>";
            xmlData = xmlData + "<ExtraDetails>ReceiverAddress</ExtraDetails>";
            xmlData = xmlData + "<ExtraDetails>shipment</ExtraDetails>";
            xmlData = xmlData + " <ExtraDetails>package</ExtraDetails>";
            xmlData = xmlData + "<ExtraDetails>Statcode</ExtraDetails>";
            xmlData = xmlData + "<ExtraDetails>statdesc</ExtraDetails>";
            for (int i = 0; i <= Split.Count() - 1; i++)
            {
                xmlData = xmlData + "<Tdata>" + Split[i] + "</Tdata>";
            }
            //xmlData= xmlData  +"<Tdata>402776739</Tdata>";
            xmlData = xmlData + "</TRACK_DATA></document>";
            //  xmlData = xmlData + "<Tdata>402888982</Tdata></TRACK_DATA></document>";
            //xmlData = xmlData + "<Tdata>GE403800235GB</Tdata></TRACK_DATA></document>";
            string sendString = formParameterName + "=" + HttpUtility.UrlEncode(xmlData);
            byte[] byteStream;
            byteStream = System.Text.Encoding.UTF8.GetBytes(sendString);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteStream.LongLength;

            using (Stream writer = request.GetRequestStream())
            {
                writer.Write(byteStream, 0, (int)request.ContentLength);
                writer.Flush();
            }

            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();


            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string final = sr.ReadToEnd();
            string endj;
            string[] lines = Regex.Split(final, "\r\n");
            if (lines.Length >= 2)
            {
                string j = null;
                lines[0] = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + "\r\n" + @"<?xml-stylesheet href=""style.xslt"" type=""text/xsl"" ?>";
                foreach (string line in lines)
                {
                    j = j + line + "\r\n";
                }
                endj = j;
            }


            return final;
        }

        public void ShowManifest(string number)
        {
            string xmlData = "GET_MANIFEST:" + number;
            ManifestXML = Generation(xmlData);
            System.Web.HttpContext.Current.Session["one"] = ManifestXML;
            Page page = (Page)HttpContext.Current.Handler;

            string url = page.ResolveUrl(@"~\carrier\TNTNINTERNATIONALMANIFEST.aspx");
            string url1 = page.Request.Url.AbsolutePath.ToString();
            string url2 = page.Request.Url.AbsoluteUri.Substring(0, page.Request.Url.AbsoluteUri.IndexOf(page.Request.Url.LocalPath));
            //System.Web.HttpContext.Current.Response.Redirect(@"Carr\About.aspx");
            //Page page = (Page)HttpContext.Current.Handler;
            //string url = page.ResolveClientUrl(@"Carr/About.aspx");
            System.Web.HttpContext.Current.Response.Write("<script>");
            System.Web.HttpContext.Current.Response.Write(@"window.open('" + url2 + url + "','_blank')");
            System.Web.HttpContext.Current.Response.Write("</script>");
        }

    }

    public class temp
    {
        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public int count { get; set; }
        public string container { get; set; }
    }

    class ProductComparer : IEqualityComparer<temp>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(temp x, temp y)
        {

            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.Weight==y.Weight && x.Length==y.Length && x.Height == y.Height && x.Width == y.Width  ;
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(temp product)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(product, null)) return 0;

            int hashProductWeight = product.Weight.GetHashCode();
            int hashProductLength = product.Length.GetHashCode();
            //Get hash code for the Name field if it is not null.
            int hashProductCode = product.Height.GetHashCode();
            int hashProductName = product.Width.GetHashCode(); 

            //Get hash code for the Code field.
           

            
            //Calculate the hash code for the product.
            return hashProductWeight ^hashProductLength^ hashProductName ^ hashProductCode ;
        }

    }



}
