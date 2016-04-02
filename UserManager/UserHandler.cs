using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Objects;

using KaizosEntities; //Data Entity
using Kaizos.Entities.Business; //Business Entity
using Kaizos.Components;  
using Kaizos.Components.GlobalLibrary;  //Libaray where all generic methods are available
using Kaizos.Components.UserManager;
using log4net;                          //Reference to Log4net component
using log4net.Config;
using System.Web;
using System.IO;


namespace Kaizos.Components.UserManager
{
  public class UserHandler
  {
    ILog logger = log4net.LogManager.GetLogger(typeof(UserHandler));

    /// <summary>
    /// Insert all types of users using the 2nd parameter user type
    /// </summary>
    /// <returns>0=&gt; successful insertion 1=&gt; insertion failured</returns>
    public int InsertUser(BUser user, BEnumUserType userType)
    {
        int result = 1;
        switch (userType)
        {
            case BEnumUserType.Administrator:
                {
                }
                break;

            case BEnumUserType.Franchise:
                {
                    
                }
                break;

            case BEnumUserType.CustomerService:
                break;

            case BEnumUserType.Referent:
                {
                }
                break;

            case BEnumUserType.Authorized:
                {

                }
                break;
        }
        return result;
    }     

    public int UpdateUser(BUser user, BEnumUserType userType)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Delete the user for the given account no
    /// </summary>
    /// <returns>0 =&gt; successful deletion 1=&gt; deletion failured</returns>
    public int DeleteUser(string accountNo)
    {
        throw new System.NotImplementedException();
    }


    /// <summary>
    /// Validate the given user name and password is  valid
    /// </summary>
    /// <returns>Return -1 if validation fails otherwise valid account no</returns>
    /// 
    public int ValidateUser(string userName, string password)
    {
        int result = 1;
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        System.Nullable<int> iReturnValue = context.uSP_LOGIN_VALIDATE(userName.Trim(), password.Trim()).SingleOrDefault();

        result = (int)iReturnValue;

        return result;
    }

    /// <summary>
    /// Get login information for the given loginID
    /// </summary>
    /// <returns>Return a result set or null value from the stored procedure uSP_GET_LOGIN
    /// 
    public BUser GetLogin(string Username)
    {
        BUser bUser = new BUser();

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        var userContext = context.uSP_GET_LOGIN(Username).SingleOrDefault();
        if (userContext != null)
        {
            bUser.AccountNo = userContext.ACCOUNT_NO.Trim();
            bUser.Email = userContext.EMAIL.Trim();
            bUser.Password = userContext.PWD.Trim();

            if (userContext.USER_TYPE.Trim() == "AD")
                bUser.UserType = BEnumUserType.Administrator;
            else if (userContext.USER_TYPE.Trim() == "FR")
                bUser.UserType = BEnumUserType.Franchise;
            else if (userContext.USER_TYPE.Trim() == "RF")
                bUser.UserType = BEnumUserType.Referent;
            else if (userContext.USER_TYPE.Trim() == "AZ")
                bUser.UserType = BEnumUserType.Authorized;
            else if (userContext.USER_TYPE.Trim() == "CS")
                bUser.UserType = BEnumUserType.CustomerService;
            else
                bUser.UserType = BEnumUserType.Administrator;

            if (userContext.STATUS.Trim() == "B")
                bUser.Status = BEnumUserStatus.BeingCreated;
            else if (userContext.STATUS.Trim() == "E")
                bUser.Status = BEnumUserStatus.Enabled;
            else if (userContext.STATUS.Trim() == "D")
                bUser.Status = BEnumUserStatus.Disabled;
            else if (userContext.STATUS.Trim() == "A")
                bUser.Status = BEnumUserStatus.Archived;
            else
                bUser.Status = BEnumUserStatus.Enabled;

            if (userContext.SALE_TARIFF_ASSIGNED.Trim() == "Y")
                bUser.IsSalesTarrifAssigned = BEnumFlag.Yes;
            else if (userContext.SALE_TARIFF_ASSIGNED.Trim() == "N")
                bUser.IsSalesTarrifAssigned = BEnumFlag.No;
            else
                bUser.IsSalesTarrifAssigned = BEnumFlag.No;

            if (userContext.TOS_ACCEPTED.Trim() == "Y")
                bUser.IsToSAccepted = BEnumFlag.Yes;
            else if (userContext.TOS_ACCEPTED.Trim() == "N")
                bUser.IsToSAccepted = BEnumFlag.No;
            else
                bUser.IsToSAccepted = BEnumFlag.No;

            bUser.ToSAcceptedDate = userContext.TOS_ACCEPTED_DATE;
            bUser.Language = userContext.LANGUAGE;
            bUser.LastLogin = userContext.LAST_LOGIN;

            if (userContext.CHANGE_PASSWORD_REQ.Trim() == "Y")
                bUser.IsChangePasswordRequired = BEnumFlag.Yes;
            else if (userContext.CHANGE_PASSWORD_REQ.Trim() == "N")
                bUser.IsChangePasswordRequired = BEnumFlag.No;
            else
                bUser.IsChangePasswordRequired = BEnumFlag.No;

            if (userContext.USER_TYPE.Trim() == "AD")
                bUser.CreatedUserType = BEnumUserType.Administrator;
            else if (userContext.USER_TYPE.Trim() == "FR")
                bUser.CreatedUserType = BEnumUserType.Franchise;
            else if (userContext.USER_TYPE.Trim() == "RF")
                bUser.CreatedUserType = BEnumUserType.Referent;
            else if (userContext.USER_TYPE.Trim() == "AZ")
                bUser.CreatedUserType = BEnumUserType.Authorized;
            else if (userContext.USER_TYPE.Trim() == "CS")
                bUser.CreatedUserType = BEnumUserType.CustomerService;
            else
                bUser.CreatedUserType = BEnumUserType.Administrator;

            bUser.CompanyName = userContext.COMPANY_NAME.Trim();
            bUser.Name = userContext.CONTACT_NAME.Trim();
            bUser.TelephoneNo = userContext.TEL_NO.Trim();
            bUser.Country = userContext.COUNTRY.Trim();
            bUser.CreatedDate = userContext.CREATED_DATE;
            bUser.LastUpdate = userContext.LAST_UPDATE;
            bUser.LegalForm = userContext.LEGAL_FORM.Trim();

            bUser.CreatedBy = userContext.CREATED_BY.Trim();

            if (userContext.CUSTOMER_TYPE.Trim() == "R")
                bUser.CustomerType = BEnumCustomerType.RegularCustomer;
            else if (userContext.CUSTOMER_TYPE.Trim() == "K")
                bUser.CustomerType = BEnumCustomerType.KeyCustomer;
            else
                bUser.CustomerType = BEnumCustomerType.RegularCustomer;

            if (userContext.CUSTOMER_TYPE_CHANGED.Trim() == "Y")
                bUser.CustomerTypeChanged = BEnumFlag.Yes;
            else if (userContext.CUSTOMER_TYPE_CHANGED.Trim() == "N")
                bUser.CustomerTypeChanged = BEnumFlag.No;
            else
                bUser.CustomerTypeChanged = BEnumFlag.No;
        }
        return bUser;
    }

    /// <summary>
    /// To Get login information for N2 user
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <returns>
    /// get the all the login related user information for the given user id for N2 user.
    /// </returns>
    public BFranchise GetFranchise(string UserName)
    {
        BFranchise bFranchise = new BFranchise();

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        var userContext = context.uSP_GET_FRANCHISE(UserName.Trim()).SingleOrDefault();
        if (userContext != null)
        {
            bFranchise.AccountNo = userContext.ACCOUNT_NO.Trim();
            bFranchise.Email = userContext.EMAIL.Trim();
            bFranchise.Password = userContext.PWD.Trim();

            if (userContext.USER_TYPE.Trim() == "AD")
                bFranchise.UserType = BEnumUserType.Administrator;
            else if (userContext.USER_TYPE.Trim() == "FR")
                bFranchise.UserType = BEnumUserType.Franchise;
            else if (userContext.USER_TYPE.Trim() == "RF")
                bFranchise.UserType = BEnumUserType.Referent;
            else if (userContext.USER_TYPE.Trim() == "AZ")
                bFranchise.UserType = BEnumUserType.Authorized;
            else if (userContext.USER_TYPE.Trim() == "CS")
                bFranchise.UserType = BEnumUserType.CustomerService;
            else
                bFranchise.UserType = BEnumUserType.Administrator;

            if (userContext.STATUS.Trim() == "B")
                bFranchise.Status = BEnumUserStatus.BeingCreated;
            else if (userContext.STATUS.Trim() == "E")
                bFranchise.Status = BEnumUserStatus.Enabled;
            else if (userContext.STATUS.Trim() == "D")
                bFranchise.Status = BEnumUserStatus.Disabled;
            else if (userContext.STATUS.Trim() == "A")
                bFranchise.Status = BEnumUserStatus.Archived;
            else
                bFranchise.Status = BEnumUserStatus.Enabled;

            if (userContext.SALE_TARIFF_ASSIGNED.Trim() == "Y")
                bFranchise.IsSalesTarrifAssigned = BEnumFlag.Yes;
            else if (userContext.SALE_TARIFF_ASSIGNED.Trim() == "N")
                bFranchise.IsSalesTarrifAssigned = BEnumFlag.No;
            else
                bFranchise.IsSalesTarrifAssigned = BEnumFlag.Yes;

            if (userContext.TOS_ACCEPTED.Trim() == "Y")
                bFranchise.IsToSAccepted = BEnumFlag.Yes;
            else if (userContext.TOS_ACCEPTED.Trim() == "N")
                bFranchise.IsToSAccepted = BEnumFlag.No;
            else
                bFranchise.IsToSAccepted = BEnumFlag.No;

            bFranchise.ToSAcceptedDate = userContext.TOS_ACCEPTED_DATE;
            bFranchise.Language = userContext.LANGUAGE.Trim();
            bFranchise.LastLogin = userContext.LAST_LOGIN;

            if (userContext.CHANGE_PASSWORD_REQ.Trim() == "Y")
                bFranchise.IsChangePasswordRequired = BEnumFlag.Yes;
            else if (userContext.CHANGE_PASSWORD_REQ.Trim() == "N")
                bFranchise.IsChangePasswordRequired = BEnumFlag.No;
            else
                bFranchise.IsChangePasswordRequired = BEnumFlag.Yes;

            bFranchise.CreatedBy = userContext.CREATED_BY.Trim();

            if (userContext.USER_TYPE.Trim() == "AD")
                bFranchise.CreatedUserType = BEnumUserType.Administrator;
            else if (userContext.USER_TYPE.Trim() == "FR")
                bFranchise.CreatedUserType = BEnumUserType.Franchise;
            else if (userContext.USER_TYPE.Trim() == "RF")
                bFranchise.CreatedUserType = BEnumUserType.Referent;
            else if (userContext.USER_TYPE.Trim() == "AZ")
                bFranchise.CreatedUserType = BEnumUserType.Authorized;
            else if (userContext.USER_TYPE.Trim() == "CS")
                bFranchise.CreatedUserType = BEnumUserType.CustomerService;
            else
                bFranchise.CreatedUserType = BEnumUserType.Administrator;

            bFranchise.CompanyName = userContext.COMPANY_NAME.Trim();
            bFranchise.Name = userContext.CONTACT_NAME.Trim();
            bFranchise.LegalForm = userContext.LEGAL_FORM.Trim();
            bFranchise.CommercialName = userContext.COMMERCIAL_NAME.Trim();
            bFranchise.ManPower = userContext.MAN_POWER;
            bFranchise.TelephoneNo = userContext.TEL_NO.Trim();
            bFranchise.FaxNo = userContext.FAX_NO.Trim();
            bFranchise.RegistrationNo = userContext.REGISTRATION_NO.Trim();
            bFranchise.Address1 = userContext.ADDRESS1.Trim();
            bFranchise.Address2 = userContext.ADDRESS2.Trim();
            bFranchise.Address3 = userContext.ADDRESS3.Trim();
            bFranchise.City = userContext.CITY.Trim();
            bFranchise.ZipCode = userContext.ZIPCODE.Trim();
            bFranchise.Country = userContext.COUNTRY.Trim();
            bFranchise.AssignedZone = userContext.ASSIGNED_ZONE.Trim();
            bFranchise.CreatedDate = (DateTime)userContext.CREATED_DATE;
            bFranchise.LastUpdate = (DateTime)userContext.LAST_UPDATE;

            if (userContext.CUSTOMER_TYPE.Trim() == "K")
                bFranchise.CustomerType = BEnumCustomerType.KeyCustomer;
            else if (userContext.CUSTOMER_TYPE.Trim() == "R")
                bFranchise.CustomerType = BEnumCustomerType.RegularCustomer;
            else
                bFranchise.CustomerType = BEnumCustomerType.KeyCustomer;

            if (userContext.CUSTOMER_TYPE_CHANGED.Trim() == "Y")
                bFranchise.CustomerTypeChanged = BEnumFlag.Yes;
            else if (userContext.CUSTOMER_TYPE_CHANGED.Trim() == "N")
                bFranchise.CustomerTypeChanged = BEnumFlag.No;
            else
                bFranchise.CustomerTypeChanged = BEnumFlag.Yes;

            bFranchise.ScannedDoc = userContext.SCANNED_DOCUMENT.Trim();
        }
        return bFranchise;
    }

