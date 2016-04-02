using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KaizosServiceLibrary.Model; //to refer all Service entities.
using Kaizos.Entities.Business; //to refre all business entities.


namespace KaizosServiceLibrary.Adapter
{
    public class UserAdapter
    {
        #region User Entity
          /// <summary>
        /// Converted Service entity to Business Entiry for User entity
        /// </summary>
        /// <param name="seUser"></param>
        /// <returns> BUser </returns>
        /// 
          public BUser ConvertStoB_User(SUser seUser)
          {
            BUser beUser = new BUser();

            if (seUser != null)
            {
                beUser.AccountNo = seUser.AccountNo;
                beUser.Email = seUser.Email;
                beUser.CreatedBy = seUser.CreatedBy;
                beUser.Password = seUser.Password;

                if (seUser.UserType == SEnumUserType.Authorized)
                    beUser.UserType = BEnumUserType.Authorized;
                else if (seUser.UserType == SEnumUserType.Administrator)
                    beUser.UserType = BEnumUserType.Administrator;
                else if (seUser.UserType == SEnumUserType.CustomerService)
                    beUser.UserType = BEnumUserType.CustomerService;
                else if (seUser.UserType == SEnumUserType.Franchise)
                    beUser.UserType = BEnumUserType.Franchise;
                else if (seUser.UserType == SEnumUserType.Referent)
                    beUser.UserType = BEnumUserType.Referent;

                if (seUser.Status == SEnumUserStatus.Archived)
                    beUser.Status = BEnumUserStatus.Archived;
                else if (seUser.Status == SEnumUserStatus.BeingCreated)
                    beUser.Status = BEnumUserStatus.BeingCreated;
                else if (seUser.Status == SEnumUserStatus.Disabled)
                    beUser.Status = BEnumUserStatus.Disabled;
                else if (seUser.Status == SEnumUserStatus.Enabled)
                    beUser.Status = BEnumUserStatus.Enabled;

                if (seUser.IsSalesTarrifAssigned == SEnumFlag.Yes)
                    beUser.IsSalesTarrifAssigned = BEnumFlag.Yes;
                else if (seUser.IsSalesTarrifAssigned == SEnumFlag.No)
                    beUser.IsSalesTarrifAssigned = BEnumFlag.No;

                beUser.ToSAcceptedDate = seUser.ToSAcceptedDate;
                beUser.Language = seUser.Language;
                beUser.LastLogin = seUser.LastLogin;

                if (seUser.IsChangePasswordRequired == SEnumFlag.Yes)
                    beUser.IsChangePasswordRequired = BEnumFlag.Yes;
                else if (seUser.IsChangePasswordRequired == SEnumFlag.No)
                    beUser.IsChangePasswordRequired = BEnumFlag.No;

                beUser.CreatedBy = seUser.CreatedBy;

                if (seUser.CreatedUserType == SEnumUserType.Administrator)
                    beUser.CreatedUserType = BEnumUserType.Administrator;
                else if (seUser.CreatedUserType == SEnumUserType.Authorized)
                    beUser.CreatedUserType = BEnumUserType.Authorized;
                else if (seUser.CreatedUserType == SEnumUserType.CustomerService)
                    beUser.CreatedUserType = BEnumUserType.CustomerService;
                else if (seUser.CreatedUserType == SEnumUserType.Franchise)
                    beUser.CreatedUserType = BEnumUserType.Franchise;
                else if (seUser.CreatedUserType == SEnumUserType.Referent)
                    beUser.CreatedUserType = BEnumUserType.Referent;

                beUser.CompanyName = seUser.CompanyName;
                beUser.Name = seUser.Name;
                beUser.TelephoneNo = seUser.TelephoneNo;
                beUser.Country = seUser.Country;
                beUser.LegalForm = seUser.LegalForm;
                beUser.CreatedDate = seUser.CreatedDate;
                beUser.LastUpdate = seUser.LastUpdate;

                if (seUser.CustomerType == SEnumCustomerType.KeyCustomer)
                    beUser.CustomerType = BEnumCustomerType.KeyCustomer;
                else if (seUser.CustomerType == SEnumCustomerType.RegularCustomer)
                    beUser.CustomerType = BEnumCustomerType.RegularCustomer;

                if (seUser.CustomerTypeChanged == SEnumFlag.Yes)
                    beUser.CustomerTypeChanged = BEnumFlag.Yes;
                else if (seUser.CustomerTypeChanged == SEnumFlag.No)
                    beUser.CustomerTypeChanged = BEnumFlag.No;
            }
            return beUser;
        }

           /// <summary>
        /// Converted Business Entiry  to Service entity for User entity
        /// </summary>
        /// <param name="BUser"></param>
        /// <returns> SUser </returns>
        /// 
          public SUser ConvertBtoS_User(BUser beUser)
        {
            SUser seUser = new SUser();

            if (beUser != null)
            {
                seUser.AccountNo = beUser.AccountNo;
                seUser.Email = beUser.Email;
                seUser.Password = beUser.Password;

                if (beUser.UserType == BEnumUserType.Authorized)
                    seUser.UserType = SEnumUserType.Authorized;
                else if (beUser.UserType == BEnumUserType.Administrator)
                    seUser.UserType = SEnumUserType.Administrator;
                else if (beUser.UserType == BEnumUserType.CustomerService)
                    seUser.UserType = SEnumUserType.CustomerService;
                else if (beUser.UserType == BEnumUserType.Franchise)
                    seUser.UserType = SEnumUserType.Franchise;
                else if (beUser.UserType == BEnumUserType.Referent)
                    seUser.UserType = SEnumUserType.Referent;

                if (beUser.Status == BEnumUserStatus.Archived)
                    seUser.Status = SEnumUserStatus.Archived;
                else if (beUser.Status == BEnumUserStatus.BeingCreated)
                    seUser.Status = SEnumUserStatus.BeingCreated;
                else if (beUser.Status == BEnumUserStatus.Disabled)
                    seUser.Status = SEnumUserStatus.Disabled;
                else if (beUser.Status == BEnumUserStatus.Enabled)
                    seUser.Status = SEnumUserStatus.Enabled;

                if (beUser.IsSalesTarrifAssigned == BEnumFlag.Yes)
                    seUser.IsSalesTarrifAssigned = SEnumFlag.Yes;
                else if (beUser.IsSalesTarrifAssigned == BEnumFlag.No)
                    seUser.IsSalesTarrifAssigned = SEnumFlag.No;

                if (beUser.IsToSAccepted == BEnumFlag.Yes)
                    seUser.IsToSAccepted = SEnumFlag.Yes;
                else if (beUser.IsToSAccepted == BEnumFlag.No)
                    seUser.IsToSAccepted = SEnumFlag.No;

                seUser.ToSAcceptedDate = beUser.ToSAcceptedDate;
                seUser.Language = beUser.Language;
                seUser.LastLogin = beUser.LastLogin;

                if (beUser.IsChangePasswordRequired == BEnumFlag.Yes)
                    seUser.IsChangePasswordRequired = SEnumFlag.Yes;
                else if (beUser.IsChangePasswordRequired == BEnumFlag.No)
                    seUser.IsChangePasswordRequired = SEnumFlag.No;

                seUser.CreatedBy = beUser.CreatedBy;

                if (beUser.CreatedUserType == BEnumUserType.Administrator)
                    seUser.CreatedUserType = SEnumUserType.Administrator;
                else if (beUser.CreatedUserType == BEnumUserType.Authorized)
                    seUser.CreatedUserType = SEnumUserType.Authorized;
                else if (beUser.CreatedUserType == BEnumUserType.CustomerService)
                    seUser.CreatedUserType = SEnumUserType.CustomerService;
                else if (beUser.CreatedUserType == BEnumUserType.Franchise)
                    seUser.CreatedUserType = SEnumUserType.Franchise;
                else if (beUser.CreatedUserType == BEnumUserType.Referent)
                    seUser.CreatedUserType = SEnumUserType.Referent;

                seUser.CompanyName = beUser.CompanyName;
                seUser.Name = beUser.Name;
                seUser.TelephoneNo = beUser.TelephoneNo;
                seUser.Country = beUser.Country;
                seUser.LegalForm = beUser.LegalForm;
                seUser.CreatedDate = beUser.CreatedDate;
                seUser.LastUpdate = beUser.LastUpdate;

                if (beUser.CustomerType == BEnumCustomerType.KeyCustomer)
                    seUser.CustomerType = SEnumCustomerType.KeyCustomer;
                else if (beUser.CustomerType == BEnumCustomerType.RegularCustomer)
                    seUser.CustomerType = SEnumCustomerType.RegularCustomer;

                if (beUser.CustomerTypeChanged == BEnumFlag.Yes)
                    seUser.CustomerTypeChanged = SEnumFlag.Yes;
                else if (beUser.CustomerTypeChanged == BEnumFlag.No)
                    seUser.CustomerTypeChanged = SEnumFlag.No;
            }

            return seUser;
        }


          /// <summary>
          /// Convert Business Entity to Service Entity - User ID
          /// </summary>
          /// <param name="sUserID"></param>
          /// <returns></returns>
          public BUserID ConvertStoB_UserID(SUserID sUserID)
          {
              BUserID bUserID = new BUserID();
              bUserID.AccountNo = sUserID.AccountNo;
              bUserID.UserName = sUserID.UserName;
              return bUserID;
          }

