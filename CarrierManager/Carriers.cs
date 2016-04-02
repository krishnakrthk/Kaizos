using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Entity;

using Kaizos.Entities.Business;
using Kaizos.Components.Carriers;
using KaizosEntities;

using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Collections;
using System.IO;

namespace Kaizos.Components.CarrierManager
{
  public class CarrierHandler
  {
      public bool isShipmentFeasible(string CarrierServiceName, ShipmentOrder shipmentOrder)
      {
          throw new System.NotImplementedException();
      }

      /// <summary>
      /// Parameter need to be fill
      /// </summary>     
      public string GetCarrierAccountNumberLevel()
      {
          throw new System.NotImplementedException();
      }

      public BShipmentOrder CarrierProcess(BShipmentOrder bShipmentOrder, BEnumCarrierProcess enumCarrierProcess, out BCarrierProcessResult bCarrierProcessResult)
      {
          bCarrierProcessResult = new BCarrierProcessResult();
          BShipmentOrder bsOrder = new BShipmentOrder();
          try
          {
              if (enumCarrierProcess == BEnumCarrierProcess.Feasable)
              {
                  bsOrder = GetFeasibitliy(bShipmentOrder);
              }
              if (enumCarrierProcess == BEnumCarrierProcess.Label)
              {
                  bsOrder = GetLabel(bShipmentOrder, out bCarrierProcessResult);
              }

          }

          catch (Exception ex)
          {
              BShipmentResult result = new BShipmentResult();
              result.isOther = BEnumFlag.Yes;
              result.OtherError = ex.Message;
              result.isFeasibility = BEnumFlag.No;
              result.isLabelGenerated = BEnumFlag.No;
              result.isManifestGenerated = BEnumFlag.No;
              bShipmentOrder.ShipmentResult = result;
              return bShipmentOrder;
          }

          return bsOrder;
      }

      public BShipmentOrder GetFeasibitliy(BShipmentOrder bshipmentOrder)
      {

          //GLS glsCarrier1 = new GLS();

          //return glsCarrier1.GetFeasability(bshipmentOrder);

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var result = context.uSP_GET_CARRIERSPECIFIC_SERVICE(bshipmentOrder.Carrier,"temp").First();
          if (result.ACTUALCODE.Equals("GLS"))
          {
              GLS glsCarrier = new GLS();
              return glsCarrier.GetFeasability(bshipmentOrder);
          }
          else if (result.ACTUALCODE.Equals("TNTINT"))
          {

              TNTINTERNATIONAL tntInter = new TNTINTERNATIONAL();
              BShipmentResult resultDomestic = new BShipmentResult();
              resultDomestic.isOther = BEnumFlag.Yes;
              resultDomestic.OtherError = "";
              resultDomestic.isFeasibility = BEnumFlag.No;
              resultDomestic.FeasibilityError = "Reciver must be other then france ";
              resultDomestic.isLabelGenerated = BEnumFlag.No;
              resultDomestic.isManifestGenerated = BEnumFlag.No;
              bshipmentOrder.ShipmentResult = resultDomestic;
              if (bshipmentOrder.RecipientCountry != "FR")
                  return tntInter.GetFeasibility(bshipmentOrder);
              else
                  return bshipmentOrder;
          }
          else if (result.ACTUALCODE.Equals("TNTDOM"))
          {
              BShipmentResult resultDomestic = new BShipmentResult();
              resultDomestic.isOther = BEnumFlag.Yes;
              resultDomestic.OtherError = "";
              resultDomestic.isFeasibility = BEnumFlag.No;
              resultDomestic.FeasibilityError = "Reciver must be in france ";
              resultDomestic.isLabelGenerated = BEnumFlag.No;
              resultDomestic.isManifestGenerated = BEnumFlag.No;
              bshipmentOrder.ShipmentResult = resultDomestic;

              TNTNATIONAL tntNational = new TNTNATIONAL();
              if (bshipmentOrder.RecipientCountry == "FR")
                  return tntNational.GetFeasibility(bshipmentOrder);
              else
                  return bshipmentOrder;
          }

          return bshipmentOrder;

      }