    /// <summary>
    /// To Get login information for Referent user
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <returns>
    /// get the all the login related user information for the given user id for Referent user.
    /// </returns>
    public BCustomer GetCustomer(string UserName)
    {

        BCustomer bCustomer = new BCustomer();

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        var userContext = context.uSP_GET_CUSTOMER(UserName.Trim()).SingleOrDefault();
        if (userContext != null)
        {
            bCustomer.AccountNo = userContext.ACCOUNT_NO.Trim();
            bCustomer.Email = userContext.EMAIL.Trim();
            bCustomer.Password = userContext.PWD.Trim();

            if (userContext.USER_TYPE.Trim() == "AD")
                bCustomer.UserType = BEnumUserType.Administrator;
            else if (userContext.USER_TYPE.Trim() == "FR")
                bCustomer.UserType = BEnumUserType.Franchise;
            else if (userContext.USER_TYPE.Trim() == "RF")
                bCustomer.UserType = BEnumUserType.Referent;
            else if (userContext.USER_TYPE.Trim() == "AZ")
                bCustomer.UserType = BEnumUserType.Authorized;
            else if (userContext.USER_TYPE.Trim() == "CS")
                bCustomer.UserType = BEnumUserType.CustomerService;
            else
                bCustomer.UserType = BEnumUserType.Administrator;

            if (userContext.STATUS.Trim() == "B")
                bCustomer.Status = BEnumUserStatus.BeingCreated;
            else if (userContext.STATUS.Trim() == "E")
                bCustomer.Status = BEnumUserStatus.Enabled;
            else if (userContext.STATUS.Trim() == "D")
                bCustomer.Status = BEnumUserStatus.Disabled;
            else if (userContext.STATUS.Trim() == "A")
                bCustomer.Status = BEnumUserStatus.Archived;
            else
                bCustomer.Status = BEnumUserStatus.Enabled;

            if (userContext.SALE_TARIFF_ASSIGNED.Trim() == "Y")
                bCustomer.IsSalesTarrifAssigned = BEnumFlag.Yes;
            else if (userContext.SALE_TARIFF_ASSIGNED.Trim() == "N")
                bCustomer.IsSalesTarrifAssigned = BEnumFlag.No;
            else
                bCustomer.IsSalesTarrifAssigned = BEnumFlag.Yes;

            if (userContext.TOS_ACCEPTED.Trim() == "Y")
                bCustomer.IsToSAccepted = BEnumFlag.Yes;
            else if (userContext.TOS_ACCEPTED.Trim() == "N")
                bCustomer.IsToSAccepted = BEnumFlag.No;
            else
                bCustomer.IsToSAccepted = BEnumFlag.Yes;

            bCustomer.ToSAcceptedDate = userContext.TOS_ACCEPTED_DATE;
            bCustomer.Language = userContext.LANGUAGE.Trim();
            bCustomer.LastLogin = userContext.LAST_LOGIN;

            if (userContext.CHANGE_PASSWORD_REQ.Trim() == "Y")
                bCustomer.IsChangePasswordRequired = BEnumFlag.Yes;
            else if (userContext.CHANGE_PASSWORD_REQ.Trim() == "N")
                bCustomer.IsChangePasswordRequired = BEnumFlag.No;
            else
                bCustomer.IsChangePasswordRequired = BEnumFlag.Yes;

            bCustomer.CreatedBy = userContext.CREATED_BY.Trim();

            if (userContext.USER_TYPE.Trim() == "AD")
                bCustomer.CreatedUserType = BEnumUserType.Administrator;
            else if (userContext.USER_TYPE.Trim() == "FR")
                bCustomer.CreatedUserType = BEnumUserType.Franchise;
            else if (userContext.USER_TYPE.Trim() == "RF")
                bCustomer.CreatedUserType = BEnumUserType.Referent;
            else if (userContext.USER_TYPE.Trim() == "AZ")
                bCustomer.CreatedUserType = BEnumUserType.Authorized;
            else if (userContext.USER_TYPE.Trim() == "CS")
                bCustomer.CreatedUserType = BEnumUserType.CustomerService;
            else
                bCustomer.CreatedUserType = BEnumUserType.Administrator;

            bCustomer.CompanyName = userContext.COMPANY_NAME.Trim();
            bCustomer.Name = userContext.CUST_NAME.Trim();
            bCustomer.Designation = userContext.DESIGNATION.Trim();
            bCustomer.TelephoneNo = userContext.TEL_NO.Trim();
            bCustomer.HqZipcode = userContext.HQ_ZIP_CODE.Trim();
            bCustomer.Country = userContext.COUNTRY.Trim();

            if (userContext.KEY_ACCOUNT.Trim() == "Y")
                bCustomer.IsKeyAccount = BEnumFlag.Yes;
            else if (userContext.KEY_ACCOUNT.Trim() == "N")
                bCustomer.IsKeyAccount = BEnumFlag.No;
            else
                bCustomer.IsKeyAccount = BEnumFlag.Yes;

            if (userContext.CUST_CATEGORY.Trim() == "A")
                bCustomer.CustomerCategory = BEnumCustCategory.A;
            else if (userContext.CUST_CATEGORY.Trim() == "B")
                bCustomer.CustomerCategory = BEnumCustCategory.B;
            else if (userContext.CUST_CATEGORY.Trim() == "C")
                bCustomer.CustomerCategory = BEnumCustCategory.C;
            else
                bCustomer.CustomerCategory = BEnumCustCategory.A;

            bCustomer.ChiefContact = userContext.CHIEF_CONTACT.Trim();
            bCustomer.IndustryType = userContext.INDUSTRY_TYPE.Trim();
            bCustomer.LegalForm = userContext.LEGAL_FORM.Trim();
            bCustomer.ContactName = userContext.CONTACT_NAME.Trim();
            bCustomer.InvoicePhoneNumber = userContext.INV_PHONE_NO.Trim();
            bCustomer.InvoiceFaxNo = userContext.INV_FAX_NO.Trim();
            bCustomer.InvoiceAddress1 = userContext.INV_ADDRESS1.Trim();
            bCustomer.InvoiceAddress2 = userContext.INV_ADDRESS2.Trim();
            bCustomer.InvoiceAddress3 = userContext.INV_ADDRESS3.Trim();
            bCustomer.InvoiceZipcode = userContext.INV_ZIP_CODE.Trim();
            bCustomer.InvoiceCity = userContext.INV_CITY.Trim();
            bCustomer.InvoiceCountry = userContext.INV_COUNTRY.Trim();

            if (userContext.USE_FOR_SHIPPING.Trim() == "Y")
                bCustomer.UsedForShipping = BEnumFlag.Yes;
            else if (userContext.USE_FOR_SHIPPING.Trim() == "N")
                bCustomer.UsedForShipping = BEnumFlag.No;
            else
                bCustomer.UsedForShipping = BEnumFlag.Yes;

            if (userContext.USE_FOR_RETURN.Trim() == "Y")
                bCustomer.UsedForReturn = BEnumFlag.Yes;
            else if (userContext.USE_FOR_RETURN.Trim() == "N")
                bCustomer.UsedForReturn = BEnumFlag.No;
            else
                bCustomer.UsedForReturn = BEnumFlag.Yes;

            //if (userContext.SHIP_PREFERENCE.Trim() == "1")
            //    bCustomer.ShipmentPreference = BEnumShipPreference.MostCompetitive;
            //else if (userContext.SHIP_PREFERENCE.Trim() == "2")
            //    bCustomer.ShipmentPreference = BEnumShipPreference.Fastest;
            //else if (userContext.SHIP_PREFERENCE.Trim() == "3")
            //    bCustomer.ShipmentPreference = BEnumShipPreference.NamedCarrier;
            //else
            //    bCustomer.ShipmentPreference = BEnumShipPreference.MostCompetitive;
            
            if (userContext.PAYMENT_METHOD.Trim() == "CC")
                bCustomer.PaymentMethod = BEnumPaymentType.CreditCard;
            else if (userContext.PAYMENT_METHOD.Trim() == "DP")
                bCustomer.PaymentMethod = BEnumPaymentType.DeferredPayment;
            else
                bCustomer.PaymentMethod = BEnumPaymentType.DeferredPayment;

            if (userContext.DEFERRED_PAY_REQ.Trim() == "Y")
                bCustomer.DeferedPaymentRequired = BEnumFlag.Yes;
            else if (userContext.DEFERRED_PAY_REQ.Trim() == "N")
                bCustomer.DeferedPaymentRequired = BEnumFlag.No;
            else
                bCustomer.DeferedPaymentRequired = BEnumFlag.Yes;

            bCustomer.BudgetAmount = userContext.TRANS_BUDGET_AMT;
            bCustomer.WishedBudgetAmount = userContext.WISHED_BUDGET_AMT;

            if (userContext.DEFERRED_PAY_AGREED.Trim() == "Y")
                bCustomer.IsDeferredPaymentAgreed = BEnumFlag.Yes;
            else if (userContext.DEFERRED_PAY_AGREED.Trim() == "N")
                bCustomer.IsDeferredPaymentAgreed = BEnumFlag.No;
            else
                bCustomer.IsDeferredPaymentAgreed = BEnumFlag.Yes;

            bCustomer.AdministratorUserId = userContext.ADMIN_USER_ID.Trim();

            if (userContext.CUSTOMER_TYPE.Trim() == "R")
                bCustomer.CustomerType = BEnumCustomerType.RegularCustomer;
            else if (userContext.CUSTOMER_TYPE.Trim() == "K")
                bCustomer.CustomerType = BEnumCustomerType.KeyCustomer;
            else
                bCustomer.CustomerType = BEnumCustomerType.RegularCustomer;

            if (userContext.CUSTOMER_TYPE_CHANGED.Trim() == "Y")
                bCustomer.CustomerTypeChanged = BEnumFlag.Yes;
            else if (userContext.CUSTOMER_TYPE_CHANGED.Trim() == "N")
                bCustomer.CustomerTypeChanged = BEnumFlag.No;
            else
                bCustomer.CustomerTypeChanged = BEnumFlag.Yes;

            bCustomer.InsuredCreditAmount = userContext.INSURED_CREDIT_AMT;
            bCustomer.PaymentDelayDays = userContext.PAYMENT_DELAY_DAYS;
            bCustomer.PaymentDelayMonth = userContext.PAYMENT_DELAY_MONTH;
            bCustomer.CompensationRate = userContext.COMPENSATION_RATE;
            bCustomer.CompensationAmount = userContext.COMPENSATION_AMT;
            bCustomer.DepositAmount = userContext.DEPOSIT_AMT;
            bCustomer.AuthorizedCreditLimit = userContext.AUTHORIZED_CREDIT_LIMIT;
            bCustomer.AuthorizedCreditAmount = userContext.AUTHORIZED_CREDIT_AMT;
            bCustomer.CarrierAccountReference = userContext.CARRIER_ACCOUNT_REF.Trim();
            bCustomer.AvailableCredit = userContext.AVAILABLE_CREDIT;
            bCustomer.CreatedDate = (DateTime)userContext.CREATED_DATE;
            bCustomer.LastUpdate = (DateTime)userContext.LAST_UPDATE;
        }
        return bCustomer;
    }

