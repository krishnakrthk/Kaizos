using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web; //for file upload
using System.Text;
using KaizosServiceLibrary.Model;
using System.IO;

namespace KaizosServiceLibrary
{
  // NOTE: If you change the interface name "IService1" here, you must also update the reference to "IService1" in App.config.
  [ServiceContract]
  public interface IKaizosServiceContract
  {
      [OperationContract]
      void DummyFaualContract(SGeneralFault sGeneralFault);

      #region TOS
      [OperationContract]
      int InsertToS(SToS tos);

      [OperationContract]
      int SpInsertTos(SToS tos);

      [OperationContract]
      SToS GetActiveToS();

      #endregion

      #region AddressBook
      [OperationContract]
      int InsertAddressBook(SAddressBook sAddressBook);

      [OperationContract]
      List<SAddressBook> GetAddress();

      [OperationContract]
      List<SAddressBook> GetAddressBookSearch(string SearchString, string UsedFor);

      [OperationContract]
      int UpdateAddress(SAddressBook bAddressBook);

      [OperationContract]
      int DeleteAddress(string AddressID);
      /***********24APR2012KM**********/
      [OperationContract]
      List<SAddressBook> GetAddressnew(string SearchString, string UsedFor, string zcode, string scountry);

      #endregion

      #region UserManagement
      [OperationContract]
      int ValidateUser(string userName, string Password);

      [OperationContract]
      SUser GetLogin(string Username);

      [OperationContract]
      SFranchise GetFranchise(string Accountno);

      [OperationContract]
      SCustomer GetCustomer(string UserName);

      [OperationContract]
      int UpdateLastLogin(string UserName, DateTime CurrentDateTime);

      [OperationContract]
      List<SFunctionality> GetFunctionality();

      [OperationContract]
      List<SFunctionalProfile> GetFunctionalProfile();

      [OperationContract]
      int InsertFunctionalProfile(List<SFunctionalProfile> lsFunctionalProfile);

      [OperationContract]
      int DeleteFunctionalProfile();

      [OperationContract]
      int ValidateEmail(string EmailID);

      [OperationContract]
      int InsertFranchise(SFranchise bFranchise);

      [OperationContract]
      int UpdateFranchise(SFranchise bFranchise);

      [OperationContract]
      int InsertMonthlyFee(SMonthlyFee sMonthlyFee);

      [OperationContract]
      List<SFranchiseContact> GetFranchiseList();

      [OperationContract]
      List<SMonthlyFee> GetMonthlyFees(string UserId);

      [OperationContract]
      int ConfirmPassword(string AccountNo, string Password);

      [OperationContract]
      string GetLanguageResource(string strCountryCode, string strLanguageCode);

      [OperationContract]
      int CustomerServiceUpdate(string AccountNo, string Pwd, string LanguageCode);

      [OperationContract]
      List<SFranchiseContact> GetCustomerServiceList();

      [OperationContract]
      int CustomerServiceUpdateAdmin(string UserID, string Status);

      [OperationContract]
      List<SIndustry> GetIndustry();

      [OperationContract]
      int InsertCustomer(SCustomer sCustomer);

      [OperationContract]
      int InsertEndCustomer(SCustomer sCustomer);

      [OperationContract]
      string ValidateHQZipcode(string HQZipcode);

      [OperationContract]
      int UpdateEndCustomerByAdmin(SCustomer sCustomer);

      [OperationContract]
      List<SCustomer> GetCustomerCredit(string AccountNo, string UserType);

      [OperationContract]
      int UpdateCustomerCredit(string AccountNo, decimal WishedAmt, decimal InsuredAmt, int PaymentDelayDay, decimal GrossMargin, int PaymentDelayMonth, decimal CompensationRate, decimal AuthorizedCreditAmt, string DeferredPaymentAgreed);

      [OperationContract]
      List<SAuthorized> GetAuthorizedList(string AccountNo);

      [OperationContract]
      int UpdateAuthorize(string AccountNo, string CompanyName, string Email, string PhoneNo, string RefAccountNo);

      [OperationContract]
      int InsertAuthorized(SAuthorized sAuthorize);