          /// <summary>
          /// Convert Service Entity to Business Entity - List of User ID
          /// </summary>
          /// <param name="sSimulationSurchargeDiscount"></param>
          /// <returns></returns>
          public List<BUserID> ConvertStoB_UserID(List<SUserID> sUserID)
          {
              List<BUserID> bUserID = new List<BUserID>();
              for (int i = 0; i < sUserID.Count; i++)
              {
                  bUserID.Add(ConvertStoB_UserID(sUserID[i]));
              }
              return bUserID;
          }

          /// <summary>
          /// Convert  Service Entity to Business Entity - User ID
          /// </summary>
          /// <param name="bUserID"></param>
          /// <returns></returns>
          public SUserID ConvertBtoS_UserID(BUserID bUserID)
          {
              SUserID sUserID = new SUserID();
              sUserID.AccountNo = bUserID.AccountNo;
              sUserID.UserName = bUserID.UserName;

              return sUserID;
          }

          /// <summary>
          /// Convert  Service Entity to Business Entity - List Simulation Surcharge Discount
          /// </summary>
          /// <param name="BSimulationSurchargeDiscount"></param>
          /// <returns></returns>
          public List<SUserID> ConvertBtoS_UserID(List<BUserID> bUserID)
          {
              List<SUserID> sUserID = new List<SUserID>();
              for (int i = 0; i < bUserID.Count; i++)
              {
                  sUserID.Add(ConvertBtoS_UserID(bUserID[i]));
              }
              return sUserID;
          }

        #endregion

        #region Franchise Entity

          /// <summary>
          /// Converted Business Entiry  to Service entity for Franchise entity
          /// </summary>
          /// <param name="BFranchise"></param>
          /// <returns> SFranchise </returns>
          /// 
          public SFranchise ConvertBtoS_Franchise(BFranchise bFranchise)
          {
            SFranchise sFranchise = new SFranchise();

            if (bFranchise != null)
            {
                sFranchise.AccountNo = bFranchise.AccountNo;
                sFranchise.Email = bFranchise.Email;
                sFranchise.Password = bFranchise.Password;

                if (bFranchise.UserType == BEnumUserType.Authorized)
                    sFranchise.UserType = SEnumUserType.Authorized;
                else if (bFranchise.UserType == BEnumUserType.Administrator)
                    sFranchise.UserType = SEnumUserType.Administrator;
                else if (bFranchise.UserType == BEnumUserType.CustomerService)
                    sFranchise.UserType = SEnumUserType.CustomerService;
                else if (bFranchise.UserType == BEnumUserType.Franchise)
                    sFranchise.UserType = SEnumUserType.Franchise;
                else if (bFranchise.UserType == BEnumUserType.Referent)
                    sFranchise.UserType = SEnumUserType.Referent;
                else
                    sFranchise.UserType = SEnumUserType.Administrator;

                if (bFranchise.Status == BEnumUserStatus.Archived)
                    sFranchise.Status = SEnumUserStatus.Archived;
                else if (bFranchise.Status == BEnumUserStatus.BeingCreated)
                    sFranchise.Status = SEnumUserStatus.BeingCreated;
                else if (bFranchise.Status == BEnumUserStatus.Disabled)
                    sFranchise.Status = SEnumUserStatus.Disabled;
                else if (bFranchise.Status == BEnumUserStatus.Enabled)
                    sFranchise.Status = SEnumUserStatus.Enabled;
                else
                    sFranchise.Status = SEnumUserStatus.Disabled;

                if (bFranchise.IsSalesTarrifAssigned == BEnumFlag.Yes)
                    sFranchise.IsSalesTarrifAssigned = SEnumFlag.Yes;
                else if (bFranchise.IsSalesTarrifAssigned == BEnumFlag.No)
                    sFranchise.IsSalesTarrifAssigned = SEnumFlag.No;
                else
                    sFranchise.IsSalesTarrifAssigned = SEnumFlag.No;

                if (bFranchise.IsToSAccepted == BEnumFlag.Yes)
                    sFranchise.IsToSAccepted = SEnumFlag.Yes;
                else if (bFranchise.IsToSAccepted == BEnumFlag.No)
                    sFranchise.IsToSAccepted = SEnumFlag.No;
                else
                    sFranchise.IsToSAccepted = SEnumFlag.No;

                sFranchise.ToSAcceptedDate = bFranchise.ToSAcceptedDate;
                sFranchise.Language = bFranchise.Language;
                sFranchise.LastLogin = bFranchise.LastLogin;

                if (bFranchise.IsChangePasswordRequired == BEnumFlag.Yes)
                    sFranchise.IsChangePasswordRequired = SEnumFlag.Yes;
                else if (bFranchise.IsChangePasswordRequired == BEnumFlag.No)
                    sFranchise.IsChangePasswordRequired = SEnumFlag.No;
                else
                    sFranchise.IsChangePasswordRequired = SEnumFlag.Yes;

                sFranchise.CreatedBy = bFranchise.CreatedBy;

                if (bFranchise.CreatedUserType == BEnumUserType.Administrator)
                    sFranchise.CreatedUserType = SEnumUserType.Administrator;
                else if (bFranchise.CreatedUserType == BEnumUserType.Authorized)
                    sFranchise.CreatedUserType = SEnumUserType.Authorized;
                else if (bFranchise.CreatedUserType == BEnumUserType.CustomerService)
                    sFranchise.CreatedUserType = SEnumUserType.CustomerService;
                else if (bFranchise.CreatedUserType == BEnumUserType.Franchise)
                    sFranchise.CreatedUserType = SEnumUserType.Franchise;
                else if (bFranchise.CreatedUserType == BEnumUserType.Referent)
                    sFranchise.CreatedUserType = SEnumUserType.Referent;
                else
                    sFranchise.CreatedUserType = SEnumUserType.Administrator;
                
                sFranchise.CompanyName  = bFranchise.CompanyName;
                sFranchise.Name = bFranchise.Name;
                sFranchise.LegalForm = bFranchise.LegalForm;
                sFranchise.CommercialName = bFranchise.CommercialName;
                sFranchise.ManPower = bFranchise.ManPower;
                sFranchise.TelephoneNo = bFranchise.TelephoneNo;
                sFranchise.FaxNo = bFranchise.FaxNo;
                sFranchise.RegistrationNo = bFranchise.RegistrationNo;
                sFranchise.Address1 = bFranchise.Address1;
                sFranchise.Address2 = bFranchise.Address2;
                sFranchise.Address3 = bFranchise.Address3;
                sFranchise.City = bFranchise.City;
                sFranchise.ZipCode = bFranchise.ZipCode;
                sFranchise.Country = bFranchise.Country;
                sFranchise.AssignedZone = bFranchise.AssignedZone;
                sFranchise.CreatedDate = bFranchise.CreatedDate;
                sFranchise.LastUpdate = bFranchise.LastUpdate;

                if (bFranchise.CustomerType == BEnumCustomerType.KeyCustomer)
                    sFranchise.CustomerType = SEnumCustomerType.KeyCustomer;

                else if (bFranchise.CustomerType == BEnumCustomerType.RegularCustomer)
                    sFranchise.CustomerType = SEnumCustomerType.RegularCustomer;
                else
                    sFranchise.CustomerType = SEnumCustomerType.RegularCustomer;

                if (bFranchise.CustomerTypeChanged == BEnumFlag.Yes)
                    sFranchise.CustomerTypeChanged = SEnumFlag.Yes;
                else if (bFranchise.CustomerTypeChanged == BEnumFlag.No)
                    sFranchise.CustomerTypeChanged = SEnumFlag.No;
                else
                    sFranchise.CustomerTypeChanged = SEnumFlag.No;

                sFranchise.ScannedDoc = bFranchise.ScannedDoc;
                sFranchise.VatNo = bFranchise.VatNo;
                sFranchise.SiretNo = bFranchise.SiretNo;
                sFranchise.PaymentDelay = bFranchise.PaymentDelay;
                sFranchise.FirmCreationDate = bFranchise.FirmCreationDate;
                sFranchise.CarrierAccountRef = bFranchise.CarrierAccountRef;
              }
              return sFranchise;
          }

