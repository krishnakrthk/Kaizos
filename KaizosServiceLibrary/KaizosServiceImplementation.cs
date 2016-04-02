using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;

//log4net libraries
using log4net;
using log4net.Config;

//kaizos libraries
using Kaizos.Entities.Business;  //business entity
using KaizosServiceLibrary.Model;  //service entity
using KaizosServiceLibrary.Adapter; //conversion between business and service entity vice versa
using Kaizos.Components.GlobalLibrary; //global general method

//business component
using Kaizos.Components.GlobalLibrary; //global general method
using Kaizos.Components.ToSManager; 
using Kaizos.Components.UserManager;
using Kaizos.Components.TariffManager;
using Kaizos.Components.AddressBookManager;

namespace KaizosServiceLibrary
{
  // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in App.config.
  public partial class KaizosService : IKaizosServiceContract
  {

    ILog logger = log4net.LogManager.GetLogger(typeof(KaizosService));

    /// <summary>
    /// Its dummy method in order to explore data contract [SGeneralFault] to client proxy.
    /// Note: DataContract used in Service implementation only exposed to client proxy.
    /// </summary>
    /// <param name="sGeneralFault"></param> 
    public void DummyFaualContract(SGeneralFault sGeneralFault)
    {
        sGeneralFault.Details = "details";
        sGeneralFault.Issue = "issue";
    }

