using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TNTNATIONAL.carrier
{
    public partial class ManifestTNTNat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            //string manifest = 
           
        }
    }

    //public class Class1
    //{
    //    public string SenderCompanyname { get; set; }
    //    public string SenderStreet1 { get; set; }
    //    public string SenderStreet2 { get; set; }
    //    public string SenderCity { get; set; }
    //    public string SenderPincode { get; set; }
    //    public string SenderName { get; set; }
    //    public string SenderCountry { get; set; }
    //    public string SenderTelno { get; set; }
    //    public string Date { get; set; }
    //    public string Time { get; set; }
    //    public string Currency { get; set; }
    //    public string Insurance { get; set; }
    //    public string OrderNumber { get; set; }
    //    public string TrackingNumber { get; set; }
    //    public string NumberofParcels { get; set; }
    //    public string TotalWeight { get; set; }
    //    public string ReceiverCompanyname { get; set; }
    //    public string ReceiverStreet1 { get; set; }
    //    public string ReceiverStreet2 { get; set; }
    //    public string ReceiverCity { get; set; }
    //    public string ReceiverPincode { get; set; }
    //    public string ReceiverCountry { get; set; }
    //    public string ReceiverName { get; set; }
    //    public string ReceiverTelno { get; set; }
    //    public string TrackingNumberDetails { get; set; }
    //    public string Length { get; set; }
    //    public string Width { get; set; }
    //    public string Height { get; set; }
    //    public string Weight { get; set; }
       
    //}

    public class class2 :ManifestTNTNat 
    {
     

        public List<Manifestdetails> manifest()
        {
            string ag = Session["ManifetstTNTNAT"].ToString(); 
            string[] Split = ag.Split(new Char[] { '|' });
            List<Manifestdetails> collection = new List<Manifestdetails>();
            for (int i = 0; i <= Split.Length - 1; i++)
            {
                Manifestdetails mf = new Manifestdetails();
                string[] Split1 = Split[i].Split(new Char[] { '~' });
                if (Split1.Length == 25)
                {
                    mf.Description = Split1[0];
                    mf.Weight = Split1[1];
                    mf.Width = Split1[2];
                    mf.Height = Split1[3];
                    mf.Length = Split1[4];
                    mf.TrackingNumber = Split1[5];
                    mf.Date = Split1[6];
                    mf.SenderCompanyname = Split1[7];
                    mf.SenderName = Split1[8];
                    mf.SenderStreet1 = Split1[9];
                    mf.SenderStreet2 = Split1[10];
                    mf.SenderCity = Split1[11];
                    mf.SenderPincode = Split1[12];
                    mf.SenderTelno = Split1[13];
                    mf.SenderCountry = Split1[14];
                    mf.ReceiverCompanyname = Split1[15];
                    mf.ReceiverName = Split1[16];
                    mf.ReceiverCity = Split1[17];
                    mf.ReceiverPincode = Split1[18];
                    mf.ReceiverCountry = Split1[19];
                    mf.Currency = Split1[20];
                    mf.Insurance = Split1[21];
                    mf.OrderNumber = Split1[22];//ShipReference
                    mf.NumberofParcels = Split1[23];
                    mf.TotalWeight = Split1[24];
                    DateTime time = DateTime.Now;              // Use current time
                    string format = "HH:mm";
                    mf.Time = time.ToString(format);
                    collection.Add(mf);
                }


            }
            return collection;
        }
    }

}