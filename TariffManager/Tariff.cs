using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.IO; //file upload
using System.Text.RegularExpressions;

using System.Data.SqlClient;
using System.Data;
using System.Data.Objects;

using System.Data.Entity;
using System.Transactions;

using Kaizos.Entities.Business; //to access business entity
using KaizosEntities; //To access data entity

namespace Kaizos.Components.TariffManager
{
  public class TariffHandler
  {
      public string[] head; // [KM 08MAR]
      public static float x = 0.0f; //[KM 21MAR12]
      /// <summary>
      /// Validate the given parameter is number or not
      /// </summary>
      /// <param name="number"></param>
      /// <returns></returns>
      protected bool IsNumber(string number)
      {
          bool result = false;
          string pattern = @"^\d{1,3}$";
          Regex ex = new Regex(pattern);
          if (ex.IsMatch(number)) result = true;
          return result;
      }

      /// <summary>
      /// Validate the given parameter is valid country or not
      /// To be cross check with COUNTRY table.
      /// </summary>
      /// <param name="number"></param>
      /// <returns></returns>
      protected bool IsValidCountryCode(string number)  //24JAN12RM enhanced COUNTRY agains table
      {
          bool result = true;

          try
          {
              string pattern = @"^[a-z,A-Z]{2,3}$";
              Regex ex = new Regex(pattern);

              if (!ex.IsMatch(number)) //Check its 2/3 digit ISO code 
              {
                  result = false;
              }
              else
              {
                  //Check against country table.
                  KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
                  COUNTRY country = context.COUNTRY.First(e => e.COUNTRY_CODE == number);
                  if (country == null) result = false;
              }
          }
          catch (Exception error)
          {
              result = false;
          }

          return result;
      }

      /// <summary>
      /// Master service type details
      /// </summary>
      /// <returns>
      /// 0 = Success
      /// 1 = Fail
      /// 2 = Already exists
      /// </returns>
      public int CreateMasterService(BMasterServiceType masterServiceType)
      {
          int result = 1;

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          string IsBulkShipment = "N";

          if (masterServiceType.isBulkShippingAvailable == BEnumFlag.Yes) IsBulkShipment = "Y";

          result = (int)context.uSP_TARIFF_CREATE_MASTER_SERVICE_TYPE(masterServiceType.ServiceTypeName, masterServiceType.Priority, IsBulkShipment, masterServiceType.Type).SingleOrDefault();

          return result;
      }

      /// <summary>
      /// To retrieve Fuel charge details forthe given tariff type
      /// </summary>
      /// <param name="TariffType"></param>
      /// <returns></returns>
      public List<BFuelSurcharge> GetFuelCharge(string tariffType, string keyAccountRef)
      {

          List<BFuelSurcharge> bFuelSurcharges = new List<BFuelSurcharge>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          //context.ExecuteStoreQuery<BFuelSurcharge> ("uSP_TARIFF_GET_FUEL_SURCHARGE",new SqlParameter("@TARIFF_TYPE",tariffType));  //direct SP call

          var fuelSurcharges = context.uSP_TARIFF_GET_FUEL_SURCHARGE(tariffType, keyAccountRef).ToList();  //SP call through function import in EF

          foreach (var fuelSurcharge in fuelSurcharges)
          {

              BFuelSurcharge bFuel = new BFuelSurcharge();
              bFuel.ServiceID = fuelSurcharge.SERVICE_ID;
              bFuel.TariffType = fuelSurcharge.TARIFF_TYPE;
              bFuel.KeyAccountReference = fuelSurcharge.KEY_ACCOUNT_REF;
              bFuel.MasterServiceName = fuelSurcharge.MASTER_SERVICE_NAME;
              bFuel.StartDate = fuelSurcharge.START_DATE;
              bFuel.LastUpdate = fuelSurcharge.LAST_UPDATE;
              bFuel.ServiceName = fuelSurcharge.SERVICE_NAME;
              bFuel.Reference = fuelSurcharge.REFERENCE;
              bFuelSurcharges.Add(bFuel);
          }

          return bFuelSurcharges;
      }

      /// <summary>
      /// Get Parameter details for the given service (ServiceID)
      /// </summary>
      /// <param name="serviceID"></param>
      /// <returns></returns>
      public List<BFuelSurchargeParameter> GetFuelChargeParameter(int serviceID)
      {

          List<BFuelSurchargeParameter> bFuelSurchargesParams = new List<BFuelSurchargeParameter>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var fuelParameters = context.uSP_TARIFF_GET_FUEL_SURCHARGE_PARAM(serviceID).ToList();  //SP call through function import in EF

          foreach (var parameter in fuelParameters)
          {

              BFuelSurchargeParameter bParameter = new BFuelSurchargeParameter();
              bParameter.ServiceID = parameter.SERVICE_ID;
              bParameter.ParameterDescription = parameter.PARAM_ID;
              bParameter.ParameterValue = parameter.PARAM_VALUE;

              bFuelSurchargesParams.Add(bParameter);
          }
          return bFuelSurchargesParams;
      }

      /// <summary>
      /// Update fuel charge parameter detail for the given service.
      /// </summary>
      /// <param name="bFuelSurchargeParameter"></param>
      /// <returns></returns>
      public int UpdateFuelChargeParameter(List<BFuelSurchargeParameter> bFuelSurchargeParameter)
      {
          int result = 1;

          for (int i = 0; i < bFuelSurchargeParameter.Count; i++)
          {
              KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

              int serviceID = bFuelSurchargeParameter[i].ServiceID;
              string paramDesc = bFuelSurchargeParameter[i].ParameterDescription;
              string paramValue = bFuelSurchargeParameter[i].ParameterValue;

              result = (int)context.uSP_TARIFF_UPDATE_FUEL_SURCHARGE_PARAM(serviceID, paramDesc, paramValue).SingleOrDefault();

          }
          return result;
      }

      /// <summary>
      /// To update Start date of surcharge
      /// </summary>
      /// <param name="ServiceId"></param>
      /// <param name="startDate"></param>
      /// <returns></returns>
      public int UpdateFuelChargeStartDate(int ServiceId, DateTime startDate)
      {
          int result = 1;

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          result = (int)context.uSP_TARIFF_UPDATE_FUEL_SURCHARGE(ServiceId, startDate).SingleOrDefault();

          return result;
      }

      /// <summary>
      /// Retrieve all carrier service information.
      /// </summary>
      /// <returns></returns>
      public List<BCarrierService> GetCarrierService()
      {

          List<BCarrierService> bCarrierServices = new List<BCarrierService>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          //var serviceList = context.uSP_TARIFF_GET_FUEL_SURCHARGE_PARAM(2).ToList();  //SP call through function import in EF
          var serviceList = context.uSP_GET_CARRIER_SERVICE().ToList();  //SP call through function import in EF

          foreach (var service in serviceList)
          {

              BCarrierService bcarrierService = new BCarrierService();

              bcarrierService.ServiceID = service.SERVICE_ID;
              bcarrierService.CarrierName = service.CARRIER_NAME;
              bcarrierService.MasterServiceName = service.MASTER_SERVICE_NAME;
              bcarrierService.Priority = service.PRIORITY == "Eco" ? BEnumPriority.Economy : BEnumPriority.Express;
              bcarrierService.ServiceName = service.SERVICE_NAME;
              bcarrierService.ServiceCode = service.SERVICE_CODE;
              bcarrierService.DeliveryDelayTable = service.DELIVERY_DELAY;
              bcarrierService.DeliveryDeadLine = service.DELIVERY_DEADLINE;
              bcarrierService.Active = service.ACTIVE == "Y" ? BEnumFlag.Yes : BEnumFlag.No;
              bcarrierService.KeyCustomerService = service.KEY_CUSTOMER_SERVICE == "Y" ? BEnumFlag.Yes : BEnumFlag.No;
              bCarrierServices.Add(bcarrierService);
          }
          return bCarrierServices;
      }

      /// <summary>
      /// Retrieve all carrier service information.
      /// </summary>
      /// <returns></returns>
      public List<BCarrierService> GetCarrierServiceForMaster()
      {

          List<BCarrierService> bCarrierServices = new List<BCarrierService>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          //var serviceList = context.uSP_TARIFF_GET_FUEL_SURCHARGE_PARAM(2).ToList();  //SP call through function import in EF
          var serviceList = context.uSP_GET_CARRIER_SERVICE_FOR_MASTER_SERVICE().DefaultIfEmpty().ToList(); ;  //SP call through function import in EF
          if (!(serviceList == null))
          {
              foreach (var service in serviceList)
              {

                  BCarrierService bcarrierService = new BCarrierService();
                  if (!(service == null))
                  {
                      int? serviceiD = service.SERVICE_ID;

                      if (serviceiD == null)
                          bcarrierService.ServiceID = 0; 
                      else
                          bcarrierService.ServiceID = Convert.ToInt32(serviceiD);

                      bcarrierService.CarrierName = service.CARRIER_NAME;
                      bcarrierService.MasterServiceName = service.MASTER_SERVICE_NAME;
                      bcarrierService.Priority = service.PRIORITY == "Eco" ? BEnumPriority.Economy : BEnumPriority.Express;
                      bcarrierService.ServiceName = service.SERVICE_NAME;
                      bcarrierService.ServiceCode = service.SERVICE_CODE;
                      if (service.DELIVERY_DELAY.Equals(string.Empty))
                      {
                          bcarrierService.DeliveryDelayTable = "NoTable";
                      }
                      else
                      {
                          bcarrierService.DeliveryDelayTable = service.DELIVERY_DELAY;
                      }
                      bcarrierService.DeliveryDeadLine = service.DELIVERY_DEADLINE;
                      bcarrierService.Active = service.ACTIVE == "Y" ? BEnumFlag.Yes : BEnumFlag.No;
                      bcarrierService.KeyCustomerService = service.KEY_CUSTOMER_SERVICE == "Y" ? BEnumFlag.Yes : BEnumFlag.No;
                      bcarrierService.Information = service.INFORMATION;
                      bcarrierService.InfoType = service.INFOTYPE;
                      bcarrierService.CarrierServiceCode = service.CARRIER_SERVICE_CODE; //Must be updated
                      bCarrierServices.Add(bcarrierService);
                  }

              }
          }
          return bCarrierServices;
      }

      public int InsertCarrierService(BCarrierService bCarrierService)
      {

          int result = 1;
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          int serviceID = bCarrierService.ServiceID;
          string serviceName = bCarrierService.ServiceName;
          string serviceCode = bCarrierService.ServiceCode;
          string deliveryDeadLine = bCarrierService.DeliveryDeadLine;
          string active = bCarrierService.Active == BEnumFlag.Yes ? "Y" : "N";

          result = (int)context.uSP_CARRIER_SERVICE_INSERT(bCarrierService.CarrierName, bCarrierService.MasterServiceName, "ECO", serviceName, serviceCode, bCarrierService.DeliveryDelayTable, deliveryDeadLine, active, active, bCarrierService.CarrierServiceCode, bCarrierService.Information, bCarrierService.InfoType).SingleOrDefault();
          return result;
      }


