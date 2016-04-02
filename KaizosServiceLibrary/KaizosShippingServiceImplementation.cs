using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.ServiceModel;
using System.IO;

//log4net libraries
using log4net;
using log4net.Config;

//kaizos libraries
using Kaizos.Entities.Business;  //business entity
using KaizosServiceLibrary.Model;  //service entity
using KaizosServiceLibrary.Adapter; //conversion between business and service entity vice versa
using Kaizos.Components.GlobalLibrary; //global general method
using Kaizos.Components.ToSManager; //business logic component to be invoked
using Kaizos.Components.ShippingManager;
using Kaizos.Components.TariffManager;
using Kaizos.Components.CarrierManager; //to access all carrier related logics tnt, gls,...


namespace KaizosServiceLibrary
{
    public partial class KaizosService : IKaizosServiceContract
    {
        /// <summary>
        /// Insert shipment order and details  (Use case : 2.1)
        /// </summary>
        /// <returns>
        /// 0 =&gt; Success
        /// 1 =&gt; Fail
        /// 2 =&gt; Failed due to shipment reference already exist
        /// </returns>
        public int CreateSingleShipment(SShipmentOrder sShipmentOrder, SShipmentQuotation sShipmentQuotation, string strSessionID)
        {
            int result;
            try
            {
                //1.Convert Service Entity to Business Entity
                ShippingAdapter adapter = new ShippingAdapter();
                BShipmentOrder bShipmentOrder = adapter.ConvertStoB_ShipmentOrder(sShipmentOrder);
                BShipmentQuotation bShipmentQuotation = adapter.ConvertStoB_ShipmentQuotation(sShipmentQuotation);

                //2. Create instance for business object and invoke method            

                ShippingHandler shippingHandler = new ShippingHandler();
                shippingHandler.CreateSingleShipment(bShipmentOrder,  bShipmentQuotation, strSessionID);
                // termsOfService.InsertToS(bToS);
                result = 0;
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in creating single shipment";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in creating single shipment"));
            }

