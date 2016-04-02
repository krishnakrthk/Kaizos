using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kaizos.Entities.Business;
using KaizosServiceLibrary.Model;

namespace KaizosServiceLibrary.Adapter
{
    class CommonAdapter
    {
        /// <summary>
        /// Convert Service Entity To Business Entity - Dimension Unit
        /// </summary>
        /// <param name="sCountryTable"></param>
        /// <returns></returns>
        public BComboText ConvertStoB_ComboText(SComboText sComboText)
        {
            BComboText bComboText = new BComboText();
            bComboText.ComboText = sComboText.ComboText.Trim();
            return bComboText;
        }

        /// <summary>
        /// Convert Business Entity To Service Entity - Combo box text
        /// </summary>
        /// <param name="bCountryTable"></param>
        /// <returns></returns>
        public SComboText ConvertBtoS_ComboText(BComboText bComboText)
        {
            SComboText sComboText = new SComboText();
            sComboText.ComboText = bComboText.ComboText.Trim();
            return sComboText;
        }

        public List<BComboText> ConvertStoB_ComboText(List<SComboText> sComboText)
        {
            List<BComboText> bComboText = new List<BComboText>();
            for (int i = 0; i < sComboText.Count; i++)
            {
                bComboText.Add(ConvertStoB_ComboText(sComboText[i]));
            }
            return bComboText;
        }

        public List<SComboText> ConvertBtoS_ComboText(List<BComboText> bComboText)
        {
            List<SComboText> sComboText = new List<SComboText>();
            for (int i = 0; i < bComboText.Count; i++)
            {
                sComboText.Add(ConvertBtoS_ComboText(bComboText[i]));
            }
            return sComboText;
        }


        /// <summary>
        /// Convert Service Entity To Business Entity - Generic Module
        /// </summary>
        /// <param name="sCountryTable"></param>
        /// <returns></returns>
        public BComboTableField ConvertStoB_ComboTableField(SComboTableField sComboTableField)
        {
            BComboTableField bComboTableField = new BComboTableField();
            bComboTableField.FieldName = sComboTableField.FieldName;
            bComboTableField.TableName = sComboTableField.TableName;
            return bComboTableField;
        }

        /// <summary>
        /// Convert Business Entity To Service Entity - Combo box text
        /// </summary>
        /// <param name="bCountryTable"></param>
        /// <returns></returns>
        public SComboTableField ConvertBtoS_ComboTableField(BComboTableField bComboTableField)
        {
            SComboTableField sComboTableField = new SComboTableField();
            sComboTableField.FieldName = bComboTableField.FieldName;
            sComboTableField.TableName = bComboTableField.TableName;
            return sComboTableField;
        }

        public BAddressBook ConvertSEAddresstoBEAddress(SAddressBook sAddressBook)
        {
            BAddressBook bAddressBook = new BAddressBook();
            
            bAddressBook.AddressID = sAddressBook.AddressID;

            if (sAddressBook.AddressType == SEnumAddressType.Company)
                bAddressBook.AddressType = BEnumAddressType.Company;
            else
                bAddressBook.AddressType = BEnumAddressType.Residential;

            bAddressBook.CompanyName = sAddressBook.CompanyName;
            bAddressBook.Name = sAddressBook.Name;
            bAddressBook.TelephoneNo = sAddressBook.TelephoneNo;
            bAddressBook.FaxNo = sAddressBook.FaxNo;
            bAddressBook.Address1 = sAddressBook.Address1;
            bAddressBook.Address2 = sAddressBook.Address2;
            bAddressBook.Address3 = sAddressBook.Address3;
            bAddressBook.ZipCode = sAddressBook.ZipCode;
            bAddressBook.City = sAddressBook.City;
            bAddressBook.State = sAddressBook.State;
            bAddressBook.Country = sAddressBook.Country;
            bAddressBook.Email = sAddressBook.Email;
            bAddressBook.LastPickupMondayToThursday = sAddressBook.LastPickupMondayToThursday;
            bAddressBook.LastPickupFriday = sAddressBook.LastPickupFriday;
            bAddressBook.Comments = sAddressBook.Comments;
            if (sAddressBook.ShipPreference == SEnumShipPreference.Fastest)
                bAddressBook.ShipPreference = BEnumShipPreference.Fastest;
            else if (sAddressBook.ShipPreference == SEnumShipPreference.MostCompetitive)
                bAddressBook.ShipPreference = BEnumShipPreference.MostCompetitive;
            else
                bAddressBook.ShipPreference = BEnumShipPreference.NamedCarrier;
            if (sAddressBook.AddressUsedFor == SEnumDeliveryType.DeliveryAddress)
                bAddressBook.AddressUsedFor = BEnumDeliveryType.DeliveryAddress;
            else if (sAddressBook.AddressUsedFor == SEnumDeliveryType.ShippingAddress)
                bAddressBook.AddressUsedFor = BEnumDeliveryType.ShippingAddress;
            else
                bAddressBook.AddressUsedFor = BEnumDeliveryType.Both;

            bAddressBook.CreatedDate = sAddressBook.CreatedDate;
            bAddressBook.LastUpdated = sAddressBook.LastUpdated;
            bAddressBook.AccountNo = sAddressBook.AccountNo;
            bAddressBook.ShipPreferenceOrder = sAddressBook.ShipPreferenceOrder;
            bAddressBook.NamedCarrier = sAddressBook.NamedCarrier;
            return bAddressBook;
        }

