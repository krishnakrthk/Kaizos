using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kaizos.Entities.Business; //reference to Business entities
using KaizosServiceLibrary.Model; //reference to Service entities


namespace KaizosServiceLibrary.Adapter
{
    class CarrierAdapter
    {

        public BCarrierService ConvertStoB_CarrierService(SCarrierService sCarrierService)
        {
            BCarrierService bCarrierService = new BCarrierService();

            bCarrierService.ServiceID   = sCarrierService.ServiceID;
            bCarrierService.CarrierName = sCarrierService.CarrierName;
            bCarrierService.MasterServiceName = sCarrierService.MasterServiceName;
            bCarrierService.Priority    = sCarrierService.Priority == SEnumPriority.Economy ? BEnumPriority.Economy : BEnumPriority.Express;
            bCarrierService.ServiceName = sCarrierService.ServiceName;
            bCarrierService.ServiceCode = sCarrierService.ServiceCode;
            bCarrierService.DeliveryDelayTable = sCarrierService.DeliveryDelayTable;
            bCarrierService.DeliveryDeadLine = sCarrierService.DeliveryDeadLine;
            bCarrierService.Active = sCarrierService.Active == SEnumFlag.Yes ? BEnumFlag.Yes : BEnumFlag.No;
            bCarrierService.KeyCustomerService = sCarrierService.KeyCustomerService == SEnumFlag.Yes ? BEnumFlag.Yes : BEnumFlag.No;
            /***********[KS15MAR12]**********/
            bCarrierService.Information=sCarrierService.Information;
            bCarrierService.InfoType=sCarrierService.InfoType;
            bCarrierService.CarrierServiceCode = sCarrierService.CarrierServiceCode;
            return bCarrierService;
        }

        public SCarrierService ConvertBtoS_CarrierService(BCarrierService bCarrierService)
        {
            SCarrierService sCarrierService = new SCarrierService();
            sCarrierService.ServiceID = bCarrierService.ServiceID;
            sCarrierService.CarrierName = bCarrierService.CarrierName;
            sCarrierService.MasterServiceName = bCarrierService.MasterServiceName;
            sCarrierService.Priority = bCarrierService.Priority == BEnumPriority.Economy ? SEnumPriority.Economy : SEnumPriority.Express;
            sCarrierService.ServiceName = bCarrierService.ServiceName;
            sCarrierService.ServiceCode = bCarrierService.ServiceCode;
            sCarrierService.DeliveryDelayTable = bCarrierService.DeliveryDelayTable;
            sCarrierService.DeliveryDeadLine = bCarrierService.DeliveryDeadLine;
            sCarrierService.Active = bCarrierService.Active == BEnumFlag.Yes ? SEnumFlag.Yes : SEnumFlag.No;
            sCarrierService.KeyCustomerService = bCarrierService.KeyCustomerService == BEnumFlag.Yes ? SEnumFlag.Yes : SEnumFlag.No;
            /***********[KS15MAR12]**********/
            sCarrierService.Information = bCarrierService.Information;
            sCarrierService.InfoType = bCarrierService.InfoType;
            sCarrierService.CarrierServiceCode = bCarrierService.CarrierServiceCode;
            return sCarrierService;
        }

        public List<SCarrierService> ConvertBtoS_CarrierService(List<BCarrierService> bCarrierService)
        {
            List<SCarrierService> sCarrierService = new List<SCarrierService>();

            if (bCarrierService != null)
            {
                for (int i = 0; i < bCarrierService.Count; i++)
                {
                    sCarrierService.Add(ConvertBtoS_CarrierService(bCarrierService[i]));
                }
            }

            return sCarrierService;
        }

        public List<BCarrierService> ConvertStoB_CarrierService(List<SCarrierService> sCarrierService)
        {
            List<BCarrierService> bCarrierService = new List<BCarrierService>();

            if (sCarrierService != null)
            {
                for (int i = 0; i < sCarrierService.Count; i++)
                {
                    bCarrierService.Add(ConvertStoB_CarrierService(sCarrierService[i]));
                }
            }

            return bCarrierService;
        }

        public BDeliveryDelay ConvertStoB_DeliveryDelay(SDeliveryDelay sDeliveryDelay)
        {
            BDeliveryDelay bDeliveryDelay = new BDeliveryDelay();
            bDeliveryDelay.Origin       = sDeliveryDelay.Origin.Trim();
            bDeliveryDelay.Destination  = sDeliveryDelay.Destination.Trim();
            bDeliveryDelay.Delay        = sDeliveryDelay.Delay;
            return bDeliveryDelay;
        }

        public SDeliveryDelay ConvertBtoS_DeliveryDelay(BDeliveryDelay bDeliveryDelay)
        {
            SDeliveryDelay sDeliveryDelay = new SDeliveryDelay();
            sDeliveryDelay.Origin       = bDeliveryDelay.Origin.Trim();
            sDeliveryDelay.Destination  = bDeliveryDelay.Destination.Trim();
            sDeliveryDelay.Delay        = bDeliveryDelay.Delay;
            return sDeliveryDelay;
        }