          /// <summary>
          /// Converted Service Entiry  to Business entity for Franchise entity
          /// </summary>
          /// <param name="SFranchise"></param>
          /// <returns> BFranchise </returns>
          /// 
          public BFranchise ConvertStoB_Franchise(SFranchise sFranchise)
          { 
            BFranchise bFranchise = new BFranchise();

            if (sFranchise != null)
            {
                bFranchise.AccountNo = sFranchise.AccountNo;
                bFranchise.Email = sFranchise.Email;
                bFranchise.Password = sFranchise.Password;

                if (sFranchise.UserType == SEnumUserType.Authorized)
                    bFranchise.UserType = BEnumUserType.Authorized;
                else if (sFranchise.UserType == SEnumUserType.Administrator)
                    bFranchise.UserType = BEnumUserType.Administrator;
                else if (sFranchise.UserType == SEnumUserType.CustomerService)
                    bFranchise.UserType = BEnumUserType.CustomerService;
                else if (sFranchise.UserType == SEnumUserType.Franchise)
                    bFranchise.UserType = BEnumUserType.Franchise;
                else if (sFranchise.UserType == SEnumUserType.Referent)
                    bFranchise.UserType = BEnumUserType.Referent;
                else
                    bFranchise.UserType = BEnumUserType.Authorized;

                if (sFranchise.Status == SEnumUserStatus.Archived)
                    bFranchise.Status = BEnumUserStatus.Archived;
                else if (sFranchise.Status == SEnumUserStatus.BeingCreated)
                    bFranchise.Status = BEnumUserStatus.BeingCreated;
                else if (sFranchise.Status == SEnumUserStatus.Disabled)
                    bFranchise.Status = BEnumUserStatus.Disabled;
                else if (sFranchise.Status == SEnumUserStatus.Enabled)
                    bFranchise.Status = BEnumUserStatus.Enabled;
                else
                    bFranchise.Status = BEnumUserStatus.Disabled;
                

                if (sFranchise.IsSalesTarrifAssigned == SEnumFlag.Yes)
                    bFranchise.IsSalesTarrifAssigned = BEnumFlag.Yes;
                else if (sFranchise.IsSalesTarrifAssigned == SEnumFlag.No)
                    bFranchise.IsSalesTarrifAssigned = BEnumFlag.No;
                else
                    bFranchise.IsSalesTarrifAssigned = BEnumFlag.No;

                if (sFranchise.IsToSAccepted == SEnumFlag.Yes)
                    bFranchise.IsToSAccepted = BEnumFlag.Yes;
                else if (sFranchise.IsToSAccepted == SEnumFlag.No)
                    bFranchise.IsToSAccepted = BEnumFlag.No;
                else
                    bFranchise.IsToSAccepted = BEnumFlag.No;

                bFranchise.ToSAcceptedDate = sFranchise.ToSAcceptedDate;
                bFranchise.Language = sFranchise.Language;
                bFranchise.LastLogin = sFranchise.LastLogin;

                if (sFranchise.IsChangePasswordRequired == SEnumFlag.Yes)
                    bFranchise.IsChangePasswordRequired = BEnumFlag.Yes;
                else if (sFranchise.IsChangePasswordRequired == SEnumFlag.No)
                    bFranchise.IsChangePasswordRequired = BEnumFlag.No;
                else
                    bFranchise.IsChangePasswordRequired = BEnumFlag.Yes;

                bFranchise.CreatedBy = sFranchise.CreatedBy;

                if (sFranchise.CreatedUserType == SEnumUserType.Administrator)
                    bFranchise.CreatedUserType = BEnumUserType.Administrator;
                else if (sFranchise.CreatedUserType == SEnumUserType.Authorized)
                    bFranchise.CreatedUserType = BEnumUserType.Authorized;
                else if (sFranchise.CreatedUserType == SEnumUserType.CustomerService)
                    bFranchise.CreatedUserType = BEnumUserType.CustomerService;
                else if (sFranchise.CreatedUserType == SEnumUserType.Franchise)
                    bFranchise.CreatedUserType = BEnumUserType.Franchise;
                else if (sFranchise.CreatedUserType == SEnumUserType.Referent)
                    bFranchise.CreatedUserType = BEnumUserType.Referent;
                else
                    bFranchise.CreatedUserType = BEnumUserType.Administrator;
                
                bFranchise.CompanyName  = sFranchise.CompanyName;
                bFranchise.Name = sFranchise.Name;
                bFranchise.LegalForm = sFranchise.LegalForm;
                bFranchise.CommercialName = sFranchise.CommercialName;
                bFranchise.ManPower = sFranchise.ManPower;
                bFranchise.TelephoneNo = sFranchise.TelephoneNo;
                bFranchise.FaxNo = sFranchise.FaxNo;
                bFranchise.RegistrationNo = sFranchise.RegistrationNo;
                bFranchise.Address1 = sFranchise.Address1;
                bFranchise.Address2 = sFranchise.Address2;
                bFranchise.Address3 = sFranchise.Address3;
                bFranchise.City = sFranchise.City;
                bFranchise.ZipCode = sFranchise.ZipCode;
                bFranchise.Country = sFranchise.Country;
                bFranchise.AssignedZone = sFranchise.AssignedZone;
                bFranchise.CreatedDate = sFranchise.CreatedDate;
                bFranchise.LastUpdate = sFranchise.LastUpdate;

                if (sFranchise.CustomerType == SEnumCustomerType.KeyCustomer)
                    bFranchise.CustomerType = BEnumCustomerType.KeyCustomer;
                else if (sFranchise.CustomerType == SEnumCustomerType.RegularCustomer)
                    bFranchise.CustomerType = BEnumCustomerType.RegularCustomer;
                else
                    bFranchise.CustomerType = BEnumCustomerType.RegularCustomer;

                if (sFranchise.CustomerTypeChanged == SEnumFlag.Yes)
                    bFranchise.CustomerTypeChanged = BEnumFlag.Yes;
                else if (sFranchise.CustomerTypeChanged == SEnumFlag.No)
                    bFranchise.CustomerTypeChanged = BEnumFlag.No;
                else
                    bFranchise.CustomerTypeChanged = BEnumFlag.No;

                bFranchise.ScannedDoc = sFranchise.ScannedDoc;
                bFranchise.VatNo = sFranchise.VatNo;
                bFranchise.SiretNo = sFranchise.SiretNo;
                bFranchise.PaymentDelay = sFranchise.PaymentDelay;
                bFranchise.FirmCreationDate = sFranchise.FirmCreationDate;
                bFranchise.CarrierAccountRef = sFranchise.CarrierAccountRef;
              }
              return bFranchise;
          }

          /// <summary>
          /// Converted Business Entiry  to Service entity for Franchise contact entity list
          /// </summary>
          /// <param name="BFranchiseContact"></param>
          /// <returns> SFranchiseContact </returns>
          /// 
          public List<SFranchiseContact> ConvertBtoS_FranchiseContactList(List<BFranchiseContact> bFranchiseContact)
          {
              List<SFranchiseContact> sFranchiseContact = new List<SFranchiseContact>();
              for (int i = 0; i < bFranchiseContact.Count; i++)
              {
                  sFranchiseContact.Add(ConvertBtoS_FranchiseContact(bFranchiseContact[i]));
              }
              return sFranchiseContact;
          }

          public SFranchiseContact ConvertBtoS_FranchiseContact(BFranchiseContact bFranchise)
          {
              SFranchiseContact sFranchise = new SFranchiseContact();

              sFranchise.CompanyName = bFranchise.CompanyName;
              sFranchise.Email = bFranchise.Email;
              sFranchise.Name = bFranchise.Name;
              sFranchise.Language = bFranchise.Language;

              return sFranchise;

          
          }

          #endregion

       #region Customer Entity