        public SAddressBook ConvertBEAddresstoSEAddress(BAddressBook bAddressBook)
        {
            SAddressBook sAddressBook = new SAddressBook();

            sAddressBook.AddressID = bAddressBook.AddressID;

            if (bAddressBook.AddressType == BEnumAddressType.Company)
                sAddressBook.AddressType = SEnumAddressType.Company;
            else
                sAddressBook.AddressType = SEnumAddressType.Residential;

            sAddressBook.CompanyName = bAddressBook.CompanyName;
            sAddressBook.Name = bAddressBook.Name;
            sAddressBook.TelephoneNo = bAddressBook.TelephoneNo;
            sAddressBook.FaxNo = bAddressBook.FaxNo;
            sAddressBook.Address1 = bAddressBook.Address1;
            sAddressBook.Address2 = bAddressBook.Address2;
            sAddressBook.Address3 = bAddressBook.Address3;
            sAddressBook.ZipCode = bAddressBook.ZipCode;
            sAddressBook.City = bAddressBook.City;
            sAddressBook.State = bAddressBook.State;
            sAddressBook.Country = bAddressBook.Country;
            sAddressBook.Email = bAddressBook.Email;
            sAddressBook.LastPickupMondayToThursday = bAddressBook.LastPickupMondayToThursday;
            sAddressBook.LastPickupFriday = bAddressBook.LastPickupFriday;
            sAddressBook.Comments = bAddressBook.Comments;
            if (bAddressBook.ShipPreference == BEnumShipPreference.Fastest)
                sAddressBook.ShipPreference = SEnumShipPreference.Fastest;
            else if (bAddressBook.ShipPreference == BEnumShipPreference.MostCompetitive)
                sAddressBook.ShipPreference = SEnumShipPreference.MostCompetitive;
            else
                sAddressBook.ShipPreference = SEnumShipPreference.NamedCarrier;
            if (bAddressBook.AddressUsedFor == BEnumDeliveryType.DeliveryAddress)
                sAddressBook.AddressUsedFor = SEnumDeliveryType.DeliveryAddress;
            else if (bAddressBook.AddressUsedFor == BEnumDeliveryType.ShippingAddress)
                sAddressBook.AddressUsedFor = SEnumDeliveryType.ShippingAddress;
            else
                sAddressBook.AddressUsedFor = SEnumDeliveryType.Both;

            sAddressBook.CreatedDate = bAddressBook.CreatedDate;
            sAddressBook.LastUpdated = bAddressBook.LastUpdated;
            sAddressBook.AccountNo = bAddressBook.AccountNo;
            sAddressBook.ShipPreferenceOrder = bAddressBook.ShipPreferenceOrder;
            sAddressBook.NamedCarrier = bAddressBook.NamedCarrier;
            return sAddressBook;
        }

        /// <summary>
        /// Convert Business Entity To Service Entity - Tracking number
        /// </summary>
        /// <param name="bCountryTable"></param>
        /// <returns></returns>
        public SNextcounter ConvertBtoS_Nextcounter(BNextcounter bNextcounter)
        {
            SNextcounter sNextcounter = new SNextcounter();
            sNextcounter.ErrorCode = bNextcounter.ErrorCode;
            sNextcounter.ErrorDescription = bNextcounter.ErrorDescription;
            sNextcounter.NextCounter = bNextcounter.NextCounter;
            return sNextcounter;
        }