        public List<BDeliveryDelay> ConvertStoB_DeliveryDelay(List<SDeliveryDelay> sDeliveryDelay)
        {
            List<BDeliveryDelay> bDeliveryDelay = new List<BDeliveryDelay>();

            if (sDeliveryDelay != null)
            {
                for (int i = 0; i < sDeliveryDelay.Count; i++)
                {
                    bDeliveryDelay.Add(ConvertStoB_DeliveryDelay(sDeliveryDelay[i]));
                }
            }

            return bDeliveryDelay;
        }

        public List<SDeliveryDelay> ConvertBtoS_DeliveryDelay(List<BDeliveryDelay> bDeliveryDelay)
        {
            List<SDeliveryDelay> sDeliveryDelay = new List<SDeliveryDelay>();

            if (bDeliveryDelay != null)
            {
                for (int i = 0; i < bDeliveryDelay.Count; i++)
                {
                    sDeliveryDelay.Add(ConvertBtoS_DeliveryDelay(bDeliveryDelay[i]));
                }
            }

            return sDeliveryDelay;
        }

        /************* Carrier Manager [14FEB12RM] **************/

        public BCarrierMaster ConvertStoB_Carrier(SCarrier sCarrier)
        {
            BCarrierMaster bCarrier = new BCarrierMaster();
            bCarrier.CarrierID = sCarrier.CarrierID;
            bCarrier.CarrierName = sCarrier.CarrierName;
            bCarrier.ClaimDelay = sCarrier.ClaimDelay;
            bCarrier.Active = sCarrier.Active ? BEnumFlag.Yes : BEnumFlag.No;
            bCarrier.ReferencedCarrier = sCarrier.ReferencedCarrier ? BEnumFlag.Yes : BEnumFlag.No;
            return bCarrier;
        }

        public SCarrier ConvertBtoS_Carrier(BCarrierMaster bCarrier)
        {
            SCarrier sCarrier = new SCarrier();
            sCarrier.CarrierID = bCarrier.CarrierID;
            sCarrier.CarrierName = bCarrier.CarrierName;
            sCarrier.ClaimDelay = bCarrier.ClaimDelay;
            sCarrier.Active = (bCarrier.Active == BEnumFlag.Yes) ? true : false;
            sCarrier.ReferencedCarrier = (bCarrier.ReferencedCarrier == BEnumFlag.Yes) ? true : false;
            return sCarrier;
        }

        public BCarrierProcessResult ConvertStoB_CarrierProcessResult(SCarrierProcessResult sCarrierProcessResult)
        {
            BCarrierProcessResult bCarrierProcessResult = new BCarrierProcessResult();
            bCarrierProcessResult.Carrier = sCarrierProcessResult.Carrier;
            bCarrierProcessResult.Output = sCarrierProcessResult.Output;
            bCarrierProcessResult.Result = sCarrierProcessResult.Result;


            return bCarrierProcessResult;
        }

        public SCarrierProcessResult ConvertBtoS_CarrierProcessResult(BCarrierProcessResult bCarrierProcessResult)
        {
            SCarrierProcessResult sCarrierProcessResult = new SCarrierProcessResult();
            sCarrierProcessResult.Carrier = bCarrierProcessResult.Carrier;
            sCarrierProcessResult.Output = bCarrierProcessResult.Output;
            sCarrierProcessResult.Result = bCarrierProcessResult.Result;


            return sCarrierProcessResult;
        }

        //for carrier output 29FEB12KS
        public BCarrierOutput ConvertStoB_CarrierOutput(SCarrierOutput sCarrierOutput)
        {
            BCarrierOutput bBCarrierOutput = new BCarrierOutput();
            bBCarrierOutput.Carrier = sCarrierOutput.Carrier;
            bBCarrierOutput.CommubicationNumber = sCarrierOutput.CommubicationNumber;
            bBCarrierOutput.Invoice = sCarrierOutput.Invoice;
            bBCarrierOutput.Label = sCarrierOutput.Label;
            bBCarrierOutput.LabelByte = sCarrierOutput.LabelByte;
            bBCarrierOutput.Manifest = sCarrierOutput.Manifest;
            bBCarrierOutput.ShippingReference = sCarrierOutput.ShippingReference;
            return bBCarrierOutput;
        }

        //for carrier output 29FEB12KS
        public SCarrierOutput ConvertBtoS_CarrierOutput(BCarrierOutput bCarrierOutput)
        {
            SCarrierOutput sCarrierOutput = new SCarrierOutput();
            sCarrierOutput.Carrier = bCarrierOutput.Carrier;
            sCarrierOutput.CommubicationNumber = bCarrierOutput.CommubicationNumber;
            sCarrierOutput.Invoice = bCarrierOutput.Invoice;
            sCarrierOutput.Label = bCarrierOutput.Label;
            sCarrierOutput.LabelByte = bCarrierOutput.LabelByte;
            sCarrierOutput.Manifest = bCarrierOutput.Manifest;
            sCarrierOutput.ShippingReference = bCarrierOutput.ShippingReference;
            return sCarrierOutput;
        }


    }
}