      private BShipmentOrder GetLabel(BShipmentOrder bshipmentOrder, out BCarrierProcessResult bCarrierProcessResult)
      {
          bCarrierProcessResult = new BCarrierProcessResult();
         
          if (bshipmentOrder.Insured == BEnumFlag.Yes)
             SendInsuranceMail(bshipmentOrder);
          //GLS glsCarrier1 = new GLS();
          //return glsCarrier1.GetLabel(bshipmentOrder, out bCarrierProcessResult); ;

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          var result = context.uSP_GET_CARRIERSPECIFIC_SERVICE(bshipmentOrder.CarrierService,"temp").First();


          if (result.ACTUALCODE.Equals("GLS"))
          {
              GLS glsCarrier = new GLS();
              return glsCarrier.GetLabel(bshipmentOrder, out bCarrierProcessResult);
          }
          else if (result.ACTUALCODE.Equals("TNTINT"))
          {
              TNTINTERNATIONAL tntInter = new TNTINTERNATIONAL();
              return tntInter.GetLabel(bshipmentOrder, out bCarrierProcessResult);
          }
          else if (result.ACTUALCODE.Equals("TNTDOM"))
          {
              TNTNATIONAL tntNational = new TNTNATIONAL();
              return tntNational.GetLabel(bshipmentOrder, out bCarrierProcessResult);
          }


          //if (result.CARRIER_NAME.Equals("GLS"))
          //{
          //    GLS glsCarrier = new GLS();
          //    return glsCarrier.GetLabel(bshipmentOrder, out bCarrierProcessResult);
          //}
          //else if (result.CARRIER_NAME.Equals("TNTINTERNATIONAL"))
          //{
          //    TNTINTERNATIONAL tntInter = new TNTINTERNATIONAL();
          //    return tntInter.GetLabel(bshipmentOrder, out bCarrierProcessResult);
          //}
          //else if (result.CARRIER_NAME.Equals("TNTNATIONAL"))
          //{
          //    TNTNATIONAL tntNational = new TNTNATIONAL();
          //    return tntNational.GetLabel(bshipmentOrder, out bCarrierProcessResult);
          //}
          return bshipmentOrder;

      }

      public BCarriercredentials GetCarrierCredentials(string sCarrierCode)
      {

          BCarriercredentials instance = new BCarriercredentials();
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var result = context.uSP_GET_CARRIER_PARAMETERS(sCarrierCode).ToList();
          foreach (var rec in result)
          {
              if (rec.KEY_CODE.Equals("UserName"))
              {
                  instance.UserName = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("PassWord"))
              {
                  instance.PassWord = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("AppIdFeasability"))
              {
                  instance.AppIdFeasability = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("CountryVersion"))
              {
                  instance.CountryVersion = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("CurrencyVersion"))
              {
                  instance.CurrencyVersion = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("PostcodeMaskVersion"))
              {
                  instance.PostcodeMaskVersion = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("ServiceVersion"))
              {
                  instance.ServiceVersion = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("OptionVersion"))
              {
                  instance.OptionVersion = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("RateID"))
              {
                  instance.RateID = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("OriginalTownGroup"))
              {
                  instance.OriginalTownGroup = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("DestTownGroup"))
              {
                  instance.DestTownGroup = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("AccountNO"))
              {
                  instance.AccountNO = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("ServiceType"))
              {
                  instance.ServiceType = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("AppIdLabel"))
              {
                  instance.AppIdLabel = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("Appverersion"))
              {
                  instance.Appverersion = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("GroupCode"))
              {
                  instance.GroupCode = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("Providance"))
              {
                  instance.Providance = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("VATSender"))
              {
                  instance.VATSender = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("VATReciver"))
              {
                  instance.VATReciver = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("PaymentId"))
              {
                  instance.PaymentId = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("Deliveryinstruction"))
              {
                  instance.Deliveryinstruction = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("Description"))
              {
                  instance.Description = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("TrackingType"))
              {
                  instance.TrackingType = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("ExtratrackingDetail1"))
              {
                  instance.ExtratrackingDetail1 = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("ExtratrackingDetail2"))
              {
                  instance.ExtratrackingDetail2 = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("ExtratrackingDetail3"))
              {
                  instance.ExtratrackingDetail3 = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("ExtratrackingDetail4"))
              {
                  instance.ExtratrackingDetail4 = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("ExtratrackingDetail5"))
              {
                  instance.ExtratrackingDetail5 = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("ExtratrackingDetail6"))
              {
                  instance.ExtratrackingDetail6 = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("ContactID"))
              {
                  instance.ContactID = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("CustomerID"))
              {
                  instance.CustomerID = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("CountryISOCode"))
              {
                  instance.CountryISOCode = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("ProductCode"))
              {
                  instance.ProductCode = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("ConsigneeRef"))
              {
                  instance.ConsigneeRef = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("CustomerRefNumber"))
              {
                  instance.CustomerRefNumber = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("ServiceandAdditional"))
              {
                  instance.ServiceandAdditional = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("ServiceInformation"))
              {
                  instance.ServiceInformation = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("T852"))
              {
                  instance.T852 = rec.KEY_VALUE;
              }
              if (rec.KEY_CODE.Equals("GLSoutboundDepot"))
              {
                  instance.GLSoutboundDepot = rec.KEY_VALUE;
              }



          }

          return instance;

      }