          public SCustomer ConvertBtoS_Customer(BCustomer bCustomer)
          {
              SCustomer sCustomer = new SCustomer();

              if (bCustomer != null)
              {
                  sCustomer.AccountNo = bCustomer.AccountNo;
                  sCustomer.Email = bCustomer.Email;
                  sCustomer.Password = bCustomer.Password;

                  if (bCustomer.UserType == BEnumUserType.Authorized)
                      sCustomer.UserType = SEnumUserType.Authorized;
                  else if (bCustomer.UserType == BEnumUserType.Administrator)
                      sCustomer.UserType = SEnumUserType.Administrator;
                  else if (bCustomer.UserType == BEnumUserType.CustomerService)
                      sCustomer.UserType = SEnumUserType.CustomerService;
                  else if (bCustomer.UserType == BEnumUserType.Franchise)
                      sCustomer.UserType = SEnumUserType.Franchise;
                  else if (bCustomer.UserType == BEnumUserType.Referent)
                      sCustomer.UserType = SEnumUserType.Referent;
                  else
                      sCustomer.UserType = SEnumUserType.Referent;

                  if (bCustomer.Status == BEnumUserStatus.Archived)
                      sCustomer.Status = SEnumUserStatus.Archived;
                  else if (bCustomer.Status == BEnumUserStatus.BeingCreated)
                      sCustomer.Status = SEnumUserStatus.BeingCreated;
                  else if (bCustomer.Status == BEnumUserStatus.Disabled)
                      sCustomer.Status = SEnumUserStatus.Disabled;
                  else if (bCustomer.Status == BEnumUserStatus.Enabled)
                      sCustomer.Status = SEnumUserStatus.Enabled;
                  else
                      sCustomer.Status = SEnumUserStatus.BeingCreated;

                  if (bCustomer.IsSalesTarrifAssigned == BEnumFlag.Yes)
                      sCustomer.IsSalesTarrifAssigned = SEnumFlag.Yes;
                  else if (bCustomer.IsSalesTarrifAssigned == BEnumFlag.No)
                      sCustomer.IsSalesTarrifAssigned = SEnumFlag.No;
                  else
                      sCustomer.IsSalesTarrifAssigned = SEnumFlag.No;

                  if (bCustomer.IsToSAccepted == BEnumFlag.Yes)
                      sCustomer.IsToSAccepted = SEnumFlag.Yes;
                  else if (bCustomer.IsToSAccepted == BEnumFlag.No)
                      sCustomer.IsToSAccepted = SEnumFlag.No;
                  else
                      sCustomer.IsToSAccepted = SEnumFlag.No;

                  sCustomer.ToSAcceptedDate = bCustomer.ToSAcceptedDate;
                  sCustomer.Language = bCustomer.Language;
                  sCustomer.LastLogin = bCustomer.LastLogin;

                  if (bCustomer.IsChangePasswordRequired == BEnumFlag.Yes)
                      sCustomer.IsChangePasswordRequired = SEnumFlag.Yes;
                  else if (bCustomer.IsChangePasswordRequired == BEnumFlag.No)
                      sCustomer.IsChangePasswordRequired = SEnumFlag.No;
                  else
                      sCustomer.IsChangePasswordRequired = SEnumFlag.No;

                  sCustomer.CreatedBy = bCustomer.CreatedBy;

                  if (bCustomer.CreatedUserType == BEnumUserType.Administrator)
                      sCustomer.CreatedUserType = SEnumUserType.Administrator;
                  else if (bCustomer.CreatedUserType == BEnumUserType.Authorized)
                      sCustomer.CreatedUserType = SEnumUserType.Authorized;
                  else if (bCustomer.CreatedUserType == BEnumUserType.CustomerService)
                      sCustomer.CreatedUserType = SEnumUserType.CustomerService;
                  else if (bCustomer.CreatedUserType == BEnumUserType.Franchise)
                      sCustomer.CreatedUserType = SEnumUserType.Franchise;
                  else if (bCustomer.CreatedUserType == BEnumUserType.Referent)
                      sCustomer.CreatedUserType = SEnumUserType.Referent;
                  else
                      sCustomer.CreatedUserType = SEnumUserType.Administrator;

                  sCustomer.CompanyName = bCustomer.CompanyName;
                  sCustomer.Name = bCustomer.Name;
                  sCustomer.Designation = bCustomer.Designation;
                  sCustomer.TelephoneNo = bCustomer.TelephoneNo;
                  sCustomer.HqZipcode = bCustomer.HqZipcode;
                  sCustomer.Country = bCustomer.Country;

                  if (bCustomer.IsKeyAccount == BEnumFlag.Yes)
                      sCustomer.IsKeyAccount = SEnumFlag.Yes;
                  else if (bCustomer.IsKeyAccount == BEnumFlag.No)
                      sCustomer.IsKeyAccount = SEnumFlag.No;
                  else
                      sCustomer.IsKeyAccount = SEnumFlag.No;

                  if (bCustomer.CustomerCategory == BEnumCustCategory.A)
                      sCustomer.CustomerCategory = SEnumCustCategory.A;
                  else if (bCustomer.CustomerCategory == BEnumCustCategory.B)
                      sCustomer.CustomerCategory = SEnumCustCategory.B;
                  else if (bCustomer.CustomerCategory == BEnumCustCategory.C)
                      sCustomer.CustomerCategory = SEnumCustCategory.C;
                  else
                      sCustomer.CustomerCategory = SEnumCustCategory.C;

                  sCustomer.ChiefContact = bCustomer.ChiefContact;
                  sCustomer.IndustryType = bCustomer.IndustryType;
                  sCustomer.LegalForm = bCustomer.LegalForm;
                  sCustomer.ContactName = bCustomer.ContactName;
                  sCustomer.InvoicePhoneNumber = bCustomer.InvoicePhoneNumber;
                  sCustomer.InvoiceFaxNo = bCustomer.InvoiceFaxNo;
                  sCustomer.InvoiceAddress1 = bCustomer.InvoiceAddress1;
                  sCustomer.InvoiceAddress2 = bCustomer.InvoiceAddress2;
                  sCustomer.InvoiceAddress3 = bCustomer.InvoiceAddress3;
                  sCustomer.InvoiceZipcode = bCustomer.InvoiceZipcode;
                  sCustomer.InvoiceCity = bCustomer.InvoiceCity;
                  sCustomer.InvoiceCountry = bCustomer.InvoiceCountry;

                  if (bCustomer.UsedForShipping == BEnumFlag.Yes)
                      sCustomer.UsedForShipping = SEnumFlag.Yes;
                  else if (bCustomer.UsedForShipping == BEnumFlag.No)
                      sCustomer.UsedForShipping = SEnumFlag.No;
                  else
                      sCustomer.UsedForShipping = SEnumFlag.No;

                  if (bCustomer.UsedForReturn == BEnumFlag.Yes)
                      sCustomer.UsedForReturn = SEnumFlag.Yes;
                  else if (bCustomer.UsedForReturn == BEnumFlag.No)
                      sCustomer.UsedForReturn = SEnumFlag.No;
                  else
                      sCustomer.UsedForReturn = SEnumFlag.No;

                  //if (bCustomer.ShipmentPreference == BEnumShipPreference.MostCompetitive)
                  //    sCustomer.ShipmentPreference = SEnumShipPreference.MostCompetitive;
                  //else if (bCustomer.ShipmentPreference == BEnumShipPreference.Fastest)
                  //    sCustomer.ShipmentPreference = SEnumShipPreference.Fastest;
                  //else if (bCustomer.ShipmentPreference == BEnumShipPreference.NamedCarrier)
                  //    sCustomer.ShipmentPreference = SEnumShipPreference.NamedCarrier;
                  //else
                  //    sCustomer.ShipmentPreference = SEnumShipPreference.MostCompetitive;
                  sCustomer.ShipmentPreference = bCustomer.ShipmentPreference;
                  if (bCustomer.PaymentMethod == BEnumPaymentType.CreditCard)
                      sCustomer.PaymentMethod = SEnumPaymentType.CreditCard;
                  else if (bCustomer.PaymentMethod == BEnumPaymentType.DeferredPayment)
                      sCustomer.PaymentMethod = SEnumPaymentType.DeferredPayment;
                  else
                      sCustomer.PaymentMethod = SEnumPaymentType.DeferredPayment;

                  if (bCustomer.DeferedPaymentRequired == BEnumFlag.Yes)
                      sCustomer.DeferedPaymentRequired = SEnumFlag.Yes;
                  else if (bCustomer.DeferedPaymentRequired == BEnumFlag.No)
                      sCustomer.DeferedPaymentRequired = SEnumFlag.No;
                  else
                      sCustomer.DeferedPaymentRequired = SEnumFlag.No;

                  sCustomer.BudgetAmount = bCustomer.BudgetAmount;

                  if (bCustomer.IsDeferredPaymentAgreed == BEnumFlag.Yes)
                      sCustomer.IsDeferredPaymentAgreed = SEnumFlag.Yes;
                  else if (bCustomer.IsDeferredPaymentAgreed == BEnumFlag.No)
                      sCustomer.IsDeferredPaymentAgreed = SEnumFlag.No;
                  else
                      sCustomer.IsDeferredPaymentAgreed = SEnumFlag.No;

                  sCustomer.WishedBudgetAmount = bCustomer.WishedBudgetAmount;
                  sCustomer.AdministratorUserId = bCustomer.AdministratorUserId;

                  if (bCustomer.CustomerType == BEnumCustomerType.RegularCustomer)
                      sCustomer.CustomerType = SEnumCustomerType.RegularCustomer;
                  else if (bCustomer.CustomerType == BEnumCustomerType.KeyCustomer)
                      sCustomer.CustomerType = SEnumCustomerType.KeyCustomer;
                  else
                      sCustomer.CustomerType = SEnumCustomerType.RegularCustomer;

                  if (bCustomer.CustomerTypeChanged == BEnumFlag.Yes)
                      sCustomer.CustomerTypeChanged = SEnumFlag.Yes;
                  else if (bCustomer.CustomerTypeChanged == BEnumFlag.No)
                      sCustomer.CustomerTypeChanged = SEnumFlag.No;
                  else
                      sCustomer.CustomerTypeChanged = SEnumFlag.No;

                  sCustomer.InsuredCreditAmount = bCustomer.InsuredCreditAmount;
                  sCustomer.PaymentDelayDays = bCustomer.PaymentDelayDays;
                  sCustomer.PaymentDelayMonth = bCustomer.PaymentDelayMonth;
                  sCustomer.CompensationRate = bCustomer.CompensationRate;
                  sCustomer.CompensationAmount = bCustomer.CompensationAmount;
                  sCustomer.DepositAmount = bCustomer.DepositAmount;
                  sCustomer.AuthorizedCreditLimit = bCustomer.AuthorizedCreditLimit;
                  sCustomer.AuthorizedCreditAmount = bCustomer.AuthorizedCreditAmount;
                  sCustomer.CarrierAccountReference = bCustomer.CarrierAccountReference;
                  sCustomer.AvailableCredit = bCustomer.AvailableCredit;
                  sCustomer.CreatedDate = bCustomer.CreatedDate;
                  sCustomer.LastUpdate = bCustomer.LastUpdate;
                  sCustomer.ManPower = bCustomer.ManPower;
                  sCustomer.VatNo = bCustomer.VatNo;
                  sCustomer.SiretNo = bCustomer.SiretNo;
                  sCustomer.GrossMargin = bCustomer.GrossMargin;
                  //16FEB12SM
                  sCustomer.DEFERED_PAYMENT_TYPE = bCustomer.DEFERED_PAYMENT_TYPE;
                  sCustomer.KEY_CARRIER = bCustomer.KEY_CARRIER;
                  sCustomer.SUBSCRIPTION_AMOUNT = bCustomer.SUBSCRIPTION_AMOUNT;
                  sCustomer.EXTRA_INFO = bCustomer.EXTRA_INFO;
                  if (bCustomer.FICTIVE_ACCOUNT == BEnumFlag.Yes)
                      sCustomer.FICTIVE_ACCOUNT = SEnumFlag.Yes;
                  else
                      sCustomer.FICTIVE_ACCOUNT = SEnumFlag.No;
                  sCustomer.TurnOver = bCustomer.TurnOver;
                  sCustomer.ADV = bCustomer.ADV;
                  sCustomer.CarrierName = bCustomer.CarrierName;
                  sCustomer.InsuredMethod = bCustomer.InsuredMethod;
                  sCustomer.FirmDate = bCustomer.FirmDate; //21-FEB-2012 HV
              }
              return sCustomer;
          }