    /// <summary>
    /// Update Last login
    /// </summary>
    /// <returns>
    /// Return 0 = success
    /// Return 1 = Failure
    /// Return 2 = Already exists
    /// </returns>
    /// 

    public int UpdateLastLogin(string UserName, DateTime CurrentDateTime)
    {

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        var result = context.uSP_UPDATE_LAST_LOGIN(UserName, CurrentDateTime).SingleOrDefault();
        return (int)result;
    }

    /// <summary>
    /// For generating password
    /// </summary>
    /// <returns>Generated password</returns>
    public string GeneratePassword(string UniqueID)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Encrypting the given value
    /// </summary>
    /// <returns>Encrpytpted password</returns>
    public string EncryptPassword(string Password)
    {
        throw new System.NotImplementedException();
    }


    /// <summary>
    /// To get the functional list
    /// </summary>
    /// <returns>Result set </returns>
    public List<BFunctionality> GetFunctionality()
    {

        List<BFunctionality> bFunctionalities = new List<BFunctionality>();

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        var userContexts = context.uSP_GET_FUNCTIONALITIES().ToList();

        foreach (var userContext in userContexts)
        {
            BFunctionality bFunctionality = new BFunctionality();
            bFunctionality.FunctionalCode = userContext.FUNCTIONAL_CODE;
            bFunctionality.FunctionalName = userContext.FUNCTIONAL_NAME.Trim();
            bFunctionality.Description = userContext.DESCRIPTION.Trim();
            bFunctionalities.Add(bFunctionality);
        }
        return bFunctionalities;
    }

    /// <summary>
    /// To get the functional-profile mapping list
    /// </summary>
    /// <returns>Result set </returns>
    public List<BFunctionalProfile> GetFunctionalProfile()
    {

        List<BFunctionalProfile> bFunctionalProfiles = new List<BFunctionalProfile>();

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        var userContexts = context.uSP_GET_PROFILE_FUNCTIONALITY_MAPPING().ToList();

        foreach (var userContext in userContexts)
        {
            BFunctionalProfile bFunctionalProfile = new BFunctionalProfile();
            bFunctionalProfile.ProfileCode = userContext.PROFILE_CODE.Trim();
            bFunctionalProfile.FunctionalCode = userContext.FUNCTIONAL_CODE;
            bFunctionalProfiles.Add(bFunctionalProfile);
        }
        return bFunctionalProfiles;

    }

    /// <summary>
    /// To insert functional-profile mapping list
    /// </summary>
    /// <returns>Integer </returns>
    public int InsertFunctionalProfile(List<BFunctionalProfile> lsFunctionalProfile)
    {
        int strFunctinoalCode;
        string strProfilecode = "";
        int result = 1;
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        for (int i = 0; i < lsFunctionalProfile.Count; i++)
        {
            strFunctinoalCode = lsFunctionalProfile[i].FunctionalCode;
            strProfilecode = lsFunctionalProfile[i].ProfileCode;
            result = (int)context.uSP_INSERT_PROFILE_FUNCTIONALITY_MAPPING(strProfilecode.Trim(), strFunctinoalCode).SingleOrDefault();
        }

        return result;
    }

    /// <summary>
    /// To delete all functional-profile mapping list
    /// </summary>
    /// <returns>Integer </returns>
    public int DeleteFunctionalProfile()
    {
        int result = 1;
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        System.Nullable<int> iReturnValue = context.uSP_DELETE_PROFILE_FUNCTIONALITY_MAPPING().SingleOrDefault();
        result = (int)iReturnValue;
        return result;
    }

    //2FEB12SM added three new fields 
    /// <summary>
    /// To insert login - Franchise entries
    /// </summary>
    /// <returns>Integer </returns>
    public int InsertFranchise(BFranchise bFranchise)
    {
        string UserType = "", Status = "", SaleTariffAssigned = "", TosAccepted = "";
        string ChangePasswordRequired = "", CreatedUserType = "", CustomerType = "";
        string CustomerTypeChanged = "";

        if (bFranchise.UserType == BEnumUserType.Administrator)
            UserType = "AD";
        else if (bFranchise.UserType == BEnumUserType.Authorized)
            UserType = "AZ";
        else if (bFranchise.UserType == BEnumUserType.CustomerService)
            UserType = "CS";
        else if (bFranchise.UserType == BEnumUserType.Franchise)
            UserType = "FR";
        else if (bFranchise.UserType == BEnumUserType.Referent)
            UserType = "RF";
        else
            UserType = "AD";

        if (bFranchise.Status == BEnumUserStatus.Enabled)
            Status = "E";
        else if (bFranchise.Status == BEnumUserStatus.Archived)
            Status = "A";
        else if (bFranchise.Status == BEnumUserStatus.BeingCreated)
            Status = "B";
        else if (bFranchise.Status == BEnumUserStatus.Disabled)
            Status = "D";

        if (bFranchise.IsSalesTarrifAssigned == BEnumFlag.Yes)
            SaleTariffAssigned = "Y";
        else
            SaleTariffAssigned = "N";

        if (bFranchise.IsToSAccepted == BEnumFlag.Yes)
            TosAccepted = "Y";
        else
            TosAccepted = "N";
        if (bFranchise.IsChangePasswordRequired == BEnumFlag.Yes)
            ChangePasswordRequired = "Y";
        else
            ChangePasswordRequired = "N";

        if (bFranchise.CreatedUserType == BEnumUserType.Administrator)
            CreatedUserType = "AD";
        else if (bFranchise.CreatedUserType == BEnumUserType.Authorized)
            CreatedUserType = "AZ";
        else if (bFranchise.CreatedUserType == BEnumUserType.CustomerService)
            CreatedUserType = "CS";
        else if (bFranchise.CreatedUserType == BEnumUserType.Franchise)
            CreatedUserType = "FR";
        else if (bFranchise.CreatedUserType == BEnumUserType.Referent)
            CreatedUserType = "RF";
        else
            CreatedUserType = "AD";

        if (bFranchise.CustomerType == BEnumCustomerType.KeyCustomer)
            CustomerType = "K";
        else
            CustomerType = "R";

        if (bFranchise.CustomerTypeChanged == BEnumFlag.Yes)
            CustomerTypeChanged = "Y";
        else
            CustomerTypeChanged = "N";

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        System.Nullable<int> iReturnValue = context.uSP_FRANCHIS_INSERT(bFranchise.AccountNo, bFranchise.Email, bFranchise.Password, UserType,
                                                                        Status, SaleTariffAssigned, TosAccepted, bFranchise.ToSAcceptedDate,
                                                                        bFranchise.Language, bFranchise.LastLogin, ChangePasswordRequired,
                                                                        bFranchise.CreatedBy, CreatedUserType, bFranchise.CompanyName,
                                                                        bFranchise.Name, bFranchise.LegalForm, bFranchise.CommercialName, bFranchise.ManPower,
                                                                        bFranchise.TelephoneNo, bFranchise.FaxNo, bFranchise.RegistrationNo,
                                                                        bFranchise.Address1, bFranchise.Address2, bFranchise.Address3,
                                                                        bFranchise.City, bFranchise.ZipCode, bFranchise.Country, bFranchise.AssignedZone,
                                                                        bFranchise.CreatedDate, bFranchise.LastUpdate, CustomerType,
                                                                        CustomerTypeChanged, bFranchise.ScannedDoc, bFranchise.SiretNo, bFranchise.VatNo, 
                                                                        bFranchise.PaymentDelay, bFranchise.FirmCreationDate, 
                                                                        bFranchise.CarrierAccountRef.Trim()).SingleOrDefault();

        return ((int)iReturnValue);
    }