      public int UpdateCarrierService(BCarrierService bCarrierService)
      {
          int result = 1;

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          int serviceID = bCarrierService.ServiceID;
          string serviceName = bCarrierService.ServiceName;
          string serviceCode = bCarrierService.ServiceCode;
          string deliveryDeadLine = bCarrierService.DeliveryDeadLine;
          string active = bCarrierService.Active == BEnumFlag.Yes ? "Y" : "N";

          result = (int)context.uSP_UPDATE_CARRIER_SERVICE(serviceID, serviceName, serviceCode, deliveryDeadLine, bCarrierService.Information, bCarrierService.InfoType, bCarrierService.CarrierServiceCode, active).SingleOrDefault();
          return result;
      }


      /// <summary>
      /// List of carrier service update 17FEB12RM
      /// </summary>
      /// <param name="bCarrierService"></param>
      /// <returns></returns>
      public int UpdateCarrierService(List<BCarrierService> bCarrierService)
      {
          int result = 1;

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          for (int i = 0; i < bCarrierService.Count; i++)
          {
              int serviceID = bCarrierService[i].ServiceID;
              string serviceName = bCarrierService[i].ServiceName;
              string serviceCode = bCarrierService[i].ServiceCode;
              string deliveryDeadLine = bCarrierService[i].DeliveryDeadLine;
              string active = bCarrierService[i].Active == BEnumFlag.Yes ? "Y" : "N";
              result = (int)context.uSP_UPDATE_CARRIER_SERVICE(serviceID, serviceName, serviceCode, deliveryDeadLine, bCarrierService[i].Information, bCarrierService[i].InfoType, bCarrierService[i].CarrierServiceCode, active).SingleOrDefault();
          }
          return result;
      }

      public List<BDeliveryDelay> GetCarrierServiceDeliveryDelay(int serviceID)
      {
          List<BDeliveryDelay> bDeliveryDelays = new List<BDeliveryDelay>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var serviceDelay = context.uSP_GET_CARRIER_SERVICE_DELIVERY_DELAY(serviceID).ToList();  //SP call through function import in EF

          foreach (var delay in serviceDelay)
          {
              BDeliveryDelay bDeliveryDelay = new BDeliveryDelay();
              bDeliveryDelay.Origin = delay.ORIGIN.Trim();
              bDeliveryDelay.Destination = delay.DESTINATION.Trim();
              bDeliveryDelay.Delay = delay.DELAY;
              bDeliveryDelays.Add(bDeliveryDelay);
          }

          return bDeliveryDelays;
      }

      public int UpdateCarrierServiceDelay(int ServiceID, string Origin, string Destination, int Delay)
      {
          int result = 1;

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          result = (Int32)context.uSP_UPDATE_CARRIER_DELIVERY_DELAY(ServiceID, Origin, Destination, Delay).SingleOrDefault();

          return result;
      }


      /* [24JAN12RM]  */
      protected bool ValidateDeliveryDelayLine(List<BFileImportStatus> bFileImportStatus, string From, string To, string DeliveryDelay, int LineNo)
      {
          bool result = true;

          //1. Validate "From" Country 
          bool bFromISO = IsValidCountryCode(From);

          //2. Validate "To" Country 
          bool bToISO = IsValidCountryCode(To);

          //3. Validate "Delivery Delay" 
          bool bDeliveryDelay = isNumericValidation(DeliveryDelay, System.Globalization.NumberStyles.Integer);


          if (!bFromISO)
          {
              bFileImportStatus.Add(GetFileImportStatus(LineNo, "From", "Country code [" + From + "] is not valid"));
          }

          if (!bToISO)
          {
              bFileImportStatus.Add(GetFileImportStatus(LineNo, "To", "Country code [" + To + "] is not valid"));
          }

          if (!bDeliveryDelay)
          {
              bFileImportStatus.Add(GetFileImportStatus(LineNo, "Delivery Delay", "Delivery Delay [" + DeliveryDelay + "] is not valid number"));
          }


          if (!bFromISO || !bToISO || !bDeliveryDelay)
          {
              result = false;
          }

          return result;
      }


      /* 24JAN12RM */
      public List<BFileImportStatus> ImportDeliveryDelay(int ServiceID, byte[] stream)
      {
          List<BFileImportStatus> bFileImportStatus = new List<BFileImportStatus>();

          //int result = 1;

          try
          {
              // Create the TransactionScope to execute the commands, guaranteeing
              // that both commands can commit or roll back as a single unit of work.
              using (TransactionScope scope = new TransactionScope())
              {

                  using (KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities())
                  {

                      DELIVER_DELAY_TABLE_NAME dELIVER_DELAY_TABLE_NAME = context.uSP_GET_DELIVERY_DELAY_TABLE_NAME(ServiceID).SingleOrDefault();

                      string deliveryTableName = dELIVER_DELAY_TABLE_NAME.DELIVERY_DELAY.Trim();

                      int deleteResult = (int)context.uSP_DELETE_RECORDS(deliveryTableName).SingleOrDefault();

                      if (deleteResult == 1 || deleteResult == 2)  //failure to delete or no table exists
                      {
                          bFileImportStatus.Add(GetFileImportStatus(0, "", "Failed to import delivery delay. Please contact administrator"));
                          return bFileImportStatus;
                      }

                      /* IMPORT STATUS: Prepare Return Import status list with header contains details */
                      bFileImportStatus.Add(GetFileImportStatus(0, "NONE", "All lines are imported successfully"));


                      Stream s = new MemoryStream(stream);

                      using (StreamReader sr = new StreamReader(s))
                      {
                          string line;

                          string[] myStrs = null;

                          int lineCount = 0;

                          while ((line = sr.ReadLine()) != null)
                          {

                              lineCount++;

                              myStrs = line.Split(';');


                              /* IMPORT STATUS: If required no of fields not available just skip its integration into table */
                              if (myStrs.Length != 3)
                              {
                                  bFileImportStatus.Add(GetFileImportStatus(lineCount, "ALL FIELDS", "Not having all required fields in the line"));
                                  continue;
                              }

                              /* Validate Tariff line and inturn log the error if any in IMPORT STATUS */
                              bool bValidateLine = ValidateDeliveryDelayLine(bFileImportStatus, myStrs[0], myStrs[1], myStrs[2], lineCount);

                              /*  If Line validation goes fine then integrate it or else skip it */
                              if (!bValidateLine)
                                  continue;
                              else
                              {
                                  //BDeliveryDelay bDeliveryDelay = new BDeliveryDelay();
                                  //if (IsValidCountryCode(myStrs[0].Trim()) && IsValidCountryCode(myStrs[1].Trim()) && IsNumber(myStrs[2].Trim()))
                                  //{
                                  context.uSP_DELIVERY_DELAY_IMPORT(deliveryTableName, myStrs[0].Trim(), myStrs[1].Trim(), Convert.ToInt32(myStrs[2].Trim())).SingleOrDefault();
                                  //}
                              }
                              Array.Clear(myStrs, 0, myStrs.Length);
                          }
                      }
                  }

                  // The Complete method commits the transaction. If an exception has been thrown,
                  // Complete is not  called and the transaction is rolled back.
                  scope.Complete();
              }

          }
          catch (TransactionAbortedException ex)
          {
              throw ex;
              //TransactionAbortedException Message//
          }
          catch (ApplicationException ex)
          {
              throw ex;
              //"ApplicationException Message//
          }
          return bFileImportStatus;
      }


      /**********************  after 13th December ****************************/

      /// <summary>
      /// To create master tariff
      /// </summary>
      /// <param name="bTariffMaster"></param>
      /// <returns>
      /// 0 : Sucess; created for first time
      /// 1 : Failure; failed to create tariff
      ///   : Sucess; droped existing tariff table for the given
      /// </returns>
      public BTariffCreationAcknowledgement CreateMasterTariff(BTariffMaster bTariffMaster)
      {

        BTariffCreationAcknowledgement bTariffCreationAck = new BTariffCreationAcknowledgement(); ;

        string CarrierName          = bTariffMaster.CarrierName.Trim();
        string TariffReference	    = bTariffMaster.TariffReference;
        string TariffType           = bTariffMaster.TariffType.ToString();
        string ContainerType 	    = bTariffMaster.ContainerType;
        string KeyUserReference     = bTariffMaster.KeyUserReference; 	
        DateTime StartDate 		    = bTariffMaster.StartDate;
        DateTime EndDate            = bTariffMaster.EndDate;
          
          
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        ObjectParameter objExistingTariffRef = new ObjectParameter("INTERNAL_TARIFF_REF", typeof(String));

        int result = (int)context.uSP_TARIFF_CREATION(CarrierName, TariffReference, TariffType, ContainerType, KeyUserReference, StartDate, EndDate, objExistingTariffRef).SingleOrDefault();

        string strExistingTariffRef = objExistingTariffRef.Value.ToString();

        switch (result)
        {
            
            case 0:
                    bTariffCreationAck.CreationStatus= 0;
                    bTariffCreationAck.TariffReference = TariffReference; //given reference
                    bTariffCreationAck.Message = String.Format("Given Tariff reference {0} created successfully", TariffReference);
                    break;

            case 1:
                    bTariffCreationAck.CreationStatus = 1;
                    bTariffCreationAck.TariffReference  = strExistingTariffRef; // failed to create
                    bTariffCreationAck.Message = String.Format("Given Tariff reference {0} failed to create contact admin", TariffReference);
                    break;

            case 2:
                    bTariffCreationAck.CreationStatus = 2;
                    bTariffCreationAck.TariffReference  = strExistingTariffRef; //already existing reference with date overlap
                    bTariffCreationAck.Message = String.Format("Given Tariff reference {0} replaced existing Tariff ref {3} as the given date range {1:MM/dd/yyyy} and {2:MM/dd/yyyy} overlap.  System cleared existing Tariff and you can import it again.", TariffReference, StartDate, EndDate, strExistingTariffRef);
                    break;
            case 3:
                    bTariffCreationAck.CreationStatus = 3;
                    bTariffCreationAck.TariffReference = strExistingTariffRef; //already similar name tariff reference name assigned to existing tariff
                    bTariffCreationAck.Message = String.Format("Given Tariff reference {0} already assigned.  Please try new another one", TariffReference);
                    break;


        }
        

        return bTariffCreationAck;

      }

      public List<String> GetOpenImportTariff(String CarrierName)
      {

          List<String> openTariffRef = new List<string>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var tariffReferece = context.uSP_GET_OPEN_IMPORT_TARIFF(CarrierName);

          foreach (var reference in tariffReferece)
          {
              openTariffRef.Add(reference.ToString());
          }

          return openTariffRef;
      }

      public List<String> GetCarrierServiceCode(String CarrierName)
      {

          List<String> serviceCode = new List<string>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var servicelist = context.uSP_GET_CARRIER_SERVICE_CODE(CarrierName);

          foreach (var reference in servicelist)
          {
              serviceCode.Add(reference.ToString());
          }

          return serviceCode;
      }