          public List<SCustomer> ConvertBtoS_CustomerList(List<BCustomer> bCustomerList)
          {
              List<SCustomer> sCustomerList = new List<SCustomer>();
              for (int i = 0; i < bCustomerList.Count; i++)
              {
                  sCustomerList.Add(ConvertBtoS_Customer(bCustomerList[i]));
              }
              return sCustomerList;
          }

          public BCustomer ConvertStoB_Customer(SCustomer sCustomer)
          {
              BCustomer bCustomer = new BCustomer();

              if (sCustomer != null)
              {
                  bCustomer.AccountNo = sCustomer.AccountNo;
                  bCustomer.Email = sCustomer.Email;
                  bCustomer.Password = sCustomer.Password;

                  if (sCustomer.UserType == SEnumUserType.Authorized)
                      bCustomer.UserType = BEnumUserType.Authorized;
                  else if (sCustomer.UserType == SEnumUserType.Administrator)
                      bCustomer.UserType = BEnumUserType.Administrator;
                  else if (sCustomer.UserType == SEnumUserType.CustomerService)
                      bCustomer.UserType = BEnumUserType.CustomerService;
                  else if (sCustomer.UserType == SEnumUserType.Franchise)
                      bCustomer.UserType = BEnumUserType.Franchise;
                  else if (sCustomer.UserType == SEnumUserType.Referent)
                      bCustomer.UserType = BEnumUserType.Referent;
                  else
                      bCustomer.UserType = BEnumUserType.Referent;

                  if (sCustomer.Status == SEnumUserStatus.Archived)
                      bCustomer.Status = BEnumUserStatus.Archived;
                  else if (sCustomer.Status == SEnumUserStatus.BeingCreated)
                      bCustomer.Status = BEnumUserStatus.BeingCreated;
                  else if (sCustomer.Status == SEnumUserStatus.Disabled)
                      bCustomer.Status = BEnumUserStatus.Disabled;
                  else if (sCustomer.Status == SEnumUserStatus.Enabled)
                      bCustomer.Status = BEnumUserStatus.Enabled;
                  else
                      bCustomer.Status = BEnumUserStatus.BeingCreated;

                  if (sCustomer.IsSalesTarrifAssigned == SEnumFlag.Yes)
                      bCustomer.IsSalesTarrifAssigned = BEnumFlag.Yes;
                  else if (sCustomer.IsSalesTarrifAssigned == SEnumFlag.No)
                      bCustomer.IsSalesTarrifAssigned = BEnumFlag.No;
                  else
                      bCustomer.IsSalesTarrifAssigned = BEnumFlag.No;

                  if (sCustomer.IsToSAccepted == SEnumFlag.Yes)
                      bCustomer.IsToSAccepted = BEnumFlag.Yes;
                  else if (sCustomer.IsToSAccepted == SEnumFlag.No)
                      bCustomer.IsToSAccepted = BEnumFlag.No;
                  else
                      bCustomer.IsToSAccepted = BEnumFlag.No;

                  bCustomer.ToSAcceptedDate = sCustomer.ToSAcceptedDate;
                  bCustomer.Language = sCustomer.Language;
                  bCustomer.LastLogin = sCustomer.LastLogin;

                  if (sCustomer.IsChangePasswordRequired == SEnumFlag.Yes)
                      bCustomer.IsChangePasswordRequired = BEnumFlag.Yes;
                  else if (sCustomer.IsChangePasswordRequired == SEnumFlag.No)
                      bCustomer.IsChangePasswordRequired = BEnumFlag.No;
                  else
                      bCustomer.IsChangePasswordRequired = BEnumFlag.No;

                  bCustomer.CreatedBy = sCustomer.CreatedBy;

                  if (sCustomer.CreatedUserType == SEnumUserType.Administrator)
                      bCustomer.CreatedUserType = BEnumUserType.Administrator;
                  else if (sCustomer.CreatedUserType == SEnumUserType.Authorized)
                      bCustomer.CreatedUserType = BEnumUserType.Authorized;
                  else if (sCustomer.CreatedUserType == SEnumUserType.CustomerService)
                      bCustomer.CreatedUserType = BEnumUserType.CustomerService;
                  else if (sCustomer.CreatedUserType == SEnumUserType.Franchise)
                      bCustomer.CreatedUserType = BEnumUserType.Franchise;
                  else if (sCustomer.CreatedUserType == SEnumUserType.Referent)
                      bCustomer.CreatedUserType = BEnumUserType.Referent;
                  else
                      bCustomer.CreatedUserType = BEnumUserType.Administrator;

                  bCustomer.CompanyName = sCustomer.CompanyName;
                  bCustomer.Name = sCustomer.Name;
                  bCustomer.Designation = sCustomer.Designation;
                  bCustomer.TelephoneNo = sCustomer.TelephoneNo;
                  bCustomer.HqZipcode = sCustomer.HqZipcode;
                  bCustomer.Country = sCustomer.Country;

                  if (sCustomer.IsKeyAccount == SEnumFlag.Yes)
                      bCustomer.IsKeyAccount = BEnumFlag.Yes;
                  else if (sCustomer.IsKeyAccount == SEnumFlag.No)
                      bCustomer.IsKeyAccount = BEnumFlag.No;
                  else
                      bCustomer.IsKeyAccount = BEnumFlag.No;

                  if (sCustomer.CustomerCategory == SEnumCustCategory.A)
                      bCustomer.CustomerCategory = BEnumCustCategory.A;
                  else if (sCustomer.CustomerCategory == SEnumCustCategory.B)
                      bCustomer.CustomerCategory = BEnumCustCategory.B;
                  else if (sCustomer.CustomerCategory == SEnumCustCategory.C)
                      bCustomer.CustomerCategory = BEnumCustCategory.C;
                  else
                      bCustomer.CustomerCategory = BEnumCustCategory.A;

                  bCustomer.ChiefContact = sCustomer.ChiefContact;
                  bCustomer.IndustryType = sCustomer.IndustryType;
                  bCustomer.LegalForm = sCustomer.LegalForm;
                  bCustomer.ContactName = sCustomer.ContactName;
                  bCustomer.InvoicePhoneNumber = sCustomer.InvoicePhoneNumber;
                  bCustomer.InvoiceFaxNo = sCustomer.InvoiceFaxNo;
                  bCustomer.InvoiceAddress1 = sCustomer.InvoiceAddress1;
                  bCustomer.InvoiceAddress2 = sCustomer.InvoiceAddress2;
                  bCustomer.InvoiceAddress3 = sCustomer.InvoiceAddress3;
                  bCustomer.InvoiceZipcode = sCustomer.InvoiceZipcode;
                  bCustomer.InvoiceCity = sCustomer.InvoiceCity;
                  bCustomer.InvoiceCountry = sCustomer.InvoiceCountry;

                  if (sCustomer.UsedForShipping == SEnumFlag.Yes)
                      bCustomer.UsedForShipping = BEnumFlag.Yes;
                  else if (sCustomer.UsedForShipping == SEnumFlag.No)
                      bCustomer.UsedForShipping = BEnumFlag.No;
                  else
                      bCustomer.UsedForShipping = BEnumFlag.No;

                  if (sCustomer.UsedForReturn == SEnumFlag.Yes)
                      bCustomer.UsedForReturn = BEnumFlag.Yes;
                  else if (sCustomer.UsedForReturn == SEnumFlag.No)
                      bCustomer.UsedForReturn = BEnumFlag.No;
                  else
                      bCustomer.UsedForReturn = BEnumFlag.No;

                  //if (sCustomer.ShipmentPreference == SEnumShipPreference.MostCompetitive)
                  //    bCustomer.ShipmentPreference = BEnumShipPreference.MostCompetitive;
                  //else if (sCustomer.ShipmentPreference == SEnumShipPreference.Fastest)
                  //    bCustomer.ShipmentPreference = BEnumShipPreference.Fastest;
                  //else if (sCustomer.ShipmentPreference == SEnumShipPreference.NamedCarrier)
                  //    bCustomer.ShipmentPreference = BEnumShipPreference.NamedCarrier;
                  //else
                  //    bCustomer.ShipmentPreference = BEnumShipPreference.NamedCarrier;

                  bCustomer.ShipmentPreference = sCustomer.ShipmentPreference;

                  if (sCustomer.PaymentMethod == SEnumPaymentType.CreditCard)
                      bCustomer.PaymentMethod = BEnumPaymentType.CreditCard;
                  else if (sCustomer.PaymentMethod == SEnumPaymentType.DeferredPayment)
                      bCustomer.PaymentMethod = BEnumPaymentType.DeferredPayment;
                  else
                      bCustomer.PaymentMethod = BEnumPaymentType.DeferredPayment;

                  if (sCustomer.DeferedPaymentRequired == SEnumFlag.Yes)
                      bCustomer.DeferedPaymentRequired = BEnumFlag.Yes;
                  else if (sCustomer.DeferedPaymentRequired == SEnumFlag.No)
                      bCustomer.DeferedPaymentRequired = BEnumFlag.No;
                  else
                      bCustomer.DeferedPaymentRequired = BEnumFlag.No;

                  bCustomer.BudgetAmount = sCustomer.BudgetAmount;

                  if (sCustomer.IsDeferredPaymentAgreed == SEnumFlag.Yes)
                      bCustomer.IsDeferredPaymentAgreed = BEnumFlag.Yes;
                  else if (sCustomer.IsDeferredPaymentAgreed == SEnumFlag.No)
                      bCustomer.IsDeferredPaymentAgreed = BEnumFlag.No;
                  else
                      bCustomer.IsDeferredPaymentAgreed = BEnumFlag.No;

                  bCustomer.WishedBudgetAmount = sCustomer.WishedBudgetAmount;
                  bCustomer.AdministratorUserId = sCustomer.AdministratorUserId;

                  if (sCustomer.CustomerType == SEnumCustomerType.RegularCustomer)
                      bCustomer.CustomerType = BEnumCustomerType.RegularCustomer;
                  else if (sCustomer.CustomerType == SEnumCustomerType.KeyCustomer)
                      bCustomer.CustomerType = BEnumCustomerType.KeyCustomer;
                  else
                      bCustomer.CustomerType = BEnumCustomerType.RegularCustomer;

                  if (sCustomer.CustomerTypeChanged == SEnumFlag.Yes)
                      bCustomer.CustomerTypeChanged = BEnumFlag.Yes;
                  else if (sCustomer.CustomerTypeChanged == SEnumFlag.No)
                      bCustomer.CustomerTypeChanged = BEnumFlag.No;
                  else
                      bCustomer.CustomerTypeChanged = BEnumFlag.No;

                  bCustomer.InsuredCreditAmount = sCustomer.InsuredCreditAmount;
                  bCustomer.PaymentDelayDays = sCustomer.PaymentDelayDays;
                  bCustomer.PaymentDelayMonth = sCustomer.PaymentDelayMonth;
                  bCustomer.CompensationRate = sCustomer.CompensationRate;
                  bCustomer.CompensationAmount = sCustomer.CompensationAmount;
                  bCustomer.DepositAmount = sCustomer.DepositAmount;
                  bCustomer.AuthorizedCreditLimit = sCustomer.AuthorizedCreditLimit;
                  bCustomer.AuthorizedCreditAmount = sCustomer.AuthorizedCreditAmount;
                  bCustomer.CarrierAccountReference = sCustomer.CarrierAccountReference;
                  bCustomer.AvailableCredit = sCustomer.AvailableCredit;
                  bCustomer.CreatedDate = sCustomer.CreatedDate;
                  bCustomer.LastUpdate = sCustomer.LastUpdate;
                  bCustomer.ManPower = sCustomer.ManPower;
                  bCustomer.VatNo = sCustomer.VatNo;
                  bCustomer.SiretNo = sCustomer.SiretNo;
                  bCustomer.GrossMargin = sCustomer.GrossMargin;

                  //16FEB12SM
                  bCustomer.DEFERED_PAYMENT_TYPE = sCustomer.DEFERED_PAYMENT_TYPE;
                  bCustomer.KEY_CARRIER = sCustomer.KEY_CARRIER;
                  bCustomer.SUBSCRIPTION_AMOUNT = sCustomer.SUBSCRIPTION_AMOUNT;
                  bCustomer.EXTRA_INFO = sCustomer.EXTRA_INFO;
                  if (sCustomer.FICTIVE_ACCOUNT == SEnumFlag.Yes)
                      bCustomer.FICTIVE_ACCOUNT = BEnumFlag.Yes;
                  else
                      bCustomer.FICTIVE_ACCOUNT = BEnumFlag.No;
                  bCustomer.TurnOver = sCustomer.TurnOver;

                  bCustomer.ADV = sCustomer.ADV;
                  bCustomer.CarrierName = sCustomer.CarrierName;
                  bCustomer.InsuredMethod = sCustomer.InsuredMethod;
                  bCustomer.FirmDate = sCustomer.FirmDate; //21-FEB-2012 HV
              }
              return bCustomer;



          }