      [OperationContract]
      int UpdateAuthorizedSelf(string AccountNo, string Password, string TelNo);

      [OperationContract]
      SAuthorized GetAuthorized(string AccountNo);

      [OperationContract]
      List<SFranchiseContact> GetEndCustomerList();

      [OperationContract]
      string GetUser(string accountNumber);

      [OperationContract]
      int UpdateEndCustomer(SCustomer sCustomer);

      [OperationContract]
      int InsertAdv(List<SAdv> sAdv);

      [OperationContract]
      SCustomer GetEndCustomer(string UserId);

      [OperationContract]
      int UpdateFranchiseAdmin(SFranchise bFranchise);

      [OperationContract]
      int DeleteAuthorized(string AccountNo);

      [OperationContract]
      int InsertCarrierAccountRef(string[] CarrierAccList, string[] CarrierAcc, string AccountNo);

      [OperationContract]
      string GetPostalCode(string strCountryCode);
      #endregion

      #region Common
      [OperationContract]
      List<SComboText> FillCombo(SComboTableField sComboTableField);

      [OperationContract]
      string EncryptString(string Message, string Passphrase);

      [OperationContract]
      string DecryptString(string Message, string Passphrase);
      
      [OperationContract]
      List<String> GetFieldValue(SComboTableField bComboTableField);

      [OperationContract]
      int ValidateZipCode(string PostalCode, string CountryCode);

      [OperationContract]
      string[] GetLanguage(string CountryCode);

      [OperationContract]
      List<String> GetLanguageList();

      [OperationContract]
      List<SCountryTable> FillCountryCombo();

      [OperationContract]
      int ValidatePassword(string password);

      [OperationContract]
      string CreatePassword(int length);

      [OperationContract]
      bool sendMessage(string UserID, string Password, string CurrentUserID);

      [OperationContract]
      List<string> CityZipcodeAutoFill(string SearchString, string Country, int Count);

      [OperationContract]
      int ValidateTime(String strTime);

      [OperationContract]
      List<SFileImportStatus> ImportAddressBook(byte[] addressBookFile, string AccountNo);

      [OperationContract]
      List<string> GetFunction();

      [OperationContract]
      bool isNumericValidation(string val, System.Globalization.NumberStyles NumberStyle);

      [OperationContract]
      bool isAlphaNumericValidation(string val);

      [OperationContract]
      SKeyValue GetValueFromParameter(string KeyCode);

      [OperationContract]
      bool sendConfirmPassord(string UserId, string CurrentUserID);

      [OperationContract]
      bool IsValidZidcode(string Zipcode);

      [OperationContract]
      List<string> GetAllRefCarrierList();

      [OperationContract]
      bool ValidatePostalcode(string strActualFormat, string strZipcode);

      [OperationContract]
      bool sendMail(string To, string CurrentUserID, string strBody, string Subject);
      
      #endregion

      #region Tariff

      [OperationContract]
      int CreateMasterService(SMasterServiceType sMasterServiceType);

      [OperationContract]
      List<SFuelSurcharge> GetFuelCharge(string tariffType, string keyAccountRef);

      [OperationContract]
      List<SFuelSurchargeParameter> GetFuelChargeParameter(int serviceID);

      [OperationContract]
      int UpdateFuelChargeParameter(List<SFuelSurchargeParameter> sFuelSurchargeParameter);

      [OperationContract]
      int UpdateFuelChargeStartDate(int ServiceId, DateTime startDate);

      [OperationContract]
      List<SCarrierService> GetCarrierService();

      ///[OperationContract]
      //int UpdateCarrierService(SCarrierService bCarrierService);

      /****** 17FEB12RM ****/
      [OperationContract]
      int UpdateCarrierService(List<SCarrierService> sCarrierService);

      [OperationContract]
      List<SDeliveryDelay> GetCarrierServiceDeliveryDelay(int serviceID);

      [OperationContract]
      int UpdateCarrierServiceDelay(int ServiceID, string Origin, string Destination, int Delay);