      public List<String> GetTariffZoneName(String TariffReference)
      {

          List<String> tariffZone = new List<string>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var zoneList = context.uSP_GET_TARIFF_ZONE_NAME(TariffReference);

          foreach (var zone in zoneList)
          {
              tariffZone.Add(zone.ToString());
          }

          return tariffZone;
      }

      protected BFileImportStatus GetFileImportStatus(int Row, string FieldName, string ErrorMsg)
      {
          BFileImportStatus bFileImportStatus = new BFileImportStatus();
          bFileImportStatus.RowNumber           = Row;
          bFileImportStatus.FieldName           = FieldName.Trim();
          bFileImportStatus.ErrorDescription    = ErrorMsg.Trim();
          return bFileImportStatus;

      }

      protected bool isNumericValidation(string val, System.Globalization.NumberStyles NumberStyle)
      {
          Double result;
          return Double.TryParse(val, NumberStyle,
              System.Globalization.CultureInfo.CurrentCulture, out result);
      }
  /*[KM 01MAR] */
      protected bool ValidateTariffLine(List<BFileImportStatus> bFileImportStatus, List<String> ServiceCode, List<String> ZoneName, string Weight_From, string Freight, string CarrierServiceCode, int LineNo)
      {
          bool result = true;

          //1. Validate Service Code
          bool bServiceCode = ServiceCode.Any(s => s.Equals(CarrierServiceCode, StringComparison.OrdinalIgnoreCase));
          //2. Validte Weight From
          bool bWtFrom = isNumericValidation(Weight_From.Replace(",", "."), System.Globalization.NumberStyles.Float);
          //3. Validate Weight To
          //bool bWtTo        =   isNumericValidation(Weight_To, System.Globalization.NumberStyles.Float);
          //4. validate zone
          //bool bZoneName = ZoneName.Any(s => s.Equals(Zone, StringComparison.OrdinalIgnoreCase));
          //5. validate frieght
          bool bFreight = isNumericValidation(Freight.Replace(",", "."), System.Globalization.NumberStyles.Float);


          if (!bServiceCode)
          {
              bFileImportStatus.Add(GetFileImportStatus(LineNo, "Service Code", "Service code [" + CarrierServiceCode + "] doesn''t exist"));
          }

          if (!bWtFrom)
          {
              bFileImportStatus.Add(GetFileImportStatus(LineNo, "Weight From", "Weight [" + Weight_From + "] is not valid"));
          }

          //if (!bWtTo)
          {
              //bFileImportStatus.Add(GetFileImportStatus(LineNo, "Weight To", "Weight [" + Weight_To + "] is not valid" ));
          }

         // if (!bZoneName)
          {
              //bFileImportStatus.Add(GetFileImportStatus(LineNo, "Zone", "Zone [" + Zone + "] doesn''t exist"));
          }

          if (!bFreight)
          {
              bFileImportStatus.Add(GetFileImportStatus(LineNo, "Freight", "Freight value [" + Freight + "] is not valid"));
          }

          if (!bServiceCode || !bWtFrom  || !bFreight)
          {
              result = false;
          }

          return result;
      }