          public BAdv ConvertStoB_Adv(SAdv sAdv)
          {
              BAdv bAdv = new BAdv();
              if (sAdv != null)
              {
                  bAdv.ACCOUNT_NO = sAdv.ACCOUNT_NO;
                  bAdv.AVERAGE_VALUE = sAdv.AVERAGE_VALUE;
                  bAdv.AVERAGE_WEIGHT = sAdv.AVERAGE_WEIGHT;
                  bAdv.CARRIER_NAME = sAdv.CARRIER_NAME;
                  bAdv.DISCOUNT = sAdv.DISCOUNT;
                  bAdv.PACKAGE_TYPE = sAdv.PACKAGE_TYPE;
                  bAdv.PRIORITY = sAdv.PRIORITY;
                  bAdv.SHIPMENT_TYPE = sAdv.SHIPMENT_TYPE;

              }
              return bAdv;
          }

          public List<SAdv> ConvertBtoS_AdvList(List<BAdv> bAdv)
          {
              List<SAdv> sAdv = new List<SAdv>();
              for (int i = 0; i < bAdv.Count; i++)
              {
                  sAdv.Add(ConvertBtoS_Adv(bAdv[i]));
              }
              return sAdv;
          }

          public SAdv ConvertBtoS_Adv(BAdv bAdv)
          {
              SAdv sAdv = new SAdv();
              if (bAdv != null)
              {
                  sAdv.ACCOUNT_NO = bAdv.ACCOUNT_NO;
                  sAdv.AVERAGE_VALUE = bAdv.AVERAGE_VALUE;
                  sAdv.AVERAGE_WEIGHT = bAdv.AVERAGE_WEIGHT;
                  sAdv.CARRIER_NAME = bAdv.CARRIER_NAME;
                  sAdv.DISCOUNT = bAdv.DISCOUNT;
                  sAdv.PACKAGE_TYPE = bAdv.PACKAGE_TYPE;
                  sAdv.PRIORITY = bAdv.PRIORITY;
                  sAdv.SHIPMENT_TYPE = bAdv.SHIPMENT_TYPE;
              }
              return sAdv;
          }

          public List<BAdv> ConvertStoB_AdvList(List<SAdv> sAdv)
          {
              List<BAdv> bAdv = new List<BAdv>();
              for (int i = 0; i < sAdv.Count; i++)
              {
                  bAdv.Add(ConvertStoB_Adv(sAdv[i]));
              }
              return bAdv;
          }

          #endregion

