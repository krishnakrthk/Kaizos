using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kaizos.Entities.Business;
using KaizosEntities;

namespace Kaizos.Components.AddressBookManager
{
  public class AddressBookHandler
  {
      /// <summary>
      /// Insert new address in Address Book
      /// </summary>
      /// <returns>
      /// 0 =&gt; Sucess
      /// 1 =&gt; Invalid data type
      /// 2 =&gt; Already exist
      /// </returns>
      public int InsertAddres(BAddressBook Address)
      {
          
          throw new System.NotImplementedException();
      }

      /// <summary>
      /// Get address by matching given string with Company Name or Name
      /// </summary>
      /// <returns>List of address book</returns>
      public List<BAddressBook> GetAddress(string Name)
      {
          throw new System.NotImplementedException();
      }

      /// <summary>
      /// To get matching address for the strinb comparing
      /// Company Name and Name
      /// </summary>
      /// <param name="SearchString"></param>
      /// <param name="UsedFor"></param>
      /// <returns></returns>
      public List<BAddressBook> GetAddressBookSearch(string SearchString, string UsedFor)
      {
          List<BAddressBook> bAddressBook = new List<BAddressBook>();
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var Address = context.uSP_GET_ADDRESS_BOOK_SEARCH(SearchString, UsedFor).ToList();

          foreach (var line in Address)
          {
              BAddressBook a = new BAddressBook();

              a.AccountNo = "";
              a.AddressID = line.ADDRESS_BOOK_ID;
              if (line.ADDRESS_TYPE == "C")
                  a.AddressType = BEnumAddressType.Company;
              else
                  a.AddressType = BEnumAddressType.Residential;

              a.Address1 = line.ADDRESS1;
              a.Address2 = line.ADDRESS2;
              a.Address3 = line.ADDRESS3;
              a.City = line.CITY;
              a.Comments = line.COMMENTS;
              a.CompanyName = line.COMPANY_NAME;
              a.Name = line.CONTACT_NAME;
              a.Email = line.EMAIL;
              a.FaxNo = line.FAX_NO;
              a.LastPickupFriday = line.LAST_PICKUP_FRI;
              a.LastPickupMondayToThursday = line.LAST_PICKUP_MON_THU;
              if (line.SHIP_PREFERENCE.Trim() == "MostCompetitive")
                  a.ShipPreference = BEnumShipPreference.MostCompetitive;
              else if (line.SHIP_PREFERENCE.Trim() == "NamedCarrier")
                  a.ShipPreference = BEnumShipPreference.NamedCarrier;
              else
                  a.ShipPreference = BEnumShipPreference.Fastest;

              a.State = line.STATE;
              a.TelephoneNo = line.TEL_NO;
              a.ZipCode = line.ZIPCODE;
              a.Country = line.COUNTRY;
              a.NamedCarrier = line.NAMEDCARRIER;
              bAddressBook.Add(a);
          }


          return bAddressBook;
      }

      /// <summary>
      /// Update exisitng Address
      /// </summary>
      /// <returns>
      /// 0 =&gt; Sucessfull update
      /// 1 =&gt; Failure
      /// </returns>
      public int UpdateAddress(BAddressBook bAddressBook)
      {
          string AddressType = "";
          string ShipPreference = "";

          if (bAddressBook.AddressType == BEnumAddressType.Company)
              AddressType = "C";
          else
              AddressType = "R";

          if (bAddressBook.ShipPreference == BEnumShipPreference.Fastest)
              ShipPreference = "Fastest";
          else if (bAddressBook.ShipPreference == BEnumShipPreference.MostCompetitive)
              ShipPreference = "MostCompetitive";
          else
              ShipPreference = "NamedCarrier";

          ShipPreference = bAddressBook.ShipPreferenceOrder;
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          System.Nullable<int> iReturnValue = context.uSP_ADDRESS_BOOK_UPDATE(Convert.ToInt32(bAddressBook.AddressID), AddressType.Trim(),
                                                                                bAddressBook.CompanyName.Trim(), bAddressBook.Name.Trim(),
                                                                                bAddressBook.TelephoneNo.Trim(), bAddressBook.FaxNo.Trim(),
                                                                                bAddressBook.Address1.Trim(), bAddressBook.Address2.Trim(),
                                                                                bAddressBook.Address3.Trim(), bAddressBook.ZipCode.Trim(),
                                                                                bAddressBook.City.Trim(), bAddressBook.State.Trim(),
                                                                                bAddressBook.Country.Trim(), bAddressBook.Email.Trim(),
                                                                                bAddressBook.LastPickupMondayToThursday.Trim(),
                                                                                bAddressBook.LastPickupFriday.Trim(),
                                                                                bAddressBook.Comments.Trim(), ShipPreference.Trim(),
                                                                                bAddressBook.LastUpdated,
                                                                                bAddressBook.AccountNo.Trim(),bAddressBook.NamedCarrier).SingleOrDefault();




          return ((int)iReturnValue);
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
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
          System.Nullable<int> iReturnValue = context.uSP_ADDRESS_BOOK_DELETE(Convert.ToInt32(AddressID.Trim())).SingleOrDefault();
          return ((int)iReturnValue);
      }

      /**********24APR2012KM***************/
      public List<BAddressBook> GetAddressnew(string SearchString, string UsedFor, string zcode, string scountry)
      {
          List<BAddressBook> bAddressBook = new List<BAddressBook>();
          KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

          var Address = context.uSP_GET_ADDRESS(SearchString, UsedFor, zcode, scountry).ToList();

          foreach (var line in Address)
          {
              BAddressBook a = new BAddressBook();

              a.AccountNo = "";
              a.AddressID = line.ADDRESS_BOOK_ID;
              if (line.ADDRESS_TYPE == "C")
                  a.AddressType = BEnumAddressType.Company;
              else
                  a.AddressType = BEnumAddressType.Residential;

              a.Address1 = line.ADDRESS1;
              a.Address2 = line.ADDRESS2;
              a.Address3 = line.ADDRESS3;
              a.City = line.CITY;
              a.Comments = line.COMMENTS;
              a.CompanyName = line.COMPANY_NAME;
              a.Name = line.CONTACT_NAME;
              a.Email = line.EMAIL;
              a.FaxNo = line.FAX_NO;
              a.LastPickupFriday = line.LAST_PICKUP_FRI;
              a.LastPickupMondayToThursday = line.LAST_PICKUP_MON_THU;
              if (line.SHIP_PREFERENCE.Trim() == "MostCompetitive")
                  a.ShipPreference = BEnumShipPreference.MostCompetitive;
              else if (line.SHIP_PREFERENCE.Trim() == "NamedCarrier")
                  a.ShipPreference = BEnumShipPreference.NamedCarrier;
              else
                  a.ShipPreference = BEnumShipPreference.Fastest;

              a.State = line.STATE;
              a.TelephoneNo = line.TEL_NO;
              a.ZipCode = line.ZIPCODE;
              a.Country = line.COUNTRY;
              bAddressBook.Add(a);
          }


          return bAddressBook;
      }


  }
}
