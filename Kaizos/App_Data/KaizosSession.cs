using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using KaizosServiceInvokers.KaizosServiceReference;


/// <summary>
/// Summary description for KaizosSession
/// </summary>
public class KaizosSession
{
    // private constructor
    private KaizosSession() { }

    // Gets the current session.
    public static KaizosSession Current
    {
        get
        {
            KaizosSession session = (KaizosSession)HttpContext.Current.Session["__MySession__"];

            if (session == null)
            {
                session = new KaizosSession();
                HttpContext.Current.Session["__MySession__"] = session;
            }
            return session;
        }
    }

    // *** Generic Sessions

    public string AccountNo { get; set; }
    public string CustomerType { get; set; }
    public string UserId    {get; set; }
    public string SessionID { get; set; }
    
    //** User Management Sessions
    public string UserStatus { get; set; }
    public string UserLanguage { get; set; }
    public string IsSalesTarifAssigned { get; set; }
    public string IsTosAccepted { get; set; }
    public string UserResourceFile { get; set; }
    
    //** User Management - Franchise Sessions
    public string CompanyName { get; set; }
    public string Email { get; set; }
    public string ContactName { get; set; }
    public string Language { get; set; }
    
    //** User Management - Authorized Sessions
    public string AZName { get; set; }
    public string AZEmail { get; set; }
    public string AZPhoneNo { get; set; }
    public string AZAccountNo { get; set; }

    //** User Management - Password Recovery Sessions
    public string PWDEmail { get; set; }

    // *** Shipping Module
    public string ShipReference { get; set; }
    public string SenderCountryCode { get; set; }
    public string SenderCountry { get; set; }
    public string SenderZip { get; set; }
    public string SenderCity { get; set; }
    public string RecipientCountry { get; set; }
    public string RecipientCountryCode { get; set; }
    public string RecipientZip { get; set; }
    public string RecipientCity { get; set; }
    public DateTime ShippingDate { get; set; }
    public int ParcelCount { get; set; }
    public float GrossWeight { get; set; }
    public string UserType { get; set; }
    public SEnumAddressType RecipientAddressType { get; set; }
    public int GroupID { get; set; }
    public List<SShipmentDetails> ShipmentDetail { get; set; }
    public SShipmentOrder ShipmentOrder { get; set; }
    public List<string> ClosedShipments { get; set; }

    public string Options { get; set; }
    /***** Error Forwarding **********/
    public string ErrorMessage  { get; set; }
    public string ReturnURL     { get; set; }

    public string CMCarrierName { get; set; }
    public string CMCarrieID { get; set; }
    public string CMReferencedCarrier { get; set; }
    public string CMActive { get; set; }
    public string CMClaimDelay { get; set; }

    //** User Management - Customer Service
    public string CSCompanyName { get; set; }
    public string CSEmail { get; set; }
    public string CSContactName { get; set; }
    public string CSLanguage { get; set; }

    //*** Zone search ***//
    public string ModeFlag { get; set; }
    public string ZoneID { get; set; }

    //*** Delivery Delay caption ***///
    public string CarrierName { get; set; }
    public string ServiceName { get; set; }
    public string ServiceID { get; set; }

  
    public int shipflag { get; set; }

    //**********Manual shipping by KS*****************************//
    public List<SShipmentQuotation> sshipmentquotation { get; set; }
    public List<SAddressBook> saddressbook { get; set; }
    public int checking { get; set; }

    public int insure_shipping { get; set; }
    public string declared_value { get; set; }
    public int pacakagetype { get; set; }
    public int closingunits { get; set; }
    public int Material_unit { get; set; }
    public int Declared_type { get; set; }
    public int insure_term_accept { get; set; }
    public bool sendermail { get; set; }
    public bool rcivemail { get; set; }
    public bool checkreturn { get; set; }
    public bool accept_term { get; set; }
    public string gender_type { get; set; }


}