using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNTNATIONAL.carrier
{
    public class TNTNatonalManifest
    {
      

    }

    public class Manifestdetails
    {
        public string SenderCompanyname
        { get; set; }
        public string SenderStreet1
        { get; set; }
        public string SenderStreet2
        { get; set; }
        public string SenderCity
        { get; set; }
        public string SenderPincode
        { get; set; }
        public string SenderName
        { get; set; }

        public string SenderCountry
        { get; set; }
        public string SenderTelno
        { get; set; }
        public string Date
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
        public string ReceiverStreet1
        { get; set; }

        public string ReceiverStreet2
        { get; set; }
        public string ReceiverCity
        { get; set; }
        public string ReceiverPincode
        { get; set; }
        public string ReceiverCountry
        { get; set; }
        public string ReceiverName
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
        public string Weight
        { get; set; }
    }


}