    /// <summary>
    /// To Update a Login - Franchise Entry
    /// </summary>
    /// <returns>Integer </returns> 
    public int UpdateFranchise(BFranchise bFranchise)
    {
        string CustomerType = "", CustomerTypeChanged = "";

        if (bFranchise.CustomerType == BEnumCustomerType.KeyCustomer)
            CustomerType = "K";
        else
            CustomerType = "R";

        if (bFranchise.CustomerTypeChanged == BEnumFlag.Yes)
            CustomerTypeChanged = "Y";
        else
            CustomerTypeChanged = "N";

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        System.Nullable<int> iReturnValue = context.uSP_FRANCHIS_UPDATE(bFranchise.Email, bFranchise.Password,bFranchise.CompanyName,
                                                                        bFranchise.Name, bFranchise.LegalForm, bFranchise.CommercialName, bFranchise.ManPower,
                                                                        bFranchise.TelephoneNo, bFranchise.FaxNo, bFranchise.RegistrationNo, bFranchise.City, 
                                                                        bFranchise.Address1.Trim(), bFranchise.Address2.Trim(), bFranchise.Address3.Trim(),
                                                                        bFranchise.ZipCode, bFranchise.Country, bFranchise.AssignedZone, bFranchise.LastUpdate, 
                                                                        CustomerType, CustomerTypeChanged, bFranchise.ScannedDoc, bFranchise.Language.Trim()).SingleOrDefault();
        return ((int)iReturnValue);
    
    }

      /// <summary>
    /// To insert Monthly fees for a particular franchise user
    /// </summary>
    /// <returns>Integer </returns>
    public int InsertMonthlyFee(BMonthlyFee bMonthlyFee)
    {
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        System.Nullable<int> iReturnValue = context.uSP_MONTHLY_FEES_INSERT(bMonthlyFee.AccountNo, (decimal)bMonthlyFee.FeeRate, bMonthlyFee.ShipmentType,
                                                                            bMonthlyFee.AdminAccountNo, bMonthlyFee.LastUpdate).SingleOrDefault();
        return ((int)iReturnValue);
    }

      /// <summary>
    /// To get Franchise user list
    /// </summary>
    /// <returns>company name, contact name, emailID for N2 user </returns>
    public List<BFranchiseContact> GetFranchiseList()
    {
        List<BFranchiseContact> bFranchiseList = new List<BFranchiseContact>();
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        var userContext = context.uSP_GET_FRANCHISE_LIST().ToList();

        foreach (var FranchiseList in userContext)
        {
            BFranchiseContact bFranchise = new BFranchiseContact();
            bFranchise.CompanyName = FranchiseList.COMPANY_NAME.Trim();
            bFranchise.Email = FranchiseList.EMAIL.Trim();
            bFranchise.Name = FranchiseList.CONTACT_NAME.Trim();
            bFranchise.Language = FranchiseList.LANGUAGE.Trim();
            bFranchiseList.Add(bFranchise);
        }
        return bFranchiseList;
    }

    /// <summary>
    /// To get Monthly fees list for a account
    /// </summary>
    /// <returns>set of record for all shipment type for a account </returns>
    public List<BMonthlyFee> GetMonthlyFees(string UserId)
    {
        List<BMonthlyFee> bMonthlyFeeList = new List<BMonthlyFee>();

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        var userContext = context.uSP_GET_MONTHLY_FEES(UserId.Trim()).ToList();

        if (userContext != null)
        {
            foreach (var userContextList in userContext)
            {
                BMonthlyFee bMonthlyFee = new BMonthlyFee();

                bMonthlyFee.AccountNo = userContextList.ACCOUNT_NO;
                bMonthlyFee.AdminAccountNo = userContextList.ADMIN_ACCOUNT_NO;
                bMonthlyFee.FeeRate = userContextList.FEE_RATE;
                bMonthlyFee.ShipmentType = userContextList.SHIPMENT_TYPE;
                bMonthlyFee.LastUpdate = (DateTime)userContextList.LAST_UPDATE;
                bMonthlyFeeList.Add(bMonthlyFee);
            }
        }
        return bMonthlyFeeList;

    }

    /// <summary>
    /// To Update confirmed password for the given account number
    /// </summary>
    /// <returns>
    /// 0- Success
    /// 1- Failure
    /// 2- Account number Not exits
    /// </returns>
    public int ConfirmPassword(string AccountNo, string Password)
    {
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        System.Nullable<int> iReturnValue = context.uSP_UPDATE_PASSWORD(AccountNo.Trim(), Password.Trim()).SingleOrDefault();
        return ((int)iReturnValue);

    }

    /// <summary>
    /// To get Language resource file name 
    /// </summary>
    /// <param name="strCountryCode">User Country code</param>
    /// <param name="strLanguageCode">User prefered Language</param>
    /// <returns>Resource file name or Default resource file name</returns>
    public string GetLanguageResource(string strCountryCode, string strLanguageCode)
    {
        string result = "";

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        ObjectParameter objParamResourceFileName = new ObjectParameter("RESOURCE_FILE_NAME", typeof(String));
        System.Nullable<int> iReturnValue1 = context.uSP_GET_LANGUAGE_CODE(strCountryCode, strLanguageCode, objParamResourceFileName).SingleOrDefault();
        result = objParamResourceFileName.Value.ToString();

        return result;

    }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="AccountNo"></param>
      /// <param name="Pwd"></param>
      /// <param name="LanguageCode"></param>
      /// <returns>
      /// 0 - Success
      /// 1 - Failure
      /// 2 - Already Exists
      /// </returns>
    public int CustomerServiceUpdate(string AccountNo, string Pwd, string LanguageCode)
    {
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        System.Nullable<int> iReturnValue = context.uSP_CUSTOMER_SERVICE_UPDATE(AccountNo.Trim(), Pwd.Trim(), LanguageCode.Trim(), DateTime.Now).SingleOrDefault();
        return ((int)iReturnValue);
    }

    /// <summary>
    /// To get Customer Service user list
    /// </summary>
    /// <returns>company name, contact name, emailID for N2 user </returns>
    public List<BFranchiseContact> GetCustomerServiceList()
    {
        List<BFranchiseContact> bFranchiseList = new List<BFranchiseContact>();
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        var userContext = context.uSP_GET_CUSTOMER_SERVICE_LIST().ToList();

        foreach (var FranchiseList in userContext)
        {
            BFranchiseContact bFranchise = new BFranchiseContact();
            bFranchise.CompanyName = FranchiseList.COMPANY_NAME.Trim();
            bFranchise.Email = FranchiseList.EMAIL.Trim();
            bFranchise.Name = FranchiseList.CONTACT_NAME.Trim();
            bFranchise.Language = FranchiseList.LANGUAGE.Trim();
            bFranchiseList.Add(bFranchise);
        }
        return bFranchiseList;
    }

      /// <summary>
      /// To update Customer Service details by Admin User
      /// </summary>
      /// <param name="UserID"></param>
      /// <param name="Status"></param>
      /// <returns>
      /// 0 - Success
      /// 1 - Failure
      /// </returns>
    public int CustomerServiceUpdateAdmin(string UserID, string Status)
    {
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        System.Nullable<int> iReturnValue = context.uSP_CUSTOMER_SERVICE_UPDATE_ADMIN(UserID.Trim(), Status.Trim(),DateTime.Now).SingleOrDefault();
        return ((int)iReturnValue);
    }

      /// <summary>
      /// To insert Customer creation basic details
      /// </summary>
      /// <param name="bCustomer"></param>
      /// <returns></returns>
    public int InsertCustomer(BCustomer bCustomer)
    {
        string UserType = "", Status = "", SaleTariffAssigned = "", TosAccepted = "";
        string ChangePasswordRequired = "", CreatedUserType = "";
        string CustomerCategory = "", KeyAccount = "";

        if (bCustomer.UserType == BEnumUserType.Administrator)
            UserType = "AD";
        else if (bCustomer.UserType == BEnumUserType.Authorized)
            UserType = "AZ";
        else if (bCustomer.UserType == BEnumUserType.CustomerService)
            UserType = "CS";
        else if (bCustomer.UserType == BEnumUserType.Franchise)
            UserType = "FR";
        else if (bCustomer.UserType == BEnumUserType.Referent)
            UserType = "RF";
        else
            UserType = "AD";

        if (bCustomer.Status == BEnumUserStatus.Enabled)
            Status = "E";
        else if (bCustomer.Status == BEnumUserStatus.Archived)
            Status = "A";
        else if (bCustomer.Status == BEnumUserStatus.BeingCreated)
            Status = "B";
        else if (bCustomer.Status == BEnumUserStatus.Disabled)
            Status = "D";
        else
            Status = "B";

        if (bCustomer.IsSalesTarrifAssigned == BEnumFlag.Yes)
            SaleTariffAssigned = "Y";
        else
            SaleTariffAssigned = "N";

        if (bCustomer.IsToSAccepted == BEnumFlag.Yes)
            TosAccepted = "Y";
        else
            TosAccepted = "N";
        if (bCustomer.IsChangePasswordRequired == BEnumFlag.Yes)
            ChangePasswordRequired = "Y";
        else
            ChangePasswordRequired = "N";


        if (bCustomer.CreatedUserType == BEnumUserType.Administrator)
            CreatedUserType = "AD";
        else if (bCustomer.CreatedUserType == BEnumUserType.Authorized)
            CreatedUserType = "AZ";
        else if (bCustomer.CreatedUserType == BEnumUserType.CustomerService)
            CreatedUserType = "CS";
        else if (bCustomer.CreatedUserType == BEnumUserType.Franchise)
            CreatedUserType = "FR";
        else if (bCustomer.CreatedUserType == BEnumUserType.Referent)
            CreatedUserType = "RF";
        else
            CreatedUserType = "AD";

        if (bCustomer.IsKeyAccount == BEnumFlag.Yes)
            KeyAccount = "Y";
        else
            KeyAccount = "N";

        if (bCustomer.IsKeyAccount == BEnumFlag.Yes)
            KeyAccount = "Y";
        else
            KeyAccount = "N";

        if (bCustomer.CustomerCategory == BEnumCustCategory.A)
            CustomerCategory = "A";
        else if (bCustomer.CustomerCategory == BEnumCustCategory.B)
            CustomerCategory = "B";
        else if (bCustomer.CustomerCategory == BEnumCustCategory.C)
            CustomerCategory = "C";
        else
            CustomerCategory = "";

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        //Commented Hari'v existing sp
        //System.Nullable<int> iReturnValue = context.uSP_CUSTOMER_INSERT(bCustomer.AccountNo, bCustomer.Email, bCustomer.Password, UserType,
        //                                                             Status, SaleTariffAssigned, TosAccepted, bCustomer.ToSAcceptedDate,
        //                                                             bCustomer.Language, bCustomer.LastLogin, ChangePasswordRequired,
        //                                                             bCustomer.CreatedBy, CreatedUserType, bCustomer.CompanyName, bCustomer.Name,
        //                                                             bCustomer.TelephoneNo, bCustomer.HqZipcode, bCustomer.Country, KeyAccount,
        //                                                             CustomerCategory, bCustomer.ChiefContact, bCustomer.IndustryType, bCustomer.LegalForm,
        //                                                              bCustomer.AdministratorUserId,bCustomer.CreatedDate, bCustomer.LastUpdate, bCustomer.ManPower).SingleOrDefault();

        // after refreshed new SP in Entit project updated the given hari v new Sp call with additional 2 param 24FEB12RM
        System.Nullable<int> iReturnValue = context.uSP_CUSTOMER_INSERT(bCustomer.AccountNo, bCustomer.Email, bCustomer.Password, UserType,
                                                                     Status, SaleTariffAssigned, TosAccepted, bCustomer.ToSAcceptedDate,
                                                                     bCustomer.Language, bCustomer.LastLogin, ChangePasswordRequired,
                                                                     bCustomer.CreatedBy, CreatedUserType, bCustomer.CompanyName, bCustomer.Name,
                                                                     bCustomer.TelephoneNo, bCustomer.HqZipcode, bCustomer.Country, KeyAccount,
                                                                     CustomerCategory, bCustomer.ChiefContact, bCustomer.IndustryType, bCustomer.LegalForm,
                                                                     bCustomer.AdministratorUserId, bCustomer.CreatedDate, bCustomer.LastUpdate, bCustomer.ManPower, bCustomer.FirmDate, bCustomer.KEY_CARRIER).SingleOrDefault();
        
        return (int)iReturnValue;
    }