        #region Authorized entity
          /// <summary>
          /// Converted Business Entiry  to Service entity for Authorized entity
          /// </summary>
          /// <param name="BAuthorized"></param>
          /// <returns> SAuthorized </returns>
          /// 
              public SAuthorized ConvertBtoS_Authorized(BAuthorized bAuthorized)
          {
              SAuthorized sAuthoirzed = new SAuthorized();
              if (bAuthorized != null)
              {
                  sAuthoirzed.AccountNo = bAuthorized.AccountNo;
                  sAuthoirzed.Email = bAuthorized.Email;
                  sAuthoirzed.Password = bAuthorized.Password;

                  if (bAuthorized.UserType == BEnumUserType.Authorized)
                      sAuthoirzed.UserType = SEnumUserType.Authorized;
                  else if (bAuthorized.UserType == BEnumUserType.Administrator)
                      sAuthoirzed.UserType = SEnumUserType.Administrator;
                  else if (bAuthorized.UserType == BEnumUserType.CustomerService)
                      sAuthoirzed.UserType = SEnumUserType.CustomerService;
                  else if (bAuthorized.UserType == BEnumUserType.Franchise)
                      sAuthoirzed.UserType = SEnumUserType.Franchise;
                  else if (bAuthorized.UserType == BEnumUserType.Referent)
                      sAuthoirzed.UserType = SEnumUserType.Referent;
                  else
                      sAuthoirzed.UserType = SEnumUserType.Referent;

                  if (bAuthorized.Status == BEnumUserStatus.Archived)
                      sAuthoirzed.Status = SEnumUserStatus.Archived;
                  else if (bAuthorized.Status == BEnumUserStatus.BeingCreated)
                      sAuthoirzed.Status = SEnumUserStatus.BeingCreated;
                  else if (bAuthorized.Status == BEnumUserStatus.Disabled)
                      sAuthoirzed.Status = SEnumUserStatus.Disabled;
                  else if (bAuthorized.Status == BEnumUserStatus.Enabled)
                      sAuthoirzed.Status = SEnumUserStatus.Enabled;
                  else
                      sAuthoirzed.Status = SEnumUserStatus.BeingCreated;

                  if (bAuthorized.IsSalesTarrifAssigned == BEnumFlag.Yes)
                      sAuthoirzed.IsSalesTarrifAssigned = SEnumFlag.Yes;
                  else if (bAuthorized.IsSalesTarrifAssigned == BEnumFlag.No)
                      sAuthoirzed.IsSalesTarrifAssigned = SEnumFlag.No;
                  else
                      sAuthoirzed.IsSalesTarrifAssigned = SEnumFlag.No;

                  if (bAuthorized.IsToSAccepted == BEnumFlag.Yes)
                      sAuthoirzed.IsToSAccepted = SEnumFlag.Yes;
                  else if (bAuthorized.IsToSAccepted == BEnumFlag.No)
                      sAuthoirzed.IsToSAccepted = SEnumFlag.No;
                  else
                      sAuthoirzed.IsToSAccepted = SEnumFlag.No;

                  sAuthoirzed.ToSAcceptedDate = bAuthorized.ToSAcceptedDate;
                  sAuthoirzed.Language = bAuthorized.Language;
                  sAuthoirzed.LastLogin = bAuthorized.LastLogin;

                  if (bAuthorized.IsChangePasswordRequired == BEnumFlag.Yes)
                      sAuthoirzed.IsChangePasswordRequired = SEnumFlag.Yes;
                  else if (bAuthorized.IsChangePasswordRequired == BEnumFlag.No)
                      sAuthoirzed.IsChangePasswordRequired = SEnumFlag.No;
                  else
                      sAuthoirzed.IsChangePasswordRequired = SEnumFlag.No;

                  sAuthoirzed.CreatedBy = bAuthorized.CreatedBy;

                  if (bAuthorized.CreatedUserType == BEnumUserType.Administrator)
                      sAuthoirzed.CreatedUserType = SEnumUserType.Administrator;
                  else if (bAuthorized.CreatedUserType == BEnumUserType.Authorized)
                      sAuthoirzed.CreatedUserType = SEnumUserType.Authorized;
                  else if (bAuthorized.CreatedUserType == BEnumUserType.CustomerService)
                      sAuthoirzed.CreatedUserType = SEnumUserType.CustomerService;
                  else if (bAuthorized.CreatedUserType == BEnumUserType.Franchise)
                      sAuthoirzed.CreatedUserType = SEnumUserType.Franchise;
                  else if (bAuthorized.CreatedUserType == BEnumUserType.Referent)
                      sAuthoirzed.CreatedUserType = SEnumUserType.Referent;
                  else
                      sAuthoirzed.CreatedUserType = SEnumUserType.Administrator;

                  if (bAuthorized.CustomerType == BEnumCustomerType.RegularCustomer)
                      sAuthoirzed.CustomerType = SEnumCustomerType.RegularCustomer;
                  else if (bAuthorized.CustomerType == BEnumCustomerType.KeyCustomer)
                      sAuthoirzed.CustomerType = SEnumCustomerType.KeyCustomer;
                  else
                      sAuthoirzed.CustomerType = SEnumCustomerType.RegularCustomer;

                  if (bAuthorized.CustomerTypeChanged == BEnumFlag.Yes)
                      sAuthoirzed.CustomerTypeChanged = SEnumFlag.Yes;
                  else if (bAuthorized.CustomerTypeChanged == BEnumFlag.No)
                      sAuthoirzed.CustomerTypeChanged = SEnumFlag.No;
                  else
                      sAuthoirzed.CustomerTypeChanged = SEnumFlag.No;

                  sAuthoirzed.ContactName = bAuthorized.ContactName;
                  sAuthoirzed.TelephoneNo = bAuthorized.TelephoneNo;
                  sAuthoirzed.ReferentUserId = bAuthorized.ReferentUserId;
              }
              return sAuthoirzed;
          }

              /// <summary>
          /// Converted Service entity to Business Entiry for Authorized entity
          /// </summary>
          /// <param name="SAuthorized"></param>
          /// <returns> BAuthorized </returns>
          /// 
              public BAuthorized ConvertStoB_Authorized(SAuthorized sAuthoried)
          {
              BAuthorized bAuthorized = new BAuthorized();
              if (sAuthoried != null)
              {
                  bAuthorized.AccountNo = sAuthoried.AccountNo;
                  bAuthorized.Email = sAuthoried.Email;
                  bAuthorized.Password = sAuthoried.Password;

                  if (sAuthoried.UserType == SEnumUserType.Authorized)
                      bAuthorized.UserType = BEnumUserType.Authorized;
                  else if (sAuthoried.UserType == SEnumUserType.Administrator)
                      bAuthorized.UserType = BEnumUserType.Administrator;
                  else if (sAuthoried.UserType == SEnumUserType.CustomerService)
                      bAuthorized.UserType = BEnumUserType.CustomerService;
                  else if (sAuthoried.UserType == SEnumUserType.Franchise)
                      bAuthorized.UserType = BEnumUserType.Franchise;
                  else if (sAuthoried.UserType == SEnumUserType.Referent)
                      bAuthorized.UserType = BEnumUserType.Referent;
                  else
                      bAuthorized.UserType = BEnumUserType.Referent;

                  if (sAuthoried.Status == SEnumUserStatus.Archived)
                      bAuthorized.Status = BEnumUserStatus.Archived;
                  else if (sAuthoried.Status == SEnumUserStatus.BeingCreated)
                      bAuthorized.Status = BEnumUserStatus.BeingCreated;
                  else if (sAuthoried.Status == SEnumUserStatus.Disabled)
                      bAuthorized.Status = BEnumUserStatus.Disabled;
                  else if (sAuthoried.Status == SEnumUserStatus.Enabled)
                      bAuthorized.Status = BEnumUserStatus.Enabled;
                  else
                      bAuthorized.Status = BEnumUserStatus.BeingCreated;

                  if (sAuthoried.IsSalesTarrifAssigned == SEnumFlag.Yes)
                      bAuthorized.IsSalesTarrifAssigned = BEnumFlag.Yes;
                  else if (sAuthoried.IsSalesTarrifAssigned == SEnumFlag.No)
                      bAuthorized.IsSalesTarrifAssigned = BEnumFlag.No;
                  else
                      bAuthorized.IsSalesTarrifAssigned = BEnumFlag.No;

                  if (sAuthoried.IsToSAccepted == SEnumFlag.Yes)
                      bAuthorized.IsToSAccepted = BEnumFlag.Yes;
                  else if (sAuthoried.IsToSAccepted == SEnumFlag.No)
                      bAuthorized.IsToSAccepted = BEnumFlag.No;
                  else
                      bAuthorized.IsToSAccepted = BEnumFlag.No;

                  bAuthorized.ToSAcceptedDate = sAuthoried.ToSAcceptedDate;
                  bAuthorized.Language = sAuthoried.Language;
                  bAuthorized.LastLogin = sAuthoried.LastLogin;

                  if (sAuthoried.IsChangePasswordRequired == SEnumFlag.Yes)
                      bAuthorized.IsChangePasswordRequired = BEnumFlag.Yes;
                  else if (sAuthoried.IsChangePasswordRequired == SEnumFlag.No)
                      bAuthorized.IsChangePasswordRequired = BEnumFlag.No;
                  else
                      bAuthorized.IsChangePasswordRequired = BEnumFlag.No;

                  bAuthorized.CreatedBy = sAuthoried.CreatedBy;

                  if (sAuthoried.CreatedUserType == SEnumUserType.Administrator)
                      bAuthorized.CreatedUserType = BEnumUserType.Administrator;
                  else if (sAuthoried.CreatedUserType == SEnumUserType.Authorized)
                      bAuthorized.CreatedUserType = BEnumUserType.Authorized;
                  else if (sAuthoried.CreatedUserType == SEnumUserType.CustomerService)
                      bAuthorized.CreatedUserType = BEnumUserType.CustomerService;
                  else if (sAuthoried.CreatedUserType == SEnumUserType.Franchise)
                      bAuthorized.CreatedUserType = BEnumUserType.Franchise;
                  else if (sAuthoried.CreatedUserType == SEnumUserType.Referent)
                      bAuthorized.CreatedUserType = BEnumUserType.Referent;
                  else
                      bAuthorized.CreatedUserType = BEnumUserType.Administrator;

                  if (sAuthoried.CustomerType == SEnumCustomerType.RegularCustomer)
                      bAuthorized.CustomerType = BEnumCustomerType.RegularCustomer;
                  else if (sAuthoried.CustomerType == SEnumCustomerType.KeyCustomer)
                      bAuthorized.CustomerType = BEnumCustomerType.KeyCustomer;
                  else
                      bAuthorized.CustomerType = BEnumCustomerType.RegularCustomer;

                  if (sAuthoried.CustomerTypeChanged == SEnumFlag.Yes)
                      bAuthorized.CustomerTypeChanged = BEnumFlag.Yes;
                  else if (sAuthoried.CustomerTypeChanged == SEnumFlag.No)
                      bAuthorized.CustomerTypeChanged = BEnumFlag.No;
                  else
                      bAuthorized.CustomerTypeChanged = BEnumFlag.No;

                  bAuthorized.ContactName = sAuthoried.ContactName;
                  bAuthorized.TelephoneNo = sAuthoried.TelephoneNo;
                  bAuthorized.ReferentUserId = sAuthoried.ReferentUserId;
              }
              return bAuthorized;
          }