    #region Tos
        public int InsertToS(SToS sToS)
    {
        int result = 1;

        try
        {
            //1.Convert Service Entity to Business Entity
            ToSAdapter adapter = new ToSAdapter();
            BToS bToS = adapter.ConvertServiceToBusinessEntity(sToS);

            //2. Create instance for business object and invoke method            
            TermsOfService termsOfService = new TermsOfService();
            termsOfService.InsertToS(bToS);
            result = 0;
        }
        catch (Exception error)
        {

            logger.Debug("From service implementation :" + Library.ExtractError(error));
            var generalFault = new SGeneralFault();
            generalFault.Issue = "Problem inserting Terms of Service";
            generalFault.Details = Library.ExtractError(error);
            throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem inserting Terms of Service"));
        }

        return result;

    }
        public int SpInsertTos(SToS sTos)
    {
        int result = 1;

        try
        {
            //1.Convert Service Entity to Business Entity
            ToSAdapter adapter = new ToSAdapter();
            BToS bToS = adapter.ConvertServiceToBusinessEntity(sTos);

            //2. Create instance for business object and invoke method            
            TermsOfService termsOfService = new TermsOfService();
            //termsOfService.InsertToS(bToS);
            result = termsOfService.SpInsertToS(bToS);
        }
        catch (Exception error)
        {

            logger.Debug("From service implementation :" + Library.ExtractError(error));
            var generalFault = new SGeneralFault();
            generalFault.Issue = "Problem inserting Terms of Service";
            generalFault.Details = Library.ExtractError(error);
            throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem inserting Terms of Service"));
        }

        return result;
    }
        public SToS GetActiveToS()
        {
            SToS sTos = new SToS();
            BToS bTos = new BToS();
            try
            {
                //Create instance for business object and invoke method            
                TermsOfService termsOfService = new TermsOfService();
                bTos = termsOfService.GetActiveToS();
                //convert business entities into data contract using "ToSAdapter" 
                ToSAdapter adapter = new ToSAdapter();
                sTos = adapter.ConvertBusinessToServiceEntity(bTos);
                return sTos;
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem selecting the Terms of Service";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem selecting the Terms of Service"));
            }
        }
    #endregion

    #region AddressBook

      /// <summary>
      /// To insert a address details into appropriate table.
      /// </summary>
      /// <param name="sAddressBook"></param>
      /// <returns>
      /// 0 - Success.
      /// 1 - Failure.
      /// 2- Address already exists.
      /// </returns>
      public int InsertAddressBook(SAddressBook sAddressBook)
        {
            int result = 1;

            try
            {
                //1.Convert Service Entity to Business Entity
                CommonAdapter adapter = new CommonAdapter();
                BAddressBook bAddressBook = adapter.ConvertSEAddresstoBEAddress(sAddressBook);


                //2. Create instance for business object and invoke method            
                TermsOfService termsOfService = new TermsOfService();

                Library globalLibrary = new Library();

                result = globalLibrary.InsertAddressBook(bAddressBook);
            }
            catch (Exception error)
            {

                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem inserting Terms of Service";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem inserting Terms of Service"));
            }

            return result;
        }

      /// <summary>
        /// Get all available address list
        /// </summary>
        /// <returns>List of address book</returns>
      public List<SAddressBook> GetAddress()
        {
            List<SAddressBook> sAddressList = new List<SAddressBook>();
            try
            {
                //1.Convert Service Entity to Business Entity
                CommonAdapter adapter = new CommonAdapter();
                BAddressBook bAddressBook = new BAddressBook();
                List<BAddressBook> bAddressList;

                //2. Create instance for business object and invoke method            
                Library globalLibrary = new Library();
                bAddressList = globalLibrary.GetAddress().ToList();

                sAddressList = adapter.ConvertBtoS_AddressBookList(bAddressList);
                return sAddressList;
            }
            catch (Exception error)
            {

                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem inserting Terms of Service";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem inserting Terms of Service"));
            }
        }

      /// <summary>
      /// To get matching address for the strinb comparing
      /// Company Name and Name
      /// </summary>
      /// <param name="SearchString"></param>
      /// <param name="UsedFor"></param>
      /// <returns></returns>
      public List<SAddressBook> GetAddressBookSearch(string SearchString, string UsedFor)
      {
          List<SAddressBook> result = null;

          try
          {
              AddressBookHandler addressBook = new AddressBookHandler();
              CommonAdapter Adapter = new CommonAdapter();

              List<BAddressBook> bAddressBook = addressBook.GetAddressBookSearch(SearchString, UsedFor).ToList();
              result = Adapter.ConvertBtoS_AddressBookList(bAddressBook).ToList();
          }
          catch (Exception error)
          {
              logger.Debug("From service implementation :" + Library.ExtractError(error));
              var generalFault = new SGeneralFault();
              generalFault.Issue = "Problem in fetch customer Designation";
              generalFault.Details = Library.ExtractError(error);
              throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in fetch customer Designation"));
          }
          return result;
      }

      /// <summary>
      /// Update exisitng Address
      /// </summary>
      /// <returns>
      /// 0 =&gt; Sucessfull update
      /// 1 =&gt; Address ID is not exist
      /// 2 =&gt; Invalid data type
      /// </returns>
      public int UpdateAddress(SAddressBook sAddressBook)
      {
          int result = 1;

          try
          {
              //1.Convert Service Entity to Business Entity

              CommonAdapter adapter = new CommonAdapter();

              BAddressBook bAddressBook = adapter.ConvertSEAddresstoBEAddress(sAddressBook);

              //2. Create instance for business object and invoke method            
              AddressBookHandler AddressHandler = new AddressBookHandler();

              result = AddressHandler.UpdateAddress(bAddressBook);
          }
          catch (Exception error)
          {
              logger.Debug("From service implementation :" + Library.ExtractError(error));
              var generalFault = new SGeneralFault();
              generalFault.Issue = "Problem in update the Address information";
              generalFault.Details = Library.ExtractError(error);
              throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in update Address information"));
          }

          return result;

      }

      /// <summary>
      /// Delete an address from Address Book
      /// </summary>
      /// <returns>
      /// 0 =&gt; Sucessfull update
      /// 1 =&gt; Failure
      /// </returns>
      public int DeleteAddress(string AddressID)
      {
          int result = 1;

          try
          {
              AddressBookHandler AddressHandler = new AddressBookHandler();
              result = AddressHandler.DeleteAddress(AddressID);
          }
          catch (Exception error)
          {
              logger.Debug("From service implementation :" + Library.ExtractError(error));
              var generalFault = new SGeneralFault();
              generalFault.Issue = "Problem in update the Address information";
              generalFault.Details = Library.ExtractError(error);
              throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in update Address information"));
          }

          return result;
      }

      /**************24APR12KM*******/
      public List<SAddressBook> GetAddressnew(string SearchString, string UsedFor, string zcode, string scountry)
      {
          List<SAddressBook> result = null;

          try
          {
              AddressBookHandler addressBook = new AddressBookHandler();
              CommonAdapter Adapter = new CommonAdapter();

              List<BAddressBook> bAddressBook = addressBook.GetAddressnew(SearchString, UsedFor, zcode, scountry).ToList();
              result = Adapter.ConvertBtoS_AddressBookList(bAddressBook).ToList();
          }
          catch (Exception error)
          {
              logger.Debug("From service implementation :" + Library.ExtractError(error));
              var generalFault = new SGeneralFault();
              generalFault.Issue = "Problem in fetch customer Designation";
              generalFault.Details = Library.ExtractError(error);
              throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in fetch customer Designation"));
          }
          return result;
      }

    #endregion

    #region UserManagement

        /// <summary>
        /// Login credential validation
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>
        /// 0- success
        /// 1 - Failure
        /// </returns>
        public int ValidateUser(string userName, string password)
        {
            int result = 1;

            try
            {
                UserHandler userHandler = new UserHandler();
                result = userHandler.ValidateUser(userName, password);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in validation of login details";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in validation of login details"));
            }

            return result;
        }

        /// <summary>
        /// To Get login information for login screen validations
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>
        /// get the all the login related user information for the given user id.
        /// </returns>
        public SUser GetLogin(string Username)
        {
            try
            {
                //1.Convert Service Entity to Business Entity
                UserAdapter adapter = new UserAdapter();

                //2. Create instance for business object and invoke method            
                UserHandler userHandler = new UserHandler();
                BUser bUser = userHandler.GetLogin(Username);

                //3. Convert BFuelSurcharge to SFuelSurcharge before return from service layer
                SUser sUser = adapter.ConvertBtoS_User(bUser);
                return sUser;
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while retrieving login details";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while retrieving login details"));
            }

        }

        /// <summary>
        /// To Get login information for N2 user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>
        /// get the all the login related user information for the given user id for N2 user.
        /// </returns>
        public SFranchise GetFranchise(string UserName)
        {
            SFranchise result = null;
            try
            {
                BFranchise bFranchise = new BFranchise();

                UserHandler userHandler = new UserHandler();
                bFranchise = userHandler.GetFranchise(UserName);

                UserAdapter adapter = new UserAdapter();
                result = adapter.ConvertBtoS_Franchise(bFranchise);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem inserting Terms of Service";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem getting user details"));
            }

            return result;

        }

        /// <summary>
        /// To Get login information for Referent user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>
        /// get the all the login related user information for the given user id for Referent user.
        /// </returns>
        public SCustomer GetCustomer(string UserName)
        {

            SCustomer result = null;
            try
            {
                BCustomer bCustomer = new BCustomer();

                UserHandler userHandler = new UserHandler();
                bCustomer = userHandler.GetCustomer(UserName);

                UserAdapter adapter = new UserAdapter();
                result = adapter.ConvertBtoS_Customer(bCustomer);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem getting the login details";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem getting the login details"));
            }

            return result;
        }

        /// <summary>
        /// To updated last login date and time
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>
        /// 0 - Success
        /// 1 - Failure
        /// 2 - User id not exists
        /// </returns>
        public int UpdateLastLogin(string UserName, DateTime CurrentDateTime)
        {
            int result = 1;

            try
            {
                UserHandler userHandler = new UserHandler();
                result = userHandler.UpdateLastLogin(UserName, CurrentDateTime);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in update the last login information";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in update the last login information"));
            }

            return result;
        }

        /// <summary>
        /// To get the functional list
        /// </summary>
        /// <returns>Result set </returns>
        public List<SFunctionality> GetFunctionality()
        {
            List<SFunctionality> result = null;
            try
            {
                List<BFunctionality> bFunctionality = new List<BFunctionality>();

                UserHandler userHandler = new UserHandler();
                bFunctionality = userHandler.GetFunctionality();

                UserAdapter adapter = new UserAdapter();
                result = adapter.ConvertBtoS_Functionality(bFunctionality);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem getting the Functionality details";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem getting the Functionality details"));
            }

            return result;

        }

        /// <summary>
        /// To get the functional-profile mapping list
        /// </summary>
        /// <returns>Result set </returns>
        public List<SFunctionalProfile> GetFunctionalProfile()
        {
            List<SFunctionalProfile> result = null;
            try
            {
                List<BFunctionalProfile> bFunctionalProfile = new List<BFunctionalProfile>();

                UserHandler userHandler = new UserHandler();
                bFunctionalProfile = userHandler.GetFunctionalProfile();

                UserAdapter adapter = new UserAdapter();
                result = adapter.ConvertBtoS_FunctionalProfile(bFunctionalProfile);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem getting the Profile-Functioanlity Mapping details";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem getting the Profile-Functioanlity Mapping details"));
            }

            return result;

        }

        /// <summary>
        /// To insert profile-Functionality Mapping
        /// </summary>
        /// <returns>int </returns>
        public int InsertFunctionalProfile(List<SFunctionalProfile> lsFunctionalProfile)
        {
            int result = 1;

            try
            {
                //1.Convert Service Entity to Business Entity
                UserAdapter adapter = new UserAdapter();
                List<BFunctionalProfile> lbFunctionalProfile = adapter.ConvertStoB_FunctionalProfile(lsFunctionalProfile);

                //2. Create instance for business object and invoke method            
                UserHandler userHandler = new UserHandler();

                result = userHandler.InsertFunctionalProfile(lbFunctionalProfile);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem inserting Profile-Functional Mapping";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem inserting Profile-Functional Mapping"));
            }

            return result;


        }

        /// <summary>
        /// To delete all Profile-Functinality entries
        /// </summary>
        /// <returns>int </returns>
        public int DeleteFunctionalProfile()
        {
            int result = 1;
            try
            {
                //1.Convert Service Entity to Business Entity

                //2. Create instance for business object and invoke method            
                UserHandler userHandler = new UserHandler();

                result = userHandler.DeleteFunctionalProfile();
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem inserting Terms of Service";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem inserting Terms of Service"));
            }

            return result;


        }

        /// <summary>
        /// To Insert user details for Franchise User
        /// </summary>
        /// <param name="CountryCode"></param>
        /// <returns>integer</returns>
        public int InsertFranchise(SFranchise sFranchise)
        {
            int result = 1;

            try
            {
                //1.Convert Service Entity to Business Entity
                UserAdapter adapter = new UserAdapter();
                BFranchise bFranchise = adapter.ConvertStoB_Franchise(sFranchise);

                //2. Create instance for business object and invoke method            
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.InsertFranchise(bFranchise);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in update the last login information";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in update the last login information"));
            }

            return result;
        }

        /// <summary>
        /// To Update a Login - Franchise Entry
        /// </summary>
        /// <returns>Integer </returns>
        public int UpdateFranchise(SFranchise sFranchise)
        {
            int result = 1;

            try
            {
                //1.Convert Service Entity to Business Entity
                UserAdapter adapter = new UserAdapter();
                BFranchise bFranchise = adapter.ConvertStoB_Franchise(sFranchise);

                //2. Create instance for business object and invoke method            
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.UpdateFranchise(bFranchise);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in update the Franchise information";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in update Franchise information"));
            }

            return result;


        }

        /// <summary>
        /// To insert Monthly fees for a particular franchise user
        /// </summary>
        /// <returns>Integer </returns>
        public int InsertMonthlyFee(SMonthlyFee sMonthlyFee)
        {
            int result = 1;

            try
            {
                //1.Convert Service Entity to Business Entity
                UserAdapter adapter = new UserAdapter();
                BMonthlyFee bMonthlyFee = adapter.ConvertStoB_MonthlyFee(sMonthlyFee);

                //2. Create instance for business object and invoke method            
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.InsertMonthlyFee(bMonthlyFee);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in insert the monthly fee";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in insert the monthly fee"));
            }
            return result;
        }

        /// <summary>
        /// To get Franchise user list
        /// </summary>
        /// <returns>company name, contact name, emailID for N2 user </returns>
        public List<SFranchiseContact> GetFranchiseList()
        {
            List<SFranchiseContact> result = null;
            try
            {
                List<BFranchiseContact> bFranchise = new List<BFranchiseContact>();

                UserHandler userHandler = new UserHandler();
                bFranchise = userHandler.GetFranchiseList().ToList();

                UserAdapter adapter = new UserAdapter();
                result = adapter.ConvertBtoS_FranchiseContactList(bFranchise);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem to get Franchise userlist";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem to get Franchise userlist"));
            }

            return result;
        }

        /// <summary>
        /// To get Monthly fees list for a account
        /// </summary>
        /// <returns>set of record for all shipment type for a account </returns>
        public List<SMonthlyFee> GetMonthlyFees(string UserId)
        {
            List<SMonthlyFee> result = null;
            try
            {
                List<BMonthlyFee> bMonthlyFee = new List<BMonthlyFee>();

                UserHandler userHandler = new UserHandler();
                bMonthlyFee = userHandler.GetMonthlyFees(UserId);

                UserAdapter adapter = new UserAdapter();
                result = adapter.ConvertBtoS_MonthlyFeeList(bMonthlyFee);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem getting the MonthlyFee details";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem getting the MonthlyFee details"));
            }

            return result;
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
            int result = 1;

            try
            {

                //2. Create instance for business object and invoke method            
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.ConfirmPassword(AccountNo, Password);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in update the confirmed password";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in update the confirmed password"));
            }

            return result;


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
            try
            {

                //2. Create instance for business object and invoke method            
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.GetLanguageResource(strCountryCode, strLanguageCode);
            }

            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [03JAN12RM] */
                SGeneralFault generalFault = new SGeneralFault();
                generalFault.Issue = "GetLanguageResource";
                generalFault.Details = Library.ExtractError(error);

                throw new FaultException<SGeneralFault>
                    (generalFault, new FaultReason(generalFault.Details), new FaultCode(generalFault.Issue));


                //logger.Debug("From service implementation :" + Library.ExtractError(error));
                //var generalFault = new SGeneralFault();
                //generalFault.Issue = "Problem while getting Language resource file name";
                //generalFault.Details = Library.ExtractError(error);
                //throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while getting Language resource file name"));
            }

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
            int result = 1;

            try
            {
                //Create instance for business object and invoke method            
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.CustomerServiceUpdate(AccountNo.Trim(), Pwd.Trim(), LanguageCode.Trim());
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in update Customer Service :" + AccountNo.Trim();
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in update Customer Service :" + AccountNo.Trim()));
            }
            return result;
        }

        /// <summary>
        /// To get Customer Service user list
        /// </summary>
        /// <returns>company name, contact name, emailID for N2 user </returns>
        public List<SFranchiseContact> GetCustomerServiceList()
        {
            List<SFranchiseContact> result = null;
            try
            {
                List<BFranchiseContact> bFranchise = new List<BFranchiseContact>();

                UserHandler userHandler = new UserHandler();
                bFranchise = userHandler.GetCustomerServiceList().ToList();

                UserAdapter adapter = new UserAdapter();
                result = adapter.ConvertBtoS_FranchiseContactList(bFranchise);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem to get Customer Service userlist";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem to get Customer Service userlist"));
            }
            return result;
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
            int result = 1;

            try
            {
                //Create instance for business object and invoke method            
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.CustomerServiceUpdateAdmin(UserID, Status);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in update Customer Service by admin User :" + UserID.Trim();
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in update Customer Service by admin User:" + UserID.Trim()));
            }
            return result;
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
            int result = 1;

            try
            {
                //Create instance for business object and invoke method            
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.UpdateAuthorize(AccountNo, CompanyName, Email, PhoneNo, RefAccountNo);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in update Anthorize user details";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in update Anthorize user details"));
            }

            return result;
        }

        /// <summary>
        /// To get all available industry details
        /// </summary>
        /// <returns>BIndustry</returns>
        public List<SIndustry> GetIndustry()
        {
            List<SIndustry> result = null;
            try
            {
                List<BIndustry> bIndustry = new List<BIndustry>();

                Library GlobalLibrary = new Library();
                bIndustry = GlobalLibrary.GetIndustry().ToList();

                CommonAdapter adapter = new CommonAdapter();

                result = adapter.ConvertBtoS_Industry(bIndustry);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem to get Industry list";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem to get Industry list"));
            }
            return result;
        }

        /// <summary>
        /// To insert Customer creation basic details
        /// </summary>
        /// <param name="bCustomer"></param>
        /// <returns></returns>
        public int InsertCustomer(SCustomer sCustomer)
        {
            int result = 1;
            try
            {
                //1.Convert Service Entity to Business Entity
                UserAdapter adapter = new UserAdapter();
                BCustomer bCustomer = adapter.ConvertStoB_Customer(sCustomer);

                //2. Create instance for business object and invoke method            
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.InsertCustomer(bCustomer);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in Customer Creation";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in Customer Creation"));
            }
            return result;
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
        public int InsertEndCustomer(SCustomer sCustomer)
        {
            int result = 1;
            try
            {
                //1.Convert Service Entity to Business Entity
                UserAdapter adapter = new UserAdapter();
                BCustomer bCustomer = adapter.ConvertStoB_Customer(sCustomer);

                //2. Create instance for business object and invoke method            
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.InsertEndCustomer(bCustomer);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in End Customer Creation";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in End Customer Creation"));
            }
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
            string result = null;
            try
            {
                //Create instance for business object and invoke method            
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.ValidateHQZipcode(HQZipcode.Trim());
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in validation of Assigned zone";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in validation of Assigned zone"));
            }
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
        public int UpdateEndCustomerByAdmin(SCustomer sCustomer)
        {
            int result = 1;
            try
            {
                //1.Convert Service Entity to Business Entity
                UserAdapter adapter = new UserAdapter();
                BCustomer bCustomer = adapter.ConvertStoB_Customer(sCustomer);

                //2. Create instance for business object and invoke method            
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.UpdateEndCustomerByAdmin(bCustomer);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in End Customer update";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in End Customer update"));
            }
            return result;


        }

        /// <summary>
        /// To get Cretdit list for list of customer ids
        /// </summary>
        /// <param name="AccountNo"></param>
        /// <param name="UserType"></param>
        /// <returns>  List<BCustomer> </returns>
        public List<SCustomer> GetCustomerCredit(string AccountNo, string UserType)
        {
            List<SCustomer> result = null;
            try
            {
                List<BCustomer> bCustomer = new List<BCustomer>();

                UserHandler userHandler = new UserHandler();
                bCustomer = userHandler.GetCustomerCredit(AccountNo, UserType);

                UserAdapter adapter = new UserAdapter();
                result = adapter.ConvertBtoS_CustomerList(bCustomer);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem to get Franchise userlist";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem to get Franchise userlist"));
            }

            return result;
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
            int result = 1;
            try
            {

                // Create instance for business object and invoke method            
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.UpdateCustomerCredit(AccountNo.Trim(), WishedAmt, InsuredAmt, PaymentDelayDay, GrossMargin, PaymentDelayMonth, CompensationRate, AuthorizedCreditAmt, DeferredPaymentAgreed);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in End Customer update";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in End Customer update"));
            }
            return result;
        }

        /// <summary>
        /// To get all available Authorized user list of a referent user
        /// </summary>
        /// <param name="AccountNo"></param>
        /// <returns></returns>
        public List<SAuthorized> GetAuthorizedList(string AccountNo)
        {

            try
            {

                List<BAuthorized> bAuthorized;
                List<SAuthorized> sAuthorized = new List<SAuthorized>();

                UserHandler userHandler = new UserHandler();
                bAuthorized = userHandler.GetAuthorizedUserList(AccountNo.Trim());

                UserAdapter adapter = new UserAdapter();
                sAuthorized = adapter.ConvertBtoS_Authorized(bAuthorized);
                return sAuthorized;
            }
            catch (Exception error)
            {

                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem inserting Terms of Service";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem inserting Terms of Service"));
            }
        }


        /// <summary>
        /// To insert a autorized User details
        /// </summary>
        /// <param name="sAuthorize"></param>
        /// <returns></returns>
        public int InsertAuthorized(SAuthorized sAuthorize)
        {
            int result = 1;
            try
            {
                //1.Convert Service Entity to Business Entity
                UserAdapter adapter = new UserAdapter();
                BAuthorized bCustomer = adapter.ConvertStoB_Authorized(sAuthorize);


                //2. Create instance for business object and invoke method            
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.InsertAuthorized(bCustomer);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in Authorized Creation";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in Authorized Creation"));
            }
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
            int result = 1;
            try
            {
                //Create instance for business object and invoke method            
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.UpdateAuthorizedSelf(AccountNo, Password, TelNo);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in update Authorized User details";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in update Authorized User details"));
            }

            return result;
        }

        /// <summary>
        /// To get Authorized user details in requried format
        /// </summary>
        /// <param name="AccountNo"></param>
        /// <returns></returns>
        public SAuthorized GetAuthorized(string AccountNo)
        {
            SAuthorized result = null;
            try
            {
                BAuthorized bAuthorized = new BAuthorized();

                UserHandler userHandler = new UserHandler();
                bAuthorized = userHandler.GetAuthorized(AccountNo);

                UserAdapter adapter = new UserAdapter();
                result = adapter.ConvertBtoS_Authorized(bAuthorized);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem getting the Authorized user details";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem getting the Authorized user details"));
            }

            return result;
        }

        /// <summary>
        /// To get End customer list user list
        /// </summary>
        /// <returns>company name, contact name, emailID for N2 user </returns>
        public List<SFranchiseContact> GetEndCustomerList()
        {
            List<SFranchiseContact> result = null;
            try
            {
                List<BFranchiseContact> bFranchise = new List<BFranchiseContact>();

                UserHandler userHandler = new UserHandler();
                bFranchise = userHandler.GetEndCustomerList().ToList();

                UserAdapter adapter = new UserAdapter();
                result = adapter.ConvertBtoS_FranchiseContactList(bFranchise);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem to get Franchise userlist";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem to get Franchise userlist"));
            }

            return result;
        }

      /// <summary>
    /// Retrive user Details
    /// </summary>
    /// <returns>User details as User entity</returns>
        public string GetUser(string accountNumber)
        {
            string result = null;
            try
            {
                //Create instance for business object and invoke method            
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.GetUser(accountNumber);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in retrieving UserId";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in retrieving UserId"));
            }
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
        public int InsertAdv(List<SAdv> sAdv)
        {
            int result = 1;
            try
            {
                //1.Convert Service Entity to Business Entity
                UserAdapter adapter = new UserAdapter();
                List<BAdv> bAdv = adapter.ConvertStoB_AdvList(sAdv);

                //2. Create instance for business object and invoke method            
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.InsertAdv(bAdv);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in Insert ADV";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in Insert ADV"));
            }
            return result;

        }

        /// <summary>
        /// To update additional details of created customer in Customer table by admin / Franchise User
        /// </summary>
        /// <param name="bCustomer"></param>
        /// <returns>
        /// 0 - Success
        /// 1 - Failure
        /// </returns>
        public int UpdateEndCustomer(SCustomer sCustomer)
        {
            int result = 1;
            try
            {
                //1.Convert Service Entity to Business Entity
                UserAdapter adapter = new UserAdapter();
                BCustomer bCustomer = adapter.ConvertStoB_Customer(sCustomer);

                //2. Create instance for business object and invoke method            
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.UpdateEndCustomer(bCustomer);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in End Customer update";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in End Customer update"));
            }
            return result;
        }

        public SCustomer GetEndCustomer(string UserId)
        {
            SCustomer result = null;
            try
            {
                BCustomer bCustomer = new BCustomer();

                UserHandler userHandler = new UserHandler();
                bCustomer = userHandler.GetEndCustomer(UserId.Trim());

                UserAdapter adapter = new UserAdapter();
                result = adapter.ConvertBtoS_Customer(bCustomer);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem getting the End Customer details";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem getting the End Customer details"));
            }

            return result;
        }

        public int UpdateFranchiseAdmin(SFranchise sFranchise)
        {
            int result = 1;

            try
            {
                //1.Convert Service Entity to Business Entity
                UserAdapter adapter = new UserAdapter();
                BFranchise bFranchise = adapter.ConvertStoB_Franchise(sFranchise);

                //2. Create instance for business object and invoke method            
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.UpdateFranchiseAdmin(bFranchise);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in update the Franchise information";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in update Franchise information"));
            }

            return result;
        
        }

        public int DeleteAuthorized(string AccountNo)
        {
            int result = 1;
            try
            {
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.DeleteAuthorized(AccountNo.Trim());
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in delete Authorized User details";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in delete Authorized User details"));
            }

            return result;
        
        }

        public int InsertCarrierAccountRef(string[] CarrierAccList, string[] CarrierAcc, string AccountNo)
        {
            int result = 1;
            try
            {
                UserHandler UserHandler = new UserHandler();
                result = UserHandler.InsertCarrierAccountRef(CarrierAccList, CarrierAcc, AccountNo);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in Insert Carrier Account reference";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in Insert Carrier Account reference"));
            }
            return result;

        }


        public string GetPostalCode(string strCountryCode)
        {
            string strFormat = string.Empty;
            try
            {

                UserHandler userHandler = new UserHandler();
                strFormat = userHandler.GetPostalCode(strCountryCode);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in Reteriving postal code";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in Reteriving postal code"));
            }
            return strFormat;
        }
        #endregion

    #region Common
        /// <summary>
        /// Get list of data from the given field name of Table
        /// </summary>
        /// <returns>
        /// for success =&gt; String List
        /// Failure =&gt; NULL
        /// </returns>
        public List<SComboText> FillCombo(SComboTableField sComboTableField)
        {
            try
            {
                //1.Convert Service Entity to Business Entity
                CommonAdapter adapter = new CommonAdapter();
                BComboTableField bComboTableField = new BComboTableField();
                List<BComboText> bComboText;
                List<SComboText> sComboText = new List<SComboText>();

                //2. Create instance for business object and invoke method            
                Library library = new Library();

                bComboTableField = adapter.ConvertStoB_ComboTableField(sComboTableField);
                bComboText = library.FillCombo(bComboTableField);
                sComboText = adapter.ConvertBtoS_ComboText(bComboText);
                return sComboText.ToList();
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem retrieving combo box filling";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem retrieving combo box filling"));
            }
        }

        /// <summary>
        /// To Encrypt the given string
        /// </summary>
        /// <returns>
        /// Return encrypted string
        /// </returns>
        public string EncryptString(string Message, string Passphrase)
        {
            string result = "";

            try
            {
                Library GlobalLibrary = new Library();
                result = GlobalLibrary.EncryptString(Message, Passphrase);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in Password Encryption";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in Password Encryption"));
            }
            return result;
        }

        /// <summary>
        /// To Decrypt the given string
        /// </summary>
        /// <returns>
        /// Return Decryptded string
        /// </returns>
        public string DecryptString(string Message, string Passphrase)
        {
            string result = "";

            try
            {
                Library GlobalLibrary = new Library();
                result = GlobalLibrary.DecryptString(Message, Passphrase);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in Password Encryption";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in Password Encryption"));
            }
            return result;

        }

        /// <summary>
        /// To retrieve a particular column
        /// </summary>
        /// <param name="sComboTableField"></param>
        /// <returns></returns>
        public List<String> GetFieldValue(SComboTableField sComboTableField)
        {
            try
            {
                /* 1. Create adapter to convert service to business entity*/
                CommonAdapter adapter = new CommonAdapter();
                BComboTableField bComboTableField = adapter.ConvertStoB_ComboTableField(sComboTableField);

                /* 2. Call business method to get reslt */
                Library library = new Library();
                List<String> TableValues = library.GetFieldValue(bComboTableField);

                /* 3. Return result */
                return TableValues;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem retrieving combo box filling";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem retrieving combo box filling"));
            }

        }

        /// <summary>
        /// Validate the given mail ID
        /// </summary>
        /// <returns>
        /// 0 =&gt; Valid
        /// 1 =&gt; Invalid
        /// </returns>
        public int ValidateEmail(string EmailID)
        {
            int result = 1;

            try
            {
                Library GlobalLibrary = new Library();
                result = GlobalLibrary.ValidateEmail(EmailID);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in Password Encryption";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in Password Encryption"));
            }
            return result;

        }

        /// <summary>
        /// Validate the postal code for a country
        /// </summary>
        /// <returns>
        /// 0 =&gt; Valid
        /// 1 =&gt; Invalid
        /// </returns>
        public int ValidateZipCode(string PostalCode, string CountryCode)
        {
            int result = 1;

            try
            {
                Library GlobalLibrary = new Library();
                result = GlobalLibrary.ValidateZipCode(PostalCode.Trim(), CountryCode.Trim());
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in validation of postal code";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in validation of postal code"));
            }
            return result;
        }

        /// <summary>
        /// To get language code for the given country code
        /// </summary>
        /// <returns>
        /// Value - Success
        /// 1 - Failure
        /// 2 - country code not exists
        /// </returns>
        public string[] GetLanguage(string CountryCode)
        {
            try
            {
                Library GlobalLibrary = new Library();
                string[] ComboText = GlobalLibrary.GetLanguage(CountryCode.Trim());
                return ComboText;
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in fetching language code";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in fetching language code"));
            }
        }

        /// <summary>
        /// To get language list
        /// </summary>
        /// <returns>
        /// result or null
        /// </returns>
        public List<String> GetLanguageList()
        {
            try
            {
                //1.Convert Service Entity to Business Entity
                CommonAdapter adapter = new CommonAdapter();

                //2. Create instance for business object and invoke method            
                Library GlobalLibrary = new Library();
                return GlobalLibrary.GetLanguageList().ToList();
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in fetching language code";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in fetching language code"));
            }
        }

        /// <summary>
        /// Get the counter for creating
        /// </summary>
        /// <returns>
        /// All positive  values =&gt; Current counter value
        /// -1 =&gt; Key code does not exists
        /// -2 =&gt; Counter out of range
        /// </returns>
        public SNextcounter GetNextCounter(string KeyCode, string CounterType, int RequiredCount)
        {
            SNextcounter sNextcounter = new SNextcounter();
            try
            {
                //1.Convert Service Entity to Business Entity
                CommonAdapter adapter = new CommonAdapter();
                BNextcounter bNextcounter = new BNextcounter();

                //2. Create instance for business object and invoke method            
                Library library = new Library();
                bNextcounter = library.GetNextCounter(KeyCode, CounterType, RequiredCount);
                sNextcounter = adapter.ConvertBtoS_Nextcounter(bNextcounter);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem getting Next Counter details";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem getting Next Counter details"));
            }

            return sNextcounter;

        }

        /// <summary>
        /// Get list of data from the given field name of Table
        /// </summary>
        /// <returns>
        /// for success =&gt; String List
        /// Failure =&gt; NULL
        /// </returns>
        public List<SCountryTable> FillCountryCombo()
        {
            List<SCountryTable> sCountryTable = new List<SCountryTable>();
            try
            {
                //1.Convert Service Entity to Business Entity
                CommonAdapter adapter = new CommonAdapter();
                List<BCountryTable> bCountryTable;// = new List<BCountryTable>();

                //2. Create instance for business object and invoke method            
                Library library = new Library();
                bCountryTable = library.FillCountryCombo();
                sCountryTable = adapter.ConvertBtoS_CountryTable(bCountryTable);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [03JAN12RM] */
                SGeneralFault generalFault = new SGeneralFault();
                generalFault.Issue = "FillCountryCombo";
                generalFault.Details = Library.ExtractError(error);

                throw new FaultException<SGeneralFault>
                    (generalFault, new FaultReason(generalFault.Details), new FaultCode(generalFault.Issue));

                //logger.Debug("From service implementation :" + Library.ExtractError(error));
                //var generalFault = new SGeneralFault();
                //generalFault.Issue = "Problem getting Country - Country code details";
                //generalFault.Details = Library.ExtractError(error);
                //throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem getting Country - Country code details"));
            }
            return sCountryTable.ToList();
        }

        /// <summary>
        /// Validate given password
        /// </summary>
        /// <returns>
        /// 0 =&gt; Valid
        /// 1 =&gt; Invalid
        /// </returns>
        public int ValidatePassword(string password)
        {
            int result = 1;

            try
            {
                Library GlobalLibrary = new Library();
                result = GlobalLibrary.ValidatePassword(password);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in Password Encryption";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in Password Encryption"));
            }
            return result;
        }

        /// <summary>
        /// Create a random password
        /// </summary>
        /// <returns>
        /// string value of password
        /// </returns>
        public string CreatePassword(int length)
        {
            try
            {
                Library GlobalLibrary = new Library();
                string ComboText = GlobalLibrary.CreatePassword(length);
                return ComboText;
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in creating password";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in creating password"));
            }

        }

        /// <summary>
        /// To send an e-mail to auditors
        /// </summary>
        /// <param name="UserID">MailID</param>
        /// <returns></returns>
        public bool sendMessage(string UserID, string Password, string CurrentUserID)
        {
            try
            {
                Library GlobalLibrary = new Library();
                bool ComboText = GlobalLibrary.sendMessage(UserID, Password, CurrentUserID);
                return ComboText;
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in creating password";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in creating password"));
            }


        }

        /// <summary>
        /// To get the list of Zip and city to show autofill
        /// </summary>
        /// <param name="SearchString">Part of string needs to be searched</param>
        /// <param name="Count">No of records required</param>
        /// <returns></returns>
        public List<string> CityZipcodeAutoFill(string SearchString, string Country, int Count)
        {
            List<string> result = new List<string>();
            try
            {
                Library GlobalLibrary = new Library();
                result = GlobalLibrary.CityZipcodeAutoFill(SearchString, Country, Count);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while getting City and Zipcodes for auto fill";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while getting City and Zipcodes for auto fill"));
            }
            return result;

        }

        /// <summary>
        /// To validate time format HH:MM
        /// </summary>
        /// <param name="strTime"></param>
        /// <returns></returns>
        public int ValidateTime(String strTime)
        {
            int result = 1;

            try
            {
                Library GlobalLibrary = new Library();
                result = GlobalLibrary.ValidateTime(strTime);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in time format Validation";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in time format Validation"));
            }
            return result;
        }

        /// <summary>
        /// To import Master Tariff Type
        /// </summary>
        /// <param name="tariffMaster"></param>
        /// <param name="tariffFile"></param>
        /// <returns>
        /// Row 0 => Number of success + Success in FieldName
        /// Row 0 => Number of Fail + Failed in FieldName 
        /// in all other positive row number will have Row ID + Field Name + Error description
        /// </returns>
        public List<SFileImportStatus> ImportAddressBook(byte[] addressBookFile, string AccountNo)
        {
            List<SFileImportStatus> sFileImportStatus;

            try
            {
                //1.Create Tariff adapter 
                TariffAdapter adapter = new TariffAdapter();

                //3. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                Library GlobalLibrary = new Library();
                List<BFileImportStatus> bFileImportStatus = GlobalLibrary.ImportAddressBook(addressBookFile, AccountNo.Trim());

                //4. Convert business entity result to service entity result before return
                sFileImportStatus = adapter.ConvertBtoS_FileImportStatus(bFileImportStatus);

                //5.return result
                return sFileImportStatus;
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while importing AddressBook";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while importing AddressBook"));
            }
        }

        /// <summary>
        /// To get Customer designation list
        /// </summary>
        /// <returns> Designation list</returns>
        public List<string> GetFunction()
        {
            List<string> result = null;

            try
            {
                Library GlobalLibrary = new Library();
                result = GlobalLibrary.GetFunction().ToList();
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in fetch customer Designation";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in fetch customer Designation"));
            }
            return result;
        }

       /// <summary>
    /// To validate given string is numeric or not
    /// </summary>
    /// <param name="val"></param>
    /// <param name="NumberStyle"></param>
    /// <returns>
    /// True - The given string is Numeric
    /// False - The given string is not Numeric
    /// </returns>
        public bool isNumericValidation(string val, System.Globalization.NumberStyles NumberStyle)
        {
            bool result = false;

            try
            {
                Library GlobalLibrary = new Library();
                result = GlobalLibrary.isNumericValidation(val,NumberStyle);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in numeric valiation";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in numeric valiation"));
            }
            return result;
        }

      /// <summary>
      /// To validate the alpha numeric characters
      /// </summary>
      /// <param name="val"></param>
      /// <returns></returns>
        public bool isAlphaNumericValidation(string val)
        {
            bool result = false;

            try
            {
                Library GlobalLibrary = new Library();
                result = GlobalLibrary.isAlphaNumericValidation(val);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in alpha numeric valiation";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in alpha numeric valiation"));
            }
            return result;
        
        
        }

        /// <summary>
        /// To get Key value for given key code
        /// </summary>
        /// <param name="KeyCode"></param>
        /// <returns></returns>
        public SKeyValue GetValueFromParameter(string KeyCode)
        {
            SKeyValue result = new SKeyValue();

            try
            {
                //1.Convert Service Entity to Business Entity
                BKeyValue bKeyValue = new BKeyValue();
                CommonAdapter adapter = new CommonAdapter();

                //2. Create instance for business object and invoke method  
                Library library = new Library();
                bKeyValue = library.GetValueFromParameter(KeyCode);
                result = adapter.ConvertBtoS_KeyValue(bKeyValue);

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in getting Key value for  [" + KeyCode + "]";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in getting Key value for  [" + KeyCode + "]"));
            }

            return result;

        }

      /// <summary>
    /// To send an e-mail to the particular user to confirm the password
    /// </summary>
    /// <param name="UserID">MailID</param>
    /// <returns></returns>
        public bool sendConfirmPassord(string UserId, string CurrentUserID)
        {
            try
            {
                Library GlobalLibrary = new Library();
                bool ComboText = GlobalLibrary.sendConfirmPassord(UserId, CurrentUserID);
                return ComboText;
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in Password recovery";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in Password recovery"));
            }
        
        }

        public bool IsValidZidcode(string Zipcode)
        {
            try
            {
                Library GlobalLibrary = new Library();
                bool ComboText = GlobalLibrary.IsValidZidcode(Zipcode);
                return ComboText;
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in Zipcode validation";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in Zipcode validation"));
            }
        }

      /// <summary>
    /// To get the list reference carriers
    /// </summary>
    /// <returns>List of carriers</returns>
        public List<string> GetAllRefCarrierList()
        {
            List<string> result = null;

            try
            {
                Library GlobalLibrary = new Library();
                result = GlobalLibrary.GetAllRefCarrierList().ToList();
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in fetch Reference Carrier List";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in fetch Reference Carrier List"));
            }
            return result;
        }

      /// <summary>
      /// Validate the postalcode format
      /// </summary>
      /// <param name="strActualFormat"></param>
      /// <returns></returns>
        public bool ValidatePostalcode(string strActualFormat, string strZipcode)
        {
            bool result = false;

            try
            {
                Library GlobalLibrary = new Library();
                result = GlobalLibrary.ValidatePostalcode(strActualFormat, strZipcode);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in postalcode valiation";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in postalcode valiation"));
            }
            return result;
        
        }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="To"></param>
      /// <param name="CurrentUserID"></param>
      /// <param name="strBody"></param>
      /// <param name="Subject"></param>
      /// <returns></returns>
        public bool sendMail(string To, string CurrentUserID, string strBody, string Subject)
        {
            bool result = true;
            try
            {
                Library GlobalLibrary = new Library();
                result = GlobalLibrary.sendMail(To, CurrentUserID, strBody, Subject);

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while sending mail";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while sending mail"));
            }
            return result;
        }

    #endregion

    #region Tariff

        /// <summary>
        /// To create Master Service
        /// </summary>
        /// <param name="sMasterServiceType"></param>
        /// <returns></returns>
        public int CreateMasterService(SMasterServiceType sMasterServiceType)
        {
            int result;

            try
            {
                //1.Convert Service Entity to Business Entity
                TariffAdapter adapter = new TariffAdapter();

                BMasterServiceType bMasterServiceType = adapter.ConvertStoB_MasterServiceType(sMasterServiceType);

                //2. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                result = tariffHandler.CreateMasterService(bMasterServiceType);

                //3.return result
                return result;

            }
            catch (Exception error)
            {

                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem inserting profile";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem creating new service type"));
            }
        }

        /// <summary>
        /// To get fuel charge details
        /// </summary>
        /// <param name="tariffType"></param>
        /// <param name="keyAccountRef"></param>
        /// <returns></returns>
        public List<SFuelSurcharge> GetFuelCharge(string tariffType, string keyAccountRef)
        {

            try
            {
                //1.Convert Service Entity to Business Entity
                TariffAdapter adapter = new TariffAdapter();


                //2. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                List<BFuelSurcharge> bFuelSurcharges = tariffHandler.GetFuelCharge(tariffType, keyAccountRef);


                //3. Convert BFuelSurcharge to SFuelSurcharge before return from service layer
                List<SFuelSurcharge> sFuelSurcharges = adapter.ConvertBtoS_FuelCharges(bFuelSurcharges);


                //4.return result
                return sFuelSurcharges;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem retrieving Fuel Charges";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while retrieving Fuel Charges"));
            }



        }

        /// <summary>
        /// Get fuel charge parameter for the given service ID
        /// </summary>
        /// <param name="serviceID"></param>
        /// <returns></returns>
        public List<SFuelSurchargeParameter> GetFuelChargeParameter(int serviceID)
        {

            try
            {
                //1.Convert Service Entity to Business Entity
                TariffAdapter adapter = new TariffAdapter();


                //2. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                List<BFuelSurchargeParameter> bFuelSurcharges = tariffHandler.GetFuelChargeParameter(serviceID);


                //3. Convert BFuelSurcharge to SFuelSurcharge before return from service layer
                List<SFuelSurchargeParameter> sFuelSurchargeParameter = adapter.ConvertBtoS_FuelChargesParameters(bFuelSurcharges);


                //4.return result
                return sFuelSurchargeParameter;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem retrieving Fuel Charges Parameter";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while retrieving Fuel Charges Parameter"));
            }



        }

        /// <summary>
        /// Update fuel charge parameter
        /// </summary>
        /// <param name="sFuelSurchargeParameter"></param>
        /// <returns></returns>
        public int UpdateFuelChargeParameter(List<SFuelSurchargeParameter> sFuelSurchargeParameter)
        {
            int result = 1;

            try
            {
                //1.Convert Service Entity to Business Entity
                TariffAdapter adapter = new TariffAdapter();
                List<BFuelSurchargeParameter> bFuelSurcharges = adapter.ConvertStoB_FuelChargesParameters(sFuelSurchargeParameter);


                //2. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                result = tariffHandler.UpdateFuelChargeParameter(bFuelSurcharges);

                //3.return result
                return result;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem retrieving Fuel Charges Parameter";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while retrieving Fuel Charges Parameter"));
            }


        }

        /// <summary>
        /// Update fuel charge start date
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public int UpdateFuelChargeStartDate(int ServiceId, DateTime startDate)
        {
            int result = 1;

            try
            {

                //1. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                result = tariffHandler.UpdateFuelChargeStartDate(ServiceId, startDate);

                //2.return result
                return result;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while updating Fuel charge start date";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while updating Fuel charge start date"));
            }
        }

        /// <summary>
        /// Get carrier service list
        /// </summary>
        /// <returns></returns>
        public List<SCarrierService> GetCarrierService()
        {
            try
            {
                //1.Convert Service Entity to Business Entity
                CarrierAdapter adapter = new CarrierAdapter();


                //2. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                List<BCarrierService> bCarrierService = tariffHandler.GetCarrierService();


                //3. Convert BFuelSurcharge to SFuelSurcharge before return from service layer
                List<SCarrierService> sFuelSurchargeParameter = adapter.ConvertBtoS_CarrierService(bCarrierService);

                //4.return result
                return sFuelSurchargeParameter;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem retrieving carrier service";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while retrieving carrier service"));
            }


        }

        ///// <summary>
        ///// Update carrier service
        ///// </summary>
        ///// <param name="sCarrierService"></param>
        ///// <returns></returns>
        //public int UpdateCarrierService(SCarrierService sCarrierService)
        //{
        //    int result = 1;

        //    try
        //    {
        //        //1.Convert Service Entity to Business Entity
        //        CarrierAdapter adapter = new CarrierAdapter();


        //        BCarrierService bCarrierService = adapter.ConvertStoB_CarrierService(sCarrierService);


        //        //2. Create instance for business object and invoke method            
        //        TariffHandler tariffHandler = new TariffHandler();
        //        result = tariffHandler.UpdateCarrierService(bCarrierService);

        //        //3.return result
        //        return result;

        //    }
        //    catch (Exception error)
        //    {
        //        logger.Debug("From service implementation :" + Library.ExtractError(error));
        //        var generalFault = new SGeneralFault();
        //        generalFault.Issue = "Problem while updating carrier service";
        //        generalFault.Details = Library.ExtractError(error);
        //        throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while updating carrier service"));
        //    }
        //}

        /// <summary>
        /// Update list of carrier service 17FEB12RM
        /// </summary>
        /// <param name="sCarrierService"></param>
        /// <returns></returns>
        public int UpdateCarrierService(List<SCarrierService> sCarrierService)
        {
            int result = 1;

            try
            {
                //1.Convert Service Entity to Business Entity
                CarrierAdapter adapter = new CarrierAdapter();

                //2. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();

                for (int i = 0; i < sCarrierService.Count; i++)
                {
                    BCarrierService bCarrierService = adapter.ConvertStoB_CarrierService(sCarrierService[i]);
                    result = tariffHandler.UpdateCarrierService(bCarrierService);
                }

                //3.return result
                return result;
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while updating carrier service";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while updating carrier service"));
            }
        }

        /// <summary>
        /// To get carrier service delivery delay for the given service ID
        /// </summary>
        /// <param name="serviceID"></param>
        /// <returns></returns>
        public List<SDeliveryDelay> GetCarrierServiceDeliveryDelay(int serviceID)
        {
            try
            {
                //1.Convert Service Entity to Business Entity
                CarrierAdapter adapter = new CarrierAdapter();


                //2. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                List<BDeliveryDelay> bDeliveryDelay = tariffHandler.GetCarrierServiceDeliveryDelay(serviceID);


                //3. Convert BFuelSurcharge to SFuelSurcharge before return from service layer
                List<SDeliveryDelay> sDeliveryDelay = adapter.ConvertBtoS_DeliveryDelay(bDeliveryDelay);


                //4.return result
                return sDeliveryDelay;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem retrieving carrier service delay";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem retrieving carrier service delay"));
            }
        }

        /// <summary>
        /// Update carrier service delay for the given service ID
        /// </summary>
        /// <param name="ServiceID"></param>
        /// <param name="Origin"></param>
        /// <param name="Destination"></param>
        /// <param name="Delay"></param>
        /// <returns></returns>
        public int UpdateCarrierServiceDelay(int ServiceID, string Origin, string Destination, int Delay)
        {
            int result = 1;

            try
            {
                //1. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                result = tariffHandler.UpdateCarrierServiceDelay(ServiceID, Origin, Destination, Delay);

                //2.return result
                return result;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while insert or update carrier service delay";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while insert or update carrier service delay"));
            }
        }

        /// <summary>
        /// Import delivery delay for the given service ID
        /// </summary>
        /// <param name="ServiceID"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public List<SFileImportStatus> ImportDeliveryDelay(int ServiceID, byte[] stream)  //24JAN12RM 
        {
            List<SFileImportStatus> sFileImportStatus;

            try
            {
                //1.Create Tariff adapter 
                TariffAdapter adapter = new TariffAdapter();

                //2. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();

                //3. Create instance for business object and invoke method            
                List<BFileImportStatus> bFileImportStatus = tariffHandler.ImportDeliveryDelay(ServiceID, stream);

                //4. Convert business entity result to service entity result before return
                sFileImportStatus = adapter.ConvertBtoS_FileImportStatus(bFileImportStatus);


                return sFileImportStatus;
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [24JAN12RM] */
                SGeneralFault generalFault = new SGeneralFault();
                generalFault.Issue = "ImportDeliveryDelay";
                generalFault.Details = Library.ExtractError(error);

                throw new FaultException<SGeneralFault>
                    (generalFault, new FaultReason(generalFault.Details), new FaultCode(generalFault.Issue));

            }
        }


        /**********************  after 13th December ****************************/

        /// <summary>
        /// Acknowledgement of Mast Tariff Creation
        /// </summary>
        /// <param name="sTariffMaster"></param>
        /// <returns></returns>
        public STariffCreationAcknowledgement CreateMasterTariff(STariffMaster sTariffMaster)
        {

            try
            {
                //1.Convert Service Entity to Business Entity
                TariffAdapter adapter = new TariffAdapter();

                //2. Convert Service to Business entity using adapter
                BTariffMaster bTariffMaster = adapter.ConvertStoB_TariffMaster(sTariffMaster);

                //3. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                BTariffCreationAcknowledgement bTariffCreationAck = tariffHandler.CreateMasterTariff(bTariffMaster);

                //4. Convert result from business entity to service entity
                STariffCreationAcknowledgement sTariffCreationAck = adapter.ConvertBtoS_TariffCreationAck(bTariffCreationAck);

                //5.return result
                return sTariffCreationAck;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while Tariff creation";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while Tariff creation"));
            }


        }

        /// <summary>
        /// To retrieve list of tariff reference for which tariff to be imported
        /// </summary>
        /// <param name="CarrierName"></param>
        /// <returns></returns>
        public List<String> GetOpenImportTariff(String CarrierName)
        {
            try
            {
                //1.Convert Service Entity to Business Entity
                CarrierAdapter adapter = new CarrierAdapter();


                //2. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                List<String> tariffRef = tariffHandler.GetOpenImportTariff(CarrierName);

                //3.return result
                return tariffRef;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem retrieving open tarrif reference";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem retrieving open tarrif reference"));
            }
        }

        public List<SFileImportStatus> ImportTariff(string CarrierName, string TariffReference, List<STariffCalculationRule> sTariffCalculationRule, byte[] tariffFile)
        {
            List<SFileImportStatus> sFileImportStatus;

            try
            {
                //1.Create Tariff adapter 
                TariffAdapter adapter = new TariffAdapter();

                //2. Convert STariffcalculation to business entity
                List<BTariffCalculationRule> bTariffCalculationRule = adapter.ConvertStoB_TariffCalculationRule(sTariffCalculationRule);

                //3. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                List<BFileImportStatus> bFileImportStatus = tariffHandler.ImportTariff(CarrierName, TariffReference, bTariffCalculationRule, tariffFile);

                //4. Convert business entity result to service entity result before return
                sFileImportStatus = adapter.ConvertBtoS_FileImportStatus(bFileImportStatus);

                //5.return result
                return sFileImportStatus;
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [18JAN12RM] */
                SGeneralFault generalFault  = new SGeneralFault();
                generalFault.Issue          = "ImportTariff";
                generalFault.Details        = Library.ExtractError(error);

                throw new FaultException<SGeneralFault>
                    (generalFault, new FaultReason(generalFault.Details), new FaultCode(generalFault.Issue));

                //logger.Debug("From service implementation :" + Library.ExtractError(error));
                //var generalFault = new SGeneralFault();
                //generalFault.Issue = "Problem while importing Tariff" + TariffReference;
                //generalFault.Details = Library.ExtractError(error);
                //throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while importing Tariff" + TariffReference));
            }

        }

        public List<STariffReferenceList> GetTariffReference(String CarrierName, String TariffType)
        {
            try
            {
                //1. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                List<BTariffReferenceList> bTariffRef = tariffHandler.GetTariffReference(CarrierName, TariffType);

                //2.Convert Service Entity to Business Entity
                TariffAdapter adapter = new TariffAdapter();
                List<STariffReferenceList> sTariffReferenceList = adapter.ConvertBtoS_TariffReferenceList(bTariffRef);

                //3.return result
                return sTariffReferenceList;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem retrieving list of assigned tarrif reference for the given carrier";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem retrieving list of assigned tarrif reference for the given carrier"));
            }
        }

        public int UpdateTariffReference(string TariffReference, DateTime StartDate, DateTime EndDate, bool Archived)
        {
            int result = 1;

            try
            {
                //1. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                result = tariffHandler.UpdateTariffReference(TariffReference, StartDate, EndDate, Archived);

                //2.return result
                return result;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while update Tariff reference " + TariffReference;
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while update Tariff reference " + TariffReference));
            }
        }

        /// <summary>
        /// To create a zone
        /// </summary>
        /// <param name="zone"></param>
        /// <returns></returns>
        public int CreateZone(SZone zone, string Flag, int ZoneID)
        {
            int result = 1;

            try
            {
                //1. Create adapter to convert Service to Business
                TariffAdapter adapter = new TariffAdapter();
                BZone bZone = adapter.ConvertStoB_Zone(zone);

                //2. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                result = tariffHandler.CreateZone(bZone, Flag, ZoneID);

                //2.return result
                return result;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while creation zone " + zone.ZoneName;
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while creation zone " + zone.ZoneName));
            }
        }

        public List<SZoneSearchDetails> GetZoneSearchDetails(string TariffReference)
        {
            List<SZoneSearchDetails> sZoneSearchDetails;
            try
            {

                //1. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                List<BZoneSearchDetails> bZoneSearchDetails = tariffHandler.GetZoneSearchDetails(TariffReference);

                //2.Create Tariff adapter and convert Business to Service entity
                TariffAdapter adapter = new TariffAdapter();
                sZoneSearchDetails = adapter.ConvertBtoS_ZoneSearchDetails(bZoneSearchDetails);

                //5.return result
                return sZoneSearchDetails;
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while searching zone details for the tariff " + TariffReference;
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while searching zone details for the tariff " + TariffReference));
            }
        }

        public SZone GetZoneDetails(int ZoneID)
        {
            SZone sZone;
            try
            {

                //1. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                BZone bZone = tariffHandler.GetZoneDetails(ZoneID);

                //2.Create Tariff adapter and convert Business to Service entity
                TariffAdapter adapter = new TariffAdapter();
                sZone = adapter.ConvertBtoS_Zone(bZone);

                //5.return result
                return sZone;
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while retrieving Zone ID " + ZoneID.ToString();
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while retrieving Zone ID " + ZoneID.ToString()));
            }
        }

        public List<String> GetTariffZoneName(String TariffReference)
        {
            try
            {
                //1.Convert Service Entity to Business Entity
                CarrierAdapter adapter = new CarrierAdapter();

                //2. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                List<String> tariffRef = tariffHandler.GetTariffZoneName(TariffReference);

                //3.return result
                return tariffRef;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem retrieving assigned zone name for the given tariff [" + TariffReference + "]";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem retrieving assigned zone name for the given tariff [" + TariffReference + "]"));
            }

        }

        public List<SZone> GetZoneCoverageList(string TariffReference)
        {
            try
            {
                //1. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                List<BZone> bZone = tariffHandler.GetZoneCoverageList(TariffReference).ToList();

                //2.Convert Service Entity to Business Entity
                TariffAdapter adapter = new TariffAdapter();
                List<SZone> sZone = adapter.ConvertBtoS_Zone(bZone);

                //3.return result
                return sZone;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem retrieving zone coverage list";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem retrieving zone coverage list"));
            }
        }


        /**********************  after 20th December ****************************/

        public List<SSurchargeMaster> GetSurchargeMaster(string TariffReference)
        {
            List<SSurchargeMaster> sSurchargeMaster;
            try
            {

                //1. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                List<BSurchargeMaster> bSurchargeMaster = tariffHandler.GetSurchargeMaster(TariffReference);


                //2.Create Tariff adapter and convert Business to Service entity
                TariffAdapter adapter = new TariffAdapter();
                sSurchargeMaster = adapter.ConvertBtoS_SurchargeMaster(bSurchargeMaster);

                //5.return result
                return sSurchargeMaster;
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while retrieving surcharge master details for the tariff " + TariffReference;
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while retrieving surcharge master details for the tariff " + TariffReference));
            }
        }

        public List<SSurchargeDetails> GetSurchargeDetails(string TariffReference, string SurchargeCode)
        {
            List<SSurchargeDetails> sSurchargeDetails;
            try
            {

                //1. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                List<BSurchargeDetails> bSurchargeMaster = tariffHandler.GetSurchargeDetails(TariffReference, SurchargeCode);


                //2.Create Tariff adapter and convert Business to Service entity
                TariffAdapter adapter = new TariffAdapter();
                sSurchargeDetails = adapter.ConvertBtoS_SurchargeDetail(bSurchargeMaster);

                //5.return result
                return sSurchargeDetails;
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while retrieving surcharge parameters tariff " + TariffReference + " surcharge " + SurchargeCode;
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while retrieving surcharge parameters tariff " + TariffReference + " surcharge " + SurchargeCode));
            }
        }

        public int UpdateSurchargeMater(string SurchargeCode, string MasterServiceName, SEnumFlag Active)
        {
            int result = 1;

            try
            {

                //1. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();

                BEnumFlag bActive = Active == SEnumFlag.Yes ? BEnumFlag.Yes : BEnumFlag.No;

                result = tariffHandler.UpdateSurchargeMater(SurchargeCode, MasterServiceName, bActive);

                //2.return result
                return result;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while updating surcharge [" + SurchargeCode + "]";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while updating surcharge [" + SurchargeCode + "]"));
            }

        }

        public int UpdateSurchargeDetails(List<SSurchargeDetails> sSurchargeDetails)
        {
            int result = 1;

            try
            {
                //1.Convert Service Entity to Business Entity
                TariffAdapter adapter = new TariffAdapter();
                List<BSurchargeDetails> bSurchargeDetails = adapter.ConvertStoB_SurchargeDetail(sSurchargeDetails);


                //2. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                result = tariffHandler.UpdateSurchargeDetails(bSurchargeDetails);

                //3.return result
                return result;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while update surcharge param";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while update surcharge param"));
            }
        }

        public List<SPublicTariffSearchResult> GetPublicTariff(string Name)
        {

            List<SPublicTariffSearchResult> sPublicTariffSearchResult;

            try
            {

                //1. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                List<BPublicTariffSearchResult> bPublicTariffSearchResult = tariffHandler.GetPublicTariff(Name);

                //2.Create Tariff adapter and convert Business to Service entity
                TariffAdapter adapter = new TariffAdapter();
                sPublicTariffSearchResult = adapter.ConvertBtoS_PublicTariffSearchResult(bPublicTariffSearchResult);

                //5.return result
                return sPublicTariffSearchResult;
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while retrieving " + Name + " tariff ";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while retrieving " + Name + " tariff "));
            }
        }

        public int UpdatePublicTariff(List<SPublicTariffSearchResult> sPublicTariffUpdated)
        {

            int result = 1;

            try
            {
                //1. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();

                //2. Adapter to convert service to business entity
                TariffAdapter adapter = new TariffAdapter();
                List<BPublicTariffSearchResult> bPublicTariffSearchResult = adapter.ConvertStoB_PublicTariffSearchResult(sPublicTariffUpdated);

                result = tariffHandler.UpdatePublicTariff(bPublicTariffSearchResult);

                //2.return result
                return result;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while updating public tariff [" + sPublicTariffUpdated[0].Name + "]";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while updating public tariff [" + sPublicTariffUpdated[0].Name + "]"));
            }
        }

      /// <summary>
      /// To check the the cretiria already available in Simulation
      /// </summary>
      /// <param name="Origin"></param>
      /// <param name="Destination"></param>
      /// <param name="MasterType"></param>
      /// <returns></returns>
        public bool isAlreadyExist(string Origin, string Destination, string MasterType, string AccountNo)
        {
            bool result = false;

            try
            {
                //1. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                result = tariffHandler.isAlreadyExist(Origin, Destination, MasterType, AccountNo);

                //2.return result
                return result;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while checking Master type";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while checking Master type"));
            }        
        }
      /*06APR04HM*/
      /// <summary>
      /// 
      /// </summary>
      /// <param name="sTariffMaster"></param>
      /// <returns></returns>
        public string   UpdateMasterTariff(STariffMaster sTariffMaster)
        {
            string  result;
            try
            {
                TariffAdapter adapter = new TariffAdapter();
                BTariffMaster bTariffMaster = adapter.ConvertStoB_TariffMaster(sTariffMaster);

                TariffHandler tr = new TariffHandler();
                result = tr.UpdateMasterTariff(bTariffMaster);
                return result;
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while updating"));
            }
        }
        /*06APR04HV*/
        /// <summary>
        /// To create Carrier Service 
        /// </summary>
        /// <param name="bCarrierService"></param>
        /// <returns></returns>
        public int InsertCarrierService(List<SCarrierService> sCarrierService)
        {
            int result = 1;

            try
            {
                //1.Convert Service Entity to Business Entity
                CarrierAdapter adapter = new CarrierAdapter();

                //2. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();

                for (int i = 0; i < sCarrierService.Count; i++)
                {
                    BCarrierService bCarrierService = adapter.ConvertStoB_CarrierService(sCarrierService[i]);
                    result = tariffHandler.InsertCarrierService(bCarrierService);
                }

                //3.return result
                return result;
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while Inserting carrier service";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while Inserting carrier service"));
            }
        }

        /// <summary>
        /// Get carrier service list
        /// </summary>
        /// <returns></returns>
        public List<SCarrierService> GetCarrierServiceForMaster()
        {
            try
            {
                //1.Convert Service Entity to Business Entity
                CarrierAdapter adapter = new CarrierAdapter();


                //2. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                List<BCarrierService> bCarrierService = tariffHandler.GetCarrierServiceForMaster();


                //3. Convert BFuelSurcharge to SFuelSurcharge before return from service layer
                List<SCarrierService> sFuelSurchargeParameter = adapter.ConvertBtoS_CarrierService(bCarrierService);

                //4.return result
                return sFuelSurchargeParameter;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem retrieving carrier service";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while retrieving carrier service"));
            }


        }

        public SUser GetUserDetailForAssignTariff(string AccountNo)
        {
            try
            {
                //1.Convert Service Entity to Business Entity
                UserAdapter adapter = new UserAdapter();


                //2. Create instance for business object and invoke method            
                TariffHandler tariffHandler = new TariffHandler();
                BUser bUser = tariffHandler.GetUserDetailForAssignTariff(AccountNo);


                //3. Convert BFuelSurcharge to SFuelSurcharge before return from service layer
                SUser sUser = adapter.ConvertBtoS_User(bUser);

                //4.return result
                return sUser;

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem retrieving User details for Assing tariff";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while retrieving User details for Assing tariff"));
            }


        }

        /// <summary>
        /// Get Validate Zone while updating the zone 
        /// </summary>
        /// <returns></returns>
        /// 14MAY12KS
        public string Getvalidatezone(string zonecountry, string tariffreference, string Geographical_coverage, int zone_id)
        {

            string result = "";
            try
            {
                TariffHandler tarrif = new TariffHandler();
                result = tarrif.Getvalidatezone(zonecountry, tariffreference, Geographical_coverage, zone_id);
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [03JAN12RM] */
                SGeneralFault generalFault = new SGeneralFault();
                generalFault.Issue = "Getvalidatezone";
                generalFault.Details = Library.ExtractError(error);

                throw new FaultException<SGeneralFault>
                    (generalFault, new FaultReason(generalFault.Details), new FaultCode(generalFault.Issue));


                //logger.Debug("From service implementation :" + Library.ExtractError(error));
                //var generalFault = new SGeneralFault();
                //generalFault.Issue = "Problem while getting Language resource file name";
                //generalFault.Details = Library.ExtractError(error);
                //throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while getting Language resource file name"));
            }
            return result;

        }  

    #endregion

  }
}
