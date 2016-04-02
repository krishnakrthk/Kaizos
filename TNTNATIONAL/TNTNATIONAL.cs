using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

using System.Text;
using System.Text.RegularExpressions;

//Web Reference to TNT service
using Carriers.TNTServiceReference;

using Kaizos.Entities.Business;
using Kaizos.Components.GlobalLibrary;

namespace Kaizos.Components.Carriers
{

    public class TNTNATIONAL
    {

        public BCarriercredentials Getcarrierdetails()
        {
            BCarriercredentials CD = new BCarriercredentials();

            #region Reteriving Carrier Crdentials from Database

            string sCarrierCode = "TNTNATIONAL";

            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            CD.ExtratrackingDetail1 = "06324676";
            var result = context.uSP_GET_CARRIER_PARAMETERS(sCarrierCode).ToList();
            foreach (var rec in result)
            {
                if (rec.KEY_CODE.Equals("AccountNo"))
                {
                    CD.ExtratrackingDetail1 = rec.KEY_VALUE;
                }
            }
            #endregion
            return CD;
        }

        public BShipmentOrder GetFeasibility(BShipmentOrder bShipmentOrder)
        {
            Library lb = new Library();
            BCarriercredentials cd;
            cd = Getcarrierdetails();
            BShipmentResult bShipmentResult = new BShipmentResult();
            bShipmentResult.FeasibilityError = "";
            bShipmentResult.isFeasibility = BEnumFlag.No;
            bShipmentResult.isLabelGenerated = BEnumFlag.No;
            bShipmentResult.isManifestGenerated = BEnumFlag.No;
            bShipmentResult.isOther = BEnumFlag.No;
            bShipmentResult.LabelError = "";
            bShipmentResult.ManifestError = "";
            bShipmentResult.OtherError = "";
            string Return;
            bool result = false;
            string scity = lb.FrtoEn(bShipmentOrder.SenderCity);
            string rcity = lb.FrtoEn(bShipmentOrder.RecipientCity);
            if (bShipmentOrder.SenderCountry == "FR" && bShipmentOrder.RecipientCountry == "FR")
            {
               

                ServicesClient proxy = new ServicesClient();

                feasibilityParameter fs = new feasibilityParameter();

                sender sen = new sender();
                receiver re = new receiver();
                sen.city = "LYON";
                sen.zipCode = "69007";

                sen.city = scity;
                sen.zipCode = bShipmentOrder.SenderZipCode;

                re.city = "ALFORTVILLE";
                re.zipCode = "94140";

                re.city = rcity;
                re.zipCode = bShipmentOrder.RecipientZipCode;

                fs.shippingDate = bShipmentOrder.ShipDateTime.ToString();
                string year = bShipmentOrder.ShipDateTime.Year.ToString();
                string month = (bShipmentOrder.ShipDateTime.Month < 10) ? "0" + bShipmentOrder.ShipDateTime.Month.ToString() : bShipmentOrder.ShipDateTime.Month.ToString();
                string day = (bShipmentOrder.ShipDateTime.Day < 10) ? "0" + bShipmentOrder.ShipDateTime.Day.ToString() : bShipmentOrder.ShipDateTime.Day.ToString();
                fs.shippingDate = year + "-" + month + "-" + day;

                //fs.shippingDate = bShipmentOrder.ShipDateTime.Year.ToString();
                //string month = (bShipmentOrder.ShipDateTime.Month < 10) ? "0" + bShipmentOrder.ShipDateTime.Month.ToString() : bShipmentOrder.ShipDateTime.Month.ToString();
                //fs.shippingDate = fs.shippingDate + "-" + month;
                //string day = (bShipmentOrder.ShipDateTime.Day < 10) ? "0" + bShipmentOrder.ShipDateTime.Day.ToString() : bShipmentOrder.ShipDateTime.Day.ToString();
                //fs.shippingDate = fs.shippingDate + "-" + day;

                fs.accountNumber = cd.ExtratrackingDetail1; //must be derived from the accountno.

                fs.sender = sen;
                fs.receiver = re;
                service[] sa;
                try
                {
                    sa = proxy.feasibility(fs);

                    //either if service can be passed as parameter it should be passed or 
                    //else the returned service should be compare with available one
                    string sServicecode;
                    //KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
                    //var res = context.uSP_GET_CARRIERSPECIFIC_SERVICE(bShipmentOrder.Carrier).First();
                    //sServicecode = res.ACTUALCODE;
                    foreach (service a in sa)
                    {
                        Return = a.dueDate + a.insurance + a.priorityGuarantee + a.saturdayDelivery + a.serviceCode + a.serviceLabel + "<br>";

                    }

                    bShipmentResult.isFeasibility = BEnumFlag.Yes;

                    result = true;
                }
                catch (Exception ee)
                {
                    bShipmentResult.FeasibilityError = ee.Message;
                    bShipmentResult.isFeasibility = BEnumFlag.No;
                    result = false;
                }
            }
            else
            {
                bShipmentResult.FeasibilityError = "Only National service  ";
                bShipmentResult.isFeasibility = BEnumFlag.No;
                result = false;
            }
            bShipmentOrder.ShipmentResult = bShipmentResult;
            return bShipmentOrder;
        }