              /// <summary>
          /// Converted list of Service entity to list of Business Entiry for Authorized entity
          /// </summary>
          /// <param name="list of SAuthorized"></param>
          /// <returns> list of BAuthorized </returns>
          /// 
              public List<BAuthorized> ConvertStoB_Authorized(List<SAuthorized> seAuthorized)
          {
              List<BAuthorized> bAuthorized = new List<BAuthorized>();
              for (int i = 0; i < seAuthorized.Count; i++)
              {
                  bAuthorized.Add(ConvertStoB_Authorized(seAuthorized[i]));
              }
              return bAuthorized;
          }

              /// <summary>
          /// Converted list of Business entity to list of Service Entiry for Authorized entity
          /// </summary>
          /// <param name="list of BAuthorized"></param>
          /// <returns> list of SAuthorized </returns>
          /// 
              public List<SAuthorized> ConvertBtoS_Authorized(List<BAuthorized> beAuthorized)
          {
              List<SAuthorized> sAuthorized = new List<SAuthorized>();
              for (int i = 0; i < beAuthorized.Count; i++)
              {
                  sAuthorized.Add(ConvertBtoS_Authorized(beAuthorized[i]));
              }
              return sAuthorized;

          }

#endregion

        #region Functionality

              /// <summary>
              /// Converted Business Entiry  to Service entity for Functionality entity
              /// </summary>
              /// <param name="BAuthorized"></param>
              /// <returns> SFunctionality </returns>
              /// 
              public SFunctionality ConvertBtoS_Functionality(BFunctionality bFunctionality)
              {
                  SFunctionality sFunctionality = new SFunctionality();
                  sFunctionality.FunctionalCode = bFunctionality.FunctionalCode;
                  sFunctionality.FunctionalName = bFunctionality.FunctionalName.Trim();
                  sFunctionality.Description = bFunctionality.Description.Trim();
                  return sFunctionality;
              }

              /// <summary>
              /// Converted Business Entiry  to Service entity for Functionality entity
              /// </summary>
              /// <param name="BAuthorized"></param>
              /// <returns> SFunctionality </returns>
              /// 
              public List<SFunctionality> ConvertBtoS_Functionality(List<BFunctionality> bFunctionality)
              {
                  List<SFunctionality> sFunctionality = new List<SFunctionality>();
                  if (bFunctionality != null)
                  {
                      for (int i = 0; i < bFunctionality.Count; i++)
                      {
                          sFunctionality.Add(ConvertBtoS_Functionality(bFunctionality[i]));
                      }
                  }

                  return sFunctionality;
              }

              /// <summary>
              /// Converted Service entity to Business Entiry for Functionality entity
              /// </summary>
              /// <param name="SAuthorized"></param>
              /// <returns> BFunctionality </returns>
              /// 
              public BFunctionality ConvertStoB_Functionality(SFunctionality sFunctionality)
              {
                  BFunctionality bFunctionality = new BFunctionality();
                  bFunctionality.FunctionalCode = sFunctionality.FunctionalCode;
                  bFunctionality.FunctionalName = sFunctionality.FunctionalName.Trim();
                  bFunctionality.Description = sFunctionality.Description.Trim();
                  return bFunctionality;
              }

              /// <summary>
              /// Converted Service entity to Business Entiry for Functionality entity
              /// </summary>
              /// <param name="SAuthorized"></param>
              /// <returns> BFunctionality </returns>
              /// 
              public List<BFunctionality> ConvertStoB_Functionality(List<SFunctionality> sFunctionality)
              {
                  List<BFunctionality> bFunctionality = new List<BFunctionality>();
                  if (sFunctionality != null)
                  {
                      for (int i = 0; i < bFunctionality.Count; i++)
                      {
                          bFunctionality.Add(ConvertStoB_Functionality(sFunctionality[i]));
                      }
                  }
                  return bFunctionality;
              }


              #endregion

        #region Functionality-Profile Mapping

              /// <summary>
              /// Converted Business Entiry  to Service entity for Functionality-Profile entity
              /// </summary>
              /// <param name="BAuthorized"></param>
              /// <returns> SFunctionalProfile </returns>
              /// 
              public SFunctionalProfile ConvertBtoS_FunctionalProfile(BFunctionalProfile bFunctionalProfile)
              {
                  SFunctionalProfile sFunctionalProfile = new SFunctionalProfile();
                  sFunctionalProfile.FunctionalCode = bFunctionalProfile.FunctionalCode;
                  sFunctionalProfile.ProfileCode = bFunctionalProfile.ProfileCode.Trim();
                  return sFunctionalProfile;
              }

              /// <summary>
              /// Converted Business Entiry  to Service entity for Functionality-Profile entity
              /// </summary>
              /// <param name="BAuthorized"></param>
              /// <returns> SFunctionalProfile </returns>
              /// 
              public List<SFunctionalProfile> ConvertBtoS_FunctionalProfile(List<BFunctionalProfile> bFunctionalProfile)
              {
                  List<SFunctionalProfile> sFunctionalProfile = new List<SFunctionalProfile>();
                  if (bFunctionalProfile != null)
                  {
                      for (int i = 0; i < bFunctionalProfile.Count; i++)
                      {
                          sFunctionalProfile.Add(ConvertBtoS_FunctionalProfile(bFunctionalProfile[i]));
                       }
                  }
                  return sFunctionalProfile;
              }

              /// <summary>
              /// Converted Business Entiry  to Service entity for Functionality-Profile entity
              /// </summary>
              /// <param name="BAuthorized"></param>
              /// <returns> SFunctionalProfile </returns>
              /// 
              public BFunctionalProfile ConvertStoB_FunctionalProfile(SFunctionalProfile sFunctionalProfile)
              {
                  BFunctionalProfile bFunctionalProfile = new BFunctionalProfile();
                  bFunctionalProfile.FunctionalCode = sFunctionalProfile.FunctionalCode;
                  bFunctionalProfile.ProfileCode = sFunctionalProfile.ProfileCode.Trim();
                  return bFunctionalProfile;
              }

              /// <summary>
              /// Converted Business Entiry  to Service entity for Functionality-Profile entity
              /// </summary>
              /// <param name="BAuthorized"></param>
              /// <returns> SFunctionalProfile </returns>
              /// 
              public List<BFunctionalProfile> ConvertStoB_FunctionalProfile(List<SFunctionalProfile> sFunctionalProfile)
              {
                  List<BFunctionalProfile> bFunctionalProfile = new List<BFunctionalProfile>();
                  if (sFunctionalProfile != null)
                  {
                      for (int i = 0; i < sFunctionalProfile.Count; i++)
                      {
                          bFunctionalProfile.Add(ConvertStoB_FunctionalProfile(sFunctionalProfile[i]));
                      }
                  }

                  return bFunctionalProfile;
              }
              #endregion

        #region MonthlyFees

        /// <summary>
              /// Converted Business Entiry  to Service entity for Monthly Fee
              /// </summary>
              /// <param name="BMonthlyFee"></param>
              /// <returns> SMonthlyFee </returns>
              /// 
        public SMonthlyFee ConvertBtoS_MonthlyFee(BMonthlyFee bMonthlyFee)
              {
                  SMonthlyFee sMonthlyFee = new SMonthlyFee();

                  sMonthlyFee.AccountNo = bMonthlyFee.AccountNo;
                  sMonthlyFee.AdminAccountNo = bMonthlyFee.AdminAccountNo;
                  sMonthlyFee.FeeRate = bMonthlyFee.FeeRate;
                  sMonthlyFee.ShipmentType = bMonthlyFee.ShipmentType;
                  sMonthlyFee.LastUpdate = bMonthlyFee.LastUpdate;
                  return sMonthlyFee;
              }

        /// <summary>
        /// Converted Service Entiry  to Business  entity for Monthly Fee
        /// </summary>
        /// <param name="SMonthlyFee"></param>
        /// <returns> BMonthlyFee </returns>
        /// 
        public BMonthlyFee ConvertStoB_MonthlyFee(SMonthlyFee sMonthlyFee)
              {
                  BMonthlyFee bMonthlyFee = new BMonthlyFee();

                  bMonthlyFee.AccountNo = sMonthlyFee.AccountNo;
                  bMonthlyFee.AdminAccountNo = sMonthlyFee.AdminAccountNo;
                  bMonthlyFee.FeeRate = sMonthlyFee.FeeRate;
                  bMonthlyFee.ShipmentType = sMonthlyFee.ShipmentType;
                  bMonthlyFee.LastUpdate = sMonthlyFee.LastUpdate;
                  return bMonthlyFee;
              }

        /// <summary>
        /// Converted Business Entiry  to Service entity for Monthly fees list
        /// </summary>
        /// <param name="BAuthorized"></param>
        /// <returns> SFunctionalProfile </returns>
        /// 
        public List<SMonthlyFee> ConvertBtoS_MonthlyFeeList(List<BMonthlyFee> bMonthlyFee)
        {
            List<SMonthlyFee> sMonthlyFee = new List<SMonthlyFee>();
            if (bMonthlyFee != null)
            {
                for (int i = 0; i < bMonthlyFee.Count; i++)
                {
                    sMonthlyFee.Add(ConvertBtoS_MonthlyFee(bMonthlyFee[i]));
                }
            }
            return sMonthlyFee;
        }

#endregion


    }
}