      /// <summary>
      /// To insert the end customer details
      /// </summary>
      /// <param name="bCustomer"></param>
      /// <returns>
      /// 0 - Success
      /// 1 - Failure
      /// 2 - User Already Exists
      /// </returns>
    public int InsertEndCustomer(BCustomer bCustomer)
    {
        int result = 1;
        #region Insert process for Customer table
        
        string UserType = "", Status = "", SaleTariffAssigned = "", TosAccepted = "";
        string ChangePasswordRequired = "", CreatedUserType = "";
        string CustomerCategory = "", KeyAccount = "";
        string UsedForShipping = "", UsedForReturn = "", PaymentMethod = "";
        string DeferedPaymentRequired = "", DeferredPaymentAgreed = "";
        string CustomerType = "", CustomerTypeChanged = "", ShipmentPreference = "";

        if (bCustomer.UserType == BEnumUserType.Administrator)
            UserType = "AD";
        else if (bCustomer.UserType == BEnumUserType.Franchise)
            UserType = "FR";
        else if (bCustomer.UserType == BEnumUserType.Authorized)
            UserType = "AZ";
        else if (bCustomer.UserType == BEnumUserType.CustomerService)
            UserType = "CS";
        else if (bCustomer.UserType == BEnumUserType.Referent)
            UserType = "RF";
        else
            UserType = "AD";

        if (bCustomer.Status == BEnumUserStatus.Enabled)
            Status = "E";
        else if (bCustomer.Status == BEnumUserStatus.Archived)
            Status = "A";
        else if (bCustomer.Status == BEnumUserStatus.BeingCreated)
            Status = "B";
        else if (bCustomer.Status == BEnumUserStatus.Disabled)
            Status = "D";

        if (bCustomer.IsSalesTarrifAssigned == BEnumFlag.Yes)
            SaleTariffAssigned = "Y";
        else
            SaleTariffAssigned = "N";

        if (bCustomer.IsToSAccepted == BEnumFlag.Yes)
            TosAccepted = "Y";
        else
            TosAccepted = "N";

        if (bCustomer.IsChangePasswordRequired == BEnumFlag.Yes)
            ChangePasswordRequired = "Y";
        else
            ChangePasswordRequired = "N";

        if (bCustomer.CreatedUserType == BEnumUserType.Administrator)
            CreatedUserType = "AD";
        else if (bCustomer.CreatedUserType == BEnumUserType.Franchise)
            CreatedUserType = "FR";
        else if (bCustomer.CreatedUserType == BEnumUserType.Authorized)
            CreatedUserType = "AZ";
        else if (bCustomer.CreatedUserType == BEnumUserType.CustomerService)
            CreatedUserType = "CS";
        else if (bCustomer.CreatedUserType == BEnumUserType.Referent)
            CreatedUserType = "RF";
        else
            CreatedUserType = "AD";

        if (bCustomer.IsKeyAccount == BEnumFlag.Yes)
            KeyAccount = "Y";
        else
            KeyAccount = "N";

        if (bCustomer.IsKeyAccount == BEnumFlag.Yes)
            KeyAccount = "Y";
        else
            KeyAccount = "N";

        if (bCustomer.CustomerCategory == BEnumCustCategory.A)
            CustomerCategory = "A";
        else if (bCustomer.CustomerCategory == BEnumCustCategory.B)
            CustomerCategory = "B";
        else if (bCustomer.CustomerCategory == BEnumCustCategory.C)
            CustomerCategory = "C";
        
        if (bCustomer.UsedForShipping == BEnumFlag.Yes)
            UsedForShipping = "Y";
        else
            UsedForShipping = "N";

        if (bCustomer.UsedForReturn == BEnumFlag.Yes)
            UsedForReturn = "Y";
        else
            UsedForReturn = "N";

        if (bCustomer.PaymentMethod == BEnumPaymentType.CreditCard)
            PaymentMethod = "CC";
        else
            PaymentMethod = "DP";

        if (bCustomer.DeferedPaymentRequired == BEnumFlag.Yes)
            DeferedPaymentRequired = "Y";
        else
            DeferedPaymentRequired = "N";

        if (bCustomer.IsDeferredPaymentAgreed == BEnumFlag.Yes)
            DeferredPaymentAgreed = "Y";
        else
            DeferredPaymentAgreed = "N";

        if (bCustomer.CustomerType == BEnumCustomerType.KeyCustomer)
            CustomerType = "K";
        else
            CustomerType = "R";

        if (bCustomer.CustomerTypeChanged == BEnumFlag.Yes)
            CustomerTypeChanged = "Y";
        else
            CustomerTypeChanged = "Y";

        //if (bCustomer.ShipmentPreference == BEnumShipPreference.MostCompetitive)
        //    ShipmentPreference = "1";
        //if (bCustomer.ShipmentPreference == BEnumShipPreference.NamedCarrier)
        //    ShipmentPreference = "3";
        //if (bCustomer.ShipmentPreference == BEnumShipPreference.Fastest)
        ShipmentPreference = bCustomer.ShipmentPreference;
        bCustomer.ChiefContact = "";


        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        System.Nullable<int> iReturnValue = context.uSP_END_CUSTOMER_INSERT(bCustomer.AccountNo, bCustomer.Email, bCustomer.Password, UserType,
                                                                     Status, SaleTariffAssigned, TosAccepted, bCustomer.ToSAcceptedDate,
                                                                     bCustomer.Language, bCustomer.LastLogin, ChangePasswordRequired,
                                                                     bCustomer.CreatedBy, CreatedUserType,bCustomer.CompanyName, bCustomer.Name,
                                                                     bCustomer.Designation, bCustomer.TelephoneNo, bCustomer.HqZipcode,
                                                                     bCustomer.Country, KeyAccount, CustomerCategory, bCustomer.ChiefContact,
                                                                     bCustomer.IndustryType, bCustomer.LegalForm, bCustomer.ContactName, bCustomer.InvoicePhoneNumber,
                                                                     bCustomer.InvoiceFaxNo, bCustomer.InvoiceAddress1, bCustomer.InvoiceAddress2, bCustomer.InvoiceAddress3,
                                                                     bCustomer.InvoiceZipcode, bCustomer.InvoiceCity, bCustomer.InvoiceCountry, UsedForShipping,
                                                                     UsedForReturn, ShipmentPreference, PaymentMethod, DeferedPaymentRequired, (decimal)bCustomer.BudgetAmount,
                                                                     DeferredPaymentAgreed, (decimal)bCustomer.WishedBudgetAmount, bCustomer.AdministratorUserId,
                                                                     CustomerType, CustomerTypeChanged, bCustomer.InsuredCreditAmount, bCustomer.PaymentDelayDays,
                                                                     bCustomer.PaymentDelayMonth, (decimal)bCustomer.CompensationRate, (decimal)bCustomer.CompensationAmount,
                                                                     (decimal)bCustomer.DepositAmount, (decimal)bCustomer.AvailableCreditLimit, (decimal)bCustomer.AuthorizedCreditAmount,
                                                                     bCustomer.CarrierAccountReference, (decimal)bCustomer.AvailableCredit, bCustomer.CreatedDate, bCustomer.LastUpdate, bCustomer.VatNo, bCustomer.SiretNo).SingleOrDefault();

        result = (int)iReturnValue;
        #endregion
        return result;
    }

      /// <summary>
      /// To validate HQZipcode with AssignedZone/Franchise table (Comma seperated Value)
      /// </summary>
      /// <param name="HQZipcode"></param>
      /// <returns>
      /// 0 - Available
      /// 1 - Not Available
      /// </returns>
    public string ValidateHQZipcode(string HQZipcode)
    {
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        var result = context.uSP_VALIDATE_HQZIPCODE(HQZipcode.Trim()).SingleOrDefault();

        return result;
    
    }

