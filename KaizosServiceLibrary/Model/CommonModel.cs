using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace KaizosServiceLibrary.Model
{
    [DataContract]
    public class SComboText
    {
        [DataMember]
        public string ComboText { get; set; }
    }

    [DataContract]
    public class SComboTableField
    {
        [DataMember]
        public string TableName { get; set; }
        [DataMember]
        public string FieldName { get; set; }
    }
    
    public class SAddressBook
    {
        public string AddressID { get; set; }
        public SEnumAddressType AddressType { get; set; }
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string TelephoneNo { get; set; }
        public string FaxNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string LastPickupMondayToThursday { get; set; }
        public string LastPickupFriday { get; set; }
        public string Comments { get; set; }
        public SEnumShipPreference ShipPreference { get; set; }
        public SEnumDeliveryType AddressUsedFor { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public string AccountNo { get; set; }
        public string NamedCarrier { get; set; }
        public string ShipPreferenceOrder { get; set; }
    }

    [DataContract]
    public class SNextcounter
    {
        [DataMember]
        public string ErrorDescription { get; set; }
        [DataMember]
        public string ErrorCode { get; set; }
        [DataMember]
        public int NextCounter { get; set; }

    }

    [DataContract]
    public class SCountryTable
    {
        [DataMember]
        public string CountryCode { get; set; }
        [DataMember]
        public string CountryName { get; set; }
        [DataMember]
        public string CodeName { get; set; }
    }

    [DataContract]
    public class SIndustry
    {
        [DataMember]
        public string Department { get; set; }
        [DataMember]
        public string Activity { get; set; }
    }

    [DataContract]
    public class SKeyValue
    {
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public bool IsValid { get; set; }
        [DataMember]
        public string ErrorDescription { get; set; }
        [DataMember]
        public string ErrorCode { get; set; }
    }
}