      protected float GetCalculatedAmount(List<BTariffCalculationRule> bTariffCalculationRule,string CarrierServiceCode,float FreightAmount)
      {
          float calculatedAmout=0;

          float grossMargin = 0;

          for (int i = 0; i < bTariffCalculationRule.Count; i++)
          {
              BTariffCalculationRule tariffRule = bTariffCalculationRule[i];

              if (tariffRule.ServiceTypeCode.Trim() == CarrierServiceCode)
              {
                  if (isNumericValidation(tariffRule.GrossMargin, System.Globalization.NumberStyles.Float))
                  {
                      grossMargin = float.Parse(tariffRule.GrossMargin);
                  }
                  break;
              }
          }
          if (grossMargin != 0)
          {
              calculatedAmout = FreightAmount * (1 - (grossMargin / 100));
          }

          return calculatedAmout;
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
      /// 
      /*[KM 08MAR]*/
      public List<BFileImportStatus> ImportTariff(string CarrierName, string TariffReference, List<BTariffCalculationRule> bTariffCalculationRule, byte[] tariffFile)
      {
          List<BFileImportStatus> bFileImportStatus = new List<BFileImportStatus>();

          try
          {
              // Create the TransactionScope to execute the commands, guaranteeing
              // that both commands can commit or roll back as a single unit of work.
              using (TransactionScope scope = new TransactionScope())
              {
                  using (KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities())
                  {
                      /* 1. Get all carrier service code for the given carrier */
                      List<String> ServiceCodeList = GetCarrierServiceCode(CarrierName);

                      /* 2. Get all carrier service code for the given carrier */
                      List<String> ZoneNameList = GetTariffZoneName(TariffReference);


                      /* 1. Attempt to create a TARIFF table */
                      ObjectParameter objTariffTableName = new ObjectParameter("TARIFF_TABLE_NAME", typeof(String));
                      ObjectParameter objTariffType = new ObjectParameter("TARIFF_TYPE_OUT", typeof(String));

                      int result = (int)context.uSP_CREATE_TARIFF_TABLE(CarrierName, TariffReference, objTariffTableName, objTariffType).SingleOrDefault();

                      /* 2. IMPORT STATUS: If above failed to create so exit the method*/
                      if (result == 1)
                      {
                          bFileImportStatus.Add(GetFileImportStatus(1, "", "Failed to import tariff for [" + CarrierName + "] . Please contact administrator"));
                          return bFileImportStatus;
                      }

                      /* 3. Retrieve created TARIFF table name and tariff type through output variable */
                      string strTariffTableName = objTariffTableName.Value.ToString();
                      string strTariffType = objTariffType.Value.ToString();


                      /* 4. IMPORT STATUS: Prepare Return Import status list with header contains details */
                      bFileImportStatus.Add(GetFileImportStatus(0, "NONE", "All lines are imported successfully"));

                      /* 5. Loop through the file and insert record by record */
                      Stream s = new MemoryStream(tariffFile);

                      using (StreamReader sr = new StreamReader(s))
                      {

                          string line;

                          string[] myStrs = null;
                          string zonevs;
                          int lineCount = 0;
                          int lineCount1 = 1;
                          float FreightAmount;
                          //int count = sr.ReadLine().Count();

                          bool strImportFlag = false; //To fix bug 1141 [07FEB12RM]

                          while ((line = sr.ReadLine()) != null)
                          {
                              lineCount++;
                              myStrs = line.Split(';');


                              if (lineCount1 == 1) //(line.StartsWith("Service"))
                              {
                                  //myStrs = line.Split(';');
                                  head = myStrs;
                                  lineCount1++;
                                  // Could not take first line value in File.
                              }

                              else
                              {


                                  /* 5.1. IMPORT STATUS: If required no of fields not available just skip its integration into table */
                                  if (myStrs.Length < 3)
                                  {
                                      bFileImportStatus.Add(GetFileImportStatus(lineCount, "ALL FIELDS", "Not having all required fields in the line"));
                                      continue;
                                  }



                                  /* 5.2. Validate Tariff line and inturn log the error if any in IMPORT STATUS */
                                  /* Zone Name has been hard coded named 'Zon1' because always the zone caption will be same */

                                  bool bValidateLine = ValidateTariffLine(bFileImportStatus, ServiceCodeList, ZoneNameList, myStrs[1], myStrs[2], myStrs[0], lineCount);


                                  /* 5.3. If Line validation goes fine then integrate it or else skip it */
                                  if (!bValidateLine)
                                      continue;
                                  else //otherwise, insert into newly created TARIFF table
                                  {
                                      float z, y = 0.0f, Wt_From; //[KM 21MAR12]

                                      //else
                                      {

                                          for (int i = 2; i <= myStrs.Length - 1; i++)
                                          {
                                              float Wt_To = float.Parse(myStrs[1].Replace(",", "."));
                                              int temp = Convert.ToInt32(Wt_To);
                                              z = Wt_To;
                                              if (z >= x && z != x)
                                              {
                                                  y = Convert.ToSingle(x + 0.01);
                                              }
                                              else
                                              {
                                                  y = Convert.ToSingle(y);
                                                  if (y == 0.0f)
                                                      y = 0.01f;
                                              }
                                              Wt_From = y;
                                              x = z;

                                              if (head[i].Equals(string.Empty))
                                              {
                                                  zonevs = "Zone Unknown";
                                              }

                                              else
                                              {
                                                  zonevs = head[i];//"zone" + (i - 1).ToString();
                                              }

                                              string Zone = zonevs.ToString();//myStrs[3].Trim();
                                              string CarrierServiceCode = myStrs[0].Trim();
                                              if (myStrs[i].Equals(string.Empty))
                                              {
                                                  FreightAmount = 0.0f;
                                              }
                                              else
                                              {
                                                  FreightAmount = float.Parse(myStrs[i].Replace(",", "."));
                                              }

                                              float CalculatedAmount = 0;

                                              /* Just before insert the line of tariff get calculated amount only for SYSTEM PURCHASE tariff */
                                              if (strTariffType.Trim() == "Purchase")
                                              {
                                                  CalculatedAmount = GetCalculatedAmount(bTariffCalculationRule, CarrierServiceCode, FreightAmount);
                                              }
                                              else
                                              {
                                                  CalculatedAmount = FreightAmount; //31JAN12RM 
                                              }

                                              /* As all lines of import status returned by BFileImportStatus object not required to handdle individiual here */
                                              int ret = (int)context.uSP_TARIFF_IMPORT(strTariffTableName, Wt_From, Wt_To, Zone, FreightAmount, CalculatedAmount, CarrierServiceCode).SingleOrDefault();

                                              /* Have a flag indicates atleast one line is imported */
                                              strImportFlag = true; ////To fix bug 1141 [07FEB12RM]

                                          }
                                      }
                                  }

                                  Array.Clear(myStrs, 0, myStrs.Length);
                              }

                          }

                          /* If none of the line is imported then ignore total importation Bug Ref 1141 [07FEB12RM] */
                          if ((!strImportFlag) && (strTariffTableName.Trim().Length != 0))
                          {
                              int result1 = (int)context.uSP_DELETE_TARIFF_TABLE(strTariffTableName.Trim(), TariffReference).SingleOrDefault();
                          }

                      }
                  }

                  // The Complete method commits the transaction. If an exception has been thrown,
                  // Complete is not  called and the transaction is rolled back.
                  scope.Complete();
              }

          }
          catch (TransactionAbortedException ex)
          {

              //TransactionAbortedException Message//
          }
          catch (ApplicationException ex)
          {
              //"ApplicationException Message//
          }

          return bFileImportStatus;
      }

      public List<BTariffReferenceList> GetTariffReference(String CarrierName,String TariffType)
      {

          List<BTariffReferenceList> tariffReference = new List<BTariffReferenceList>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var tariffList = context.uSP_GET_TARIFF_REFERENCE(CarrierName, TariffType).ToList();

          foreach (var tariff in tariffList)
          {
              BTariffReferenceList bTariffReference = new BTariffReferenceList();
              bTariffReference.TariffReference  =   tariff.TARIFF_REFERENCE;
              bTariffReference.StartDate        =   (DateTime)tariff.START_DATE;
              bTariffReference.EndDate          =   (DateTime)tariff.END_DATE;
              tariffReference.Add(bTariffReference);
          }

          return tariffReference;
      }

      /// <summary>
      /// To update status of a given Tariff
      /// </summary>
      /// <param name="TariffReference"></param>
      /// <param name="StartDate"></param>
      /// <param name="EndDate"></param>
      /// <param name="Archived"></param>
      /// <returns></returns>
      public int UpdateTariffReference(string TariffReference, DateTime StartDate,DateTime EndDate,bool Archived)
      {
          int result = 1;

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          result = (Int32)context.uSP_UPDATE_TARIFF_REFERENCE(TariffReference,StartDate,EndDate,Archived).SingleOrDefault();

          return result;
      }

      public int CreateZone(BZone zone,string FlagName,int ZoneID)
      {
          int result = 1;
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          result = (int)context.uSP_ZONE_CREATION(zone.TariffReference, zone.ZoneName, zone.Direction, zone.MasterServiceName, zone.GeographicalCoverage, zone.CountryCode, zone.CoverageList, ZoneID, FlagName).SingleOrDefault();
          return result;
      }

      public List<BZoneSearchDetails> GetZoneSearchDetails(string TariffReference)
      {
          List<BZoneSearchDetails> bZoneSearchDetails = new List<BZoneSearchDetails>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var zoneDetails = context.uSP_GET_ZONE_SEARCH_DETAILS(TariffReference);

          foreach (var zone in zoneDetails)
          {
              BZoneSearchDetails bZone = new BZoneSearchDetails();
              bZone.ZoneID          = zone.ZONE_ID;
              bZone.CarrierName     = zone.CARRIER_NAME;
              bZone.ZoneName        = zone.ZONE_NAME;
              bZone.Direction       = zone.DIRECTION;
              bZone.StartDate       = (DateTime)zone.START_DATE;
              bZone.EndDate         = (DateTime)zone.END_DATE;
              bZoneSearchDetails.Add(bZone);
          }

          return bZoneSearchDetails;
      }

      public BZone GetZoneDetails(int ZoneID)
      {
          BZone bZoneDetails = new BZone();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var zoneDetails = context.uSP_GET_ZONE_DETAILS(ZoneID).SingleOrDefault();

          if (zoneDetails != null)
          {
              bZoneDetails.TariffReference = zoneDetails.TARIFF_REFERENCE;
              bZoneDetails.ZoneName = zoneDetails.ZONE_NAME;
              bZoneDetails.MasterServiceName = zoneDetails.MASTER_SERVICE_NAME;
              bZoneDetails.GeographicalCoverage = zoneDetails.GEOGRAPHICAL_COVERAGE;
              bZoneDetails.CountryCode = zoneDetails.COUNTRY_CODE;
              bZoneDetails.CoverageList = zoneDetails.COVERAGE_LIST;
              bZoneDetails.Direction = zoneDetails.DIRECTION;
          }
          
          return bZoneDetails;
      }

      public List<BZone> GetZoneCoverageList(string TariffReference)
      {
          List<BZone> bZoneDetails = new List<BZone>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var zoneDetails = context.uSP_GET_ZONE_COVERAGELIST(TariffReference).ToList();

          foreach (var zone in zoneDetails)
          {
              BZone bZone = new BZone();
              bZone.ZoneID                  = zone.ZONE_ID;
              bZone.TariffReference         = zone.TARIFF_REFERENCE;
              bZone.ZoneName                = zone.ZONE_NAME;
              bZone.Direction               = zone.DIRECTION;
              bZone.MasterServiceName       = zone.MASTER_SERVICE_NAME;
              bZone.GeographicalCoverage    = zone.GEOGRAPHICAL_COVERAGE;
              bZone.CountryCode             = zone.COUNTRY_CODE;
              bZone.CoverageList            = zone.COVERAGE_LIST;
              bZoneDetails.Add(bZone);
          }

          return bZoneDetails;
      }


      /**********************  after 20th December ****************************/

      public List<String> GetMasterServiceNameList(string DelimitedServiceName)
      {
          return DelimitedServiceName.Split(',').ToList();

      }

      public List<BSurchargeMaster> GetSurchargeMaster(string TariffReference)
      {
          List<BSurchargeMaster> bSurchargeMasterList = new List<BSurchargeMaster>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var surchargeList = context.uSP_GET_SURCHARGE_MAST(TariffReference).ToList();


          foreach (var surcharge in surchargeList)
          {
              BSurchargeMaster bSurchargeMaster = new BSurchargeMaster();
              bSurchargeMaster.SurchargeCode = surcharge.SURCHARGE_CODE;
              bSurchargeMaster.SurchargeDescription = surcharge.DESCRIPTION;
              bSurchargeMaster.SurchargeType = surcharge.TYPE == "S" ? BEnumSurchargeType.S : BEnumSurchargeType.O;
              bSurchargeMaster.MasterServiceName = surcharge.MASTER_SERVICE_NAME;
              bSurchargeMaster.Active = surcharge.ACTIVE;
              bSurchargeMasterList.Add(bSurchargeMaster);
          }

          return bSurchargeMasterList;
      }

      public List<BSurchargeDetails> GetSurchargeDetails(string TariffReference, string SurchargeCode)
      {
          List<BSurchargeDetails> bSurchargeDetailList = new List<BSurchargeDetails>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var surchargeDetailList = context.uSP_GET_SURCHARGE_DET(TariffReference, SurchargeCode).ToList();

          foreach (var detail in surchargeDetailList)
          {
              BSurchargeDetails bSurchargeDetails = new BSurchargeDetails();
              bSurchargeDetails.ServiceID = detail.SERVICE_ID;
              bSurchargeDetails.SurchageCode = detail.SURCHARGE_CODE;
              bSurchargeDetails.TariffReference = detail.TARIFF_REFERENCE;
              bSurchargeDetails.ServiceName = detail.SERVICE_NAME;
              bSurchargeDetails.ParamID = detail.PARAM_ID;
              bSurchargeDetails.ParamValue = detail.PARAM_VALUE;
              bSurchargeDetailList.Add(bSurchargeDetails);
          }

          return bSurchargeDetailList;
      }

      public int UpdateSurchargeMater(string SurchargeCode, string MasterServiceName, BEnumFlag Active)
      {
          int result = 1;

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          string strActive = Active == BEnumFlag.Yes ? "Y" : "N";

          result = (int)context.uSP_UPDATE_SURCHARGE_MASTER(SurchargeCode, MasterServiceName, strActive).SingleOrDefault();

          return result;
      }

      /// <summary>
      /// /// Update surcharge detail for the given service.
      /// </summary>
      /// <param name="bSurchargeDetails"></param>
      /// <returns></returns>
      public int UpdateSurchargeDetails(List<BSurchargeDetails> bSurchargeDetails)
      {
          int result = 1;

          for (int i = 0; i < bSurchargeDetails.Count; i++)
          {
              KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

              int serviceID = bSurchargeDetails[i].ServiceID;
              string SurchargeCode = bSurchargeDetails[i].SurchageCode;
              string TariffReference = bSurchargeDetails[i].TariffReference;
              string ParamID = bSurchargeDetails[i].ParamID;
              decimal ParamValue = bSurchargeDetails[i].ParamValue;

              result = (int)context.uSP_UPDATE_SURCHARGE_DETAILS(serviceID, TariffReference, SurchargeCode, ParamID, ParamValue).SingleOrDefault();
          }
          return result;
      }

      public BEnumPublicTariffType GetPublicTariffType(string type)
      {
          BEnumPublicTariffType bEnumPublicTariffType = new BEnumPublicTariffType();

          if (type.Trim().ToUpper().Equals("DOMESTIC"))
              bEnumPublicTariffType = BEnumPublicTariffType.Domestic;
          else if (type.Trim().ToUpper().Equals("EXPORT"))
              bEnumPublicTariffType = BEnumPublicTariffType.Export;
          else if (type.Trim().ToUpper().Equals("IMPORT"))
              bEnumPublicTariffType = BEnumPublicTariffType.Import;

          return bEnumPublicTariffType;
      }

      public string GetTariffTypeString(BEnumPublicTariffType bEnumPublicTariffType)
      {

          string result = "";
          if (bEnumPublicTariffType == BEnumPublicTariffType.Domestic)
              result = "Domestic";
          else if (bEnumPublicTariffType == BEnumPublicTariffType.Import)
              result = "Import";
          else if (bEnumPublicTariffType == BEnumPublicTariffType.Export)
              result = "Export";

          return result;

      }

      // Redesigned table and public tariff screen as per YL request during DEMO [15FEB12RM]
      public List<BPublicTariffSearchResult> GetPublicTariff(string Name)
      {
          List<BPublicTariffSearchResult> bPublicTariffSearchResultList = new List<BPublicTariffSearchResult>();

          #region OLD


          //KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          //var publicTariff = context.uSP_GET_PUBLIC_TARIFF(Name).ToList();

          //foreach (var tariff in publicTariff)
          //{
          //    BPublicTariffSearchResult bTariffResult = new BPublicTariffSearchResult();

          //    bTariffResult.Origin = tariff.ORIGIN;
          //    bTariffResult.ShipType = GetPublicTariffType(tariff.SHIP_TYPE);
          //    bTariffResult.MasterServiceName = tariff.MASTER_SERVICE_NAME;
          //    bTariffResult.Destination = tariff.DESTINATION;
          //    bTariffResult.Name = tariff.NAME;

          //    if (tariff.WEIGHT == "WEIGHT_UPTO_30")
          //        bTariffResult.WeightRange = "0-30";
          //    else
          //        bTariffResult.WeightRange = "31-N";

          //    bTariffResult.Domestic = -1;
          //    bTariffResult.Europe = -1;
          //    bTariffResult.International = -1;


          //    /* 1. Domestic to Dom, Eur, Int */
          //    if (tariff.ORIGIN.ToUpper().Equals("DOM") && tariff.DESTINATION.ToUpper().Equals("DOM"))
          //    {
          //        bTariffResult.Domestic = (float)tariff.WEIGHT_VALUE;
          //    }
          //    else if (tariff.ORIGIN.ToUpper().Equals("DOM") && tariff.DESTINATION.ToUpper().Equals("EUR"))
          //    {
          //        bTariffResult.Europe = (float)tariff.WEIGHT_VALUE;
          //    }
          //    else if (tariff.ORIGIN.ToUpper().Equals("DOM") && tariff.DESTINATION.ToUpper().Equals("INT"))
          //    {
          //        bTariffResult.International = (float)tariff.WEIGHT_VALUE;
          //    }


          //    ///* 2. Eur to Eur, Do, Int */
          //    //if (tariff.ORIGIN.ToUpper().Equals("EUR") && tariff.DESTINATION.ToUpper().Equals("DOM"))
          //    //{
          //    //    bTariffResult.Domestic = (float)tariff.WEIGHT_VALUE;

          //    //    /*if (bTariffResult.ShipType==BEnumPublicTariffType.Domestic)
          //    //      bTariffResult.Domestic = (float)tariff.WEIGHT_VALUE;
          //    //    else if (bTariffResult.ShipType==BEnumPublicTariffType.Import)
          //    //      bTariffResult.Europe= (float)tariff.WEIGHT_VALUE;
          //    //    else if (bTariffResult.ShipType==BEnumPublicTariffType.Export)
          //    //      bTariffResult.Europe = (float)tariff.WEIGHT_VALUE;
          //    //     */
          //    //}
          //    //else if (tariff.ORIGIN.ToUpper().Equals("EUR") && tariff.DESTINATION.ToUpper().Equals("EUR"))
          //    //{
          //    //    bTariffResult.Europe = (float)tariff.WEIGHT_VALUE;
          //    //}
          //    //else if (tariff.ORIGIN.ToUpper().Equals("EUR") && tariff.DESTINATION.ToUpper().Equals("INT"))
          //    //{
          //    //    bTariffResult.International = (float)tariff.WEIGHT_VALUE;
          //    //}

          //    /* 3. Int to Int, Dom, Eur */
          //    if (tariff.ORIGIN.ToUpper().Equals("INT") && tariff.DESTINATION.ToUpper().Equals("DOM"))
          //    {
          //        bTariffResult.Domestic = (float)tariff.WEIGHT_VALUE;
          //    }
          //    else if (tariff.ORIGIN.ToUpper().Equals("INT") && tariff.DESTINATION.ToUpper().Equals("EUR"))
          //    {
          //        bTariffResult.Europe = (float)tariff.WEIGHT_VALUE;
          //    }
          //    else if (tariff.ORIGIN.ToUpper().Equals("INT") && tariff.DESTINATION.ToUpper().Equals("INT"))
          //    {
          //        bTariffResult.International = (float)tariff.WEIGHT_VALUE;
          //    }

          //    bPublicTariffSearchResultList.Add(bTariffResult);
          //}
          #endregion

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var publicTariff = context.uSP_GET_PUBLIC_TARIFF(Name).ToList();

          foreach (var tariff in publicTariff)
          {
              BPublicTariffSearchResult bTariffResult = new BPublicTariffSearchResult();
              bTariffResult.Caption = tariff.CAPTION;

              bTariffResult.Name = tariff.NAME;
              bTariffResult.ShipType = GetPublicTariffType(tariff.SHIP_TYPE);
              bTariffResult.MasterServiceName = tariff.MASTER_SERVICE_NAME;
              bTariffResult.MinWt = tariff.MIN_WT;
              bTariffResult.MaxWt = tariff.MAX_WT;
              bTariffResult.WtRangeCaption = tariff.WEIGHT_RANGE_CAPTION;
              bTariffResult.Dom = (float)tariff.DOM;
              bTariffResult.Eur = (float)tariff.EUR;
              bTariffResult.Int = (float)tariff.INT;
              bTariffResult.DispOrder = tariff.DISP_ORDER;
              bPublicTariffSearchResultList.Add(bTariffResult);
          }

          return bPublicTariffSearchResultList;

      }

      public float GetPublicTariffMargin(float Domestic, float Europe, float International)
      {
          float result = -1;

          if (Domestic != -1)
              result = Domestic;
          else if (Europe != -1)
              result = Europe;
          else if (International != -1)
              result = International;
          return result;
      }

      //commented as BPublicTariffSearchResult is changed and it has be modified again [15FEB12RM]
      public int UpdatePublicTariff(List<BPublicTariffSearchResult> bPublicTariffUpdated)
      {
          int result = 1;

          try
          {

              for (int i = 0; i < bPublicTariffUpdated.Count ; i++) //24feb12, removed -1 as last record doesn't considered
              {

                  BPublicTariffSearchResult bTariff = bPublicTariffUpdated[i];

                  string Caption            = bTariff.Caption;
                  string Name               = bTariff.Name;
                  string ShipType           = GetTariffTypeString(bTariff.ShipType);
                  string MasterServiceName  = bTariff.MasterServiceName;
                  int    MinWt              = bTariff.MinWt;
                  int   MaxWt               = bTariff.MaxWt;
                  float Dom                 = bTariff.Dom;
                  float Eur                 = bTariff.Eur;
                  float Int                 = bTariff.Int;

                  #region Old
                  
                  
                  //string Destination = bTariff.Destination;
                  //string MasterServiceName = bTariff.MasterServiceName;
                  //string ShipType = GetTariffTypeString(bTariff.ShipType);
                  //float WeightUpto30 = -1;
                  //float WeightAbove30 = -1;

                  //if (bTariff.WeightRange.Trim() == "0-30")
                  //{
                  //    WeightUpto30 = GetPublicTariffMargin(bTariff.Domestic, bTariff.Europe, bTariff.International);

                  //    for (int j = 0; j < bPublicTariffUpdated.Count; j++)
                  //    {

                  //        if (bPublicTariffUpdated[j].Name.Equals(Name) && bPublicTariffUpdated[j].Origin.Equals(Origin) &&
                  //            bPublicTariffUpdated[j].Destination.Equals(Destination) && bPublicTariffUpdated[j].MasterServiceName.Equals(MasterServiceName) &&
                  //            bPublicTariffUpdated[j].ShipType.Equals(bPublicTariffUpdated[i].ShipType) &&
                  //            bPublicTariffUpdated[j].WeightRange.Trim().Equals("31-N"))
                  //        {
                  //            WeightAbove30 = GetPublicTariffMargin(bPublicTariffUpdated[j].Domestic, bPublicTariffUpdated[j].Europe, bPublicTariffUpdated[j].International);
                  //            break;
                  //        }
                  //    }
                  //}


                  //if (bTariff.WeightRange.Trim() == "31-N")
                  //{
                  //    WeightAbove30 = GetPublicTariffMargin(bTariff.Domestic, bTariff.Europe, bTariff.International);

                  //    for (int j = 0; j < bPublicTariffUpdated.Count; j++)
                  //    {

                  //        if (bPublicTariffUpdated[j].Name.Equals(Name) && bPublicTariffUpdated[j].Origin.Equals(Origin) &&
                  //            bPublicTariffUpdated[j].Destination.Equals(Destination) && bPublicTariffUpdated[j].MasterServiceName.Equals(MasterServiceName) &&
                  //            bPublicTariffUpdated[j].ShipType.Equals(bPublicTariffUpdated[i].ShipType) &&
                  //            bPublicTariffUpdated[j].WeightRange.Trim().Equals("0-30"))
                  //        {
                  //            WeightUpto30 = GetPublicTariffMargin(bPublicTariffUpdated[j].Domestic, bPublicTariffUpdated[j].Europe, bPublicTariffUpdated[j].International);
                  //            break;
                  //        }
                  //    }
                  //}
                  #endregion

                  KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

                  result = (int)context.uSP_PUBLIC_TARIFF_UPDATE(Caption,Name,ShipType,MasterServiceName,MinWt,MaxWt,Dom,Eur,Int).SingleOrDefault();

                  if (result != 0)
                  {
                      string str = "";
                      //Name, Origin, Destination, ShipType, MasterServiceName, WeightUpto30, WeightAbove30
                  }

              }
          }
          catch (Exception error)
          {

              string str = error.Message;
          }

          return result;
      }

      /******** 26DEC11HN***********/
      /// <summary>
      /// Get list of PUBLIC tariff names
      /// </summary>
      /// <returns>
      /// for success =&gt; String List
      /// Failure =&gt; NULL
      /// </returns>
      public List<string> GetPublicTariffNames()
      {
          List<string> result = new List<string>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          var tblTariffNames = context.uSP_GET_PUBLIC_TARIFF_NAMES();
          foreach (var names in tblTariffNames)
          {
              result.Add(names.NAME);
          }

          return result;
      }

      /// <summary>
      /// Get list of tariff names for the carrier
      /// </summary>
      /// <returns>
      /// for success =&gt; String List
      /// Failure =&gt; NULL
      /// </returns>
      public List<BTariffMaster> GetCarrierTariffNames(string TariffType, string Carrier)
      {
          List<BTariffMaster> result = new List<BTariffMaster>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          var tblTariffNames = context.uSP_GET_CARRIER_TARIFF_NAMES(TariffType, Carrier).ToList();

          BTariffMaster b = new BTariffMaster();
          b.TariffReference = "N/A";
          //just to fill default value
          b.CarrierName = Carrier;
          b.ContainerType = "";
          b.EndDate = DateTime.Now;
          b.KeyUserReference = "";
          b.StartDate = DateTime.Now;
          b.TariffType = BEnumTariffType.CarrierPublic;

          result.Add(b);

          foreach (var names in tblTariffNames)
          {
              BTariffMaster t = new BTariffMaster();
              t.TariffReference = names.TARIFF_REFERENCE.Trim();

              //just to fill default value
              t.CarrierName = Carrier;
              t.ContainerType = "";
              t.EndDate = DateTime.Now;
              t.KeyUserReference = "";
              t.StartDate = DateTime.Now;
              t.TariffType = BEnumTariffType.CarrierPublic;

              result.Add(t);
          }

          return result;
      }

      /// <summary>
      /// Get list of Carrier names for given Tariff type
      /// </summary>
      /// <returns>
      /// for success =&gt; String List
      /// Failure =&gt; NULL
      /// </returns>
      public List<BTariffMaster> GetTariffAssignedCarrierNames(string TariffType)
      {
          List<BTariffMaster> result = new List<BTariffMaster>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          var tblTariffNames = context.uSP_GET_CARRIER_NAMES_FOR_TARIFF_TYPE(TariffType).ToList();


          foreach (var names in tblTariffNames)
          {
              BTariffMaster t = new BTariffMaster();
              t.CarrierName = names.CARRIER_NAME.Trim();

              //just to fill default value
              t.TariffReference = "";
              t.ContainerType = "";
              t.EndDate = DateTime.Now;
              t.KeyUserReference = "";
              t.StartDate = DateTime.Now;
              t.TariffType = BEnumTariffType.CarrierPublic;

              result.Add(t);
          }

          return result;
      }

      /// <summary>
      /// To get the list available carriers
      /// </summary>
      /// <returns>List of carriers</returns>
      public List<BTariffMaster> GetAllCarrierNames()
      {
          List<BTariffMaster> result = new List<BTariffMaster>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          var tblCarrierNames = context.uSP_GET_ALL_CARRIER_NAMES().ToList();

          foreach (var names in tblCarrierNames)
          {
              BTariffMaster t = new BTariffMaster();
              t.CarrierName = names.CARRIER_NAME.Trim();
              

              //just to fill default value
              t.TariffReference = "";
              t.ContainerType = "";
              t.EndDate = DateTime.Now;
              t.KeyUserReference = "";
              t.StartDate = DateTime.Now;
              t.TariffType = BEnumTariffType.CarrierPublic;

              result.Add(t);
          }

          return result;
      }

      /// <summary>
      /// Propose optimal carrier for the shipment with quote
      /// </summary>
      /// <param name="bShipmentOrder">Shipment Order information</param>
      /// <returns>
      /// List of carriers with quote
      /// </returns>
      public List<BShipmentQuotation> GetQuoteForTool(List<BTariffMaster> bTariffMaster, string AssignedTariff, string UODType, string Origin, string Destination, string MasterServiceName, float Weight)
      {
          List<BShipmentQuotation> bShipmentQuotation = new List<BShipmentQuotation>();
          List<BTariffInfo> bTariffDetail = new List<BTariffInfo>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          string strPriority = "";
          string strCarrier = "";
          string strService = "";
          string strTariffReference = "";
          string strTariffType = ""; // K => Key type + N => Normal tariff

          for (int i = 0; i < bTariffMaster.Count; i++)
          {

              BShipmentQuotation q = new BShipmentQuotation();

              strCarrier = bTariffMaster[i].CarrierName;
              strTariffReference = bTariffMaster[i].TariffReference;

              decimal f = 12345678.78m;             //07FEB12HN
              //System.Data.Objects.ObjectParameter a = new System.Data.Objects.ObjectParameter("TRY", f);


              ObjectParameter objCalculatedTariff = new ObjectParameter("CALCULATED_TARIFF", f); //07FEB12HN
              ObjectParameter objMargin = new ObjectParameter("MARGIN", f);//07FEB12HN

              ObjectParameter objFuelSurcharge = new ObjectParameter("FUEL_SURCHARGE", f);//07FEB12HN
              //var objFuelSurcharge = new ObjectParameter("FUEL_SURCHARGE", typeof(decimal));
              ObjectParameter objOptionsValue = new ObjectParameter("OPTION_VALUES", typeof(String));
              ObjectParameter objSurchargeValue = new ObjectParameter("SURCHARGE_VALUES", typeof(String));
              ObjectParameter objOptionsDescriptions = new ObjectParameter("OPTION_DESCRIPTION", typeof(String));
              ObjectParameter objSurchargeDescriptions = new ObjectParameter("SURCHARGE_DESCRIPTION", typeof(String));


              System.Nullable<int> iReturnValue2 = context.uSP_GET_TARIFF_DETAILS_FOR_TOOL(strCarrier,
                                                                                          AssignedTariff,
                                                                                          strTariffReference,
                                                                                          Origin,
                                                                                          Destination,
                                                                                          MasterServiceName,
                                                                                          (decimal)Weight,
                                                                                          0,
                                                                                          0,
                                                                                          0,
                                                                                          "Parcel",
                                                                                          1,
                                                                                          objCalculatedTariff,
                                                                                          objMargin,
                                                                                          objFuelSurcharge,
                                                                                          objOptionsValue,
                                                                                          objSurchargeValue,
                                                                                          objOptionsDescriptions,
                                                                                          objSurchargeDescriptions).SingleOrDefault();


              //q.CalculatedTariff = Convert.ToDouble(objCalculatedTariff.Value);
              if (isNumericValidation(objCalculatedTariff.Value.ToString(), System.Globalization.NumberStyles.Float))
              {
                  q.CalculatedTariff = float.Parse(objCalculatedTariff.Value.ToString());
              }
              else
              {
                  q.CalculatedTariff = 0f;
              }

              //q.FuelSurcharge = Convert.ToDouble(objFuelSurcharge.Value);
              if (isNumericValidation(objFuelSurcharge.Value.ToString(), System.Globalization.NumberStyles.Float))
              {
                  q.FuelSurcharge = float.Parse(objFuelSurcharge.Value.ToString());
              }
              else
              {
                  q.FuelSurcharge = 0f;
              }

              q.CarrierName = strCarrier;
              q.CarrierPriority = strPriority;
              q.CarrierType = strTariffType;


              q.Options = objOptionsValue.Value.ToString();
              q.OptionsDescription = objOptionsDescriptions.Value.ToString();
              q.ServiceName = strService;
              q.Surcharge = objSurchargeValue.Value.ToString();
              q.SurchargeDescription = objSurchargeDescriptions.Value.ToString();

              // Just to set default value
              q.DeliveryDate = DateTime.Now;
              q.ShippingDate = DateTime.Now;

              if (q.CalculatedTariff > 0)
                  bShipmentQuotation.Add(q);
          }

          return bShipmentQuotation;

      }

      /// <summary>
      /// To insert Simulation header details : Default Tariff , Validity, Weight detail
      /// </summary>
      /// <param name="bSimulationHeader"></param>
      /// <returns></returns>
      public int SimulationHeaderInsert(BSimulationHeader bSimulationHeader)
      {
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          System.Nullable<int> iReturnValue = context.uSP_SIMULATION_HEADER_INSERT(bSimulationHeader.AccountNo,
                                                                                       bSimulationHeader.AssignedTariff,
                                                                                       bSimulationHeader.SimulationID,
                                                                                       bSimulationHeader.Valid,
                                                                                       (decimal)bSimulationHeader.WeightIncrement,
                                                                                       (decimal)bSimulationHeader.WeightLimit).SingleOrDefault();
          return (int)iReturnValue;
      }

      /// <summary>
      /// To insert Customer Surcharge discount for simulation process
      /// </summary>
      /// <param name="bSimulationSurchargeDiscount"></param>
      /// <returns></returns>
      public int SimulationSurchargeDiscount(List<BSimulationSurchargeDiscount> bSimulationSurchargeDiscount)
      {
          int result = 0;
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          for (int i = 0; i < bSimulationSurchargeDiscount.Count; i++)
          {

              System.Nullable<int> iReturnValue = context.uSP_SIMULATION_SURCHARGE_DISCOUNT(bSimulationSurchargeDiscount[i].AccountNo,
                                                                                                bSimulationSurchargeDiscount[i].SimulationID,
                                                                                                bSimulationSurchargeDiscount[i].CarrierName,
                                                                                                (decimal)bSimulationSurchargeDiscount[i].SafetyDiscount,
                                                                                                (decimal)bSimulationSurchargeDiscount[i].FuelDiscount).SingleOrDefault();
              result = (int)iReturnValue;
          }

          return result;
      }

      /// <summary>
      /// To insert Simulated tariff for the customer
      /// </summary>
      /// <param name="bSimulationTariff"></param>
      /// <returns></returns>
      public int SimulationTariff(List<BSimulationTariff> bSimulationTariff)
      {
          int result = 0;
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          for (int i = 0; i < bSimulationTariff.Count; i++)
          {
              System.Nullable<int> iReturnValue = context.uSP_SIMULATION_TARIFF(bSimulationTariff[i].AccountNo,
                                                                                    bSimulationTariff[i].WeightRange,
                                                                                    bSimulationTariff[i].ShipCountry,
                                                                                    bSimulationTariff[i].DeliveryCountry,
                                                                                    bSimulationTariff[i].MasterServiceName,
                                                                                    (decimal)bSimulationTariff[i].AverageWeight,
                                                                                    (decimal)bSimulationTariff[i].ADV,
                                                                                    (decimal)bSimulationTariff[i].PurchaseFreight,
                                                                                    (decimal)bSimulationTariff[i].PurchaseTariff,
                                                                                    bSimulationTariff[i].PurchaseCarrier,
                                                                                    (decimal)bSimulationTariff[i].PublicDiscount,
                                                                                    bSimulationTariff[i].PublicCarrier,
                                                                                    (decimal)bSimulationTariff[i].PublicFreight,
                                                                                    (decimal)bSimulationTariff[i].PublicTariff,
                                                                                    (decimal)bSimulationTariff[i].PublicTurnOver,
                                                                                    (decimal)bSimulationTariff[i].SaleMargin,
                                                                                    (decimal)bSimulationTariff[i].SaleFreight,
                                                                                    (decimal)bSimulationTariff[i].SaleTariff,
                                                                                    (decimal)bSimulationTariff[i].SaleTurnOver,
                                                                                    (decimal)bSimulationTariff[i].SaleGrossMargin,
                                                                                    (decimal)bSimulationTariff[i].ComparisonSaleTariff,
                                                                                    (decimal)bSimulationTariff[i].ComparisonMargin,
                                                                                    bSimulationTariff[i].SimulationID,
                                                                                    bSimulationTariff[i].PublicSurcharge).SingleOrDefault();

              result = (int)iReturnValue;
          }

          return result;
      }

      /// <summary>
      /// To insert the Tariff details used for simulation
      /// </summary>
      /// <param name="bSimulationTariffBased"></param>
      /// <returns></returns>
      public int SimulationTariffBased(List<BSimulationTariffBased> bSimulationTariffBased)
      {
          int result = 0;
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          for (int i = 0; i < bSimulationTariffBased.Count; i++)
          {
              System.Nullable<int> iReturnValue = context.uSP_SIMULATION_TARIFF_BASED(bSimulationTariffBased[i].AccountNo,
                                                                                        bSimulationTariffBased[i].SimulationID,
                                                                                        bSimulationTariffBased[i].CarrierName,
                                                                                        bSimulationTariffBased[i].TariffReference,
                                                                                        bSimulationTariffBased[i].Assigned,
                                                                                        bSimulationTariffBased[i].TariffType).SingleOrDefault();
              result = (int)iReturnValue;
          }

          return result;
      }

      /// <summary>
      /// To insert Subtotal fo simulation tool
      /// </summary>
      /// <param name="bSimulationSubTotal"></param>
      /// <returns></returns>
      public int SimulationSubTotal(BSimulationSubTotal bSimulationSubTotal)
      {
          int result = 0;
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          System.Nullable<int> iReturnValue = context.uSP_SIMULATION_SUB_TOTAL(bSimulationSubTotal.AccountNo,
                                                                                  bSimulationSubTotal.SimulationID,
                                                                                  (decimal)bSimulationSubTotal.SubTotalWeight,
                                                                                  (decimal)bSimulationSubTotal.SubTotalADV,
                                                                                  (decimal)bSimulationSubTotal.SubTotalFreight,
                                                                                  (decimal)bSimulationSubTotal.SubTotalPurchase,
                                                                                  (decimal)bSimulationSubTotal.SubTotalCurrentDiscount,
                                                                                  (decimal)bSimulationSubTotal.SubTotalPublic,
                                                                                  (decimal)bSimulationSubTotal.SubTotalCurrentSale,
                                                                                  (decimal)bSimulationSubTotal.SubTotalTurnOver,
                                                                                  (decimal)bSimulationSubTotal.SubTotalGrossMargin,
                                                                                  (decimal)bSimulationSubTotal.SubTotalSalesFretTariff,
                                                                                  (decimal)bSimulationSubTotal.SubTotalProposedSalesTariff,
                                                                                  (decimal)bSimulationSubTotal.SubTotalSalesTurnOver,
                                                                                  (decimal)bSimulationSubTotal.SubTotalSalesGrossMargin,
                                                                                  (decimal)bSimulationSubTotal.SubtotalCompare,
                                                                                  (decimal)bSimulationSubTotal.SubTotalCompareMargin).SingleOrDefault();

          result = (int)iReturnValue;

          return result;

      }

      /// <summary>
      /// To get the list of Assigned users
      /// </summary>
      /// <param name="AccountNo"></param>
      /// <returns></returns>
      public List<BUserID> GetAssignedUsers(string AccountNo)
      {
          List<BUserID> result = new List<BUserID>();
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var tblUserID = context.uSP_GET_ASSGINED_USERS(AccountNo).ToList();
          foreach (var names in tblUserID)
          {
              BUserID t = new BUserID();
              t.AccountNo = names.ACCOUNT_NO.ToString();
              t.UserName = names.EMAIL.ToString();
              result.Add(t);
          }

          return result;
      }

      /// <summary>
      /// To get the list of Simulation IDs of given Account numbers
      /// </summary>
      /// <param name="AccountNo"></param>
      /// <returns></returns>
      public List<BSimulationList> GetSimulationID(string AccountNo)
      {
          List<BSimulationList> result = new List<BSimulationList>();
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var tblSimulationID = context.uSP_GET_SIMULATION_ID(AccountNo).ToList();
          foreach (var names in tblSimulationID)
          {
              BSimulationList t = new BSimulationList();
              t.DataField = names.SIMULATION_ID.ToString();
              t.TextField = names.TEXT_FIELD.ToString();
              result.Add(t);
          }

          return result;

      }

      /// <summary>
      /// To get the discount for the customer for specific route
      /// </summary>
      /// <param name="AccountNo">Selected customer account number</param>
      /// <param name="bTariffMaster">Objec filled with Carrier, TariffREf </param>
      /// <param name="Weight">List of Weight</param>
      /// <param name="Origin">Country code</param>
      /// <param name="Destination"> Country Code</param>
      /// <param name="MasterServiceName"> Master Service Name</param>
      /// <returns></returns>
      public List<string> GetDiscount(string AccountNo, List<BTariffMaster> bTariffMaster, List<float> Weight, string Origin, string Destination, string MasterServiceName)
      {
          List<string> Discount = new List<string>();


          return Discount;
      }

      /// <summary>
      /// To get the simulated tariff for the Selected Simulation ID 
      /// </summary>
      /// <param name="SimulationID"></param>
      /// <returns></returns>
      public List<BSimulationTariff> GetSimulationTariff(string SimulationID)
      {
          List<BSimulationTariff> result = new List<BSimulationTariff>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var tblSimulationTariff = context.uSP_GET_SIMULATION_TARIFF(SimulationID);


          foreach (var tariff in tblSimulationTariff)
          {
              BSimulationTariff t = new BSimulationTariff();
              t.AccountNo = tariff.ACCOUNT_NO.ToString();
              t.ADV = (float)tariff.ADV;
              t.AverageWeight = (float)tariff.AVERAGE_WEIGHT;
              t.ComparisonMargin = (float)tariff.COMPARISON_MARGIN;
              t.ComparisonSaleTariff = (float)tariff.COMPARISON_SALE_TARIFF;
              t.DeliveryCountry = tariff.DELIVERY_COUNTRY;
              t.MasterServiceName = tariff.MASTER_SERVICE_NAME;
              t.PublicCarrier = tariff.PUBLIC_CARRIER;
              t.PublicDiscount = (float)tariff.PUBLIC_DISCOUNT;
              t.PublicFreight = (float)tariff.PUBLIC_FREIGHT;
              t.PublicTariff = (float)tariff.PUBLIC_TARIFF;
              t.PublicTurnOver = (float)tariff.PUBLIC_TURNOVER;
              t.PurchaseCarrier = tariff.PURCHASE_CARRIER;
              t.PurchaseFreight = (float)tariff.PURCHASE_FREIGHT;
              t.PurchaseTariff = (float)tariff.PURCHASE_TARIFF;
              t.SaleFreight = (float)tariff.SALE__FREIGHT;
              t.SaleGrossMargin = (float)tariff.SALE_GROSS_MARGIN;
              t.SaleMargin = (float)tariff.SALE_MARGIN;
              t.SaleTariff = (float)tariff.SALE_TARIFF;
              t.SaleTurnOver = (float)tariff.SALE_TURNOVER;
              t.ShipCountry = tariff.SHIP_COUNTRY;
              t.SimulationID = tariff.SIMULATION_ID;
              t.WeightRange = tariff.WEIGHT_RANGE;
              t.PublicSurcharge = tariff.PUBLIC_SURCHARGE;
              result.Add(t);
          }

          return result;
      }

      /// <summary>
      /// To get the header details for the given Simulation ID
      /// </summary>
      /// <param name="SimulationID"></param>
      /// <returns></returns>
      public BSimulationHeader GetSimulationHeader(string SimulationID)
      {
          BSimulationHeader result = new BSimulationHeader();
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var tblSimulationHeader = context.uSP_GET_SIMULATION_HEADER(SimulationID).ToList();
          if (tblSimulationHeader.Count == 0)
          {
              result.AccountNo = "";
              result.AssignedTariff = "";
              result.SimulationID = "";
              result.Valid = DateTime.Now;
              result.WeightIncrement = 0;
              result.WeightLimit = 0;
          }
          else
          {
              foreach (var names in tblSimulationHeader)
              {
                  result.AccountNo = names.ACCOUNT_NO;
                  result.AssignedTariff = names.ASSIGNED_TARIFF;
                  result.SimulationID = names.SIMULATION_ID;
                  result.Valid = Convert.ToDateTime(names.VALID);
                  result.WeightIncrement = (float) names.WEIGHT_INCREMENT;
                  result.WeightLimit = (float) names.WEIGHT_LIMIT;
              }
          }
          return result;

      }

      /// <summary>
      /// To get the Customer Discounts of Surcharges for Simulation
      /// </summary>
      /// <param name="SimulationID"></param>
      /// <returns></returns>
      public List<BSimulationSurchargeDiscount> GetSimulationSurchargeDiscount(string SimulationID)
      {
          List<BSimulationSurchargeDiscount> result = new List<BSimulationSurchargeDiscount>();
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var tblSimulationHeader = context.uSP_GET_SIMULATION_SURCHARGE_DISCOUNT(SimulationID).ToList();
          foreach (var names in tblSimulationHeader)
          {
              BSimulationSurchargeDiscount t = new BSimulationSurchargeDiscount();
              t.AccountNo = names.ACCOUNT_NO;
              t.CarrierName = names.CARRIER_NAME;
              t.FuelDiscount = (float)names.FUEL_DISCOUNT;
              t.SafetyDiscount = (float)names.SAFETY_DISCOUNT;
              t.SimulationID = names.SIMULATION_ID;
              result.Add(t);
          }

          return result;

      }

      /// <summary>
      /// To get the Tariff list to calculate the Simulation 
      /// </summary>
      /// <param name="SimulationID"></param>
      /// <returns></returns>
      public List<BSimulationTariffBased> GetSimulationTariffBased(string SimulationID)
      {
          List<BSimulationTariffBased> result = new List<BSimulationTariffBased>();
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var tblSimulationBased = context.uSP_GET_SIMULATION_TARIFF_BASED(SimulationID).ToList();
          foreach (var names in tblSimulationBased)
          {
              BSimulationTariffBased t = new BSimulationTariffBased();
              t.AccountNo = names.ACCOUNT_NO;
              t.CarrierName = names.CARRIER_NAME;
              t.Assigned = names.ASSIGNED;
              t.SimulationID = names.SIMULATION_ID;
              t.TariffReference = names.TARIFF_REFERENCE;
              t.TariffType = names.TARIFF_TYPE;

              result.Add(t);
          }

          return result;

      }

      /// <summary>
      /// To get the calculated Sub totals of the given simulation ID
      /// </summary>
      /// <param name="SimulationID"></param>
      /// <returns></returns>
      public BSimulationSubTotal GetSimulationSubTotal(string SimulationID)
      {
          BSimulationSubTotal result = new BSimulationSubTotal();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var tblSimulationSubTotal = context.uSP_GET_SIMULATION_SUB_TOTAL(SimulationID).ToList();
          foreach (var names in tblSimulationSubTotal)
          {
              result.AccountNo = names.ACCOUNT_NO;
              result.SimulationID = names.SIMULATION_ID;
              result.SubTotalADV = (float)names.TOTAL_ADV;
              result.SubTotalCompareMargin = (float)names.TOTAL_COMPARE_MARGIN;
              result.SubtotalCompare = (float)names.TOTAL_COMPARE_SALES;
              result.SubTotalCurrentDiscount = (float)names.TOTAL_CURRENT_DISCOUNT;
              result.SubTotalCurrentSale = (float)names.TOTAL_CURRENT_SALE;
              result.SubTotalFreight = (float)names.TOTAL_FREIGHT;
              result.SubTotalGrossMargin = (float)names.TOTAL_GROSS_MARGIN;
              result.SubTotalProposedSalesTariff = (float)names.TOTAL_PROPOSED_SALES_TARIFF;
              result.SubTotalPublic = (float)names.TOTAL_PUBLIC;
              result.SubTotalPurchase = (float)names.TOTAL_PURCHASE;
              result.SubTotalSalesFretTariff = (float)names.TOTAL_SALES_FRET_TARIFF;
              result.SubTotalSalesGrossMargin = (float)names.TOTAL_SALES_GROSS_MARGIN;
              result.SubTotalSalesTurnOver = (float)names.TOTAL_SALES_TURNOVER;
              result.SubTotalTurnOver = (float)names.TOTAL_TURNOVER;
              result.SubTotalWeight = (float)names.TOTAL_WEIGHT;
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
      public BSurchargeDetails GetSimulationSurcharge(string TariifReference, string Origin, String Destination, float Weight, string MasterServiceName)
      {
          BSurchargeDetails bSurchargeDetails = new BSurchargeDetails();
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          decimal f = 12345678.78m; //07FEB12HN

          ObjectParameter objServiceID = new ObjectParameter("SERVICE_ID", typeof(int));
          ObjectParameter objServiceName = new ObjectParameter("SERVICE_NAME", typeof(String));
          ObjectParameter objCarrierName = new ObjectParameter("CARRIER_NAME", typeof(String));


          System.Nullable<int> iReturnValue1 = context.uSP_GET_CARRIER_DETAIL_FOR_TARIFF_REF(TariifReference, MasterServiceName, objServiceID, objServiceName, objCarrierName).SingleOrDefault();

          int ServiceId = 0;
          if (isNumericValidation(objServiceID.Value.ToString(), System.Globalization.NumberStyles.Integer))
          {
              ServiceId = Int32.Parse(objServiceID.Value.ToString());
          }
          else
          {
              ServiceId = 0;
          }
          string ServiceName = objServiceName.Value.ToString();
          string CarrierName = objCarrierName.Value.ToString();


          ObjectParameter objSurchargeTotal = new ObjectParameter("SURCHARGE_TOTAL", f); //07FEB12HN
          ObjectParameter objSurchargeValue = new ObjectParameter("SURCHARGE_VALUES", typeof(String));
          ObjectParameter objSurchargeDescriptions = new ObjectParameter("SURCHARGE_DESCRIPTION", typeof(String));

          System.Nullable<int> iReturnValue2 = context.uSP_GET_SURCHARGE(TariifReference.Trim(),
                                                                             ServiceId,
                                                                             MasterServiceName,
                                                                             CarrierName,
                                                                             Origin,
                                                                             Destination,
                                                                             "",
                                                                             1,
                                                                             (decimal)Weight,
                                                                             0,
                                                                             0,
                                                                             0,
                                                                             "",
                                                                             objSurchargeValue,
                                                                             objSurchargeDescriptions,
                                                                             objSurchargeTotal).SingleOrDefault();


          bSurchargeDetails.ParamID = objSurchargeValue.Value.ToString();           //Used to take list of values
          bSurchargeDetails.SurchageCode = objSurchargeDescriptions.Value.ToString();

          return bSurchargeDetails;
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
          decimal f = 12345678.78m; //07FEB12HN

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          ObjectParameter objServiceID = new ObjectParameter("SERVICE_ID", typeof(int));
          ObjectParameter objServiceName = new ObjectParameter("SERVICE_NAME", typeof(String));
          ObjectParameter objCarrierName = new ObjectParameter("CARRIER_NAME", typeof(String));


          System.Nullable<int> iReturnValue1 = context.uSP_GET_CARRIER_DETAIL_FOR_TARIFF_REF(TariifReference, MasterServiceName, objServiceID, objServiceName, objCarrierName).SingleOrDefault();

          int ServiceId = 0;
          if (isNumericValidation(objServiceID.Value.ToString(), System.Globalization.NumberStyles.Integer))
          {
              ServiceId = Int32.Parse(objServiceID.Value.ToString());
          }
          else
          {
              ServiceId = 0;
          }
          string ServiceName = objServiceName.Value.ToString();
          string CarrierName = objCarrierName.Value.ToString();

          ObjectParameter objFuelCharge = new ObjectParameter("RESULT", f); //07FEB12HN

          System.Nullable<int> iReturnValue2 = context.uSP_GET_FUEL_SURCHARGE(ServiceId, ServiceName, "", "", MasterServiceName, (decimal)TariffAmount, objFuelCharge).SingleOrDefault();

          result = (float)Convert.ToDecimal(objFuelCharge.Value.ToString());

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
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          System.Nullable<int> iReturnValue1 = context.uSP_SIMULATION_DELETE(SimulationID).SingleOrDefault();

          result = (int)iReturnValue1;

          return result;
      }
      /******** Dummy ***********/
      /// <summary>
      /// Creating a Zone
      /// </summary>
      /// <returns>
      /// 0 =&gt; Success
      /// 1 =&gt; Fail
      /// 2 =&gt; Zone already exists
      /// </returns>
      
      public int CreateTariffID()
      {
          throw new System.NotImplementedException();
      }

      public int GetTariffID()
      {
          return 121212;
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
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          System.Nullable<int> iReturnValue1 = context.uSP_CHECK_SIMULATION_EXIST(Origin, Destination, MasterType, AccountNo).SingleOrDefault();

          if (iReturnValue1 > 0)
          {
              result = true;
          }
          return result;
      }

      /*16FEB12HN*/

      /// <summary>
      /// To get the Gross total for the account . Excluding given simulation ID
      /// It was excluded because it will be added dynamically at the time of EDIT
      /// </summary>
      /// <param name="SimulationID"></param>
      /// <param name="AccountNo"></param>
      /// <returns></returns>
      public List<BSimulationSubTotal> GetSimulationGrossTotal(string SimulationID, string AccountNo)
      {
          List<BSimulationSubTotal> result = new List<BSimulationSubTotal>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var tblSimulationSubTotal = context.uSP_GET_SIMULATION_GROSS_TOTAL(AccountNo, SimulationID).ToList();
          foreach (var names in tblSimulationSubTotal)
          {
              BSimulationSubTotal r = new BSimulationSubTotal();

              r.AccountNo = AccountNo;
              r.SimulationID = names.SIMULATION_ID;
              r.SubTotalADV = (float)names.TOTAL_ADV;
              r.SubTotalCompareMargin = (float)names.TOTAL_COMPARE_MARGIN;
              r.SubtotalCompare = (float)names.TOTAL_COMPARE_SALES;
              r.SubTotalCurrentDiscount = (float)names.TOTAL_CURRENT_DISCOUNT;
              r.SubTotalCurrentSale = (float)names.TOTAL_CURRENT_SALE;
              r.SubTotalFreight = (float)names.TOTAL_FREIGHT;
              r.SubTotalGrossMargin = (float)names.TOTAL_GROSS_MARGIN;
              r.SubTotalProposedSalesTariff = (float)names.TOTAL_PROPOSED_SALES_TARIFF;
              r.SubTotalPublic = (float)names.TOTAL_PUBLIC;
              r.SubTotalPurchase = (float)names.TOTAL_PURCHASE;
              r.SubTotalSalesFretTariff = (float)names.TOTAL_SALES_FRET_TARIFF;
              r.SubTotalSalesGrossMargin = (float)names.TOTAL_SALES_GROSS_MARGIN;
              r.SubTotalSalesTurnOver = (float)names.TOTAL_SALES_TURNOVER;
              r.SubTotalTurnOver = (float)names.TOTAL_TURNOVER;
              r.SubTotalWeight = (float)names.TOTAL_WEIGHT;

              result.Add(r);

          }
          return result;
      }


      /*01MAR12HN*/
      /// <summary>
      /// To Assign a calculated tariff using Simulation Tool
      /// </summary>
      /// <param name="bSimulationTariff"></param>
      /// <returns></returns>
      public int SimulationAssign(List<BSimulationTariff> bSimulationTariff)
      {
          int result = 0;
          int FirstTime = 0;
          string[] Weight = new string[2];

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          for (int i = 0; i < bSimulationTariff.Count; i++)
          {
              Weight = bSimulationTariff[i].WeightRange.Split('-');

              System.Nullable<int> iReturnValue = context.uSP_SIMULATION_ASSIGN(bSimulationTariff[i].SimulationID,
                                                        bSimulationTariff[i].AccountNo,
                                                        (decimal)Convert.ToDecimal(Weight[0]),
                                                        (decimal)Convert.ToDecimal(Weight[1]),
                                                        (decimal)bSimulationTariff[i].AverageWeight,
                                                        bSimulationTariff[i].ShipCountry,
                                                        bSimulationTariff[i].DeliveryCountry,
                                                        bSimulationTariff[i].MasterServiceName,
                                                        bSimulationTariff[i].PublicCarrier,         //Used to send Assigned Tariff Public1/Publilc2
                                                        (decimal)bSimulationTariff[i].SaleMargin,
                                                        bSimulationTariff[i].PurchaseCarrier,
                                                        (decimal)0,
                                                        bSimulationTariff[i].ContainerType).SingleOrDefault();
              result = (int)iReturnValue;
              if (i == 0 && result == 2)   // Self Creation
              {
                  FirstTime = 1;
              }

              if (i == 0 && result == 3)
              {
                  FirstTime = 2;
              }
          }

          return FirstTime;
      }

      /*24APR12KM*/
      /// <summary>
      /// 
      /// </summary>
      /// <param name="bTariffMaster"></param>
      /// <returns></returns>
      public string UpdateMasterTariff(BTariffMaster bTariffMaster)
      {
          BTariffCreationAcknowledgement bTariffCreationAck = new BTariffCreationAcknowledgement(); ;
          string cAR_NAME = bTariffMaster.CarrierName.Trim();
          string tAR_REF = bTariffMaster.TariffReference;
          string cAR_TYPE = bTariffMaster.TariffType.ToString();
          string ContainerType = bTariffMaster.ContainerType;
          string KeyUserReference = bTariffMaster.KeyUserReference;
          DateTime s_DATE = bTariffMaster.StartDate;
          DateTime e_DATE = bTariffMaster.EndDate;


          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          string result=context.uSP_TARIFF_VAL(cAR_NAME, cAR_TYPE, s_DATE, e_DATE, tAR_REF).SingleOrDefault();
           


          return result;

      }

      public BUser GetUserDetailForAssignTariff(string AccountNo)
      {
          BUser bUser = new BUser();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          ObjectParameter objAdminPhone = new ObjectParameter("ADMIN_PHONE", typeof(string));
          ObjectParameter objAdminMail = new ObjectParameter("ADMIN_MAIL", typeof(String));
          ObjectParameter objCustName = new ObjectParameter("CUST_NAME", typeof(String));
          ObjectParameter objCustCompany = new ObjectParameter("CUST_COMPANY", typeof(String));

          System.Nullable<int> iReturnValue1 = context.uSP_GET_USER_DETAIL_FOR_ASSIGN_TARIFF(AccountNo, objAdminPhone, objAdminMail, objCustName, objCustCompany).SingleOrDefault();

          bUser.AccountNo = AccountNo;
          bUser.CompanyName = objCustCompany.Value.ToString();
          bUser.CreatedBy = objAdminMail.Value.ToString();
          bUser.Name = objCustName.Value.ToString();
          bUser.TelephoneNo = objAdminPhone.Value.ToString();

          #region Default values for Buser
          bUser.Country = "";
          bUser.CreatedDate = DateTime.Now;
          bUser.CreatedUserType = BEnumUserType.Administrator;
          bUser.CustomerType = BEnumCustomerType.RegularCustomer;
          bUser.CustomerTypeChanged = BEnumFlag.No;
          bUser.Email = "";
          bUser.IsChangePasswordRequired = BEnumFlag.No;
          bUser.IsSalesTarrifAssigned = BEnumFlag.No;
          bUser.IsToSAccepted = BEnumFlag.No;
          bUser.Language = "";
          bUser.LastLogin = DateTime.Now;
          bUser.LastUpdate = DateTime.Now;
          bUser.LegalForm = "";
          bUser.Password = "";
          bUser.SiretNo = "";
          bUser.Status = BEnumUserStatus.BeingCreated;
          bUser.ToSAcceptedDate = DateTime.Now;
          bUser.UserType = BEnumUserType.Administrator;
          bUser.VatNo = "";
          #endregion

          return bUser;


      }

      /*14MAY12KS*/
      /// <summary>
      /// 
      /// </summary>
      /// <param name="validatedcountry"></param>
      /// <returns></returns>
      public string Getvalidatezone(string zonecountry, string tariffreference, string Geographical_coverage, int zone_id)
      {
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          ObjectParameter objParamresult = new ObjectParameter("RESULT", typeof(String));
          context.uSP_VALIDATE_ZONE_COVREAGELIST(zonecountry, tariffreference, Geographical_coverage, zone_id, objParamresult);
          string result = objParamresult.Value.ToString();
          return result;
      }

  }
}
