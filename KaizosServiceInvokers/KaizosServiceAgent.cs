using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ServiceModel;

//carrier manager [14FEB12RM]
using System.Web;
using System.Web.UI; 

using log4net;
using log4net.Config;

using KaizosServiceInvokers.KaizosServiceReference;
using Kaizos.Components.GlobalLibrary;

namespace KaizosServiceInvokers
{
  public class KaizosServiceAgent
  {

    ILog logger = log4net.LogManager.GetLogger(typeof(KaizosServiceAgent));

    /* To be introduce FACADE pattern in case of having more than one service tomorrow when started using more than one service */
      
    #region Tos

      /// <summary>
      /// Insert TOS information through Data Entiry
      /// </summary>
      /// <param name="tos"></param>
      /// <returns>
      /// 0- Success
      /// 1 - Failure
      /// 2 - Data is not exists
      /// </returns>
        public int InsertToS(SToS tos)
    {

      int result=0;
      
      KaizosServiceContractClient kaizosServiceProxy = null;

      try
      {
        //1.Create Proxy
        kaizosServiceProxy = new KaizosServiceContractClient();
        //2. Do operation using Proxy
        result = kaizosServiceProxy.InsertToS(tos);
 
      }

      //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
      //{
      //  logger.Debug("From ServiceAgent:"+ generalFault.Detail);
      //}
      catch (Exception)
      {
        //4.Abort the proxy in case of exception 
        kaizosServiceProxy.Abort();
        throw;
      }
      finally
      {
        //3. always close Proxy (or else client will get timeout error at 11th request.
        ((KaizosServiceContractClient)kaizosServiceProxy).Close();
      }

      return result;
    }

      /// <summary>
      /// Get a active TOS
      /// </summary>
      /// <returns>
      /// Get the active Terms of Service
      /// </returns>
        public SToS GetActiveTos()
        {
            SToS sTos = new SToS();

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                sTos = kaizosServiceProxy.GetActiveToS();

            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sTos;

        }
    #endregion

    #region Address Book

       /// <summary>
        /// Get all available address list
        /// </summary>
        /// <returns>List of address book</returns>
       public List<SAddressBook> GetAddress()
        {
            List<SAddressBook> sAddressList;

            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                sAddressList = kaizosServiceProxy.GetAddress().ToList();
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return sAddressList;
        }

       /// <summary>
       /// To get matching address for the strinb comparing
       /// Company Name and Name
       /// </summary>
       /// <param name="SearchString"></param>
       /// <param name="UsedFor"></param>
       /// <returns></returns>

       /**************24APR12KM*******/
       public List<SAddressBook> GetAddressBookSearch(string SearchString, string UsedFor)
       {
           List<SAddressBook> sAddressList;

           KaizosServiceContractClient kaizosServiceProxy = null;
           try
           {
               //1.Create Proxy
               kaizosServiceProxy = new KaizosServiceContractClient();
               //2. Do operation using Proxy
               sAddressList = kaizosServiceProxy.GetAddress().ToList();
           }

           catch (Exception)
           {
               //4.Abort the proxy in case of exception 
               kaizosServiceProxy.Abort();
               throw;
           }
           finally
           {
               //3. always close Proxy (or else client will get timeout error at 11th request.
               ((KaizosServiceContractClient)kaizosServiceProxy).Close();
           }
           return sAddressList;
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

           KaizosServiceContractClient kaizosServiceProxy = null;

           try
           {
               //1.Create Proxy
               kaizosServiceProxy = new KaizosServiceContractClient();
               //2. Do operation using Proxy
               result = kaizosServiceProxy.UpdateAddress(sAddressBook);
           }
           catch (Exception)
           {
               //4.Abort the proxy in case of exception 
               kaizosServiceProxy.Abort();
               throw;
           }
           finally
           {
               //3. always close Proxy (or else client will get timeout error at 11th request.
               ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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

           KaizosServiceContractClient kaizosServiceProxy = null;

           try
           {
               //1.Create Proxy
               kaizosServiceProxy = new KaizosServiceContractClient();
               //2. Do operation using Proxy
               result = kaizosServiceProxy.DeleteAddress(AddressID);
           }
           catch (Exception)
           {
               //2.Abort the proxy in case of exception 
               kaizosServiceProxy.Abort();
               throw;
           }
           finally
           {
               //3. always close Proxy (or else client will get timeout error at 11th request.
               ((KaizosServiceContractClient)kaizosServiceProxy).Close();
           }

           return result;

       }

       public List<SAddressBook> GetAddressnew(string SearchString, string UsedFor, string zcode, string scountry)
       {
           List<SAddressBook> sAddressList;

           KaizosServiceContractClient kaizosServiceProxy = null;
           try
           {
               //1.Create Proxy
               kaizosServiceProxy = new KaizosServiceContractClient();
               //2. Do operation using Proxy
               sAddressList = kaizosServiceProxy.GetAddress().ToList();
           }

           catch (Exception)
           {
               //4.Abort the proxy in case of exception 
               kaizosServiceProxy.Abort();
               throw;
           }
           finally
           {
               //3. always close Proxy (or else client will get timeout error at 11th request.
               ((KaizosServiceContractClient)kaizosServiceProxy).Close();
           }
           return sAddressList;
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

            int result = 0;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.ValidateUser(userName, password);

            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            SUser sUser;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                sUser = kaizosServiceProxy.GetLogin(Username);

            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sUser;
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
            SFranchise sFranchise;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                sFranchise = kaizosServiceProxy.GetFranchise(UserName);

            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sFranchise;
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
            SCustomer sCustomer;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                sCustomer = kaizosServiceProxy.GetCustomer(UserName);

            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sCustomer;
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
        public int UpdateLastLogin(string USername, DateTime CurrentDateTime)
        {
            int result = 1;
            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.UpdateLastLogin(USername, CurrentDateTime);
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;
        }

        /// <summary>
        /// To get the functional list
        /// </summary>
        /// <returns>Result set </returns>
        public List<SFunctionality> GetFunctionality()
        {
            List<SFunctionality> sFunctionality;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                sFunctionality = kaizosServiceProxy.GetFunctionality().ToList();

            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sFunctionality;
        }

        /// <summary>
        /// To get the functional-profile mapping list
        /// </summary>
        /// <returns>Result set </returns>
        public List<SFunctionalProfile> GetFunctionalProfile()
        {
            List<SFunctionalProfile> sFunctionalProfile;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                sFunctionalProfile = kaizosServiceProxy.GetFunctionalProfile().ToList();

            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sFunctionalProfile;
        }

        /// <summary>
        /// To insert functional-profile mapping list
        /// </summary>
        /// <returns>int </returns>
        public int InsertFunctionalProfile(List<SFunctionalProfile> lsFunctionalProfile)
        {
            int result = 1;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.InsertFunctionalProfile(lsFunctionalProfile.ToArray());
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;
        }

        /// <summary>
        /// To delete all functional-profile mapping list
        /// </summary>
        /// <returns>int </returns>
        public int DeleteFunctionalProfile()
        {
            int result = 1;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.DeleteFunctionalProfile();
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;
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
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                return (kaizosServiceProxy.ValidateEmail(EmailID));
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
        }

        /// <summary>
        /// To Insert user details for Franchise User
        /// </summary>
        /// <param name="CountryCode"></param>
        /// <returns>integer</returns>
        public int InsertFranchise(SFranchise sFranchise)
        {
            int result = 1;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.InsertFranchise(sFranchise);
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.UpdateFranchise(sFranchise);
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.InsertMonthlyFee(sMonthlyFee);
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        }

        /// <summary>
        /// To get Monthly fees list for a account
        /// </summary>
        /// <returns>set of record for all shipment type for a account </returns>
        public List<SMonthlyFee> GetMonthlyFees(string UserId)
        {
            List<SMonthlyFee> sMonthlyFee;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                sMonthlyFee = kaizosServiceProxy.GetMonthlyFees(UserId.Trim()).ToList();

            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return sMonthlyFee;
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
            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.ConfirmPassword(AccountNo.Trim(), Password.Trim());
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetLanguageResource(strCountryCode, strLanguageCode);
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.CustomerServiceUpdate(AccountNo, Pwd, LanguageCode);
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetCustomerServiceList().ToList();
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.CustomerServiceUpdateAdmin(UserID, Status);
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetIndustry().ToList();
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.InsertCustomer(sCustomer);
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.InsertEndCustomer(sCustomer);
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.ValidateHQZipcode(HQZipcode.Trim());
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.UpdateEndCustomerByAdmin(sCustomer);
            }
            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetCustomerCredit(AccountNo, UserType).ToList();
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.UpdateCustomerCredit(AccountNo.Trim(), WishedAmt, InsuredAmt, PaymentDelayDay, GrossMargin, PaymentDelayMonth, CompensationRate, AuthorizedCreditAmt, DeferredPaymentAgreed);
            }
            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            List<SAuthorized> result = null;
            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetAuthorizedList(AccountNo.Trim()).ToList();
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.UpdateAuthorize(AccountNo, CompanyName, Email, PhoneNo, RefAccountNo);
            }
            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;
        }