        public BShipmentOrder GetLabel(BShipmentOrder bshipmentOrder, out BCarrierProcessResult bCarrierProcessResult)
        {
            Library lb = new Library();
            BCarriercredentials cd;
            cd = Getcarrierdetails();
            byte[] dd = null;
            bool Result;
            string[] email = new string[1];
            string servicetype = "A";
            ServicesClient proxy = new ServicesClient();
            BShipmentResult bShipmentResult = new BShipmentResult();
            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            string servicecode = context.uSP_CARRIER_SERVICE_CODE(bshipmentOrder.CarrierService).DefaultIfEmpty().First();
            if (!(servicecode == null))
            {
                servicetype = servicecode;//.CARRIER_SERVICE_CODE;
            }
            bShipmentResult.FeasibilityError = "";
            bShipmentResult.isFeasibility = BEnumFlag.No;
            bShipmentResult.isLabelGenerated = BEnumFlag.No;
            bShipmentResult.isManifestGenerated = BEnumFlag.No;
            bShipmentResult.isOther = BEnumFlag.No;
            bShipmentResult.LabelError = "";
            bShipmentResult.ManifestError = "";
            bShipmentResult.OtherError = "";
            string scity = lb.FrtoEn(bshipmentOrder.SenderCity);
            string rcity = lb.FrtoEn(bshipmentOrder.RecipientCity);


            #region Hardcoded values
            // expeditionCreationParameter EC = new expeditionCreationParameter();
            // EC.accountNumber = "08920975";
            // EC.shippingDate = "2012-02-02";
            // pickUpRequest pur = new pickUpRequest();
            // pur.closingTime = "16:00";
            // mediaType mm1 = mediaType.EMAIL;
            // pur.media = mm1;
            // pur.mediaSpecified  = true ; // mediaType.EMAIL;
            // pur.phoneNumber = "0238557354";

            // email[0] = "jjjj@gmail.com";
            // pur.emailAddress = email;
            // EC.pickUpRequest  = pur;
            // sender2 ss = new sender2();
            // ss.address1 = "58, avenue Leclerc";
            // ss.city = "LYON";
            // ss.zipCode = "69007";
            // ss.phoneNumber = "0248736433";
            // ss.name = "TNT";
            //// ss.typeId = "y";
            // ss.emailAddress = "ssss@tnt.com";
            // ss.contactFirstName = "rex";
            // ss.contactLastName = "josh";
            // ss.faxNumber = "0248659617";
            // EC.sender = ss;
            // receiver2 rc2 = new receiver2();
            // rc2.name = "TNT-CRETEIL";
            // rc2.address1 = "13 ALLEE JEAN BAPTISTE PREUX ";
            // rc2.address2 = "PARC D'ACTIVITE DU VAL DE SEINE ";
            // rc2.zipCode = "94140";
            // rc2.city = "ALFORTVILLE";
            // rc2.contactFirstName = "JOHN";
            // rc2.contactLastName = "PAUL";
            // rc2.emailAddress = "jj@yahoo.com";
            // rc2.phoneNumber = "0287378798";
            // rc2.accessCode = "y";
            // EC.receiver = rc2;
            // EC.serviceCode = "A";
            // EC.quantity = "10";
            // paybackInfo pbs = new paybackInfo();
            // pbs.address1 = "";
            // pbs.address2 = "";

            // parcelsRequest pcreq = new parcelsRequest();
            // parcelRequest[] PQ;
            // PQ = new parcelRequest[10];
            // int j = 1;
            // for (int i = 0; i <= 9; i++)
            // {
            //     PQ[i] = new parcelRequest();
            //     PQ[i].sequenceNumber = j.ToString();
            //     PQ[i].customerReference = "Peter"+i;
            //     PQ[i].weight = (i+2).ToString();
            //     PQ[i].insuranceAmount = (25+i).ToString();
            //     PQ[i].comment = "Handle with care";
            //     j++;
            // }
            // pcreq.parcelRequest = PQ;
            // EC.parcelsRequest = pcreq;
            // EC.saturdayDelivery = "0";
            // EC.labelFormat = "STDA4";
            #endregion

            expeditionCreationParameter EC = new expeditionCreationParameter();
            EC.accountNumber = cd.ExtratrackingDetail1; // /*bshipmentOrder.AccountNo;*/"06324676";
            string year = bshipmentOrder.ShipDateTime.Year.ToString();
            string month = (bshipmentOrder.ShipDateTime.Month < 10) ? "0" + bshipmentOrder.ShipDateTime.Month.ToString() : bshipmentOrder.ShipDateTime.Month.ToString();
            string day = (bshipmentOrder.ShipDateTime.Day < 10) ? "0" + bshipmentOrder.ShipDateTime.Day.ToString() : bshipmentOrder.ShipDateTime.Day.ToString();
            EC.shippingDate = year + "-" + month + "-" + day; //bshipmentOrder.ShipDateTime.ToString();//"2012-02-10";

            pickUpRequest pur = new pickUpRequest();
            mediaType mm1 = mediaType.EMAIL;
            pur.media = mm1;
            pur.mediaSpecified = true;
            pur.faxNumber = "";
            email[0] = bshipmentOrder.SenderEmail;//"jjjj@gmail.com";
            pur.emailAddress = email;
            pur.notifySuccess = "1";
            pur.service = "";
            string[] Split = bshipmentOrder.SenderName.Split(new Char[] { '-' });
            pur.firstName = Split[0]; //"Yohann";
            pur.lastName = Split[1];
            if (bshipmentOrder.SenderPhone.Length <= 10 && bshipmentOrder.SenderPhone.Length > 1)
            {
                if (bshipmentOrder.SenderPhone[0] == '0')
                {
                    pur.phoneNumber = bshipmentOrder.SenderPhone; //"0238557354";
                }
            }
            else
                pur.phoneNumber = "0222557354";
            pur.closingTime = bshipmentOrder.SenderCollectDeadLine; //"16:00";
            pur.instructions = bshipmentOrder.SenderComments;//"hhhhhhhhhh";
            // mediaType.EMAIL;
            EC.pickUpRequest = pur;
            sender2 ss = new sender2();
            ss.address1 = bshipmentOrder.SenderAddress1;//"58, avenue Leclerc";
            ss.address2 = bshipmentOrder.SenderAddress2 + bshipmentOrder.SenderAddress3;
            ss.city = scity;//"LYON";
            ss.zipCode = bshipmentOrder.SenderZipCode;//"69007";
            //ss.phoneNumber = /*bshipmentOrder.SenderPhone;*/"0248736433";
            if (bshipmentOrder.SenderPhone.Length == 10)
            {
                if (bshipmentOrder.SenderPhone[0] == '0')
                {
                    ss.phoneNumber = bshipmentOrder.SenderPhone; //"0238557354";
                }
            }
            else
                ss.phoneNumber = "";

            ss.name = bshipmentOrder.SenderName;//"TNT";
            //ss.typeId = "y";
            ss.emailAddress = bshipmentOrder.SenderEmail; //"ssss@tnt.com";
            string[] Split1 = bshipmentOrder.SenderName.Split(new Char[] { '-' });
            ss.contactFirstName = Split1[0];//"rex";
            ss.contactLastName = Split1[1];//"josh";
            ss.faxNumber = "";//"0248659617";
            EC.sender = ss;
            receiver2 rc2 = new receiver2();
            rc2.name = bshipmentOrder.RecipientCompany;//"TNT-CRETEIL";
            rc2.address1 = bshipmentOrder.RecipientAddress1;//"13 ALLEE JEAN BAPTISTE PREUX ";
            rc2.address2 = bshipmentOrder.RecipientAddress2 + bshipmentOrder.RecipientAddress3;//"PARC D'ACTIVITE DU VAL DE SEINE ";
            rc2.zipCode = bshipmentOrder.RecipientZipCode;//"94140";
            rc2.city = rcity; //"ALFORTVILLE";
            string[] Split2 = bshipmentOrder.SenderName.Split(new Char[] { '-' });
            rc2.contactFirstName = Split2[0];//"JOHN";
            rc2.contactLastName = Split2[1];//"PAUL";
            rc2.emailAddress = bshipmentOrder.RecipientEmail;//"jj@yahoo.com";
            //rc2.phoneNumber = /*bshipmentOrder.RecipientPhone*/"0287378798";
            bshipmentOrder.RecipientPhone = bshipmentOrder.RecipientPhone.Trim();
            if (bshipmentOrder.RecipientPhone.Length == 10)
            {
                if (bshipmentOrder.RecipientPhone[0] == '0')
                {
                    rc2.phoneNumber = bshipmentOrder.RecipientPhone; //"0238557354";
                }
            }
            else
                rc2.phoneNumber = "";
            rc2.accessCode = "y";
            EC.receiver = rc2;
            EC.serviceCode = servicetype;
            EC.quantity = bshipmentOrder.UODCount.ToString();
            paybackInfo pbs = new paybackInfo();
            pbs.address1 = "";
            pbs.address2 = "";
            pbs.paybackAmount = "100";
            //EC.paybackInfo = pbs;
            parcelsRequest pcreq = new parcelsRequest();
            parcelRequest[] PQ;
            PQ = new parcelRequest[bshipmentOrder.UODCount];
            int j = 1;
            foreach (BShipmentDetails bShipmentDetails in bshipmentOrder.ShipDetail)
            // for (int i = 0; i <= 9; i++)
            {
                PQ[j - 1] = new parcelRequest();
                PQ[j - 1].sequenceNumber = j.ToString();
                PQ[j - 1].customerReference = bShipmentDetails.ContentType;
                PQ[j - 1].weight = bShipmentDetails.Weight.ToString();//(i+2).ToString();
                PQ[j - 1].insuranceAmount = "";
                PQ[j - 1].comment = bShipmentDetails.Container;
                j++;
            }
            pcreq.parcelRequest = PQ;
            EC.parcelsRequest = pcreq;
            EC.saturdayDelivery = "0";
            EC.labelFormat = "STDA4";
            bCarrierProcessResult = new BCarrierProcessResult();
            try
            {
                //testname a = new testname();
                //a.er = proxy.expeditionCreation(EC);
                expeditionResponse er = proxy.expeditionCreation(EC);
                // string h=  er.ToString();
                dd = er.PDFLabels;
                string[] name = new string[bshipmentOrder.UODCount];
                parcelResponse[] pr = new parcelResponse[bshipmentOrder.UODCount];
                int k = 0;
                string manifest = string.Empty;
                string stemp = string.Empty;
                foreach (BShipmentDetails bship in bshipmentOrder.ShipDetail)                      // (int k = 0; k <= 9; k++)
                {
                    pr[k] = er.parcelResponses[k];
                    name[k] = pr[k].parcelNumber;
                    bship.TrackingNo = name[k];
                    string sDate = bshipmentOrder.ShipDateTime.ToString("dd/MM/yyyy");
                    string hhh = bship.ContentType + "~" + bship.Weight + "~" + bship.Width + "~" + bship.Height + "~" + bship.Length + "~" + name[k] + "~" + sDate + "~" + bshipmentOrder.SenderCompany + "~" + bshipmentOrder.SenderName + "~" + bshipmentOrder.SenderAddress1 + "~" + bshipmentOrder.SenderAddress2 + bshipmentOrder.SenderAddress3 + "~" + bshipmentOrder.SenderCity + "~" + bshipmentOrder.SenderZipCode + "~" + bshipmentOrder.SenderPhone + "~" + bshipmentOrder.SenderCountry;
                    hhh = hhh + "~" + bshipmentOrder.RecipientCompany + "~" + bshipmentOrder.RecipientName + "~" + bshipmentOrder.RecipientCity + "~" + bshipmentOrder.RecipientZipCode + "~" + bshipmentOrder.RecipientCountry;
                    hhh = hhh + "~" + bshipmentOrder.CurrencyType + "~" + "YES" + "~" + bshipmentOrder.ShipReference + "~" + bshipmentOrder.UODCount + "~" + bshipmentOrder.TotalWeight;
                    manifest = manifest + hhh + "|";
                    k++;
                }
                bShipmentResult.isLabelGenerated = BEnumFlag.Yes;
                Result = true;


                bCarrierProcessResult.Carrier = "TNTNATIONAL";
                bCarrierProcessResult.Result = dd;
                //label should be stored in the database
                //manifest should be stored in the database
                // KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
                System.Nullable<int> result = context.uSP_CARRIER_DOCUMENTS_STORE(bshipmentOrder.ShipReference, "TNTNATIONAL", null, dd, null, null, null, DateTime.Today, null, null).First();
                result = context.uSP_CARRIER_DOCUMENTS_STORE(bshipmentOrder.ShipReference, "TNTNATIONAL", null, null, null, manifest, null, null, DateTime.Today, null).First();
                //   result = context.uSP_CARRIER_DOCUMENTS_STORE(bshipmentOrder.ShipReference, "GLS", null, null, null, strOutput, null, null, DateTime.Today, null).First();

            }
            catch (Exception ee)
            {
                bShipmentResult.isLabelGenerated = BEnumFlag.No;
                bShipmentResult.LabelError = ee.Message;
                Result = false;
            }
            //ShowLabel(dd);
            bshipmentOrder.ShipmentResult = bShipmentResult;

            return bshipmentOrder;
        }


        public void ShowLabel(byte[] ar)
        {
            System.Web.HttpContext.Current.Session["one"] = ar;
            Page page = (Page)HttpContext.Current.Handler;
            string url = page.ResolveUrl(@"~\carrier\LabelTNTnat.aspx");
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

}