      public int CreateCarrier(BCarrierMaster carrier)
      {
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          System.Nullable<int> result = context.uSP_CARRIER_MAST_INSERT(carrier.CarrierName, carrier.ReferencedCarrier.ToString(), carrier.Active.ToString(), carrier.ClaimDelay, "N/A").First();
          return (int)result; 
      }

      public int UpdateCarrier(BCarrierMaster carrier)
      {
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          var result = context.uSP_CARRIER_MAST_UPDATE(carrier.CarrierName, carrier.ReferencedCarrier.ToString(), carrier.Active.ToString(), carrier.ClaimDelay, "N/A", carrier.CarrierID).First(); 
          return (int)result;
      }

      public List<BCarrierMaster> GetCarriers()
      {
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
         var result = context.uSP_GET_CARRIER_MAST(null).DefaultIfEmpty().ToList() ;
           List<BCarrierMaster> lstCarrierMast =null;
           if (!(result == null))
           {
               lstCarrierMast = new List<BCarrierMaster>();
               BCarrierMaster carrierMaster;
               foreach (var rec in result)
               {
                   carrierMaster = new BCarrierMaster();
                   carrierMaster.CarrierID = rec.ID;
                   carrierMaster.CarrierName = rec.CARRIER_NAME;
                   carrierMaster.ClaimDelay = rec.CLAIM_DELAY;
                   carrierMaster.Active = rec.ACTIVE.Equals("Y") ? BEnumFlag.Yes : BEnumFlag.No;
                   carrierMaster.ReferencedCarrier = rec.REFERENCED_CARRIER.Equals("Y") ? BEnumFlag.Yes : BEnumFlag.No;
                   lstCarrierMast.Add(carrierMaster);
               }
           }
          return lstCarrierMast;

 
      }

      public List<BCarrierMaster> GetReferencedCarriers()
      {
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          var result = context.uSP_GET_CARRIER_MAST("RF").DefaultIfEmpty().ToList();
          List<BCarrierMaster> lstCarrierMast = null;
          if (!(result == null))
          {
              lstCarrierMast = new List<BCarrierMaster>();
              BCarrierMaster carrierMaster;
              foreach (var rec in result)
              {
                  carrierMaster = new BCarrierMaster();
                  carrierMaster.CarrierID = rec.ID;
                  carrierMaster.CarrierName = rec.CARRIER_NAME;
                  carrierMaster.ClaimDelay = rec.CLAIM_DELAY;
                  carrierMaster.Active = rec.ACTIVE.Equals("Y") ? BEnumFlag.Yes : BEnumFlag.No;
                  carrierMaster.ReferencedCarrier = rec.REFERENCED_CARRIER.Equals("Y") ? BEnumFlag.Yes : BEnumFlag.No;
                  lstCarrierMast.Add(carrierMaster);
              }
          }
          return lstCarrierMast;


      }