        /// <summary>
        /// Convert Service Entity To Business Entity - Couontry Table
        /// </summary>
        /// <param name="sCountryTable"></param>
        /// <returns></returns>
        public BCountryTable ConvertStoB_CountryTable(SCountryTable sCountryTable)
        {
            BCountryTable bCountryTable = new BCountryTable();
            bCountryTable.CountryCode = sCountryTable.CountryCode;
            bCountryTable.CountryName = sCountryTable.CountryName;
            bCountryTable.CodeName = sCountryTable.CodeName;
            return bCountryTable;
        }

        /// <summary>
        /// Convert Business Entity To Service Entity - Couontry Table
        /// </summary>
        /// <param name="bCountryTable"></param>
        /// <returns></returns>
        public SCountryTable ConvertBtoS_CountryTable(BCountryTable bCountryTable)
        {
            SCountryTable sCountryTable = new SCountryTable();
            sCountryTable.CountryCode = bCountryTable.CountryCode;
            sCountryTable.CountryName = bCountryTable.CountryName;
            sCountryTable.CodeName = bCountryTable.CodeName;
            return sCountryTable;
        }

        public List<BCountryTable> ConvertStoB_CountryTable(List<SCountryTable> sCountryTable)
        {
            List<BCountryTable> bCountryTable = new List<BCountryTable>();
            for (int i = 0; i < sCountryTable.Count; i++)
            {
                bCountryTable.Add(ConvertStoB_CountryTable(sCountryTable[i]));
            }
            return bCountryTable;
        }

        public List<SCountryTable> ConvertBtoS_CountryTable(List<BCountryTable> bCountryTable)
        {
            List<SCountryTable> sCountryTable = new List<SCountryTable>();
            for (int i = 0; i < bCountryTable.Count; i++)
            {
                sCountryTable.Add(ConvertBtoS_CountryTable(bCountryTable[i]));
            }
            return sCountryTable;
        }

        /// <summary>
        /// Convert Service Entity To Business Entity - AddressBook list
        /// </summary>
        /// <param name="sAddressBook"></param>
        /// <returns></returns>

        public List<SAddressBook> ConvertBtoS_AddressBookList(List<BAddressBook> bAddressList)
        {
            List<SAddressBook> sAddressList = new List<SAddressBook>();
            for (int i = 0; i < bAddressList.Count; i++)
            {
                sAddressList.Add(ConvertBEAddresstoSEAddress(bAddressList[i]));
            }
            return sAddressList;
        }

        /// <summary>
        /// To convert Business to Service Entity for Industry
        /// </summary>
        /// <param name="bIndustry"></param>
        /// <returns></returns>
        public SIndustry ConvertBtoS_Industry(BIndustry bIndustry)
        {
            SIndustry sIndustry = new SIndustry();
            sIndustry.Department = bIndustry.Department;
            sIndustry.Activity = bIndustry.Activity;
            return sIndustry;
        }

        /// <summary>
        /// To convert bussiness to Service entiry for Industry list
        /// </summary>
        /// <param name="bIndustryList"></param>
        /// <returns></returns>
        public List<SIndustry> ConvertBtoS_Industry(List<BIndustry> bIndustryList)
        {
            List<SIndustry> sIndustryList = new List<SIndustry>();
            for (int i = 0; i < bIndustryList.Count; i++)
            {
                sIndustryList.Add(ConvertBtoS_Industry(bIndustryList[i]));
            }
            return sIndustryList;
        }

        /// <summary>
        /// To convert Service to Business - Keyvalue
        /// </summary>
        /// <param name="sKeyValue"></param>
        /// <returns></returns>
        public BKeyValue ConvertStoB_KeyValue(SKeyValue sKeyValue)
        {
            BKeyValue bKeyValue = new BKeyValue();
            bKeyValue.ErrorCode = sKeyValue.ErrorCode;
            bKeyValue.ErrorDescription = sKeyValue.ErrorDescription;
            bKeyValue.IsValid = sKeyValue.IsValid;
            bKeyValue.Value = sKeyValue.Value;
            return bKeyValue;
        }

        /// <summary>
        /// To convert Service to Business - Keyvalue
        /// </summary>
        /// <param name="sKeyValue"></param>
        /// <returns></returns>
        public SKeyValue ConvertBtoS_KeyValue(BKeyValue bKeyValue)
        {
            SKeyValue sKeyValue = new SKeyValue();
            sKeyValue.ErrorCode = bKeyValue.ErrorCode;
            sKeyValue.ErrorDescription = bKeyValue.ErrorDescription;
            sKeyValue.IsValid = bKeyValue.IsValid;
            sKeyValue.Value = bKeyValue.Value;
            return sKeyValue;
        }


    }
}