      /// <summary>
      /// To update additional details of created customer in Customer table by admin / Franchise User
      /// </summary>
      /// <param name="bCustomer"></param>
      /// <returns>
      /// 0 - Success
      /// 1 - Failure
      /// 2 - User not exists
      /// </returns>
    public int UpdateEndCustomerByAdmin(BCustomer bCustomer)
    {
        int result = 1;

        #region Insert process for Customer table

        string UserType = "", Status = "", SaleTariffAssigned = "", TosAccepted = "";
        string ChangePasswordRequired = "", CreatedUserType = "";
        string CustomerCategory = "", KeyAccount = "";
        string UsedForShipping = "", UsedForReturn = "", PaymentMethod = "";
        string DeferedPaymentRequired = "", DeferredPaymentAgreed = "";
        string CustomerType = "", CustomerTypeChanged = "", ShipmentPreference = "";

        if (bCustomer.UserType == BEnumUserType.Administrator)
            UserType = "AD";
        else if (bCustomer.UserType == BEnumUserType.Franchise)
            UserType = "FR";
        else if (bCustomer.UserType == BEnumUserType.Authorized)
            UserType = "AZ";
        else if (bCustomer.UserType == BEnumUserType.CustomerService)
            UserType = "CS";
        else if (bCustomer.UserType == BEnumUserType.Referent)
            UserType = "RF";
        else
            UserType = "AD";

        if (bCustomer.Status == BEnumUserStatus.Enabled)
            Status = "E";
        else if (bCustomer.Status == BEnumUserStatus.Archived)
            Status = "A";
        else if (bCustomer.Status == BEnumUserStatus.BeingCreated)
            Status = "B";
        else if (bCustomer.Status == BEnumUserStatus.Disabled)
            Status = "D";

        if (bCustomer.IsSalesTarrifAssigned == BEnumFlag.Yes)
            SaleTariffAssigned = "Y";
        else
            SaleTariffAssigned = "N";

        if (bCustomer.IsToSAccepted == BEnumFlag.Yes)
            TosAccepted = "Y";
        else
            TosAccepted = "N";

        if (bCustomer.IsChangePasswordRequired == BEnumFlag.Yes)
            ChangePasswordRequired = "Y";
        else
            ChangePasswordRequired = "N";

        if (bCustomer.CreatedUserType == BEnumUserType.Administrator)
            CreatedUserType = "AD";
        else if (bCustomer.CreatedUserType == BEnumUserType.Franchise)
            CreatedUserType = "FR";
        else if (bCustomer.CreatedUserType == BEnumUserType.Authorized)
            CreatedUserType = "AZ";
        else if (bCustomer.CreatedUserType == BEnumUserType.CustomerService)
            CreatedUserType = "CS";
        else if (bCustomer.CreatedUserType == BEnumUserType.Referent)
            CreatedUserType = "RF";
        else
            CreatedUserType = "AD";

        if (bCustomer.IsKeyAccount == BEnumFlag.Yes)
            KeyAccount = "Y";
        else
            KeyAccount = "N";

        if (bCustomer.IsKeyAccount == BEnumFlag.Yes)
            KeyAccount = "Y";
        else
            KeyAccount = "N";

        if (bCustomer.CustomerCategory == BEnumCustCategory.A)
            CustomerCategory = "A";
        else if (bCustomer.CustomerCategory == BEnumCustCategory.B)
            CustomerCategory = "B";
        else if (bCustomer.CustomerCategory == BEnumCustCategory.C)
            CustomerCategory = "C";

        if (bCustomer.UsedForShipping == BEnumFlag.Yes)
            UsedForShipping = "Y";
        else
            UsedForShipping = "N";

        if (bCustomer.UsedForReturn == BEnumFlag.Yes)
            UsedForReturn = "Y";
        else
            UsedForReturn = "N";

        if (bCustomer.PaymentMethod == BEnumPaymentType.CreditCard)
            PaymentMethod = "CC";
        else
            PaymentMethod = "DP";

        if (bCustomer.DeferedPaymentRequired == BEnumFlag.Yes)
            DeferedPaymentRequired = "Y";
        else
            DeferedPaymentRequired = "N";

        if (bCustomer.IsDeferredPaymentAgreed == BEnumFlag.Yes)
            DeferredPaymentAgreed = "Y";
        else
            DeferredPaymentAgreed = "N";

        if (bCustomer.CustomerType == BEnumCustomerType.KeyCustomer)
            CustomerType = "K";
        else
            CustomerType = "R";

        if (bCustomer.CustomerTypeChanged == BEnumFlag.Yes)
            CustomerTypeChanged = "Y";
        else
            CustomerTypeChanged = "Y";

        //if (bCustomer.ShipmentPreference == BEnumShipPreference.MostCompetitive)
        //    ShipmentPreference = "1";
        //if (bCustomer.ShipmentPreference == BEnumShipPreference.NamedCarrier)
        //    ShipmentPreference = "3";
        //if (bCustomer.ShipmentPreference == BEnumShipPreference.Fastest)
        ShipmentPreference = bCustomer.ShipmentPreference;
        //bCustomer.ChiefContact = "";

        #endregion

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        System.Nullable<int> iReturnValue = context.uSP_END_CUSTOMER_UPDATE_BY_ADMIN(bCustomer.Email, Status, TosAccepted, bCustomer.ToSAcceptedDate,
                                                                     bCustomer.ContactName, bCustomer.InvoicePhoneNumber,
                                                                     bCustomer.InvoiceFaxNo, bCustomer.InvoiceAddress1, bCustomer.InvoiceAddress2, bCustomer.InvoiceAddress3,
                                                                     bCustomer.InvoiceZipcode, bCustomer.InvoiceCity, bCustomer.InvoiceCountry, UsedForShipping,
                                                                     UsedForReturn, ShipmentPreference, PaymentMethod, DeferedPaymentRequired, (decimal)bCustomer.BudgetAmount,
                                                                     DeferredPaymentAgreed, bCustomer.AdministratorUserId,
                                                                     CustomerType, CustomerTypeChanged, bCustomer.LastUpdate, bCustomer.VatNo, bCustomer.SiretNo).SingleOrDefault();

        result = (int)iReturnValue;
       
        return result;
    
    
    
    }

      /// <summary>
      /// To get Cretdit list for list of customer ids
      /// </summary>
      /// <param name="AccountNo"></param>
      /// <param name="UserType"></param>
      /// <returns>  List<BCustomer> </returns>
    public List<BCustomer> GetCustomerCredit(string AccountNo, string UserType)
    {
        List<BCustomer> bCustomerList = new List<BCustomer>();

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        var userContext = context.uSP_GET_CREDIT_REQUEST_ACCEPT(AccountNo.Trim(), UserType.Trim()).ToList();

        foreach (var GetList in userContext)
        {
            BCustomer bCustomer = new BCustomer();
            bCustomer.AccountNo = GetList.ACCOUNT_NO.Trim();
            bCustomer.CompanyName = GetList.COMPANY_NAME.Trim();
            bCustomer.WishedBudgetAmount = GetList.WISHED_BUDGET_AMT;
            bCustomer.BudgetAmount = GetList.TRANS_BUDGET_AMT;
            bCustomer.InsuredCreditAmount = GetList.INSURED_CREDIT_AMT;
            bCustomer.PaymentDelayDays = GetList.PAYMENT_DELAY_DAYS;
            bCustomer.GrossMargin = GetList.GROSS_MARGIN;
            bCustomer.PaymentDelayMonth = GetList.PAYMENT_DELAY_MONTH;
            bCustomer.CompensationRate = GetList.COMPENSATION_RATE;
            bCustomer.DepositAmount = (decimal)GetList.DEPOSIT_AMT;
            bCustomer.AuthorizedCreditLimit = (decimal)GetList.AUTHORIZED_CREDIT_LIMIT;
            bCustomer.AuthorizedCreditAmount = GetList.AUTHORIZED_CREDIT_AMT;
            bCustomerList.Add(bCustomer);
        }
        return bCustomerList;
    }

      /// <summary>
      /// To update Credit details for a customer
      /// </summary>
      /// <param name="AccountNo"></param>
      /// <param name="WishedAmt"></param>
      /// <param name="InsuredAmt"></param>
      /// <param name="PaymentDelayDay"></param>
      /// <param name="GrossMargin"></param>
      /// <param name="PaymentDelayMonth"></param>
      /// <param name="CompensationRate"></param>
      /// <param name="AuthorizedCreditAmt"></param>
      /// <param name="DeferredPaymentAgreed"></param>
      /// <returns></returns>
    public int UpdateCustomerCredit(string AccountNo, decimal WishedAmt, decimal InsuredAmt, int PaymentDelayDay, decimal GrossMargin, int PaymentDelayMonth, decimal CompensationRate, decimal AuthorizedCreditAmt, string DeferredPaymentAgreed)
    {
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        System.Nullable<int> iReturnValue = context.uSP_GET_CREDIT_REQUEST_ACCEPT_UPDATE(AccountNo.Trim(), WishedAmt, InsuredAmt, PaymentDelayDay, GrossMargin, PaymentDelayMonth, CompensationRate, AuthorizedCreditAmt, DeferredPaymentAgreed, DateTime.Now).SingleOrDefault();
        return (int)iReturnValue;
    }

      /// <summary>
      /// To get all the available Authorized user list
      /// </summary>
      /// <returns></returns>
    public List<BAuthorized> GetAuthorizedUserList(string AccountNo)
    {
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        var lstAuthorizedList = context.uSP_GET_AUTHORIZED_USER_LIST(AccountNo.Trim()).ToList();

        List<BAuthorized> bAuthourizedList = new List<BAuthorized>();
        foreach (var authorizedList in lstAuthorizedList)
        {
            BAuthorized bAuthorized = new BAuthorized();
            bAuthorized.AccountNo = authorizedList.ACCOUNT_NO.Trim();
            bAuthorized.ContactName = authorizedList.CONTACT_NAME.Trim();
            bAuthorized.Email= authorizedList.EMAIL.Trim();
            bAuthorized.Password ="";
            bAuthorized.TelephoneNo = authorizedList.TEL_NO.Trim();
            bAuthorized.ReferentUserId = authorizedList.REFERENT_USER_ID.Trim();
            bAuthourizedList.Add(bAuthorized);
        }

        return bAuthourizedList;
    }

      /// <summary>
      /// To update Authorized User details
      /// </summary>
      /// <param name="AccountNo"></param>
      /// <param name="CompanyName"></param>
      /// <param name="Email"></param>
      /// <param name="PhoneNo"></param>
      /// <param name="RefAccountNo"></param>
      /// <returns></returns>
    public int UpdateAuthorize(string AccountNo, string CompanyName, string Email, string PhoneNo, string RefAccountNo)
    {
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        System.Nullable<int> iReturnValue = context.uSP_AUTHORIZED_UPDATE(AccountNo.Trim(), CompanyName.Trim(), Email.Trim(), PhoneNo.Trim(), RefAccountNo.Trim()).SingleOrDefault();
        return (int)iReturnValue;
    }