      public BCarrierMaster GetCarrier(string sCarrierName)
      {
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          var result = context.uSP_GET_CARRIER_MAST(sCarrierName).DefaultIfEmpty().First();
          
          BCarrierMaster carrierMaster=null;
          if (!(result == null))
          {
              carrierMaster = new BCarrierMaster();
              carrierMaster.CarrierID = result.ID;
              carrierMaster.CarrierName = result.CARRIER_NAME;
              carrierMaster.ClaimDelay = result.CLAIM_DELAY;
              carrierMaster.Active = result.ACTIVE.Equals("Y") ? BEnumFlag.Yes : BEnumFlag.No;
              carrierMaster.ReferencedCarrier = result.REFERENCED_CARRIER.Equals("Y") ? BEnumFlag.Yes : BEnumFlag.No;

          }
              return carrierMaster;


      }

      //to integrate carrier into a single page 29FEB12KS
      public List<BCarrierOutput> GetCarrierOutput(string ShipDetail)
      {
          //string ShipDetail = "1189!1190!1191";

          List<BCarrierOutput> CarrierOutputcollection = new List<BCarrierOutput>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          string[] Sp = ShipDetail.Split(new Char[] { '!' });
          for (int i = 0; i <= Sp.Length - 1; i++)
          {
              BCarrierOutput bCarrierOutput = new BCarrierOutput();
              var Result = context.uSP_GET_CARRIER_LABEL_MANIFEST(Sp[i]).DefaultIfEmpty().First();
              bCarrierOutput.Carrier = Result.CARRIER;
              bCarrierOutput.CommubicationNumber = Result.COMMUNICATIONNUMBER;
              bCarrierOutput.Invoice = Result.INVOICE;
              bCarrierOutput.Label = Result.LABEL;
              bCarrierOutput.LabelByte = Result.LABELBYTE;
              bCarrierOutput.Manifest = Result.MANIFEST;
              bCarrierOutput.ShippingReference = Result.SHIPPINGREFERENCE;
              CarrierOutputcollection.Add(bCarrierOutput);
          }

          return CarrierOutputcollection;
      }

      public List<BCarrierOutput> GetAllCarrierOutput(string EmailID)
      {
          //string ShipDetail = "1189!1190!1191";

          List<BCarrierOutput> lstCarrierOutputcollection = new List<BCarrierOutput>();

          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          string strShippingReference = string.Empty;

          BCarrierOutput bCarrierOutput = new BCarrierOutput();
          var result = context.uSP_GET_ALL_CARRIER_LABEL_MANIFEST(EmailID).DefaultIfEmpty().ToList();
          if (!(result == null))
          {
              foreach (var obj in result)
              {
                  bCarrierOutput = new BCarrierOutput();
                  bCarrierOutput.Carrier = obj.CARRIER;
                  bCarrierOutput.CommubicationNumber = obj.COMMUNICATIONNUMBER;
                  bCarrierOutput.Invoice = obj.INVOICE;
                  bCarrierOutput.Label = obj.LABEL;
                  bCarrierOutput.LabelByte = obj.LABELBYTE;
                  bCarrierOutput.Manifest = obj.MANIFEST;
                  bCarrierOutput.ShippingReference = obj.SHIPPINGREFERENCE;
                  lstCarrierOutputcollection.Add(bCarrierOutput);

              }
          }
          return lstCarrierOutputcollection;
      }

