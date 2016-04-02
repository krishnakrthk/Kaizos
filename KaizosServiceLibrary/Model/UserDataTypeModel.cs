using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace KaizosServiceLibrary.Model
{
    
    public enum SEnumPriority
    {
        /// <summary>
        /// Express service priority
        /// </summary>
        Express=1,//= "EXP",
        /// <summary>
        /// Economty service priority
        /// </summary>
        Economy=2,//= "ECO"
    }

    public enum SEnumFlag
    {
        /// <summary>
        /// Y=&gt; RegularCustomer
        /// </summary>
        Yes = 'Y',
        /// <summary>
        /// N=&gt; ShippingAddress
        /// </summary>
        No = 'N',
    }

    public enum SEnumUserType
    {
        /// <summary>
        /// Admin User {N1}
        /// </summary>
        Administrator=1,//= AD,
        /// <summary>
        /// ShippingAddress User {N2}
        /// </summary>
        Franchise=2,//= FR,
        /// <summary>
        /// Customer Service User
        /// </summary>
        CustomerService=3,// = CS,
        /// <summary>
        /// KeyCustomer User
        /// </summary>
        Referent=4,//= RF,
        /// <summary>
        /// Authorized User
        /// </summary>
        Authorized=5,//= AZ,
    }


    public enum SEnumClaimStatus
    {
        /// <summary>
        /// Claim still opened
        /// </summary>
        Open=1,//= "O",
        /// <summary>
        /// Claim is closed
        /// </summary>
        Closed=2 //= "C",
    }

    public enum SEnumIssueType
    {
        /// <summary>
        /// Missing Issue
        /// </summary>
        Missing=1,//= "M",
        /// <summary>
        /// Damage Issue
        /// </summary>
        Damage=2,//= "D",
        /// <summary>
        /// Loss Issue
        /// </summary>
        Loss=3,//= "L",
    }

    public enum SEnumDeliveryType
    {
        /// <summary>
        /// Address used for delivery
        /// </summary>
        DeliveryAddress=1,//= "D",
        /// <summary>
        /// Address used for shipping
        /// </summary>
        ShippingAddress=2,//= "S",
        /// <summary>
        /// Address used for both delivery and shipping
        /// </summary>
        Both=3,//= "B",
    }

    public enum SEnumOrderType
    {
        /// <summary>
        /// Manual Order Creation
        /// </summary>
        Manual=1,//= "M",
        /// <summary>
        /// Order imported from file
        /// </summary>
        Import=2,//= "I",
    }

    public enum SEnumAddressType
    {
        /// <summary>
        /// Residential address type
        /// </summary>
        Residential=1,//= "R",
        /// <summary>
        /// Company address type
        /// </summary>
        Company=2,//= "C",
    }

    public enum SEnumCustCategory
    {
        /// <summary>
        /// </summary>
        A=1,//= "RegularCustomer",
        /// <summary>
        /// </summary>
        B=2,//= "KeyCustomer",
        C=3,//= "NamedCarrier",
    }

    public enum SEnumTariffType
    {
        /// <summary>
        /// Carrier public tariff
        /// </summary>
        CarrierPublic = 1,//= "Surcharge",
        /// <summary>
        /// System purchase tariff
        /// </summary>
        Purchase = 2,//= "Options",
        /// <summary>
        /// Key customer negotirated tariff
        /// </summary>
        Negotiated = 3,//= "KeyCustomerNegotiated",
    }

    public enum SEnumSurchargeType
    {
        S,
        O,
    }

    public enum SEnumContainerType
    {
        /// <summary>
        /// Carrier public tariff
        /// </summary>
        CarrierPublic=1,//= "Surcharge",
        /// <summary>
        /// System purchase tariff
        /// </summary>
        SystemPurchase=2,//= "Options",
        /// <summary>
        /// Key customer negotirated tariff
        /// </summary>
        KeyCustomerNegotiated=3,//= "KeyCustomerNegotiated",
    }

    public enum SEnumShipPreference
    {
        /// <summary>
        /// Cheapest Carrier
        /// </summary>
        MostCompetitive=1,//= "Competitive",
        /// <summary>
        /// KeyCustomer Carrier
        /// </summary>
        Fastest=2,//= "KeyCustomer",
        /// <summary>
        /// Carrier selected by User
        /// </summary>
        NamedCarrier=3,//= "Named",
    }

    public enum SEnumPaymentType
    {
        /// <summary>
        /// Credit card Payment
        /// </summary>
        CreditCard=1,//= "CC",
        /// <summary>
        /// Deferred Payment
        /// </summary>
        DeferredPayment=2,//= "DP",
    }

    public enum SEnumCustomerType
    {
        /// <summary>
        /// Regular Customer
        /// </summary>
        RegularCustomer=1,//= "R",
        /// <summary>
        /// Key Customer
        /// </summary>
        KeyCustomer=2,//= "K",
    }

    public enum SEnumUserStatus
    {
        /// <summary>
        /// RegularCustomer =&gt; User Archived
        /// </summary>
        Archived=1,//= 'A',
        /// <summary>
        /// KeyCustomer=&gt; User Being Created
        /// </summary>
        BeingCreated=2,//= 'B',
        /// <summary>
        /// D =&gt; User Disabled
        /// </summary>
        Disabled=3,//= 'D',
        /// <summary>
        /// E =&gt; User Enabled
        /// </summary>
        Enabled=4,//= 'E',
    }

    public enum SEnumShipmentType
    {
        /// <summary>
        /// RegularCustomer =&gt; User Archived
        /// </summary>
        Archived=1,//= 'A',
        /// <summary>
        /// KeyCustomer=&gt; User Being Created
        /// </summary>
        BeingCreated=2,//= 'B',
        /// <summary>
        /// D =&gt; User Disabled
        /// </summary>
        Disabled=3,//= 'D',
        /// <summary>
        /// E =&gt; User Enabled
        /// </summary>
        Enabled=4,//= 'E',
    }

    /*************** after 20th dec **********/
    public enum SEnumPublicTariffType
    {
        Import,
        Export,
        Domestic,
    }


}