      /// <summary>
      /// To insert a autorized User details
      /// </summary>
      /// <param name="bAuthorize"></param>
      /// <returns></returns>
    public int InsertAuthorized(BAuthorized bAuthorize)
    {
        int result = 1;

        #region Authorized User creation part

        string UserType = "", Status = "", SaleTariffAssigned = "", TosAccepted = "";
        string ChangePasswordRequired = "", CreatedUserType = "";

        if (bAuthorize.UserType == BEnumUserType.Administrator)
            UserType = "AD";
        else if (bAuthorize.UserType == BEnumUserType.Franchise)
            UserType = "FR";
        else if (bAuthorize.UserType == BEnumUserType.Authorized)
            UserType = "AZ";
        else if (bAuthorize.UserType == BEnumUserType.CustomerService)
            UserType = "CS";
        else if (bAuthorize.UserType == BEnumUserType.Referent)
            UserType = "RF";
        else
            UserType = "AD";

        if (bAuthorize.Status == BEnumUserStatus.Enabled)
            Status = "E";
        else if (bAuthorize.Status == BEnumUserStatus.Archived)
            Status = "A";
        else if (bAuthorize.Status == BEnumUserStatus.BeingCreated)
            Status = "B";
        else if (bAuthorize.Status == BEnumUserStatus.Disabled)
            Status = "D";

        if (bAuthorize.IsSalesTarrifAssigned == BEnumFlag.Yes)
            SaleTariffAssigned = "Y";
        else
            SaleTariffAssigned = "N";

        if (bAuthorize.IsToSAccepted == BEnumFlag.Yes)
            TosAccepted = "Y";
        else
            TosAccepted = "N";

        if (bAuthorize.IsChangePasswordRequired == BEnumFlag.Yes)
            ChangePasswordRequired = "Y";
        else
            ChangePasswordRequired = "N";

        if (bAuthorize.CreatedUserType == BEnumUserType.Administrator)
            CreatedUserType = "AD";
        else if (bAuthorize.CreatedUserType == BEnumUserType.Franchise)
            CreatedUserType = "FR";
        else if (bAuthorize.CreatedUserType == BEnumUserType.Authorized)
            CreatedUserType = "AZ";
        else if (bAuthorize.CreatedUserType == BEnumUserType.CustomerService)
            CreatedUserType = "CS";
        else if (bAuthorize.CreatedUserType == BEnumUserType.Referent)
            CreatedUserType = "RF";
        else
            CreatedUserType = "AD";

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        System.Nullable<int> iReturnValue = context.uSP_AUTHORIZED_INSERT(bAuthorize.AccountNo, bAuthorize.Email, bAuthorize.Password, UserType,
                                                                     Status, SaleTariffAssigned, TosAccepted, bAuthorize.ToSAcceptedDate,
                                                                     bAuthorize.Language, bAuthorize.LastLogin, ChangePasswordRequired,
                                                                     bAuthorize.CreatedBy, CreatedUserType, bAuthorize.ContactName,
                                                                     bAuthorize.TelephoneNo,bAuthorize.ReferentUserId).SingleOrDefault();

        
        #endregion   

        result = (int)iReturnValue;
        return result;
    }

      /// <summary>
      /// To update Authorized user details
      /// </summary>
      /// <param name="AccountNo"></param>
      /// <param name="Password"></param>
      /// <param name="TelNo"></param>
      /// <returns></returns>
    public int UpdateAuthorizedSelf(string AccountNo, string Password, string TelNo)
    {
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        System.Nullable<int> iReturnValue = context.uSP_AUTHORIZED_SELF_UPDATE(AccountNo.Trim(), Password.Trim(),TelNo.Trim()).SingleOrDefault();
        return (int)iReturnValue;
    }

      /// <summary>
      /// To get Authorized user details in requried format
      /// </summary>
      /// <param name="AccountNo"></param>
      /// <returns></returns>
    public BAuthorized GetAuthorized(string AccountNo)
    {

        BAuthorized bAuthorized = new BAuthorized();
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        var userContext = context.uSP_GET_AUTHORIZED(AccountNo.Trim()).SingleOrDefault();
        if (userContext != null)
        {
            bAuthorized.ContactName = userContext.COMPANY_NAME.Trim();
            bAuthorized.ReferentUserId = userContext.REFERENT_ACCOUNT_NO.Trim();
            bAuthorized.TelephoneNo = userContext.TEL_NO.Trim();
        }
        return bAuthorized;    
    }

    /// <summary>
    /// To get End customer list user list
    /// </summary>
    /// <returns>company name, contact name, emailID for N2 user </returns>
    public List<BFranchiseContact> GetEndCustomerList()
    {
        List<BFranchiseContact> bFranchiseList = new List<BFranchiseContact>();
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        var userContext = context.uSP_GET_ENDCUSTOMER_LIST().ToList();

        foreach (var FranchiseList in userContext)
        {
            BFranchiseContact bFranchise = new BFranchiseContact();
            bFranchise.CompanyName = FranchiseList.COMPANY_NAME.Trim();
            bFranchise.Email = FranchiseList.EMAIL.Trim();
            bFranchise.Name = FranchiseList.CONTACT_NAME.Trim();
            bFranchise.Language = FranchiseList.LANGUAGE.Trim();
            bFranchiseList.Add(bFranchise);
        }
        return bFranchiseList;
    }

    /// <summary>
    /// Retrive user Details
    /// </summary>
    /// <returns>User details as User entity</returns>
    public string GetUser(string accountNumber)
    {
        string result = "";
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        result = context.uSP_GET_USERID(accountNumber.Trim()).SingleOrDefault();
        return result;
    }

    /// <summary>
    /// To update additional details of created customer in Customer table by admin / Franchise User
    /// </summary>
    /// <param name="bCustomer"></param>
    /// <returns>
    /// 0 - Success
    /// 1 - Failure
    /// 2 - User not exists
    /// </returns>
    public int UpdateEndCustomer(BCustomer bCustomer)
    {
        int result = 1;

        #region Insert process for Customer table

        string UserType = "", Status = "", SaleTariffAssigned = "", TosAccepted = "";
        string ChangePasswordRequired = "", CreatedUserType = "";
        string CustomerCategory = "", KeyAccount = "";
        string UsedForShipping = "", UsedForReturn = "", PaymentMethod = "";
        string DeferedPaymentRequired = "", DeferredPaymentAgreed = "";
        string CustomerType = "", CustomerTypeChanged = "", ShipmentPreference = "";
        string FictiveAccount = "";

        if (bCustomer.UserType == BEnumUserType.Administrator)
            UserType = "AD";
        else if (bCustomer.UserType == BEnumUserType.Franchise)
            UserType = "FR";
        else if (bCustomer.UserType == BEnumUserType.Authorized)
            UserType = "AZ";
        else if (bCustomer.UserType == BEnumUserType.CustomerService)
            UserType = "CS";
        else if (bCustomer.UserType == BEnumUserType.Referent)
            UserType = "RF";
        else
            UserType = "AD";

        if (bCustomer.Status == BEnumUserStatus.Enabled)
            Status = "E";
        else if (bCustomer.Status == BEnumUserStatus.Archived)
            Status = "A";
        else if (bCustomer.Status == BEnumUserStatus.BeingCreated)
            Status = "B";
        else if (bCustomer.Status == BEnumUserStatus.Disabled)
            Status = "D";

        if (bCustomer.IsSalesTarrifAssigned == BEnumFlag.Yes)
            SaleTariffAssigned = "Y";
        else
            SaleTariffAssigned = "N";

        if (bCustomer.IsToSAccepted == BEnumFlag.Yes)
            TosAccepted = "Y";
        else
            TosAccepted = "N";

        if (bCustomer.IsChangePasswordRequired == BEnumFlag.Yes)
            ChangePasswordRequired = "Y";
        else
            ChangePasswordRequired = "N";

        if (bCustomer.CreatedUserType == BEnumUserType.Administrator)
            CreatedUserType = "AD";
        else if (bCustomer.CreatedUserType == BEnumUserType.Franchise)
            CreatedUserType = "FR";
        else if (bCustomer.CreatedUserType == BEnumUserType.Authorized)
            CreatedUserType = "AZ";
        else if (bCustomer.CreatedUserType == BEnumUserType.CustomerService)
            CreatedUserType = "CS";
        else if (bCustomer.CreatedUserType == BEnumUserType.Referent)
            CreatedUserType = "RF";
        else
            CreatedUserType = "AD";

        if (bCustomer.IsKeyAccount == BEnumFlag.Yes)
            KeyAccount = "Y";
        else
            KeyAccount = "N";

        if (bCustomer.IsKeyAccount == BEnumFlag.Yes)
            KeyAccount = "Y";
        else
            KeyAccount = "N";

        if (bCustomer.CustomerCategory == BEnumCustCategory.A)
            CustomerCategory = "A";
        else if (bCustomer.CustomerCategory == BEnumCustCategory.B)
            CustomerCategory = "B";
        else if (bCustomer.CustomerCategory == BEnumCustCategory.C)
            CustomerCategory = "C";

        if (bCustomer.UsedForShipping == BEnumFlag.Yes)
            UsedForShipping = "Y";
        else
            UsedForShipping = "N";

        if (bCustomer.UsedForReturn == BEnumFlag.Yes)
            UsedForReturn = "Y";
        else
            UsedForReturn = "N";

        if (bCustomer.PaymentMethod == BEnumPaymentType.CreditCard)
            PaymentMethod = "CC";
        else
            PaymentMethod = "DP";

        if (bCustomer.DeferedPaymentRequired == BEnumFlag.Yes)
            DeferedPaymentRequired = "Y";
        else
            DeferedPaymentRequired = "N";

        if (bCustomer.IsDeferredPaymentAgreed == BEnumFlag.Yes)
            DeferredPaymentAgreed = "Y";
        else
            DeferredPaymentAgreed = "N";

        if (bCustomer.CustomerType == BEnumCustomerType.KeyCustomer)
            CustomerType = "K";
        else
            CustomerType = "R";

        if (bCustomer.CustomerTypeChanged == BEnumFlag.Yes)
            CustomerTypeChanged = "Y";
        else
            CustomerTypeChanged = "Y";

        //if (bCustomer.ShipmentPreference == BEnumShipPreference.MostCompetitive)
        //    ShipmentPreference = "1";
        //if (bCustomer.ShipmentPreference == BEnumShipPreference.NamedCarrier)
        //    ShipmentPreference = "3";
        //if (bCustomer.ShipmentPreference == BEnumShipPreference.Fastest)
        ShipmentPreference = bCustomer.ShipmentPreference;
        //bCustomer.ChiefContact = "";

        if (bCustomer.FICTIVE_ACCOUNT == BEnumFlag.Yes)
            FictiveAccount = "Y";
        else
            FictiveAccount = "N";
        #endregion

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        System.Nullable<int> iReturnValue = context.uSP_END_CUSTOMER_UPDATE(bCustomer.Email, bCustomer.Password, Status,
                                                                              bCustomer.HqZipcode, bCustomer.CompanyName, bCustomer.TurnOver,
                                                                              bCustomer.GrossMargin, bCustomer.ADV, CustomerCategory,
                                                                              bCustomer.Name, bCustomer.IndustryType, bCustomer.LegalForm,
                                                                              bCustomer.ManPower, bCustomer.ChiefContact, bCustomer.InvoicePhoneNumber,
                                                                              bCustomer.InvoiceFaxNo, bCustomer.VatNo, bCustomer.SiretNo, bCustomer.InvoiceAddress1,
                                                                              bCustomer.InvoiceAddress2, bCustomer.InvoiceAddress3, bCustomer.InvoiceZipcode,
                                                                              bCustomer.InvoiceCity, bCustomer.InvoiceCountry, UsedForShipping, ShipmentPreference,
                                                                              bCustomer.CarrierName, bCustomer.InsuredMethod, PaymentMethod, DeferedPaymentRequired,
                                                                              bCustomer.DEFERED_PAYMENT_TYPE, (decimal)bCustomer.BudgetAmount, bCustomer.BudgetAmount,
                                                                              bCustomer.PaymentDelayDays, KeyAccount, bCustomer.KEY_CARRIER, bCustomer.SUBSCRIPTION_AMOUNT,
                                                                              bCustomer.EXTRA_INFO, FictiveAccount, DateTime.Now, CustomerType, CustomerTypeChanged).SingleOrDefault();

        result = (int)iReturnValue;
        return result;



    }

