using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kaizos.Entities.Business
{

    public class BUser
    {
        public string       AccountNo { get; set; }
        public string       Email { get; set; }
        public string       Password { get; set; }
        public BEnumUserType UserType { get; set; }
        public BEnumUserStatus Status { get; set; }
        public BEnumFlag     IsSalesTarrifAssigned { get; set; }
        public BEnumFlag     IsToSAccepted {get;set;}
        public DateTime     ToSAcceptedDate { get; set; }
        public string       Language { get; set; }
        public DateTime     LastLogin { get; set; }
        public BEnumFlag     IsChangePasswordRequired { get; set; }
        public string       CreatedBy { get; set; }
        public BEnumUserType CreatedUserType { get; set; }
        public string       CompanyName { get; set; }
        public string       Name { get; set; }
        public string       TelephoneNo { get; set; }
        public string       Country { get; set; }
        public DateTime     CreatedDate { get; set; }
        public DateTime     LastUpdate { get; set; }
        public string       LegalForm { get; set; }
        public BEnumCustomerType CustomerType { get; set; }
        public BEnumFlag         CustomerTypeChanged { get; set; }
        public string VatNo { get; set; }
        public string SiretNo { get; set; }
    }

    public class BAdministrator : BUser
    {
        //fields to be freeze
    }

    public class BFranchise : BUser
    {
        public string   CommercialName { get; set; }
        public int      ManPower { get; set; }
        public string   FaxNo { get; set; }
        public string RegistrationNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string AssignedZone { get; set; }
        public string ScannedDoc { get; set; }  //to be deside to store either directly in table or in physical folder.
        public int PaymentDelay { get; set; }    //22FEB12SM
        public DateTime FirmCreationDate { get; set; } //22FEB12SM
        public string CarrierAccountRef { get; set; } //22FEB12SM
    }

    public class BCustomerService : BUser
    {
    }

    public class BCustomer : BUser
    {
        public string Designation { get; set; }
        public string HqZipcode { get; set; }
        public BEnumFlag IsKeyAccount { get; set; }
        public BEnumCustCategory CustomerCategory { get; set;}
        public string ZipContact { get; set; }
        public string IndustryType { get; set; }
        public string ChiefContact { get; set; }
        public string ContactName { get; set; }
        public string InvoicePhoneNumber { get; set; }
        public string InvoiceFaxNo { get; set; }
        public string InvoiceAddress1 { get; set; }
        public string InvoiceAddress2 { get; set; }
        public string InvoiceAddress3 { get; set; }
        public string InvoiceZipcode { get; set; }
        public string InvoiceCity { get; set; }
        public string InvoiceCountry { get; set; }
        public BEnumFlag UsedForShipping { get; set; }
        public BEnumFlag UsedForReturn { get; set; }
        public string ShipmentPreference { get; set; }
        public BEnumPaymentType PaymentMethod { get; set; }
        public BEnumFlag DeferedPaymentRequired { get; set; }
        public decimal BudgetAmount { get; set; }
        public BEnumFlag IsDeferredPaymentAgreed { get; set; }
        public decimal WishedBudgetAmount { get; set; }
        public string AdministratorUserId { get; set; }
        public decimal InsuredCreditAmount { get; set; }
        public int PaymentDelayDays { get; set; }
        public int PaymentDelayMonth { get; set; }
        public decimal CompensationRate { get; set; }
        public decimal CompensationAmount { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal AuthorizedCreditLimit { get; set; }
        public decimal AuthorizedCreditAmount { get; set; }
        public string CarrierAccountReference { get; set; }
        public decimal AvailableCreditLimit { get; set; }
        public decimal AvailableCredit { get; set; }
        public int ManPower { get; set; }
        public decimal GrossMargin { get; set; }
        public string DEFERED_PAYMENT_TYPE { get; set; }    //[16FEB12SM]
        public string KEY_CARRIER { get; set; }             //[16FEB12SM]
        public decimal SUBSCRIPTION_AMOUNT { get; set; }    //[16FEB12SM]
        public string EXTRA_INFO { get; set; }              //[16FEB12SM]
        public BEnumFlag FICTIVE_ACCOUNT { get; set; }      //[16FEB12SM]
        public decimal TurnOver { get; set; }      //[16FEB12SM]
        public int ADV { get; set; }      //[16FEB12SM]
        public string CarrierName { get; set; }      //[16FEB12SM]
        public string InsuredMethod { get; set; }      //[16FEB12SM] 
        public DateTime FirmDate { get; set; }  //21-FEB-2012 HV

    }

    public class BFunctionality
    {
        public int FunctionalCode { get; set; }
        public string FunctionalName { get; set; }
        public string Description { get; set; }
    }

    public class BProfile
    {
        public int ProfileCode { get; set; }
        public string ProfileName { get; set; }
        public string Description { get; set; }
        public BEnumUserType UserType { get; set; }
    }

    public class BFunctionalProfile
    {
        public string ProfileCode { get; set; }
        public int FunctionalCode { get; set; }
    }

    public class BMonthlyFee
    {
        public string AccountNo { get; set; }
        public decimal FeeRate { get; set; }
        public string ShipmentType { get; set; }
        public string AdminAccountNo { get; set; }
        public DateTime LastUpdate { get; set; }
    }

    public class BAuthorized :BUser
    {
        public string ContactName {get; set;}
        public string ReferentUserId { get; set; }
    }

    public class BFranchiseContact
    {
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
    }

    public class BUserID
    {
        public string UserName { get; set; }
        public string AccountNo { get; set; }
    }

    //16FEB12SM
    public class BAdv
    {
        public string ACCOUNT_NO { get; set; }
        public string CARRIER_NAME { get; set; }
        public string PRIORITY { get; set; }
        public decimal AVERAGE_VALUE { get; set; }
        public decimal AVERAGE_WEIGHT { get; set; }
        public string PACKAGE_TYPE { get; set; }
        public string SHIPMENT_TYPE { get; set; }
        public decimal DISCOUNT { get; set; }
    }
}