      // To Include insurance broker mail to end customer by 09MAR12KS
      public void SendInsuranceMail(BShipmentOrder bShipmentOrder)
      {
          string content = "";
          foreach (BShipmentDetails bd in bShipmentOrder.ShipDetail)
          {
              content = content + bd.ContentType + ",";
          }
          content = content.Substring(0, content.Length - 1);
          string[] contentarr = content.Split(',');
          contentarr = contentarr.Distinct().ToArray();
          string strcontent = "";
          for (int i = 0; i <= contentarr.Count() - 1; i++)
          {
              strcontent = strcontent + contentarr[i] + ",";

          }

          strcontent = strcontent.Substring(0, strcontent.Length - 1);
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          var result1 = context.uSP_GET_CARRIERSPECIFIC_SERVICE(bShipmentOrder.Carrier,"temp").First();
          string carriersend = result1.ACTUALCODE;
          
          if (result1.ACTUALCODE == "TNTINT")
              carriersend = "TNT INT";
          else if (result1.ACTUALCODE == "TNTDOM")
              carriersend = "TNT";


          var rr = context.uSP_GET_INSURANCE_RATE(bShipmentOrder.AccountNo, bShipmentOrder.RecipientCountry, bShipmentOrder.Carrier, carriersend).First();
          
          
          float rate =(float)rr.RATE;




          //bShipmentOrder.ContainerType = "content";
          string Mailsubject = "Dde Navilib ID : " + bShipmentOrder.ShipReference + "- Certificat ADVALO";
          string Mailcontent = "\n\t" + strcontent + "\n\t" + bShipmentOrder.RecipientCountry + "\n\t" + bShipmentOrder.SenderEmail;
          float ff = (rate * bShipmentOrder.DeclaredValue) / 100;
          if (ff < 5.0)
          {
              if (rr.MIN_CHARGE > 0)
                  ff = (float)rr.MIN_CHARGE;
          }
          string attchedcsv = "NumGarantie;DateGarantie;DateModif;IdClient;RaisonSocialeClient;CodePoliceClient;DateChargement;DateLivraison;ModeTransport;ZoneTransport;CategorieMarchandise;CommentairesMarchandises;PoidsMarchandises;MarquesMarchandises;ConditionsGeneralesMarchandises;ValeurMarchandises;Taux;Prime;LibStatut;nb_conteneurs;AssureNom;AssureAdresse;Assurecp;Assureville;Assurepays;AssureTel;Assurefax;Assuremel";
          string completedattach = attchedcsv + "\n" + bShipmentOrder.ShipReference + ";" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ";" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ";" + "59296;NAVILIB SAS (Global freight);82800316;" + bShipmentOrder.ShipDateTime + ";;" + "Aérien;France;" + strcontent + ";" + strcontent + ";" + bShipmentOrder.TotalWeight + ";;Tous risques;" + bShipmentOrder.DeclaredValue.ToString() + ";";
          completedattach = completedattach + rate.ToString() + ";" + ff.ToString() + ";Validée;;" + bShipmentOrder.SenderName + ";" + bShipmentOrder.SenderAddress1 + bShipmentOrder.SenderAddress2 + bShipmentOrder.SenderAddress3 + ";" + bShipmentOrder.SenderZipCode + ";" + bShipmentOrder.SenderCity + ";" + bShipmentOrder.SenderCountry + ";" + bShipmentOrder.SenderPhone + ";;" + bShipmentOrder.SenderEmail + ";";
          MemoryStream mm;
          mm = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(completedattach));
          string result = "";

          result = context.uSP_GET_INSURANCE_BROKER().SingleOrDefault();



          MailMessage message = new MailMessage();
          MailAddress fromAddress = new MailAddress("KAIZOS_SUPPORT@KAIZOS.COM");
          message.From = fromAddress;

          message.Subject = Mailsubject;
          message.IsBodyHtml = false;

          message.Body = Mailcontent;

          message.To.Add(result);

          Attachment data = new Attachment(mm, MediaTypeNames.Text.Plain);
          ContentDisposition disposition = data.ContentDisposition;

          disposition.FileName = "insurance.csv";
          message.Attachments.Add(data);

          SmtpClient client = new SmtpClient();
          client.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;

          client.Host = "localhost";
          client.Send(message);

      }




  }

 
}