      [OperationContract]
      [WebInvoke(Method = "POST", UriTemplate = "FileUpload/{fileName}")]
      List<SFileImportStatus> ImportDeliveryDelay(int ServiceID, byte[] stream);  //24JAN12RM changed return type from int to List<SFileImportStatus> 


      /**********************  after 13th December ****************************/
      [OperationContract]
      STariffCreationAcknowledgement CreateMasterTariff(STariffMaster bTariffMaster);

      [OperationContract]
      List<String> GetOpenImportTariff(String CarrierName);

      [OperationContract]
      List<SFileImportStatus> ImportTariff(string CarrierName, string TariffReference, List<STariffCalculationRule> sTariffCalculationRule, byte[] tariffFile);

      [OperationContract]
      List<STariffReferenceList> GetTariffReference(String CarrierName, String TariffType);

      [OperationContract]
      int UpdateTariffReference(string TariffReference, DateTime StartDate, DateTime EndDate, bool Archived);

      [OperationContract]
      int CreateZone(SZone zone, string Flag, int ZoneID);

      [OperationContract]
      List<SZoneSearchDetails> GetZoneSearchDetails(string TariffReference);

      [OperationContract]
      SZone GetZoneDetails(int ZoneID);

      [OperationContract]
      List<String> GetTariffZoneName(String TariffReference);

      [OperationContract]
      List<SZone> GetZoneCoverageList(string TariffReference);


      /**********************  after 20th December ****************************/
      [OperationContract]
      List<SSurchargeMaster> GetSurchargeMaster(string TariffReference);

      [OperationContract]
      List<SSurchargeDetails> GetSurchargeDetails(string TariffReference, string SurchargeCode);

      [OperationContract]
      int UpdateSurchargeMater(string SurchargeCode, string MasterServiceName, SEnumFlag Active);

      [OperationContract]
      int UpdateSurchargeDetails(List<SSurchargeDetails> bSurchargeDetails);

      [OperationContract]
      List<SPublicTariffSearchResult> GetPublicTariff(string Name);

      [OperationContract]
      int UpdatePublicTariff(List<SPublicTariffSearchResult> sPublicTariffUpdated);

      [OperationContract]
      bool isAlreadyExist(string Origin, string Destination, string MasterType, string AccountNo);

      [OperationContract]
      string  UpdateMasterTariff(STariffMaster sTariffMaster);
       //06APR2012 HV
      [OperationContract]
      int InsertCarrierService(List<SCarrierService> sCarrierService);
     
      [OperationContract]
      List<SCarrierService> GetCarrierServiceForMaster();

      //25APR12HN
      [OperationContract]
      SUser GetUserDetailForAssignTariff(string AccountNo);

      //14MAY12KS
      [OperationContract]
      string Getvalidatezone(string zonecountry, string tariffreference, string Geographical_coverage, int zone_id);
      #endregion

      #region Shipping Module
      [OperationContract]
      int CreateSingleShipment(SShipmentOrder sShipmentOrder, SShipmentQuotation sShipmentQuotation, string strSessionID);

      [OperationContract]
      List<string> GetPublicTariffNames();

      [OperationContract]
      List<STariffMaster> GetCarrierTariffNames(string TariffType, string Carrier);

      [OperationContract]
      List<STariffMaster> GetTariffAssignedCarrierNames(string TariffType);

      [OperationContract]
      List<SShipmentQuotation> GetQuoteForTool(List<STariffMaster> sTariffMaster, string AssignedTariff, string UODType, string Origin, string Destination, string MasterServiceName, float Weight);

      [OperationContract]
      List<string> GetDiscount(string AccountNo, List<STariffMaster> sTariffMaster, List<float> Weight, string Origin, string Destination, string MasterServiceName);

      //[OperationContract]
      //List<STariffName> GetAffectedTariffName(string strAccountNo);

      [OperationContract]
      List<SShipmentQuotation> GetQuote(SShipmentOrder sShipmentOrder);

      [OperationContract]
      int ConfirmShipment(SShipmentOrder sShipmentOrder);

      [OperationContract]
      List<SShipmentOrder> GetShipmentDetails(int iGroupID, string strSessionID, string strStatus, string strAccountNo);