    /// <summary>
    /// To insert list of ADVs into ADV table
    /// </summary>
    /// <param name="bAdv"></param>
    /// <returns>
    /// 0 = Success
    /// 1 = Error
    /// 
    /// </returns>
    public int InsertAdv(List<BAdv> bAdv)
    {
        int result = 1;

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        for (int i = 0; i < bAdv.Count; i++)
        {
            System.Nullable<int> iReturnValue = context.uSP_ADV_INSERT(bAdv[i].ACCOUNT_NO, bAdv[i].CARRIER_NAME, bAdv[i].PRIORITY, bAdv[i].AVERAGE_VALUE,
                                                                        bAdv[i].AVERAGE_WEIGHT, bAdv[i].PACKAGE_TYPE, bAdv[i].SHIPMENT_TYPE, bAdv[i].DISCOUNT).SingleOrDefault();
            result = (int)iReturnValue;
        }
        return result;
    }

    public BCustomer GetEndCustomer(string UserId)
    {
        BCustomer bCustomer = new BCustomer();

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        var userContext = context.uSP_GET_END_CUSTOMER(UserId.Trim()).SingleOrDefault();
        if (userContext != null)
        {

            bCustomer.UserType = BEnumUserType.Administrator;

            if (userContext.STATUS.Trim() == "B")
                bCustomer.Status = BEnumUserStatus.BeingCreated;
            else if (userContext.STATUS.Trim() == "E")
                bCustomer.Status = BEnumUserStatus.Enabled;
            else if (userContext.STATUS.Trim() == "D")
                bCustomer.Status = BEnumUserStatus.Disabled;
            else if (userContext.STATUS.Trim() == "A")
                bCustomer.Status = BEnumUserStatus.Archived;
            else
                bCustomer.Status = BEnumUserStatus.Enabled;

            bCustomer.IsSalesTarrifAssigned = BEnumFlag.Yes;
            bCustomer.IsToSAccepted = BEnumFlag.Yes;
            bCustomer.IsChangePasswordRequired = BEnumFlag.Yes;
            bCustomer.CreatedUserType = BEnumUserType.Administrator;

            bCustomer.HqZipcode = userContext.HQ_ZIP_CODE.Trim();
            bCustomer.CompanyName = userContext.COMPANY_NAME.Trim();
            bCustomer.TurnOver = (decimal)userContext.TURN_OVER;
            bCustomer.GrossMargin = (decimal)userContext.GROSS_MARGIN;
            bCustomer.ADV = (int)userContext.ADV;
            //bCustomer.Country = userContext.COUNTRY.Trim();

            if (userContext.CUST_CATEGORY.Trim() == "A")
                bCustomer.CustomerCategory = BEnumCustCategory.A;
            else if (userContext.CUST_CATEGORY.Trim() == "B")
                bCustomer.CustomerCategory = BEnumCustCategory.B;
            else if (userContext.CUST_CATEGORY.Trim() == "C")
                bCustomer.CustomerCategory = BEnumCustCategory.C;
            else
                bCustomer.CustomerCategory = BEnumCustCategory.A;

            bCustomer.Name = userContext.CUST_NAME.Trim();
            bCustomer.IndustryType = userContext.INDUSTRY_TYPE.Trim();
            bCustomer.LegalForm = userContext.LEGAL_FORM.Trim();
            bCustomer.ManPower = (int)userContext.MAN_POWER;
            bCustomer.ChiefContact = userContext.CHIEF_CONTACT;
            bCustomer.InvoicePhoneNumber = userContext.INV_PHONE_NO.Trim();
            bCustomer.InvoiceFaxNo = userContext.INV_FAX_NO.Trim();
            bCustomer.VatNo = userContext.VAT_NO;
            bCustomer.SiretNo = userContext.SIRET_NO;
            bCustomer.InvoiceAddress1 = userContext.INV_ADDRESS1.Trim();
            bCustomer.InvoiceAddress2 = userContext.INV_ADDRESS2.Trim();
            bCustomer.InvoiceAddress3 = userContext.INV_ADDRESS3.Trim();
            bCustomer.InvoiceZipcode = userContext.INV_ZIP_CODE.Trim();
            bCustomer.InvoiceCity = userContext.INV_CITY.Trim();
            bCustomer.InvoiceCountry = userContext.INV_COUNTRY.Trim();

            if (userContext.USE_FOR_SHIPPING.Trim() == "Y")
                bCustomer.UsedForShipping = BEnumFlag.Yes;
            else if (userContext.USE_FOR_SHIPPING.Trim() == "N")
                bCustomer.UsedForShipping = BEnumFlag.No;
            else
                bCustomer.UsedForShipping = BEnumFlag.Yes;

            //if (userContext.SHIP_PREFERENCE.Trim() == "1")
            //    bCustomer.ShipmentPreference = BEnumShipPreference.MostCompetitive;
            //else if (userContext.SHIP_PREFERENCE.Trim() == "2")
            //    bCustomer.ShipmentPreference = BEnumShipPreference.Fastest;
            //else if (userContext.SHIP_PREFERENCE.Trim() == "3")
            //    bCustomer.ShipmentPreference = BEnumShipPreference.NamedCarrier;
            //else
            //    bCustomer.ShipmentPreference = BEnumShipPreference.MostCompetitive;

            bCustomer.CarrierName = userContext.CARRIER_NAME.Trim();
            bCustomer.InsuredMethod = userContext.INSURED_METHOD.Trim();

            if (userContext.PAYMENT_METHOD.Trim() == "CC")
                bCustomer.PaymentMethod = BEnumPaymentType.CreditCard;
            else if (userContext.PAYMENT_METHOD.Trim() == "DP")
                bCustomer.PaymentMethod = BEnumPaymentType.DeferredPayment;
            else
                bCustomer.PaymentMethod = BEnumPaymentType.DeferredPayment;

            if (userContext.DEFERRED_PAY_REQ.Trim() == "Y")
                bCustomer.DeferedPaymentRequired = BEnumFlag.Yes;
            else if (userContext.DEFERRED_PAY_REQ.Trim() == "N")
                bCustomer.DeferedPaymentRequired = BEnumFlag.No;
            else
                bCustomer.DeferedPaymentRequired = BEnumFlag.Yes;

            bCustomer.DEFERED_PAYMENT_TYPE = userContext.DEFERED_PAYMENT_TYPE.Trim();
            bCustomer.BudgetAmount = (decimal)userContext.TRANS_BUDGET_AMT;
            bCustomer.DepositAmount = (decimal)userContext.DEPOSIT_AMT;
            bCustomer.PaymentDelayDays = (int)userContext.PAYMENT_DELAY_DAYS;

            if (userContext.KEY_ACCOUNT.Trim() == "Y")
                bCustomer.IsKeyAccount = BEnumFlag.Yes;
            else if (userContext.KEY_ACCOUNT.Trim() == "N")
                bCustomer.IsKeyAccount = BEnumFlag.No;
            else
                bCustomer.IsKeyAccount = BEnumFlag.Yes;

            bCustomer.KEY_CARRIER = userContext.KEY_CARRIER.Trim();

            bCustomer.SUBSCRIPTION_AMOUNT = (decimal)userContext.SUBSCRIPTION_AMOUNT;
            bCustomer.EXTRA_INFO = userContext.EXTRA_INFO.Trim();

            if (userContext.FICTIVE_ACCOUNT.Trim() == "Y")
                bCustomer.FICTIVE_ACCOUNT = BEnumFlag.Yes;
            else if (userContext.FICTIVE_ACCOUNT.Trim() == "N")
                bCustomer.FICTIVE_ACCOUNT = BEnumFlag.No;
            else
                bCustomer.FICTIVE_ACCOUNT = BEnumFlag.Yes;

            bCustomer.UsedForReturn = BEnumFlag.Yes;
            bCustomer.IsDeferredPaymentAgreed = BEnumFlag.No;
            bCustomer.CustomerType = BEnumCustomerType.RegularCustomer;
            bCustomer.CustomerTypeChanged = BEnumFlag.Yes;
        }
        return bCustomer;
    }

    public int UpdateFranchiseAdmin(BFranchise bFranchise)
    {
        string CustomerType = "", CustomerTypeChanged = "", Status = "";

        if (bFranchise.CustomerType == BEnumCustomerType.KeyCustomer)
            CustomerType = "K";
        else
            CustomerType = "R";

        if (bFranchise.CustomerTypeChanged == BEnumFlag.Yes)
            CustomerTypeChanged = "Y";
        else
            CustomerTypeChanged = "N";

        if (bFranchise.Status == BEnumUserStatus.Archived)
            Status = "A";
        else if (bFranchise.Status == BEnumUserStatus.Disabled)
            Status = "D";
        else if (bFranchise.Status == BEnumUserStatus.Enabled)
            Status = "E";

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        System.Nullable<int> iReturnValue = context.uSP_FRANCHIS_UPDATE_ADMIN(bFranchise.Email, Status.Trim(), bFranchise.CompanyName,
                                                                               bFranchise.AssignedZone, bFranchise.ScannedDoc,
                                                                               bFranchise.Language, DateTime.Now).SingleOrDefault();
            
        return ((int)iReturnValue);
    
    }
    public int DeleteAuthorized(string AccountNo)
    {
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        System.Nullable<int> iReturnValue = context.uSP_AUTHORIZED_DELETE(AccountNo.Trim()).SingleOrDefault();
        return (int)iReturnValue;
    }

    public int InsertCarrierAccountRef(string[] CarrierAccList, string[] CarrierAcc, string AccountNo)
    {
        int result = 1;

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        for (int i = 0; i < CarrierAccList.Count(); i++)
        {
            System.Nullable<int> iReturnValue = context.uSP_CARRIER_ACCOUNT_REF_INSERT(AccountNo.Trim(), CarrierAccList[i].Trim(), CarrierAcc[i].Trim()).SingleOrDefault();
            result = (int)iReturnValue;
        }
        return result;
    }
    public string GetPostalCode(string strCountryCode)
    {
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        string strFormat = string.Empty;
        var result = context.uSP_GET_POSTAL_CODE_FORMAT(strCountryCode).DefaultIfEmpty().First();
        if (!(result == null))
        {
            strFormat = result.POSTAL_CODE_FORMAT;
        }

        return strFormat;
    }
  }
}
