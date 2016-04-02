using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kaizos.Entities.Business
{
    public class BAddressBook
    {
        public string AddressID { get; set; }
        public BEnumAddressType AddressType { get; set; }
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
        public BEnumShipPreference ShipPreference { get; set; }
        public BEnumDeliveryType AddressUsedFor { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public string AccountNo { get; set; }
        public string NamedCarrier { get; set; }
        public string ShipPreferenceOrder { get; set; }
    }

    public class BToS
    {
        public int ID { get; set; }
        public string TermsOfService { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public BEnumFlag Active { get; set; }
        public string AccountNo { get; set; }
    }

    public class PaymentDetails
    {
        public double TotalAmount { get; set; }

        public string CreditCardType
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int CreditCardNumber
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public string CreditCardExpiryDate
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public string CreditCardCVVNumber
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public string ReturnURL
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }

    public class Claim
    {
        public string AccountNo { get; set; }
        public string ShipReference { get; set; }
        public BEnumIssueType IssueType { get; set; }
        public string IssueDescription {get;set;}
        public float GoodsValue { get; set; }
        public float DamageValue { get; set; }
        public float CostOfRepair { get; set; }
        public int WeightOfDamagedGoods { get; set; }
        public BEnumFlag ReservesTaken { get; set; }
        public string ConsignmentFile { get; set; }
        public string InvoiceFile { get; set; }
        public string BillFile { get; set; }
        public string EstimationFile { get; set; }
        public string ReservesFile { get; set; }
        public BEnumClaimStatus Status { get; set; }
        public DateTime ClaimDate { get; set; }
        public DateTime ClosedDate { get; set; }
    }

    public class Directory
    {
        public string CountryCode { get; set; }
        public string Language { get; set; }
        public string DirectoryID { get; set; }
        public string ISO2 { get; set; }
        public string Region1 { get; set; }
        public string Region2 { get; set; }
        public string Region3 { get; set; }
        public string Region4 { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Area1 { get; set; }
        public string Area2 { get; set; }
        public string TimeZone { get; set; }
        public string UniversalTime { get; set; }
        public BEnumFlag DayLightSaving { get; set; }
    }

    /// <summary>
    /// To return the status of the import file after process
    /// </summary>
    public class BFileImportStatus
    {
        public int RowNumber { get; set; }
        public string FieldName { get; set; }
        public string ErrorDescription { get; set; }
    }

    public class BComboText
    {
        public string ComboText { get; set; }
    }

    public class BComboTableField
    {
        public string TableName { get; set; }
        public string FieldName { get; set; }
    }

    public class BNextcounter
    {
        public string ErrorDescription { get; set; }
        public string ErrorCode { get; set; }
        public int NextCounter { get; set; }

    }

    public class BCountryTable
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CodeName { get; set; }
    }

    public class BIndustry
    {
        public string Department { get; set; }
        public string Activity { get; set; }
    }
    public class BKeyValue
    {
        public string Value { get; set; }
        public bool IsValid { get; set; }
        public string ErrorDescription { get; set; }
        public string ErrorCode { get; set; }
    }
}
