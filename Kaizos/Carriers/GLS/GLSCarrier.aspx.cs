using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.ServiceModel;
using System.ServiceModel.Channels;
using KaizosServiceInvokers.KaizosServiceReference;

//using BusinessLogic;
using OnBarcode.Barcode;

using log4net;
using log4net.Config;

using Kaizos;

namespace GLSFinal.Carrier
{
    public partial class GLSCarrier : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ILog logger = log4net.LogManager.GetLogger(typeof(GLSCarrier));
            //try
            {
                display a = new display();
                if (a.value == "Normal")
                {

                    ReportViewer1.LocalReport.ReportPath = @"Carriers\GLS\NormalReport.rdlc";
                    ObjectDataSource1.TypeName = "GLSFinal.Carrier.display";
                    ObjectDataSource1.SelectMethod = "NormalLbl";
                    ReportViewer1.Visible = true;

                }
                else if (a.value == "Emergency")
                {

                    ReportViewer1.LocalReport.ReportPath = @"Carriers\GLS\EmergencyReport.rdlc";
                    ObjectDataSource1.TypeName = "GLSFinal.Carrier.display";
                    ObjectDataSource1.SelectMethod = "EmergencyLbl";
                    ReportViewer1.Visible = true;

                }
                else if (a.value == "Zipcode Error")
                {
                    Response.Write("Error in Zipcode");
                    ReportViewer1.Visible = false;

                }
                else if (a.value == "Countrycode Error")
                {

                    Response.Write("Error in Country Code");
                    ReportViewer1.Visible = false;

                }

            }


        }
    }

    public class display : GLSCarrier
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(GLSCarrier));
        public string value;
        public string eBar;
        public int parcelNumber, cnt, wt;
        public string strResponse;
        public string exp;
        public int totwgt, nop = 0;
        public string t541 = "hai111", t7864 = "Bye", res, t8702, t8973, wid, hgt;
        public string ncode, pcode, isocode, legh, cusref, conref, serv, desc, ordno, curr, insur, reccom, sencom, senadd1, senadd2, towgt, t540, t530, t860, t863, t861, t862, t330, t864, t100, t8906, t871, t859, t854, t8908, t810, t821, t822, t823, t8700, t8915, t8914, t8904, t8905, t8975, t082, t090, t110, t310, t101, t320, t8913, t8902, t8903;
        public float lent, slen, swid, shgt;
        SShipmentOrder bs = new SShipmentOrder();
        SShipmentDetails bo = new SShipmentDetails();
        public string upt;
        public int j = 1;
        public bool val;
        public string[] wgt = new string[99];
        public string[] lgh = new string[99];
        public string[] wit = new string[99];
        public string[] hit = new string[99];
        public string[] tId = new string[99];
        public string[] pBcode = new string[99];
        public string[] sBcode = new string[99];
        public string[] conId = new string[99];
        public string[] cusId = new string[99];
        public string[] tourNo = new string[99];
        public string[] finalLoc = new string[99];
        public string[] outBound = new string[99];
        public string[] pno = new string[99];
        public string[] inBound = new string[99];
        public string[] parcelCount = new string[99];
        public string[] pickupLoc = new string[99];
        public string[] consigref = new string[99];
        public string[] custref = new string[99];
        public string[] shipref = new string[99];
        public string[] objdesc = new string[99];
        public int l;
        public display()
        {
            // try
            {
                strResponse = Session["Label"].ToString();
                string[] Split1 = strResponse.Split(new Char[] { ';' });
                l = Split1.Length;
                #region loop
                for (int k = 0; k <= l - 1; k++)
                {
                    string[] Split = Split1[k].Split(new Char[] { '|' });
                    int len = Split.Length;
                    for (int i = 1; i < len - 1; i++)
                    {

                        if (Split[i].Contains("T540:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t540 = Split[i].Replace("T540:", "");

                            }
                        }
                        else if (Split[i].Contains("T530:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t530 = Split[i].Replace("T530:", "");
                                wgt[k] = t530;
                            }
                        }
                        else if (Split[i].Contains("T541:"))
                        {
                            t541 = Split[i].Replace("T541:", "");
                        }

                        else if (Split[i].Contains("T860:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t860 = Split[i].Replace("T860:", "");

                            }
                        }
                        else if (Split[i].Contains("T863:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t863 = Split[i].Replace("T863:", "");
                            }
                        }
                        else if (Split[i].Contains("T861:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t861 = Split[i].Replace("T861:", "");
                            }
                        }
                        else if (Split[i].Contains("T862:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t862 = Split[i].Replace("T862:", "");
                            }
                        }
                        else if (Split[i].Contains("T330:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t330 = Split[i].Replace("T330:", "");
                            }
                        }
                        else if (Split[i].Contains("T864:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t864 = Split[i].Replace("T864:", "");
                            }
                        }
                        else if (Split[i].Contains("T100:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t100 = Split[i].Replace("T100:", "");
                            }
                        }
                        else if (Split[i].Contains("T8906:"))
                        {
                            t8906 = Split[i].Replace("T8906:", "");
                        }
                        else if (Split[i].Contains("T871:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t871 = Split[i].Replace("T871:", "");
                            }
                        }
                        else if (Split[i].Contains("T859:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t859 = Split[i].Replace("T859:", "");
                            }
                        }
                        else if (Split[i].Contains("T8908:"))
                        {
                            t8908 = Split[i].Replace("T8908:", "");
                        }
                        else if (Split[i].Contains("T810:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t810 = Split[i].Replace("T810:", "");
                            }
                        }
                        else if (Split[i].Contains("T821:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t821 = Split[i].Replace("T821:", "");
                            }
                        }
                        else if (Split[i].Contains("T822:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t822 = Split[i].Replace("T822:", "");
                            }
                        }
                        else if (Split[i].Contains("T823:"))
                        {
                            t823 = Split[i].Replace("T823:", "");
                        }
                        else if (Split[i].Contains("T8700:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t8700 = Split[i].Replace("T8700:", "");
                                pickupLoc[k] = t8700;
                            }
                        }
                        else if (Split[i].Contains("T8915:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t8915 = Split[i].Replace("T8915:", "");
                                cusId[k] = t8915;
                            }
                        }
                        else if (Split[i].Contains("T8914:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t8914 = Split[i].Replace("T8914:", "");
                                conId[k] = t8914;
                            }
                        }
                        else if (Split[i].Contains("T8904:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t8904 = Split[i].Replace("T8904:", "");
                            }
                        }
                        else if (Split[i].Contains("T8905:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t8905 = Split[i].Replace("T8905:", "");
                            }
                        }
                        else if (Split[i].Contains("T8975:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t8975 = Split[i].Replace("T8975:", "");
                                pno[k] = t8975;

                            }

                        }
                        else if (Split[i].Contains("T082:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t082 = Split[i].Replace("T082:", "");
                            }
                        }
                        else if (Split[i].Contains("T090:"))
                        {
                            if (Split[i].Contains("RESULT:"))
                            {
                                res = Split[i];
                            }
                            else
                            {
                                t090 = Split[i].Replace("T090:", "");
                            }
                        }
                        else if (Split[i].Contains("T110:"))
                        {
                            t110 = Split[i].Replace("T110:", "");
                            outBound[k] = t110;
                        }
                        else if (Split[i].Contains("T310:"))
                        {
                            t310 = Split[i].Replace("T310:", "");
                            inBound[k] = t310;
                        }
                        else if (Split[i].Contains("len"))
                        {
                            legh = Split[i].Replace("len:", "");
                            lgh[k] = legh;
                        }
                        else if (Split[i].Contains("wid"))
                        {
                            wid = Split[i].Replace("wid:", "");
                            wit[k] = wid;
                        }
                        else if (Split[i].Contains("hgt"))
                        {
                            hgt = Split[i].Replace("hgt:", "");
                            hit[k] = hgt;
                        }
                        else if (Split[i].Contains("T101:"))
                        {
                            t101 = Split[i].Replace("T101:", "");
                            finalLoc[k] = t101;
                        }
                        else if (Split[i].Contains("T320:"))
                        {
                            t320 = Split[i].Replace("T320:", "");
                            tourNo[k] = t320;
                        }
                        else if (Split[i].Contains("T8913:"))
                        {
                            t8913 = Split[i].Replace("T8913:", "");
                            tId[k] = t8913;
                        }
                        else if (Split[i].Contains("T8902:"))
                        {
                            t8902 = Split[i].Replace("T8902:", "");
                            pBcode[k] = t8902;
                        }
                        else if (Split[i].Contains("T8903:"))
                        {
                            t8903 = Split[i].Replace("T8903:", "");
                            sBcode[k] = t8903;
                        }
                        else if (Split[i].Contains("RESULT"))
                        {
                            res = Split[i];
                        }
                        else if (Split[i].Contains("T8702:"))
                        {
                            t8702 = Split[i].Replace("T8702:", "");
                            parcelCount[k] = t8702;
                        }
                        else if (Split[i].Contains("nc:"))
                        {
                            ncode = Split[i].Replace("nc:", "");
                            //parcelCount[k] = t8702;
                        }
                        else if (Split[i].Contains("pc:"))
                        {
                            pcode = Split[i].Replace("pc:", "");
                            //parcelCount[k] = t8702;
                        }
                        else if (Split[i].Contains("isocode:"))
                        {
                            isocode = Split[i].Replace("isocode:", "");
                            //parcelCount[k] = t8702;
                        }
                        else if (Split[i].Contains("custref:"))
                        {
                            cusref = Split[i].Replace("custref:", "");
                            custref[k] = cusref;
                        }
                        else if (Split[i].Contains("conref:"))
                        {
                            conref = Split[i].Replace("conref:", "");
                            consigref[k] = conref;
                        }
                        else if (Split[i].Contains("serv:"))
                        {
                            serv = Split[i].Replace("serv:", "");
                        }
                        else if (Split[i].Contains("T8973:"))
                        {
                            t8973 = Split[i].Replace("T8973:", "");
                        }
                        else if (Split[i].Contains("desc:"))
                        {
                            desc = Split[i].Replace("desc:", "");
                            objdesc[k] = desc;
                        }
                        else if (Split[i].Contains("orderno:"))
                        {
                            ordno = Split[i].Replace("orderno:", "");
                            shipref[k] = ordno;
                        }
                        else if (Split[i].Contains("curr:"))
                        {
                            curr = Split[i].Replace("curr:", "");
                        }
                        else if (Split[i].Contains("curr:"))
                        {
                            curr = Split[i].Replace("curr:", "");
                        }
                        else if (Split[i].Contains("insur:"))
                        {
                            insur = Split[i].Replace("insur:", "");
                        }
                        else if (Split[i].Contains("reccom:"))
                        {
                            reccom = Split[i].Replace("reccom:", "");
                        }
                        else if (Split[i].Contains("totwgt:"))
                        {
                            towgt = Split[i].Replace("totwgt:", "");
                        }
                        else if (Split[i].Contains("sencom:"))
                        {
                            sencom = Split[i].Replace("sencom:", "");
                        }
                        else if (Split[i].Contains("senadd1:"))
                        {
                            senadd1 = Split[i].Replace("senadd1:", "");
                        }
                        else if (Split[i].Contains("senadd2:"))
                        {
                            senadd2 = Split[i].Replace("senadd2:", "");
                        }



                    }
                }

                #endregion

                if (res.Contains("RESULT:E000"))
                {

                    value = "Normal";

                }
                else if (res.Contains("RESULT:E002:T330"))
                {
                    value = "Zipcode Error";
                }

                else if (res.Contains("RESULT:E002:T100"))
                {
                    value = "Countrycode Error";

                }

                else if (res.Contains("RESULT:E006"))
                {
                    value = "Emergency";

                }
                else if (res.Contains("RESULT:E999"))
                {
                    value = "Emergency";

                }

                else
                {
                    value = "Emergency";

                }
            }



        }

        public List<Class1> NormalLbl()
        {
            ILog logger = log4net.LogManager.GetLogger(typeof(GLSCarrier));
            int totwgt1 = 0;
            List<Class1> objList = new List<Class1>();
            Class1 obj;
            //foreach (BShipmentDetails bo in bs.ShipDetail)
            for (cnt = 0; cnt < l - 1; cnt++)
            {
                //try
                {
                    obj = new Class1();
                    obj.Shippingdate = t540;
                    obj.ConsigneeName = reccom;  //t860
                    obj.ConsigneeStreet = t863;  //t863
                    obj.Consigneestreet2 = t861; //t861
                    obj.ConsigneeStreet3 = senadd1;//t861; //t862
                    obj.Consigneezipcode = t330;
                    obj.Consigneetown = t864;
                    obj.Commentary = t8906;
                    obj.Consigneephonenumber = t871;
                    obj.Consigneereference = t859;
                    obj.Referencesupp1 = t8906;
                    obj.Referencesupp2 = t860.Replace("-", " ");
                    obj.ShipperName = sencom;  //t810;
                    obj.Shippercountry = t821;
                    obj.ShipperZipcode = t822;
                    obj.ShipperTown = t823;
                    obj.GLSoutboundDepot = pickupLoc[cnt];
                    if (cusId[cnt] == null)
                    {
                        obj.CustomerId = "";
                    }
                    else
                    {
                        obj.CustomerId = StringTool.Truncate(cusId[cnt], 10);
                    }
                    if (conId[cnt] == null)
                    {
                        obj.ContactId = "";
                    }
                    else
                    {
                        obj.ContactId = StringTool.Truncate(conId[cnt], 10);
                    }
                    obj.Parcelrangeindeliveryconsignment = parcelCount[cnt];
                    obj.Amoutparcelindeliveryconsignment = t8973;
                    obj.GLSOriginReference = pno[cnt];
                    obj.Constant = t090;
                    obj.UNIQUENO = t541;
                    obj.Outboundsortingflag = outBound[cnt];//StringTool.Truncate(outBound[cnt], 3);
                    obj.Inboundsirtubgflag = inBound[cnt];//.Truncate(inBound[cnt], 1);
                    obj.Destinationcountry = StringTool.Truncate(t100, 2);
                    obj.Finallocation = StringTool.Truncate(t101, 4);
                    obj.Tournumber = tourNo[cnt];//.Truncate(tourNo[cnt], 4);
                    obj.ZIPCode = t330;
                    obj.GLSTrackId = StringTool.Truncate(tId[cnt], 8);
                    DataMatrix dm1 = new DataMatrix();
                    dm1.Resolution = 36 * 36;
                    dm1.Data = StringTool.Truncate(pBcode[cnt], 123);
                    DataMatrix dm2 = new DataMatrix();
                    dm2.Resolution = 36 * 36;
                    dm2.Data = StringTool.Truncate(sBcode[cnt], 106);
                    obj.PrimaryDatamatrixbarcode = dm1.drawBarcodeAsBytes();
                    obj.SecondaryDatamatrixbarcode = dm2.drawBarcodeAsBytes();
                    obj.Weight = wgt[cnt].ToString();
                    obj.SenderCompanyname = sencom;
                    obj.SenderStreet1 = senadd1;
                    obj.SenderStreet2 = senadd2;
                    obj.Time = t541;
                    obj.Currency = curr;
                    obj.Insurance = insur;
                    obj.OrderNumber = shipref[cnt];
                    obj.TotalWeight = towgt;
                    obj.ReceiverCompanyname = reccom;
                    obj.ReceiverPincode = t330;//"60018";
                    obj.TrackingNumberDetails = t8913;
                    obj.Description = desc;
                    obj.Length = lgh[cnt];// "25cm";
                    obj.Width = wit[cnt];// "20cm";
                    obj.Height = hit[cnt];// "30cm";
                    obj.ReceiverCountry = t821;
                    totwgt1 = totwgt1 + cnt;
                    nop = cnt;
                    //obj.TotalWeight = totwgt1.ToString();
                    obj.NumberofParcels = nop.ToString();
                    objList.Add(obj);
                }



            }


            return objList;

        }


        public List<Class1> EmergencyLbl()
        {
            ILog logger = log4net.LogManager.GetLogger(typeof(GLSCarrier));
            int totwgt1 = 0;
            List<Class1> objList = new List<Class1>();
            for (int e = 0; e < l - 1; e++)
            {
                //try
                {
                    string T860 = ESplit.MetSplit(t860, 20);
                    string T861 = ESplit.MetSplit(t861, 20);
                    string T862 = ESplit.MetSplit(t862, 20);
                    string T863 = ESplit.MetSplit(t863, 20);
                    string T8915 = ESplit.MetSplit(t8915, 10);
                    string T8914 = ESplit.MetSplit(t8914, 10);
                    string T330 = ESplit.MetSplit(t330, 7);
                    string T8702 = ESplit.MetSplit(t8702, 3);
                    string T8973 = ESplit.MetSplit(t8973, 3);
                    string T864 = ESplit.MetSplit(t864, 20);
                    string T871 = ESplit.MetSplit(t871, 20);
                    string T8975 = ESplit.MetSplit(t8975, 22);
                    string T530 = ESplit.MetSplit(t530, 6);
                    string sno = "null";
                    string streetno = ESplit.MetSplit(sno, 5);
                    string crefn = consigref[e];
                    string crefnumber = ESplit.MetSplit(crefn, 20);
                    string nrefn = custref[e];
                    string nrefnumber = ESplit.MetSplit(nrefn, 20);
                    string service = serv.ToString();
                    string comment = ESplit.MetSplit(service, 52);
                    string procode = ESplit.MetSplit(pcode, 2);
                    string isoccode = ESplit.MetSplit(isocode, 3);


                    eBar = "A|" + T8915 + "|" + T8914 + "|" + procode + "|" + isoccode + "|" + T330 + "|" + T8702 + "|" + T8973 + "|" + crefnumber + "|" + T860 + "|" + T861 + "|" + T862 + "|" + T863 + "|" + streetno + "|" + T864 + "|" + T871 + "|" + nrefnumber + "|" + T8975 + "|" + T530 + "|" + comment + "|";
                    int len = eBar.Length;

                    Class1 eobj;
                    {
                        eobj = new Class1();
                        eobj.Shippingdate = t540;
                        eobj.Weight = wgt[e].ToString();
                        eobj.ConsigneeName = reccom; // cg4
                        eobj.ConsigneeStreet = t863;
                        eobj.Consigneestreet2 = t861;
                        eobj.ConsigneeStreet3 = t862;
                        eobj.Consigneezipcode = t330;
                        eobj.Consigneetown = t864;
                        eobj.Commentary = t8906;
                        eobj.Consigneephonenumber = t871; // cg2
                        eobj.Consigneereference = t859;
                        eobj.Referencesupp1 = t8906; //cg3
                        eobj.Referencesupp2 = t860.Replace("-", " "); // cg1
                        eobj.ShipperName = sencom; // cg5
                        eobj.Shippercountry = t821;
                        eobj.ShipperZipcode = t822;
                        eobj.ShipperTown = t823;
                        eobj.GLSoutboundDepot = t8700;
                        eobj.CustomerId = t8915;
                        eobj.ContactId = t8914;
                        eobj.Parcelrangeindeliveryconsignment = t8904;
                        eobj.Amoutparcelindeliveryconsignment = t8905;
                        eobj.GLSOriginReference = t8975;
                        eobj.Constant = t090;
                        eobj.UNIQUENO = t541;
                        eobj.Outboundsortingflag = t110;
                        eobj.Inboundsirtubgflag = t310;
                        eobj.Destinationcountry = t100;
                        eobj.Finallocation = t101;
                        eobj.Tournumber = t320;
                        eobj.ZIPCode = t330;
                        eobj.GLSTrackId = consigref[e];
                        DataMatrix dm1 = new DataMatrix();
                        dm1.Resolution = 36 * 36;
                        dm1.Data = eBar;
                        DataMatrix dm2 = new DataMatrix();
                        dm2.Resolution = 36 * 36;
                        dm2.Data = t8903;
                        eobj.PrimaryDatamatrixbarcode = dm1.drawBarcodeAsBytes();
                        eobj.SecondaryDatamatrixbarcode = dm2.drawBarcodeAsBytes();
                        eobj.SenderCompanyname = sencom;
                        eobj.SenderStreet1 = senadd1;
                        eobj.SenderStreet2 = senadd2;
                        eobj.Time = t541;
                        eobj.Currency = curr;
                        eobj.Insurance = insur;
                        eobj.OrderNumber = shipref[e];
                        eobj.TotalWeight = towgt;
                        eobj.ReceiverCompanyname = reccom;
                        eobj.ReceiverPincode = t330;
                        eobj.TrackingNumberDetails = t8913;
                        eobj.Description = desc;
                        eobj.Length = lgh[e];// "25cm";
                        eobj.Width = wit[e];// "20cm";
                        eobj.Height = hit[e];// "30cm";
                        eobj.ReceiverCountry = t821;
                        //totwgt1 = totwgt1 + e;
                        nop = e;
                        //eobj.TotalWeight = totwgt1.ToString();
                        eobj.NumberofParcels = nop.ToString();
                        objList.Add(eobj);
                    }


                }


            }

            return objList;


        }

    }

    public class Class1
    {
        public static string value;
        public static string wgrst;
        public static string maxweight;
        public string Shippingdate
        { get; set; }
        public string Weight
        { get; set; }
        public string ConsigneeName
        { get; set; }
        public string ConsigneeStreet
        { get; set; }
        public string Consigneestreet2
        { get; set; }
        public string ConsigneeStreet3
        { get; set; }
        public string Consigneezipcode
        { get; set; }
        public string Consigneetown
        { get; set; }
        public string Consigneecountry
        { get; set; }
        public string Commentary
        { get; set; }
        public string Consigneephonenumber
        { get; set; }
        public string Consigneereference
        { get; set; }
        public string Referencesupp1
        { get; set; }
        public string Referencesupp2
        { get; set; }
        public string ShipperName
        { get; set; }
        public string Shippercountry
        { get; set; }
        public string ShipperZipcode
        { get; set; }
        public string ShipperTown
        { get; set; }
        public string GLSoutboundDepot
        { get; set; }
        public string CustomerId
        { get; set; }
        public string ContactId
        { get; set; }
        public string Parcelrangeindeliveryconsignment
        { get; set; }
        public string Amoutparcelindeliveryconsignment
        { get; set; }
        public string GLSOriginReference
        { get; set; }
        public string UNIQUENO
        { get; set; }
        public string Constant
        { get; set; }
        public string Outboundsortingflag
        { get; set; }
        public string Inboundsirtubgflag
        { get; set; }
        public string Destinationcountry
        { get; set; }
        public string Finallocation
        { get; set; }
        public string Tournumber
        { get; set; }
        public string ZIPCode
        { get; set; }
        public string GLSTrackId
        { get; set; }
        public byte[] PrimaryDatamatrixbarcode
        { get; set; }
        public byte[] SecondaryDatamatrixbarcode
        { get; set; }
        public string SenderCompanyname
        { get; set; }
        public string SenderStreet1
        { get; set; }
        public string SenderStreet2
        { get; set; }
        public string SenderPincode
        { get; set; }
        public string SenderTelno
        { get; set; }
        public string Time
        { get; set; }
        public string Currency
        { get; set; }
        public string Insurance
        { get; set; }
        public string OrderNumber
        { get; set; }
        public string TrackingNumber
        { get; set; }
        public string NumberofParcels
        { get; set; }
        public string TotalWeight
        { get; set; }
        public string ReceiverCompanyname
        { get; set; }
        public string ReceiverPincode
        { get; set; }
        public string ReceiverCountry
        { get; set; }
        public string ReceiverTelno
        { get; set; }
        public string TrackingNumberDetails
        { get; set; }
        public string Description
        { get; set; }
        public string Length
        { get; set; }
        public string Width
        { get; set; }
        public string Height
        { get; set; }

    }

    public static class ESplit
    {
        public static string MetSplit(string name, int lent)
        {
            int astringlen = name.Length;
            if (astringlen < lent)
            {
                int p = lent - astringlen;
                for (int j = 1; j <= p; j++)
                {
                    name = name + " ";
                }
            }
            else
            {
                name = name.Substring(0, lent);
            }

            return name;
        }
    }

    public static class StringTool
    {
        public static string Truncate(string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length);
            }
            return source;
        }
    }

}