        /// <summary>
        /// To insert a autorized User details
        /// </summary>
        /// <param name="sAuthorize"></param>
        /// <returns></returns>
        public int InsertAuthorized(SAuthorized sAuthorize)
        {
            int result = 1;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.InsertAuthorized(sAuthorize);
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.UpdateAuthorizedSelf(AccountNo, Password, TelNo);
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            SAuthorized sAuthorized;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                sAuthorized = kaizosServiceProxy.GetAuthorized(AccountNo);
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sAuthorized;
        }

        /// <summary>
        /// To get End customer list user list
        /// </summary>
        /// <returns>company name, contact name, emailID for N2 user </returns>
        public List<SFranchiseContact> GetEndCustomerList()
        {
            List<SFranchiseContact> result = null;
            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetEndCustomerList().ToList();
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetUser(accountNumber.Trim());
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.InsertAdv(sAdv.ToArray());
            }

            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.UpdateEndCustomer(sCustomer);
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;

        }

        public SCustomer GetEndCustomer(string UserId)
        {
            SCustomer sCustomer;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                sCustomer = kaizosServiceProxy.GetEndCustomer(UserId.Trim());
            }

            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sCustomer;
        }

        public int UpdateFranchiseAdmin(SFranchise sFranchise)
        {
            int result = 1;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.UpdateFranchiseAdmin(sFranchise);
            }

            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        }

        public int DeleteAuthorized(string AccountNo)
        {
            int result = 1;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.DeleteAuthorized(AccountNo.Trim());
            }

            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        }

        public int InsertCarrierAccountRef(string[] CarrierAccList, string[] CarrierAcc, string AccountNo)
        {
            int result = 1;
            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.InsertCarrierAccountRef(CarrierAccList, CarrierAcc, AccountNo);
            }

            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;
        }


        public string GetPostalCode(string strCountryCode)
        {
            string strFormat = string.Empty;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //  2. Do operation using Proxy
                strFormat = kaizosServiceProxy.GetPostalCode(strCountryCode);
            }

            catch (Exception)
            {
                // 4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                // 3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return strFormat;


        }
        #endregion

    #region Common
        /// <summary>
        /// To retrieve a particular colum to fill list/combo controls
        /// </summary>
        /// <param name="sComboTableField"></param>
        /// <returns></returns>
        public List<SComboText> FillCombo(SComboTableField sComboTableField)
    {
        KaizosServiceContractClient kaizosServiceProxy = null;

        List<SComboText> sComboText;

        try
        {
            //1.Create Proxy
            kaizosServiceProxy = new KaizosServiceContractClient();

            //2. Do operation using Proxy
            sComboText = kaizosServiceProxy.FillCombo(sComboTableField).ToList();

        }

        catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
        {
            logger.Debug("From ServiceAgent:" + generalFault.Detail);
            kaizosServiceProxy.Abort();
            throw;
        }
        catch (Exception)
        {
            //4.Abort the proxy in case of exception 
            kaizosServiceProxy.Abort();
            throw;
        }
        finally
        {
            //3. always close Proxy (or else client will get timeout error at 11th request.
            ((KaizosServiceContractClient)kaizosServiceProxy).Close();
        }

        return sComboText;
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
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.EncryptString(Message, Passphrase);
            }
            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.DecryptString(Message, Passphrase);
            }
            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;


        }

        public List<String> GetFieldValue(SComboTableField sComboTableField)
        {
            KaizosServiceContractClient kaizosServiceProxy = null;

            List<String> FieldValue;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                FieldValue = kaizosServiceProxy.GetFieldValue(sComboTableField).ToList();

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return FieldValue;
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
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.ValidateZipCode(PostalCode.Trim(), CountryCode.Trim());
            }
            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                return (kaizosServiceProxy.GetLanguage(CountryCode));
            }
            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
        }

        /// <summary>
        /// To get language list
        /// </summary>
        /// <returns>
        /// result or null
        /// </returns>
        public List<string> GetLanguageList()
        {
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                return (kaizosServiceProxy.GetLanguageList().ToList());
            }
            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
        }

        /// <summary>
        /// To get Franchise user list
        /// </summary>
        /// <returns>company name, contact name, emailID for N2 user </returns>
        public List<SFranchiseContact> GetFranchiseList()
        {
            KaizosServiceContractClient kaizosServiceProxy = null;

            List<SFranchiseContact> sFranchise;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                sFranchise = kaizosServiceProxy.GetFranchiseList().ToList();
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sFranchise;

        }

        /// <summary>
        /// To fill country
        /// </summary>
        /// <returns></returns>
        public List<SCountryTable> FillCountryCombo()
        {
            List<SCountryTable> sCountryTable = new List<SCountryTable>();

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                sCountryTable = kaizosServiceProxy.FillCountryCombo().ToList();
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sCountryTable;
        }

        /// <summary>
        /// To get counter value
        /// </summary>
        /// <param name="KeyCode"></param>
        /// <param name="CounterType"></param>
        /// <param name="RequiredCount"></param>
        /// <returns></returns>
        public SNextcounter GetNextCounter(string KeyCode, string CounterType, int RequiredCount)
        {
            SNextcounter sNextcounter = new SNextcounter();

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                sNextcounter = kaizosServiceProxy.GetNextCounter(KeyCode, CounterType, RequiredCount);

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sNextcounter;
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
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.ValidatePassword(password.Trim());
            }
            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            string result = null;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.CreatePassword(length);
            }
            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;


        }

        /// <summary>
        /// To send an e-mail to auditors
        /// </summary>
        /// <param name="UserID">MailID</param>
        /// <returns></returns>
        public bool sendMessage(string UserID, string Password, string CurrentUserID)
        {
            bool result = false;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.sendMessage(UserID, Password, CurrentUserID);
            }
            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;


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
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.CityZipcodeAutoFill(SearchString, Country, Count).ToList();
            }
            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.ValidateTime(strTime.Trim());
            }
            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                sFileImportStatus = kaizosServiceProxy.ImportAddressBook(addressBookFile, AccountNo).ToList(); ;
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sFileImportStatus;
        }

        /// <summary>
        /// To get Customer designation list
        /// </summary>
        /// <returns> Designation list</returns>
        public List<string> GetFunction()
        {
            List<string> result = null;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetFunction().ToList();
            }
            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.isNumericValidation(val, NumberStyle);
            }
            
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.isAlphaNumericValidation(val);
            }
           catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;        
        }

        /// <summary>
        /// Used to get Key value for the code
        /// </summary>
        /// <param name="KeycCode"></param>
        /// <returns></returns>
        public SKeyValue GetValueFromParameter(string KeycCode)
        {
            SKeyValue sKeyValue;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                sKeyValue = kaizosServiceProxy.GetValueFromParameter(KeycCode);
            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sKeyValue;
        }

        /// <summary>
    /// To send an e-mail to the particular user to confirm the password
    /// </summary>
    /// <param name="UserID">MailID</param>
    /// <returns></returns>
        public bool sendConfirmPassord(string UserId, string CurrentUserID)
        {
            bool result = false;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.sendConfirmPassord(UserId, CurrentUserID);
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;
        }

        public bool IsValidZidcode(string Zipcode)
        {
            bool result = false;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.IsValidZidcode(Zipcode);
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;
        
        }

       /// <summary>
    /// To get the list reference carriers
    /// </summary>
    /// <returns>List of carriers</returns>
        public List<string> GetAllRefCarrierList()
        {
            List<string> result = null;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetAllRefCarrierList().ToList();
            }

            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
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
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.ValidatePostalcode(strActualFormat, strZipcode);
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;
        }

        public bool sendMail(string To, string CurrentUserID, string strBody, string Subject)
        {
            bool result = false;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.sendMail(To, CurrentUserID, strBody, Subject);
            }
            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;


        }

    #endregion

    #region Tariff

        public int CreateMasterService(SMasterServiceType sMasterServiceType)
        {

            int result = 0;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.CreateMasterService(sMasterServiceType);

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        }

        public List<SFuelSurcharge> GetFuelCharge(string tariffType, string keyAccountRef)
        {

            KaizosServiceContractClient kaizosServiceProxy = null;

            List<SFuelSurcharge> sFuelCharge;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                sFuelCharge = kaizosServiceProxy.GetFuelCharge(tariffType, keyAccountRef).ToList();

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sFuelCharge;

        }

        public List<SFuelSurchargeParameter> GetFuelChargeParameter(int serviceID)
        {


            KaizosServiceContractClient kaizosServiceProxy = null;

            List<SFuelSurchargeParameter> sFuelSurchargeParameter;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                sFuelSurchargeParameter = kaizosServiceProxy.GetFuelChargeParameter(serviceID).ToList();

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sFuelSurchargeParameter;


        }

        public int UpdateFuelChargeParameter(List<SFuelSurchargeParameter> sFuelSurchargeParameter)
        {
            int result = 0;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.UpdateFuelChargeParameter(sFuelSurchargeParameter.ToArray());

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        }

        public int UpdateFuelChargeStartDate(int ServiceId, DateTime startDate)
        {
            int result = 0;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.UpdateFuelChargeStartDate(ServiceId, startDate);

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;


        }

        public List<SCarrierService> GetCarrierService()
        {

            KaizosServiceContractClient kaizosServiceProxy = null;

            List<SCarrierService> sCarrierService;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                sCarrierService = kaizosServiceProxy.GetCarrierService().ToList();

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sCarrierService;

        }

        // To update list of carrier service 17FEB12RM
        public int UpdateCarrierService(List<SCarrierService> sCarrierService)
        {

            int result = 0;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.UpdateCarrierService(sCarrierService.ToArray());
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;

        }

        public List<SDeliveryDelay> GetCarrierServiceDeliveryDelay(int serviceID)
        {

            KaizosServiceContractClient kaizosServiceProxy = null;

            List<SDeliveryDelay> sDeliveryDelay;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                sDeliveryDelay = kaizosServiceProxy.GetCarrierServiceDeliveryDelay(serviceID).ToList();

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sDeliveryDelay;

        }

        public int UpdateCarrierServiceDelay(int ServiceID, string Origin, string Destination, int Delay)
        {
            int result = 0;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.UpdateCarrierServiceDelay(ServiceID, Origin, Destination, Delay);

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;

        }

        public List<SFileImportStatus> ImportDeliveryDelay(int ServiceID, byte[] stream)
        {
            //int result = 0;

            List<SFileImportStatus> sFileImportStatus;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                sFileImportStatus = kaizosServiceProxy.ImportDeliveryDelay(ServiceID, stream).ToList(); //24JAN12RM

            }

            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sFileImportStatus; //24JAN12RM
        }


        /**********************  after 13th December ****************************/

        /// <summary>
        /// Acknowledgement of Mast Tariff Creation
        /// </summary>
        /// <param name="sTariffMaster"></param>
        /// <returns></returns>
        public STariffCreationAcknowledgement CreateMasterTariff(STariffMaster sTariffMaster)
        {
            KaizosServiceContractClient kaizosServiceProxy = null;

            STariffCreationAcknowledgement sTariffCreationAck;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                sTariffCreationAck = kaizosServiceProxy.CreateMasterTariff(sTariffMaster);

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sTariffCreationAck;
        }

        /// <summary>
        /// To retrieve list of tariff reference for which tariff to be imported
        /// </summary>
        /// <param name="CarrierName"></param>
        /// <returns></returns>
        public List<String> GetOpenImportTariff(String CarrierName)
        {
            KaizosServiceContractClient kaizosServiceProxy = null;

            List<String> tariffRef;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                tariffRef = kaizosServiceProxy.GetOpenImportTariff(CarrierName).ToList();

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return tariffRef;
        }

        /// <summary>
        /// To import Tariff
        /// </summary>
        /// <param name="CarrierName"></param>
        /// <param name="TariffReference"></param>
        /// <param name="sTariffCalculationRule"></param>
        /// <param name="tariffFile"></param>
        /// <returns></returns>
        public List<SFileImportStatus> ImportTariff(string CarrierName, string TariffReference, List<STariffCalculationRule> sTariffCalculationRule, byte[] tariffFile)
        {

            List<SFileImportStatus> sFileImportStatus;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();



                //2. Do operation using Proxy
                sFileImportStatus = kaizosServiceProxy.ImportTariff(CarrierName, TariffReference, sTariffCalculationRule.ToArray(), tariffFile).ToList();

            }

            /* 18JAN12RM */
            //catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            //{
            //    logger.Debug("From ServiceAgent:" + generalFault.Detail);
            //    kaizosServiceProxy.Abort();
            //    throw;
            //}
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sFileImportStatus;

        }

        /// <summary>
        /// Retrieve all specified type of Tariff assigned for the given carrier 
        /// </summary>
        /// <param name="CarrierName"></param>
        /// <param name="TariffType"></param>
        /// <returns></returns>
        public List<STariffReferenceList> GetTariffReference(String CarrierName, String TariffType)
        {
            KaizosServiceContractClient kaizosServiceProxy = null;

            List<STariffReferenceList> tariffRef;


            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                tariffRef = kaizosServiceProxy.GetTariffReference(CarrierName, TariffType).ToList();

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return tariffRef;

        }

        /// <summary>
        /// To update Tariff details for the given Tariff Reference
        /// </summary>
        /// <param name="TariffReference"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Archived"></param>
        /// <returns></returns>
        public int UpdateTariffReference(string TariffReference, DateTime StartDate, DateTime EndDate, bool Archived)
        {
            int result = 0;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.UpdateTariffReference(TariffReference, StartDate, EndDate, Archived);
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);

                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;

        }

        public int CreateZone(SZone zone, string Flag, int ZoneID)
        {
            int result = 0;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.CreateZone(zone, Flag, ZoneID);
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);

                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;

        }

        public List<SZoneSearchDetails> GetZoneDetails(string TariffReference)
        {
            List<SZoneSearchDetails> sZoneSearchDetails;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                sZoneSearchDetails = kaizosServiceProxy.GetZoneSearchDetails(TariffReference).ToList();

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sZoneSearchDetails;
        }

        public SZone GetZoneDetails(int ZoneID)
        {
            SZone sZoneList;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                sZoneList = kaizosServiceProxy.GetZoneDetails(ZoneID);

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sZoneList;

        }

        public List<String> GetTariffZoneName(String TariffReference)
        {
            KaizosServiceContractClient kaizosServiceProxy = null;

            List<String> zoneName;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                zoneName = kaizosServiceProxy.GetTariffZoneName(TariffReference).ToList();
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return zoneName;
        }

        public List<SZone> GetZoneCoverageList(string TariffReference)
        {
            List<SZone> sZoneCoverageList;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                sZoneCoverageList = kaizosServiceProxy.GetZoneCoverageList(TariffReference).ToList();

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sZoneCoverageList;

        }


        /**********************  after 20th December ****************************/

        public List<SSurchargeMaster> GetSurchargeMaster(string TariffReference)
        {
            List<SSurchargeMaster> sSurchargeMaster;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                sSurchargeMaster = kaizosServiceProxy.GetSurchargeMaster(TariffReference).ToList();

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sSurchargeMaster;
        }

        public List<SSurchargeDetails> GetSurchargeDetails(string TariffReference, string SurchargeCode)
        {
            List<SSurchargeDetails> sSurchargeDetails;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                sSurchargeDetails = kaizosServiceProxy.GetSurchargeDetails(TariffReference, SurchargeCode).ToList();

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sSurchargeDetails;
        }

        public int UpdateSurchargeMater(string SurchargeCode, string MasterServiceName, SEnumFlag Active)
        {
            int result = 0;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.UpdateSurchargeMater(SurchargeCode, MasterServiceName, Active);

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;

        }

        public int UpdateSurchargeDetails(List<SSurchargeDetails> sSurchargeDetails)
        {
            int result = 0;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.UpdateSurchargeDetails(sSurchargeDetails.ToArray());
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;

        }

        public List<SPublicTariffSearchResult> GetPublicTariff(string Name)
        {

            List<SPublicTariffSearchResult> sPublicTariffSearchResult;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                sPublicTariffSearchResult = kaizosServiceProxy.GetPublicTariff(Name).ToList();

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sPublicTariffSearchResult;

        }

        public int UpdatePublicTariff(List<SPublicTariffSearchResult> sPublicTariffUpdated)
        {
            int result = 0;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.UpdatePublicTariff(sPublicTariffUpdated.ToArray());

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
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

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.isAlreadyExist(Origin, Destination, MasterType, AccountNo);
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        
        }
     /*06APR12KM*/
      /// <summary>
     /// 
     /// </summary>
     /// <param name="sTariffMaster"></param>
     /// <returns></returns>
        public string  Updatetarrifmaster(STariffMaster sTariffMaster)
        {
            string  result;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                kaizosServiceProxy = new KaizosServiceContractClient();
                result = kaizosServiceProxy.UpdateMasterTariff(sTariffMaster);
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        }


        public List<SCarrierService> GetCarrierServiceForMaster()
        {

            KaizosServiceContractClient kaizosServiceProxy = null;

            List<SCarrierService> sCarrierService;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                sCarrierService = kaizosServiceProxy.GetCarrierServiceForMaster().ToList();

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sCarrierService;

        }
        // To update list of carrier service 17FEB12RM
        public int InsertCarrierService(List<SCarrierService> sCarrierService)
        {

            int result = 0;

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                result = kaizosServiceProxy.InsertCarrierService(sCarrierService.ToArray());
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;

        }

        public SUser GetUserDetailForAssignTariff(string AccountNo)
        {

            KaizosServiceContractClient kaizosServiceProxy = null;

            SUser sUser = new SUser();

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                sUser = kaizosServiceProxy.GetUserDetailForAssignTariff(AccountNo);

            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sUser;

        }

        /*14MAY12KS*/
        public string Getvalidatezone(string zonecountry, string tariffreference, string Geographical_coverage, int zone_id)
        {
            KaizosServiceContractClient kaizosServiceProxy = null;
            string result = "";
            try
            {
                kaizosServiceProxy = new KaizosServiceContractClient();
                result = kaizosServiceProxy.Getvalidatezone(zonecountry, tariffreference, Geographical_coverage, zone_id);

            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;
        }

    #endregion

    #region Shipping service Agent

        public List<SShipmentQuotation> GetQuote(SShipmentOrder sShipmentOrder)
        {
            List<SShipmentQuotation> sShipmentQuotation = new List<SShipmentQuotation>();

            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                sShipmentQuotation = kaizosServiceProxy.GetQuote(sShipmentOrder).ToList();
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sShipmentQuotation;
        }

        public List<SShipmentOrder> GetShipmentDetails(int iGroupID, string strSessionID, string strStatus, string strAccountNo)
        {
            List<SShipmentOrder> sShipmentOrder = new List<SShipmentOrder>();
            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                sShipmentOrder = kaizosServiceProxy.GetShipmentDetails(iGroupID, strSessionID, strStatus, strAccountNo).ToList();
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sShipmentOrder;
        }

        public SShipmentOrder GetOrderInformation(string ShipmentReference)
        {
            SShipmentOrder sShipmentOrder = new SShipmentOrder();
            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                //2. Do operation using Proxy
                sShipmentOrder = kaizosServiceProxy.GetOrderInformation(ShipmentReference);
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sShipmentOrder;
        }

        public int CreateSingleShipment(SShipmentOrder sShipmentOrder, SShipmentQuotation sShipmentQuotation, string strSessionID)
        {
            int result;

            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.CreateSingleShipment(sShipmentOrder, sShipmentQuotation, strSessionID);
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        }

        public int ConfirmShipment(SShipmentOrder sShipmentOrder)
        {
            int result = 0;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.ConfirmShipment(sShipmentOrder);
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;

        }

        public SPaymentInfo GetPaymentMethodAndInfo(string strAccountNo, string strUserType)
        {
            SPaymentInfo sPaymentInfo = new SPaymentInfo();

            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                sPaymentInfo = kaizosServiceProxy.GetPaymentMethodAndInfo(strAccountNo, strUserType);
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return sPaymentInfo;
        }

        public int DeferredPayment(List<string> ClosedReference, string AccountNo, float fTotalAmount)
        {
            int result;

            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.DeferredPayment(ClosedReference.ToArray(), AccountNo, fTotalAmount);
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;
        }

        public int DeleteShipment(string ShipmentReference)
        {
            int result;

            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.DeleteShipment(ShipmentReference);
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;
        }

        public int CancelShipment(string ShipmentReference)
        {
            int result;

            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.CancelShipment(ShipmentReference);
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;
        }


        public int CarrierProcess(List<SShipmentOrder> sShipmentOrder)
        {
            int result = 0;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.CarrierProcess(sShipmentOrder.ToArray());
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;


        }


        public List<string> GetPublicTariffNames()
        {
            List<string> result = new List<string>();
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetPublicTariffNames().ToList();
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;

        }

      // 19JAN12 HN
        public List<STariffMaster> GetCarrierTariffNames(string TariffType, string Carrier)
        {
            List<STariffMaster> result = new List<STariffMaster>();
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetCarrierTariffNames(TariffType, Carrier).ToList();
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;

        }

        public List<STariffMaster> GetTariffAssignedCarrierNames(string TariffType)
        {
            List<STariffMaster> result = new List<STariffMaster>();
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetTariffAssignedCarrierNames(TariffType).ToList();
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;

        }

        public List<SShipmentQuotation> GetQuoteForTool(List<STariffMaster> sTariffMaster, string AssignedTariff, string UODType, string Origin, string Destination, string MasterServiceName, float Weight)
        {
            List<SShipmentQuotation> result = new List<SShipmentQuotation>();
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetQuoteForTool(sTariffMaster.ToArray(), AssignedTariff, UODType, Origin, Destination, MasterServiceName, Weight).ToList();
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;


        }

        /// <summary>
        /// To get the list available carriers
        /// </summary>
        /// <returns>List of carriers</returns>
        public List<STariffMaster> GetAllCarrierNames()
        {
            List<STariffMaster> result = new List<STariffMaster>();

            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetAllCarrierNames().ToList();
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        }

        /// <summary>
        /// To insert Simulation header details : Default Tariff , Validity, Weight detail
        /// </summary>
        /// <param name="bSimulationHeader"></param>
        /// <returns></returns>
        public int SimulationHeaderInsert(SSimulationHeader sSimulationHeader)
        {
            int result = 0;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.SimulationHeaderInsert(sSimulationHeader);
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        }

        /// <summary>
        /// To insert Customer Surcharge discount for simulation process
        /// </summary>
        /// <param name="bSimulationSurchargeDiscount"></param>
        /// <returns></returns>
        public int SimulationSurchargeDiscount(List<SSimulationSurchargeDiscount> sSimulationSurchargeDiscount)
        {
            int result = 0;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.SimulationSurchargeDiscount(sSimulationSurchargeDiscount.ToArray());
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;
        }

        /// <summary>
        /// To insert Simulated tariff for the customer
        /// </summary>
        /// <param name="bSimulationTariff"></param>
        /// <returns></returns>
        public int SimulationTariff(List<SSimulationTariff> sSimulationTariff)
        {
            int result = 0;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.SimulationTariff(sSimulationTariff.ToArray());
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;
        }

        /// <summary>
        /// To insert the Tariff details used for simulation
        /// </summary>
        /// <param name="bSimulationTariffBased"></param>
        /// <returns></returns>
        public int SimulationTariffBased(List<SSimulationTariffBased> sSimulationTariffBased)
        {
            int result = 0;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.SimulationTariffBased(sSimulationTariffBased.ToArray());
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;
        }


        /// <summary>
        /// To insert Sub total details
        /// </summary>
        /// <param name="sSimulationSubTotal"></param>
        /// <returns></returns>
        public int SimulationSubTotal(SSimulationSubTotal sSimulationSubTotal)
        {
            int result = 0;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.SimulationSubTotal(sSimulationSubTotal);
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        }

        /// <summary>
        /// To get the list of Assigned users
        /// </summary>
        /// <param name="AccountNo"></param>
        /// <returns></returns>
        public List<SUserID> GetAssignedUsers(string AccountNo)
        {
            List<SUserID> result = new List<SUserID>();
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetAssignedUsers(AccountNo).ToList();
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }


            return result;
        }

        /// <summary>
        /// To get the list of Simulation IDs of given Account numbers
        /// </summary>
        /// <param name="AccountNo"></param>
        /// <returns></returns>
        public List<SSimulationList> GetSimulationID(string UserName)
        {
            List<SSimulationList> result = new List<SSimulationList>();

            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetSimulationID(UserName).ToList();
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        }

        /// <summary>
        /// To get the simulated tariff for the Selected Simulation ID 
        /// </summary>
        /// <param name="SimulationID"></param>
        /// <returns></returns>
        public List<SSimulationTariff> GetSimulationTariff(string SimulationID)
        {
            List<SSimulationTariff> result = new List<SSimulationTariff>();

            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetSimulationTariff(SimulationID).ToList();
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        }

        /// <summary>
        /// To get the header details for the given Simulation ID
        /// </summary>
        /// <param name="SimulationID"></param>
        /// <returns></returns>
        public SSimulationHeader GetSimulationHeader(string SimulationID)
        {
            SSimulationHeader result = new SSimulationHeader();
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetSimulationHeader(SimulationID);
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        }

        /// <summary>
        /// To get the Customer Discounts of Surcharges for Simulation
        /// </summary>
        /// <param name="SimulationID"></param>
        /// <returns></returns>
        public List<SSimulationSurchargeDiscount> GetSimulationSurchargeDiscount(string SimulationID)
        {
            List<SSimulationSurchargeDiscount> result = new List<SSimulationSurchargeDiscount>();

            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetSimulationSurchargeDiscount(SimulationID).ToList();
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        }

        /// <summary>
        /// To get the Tariff list to calculate the Simulation 
        /// </summary>
        /// <param name="SimulationID"></param>
        /// <returns></returns>
        public List<SSimulationTariffBased> GetSimulationTariffBased(string SimulationID)
        {
            List<SSimulationTariffBased> result = new List<SSimulationTariffBased>();

            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetSimulationTariffBased(SimulationID).ToList();
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;
        }

        /// <summary>
        /// To get the calculated Sub totals of the given simulation ID
        /// </summary>
        /// <param name="SimulationID"></param>
        /// <returns></returns>
        public SSimulationSubTotal GetSimulationSubTotal(string SimulationID)
        {
            SSimulationSubTotal result = new SSimulationSubTotal();

            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetSimulationSubTotal(SimulationID);
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        }

        List<string> GetDiscount(string AccountNo, List<STariffMaster> sTariffMaster, List<float> Weight, string Origin, string Destination, string MasterServiceName)
        {
            List<string> Discount = new List<string>();
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                Discount = kaizosServiceProxy.GetDiscount(AccountNo, sTariffMaster.ToArray(), Weight.ToArray(), Origin, Destination, MasterServiceName).ToList();
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }


            return Discount;
        }

        /************** after 30JAN12HN *******************/

        /// <summary>
        /// To get Surcharge for Simulation tool
        /// </summary>
        /// <param name="TariifReference"></param>
        /// <param name="Origin"></param>
        /// <param name="Destination"></param>
        /// <param name="Weight"></param>
        /// <param name="MasterServiceName"></param>
        /// <returns></returns>
        public SSurchargeDetails GetSimulationSurcharge(string TariifReference, string Origin, String Destination, float Weight, string MasterServiceName)
        {
            SSurchargeDetails result = new SSurchargeDetails();

            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetSimulationSurcharge(TariifReference, Origin, Destination, Weight, MasterServiceName);
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }


            return result;
        }


        /// <summary>
        /// To get Surcharge for Simulation tool
        /// </summary>
        /// <param name="TariifReference"></param>
        /// <param name="Origin"></param>
        /// <param name="Destination"></param>
        /// <param name="Weight"></param>
        /// <param name="MasterServiceName"></param>
        /// <returns></returns>
        public float GetFuelSurcharge(string TariifReference, string Origin, String Destination, float Weight, string MasterServiceName, float TariffAmount)
        {
            float result = 0;

            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetFuelSurcharge(TariifReference, Origin, Destination, Weight, MasterServiceName, TariffAmount);
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SimulationID"></param>
        /// <returns></returns>
        public int DeleteSimulationID(string SimulationID)
        {
            int result = 0;

            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.DeleteSimulationID(SimulationID);
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }


            return result;
        }

        /// <summary>
        /// To get the calculated Sub totals of the given simulation ID
        /// </summary>
        /// <param name="SimulationID"></param>
        /// <returns></returns>
        public List<SSimulationSubTotal> GetSimulationGrossTotal(string SimulationID, string AccountNo)
        {
            List<SSimulationSubTotal> result = new List<SSimulationSubTotal>();

            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.GetSimulationGrossTotal(SimulationID, AccountNo).ToList();
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        }

        /// <summary>
        /// To get the Email from Broker Insurance
        /// </summary>
        /// <param name="SimulationID"></param>
        /// <returns></returns>

        public string getBrokerEmailId()
        {
            string result;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                result = kaizosServiceProxy.getBrokerEmailId();
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;

        }

        public int UpdateBrokerEmailId(string Email)
        {
            int result;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                // result = kaizosServiceProxy.update();
                result = kaizosServiceProxy.UpdateBrokerEmailId(Email);

            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        }

        /// <summary>
        /// To update Carrier details for the given reference
        /// </summary>
        /// <param name="ShipmentReference"></param>
        /// <param name="TrackingNumber"></param>
        /// <param name="ParameterValues"></param>
        /// <returns></returns>
        public int ShipmentDetailUpdate(string ShipmentReference, string TrackingNumber, string ParameterValues)
        {
            int result = 0;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.ShipmentDetailUpdate(ShipmentReference, TrackingNumber, ParameterValues);
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        }

        /*01MAR12HN*/
        /// <summary>
        /// To Assign a calculated tariff using Simulation Tool
        /// </summary>
        /// <param name="sSimulationTariff"></param>
        /// <returns></returns>
        public int SimulationAssign(List<SSimulationTariff> sSimulationTariff)
        {
            int result = 0;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.SimulationAssign(sSimulationTariff.ToArray());
            }

            catch (FaultException<SGeneralFault> generalFault) //client handles WCF Fault
            {
                logger.Debug("From ServiceAgent:" + generalFault.Detail);
                kaizosServiceProxy.Abort();
                throw;
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }
            return result;
        }

        #endregion Shipping service agent

    #region CarrierIntegration

        public bool CheckForDuplicateCarrierName(string sCarrierName)
        {

            bool result;
            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.CheckForDuplicateCarrierName(sCarrierName);
            }

           
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;
        }

        public int CreateCarrier(SCarrier sCarrier)
        {
            int result;
            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.CreateCarrier(sCarrier);
            }

           
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;

        }

        public int UpdateCarrier(SCarrier sCarrier)
        {
            int result;
            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                result = kaizosServiceProxy.UpdateCarrier(sCarrier);
            }

           
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return result;

        }

        public List<SCarrier> GetCarriers()
        {
            List<SCarrier> lstCarrier;
            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                lstCarrier = kaizosServiceProxy.GetCarriers().ToList<SCarrier>();
            }

           
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return lstCarrier;
        }

        public SCarrier GetCarrier(string sCarrierName)
        {
            SCarrier sCarrier;
            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();

                //2. Do operation using Proxy
                sCarrier = kaizosServiceProxy.GetCarrier(sCarrierName);
            }

          
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sCarrier;

        }

        public SShipmentOrder CarrierProcessing(SShipmentOrder sShipmentOrder, SEnumCarrierProcess sEnumCarrierProcess)
        {
            SShipmentOrder sShipmentOrderResult = new SShipmentOrder();
            KaizosServiceContractClient kaizosServiceProxy = null;

            try
            {
                //1.Create Proxy
                kaizosServiceProxy = new KaizosServiceContractClient();
                SCarrierProcessResult sCarrierProcessResult;
                //2. Do operation using Proxy
                sShipmentOrderResult = kaizosServiceProxy.CarrierProcessing(out sCarrierProcessResult, sShipmentOrder, sEnumCarrierProcess);
                string shipresult = string.Empty;
                if (sEnumCarrierProcess == SEnumCarrierProcess.Label)
                {
                    if (sShipmentOrderResult.ShipmentResult.isLabelGenerated == SEnumFlag.Yes)
                    {
                        if (System.Web.HttpContext.Current.Session["shipresult"] == null)
                        {

                            System.Web.HttpContext.Current.Session["shipresult"] = sShipmentOrderResult.ShipReference.ToString() + "!";
                            // System.Web.HttpContext.Current.Session["shown"] = "1";
                        }
                        else
                        {
                            shipresult = System.Web.HttpContext.Current.Session["shipresult"].ToString();
                            System.Web.HttpContext.Current.Session["shipresult"] = shipresult + sShipmentOrderResult.ShipReference.ToString() + "!";

                        }

                        //Page page = (Page)HttpContext.Current.Handler;
                        //string url = string.Empty;

                        //if (System.Web.HttpContext.Current.Session["shown"].ToString() == "1")
                        //{
                        //    url = page.ResolveUrl(@"frmcarrierLabelDisplay.aspx");
                        //    string url2 = page.Request.Url.AbsoluteUri.Substring(0, page.Request.Url.AbsoluteUri.IndexOf(page.Request.Url.LocalPath));
                        //    System.Web.HttpContext.Current.Response.Write("<script>");
                        //    System.Web.HttpContext.Current.Response.Write(@"window.open('" + url2 + url + "','_blank')");
                        //    System.Web.HttpContext.Current.Response.Write("</script>");
                        //    System.Web.HttpContext.Current.Session["shown"] = "0";
                        //}
                    }
                    //if(!(sCarrierProcessResult.Carrier== null) && !(sCarrierProcessResult.Carrier.Equals(string.Empty)) )
                    //{

                    //    //ShowLabel(sCarrierProcessResult);
                    //}
                }
            }


            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return sShipmentOrderResult;
        }



        public void ShowLabel(SCarrierProcessResult sCarrierProcessResult)
        {

            //  System.Web.HttpContext.Current.Session["one"] = sCarrierProcessResult.Output;
            Page page = (Page)HttpContext.Current.Handler;
            string url = string.Empty;
            if (sCarrierProcessResult.Carrier == "GLS")
            {
                url = page.ResolveUrl(@"~\Carriers\GLS\GLSCarrier.aspx");
                System.Web.HttpContext.Current.Session["Label"] = sCarrierProcessResult.Output;
            }
            else if (sCarrierProcessResult.Carrier == "TNTINTERNATIONAL")
            {
                url = page.ResolveUrl(@"~\Carriers\TNTInternational\Label.aspx");
                System.Web.HttpContext.Current.Session["Label"] = sCarrierProcessResult.Output;
            }
            else if (sCarrierProcessResult.Carrier == "TNTNATIONAL")
            {
                url = page.ResolveUrl(@"~\Carriers\TNTNational\LabelTNTnat.aspx");
                System.Web.HttpContext.Current.Session["PDfLabel"] = sCarrierProcessResult.Result;

            }
            string url1 = page.Request.Url.AbsolutePath.ToString();
            string url2 = page.Request.Url.AbsoluteUri.Substring(0, page.Request.Url.AbsoluteUri.IndexOf(page.Request.Url.LocalPath));
            //System.Web.HttpContext.Current.Response.Redirect(@"Carr\About.aspx");
            //Page page = (Page)HttpContext.Current.Handler;
            //string url = page.ResolveClientUrl(@"Carr/About.aspx");
            System.Web.HttpContext.Current.Response.Write("<script>");
            System.Web.HttpContext.Current.Response.Write(@"window.open('" + url2 + url + "','_blank')");
            System.Web.HttpContext.Current.Response.Write("</script>");

        }

        //to integrate all carriers into a single file 29FEB12KS
        public List<SCarrierOutput> GetCarrierOutput(string ShipDetail)
        {
            List<SCarrierOutput> lstCarrierOutputcollection;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                kaizosServiceProxy = new KaizosServiceContractClient();
                lstCarrierOutputcollection = kaizosServiceProxy.GetCarrierOutput(ShipDetail).ToList<SCarrierOutput>();
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return lstCarrierOutputcollection;

        }

        public List<SCarrierOutput> GetAllCarrierOutput(string EmailID)
        {
            List<SCarrierOutput> lstCarrierOutputcollection;
            KaizosServiceContractClient kaizosServiceProxy = null;
            try
            {
                kaizosServiceProxy = new KaizosServiceContractClient();
                lstCarrierOutputcollection = kaizosServiceProxy.GetAllCarrierOutput(EmailID).ToList<SCarrierOutput>();
            }
            catch (Exception)
            {
                //4.Abort the proxy in case of exception 
                kaizosServiceProxy.Abort();
                throw;
            }
            finally
            {
                //3. always close Proxy (or else client will get timeout error at 11th request.
                ((KaizosServiceContractClient)kaizosServiceProxy).Close();
            }

            return lstCarrierOutputcollection;
        }
        #endregion


  }
}
