using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace KaizosServiceLibrary.Model
{
    [DataContract]
    public class SUser
    {
        [DataMember]
        public string AccountNo { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public SEnumUserType UserType { get; set; }
        [DataMember]
        public SEnumUserStatus Status { get; set; }
        [DataMember]
        public SEnumFlag IsSalesTarrifAssigned { get; set; }
        [DataMember]
        public SEnumFlag IsToSAccepted { get; set; }
        [DataMember]
        public DateTime ToSAcceptedDate { get; set; }
        [DataMember]
        public string Language { get; set; }
        [DataMember]
        public DateTime LastLogin { get; set; }
        [DataMember]
        public SEnumFlag IsChangePasswordRequired { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public SEnumUserType CreatedUserType { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string TelephoneNo { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public DateTime LastUpdate { get; set; }
        [DataMember]
        public string LegalForm { get; set; }
        [DataMember]
        public SEnumCustomerType CustomerType { get; set; }
        [DataMember]
        public SEnumFlag CustomerTypeChanged { get; set; }
        [DataMember]
        public string VatNo { get; set; }
        [DataMember]
        public string SiretNo { get; set; }
    }

    [DataContract]
    public class SFranchise :SUser
    {
        [DataMember]
        public string CommercialName { get; set; }
        [DataMember]
        public int ManPower { get; set; }
        [DataMember]
        public string FaxNo { get; set; }
        [DataMember]
        public string RegistrationNo { get; set; }
        [DataMember]
        public string Address1 { get; set; }
        [DataMember]
        public string Address2 { get; set; }
        [DataMember]
        public string Address3 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
        [DataMember]
        public string AssignedZone { get; set; }
        [DataMember]
        public string ScannedDoc { get; set; }    //to be deside to store either directly in table or in physical folder.
        [DataMember]
        public int PaymentDelay { get; set; }    //22FEB12SM
        [DataMember]
        public DateTime FirmCreationDate { get; set; } //22FEB12SM
        [DataMember]
        public string CarrierAccountRef { get; set; } //22FEB12SM
    }

    [DataContract]
    public class SAuthorized:SUser
    {
        [DataMember]
        public string ContactName { get; set; }
        [DataMember]
        public string ReferentUserId { get; set; }
    }

    [DataContract]
    public class SCustomer : SUser
    {
        [DataMember]
        public string Designation { get; set; }
        [DataMember]
        public string HqZipcode { get; set; }
        [DataMember]
        public SEnumFlag IsKeyAccount { get; set; }
        [DataMember]
        public SEnumCustCategory CustomerCategory { get; set; }
        [DataMember]
        public string ZipContact { get; set; }
        [DataMember]
        public string IndustryType { get; set; }
        [DataMember]
        public string ChiefContact { get; set; }
        [DataMember]
        public string ContactName { get; set; }
        [DataMember]
        public string InvoicePhoneNumber { get; set; }
        [DataMember]
        public string InvoiceFaxNo { get; set; }
        [DataMember]
        public string InvoiceAddress1 { get; set; }
        [DataMember]
        public string InvoiceAddress2 { get; set; }
        [DataMember]
        public string InvoiceAddress3 { get; set; }
        [DataMember]
        public string InvoiceZipcode { get; set; }
        [DataMember]
        public string InvoiceCity { get; set; }
        [DataMember]
        public string InvoiceCountry { get; set; }
        [DataMember]
        public SEnumFlag UsedForShipping { get; set; }
        [DataMember]
        public SEnumFlag UsedForReturn { get; set; }
        [DataMember]
        public string ShipmentPreference { get; set; }
        [DataMember]
        public SEnumPaymentType PaymentMethod { get; set; }
        [DataMember]
        public SEnumFlag DeferedPaymentRequired { get; set; }
        [DataMember]
        public decimal BudgetAmount { get; set; }
        [DataMember]
        public SEnumFlag IsDeferredPaymentAgreed { get; set; }
        [DataMember]
        public decimal WishedBudgetAmount { get; set; }
        [DataMember]
        public String AdministratorUserId { get; set; }
        [DataMember]
        public decimal InsuredCreditAmount { get; set; }
        [DataMember]
        public int PaymentDelayDays { get; set; }
        [DataMember]
        public int PaymentDelayMonth { get; set; }
        [DataMember]
        public decimal CompensationRate { get; set; }
        [DataMember]
        public decimal CompensationAmount { get; set; }
        [DataMember]
        public decimal DepositAmount { get; set; }
        [DataMember]
        public decimal AuthorizedCreditLimit { get; set; }
        [DataMember]
        public decimal AuthorizedCreditAmount { get; set; }
        [DataMember]
        public string CarrierAccountReference { get; set; }
        [DataMember]
        public decimal AvailableCreditLimit { get; set; }
        [DataMember]
        public decimal AvailableCredit { get; set; }
        [DataMember]
        public int ManPower { get; set; }
        [DataMember]
        public decimal GrossMargin { get; set; }
        [DataMember]
        public string DEFERED_PAYMENT_TYPE { get; set; }    //[16FEB12SM]
        [DataMember]
        public string KEY_CARRIER { get; set; }             //[16FEB12SM]
        [DataMember]
        public decimal SUBSCRIPTION_AMOUNT { get; set; }    //[16FEB12SM]
        [DataMember]
        public string EXTRA_INFO { get; set; }              //[16FEB12SM]
        [DataMember]
        public SEnumFlag FICTIVE_ACCOUNT { get; set; }      //[16FEB12SM]
        [DataMember]
        public decimal TurnOver { get; set; }      //[16FEB12SM]
        [DataMember]
        public int ADV { get; set; }      //[16FEB12SM]
        [DataMember]
        public string CarrierName { get; set; }      //[16FEB12SM]
        [DataMember]
        public string InsuredMethod { get; set; }      //[16FEB12SM]
        [DataMember]
        public DateTime FirmDate { get; set; }
    }

    [DataContract]
    public class SFunctionality
    {
        [DataMember]
        public int FunctionalCode { get; set; }
        [DataMember]
        public string FunctionalName { get; set; }
        [DataMember]
        public string Description { get; set; }
    }

    [DataContract]
    public class SFunctionalProfile
    {
        [DataMember]
        public string ProfileCode { get; set; }
        [DataMember]
        public int FunctionalCode { get; set; }
    }

    [DataContract]
    public class SMonthlyFee
    {
        [DataMember]
        public string AccountNo { get; set; }
        [DataMember]
        public decimal FeeRate { get; set; }
        [DataMember]
        public string ShipmentType { get; set; }
        [DataMember]
        public string AdminAccountNo { get; set; }
        [DataMember]
        public DateTime LastUpdate { get; set; }
    }

    [DataContract]
    public class SFranchiseContact
    {
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Language { get; set; }
    }

    //19JAN12 HN
    [DataContract]
    public class SUserID
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string AccountNo { get; set; }
    }

    //16FEB12SM
    [DataContract]
    public class SAdv
    {
        [DataMember]
        public string ACCOUNT_NO { get; set; }
        [DataMember]
        public string CARRIER_NAME { get; set; }
        [DataMember]
        public string PRIORITY { get; set; }
        [DataMember]
        public decimal AVERAGE_VALUE { get; set; }
        [DataMember]
        public decimal AVERAGE_WEIGHT { get; set; }
        [DataMember]
        public string PACKAGE_TYPE { get; set; }
        [DataMember]
        public string SHIPMENT_TYPE { get; set; }
        [DataMember]
        public decimal DISCOUNT { get; set; }
    }
}