            return result;
        }

        //public List<STariffName> GetAffectedTariffName(string strAccountNo)
        //{
        //    List<STariffName> sTariffName = new List<STariffName>();
        //    try
        //    {
        //        //1.Convert Service Entity to Business Entity
        //        ShippingAdapter adapter = new ShippingAdapter();
        //        List<BTariffName> bTariffName;// = new List<BCountryTable>();
                
        //        //2. Create instance for business object and invoke method            
        //        ShippingHandler shippingHandler = new ShippingHandler();
        //        bTariffName = shippingHandler.GetAffectedTariffName(strAccountNo);
        //        sTariffName = adapter.ConvertBtoS_TariffNameList(bTariffName);
                
        //    }
        //    catch (Exception error)
        //    {
        //        logger.Debug("From service implementation :" + Library.ExtractError(error));
        //        var generalFault = new SGeneralFault();
        //        generalFault.Issue = "Problem inserting profile";
        //        generalFault.Details = Library.ExtractError(error);
        //        throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem inserting profile"));
        //    }
        //    return sTariffName.ToList();
        //}

        /// <summary>
        /// Get list of tariff details
        /// </summary>
        /// <returns>
        /// for success =&gt; String List
        /// Failure =&gt; NULL
        /// </returns>
        public List<SShipmentQuotation> GetQuote(SShipmentOrder sShipmentOrder)
        {
            List<SShipmentQuotation> sShipmentQuotation = new List<SShipmentQuotation>();
            try
            {
                //1.Convert Service Entity to Business Entity
                ShippingAdapter adapter = new ShippingAdapter();
                BShipmentOrder bShipmentOrder = new BShipmentOrder();
                List<BShipmentQuotation> bShipmentQuotation;// = new List<BCountryTable>();
                bShipmentOrder = adapter.ConvertStoB_ShipmentOrder(sShipmentOrder);

                //2. Create instance for business object and invoke method    
                ShippingHandler shipping = new ShippingHandler();
                //adapter.con
                bShipmentQuotation = shipping.GetQuote(bShipmentOrder); //library.FillCountryCombo();
                sShipmentQuotation = adapter.ConvertBtoS_ShipmentQuotationList(bShipmentQuotation);
                //sTariffDetail = adapter.ConvertBtoS_ShipmentQuotationList(bShipmentQuotation);

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem getting Tariff details";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem getting Tariff details"));
            }
            return sShipmentQuotation.ToList();
        }

        /// <summary>
        /// Updates all flags and address details for the confirmed shipment
        /// </summary>
        /// <returns>
        /// 0 =&gt; Success
        /// 1 =&gt; Fail
        /// 2 =&gt; Shipment reference not available
        /// </returns>
        public int ConfirmShipment(SShipmentOrder sShipmentOrder)
        {
            int result = 0;
            try
            {
                //1.Convert Service Entity to Business Entity
                ShippingAdapter adapter = new ShippingAdapter();
                BShipmentOrder bShipmentOrder = new BShipmentOrder();
                bShipmentOrder = adapter.ConvertStoB_ShipmentOrder(sShipmentOrder);

                //2. Create instance for business object and invoke method    
                ShippingHandler shipping = new ShippingHandler();
                result = shipping.ConfirmShipment(bShipmentOrder);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem getting Tariff details";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem getting Tariff details"));
            }
            return result;
        }

         /// <summary>
        /// To get all the confirmed order to ship for the session
        /// </summary>
        /// <returns>
        /// List of ShipmentOrders
        /// </returns>
        public List<SShipmentOrder> GetShipmentDetails(int iGroupID, string strSessionID, string strStatus, string strAccountNo)
        {
            List<SShipmentOrder> sShipmentOrder = new List<SShipmentOrder>();
            try
            {
                //1.Convert Service Entity to Business Entity
                List<BShipmentOrder> bShipmentOrder;
                ShippingAdapter adapter = new ShippingAdapter();

                //2. Create instance for business object and invoke method    
                ShippingHandler shipping = new ShippingHandler();
                bShipmentOrder = shipping.GetShipmentDetails(iGroupID, strSessionID, strStatus, strAccountNo);
                sShipmentOrder = adapter.ConvertBtoS_ShipmentOrderList(bShipmentOrder);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem getting Confirmed shipment details";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem getting Confirmed shipment details"));
            }

            return sShipmentOrder.ToList();
        }

         /// <summary>
        /// To get all the Order information for the given shipment reference
        /// </summary>
        /// <returns>
        /// Order information
        /// </returns>
        public SShipmentOrder GetOrderInformation(string ShipmentReference)
        {
            SShipmentOrder sShipmentOrder = new SShipmentOrder();
            try
            {
                //1.Convert Service Entity to Business Entity
                BShipmentOrder bShipmentOrder;
                ShippingAdapter adapter = new ShippingAdapter();

                //2. Create instance for business object and invoke method    
                ShippingHandler shipping = new ShippingHandler();
                bShipmentOrder = shipping.GetOrderInformation(ShipmentReference);
                sShipmentOrder = adapter.ConvertBtoS_ShipmentOrder(bShipmentOrder);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem getting Confirmed shipment details";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem getting Confirmed shipment details"));
            }
            return sShipmentOrder;
        }

          /// <summary>
        /// To get all the Payment method and infor of the user
        /// </summary>
        /// <returns>
        /// Payment method and admin id
        /// </returns>
        public SPaymentInfo GetPaymentMethodAndInfo(string strAccountNo, string strUserType)
        {
            SPaymentInfo sPaymentInfo = new SPaymentInfo();
            try
            {
                //1.Convert Service Entity to Business Entity
                BPaymentInfo bPaymentInfo;
                ShippingAdapter adapter = new ShippingAdapter();

                //2. Create instance for business object and invoke method    
                ShippingHandler shipping = new ShippingHandler();
                bPaymentInfo = shipping.GetPaymentMethodAndInfo(strAccountNo, strUserType);
                sPaymentInfo = adapter.ConvertBtoS_PaymentInfo(bPaymentInfo);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem getting Payment method and info";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem getting Payment method and info"));
            }


            return sPaymentInfo;
        
        }

        /// <summary>
        /// To get all the Deferred payment process
        /// </summary>
        /// <returns>
        /// Updates the closed shipments and deduct total amount from available limit 
        /// </returns>
        public int DeferredPayment(List<string> ClosedReference, string AccountNo, float fTotalAmount)
        {
            int result = 0;
            try
            {
                //1.Convert Service Entity to Business Entity
               

                //2. Create instance for business object and invoke method    
                ShippingHandler shipping = new ShippingHandler();
                result = shipping.DeferredPayment(ClosedReference, AccountNo, fTotalAmount);
                
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem updating Deferred Payment";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem updating Deferred Payment"));
            }
            return result;
        }

        public int DeleteShipment(string ShipmentReference)
        {
            int result = 0;
            try
            {
                //1.Convert Service Entity to Business Entity


                //2. Create instance for business object and invoke method    
                ShippingHandler shipping = new ShippingHandler();
                result = shipping.DeleteShipment(ShipmentReference);

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem deleting shipment details";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem deleting shipment details"));
            }
            return result;
        }

        /// <summary>
        /// Cancel the shipment by passing shipment reference
        /// </summary>
        /// <param name="ShipmentReference"></param>
        /// <returns>
        ///  0 => Success
        ///  1 => Fail
        ///  2 => Failed die to shipment reference not available
        /// </returns>
        public int CancelShipment(string ShipmentReference)
        {
            int result = 0;

            try
            {
                //1.Convert Service Entity to Business Entity


                //2. Create instance for business object and invoke method    
                ShippingHandler shipping = new ShippingHandler();
                result = shipping.DeleteShipment(ShipmentReference);

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem canceling shipment details";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem canceling shipment details"));
            }

            return result;
        }

        /// <summary>
        /// Carrier Process
        /// </summary>
        /// <param name="ShipmentReference"></param>
        /// <returns>
        /// 
        /// </returns>
        public int CarrierProcess(List<SShipmentOrder> sShipmentOrder)
        {
            int result = 0;
            try
            {
                //1.Convert Service Entity to Business Entity
                ShippingAdapter adapter = new ShippingAdapter();
                List<BShipmentOrder> bShipmentOrder = new List<BShipmentOrder>();
                
                bShipmentOrder = adapter.ConvertStoB_ShipmentOrdersList(sShipmentOrder);

                //2. Create instance for business object and invoke method   
                ShippingHandler shipping = new ShippingHandler();
                result = shipping.CarrierProcess(bShipmentOrder);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in processing carrier logic";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in processing carrier logic"));
            }
            return result;
        }
        /************** Jan 2012 *****************/

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

            try
            {
                //1.Convert Service Entity to Business Entity


                //2. Create instance for business object and invoke method   
                TariffHandler tariff = new TariffHandler();
                result = tariff.GetPublicTariffNames();
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in processing carrier logic";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in processing carrier logic"));
            }
            return result;


        }

        /// <summary>
        /// Get list of PUBLIC tariff names
        /// </summary>
        /// <returns>
        /// for success =&gt; String List
        /// Failure =&gt; NULL
        /// </returns>
        public List<STariffMaster> GetCarrierTariffNames(string TariffType, string Carrier)
        {
            List<STariffMaster> result = new List<STariffMaster>();


            try
            {
                //1.Convert Service Entity to Business Entity
                List<BTariffMaster> bTariffMaster = new List<BTariffMaster>();
                TariffAdapter adapter = new TariffAdapter();

                //2. Create instance for business object and invoke method   
                TariffHandler tariff = new TariffHandler();
                bTariffMaster = tariff.GetCarrierTariffNames(TariffType, Carrier);
                result = adapter.ConvertBtoS_TariffMaster(bTariffMaster);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in processing carrier logic";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in processing carrier logic"));
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
        public List<STariffMaster> GetTariffAssignedCarrierNames(string TariffType)
        {
            List<STariffMaster> result = new List<STariffMaster>();


            try
            {
                //1.Convert Service Entity to Business Entity
                List<BTariffMaster> bTariffMaster = new List<BTariffMaster>();
                TariffAdapter adapter = new TariffAdapter();

                //2. Create instance for business object and invoke method   
                TariffHandler tariff = new TariffHandler();
                bTariffMaster = tariff.GetTariffAssignedCarrierNames(TariffType);
                result = adapter.ConvertBtoS_TariffMaster(bTariffMaster);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in processing carrier logic";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in processing carrier logic"));
            }
            return result;

        }

        public List<SShipmentQuotation> GetQuoteForTool(List<STariffMaster> sTariffMaster, string AssignedTariff, string UODType, string Origin, string Destination, string MasterServiceName, float Weight)
        {

            List<SShipmentQuotation> result = new List<SShipmentQuotation>();


            try
            {
                //1.Convert Service Entity to Business Entity
                List<BShipmentQuotation> bShipmentQuotation = new List<BShipmentQuotation>();
                List<BTariffMaster> bTariffMaster = new List<BTariffMaster>();

                TariffAdapter adapter = new TariffAdapter();
                ShippingAdapter sAdapter = new ShippingAdapter();


                //2. Create instance for business object and invoke method   
                TariffHandler tariff = new TariffHandler();
                bTariffMaster = adapter.ConvertStoB_TariffMasterList(sTariffMaster);

                bShipmentQuotation = tariff.GetQuoteForTool(bTariffMaster, AssignedTariff, UODType, Origin, Destination, MasterServiceName, Weight);

                result = sAdapter.ConvertBtoS_ShipmentQuotationList(bShipmentQuotation).ToList();
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in processing carrier logic";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in processing carrier logic"));
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
            try
            {
                //1.Convert Service Entity to Business Entity
                List<BTariffMaster> bTariffMaster = new List<BTariffMaster>();
                TariffAdapter adapter = new TariffAdapter();

                //2. Create instance for business object and invoke method   
                TariffHandler tariff = new TariffHandler();
                bTariffMaster = tariff.GetAllCarrierNames();
                result = adapter.ConvertBtoS_TariffMaster(bTariffMaster);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in getting Carrier Names for Tool";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in getting Carrier Names for Tool"));
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

            try
            {
                //1.Convert Service Entity to Business Entity
                TariffAdapter adapter = new TariffAdapter();
                BSimulationHeader bSimulationHeader = new BSimulationHeader();

                bSimulationHeader = adapter.ConvertStoB_SimulationHeader(sSimulationHeader);

                //2. Create instance for business object and invoke method   
                TariffHandler Tariff = new TariffHandler();
                result = Tariff.SimulationHeaderInsert(bSimulationHeader);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in inserting Simulation Header";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in inserting Simulation Header"));
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
            try
            {
                //1.Convert Service Entity to Business Entity
                TariffAdapter adapter = new TariffAdapter();
                List<BSimulationSurchargeDiscount> bSimulationSurchargeDiscount = new List<BSimulationSurchargeDiscount>();

                bSimulationSurchargeDiscount = adapter.ConvertStoB_SimulationSurchargeDiscount(sSimulationSurchargeDiscount);

                //2. Create instance for business object and invoke method   
                TariffHandler Tariff = new TariffHandler();
                result = Tariff.SimulationSurchargeDiscount(bSimulationSurchargeDiscount);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in inserting Simulation Surcharge Discount";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in inserting Simulation Surcharge Discount"));
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
            try
            {
                //1.Convert Service Entity to Business Entity
                TariffAdapter adapter = new TariffAdapter();
                List<BSimulationTariff> bSimulationTariff = new List<BSimulationTariff>();

                bSimulationTariff = adapter.ConvertStoB_SimulationTariff(sSimulationTariff);

                //2. Create instance for business object and invoke method   
                TariffHandler Tariff = new TariffHandler();
                result = Tariff.SimulationTariff(bSimulationTariff);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in inserting Simulation Tariff";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in inserting Simulation Tariff"));
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
            try
            {
                //1.Convert Service Entity to Business Entity
                TariffAdapter adapter = new TariffAdapter();
                List<BSimulationTariffBased> bSimulationTariffBased = new List<BSimulationTariffBased>();

                bSimulationTariffBased = adapter.ConvertStoB_SimulationTariffBased(sSimulationTariffBased);

                //2. Create instance for business object and invoke method   
                TariffHandler Tariff = new TariffHandler();
                result = Tariff.SimulationTariffBased(bSimulationTariffBased);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in inserting Simulation Tariff Based";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in inserting Simulation Tariff Based"));
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
            try
            {
                //1.Convert Service Entity to Business Entity
                TariffAdapter adapter = new TariffAdapter();
                BSimulationSubTotal bSimulationSubTotal = new BSimulationSubTotal();

                bSimulationSubTotal = adapter.ConvertStoB_SimulationSubTotal(sSimulationSubTotal);

                //2. Create instance for business object and invoke method   
                TariffHandler Tariff = new TariffHandler();
                result = Tariff.SimulationSubTotal(bSimulationSubTotal);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in inserting Simulation Sub Total";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in inserting Simulation Sub Total"));
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

            try
            {
                //1.Convert Service Entity to Business Entity
                List<BUserID> bUserID = new List<BUserID>();
                UserAdapter adapter = new UserAdapter();

                //2. Create instance for business object and invoke method  
                TariffHandler tariff = new TariffHandler();
                bUserID = tariff.GetAssignedUsers(AccountNo);

                result = adapter.ConvertBtoS_UserID(bUserID);

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in getting User names for Tool";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in  getting User names for Tool"));
            }

            return result;
        }

        /// <summary>
        /// To get the list of Simulation IDs of given Account numbers
        /// </summary>
        /// <param name="AccountNo"></param>
        /// <returns></returns>
        public List<SSimulationList> GetSimulationID(string AccountNo)
        {
            List<SSimulationList> result = new List<SSimulationList>();

            try
            {

                //1.Convert Service Entity to Business Entity
                List<BSimulationList> bSimulationList = new List<BSimulationList>();
                TariffAdapter adapter = new TariffAdapter();

                //2. Create instance for business object and invoke method  
                TariffHandler tariff = new TariffHandler();
                bSimulationList = tariff.GetSimulationID(AccountNo);

                result = adapter.ConvertBtoS_SimulationList(bSimulationList);

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in getting Simulation IDs of selected user for Tool";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in  getting Simulation IDs of selected user for Tool"));
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

            try
            {

                //1.Convert Service Entity to Business Entity
                List<BSimulationTariff> bSimulationTariff = new List<BSimulationTariff>();
                TariffAdapter adapter = new TariffAdapter();

                //2. Create instance for business object and invoke method  
                TariffHandler tariff = new TariffHandler();
                bSimulationTariff = tariff.GetSimulationTariff(SimulationID);

                result = adapter.ConvertBtoS_SimulationTariff(bSimulationTariff);

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in getting Simulation Tariffs of selected user for Tool";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in  getting Simulation Tariffs of selected user for Tool"));
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
            try
            {

                //1.Convert Service Entity to Business Entity
                BSimulationHeader bSimulationHeader = new BSimulationHeader();
                TariffAdapter adapter = new TariffAdapter();

                //2. Create instance for business object and invoke method  
                TariffHandler tariff = new TariffHandler();
                bSimulationHeader = tariff.GetSimulationHeader(SimulationID);

                result = adapter.ConvertBtoS_SimulationHeader(bSimulationHeader);

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in getting Simulation Header of selected user for Tool";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in  getting Simulation Header of selected user for Tool"));
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

            try
            {

                //1.Convert Service Entity to Business Entity
                List<BSimulationSurchargeDiscount> bSimulationSurchargeDiscount = new List<BSimulationSurchargeDiscount>();
                TariffAdapter adapter = new TariffAdapter();

                //2. Create instance for business object and invoke method  
                TariffHandler tariff = new TariffHandler();
                bSimulationSurchargeDiscount = tariff.GetSimulationSurchargeDiscount(SimulationID);

                result = adapter.ConvertBtoS_SimulationSurchargeDiscount(bSimulationSurchargeDiscount);

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in getting Simulation Surcharge Discount of selected user for Tool";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in  getting Simulation Surcharge Discount of selected user for Tool"));
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

            try
            {

                //1.Convert Service Entity to Business Entity
                List<BSimulationTariffBased> bSimulationTariffBased = new List<BSimulationTariffBased>();
                TariffAdapter adapter = new TariffAdapter();

                //2. Create instance for business object and invoke method  
                TariffHandler tariff = new TariffHandler();
                bSimulationTariffBased = tariff.GetSimulationTariffBased(SimulationID);

                result = adapter.ConvertBtoS_SimulationTariffBased(bSimulationTariffBased);

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in getting Simulation Tariff Based of selected user for Tool";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in  getting Simulation Tariff Based of selected user for Tool"));
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

            try
            {

                //1.Convert Service Entity to Business Entity
                BSimulationSubTotal bSimulationSubTotal = new BSimulationSubTotal();
                TariffAdapter adapter = new TariffAdapter();

                //2. Create instance for business object and invoke method  
                TariffHandler tariff = new TariffHandler();
                bSimulationSubTotal = tariff.GetSimulationSubTotal(SimulationID);

                result = adapter.ConvertBtoS_SimulationSubTotal(bSimulationSubTotal);

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in getting Simulation Sub total of selected user for Tool";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in  getting Simulation Sub total of selected user for Tool"));
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
        public List<string> GetDiscount(string AccountNo, List<STariffMaster> sTariffMaster, List<float> Weight, string Origin, string Destination, string MasterServiceName)
        {
            List<string> Discount = new List<string>();
            try
            {
                //1.Convert Service Entity to Business Entity
                List<BTariffMaster> bTariffMaster = new List<BTariffMaster>();

                //2. Create instance for business object and invoke method   
                TariffHandler tariff = new TariffHandler();
                Discount = tariff.GetDiscount(AccountNo, bTariffMaster, Weight, Origin, Destination, MasterServiceName);

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in getting discount for simulation";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in getting discount for simulation"));
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

            try
            {

                //1.Convert Service Entity to Business Entity
                BSurchargeDetails bSurchargeDetails = new BSurchargeDetails();
                TariffAdapter adapter = new TariffAdapter();

                //2. Create instance for business object and invoke method  
                TariffHandler tariff = new TariffHandler();
                bSurchargeDetails = tariff.GetSimulationSurcharge(TariifReference, Origin, Destination, Weight, MasterServiceName);
                result = adapter.ConvertBtoS_SurchargeDetail(bSurchargeDetails);

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in getting Surcharge for selected carrier with Tariff Reference [" + TariifReference + "]for Tool";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in getting Surcharge for selected carrier with Tariff Reference [" + TariifReference + "]for Tool"));
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

            try
            {
                //1.Convert Service Entity to Business Entity

                //2. Create instance for business object and invoke method  
                TariffHandler tariff = new TariffHandler();
                result = tariff.GetFuelSurcharge(TariifReference, Origin, Destination, Weight, MasterServiceName, TariffAmount);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in getting Fuel Surcharge for selected carrier with Tariff Reference [" + TariifReference + "]for Tool";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in getting Fuel Surcharge for selected carrier with Tariff Reference [" + TariifReference + "]for Tool"));
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

            try
            {
                //1.Convert Service Entity to Business Entity

                //2. Create instance for business object and invoke method  
                TariffHandler tariff = new TariffHandler();
                result = tariff.DeleteSimulationID(SimulationID);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in deleting simulation details of [ " + SimulationID + " ]";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in deleting simulation details of [ " + SimulationID + " ]"));
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

            try
            {
                //1.Convert Service Entity to Business Entity
                List<BSimulationSubTotal> bSimulationSubTotal = new List<BSimulationSubTotal>();
                TariffAdapter adapter = new TariffAdapter();

                //2. Create instance for business object and invoke method  
                TariffHandler tariff = new TariffHandler();
                bSimulationSubTotal = tariff.GetSimulationGrossTotal(SimulationID, AccountNo);

                result = adapter.ConvertBtoS_SimulationSubTotal(bSimulationSubTotal);

            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in getting Simulation Gross total of selected user for Tool";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in  getting Simulation Gross total of selected user for Tool"));
            }

            return result;
        }


        /************************** Broker Details 21FEB12KS *****************/
        public string getBrokerEmailId()
        {
            string result = "";
            try
            {
                ShippingHandler shippingHandler = new ShippingHandler();
                result = shippingHandler.getBrokerEmailId();


            }
            catch (Exception error)
            {
                SGeneralFault generalFault = new SGeneralFault();
                generalFault.Issue = "getBrokerEmailId";
                generalFault.Details = Library.ExtractError(error);

                throw new FaultException<SGeneralFault>
                    (generalFault, new FaultReason(generalFault.Details), new FaultCode(generalFault.Issue));
            }
            return result;
        }

        public int UpdateBrokerEmailId(string Email)
        {
            ShippingHandler shippingHandler = new ShippingHandler();
            int result = shippingHandler.UpdateBrokerEmailId(Email);
            return result;
        }

        /*01MAR12HN*/
        /// <summary>
        /// To Assign a calculated tariff using Simulation Tool
        /// </summary>
        /// <param name="SimulationID"></param>
        /// <param name="AccountNo"></param>
        /// <param name="WeightFrom"></param>
        /// <param name="WeightTo"></param>
        /// <param name="AverageWeight"></param>
        /// <param name="ShipCountry"></param>
        /// <param name="DeliveryCountry"></param>
        /// <param name="MasterServiceName"></param>
        /// <param name="TariffRef"></param>
        /// <param name="Margin"></param>
        /// <param name="Carrier"></param>
        /// <param name="Discount"></param>
        /// <returns></returns>
        public int SimulationAssign(List<SSimulationTariff> sSimulationTariff)
        {
            int result = 0;
            try
            {
                //1.Convert Service Entity to Business Entity
                TariffAdapter adapter = new TariffAdapter();
                List<BSimulationTariff> bSimulationTariff = new List<BSimulationTariff>();

                bSimulationTariff = adapter.ConvertStoB_SimulationTariff(sSimulationTariff);

                //2. Create instance for business object and invoke method   
                TariffHandler Tariff = new TariffHandler();
                result = Tariff.SimulationAssign(bSimulationTariff);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem in inserting Assigned tarriff from Simulation tool";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem in inserting Assigned tarriff from Simulation tool"));
            }

            return result;
        }

        /************************* Carrier Manager [14FEB12RM] *******************/
        #region CarrierIntegration

        public SShipmentOrder CarrierProcessing(SShipmentOrder sShipmentOrder, SEnumCarrierProcess sEnumCarrierProcess, out SCarrierProcessResult sCarrierProcessResult)
        {

            try
            {
                //1.Convert Service Entity to Business Entity
                ShippingAdapter adapter = new ShippingAdapter();
                BShipmentOrder bShipmentOrder = adapter.ConvertStoB_ShipmentOrder(sShipmentOrder);
                BCarrierProcessResult bCarrierProcessResult = new BCarrierProcessResult();
                CarrierAdapter adapterCarrier = new CarrierAdapter();


                //2. Create instance for business object and invoke method            

                CarrierHandler carrierHandler = new CarrierHandler();
                if (sEnumCarrierProcess == SEnumCarrierProcess.Feasable)
                {
                    bShipmentOrder = carrierHandler.CarrierProcess(bShipmentOrder, BEnumCarrierProcess.Feasable, out bCarrierProcessResult);

                }
                if (sEnumCarrierProcess == SEnumCarrierProcess.Label)
                {
                    bShipmentOrder = carrierHandler.CarrierProcess(bShipmentOrder, BEnumCarrierProcess.Label, out bCarrierProcessResult);

                }
                sShipmentOrder = adapter.ConvertBtoS_ShipmentOrder(bShipmentOrder);
                sCarrierProcessResult = adapterCarrier.ConvertBtoS_CarrierProcessResult(bCarrierProcessResult);
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [03JAN12RM] */
                SGeneralFault generalFault = new SGeneralFault();
                generalFault.Issue = "CarrierProcessing";
                generalFault.Details = Library.ExtractError(error);

                throw new FaultException<SGeneralFault>
                    (generalFault, new FaultReason(generalFault.Details), new FaultCode(generalFault.Issue));

            }

            return sShipmentOrder;


        }

        public SShipmentOrder GetFeasability(SShipmentOrder sShipmentOrder)
        {
            bool result;
            try
            {
                //1.Convert Service Entity to Business Entity
                ShippingAdapter adapter = new ShippingAdapter();
                BShipmentOrder bShipmentOrder = adapter.ConvertStoB_ShipmentOrder(sShipmentOrder);
                //2. Create instance for business object and invoke method            

                CarrierHandler carrierHandler = new CarrierHandler();
                sShipmentOrder = adapter.ConvertBtoS_ShipmentOrder(bShipmentOrder);
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [03JAN12RM] */
                SGeneralFault generalFault = new SGeneralFault();
                generalFault.Issue = "GetFeasability";
                generalFault.Details = Library.ExtractError(error);

                throw new FaultException<SGeneralFault>
                    (generalFault, new FaultReason(generalFault.Details), new FaultCode(generalFault.Issue));

            }

            return sShipmentOrder;

        }

        public bool CheckForDuplicateCarrierName(string sCarrierName)
        {
            bool result;
            try
            {
                CarrierHandler carrierHandler = new CarrierHandler();
                BCarrierMaster carrierMaster = carrierHandler.GetCarrier(sCarrierName);
                if (carrierMaster == null)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [03JAN12RM] */
                SGeneralFault generalFault = new SGeneralFault();
                generalFault.Issue = "CheckForDuplicateCarrierName";
                generalFault.Details = Library.ExtractError(error);

                throw new FaultException<SGeneralFault>
                    (generalFault, new FaultReason(generalFault.Details), new FaultCode(generalFault.Issue));

            }
            return result;
        }

        public int CreateCarrier(SCarrier sCarrier)
        {
            int result;
            try
            {
                CarrierAdapter adapter = new CarrierAdapter();

                BCarrierMaster carrierMast = adapter.ConvertStoB_Carrier(sCarrier);

                CarrierHandler carrierHandler = new CarrierHandler();

                result = carrierHandler.CreateCarrier(carrierMast);
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [03JAN12RM] */
                SGeneralFault generalFault = new SGeneralFault();
                generalFault.Issue = "CreateCarrier";
                generalFault.Details = Library.ExtractError(error);

                throw new FaultException<SGeneralFault>
                    (generalFault, new FaultReason(generalFault.Details), new FaultCode(generalFault.Issue));

            }
            return result;
        }

        public int UpdateCarrier(SCarrier sCarrier)
        {
            int result;
            try
            {
                CarrierAdapter adapter = new CarrierAdapter();

                BCarrierMaster carrierMast = adapter.ConvertStoB_Carrier(sCarrier);

                CarrierHandler carrierHandler = new CarrierHandler();

                result = carrierHandler.UpdateCarrier(carrierMast);
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [03JAN12RM] */
                SGeneralFault generalFault = new SGeneralFault();
                generalFault.Issue = "UpdateCarrier";
                generalFault.Details = Library.ExtractError(error);

                throw new FaultException<SGeneralFault>
                    (generalFault, new FaultReason(generalFault.Details), new FaultCode(generalFault.Issue));

            }
            return result;
        }

        public SCarrier GetCarrier(string sCarrierName)
        {
            CarrierHandler carrierHandler = new CarrierHandler();
            SCarrier sCarrier = null;
            try
            {

                BCarrierMaster carrierMaster = carrierHandler.GetCarrier(sCarrierName);
                CarrierAdapter carrierAdapter = new CarrierAdapter();
                if (!(carrierMaster == null))
                {
                    sCarrier = carrierAdapter.ConvertBtoS_Carrier(carrierMaster);
                }
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [03JAN12RM] */
                SGeneralFault generalFault = new SGeneralFault();
                generalFault.Issue = "GetCarrier";
                generalFault.Details = Library.ExtractError(error);

                throw new FaultException<SGeneralFault>
                    (generalFault, new FaultReason(generalFault.Details), new FaultCode(generalFault.Issue));
                
            }
            return sCarrier;
        }

        public List<SCarrier> GetCarriers()
        {
            CarrierHandler carrierHandler = new CarrierHandler();
            List<SCarrier> lstCarrier = new List<SCarrier>();

            try
            {

                List<BCarrierMaster> lstcarrierMaster = carrierHandler.GetCarriers();
                CarrierAdapter carrierAdapter = new CarrierAdapter();
                if (!(lstcarrierMaster == null))
                {
                    foreach (BCarrierMaster cm in lstcarrierMaster)
                    {
                        SCarrier sCarrier = new SCarrier();
                        sCarrier = carrierAdapter.ConvertBtoS_Carrier(cm);
                        lstCarrier.Add(sCarrier);
                    }
                }
            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [03JAN12RM] */
                SGeneralFault generalFault = new SGeneralFault();
                generalFault.Issue = "GetCarriers";
                generalFault.Details = Library.ExtractError(error);

                throw new FaultException<SGeneralFault>
                    (generalFault, new FaultReason(generalFault.Details), new FaultCode(generalFault.Issue));

            }
            return lstCarrier;
        }

        #endregion


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

            try
            {
                //1.Convert Service Entity to Business Entity

                //2. Create instance for business object and invoke method  
                ShippingHandler shipping = new ShippingHandler();
                result = shipping.ShipmentDetailUpdate(ShipmentReference, TrackingNumber, ParameterValues);
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while inserting shipment details of [ " + ShipmentReference + " ]";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while inserting shipment details of [ " + ShipmentReference + " ]"));
            }

            return result;
        }

        //to integrate carrier into a single page 29FEB12KS
        public List<SCarrierOutput> GetCarrierOutput(string ShipDetail)
        {

            CarrierHandler carrierHandler = new CarrierHandler();
            List<SCarrierOutput> lstCarrierOutputcollection = new List<SCarrierOutput>();
            try
            {


                List<BCarrierOutput> carrieroutput = carrierHandler.GetCarrierOutput(ShipDetail);

                CarrierAdapter carrierAdapter = new CarrierAdapter();
                if (!(carrieroutput == null))
                {
                    foreach (BCarrierOutput bo in carrieroutput)
                    {
                        SCarrierOutput sCarrierOutput = new SCarrierOutput();
                        sCarrierOutput = carrierAdapter.ConvertBtoS_CarrierOutput(bo);
                        lstCarrierOutputcollection.Add(sCarrierOutput);
                    }
                }
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while reteriving shipment documents";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while reteriving shipment documents"));
            }
            return lstCarrierOutputcollection;
        }


        public List<SCarrierOutput> GetAllCarrierOutput(string EmailID)
        {
            CarrierHandler carrierHandler = new CarrierHandler();
            List<SCarrierOutput> lstCarrierOutputcollection = new List<SCarrierOutput>();
            try
            {
                List<BCarrierOutput> carrieroutput = carrierHandler.GetAllCarrierOutput(EmailID);

                CarrierAdapter carrierAdapter = new CarrierAdapter();
                if (!(carrieroutput == null))
                {
                    foreach (BCarrierOutput bo in carrieroutput)
                    {
                        SCarrierOutput sCarrierOutput = new SCarrierOutput();
                        sCarrierOutput = carrierAdapter.ConvertBtoS_CarrierOutput(bo);
                        lstCarrierOutputcollection.Add(sCarrierOutput);
                    }
                }
            }
            catch (Exception error)
            {
                logger.Debug("From service implementation :" + Library.ExtractError(error));
                var generalFault = new SGeneralFault();
                generalFault.Issue = "Problem while reteriving shipment documents";
                generalFault.Details = Library.ExtractError(error);
                throw new FaultException<SGeneralFault>(generalFault, new FaultReason("Problem while reteriving shipment documents"));
            }
            return lstCarrierOutputcollection;
        }



    }
}
