using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using Kaizos.Entities.Business;

namespace KaizosServiceLibrary.Model
{
    [DataContract]
    public class SCarrierService
    {
        [DataMember]
        public int ServiceID { get; set; }
        [DataMember]
        public string CarrierName { get; set; }
        [DataMember]
        public string MasterServiceName { get; set; }
        [DataMember]
        public SEnumPriority Priority { get; set; }
        [DataMember]
        public string ServiceName { get; set; }
        [DataMember]
        public string ServiceCode { get; set; }
        [DataMember]
        public string DeliveryDelayTable { get; set; }
        [DataMember]
        public string DeliveryDeadLine { get; set; }
        [DataMember]
        public SEnumFlag Active { get; set; }
        [DataMember]
        public SEnumFlag KeyCustomerService { get; set; }

        /***********[KS15MAR12]**********/
        [DataMember]
        public string Information { get; set; }
        [DataMember]
        public string InfoType { get; set; }
        [DataMember]
        public string CarrierServiceCode { get; set; }
    }
    
    [DataContract]
    public class SDeliveryDelay
    {
        [DataMember]
        public string Origin { get; set; }
        [DataMember]
        public string Destination { get; set; }
        [DataMember]
        public int Delay { get; set; }
    }

    [DataContract]
    public class SCarrier
    {
        [DataMember]
        public int CarrierID { get; set; }
        [DataMember]
        public string CarrierName { get; set; }
        [DataMember]
        public bool ReferencedCarrier { get; set; }
        [DataMember]
        public bool Active { get; set; }
        [DataMember]
        public int ClaimDelay { get; set; }

    }

    [DataContract]
    public class SCarrierProcessType
        {
          [DataMember(IsRequired=true)]
          public SEnumCarrierProcess enumCarrierProcessType { get; set; }
        }

    [DataContract] 
    public class SCarrierProcessResult
     {
        [DataMember]
         public string Carrier { get; set; }
        [DataMember]
         public string Output { get; set; }
        [DataMember]
         public byte[] Result { get; set; }

     }

    [DataContract]
    public enum SEnumCarrierProcess
    {
        [EnumMember]
        Dummy =0,
        [EnumMember]
        Feasable =1 ,
        [EnumMember]
        Label =2 ,
        [EnumMember]
        Manifes =3,
        [EnumMember]
        Track=4,
        [EnumMember]
        Invoice=5,
        [EnumMember]
        ShowLabel=6,
        [EnumMember]
        ShowManifest=7,
        [EnumMember]
        ShowInvoice=8

    }


    //for carrier output 29FEB12KS
    [DataContract]
    public class SCarrierOutput
    {
        [DataMember]
        public string ShippingReference { get; set; }
        [DataMember]
        public string Carrier { get; set; }
        [DataMember]
        public string CommubicationNumber { get; set; }
        [DataMember]
        public byte[] LabelByte { get; set; }
        [DataMember]
        public string Label { get; set; }
        [DataMember]
        public string Manifest { get; set; }
        [DataMember]
        public string Invoice { get; set; }
    }


}