      [OperationContract]
      SShipmentOrder GetOrderInformation(string ShipmentReference);

      [OperationContract]
      int DeferredPayment(List<string> ClosedReference, string AccountNo, float fTotalAmount);

      [OperationContract]
      int DeleteShipment(string ShipmentReference);

      [OperationContract]
      int CancelShipment(string ShipmentReference);

      [OperationContract]
      int CarrierProcess(List<SShipmentOrder> bShipmentOrder);

      [OperationContract]
      SNextcounter GetNextCounter(string KeyCode, string CounterType, int RequiredCount);

      [OperationContract]
      SPaymentInfo GetPaymentMethodAndInfo(string strAccountNo, string strUserType);

      //19JAN11 HN

      [OperationContract]
      List<STariffMaster> GetAllCarrierNames();

      [OperationContract]
      int SimulationHeaderInsert(SSimulationHeader sSimulationHeader);

      [OperationContract]
      int SimulationSurchargeDiscount(List<SSimulationSurchargeDiscount> sSimulationSurchargeDiscount);

      [OperationContract]
      int SimulationTariff(List<SSimulationTariff> sSimulationTariff);

      [OperationContract]
      int SimulationTariffBased(List<SSimulationTariffBased> sSimulationTariffBased);

      [OperationContract]
      int SimulationSubTotal(SSimulationSubTotal sSimulationSubTotal);

      [OperationContract]
      List<SUserID> GetAssignedUsers(string AccountNo);

      [OperationContract]
      List<SSimulationList> GetSimulationID(string AccountNo);

      [OperationContract]
      List<SSimulationTariff> GetSimulationTariff(string SimulationID);

      [OperationContract]
      SSimulationHeader GetSimulationHeader(string SimulationID);

      [OperationContract]
      List<SSimulationSurchargeDiscount> GetSimulationSurchargeDiscount(string SimulationID);

      [OperationContract]
      List<SSimulationTariffBased> GetSimulationTariffBased(string SimulationID);

      [OperationContract]
      SSimulationSubTotal GetSimulationSubTotal(string SimulationID);


      /************** after 30JAN12HN *******************/

      [OperationContract]
      SSurchargeDetails GetSimulationSurcharge(string TariifReference, string Origin, String Destination, float Weight, string MasterServiceName);

      [OperationContract]
      float GetFuelSurcharge(string TariifReference, string Origin, String Destination, float Weight, string MasterServiceName, float TariffAmount);

      [OperationContract]
      int DeleteSimulationID(string SimulationID);

      /* 16FEB12HN */
      [OperationContract]
      List<SSimulationSubTotal> GetSimulationGrossTotal(string SimulationID, string AccountNo);

      /* 21FEB12KS */
      [OperationContract]
      string getBrokerEmailId();

      [OperationContract]
      int UpdateBrokerEmailId(string Email);

      /*29FEB12HN*/
      [OperationContract]
      int ShipmentDetailUpdate(string ShipmentReference, string TrackingNumber, string ParameterValues);

      /*01MAR12HN*/
      [OperationContract]
      int SimulationAssign(List<SSimulationTariff> sSimulationTariff);

      #endregion

      #region Carrier
      [OperationContract]
      bool CheckForDuplicateCarrierName(string sCarrierName);

      [OperationContract]
      int CreateCarrier(SCarrier sCarrier);

      [OperationContract]
      int UpdateCarrier(SCarrier sCarrier);

      [OperationContract]
      List<SCarrier> GetCarriers();

      [OperationContract]
      SCarrier GetCarrier(string sCarrierName);

      [OperationContract]
      SShipmentOrder CarrierProcessing(SShipmentOrder sShipmentOrder, SEnumCarrierProcess sEnumCarrierProcess, out SCarrierProcessResult sCarrierProcessResult);

      [OperationContract]
      SShipmentOrder GetFeasability(SShipmentOrder sShipmentOrder);

      [OperationContract]
      List<SCarrierOutput> GetCarrierOutput(string ShipDetail);

      [OperationContract]
      List<SCarrierOutput> GetAllCarrierOutput(string ShipDetail);

      #endregion


  }

}
