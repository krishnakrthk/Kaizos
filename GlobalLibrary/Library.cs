using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Objects;

using System.Transactions;
using System.IO; //file upload

using Kaizos.Entities.Business;
using KaizosEntities;
//For Regular Expression
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;

namespace Kaizos.Components.GlobalLibrary
{
  public class Library
  {
    /// <summary>
    /// Loop the Exception and concate all main and inner exception error message
    /// </summary>
    /// <param name="error"></param>
    /// <returns>string</returns>
    public static string ExtractError(Exception error)
    {
      string TotalError = null;

      while (error != null)
      {
        TotalError = TotalError + error.Message;
        error = error.InnerException;
      }
      return TotalError;
    }

    /// <summary>
    /// Get the counter for creating
    /// </summary>
    /// <returns>
    /// All positive  values =&gt; Current counter value
    /// -1 =&gt; Key code does not exists
    /// -2 =&gt; Counter out of range
    /// </returns>
    public int GetNextCounter(string KeyCode)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Get list of data from the given field name of Table
    /// </summary>
    /// <returns>
    /// for success =&gt; String List
    /// Failure =&gt; NULL
    /// </returns>
    public List<string> FillCombo(string TableName, string FieldName)
    {
        throw new System.NotImplementedException();
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
        string patternStrict = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				                [0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				                [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

        Regex strEmail = new Regex(patternStrict);

        if (strEmail.IsMatch(EmailID))
        {
            return 0;
        }
        else
        {
            return 1;
        }
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
        bool lower = false;
        bool other = false;
        bool upper = false;
        bool number = false;

        string numberChars = "0123456789";
        string lowerChars = "abcdefghijklmnopqrstuvwxyz";
        string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string otherChars = "/_,-,+,=,!,:,.,,,?,(,),$,&/";

        string PasswordPattern = @"/[a-z]/" +
                                    @"/[A-Z]/" +
                                    @"/[0-9]/" +
                                    @"/_,-,+,=,!,:,.,,,?,(,),$,&/";

        Regex strPassword = new Regex(PasswordPattern);
        if (password.Length < 8 && password.Length > 15)
            result = 1;
        else if (strPassword.IsMatch(password))
        {
            result = 1;
        }
        else
        {
            result = 0;
            int localcount = 0;

            for (int i = 0; i < password.Length; i++)
            {
                localcount = 0;

                for (int j = 0; j < password.Length; j++)
                {
                    if (password[i].Equals(password[j]))
                    {
                        localcount++;
                        if (localcount == 3)
                            return result = 1;
                    }
                }

                //To validate if any one number is avilable or not
                for (int k = 0; k < numberChars.Length; k++)
                {
                    if (password[i].Equals(numberChars[k]))
                        number = true;
                }

                //To validate if any one Upper character is avilable or not
                for (int k = 0; k < upperChars.Length; k++)
                {
                    if (password[i].Equals(upperChars[k]))
                        upper = true;
                }

                //To validate if any one lower character is avilable or not
                for (int k = 0; k < lowerChars.Length; k++)
                {
                    if (password[i].Equals(lowerChars[k]))
                        lower = true;
                }

                //To validate if any one Other character is avilable or not
                for (int k = 0; k < otherChars.Length; k++)
                {
                    if (password[i].Equals(otherChars[k]))
                        other = true;
                }
            }
        }

        int status = 0;
        if (number)
            status++;
        if (upper)
            status++;
        if (lower)
            status++;
        if (other)
            status++;

        if (status >= 3)
            result = 0;
        else
            result = 1;

        return result;
    }

    /// <summary>
    /// Retrieve allowed file types for upload
    /// </summary>
    /// <returns>string  =&gt; list of allowed type with comma seperator</returns>
    public string GetFileType(string KeyCode)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Get list of data from the given field name of Table
    /// </summary>
    /// <returns>
    /// for success =&gt; String List
    /// Failure =&gt; NULL
    /// </returns>
    /// 
    public List<BComboText> FillCombo(BComboTableField bComboTableField)
    {
        List<BComboText> bComboText = new List<BComboText>();

        try
        {
            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
           
            var tblComboText = context.uSP_GET_COMBO_LIST(bComboTableField.TableName, bComboTableField.FieldName).ToList();

            for (int i = 0; i < tblComboText.Count; i++)
            {
                BComboText c = new BComboText();
                c.ComboText = tblComboText[i].ToString();
                bComboText.Add(c);
            }
        }
        catch (Exception error)
        {
            //return 1;

        }

        return bComboText;
    }

    /// <summary>
      /// To insert a Address book entry
      /// </summary>
      /// <param name="bAddressBook"></param>
      /// <returns></returns>
    public int InsertAddressBook(BAddressBook bAddressBook)
    {
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        string strAddressType = "", strDeliveryType = "", strShipPreference = "";
        
        if (bAddressBook.AddressType == BEnumAddressType.Company)
            strAddressType = "C";
        else
            strAddressType = "R";

        if (bAddressBook.AddressUsedFor == BEnumDeliveryType.DeliveryAddress)
            strDeliveryType = "D";
        else if (bAddressBook.AddressUsedFor == BEnumDeliveryType.ShippingAddress)
            strDeliveryType = "S";
        else
            strDeliveryType = "B";

        if (bAddressBook.ShipPreference == BEnumShipPreference.Fastest)
            strShipPreference = "Fastest";
        else if (bAddressBook.ShipPreference == BEnumShipPreference.MostCompetitive)
            strShipPreference = "MostCompetitive";
        else
            strShipPreference = "NamedCarrier";
        strShipPreference = bAddressBook.ShipPreferenceOrder;
        System.Nullable<int> iReturnValue = context.uSP_ADDRESS_BOOK_INSERT(strAddressType, bAddressBook.CompanyName, bAddressBook.Name, bAddressBook.TelephoneNo, bAddressBook.FaxNo,
                                                                            bAddressBook.Address1, bAddressBook.Address2, bAddressBook.Address3, bAddressBook.ZipCode,
                                                                            bAddressBook.City, bAddressBook.State, bAddressBook.Country, bAddressBook.Email, bAddressBook.LastPickupMondayToThursday,
                                                                            bAddressBook.LastPickupFriday, bAddressBook.Comments, strShipPreference, strDeliveryType, bAddressBook.CreatedDate,
                                                                            bAddressBook.LastUpdated, bAddressBook.AccountNo,bAddressBook.NamedCarrier).SingleOrDefault();
                                                                            
        return (int)iReturnValue;

    }

    /// <summary>
    /// To Encrypte a given string 
    /// </summary>
    /// <param name="Message"></param>
    /// <param name="Passphrase"></param>
    /// <returns>
    /// To return Encrypted string
    /// </returns>
    /// 
    public string EncryptString(string Message, string Passphrase)
    {
        byte[] Results;
        System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

        // Step 1. We hash the passphrase using MD5
        // We use the MD5 hash generator as the result is a 128 bit byte array
        // which is a valid length for the TripleDES encoder we use below
        MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
        byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

        // Step 2. Create a new TripleDESCryptoServiceProvider object
        TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

        // Step 3. Setup the encoder
        TDESAlgorithm.Key = TDESKey;
        TDESAlgorithm.Mode = CipherMode.ECB;
        TDESAlgorithm.Padding = PaddingMode.PKCS7;

        // Step 4. Convert the input string to a byte[]
        byte[] DataToEncrypt = UTF8.GetBytes(Message);

        // Step 5. Attempt to encrypt the string
        try
        {
            ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
            Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
        }
        finally
        {
            // Clear the TripleDes and Hashprovider services of any sensitive information
            TDESAlgorithm.Clear();
            HashProvider.Clear();
        }

        // Step 6. Return the encrypted string as a base64 encoded string
        return Convert.ToBase64String(Results);
    }

    /// <summary>
    /// To decrypted a given string 
    /// </summary>
    /// <param name="Message"></param>
    /// <param name="Passphrase"></param>
    /// <returns>
    /// To return decrypted string
    /// </returns>
    public string DecryptString(string Message, string Passphrase)
    {
        byte[] Results;
        Message = Message.Replace(' ', '+');
        System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

        // Step 1. We hash the passphrase using MD5
        // We use the MD5 hash generator as the result is a 128 bit byte array
        // which is a valid length for the TripleDES encoder we use below
        MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
        byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

        // Step 2. Create a new TripleDESCryptoServiceProvider object
        TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

        // Step 3. Setup the decoder
        TDESAlgorithm.Key = TDESKey;
        TDESAlgorithm.Mode = CipherMode.ECB;
        TDESAlgorithm.Padding = PaddingMode.PKCS7;

        // Step 4. Convert the input string to a byte[]
        byte[] DataToDecrypt = Convert.FromBase64String(Message);

        // Step 5. Attempt to decrypt the string
        try
        {
            ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
            Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
        }
        finally
        {
            // Clear the TripleDes and Hashprovider services of any sensitive information
            TDESAlgorithm.Clear();
            HashProvider.Clear();
        }

        // Step 6. Return the decrypted string in UTF8 format
        return UTF8.GetString(Results);
    }

    public List<String> GetFieldValue(BComboTableField bComboTableField)
    {
        List<String> bFieldValue = new List<String>();

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        var tblComboText = context.uSP_GET_COMBO_LIST(bComboTableField.TableName, bComboTableField.FieldName).ToList();

        for (int i = 0; i < tblComboText.Count; i++)
        {
            bFieldValue.Add(tblComboText[i].ToString().Trim());
        }

        return bFieldValue;
    }

    /// <summary>
    /// Validate given zipcode
    /// </summary>
    /// <returns>
    /// 0 =&gt; Valid
    /// 1 =&gt; Invalid
    /// </returns>
    public int ValidateZipCode(string PostalCode, string CountryCode)
    {
        int result = 1;
        string strCountry = CountryCode.Trim();
        string slPostalFormatList = "NNNN"; // Need to fetch this value from country table after get the confirmation from Yohann

        if (PostalCode.Trim() == "")
        {
            result = 0;
            return result;
        }

        int istrLength = PostalCode.Trim().Length;
        int iLoop = 0;
        string strInputFormat = "";

        while (iLoop < istrLength)
        {
            if ((PostalCode[iLoop] >= 'A' || PostalCode[iLoop] >= 'a') && (PostalCode[iLoop] <= 'Z' || PostalCode[iLoop] <= 'z'))
                strInputFormat = strInputFormat + "A";
            else if (PostalCode[iLoop] >= '0' || PostalCode[iLoop] <= '9')
                strInputFormat = strInputFormat + "N";
            else
            {
                result = 1;
                return result;
            }
            iLoop = iLoop + 1;
        }

        if (PostalCode.Trim() == "" && strInputFormat.Trim() == "")
        {
            result = 1;
            return result;
        }

        if (strInputFormat.Trim() == slPostalFormatList.Trim())
        {
            result = 0;
            return result;
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
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        var LanguageList = context.uSP_GET_LANGUAGE(CountryCode.Trim()).SingleOrDefault();
        string[] LanguageList1 = LanguageList.Split(',');
        Array.Sort(LanguageList1);
        return LanguageList1;
    }

    /// <summary>
    /// To get language list
    /// </summary>
    /// <returns>
    /// result or null
    /// </returns>
    public List<String> GetLanguageList()
    {
        string[] result = null;
        List<BComboText> bComboText = new List<BComboText>();

        List<String> temp = new List<String>();

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        var LanguageList = context.uSP_GET_LANGUAGE_LIST().ToList();
        
        for (int i = 0; i < LanguageList.Count; i++)
        {
            result = LanguageList[i].LANGUAGES.Split(',');
            for (int j = 0; j < result.Count(); j++)
            {
                BComboText bCombo = new BComboText();
                bCombo.ComboText = result[j];
                bComboText.Add(bCombo);
            }
        }

        for (int i = 0; i < bComboText.Count; i++)
        {
            temp.Add(bComboText[i].ComboText.ToString());
        }

        List<String> temp1 = new List<String>();
        temp1.AddRange(temp.Distinct());
        temp1.Sort();
        return temp1;

    }

    /// <summary>
    /// Get the counter for creating
    /// </summary>
    /// <returns>
    /// All positive  values =&gt; Current counter value
    /// -1 =&gt; Key code does not exists
    /// -2 =&gt; Counter out of range
    /// </returns>
    public BNextcounter GetNextCounter(string KeyCode, string CounterType, int RequiredCount)
    {
        BNextcounter bNextcounter = new BNextcounter();
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();

        ObjectParameter objParamErrrorCode = new ObjectParameter("ERROR", typeof(String));
        ObjectParameter objParamNewCounter = new ObjectParameter("NEW_COUNTER", typeof(Int32));

        System.Nullable<int> iReturnValue1 = context.uSP_GET_NEXT_COUNTER(KeyCode, CounterType, RequiredCount, objParamNewCounter, objParamErrrorCode).SingleOrDefault();

        bNextcounter.ErrorCode = objParamErrrorCode.Value.ToString();
        bNextcounter.ErrorDescription = "";
        bNextcounter.NextCounter = Convert.ToInt32(objParamNewCounter.Value);

        return bNextcounter;

    }

    /// <summary>
    /// Get list of data from the given field name of Table
    /// </summary>
    /// <returns>
    /// for success =&gt; String List
    /// Failure =&gt; NULL
    /// </returns>
    public List<BCountryTable> FillCountryCombo()
    {
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        var countries = context.uSP_GET_COUNTRY_DETAILS().ToList();

        List<BCountryTable> bCountry = new List<BCountryTable>();

        foreach (var counry in countries)
        {
            BCountryTable c = new BCountryTable();
            c.CountryCode = counry.COUNTRY_CODE.Trim();
            c.CountryName = counry.COUNTRY_NAME.Trim();
            c.CodeName = counry.CODE_NAME.Trim();
            bCountry.Add(c);
        }

        return bCountry;
    }

    /// <summary>
    /// To create a given length of password with in the assigned characters
    /// </summary>
    ///<returns>
    /// Return a password for the given length
    /// </returns>
    
    public string CreatePassword(int length)
    {
        string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_-+=!:.,?()$&";
        string res = "";
        Random rnd = new Random();
        while (0 < length--)
            res += valid[rnd.Next(valid.Length)];
        return res;
    }


    /// <summary>
    /// To send an e-mail to auditors
    /// </summary>
    /// <param name="UserID">MailID</param>
    /// <returns></returns>
    public bool sendMessage(string UserID, string Password, string CurrentUserID)
    {
        bool bSend = true;
        string strUrl = "";
        try
        {
            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            var userContext = context.uSP_GET_MAIL_DETAIL().SingleOrDefault();
            BKeyValue bKeyvalue = new BKeyValue();
            bKeyvalue = GetValueFromParameter("ACTIVATE_URL"); //[15FEB12SM]
            strUrl = bKeyvalue.Value.Trim() + "?UserID=" + EncryptString(UserID.Trim(), "Password"); //[15FEB12SM]


            /* Get mail rely server information from config file */
            string strHostAddress = userContext.HOSTADDRESS.Trim();
            string strHostUserID = userContext.HOSTUSERID.Trim();
            string strHostPwd = userContext.HOSTPWD.Trim();
            string strPort = userContext.PORT.Trim();
            string strSSLEnabled = userContext.SSLENABLED.Trim();

            string strBody = "";
            /* //[15FEB12SM]
            strBody = "User ID : " + UserID.Trim() + Environment.NewLine +
                      "Password : " + Password.Trim();
            */
            strBody = "Confirmation of user creation\n\n" + strUrl.Trim(); //[15FEB12SM]

            MailMessage mm = new MailMessage(); ;

            /* Prepare from and to address */
            mm.To.Add(new MailAddress(UserID));
            mm.From = new MailAddress(CurrentUserID);

            /* Fill subject of the mail*/
            mm.Subject = "Kaizos -Confirmation of user creation";

            /* Attach content of the mail */
            mm.Body = strBody;

            /* Configure Smtp rely server with port */
            SmtpClient smtp = new SmtpClient(strHostAddress, Convert.ToInt32(strPort));

            /* Enable SSL if required */
            if (strSSLEnabled.ToUpper().Equals("YES"))
                smtp.EnableSsl = true;
            else
                smtp.EnableSsl = false;

            /* Disable default credential and give explicit credentail to access SMTP host */
            smtp.UseDefaultCredentials = false;

            /* Smtp host credentails */
            smtp.Credentials = new NetworkCredential(strHostUserID, strHostPwd);

            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            /* Send the message */
            smtp.Send(mm);
        }
        catch (Exception e)
        {
            bSend = false;
        }

        return bSend;
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
        try
        {
            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            var ZipCity = context.uSP_GET_AUTO_FILL_LIST(SearchString, Country, Count);
            string strResult = "";
            foreach (var z in ZipCity)
            {
                //strResult = z.ZIP_CODE.Trim() + " + " + z.CITY.Trim();
                strResult = z.CITY.Trim() + " - " + z.ZIP_CODE.Trim();
                result.Add(strResult);
            }

        }
        catch (Exception error)
        {

        }


        return result;

    }

    /// <summary>
    /// Get all available address list
    /// </summary>
    /// <returns>List of address book</returns>

    public List<BAddressBook> GetAddress()
    {
        List<BAddressBook> bAddressList = new List<BAddressBook>();

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        var AddressList = context.uSP_GET_ADDRESS_BOOK_LIST().ToList();

        for (int i = 0; i < AddressList.Count; i++)
        {
            BAddressBook bAddressBook = new BAddressBook();

            bAddressBook.AccountNo = "";
            bAddressBook.Address1 = AddressList[i].ADDRESS1;
            bAddressBook.Address2 = AddressList[i].ADDRESS2;
            bAddressBook.Address3 = AddressList[i].ADDRESS3;

            if (AddressList[i].ADDRESS_TYPE == "R")
                bAddressBook.AddressType = BEnumAddressType.Residential;
            else
                bAddressBook.AddressType = BEnumAddressType.Company;

            if (AddressList[i].ADDR_USED_FOR == "D")
                bAddressBook.AddressUsedFor = BEnumDeliveryType.DeliveryAddress;
            else if (AddressList[i].ADDR_USED_FOR == "S")
                bAddressBook.AddressUsedFor = BEnumDeliveryType.ShippingAddress;
            else
                bAddressBook.AddressUsedFor = BEnumDeliveryType.Both;

            bAddressBook.City = AddressList[i].CITY;
            bAddressBook.Comments = AddressList[i].COMMENTS;
            bAddressBook.CompanyName = AddressList[i].COMPANY_NAME;
            bAddressBook.Country = AddressList[i].COUNTRY;
            //bAddressBook.CreatedDate;
            bAddressBook.Email = AddressList[i].EMAIL;
            bAddressBook.FaxNo = AddressList[i].FAX_NO;
            bAddressBook.LastPickupFriday = AddressList[i].LAST_PICKUP_FRI;
            bAddressBook.LastPickupMondayToThursday = AddressList[i].LAST_PICKUP_MON_THU;
            //bAddressBook.LastUpdated;
            bAddressBook.Name = AddressList[i].CONTACT_NAME;

            if (AddressList[i].SHIP_PREFERENCE.Trim() == "MostCompetitive")
                bAddressBook.ShipPreference = BEnumShipPreference.MostCompetitive;
            else if (AddressList[i].SHIP_PREFERENCE.Trim() == "NamedCarrier")
                bAddressBook.ShipPreference = BEnumShipPreference.NamedCarrier;
            else
                bAddressBook.ShipPreference = BEnumShipPreference.Fastest;

            bAddressBook.State = AddressList[i].STATE;
            bAddressBook.TelephoneNo = AddressList[i].TEL_NO;
            bAddressBook.ZipCode = AddressList[i].ZIPCODE;
            bAddressBook.NamedCarrier = AddressList[i].NAMEDCARRIER;//added by HV 16APR2012
            bAddressBook.ShipPreferenceOrder = AddressList[i].SHIP_PREFERENCE;//added by HV 16APR2012
            bAddressList.Add(bAddressBook);
        }
        return bAddressList;
    }

    /// <summary>
    /// To validate time format HH:MM
    /// </summary>
    /// <param name="strTime"></param>
    /// <returns></returns>
    public int ValidateTime(String strTime)
    {
        int result = 1;
        string PasswordPattern = "^([0-1][0-9]|[2][0-3]):([0-5][0-9])$";

        Regex strPassword = new Regex(PasswordPattern);
        if (strPassword.IsMatch(strTime))
            result = 0;
        return result;
    }

    protected BFileImportStatus GetFileImportStatus(int Row, string FieldName, string ErrorMsg)
    {
        BFileImportStatus bFileImportStatus = new BFileImportStatus();
        bFileImportStatus.RowNumber = Row;
        bFileImportStatus.FieldName = FieldName.Trim();
        bFileImportStatus.ErrorDescription = ErrorMsg.Trim();
        return bFileImportStatus;

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
    public List<BFileImportStatus> ImportAddressBook(byte[] addressBookFile, string AccountNo)
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
                    bool bValidateLine;

                    /* 1. IMPORT STATUS: Prepare Return Import status list with header contains details */
                    bFileImportStatus.Add(GetFileImportStatus(0, "NONE", "All lines are imported successfully"));

                    /* 2. Loop through the file and insert record by record */
                    Stream s = new MemoryStream(addressBookFile);

                    using (StreamReader sr = new StreamReader(s))
                    {
                        string line;

                        string[] myStrs = null;

                        int lineCount = 0;

                        while ((line = sr.ReadLine()) != null)
                        {
                            bValidateLine = true;
                            lineCount++;

                            myStrs = line.Split(';');

                            /* 2.1. IMPORT STATUS: If required no of fields not available just skip its integration into table */
                            if (myStrs.Length != 18)
                            {
                                bFileImportStatus.Add(GetFileImportStatus(lineCount, "ALL FIELDS", "Not having all required fields in the line"));
                                continue;
                            }

                            /* 2.2. Validate AddressBook line and inturn log the error if any in IMPORT STATUS */

                            if (myStrs[0].Length == 0)
                            {
                                bFileImportStatus.Add(GetFileImportStatus(lineCount, "Address Type", "Doesn''t exist"));
                                bValidateLine = false;
                            }
                            if (myStrs[1].Length == 0)
                            {
                                bFileImportStatus.Add(GetFileImportStatus(lineCount, "Company Name", "Doesn''t exist"));
                                bValidateLine = false;
                            }
                            if (myStrs[2].Length == 0)
                            {
                                bFileImportStatus.Add(GetFileImportStatus(lineCount, "Contact Name", "Doesn''t exist"));
                                bValidateLine = false;
                            }
                            if (myStrs[3].Length == 0)
                            {
                                bFileImportStatus.Add(GetFileImportStatus(lineCount, "Phone Number", "Doesn''t exist"));
                                bValidateLine = false;
                            }
                            if (myStrs[5].Length == 0)
                            {
                                bFileImportStatus.Add(GetFileImportStatus(lineCount, "Address1", "Doesn''t exist"));
                                bValidateLine = false;
                            }
                            //if (myStrs[6].Length == 0)
                            //{
                            //    bFileImportStatus.Add(GetFileImportStatus(lineCount, "Address2", "Doesn''t exist"));
                            //    bValidateLine = false;
                            //}
                            if (myStrs[8].Length == 0)
                            {
                                bFileImportStatus.Add(GetFileImportStatus(lineCount, "Zipcode", "Doesn''t exist"));
                                bValidateLine = false;
                            }
                            if (myStrs[9].Length == 0)
                            {
                                bFileImportStatus.Add(GetFileImportStatus(lineCount, "City", "Doesn''t exist"));
                                bValidateLine = false;
                            }
                            //if (myStrs[10].Length == 0)
                            //{
                            //    bFileImportStatus.Add(GetFileImportStatus(lineCount, "State", "Doesn''t exist"));
                            //    bValidateLine = false;
                            //}
                            if (myStrs[11].Length == 0)
                            {
                                bFileImportStatus.Add(GetFileImportStatus(lineCount, "Country", "Doesn''t exist"));
                                bValidateLine = false;
                            }
                            if (myStrs[12].Trim().Length != 0)
                            {
                                if (ValidateEmail(myStrs[12].Trim()) != 0)
                                {
                                    bFileImportStatus.Add(GetFileImportStatus(lineCount, "Email", "Email format is not valid"));
                                    bValidateLine = false;
                                }
                            }

                            if (myStrs[13].Trim().Length != 0)
                            {
                                if (ValidateTime(myStrs[13].Trim()) != 0)
                                {
                                    bFileImportStatus.Add(GetFileImportStatus(lineCount, "Last Pickup Mon-Thu", "Time format is not valid (HH:MM)"));
                                    bValidateLine = false;
                                }
                            }
                            else
                            {
                                bFileImportStatus.Add(GetFileImportStatus(lineCount, "Last Pickup Mon-Thu", "Doesn''t exist"));
                                bValidateLine = false;
                            }
                            if (myStrs[14].Trim().Length != 0)
                            {
                                if (ValidateTime(myStrs[14].Trim()) != 0)
                                {
                                    bFileImportStatus.Add(GetFileImportStatus(lineCount, "Last Pickup Friday", "Time format is not valid (HH:MM)"));
                                    bValidateLine = false;
                                }
                            }
                            else
                            {
                                bFileImportStatus.Add(GetFileImportStatus(lineCount, "Last Pickup Friday", "Doesn''t exist"));
                                bValidateLine = false;
                            }

                            if (myStrs[16].Length == 0)
                            {
                                bFileImportStatus.Add(GetFileImportStatus(lineCount, "Shipping Preference", "Doesn''t exist"));
                                bValidateLine = false;
                            } //4APR12SM - BugId : 1244
                            else if (myStrs[16].ToString() != "Fastest" && myStrs[16].ToString() != "MostCompetitive" && myStrs[16].ToString() != "NamedCarrier")
                            {
                                bFileImportStatus.Add(GetFileImportStatus(lineCount, "Shipping Preference", "Invalid Value"));
                                bValidateLine = false;

                            }
                            if (myStrs[17].Length == 0)
                            {
                                bFileImportStatus.Add(GetFileImportStatus(lineCount, "Address Used for", "Doesn''t exist"));
                                bValidateLine = false;
                            }

                            /* 5.3. If Line validation goes fine then integrate it or else skip it */
                            if (!bValidateLine)
                                continue;
                            else //otherwise, insert into newly created TARIFF table
                            {
                                string AddressType = myStrs[0].Trim();
                                string CompanyName = myStrs[1].Trim();
                                string ContactName = myStrs[2].Trim();
                                string Telephone = myStrs[3].Trim();
                                string Fax = myStrs[4].Trim();
                                string Address1 = myStrs[5].Trim();
                                string Address2 = myStrs[6].Trim();
                                string Address3 = myStrs[7].Trim();
                                string Zipcode = myStrs[8].Trim();
                                string City = myStrs[9].Trim();
                                string State = myStrs[10].Trim();
                                string Country = myStrs[11].Trim();
                                string Email = myStrs[12].Trim();
                                string LastPickup_M_T = myStrs[13].Trim();
                                string LastPickup_F = myStrs[14].Trim();
                                string Comments = myStrs[15].Trim();
                                string Ship_preference = myStrs[16].Trim();
                                string Addr_Used_for = myStrs[17].Trim();
                                string Account_no = AccountNo.Trim();

                                /* As all lines of import status returned by BFileImportStatus object not required to handdle individiual here */
                                int ret = (int)context.uSP_ADDRESS_BOOK_IMPORT(AddressType, CompanyName, ContactName, Telephone, Fax, Address1, Address2,
                                                                                Address3, Zipcode, City, State, Country, Email, LastPickup_M_T, LastPickup_F, Comments,
                                                                                Ship_preference, Addr_Used_for, Account_no).SingleOrDefault();

                                if (ret == 2)
                                    bFileImportStatus.Add(GetFileImportStatus(lineCount, Address1.Trim(), "Already exists in address list"));

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

            //TransactionAbortedException Message//
        }
        catch (ApplicationException ex)
        {
            //"ApplicationException Message//
        }

        return bFileImportStatus;
    }

    /// <summary>
    /// To get all available industry details
    /// </summary>
    /// <returns>BIndustry</returns>
    public List<BIndustry> GetIndustry()
    {
        List<BIndustry> bIndustry = new List<BIndustry>();

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        var IndustryList = context.uSP_GET_INDUSTRY().ToList();

        for (int i = 0; i < IndustryList.Count; i++)
        {
            BIndustry Industry = new BIndustry();

            Industry.Department = IndustryList[i].DEPARTMENT.Trim();
            Industry.Activity = IndustryList[i].ACTIVITY.Trim();
            bIndustry.Add(Industry);
        }
        return bIndustry;
    }

    /// <summary>
    /// To get Customer designation list
    /// </summary>
    /// <returns> Designation list</returns>
    public List<string> GetFunction()
    {
        List<string> result = new List<string>();

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        var FunctionList = context.uSP_GET_FUNCTION().ToList();
        string FunctionName;
        for (int i = 0; i < FunctionList.Count; i++)
        {
            FunctionName = FunctionList[i].FUNCTION_NAME.Trim();
            result.Add(FunctionName);
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
        Double result;
        return Double.TryParse(val, NumberStyle,
            System.Globalization.CultureInfo.CurrentCulture, out result);
    }

    /// <summary>
      /// To validate the alpha numeric characters
      /// </summary>
      /// <param name="val"></param>
      /// <returns></returns>
    public bool isAlphaNumericValidation(string val)
    {
        string PasswordPattern = @"^[a-zA-Z0-9 ]+$";

        Regex strPassword = new Regex(PasswordPattern);

        return (strPassword.IsMatch(val.Trim()));
    }

      /// <summary>
      /// To retrieve the parameter value for the given key code
      /// </summary>
      /// <param name="KeyCode"></param>
      /// <returns></returns>
    public BKeyValue GetValueFromParameter(string KeyCode)
    {
        BKeyValue result = new BKeyValue();
        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        ObjectParameter objKeyValue = new ObjectParameter("KEY_VALUE", typeof(String));
        System.Nullable<int> iReturnValue1 = context.uSP_GET_VALUE_FROM_PARAMETER(KeyCode, objKeyValue).SingleOrDefault();
        result.Value = objKeyValue.Value.ToString().Trim();
        result.ErrorCode = iReturnValue1.ToString();
        return result;
    }

    /// <summary>
    /// To send an e-mail to the particular user to confirm the password
    /// </summary>
    /// <param name="UserID">MailID</param>
    /// <returns></returns>
    public bool sendConfirmPassord(string UserId, string CurrentUserID)
    {
        bool bSend = true;
        string strUrl = "";
        try
        {
            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            var userContext = context.uSP_GET_MAIL_DETAIL().SingleOrDefault();
            BKeyValue bKeyvalue = new BKeyValue();
            bKeyvalue = GetValueFromParameter("CONFIRM_PWD_URL");
            strUrl = bKeyvalue.Value.Trim() + "?UserID=" + EncryptString(UserId.Trim(), "Password"); 


            /* Get mail rely server information from config file */
            string strHostAddress = userContext.HOSTADDRESS.Trim();
            string strHostUserID = userContext.HOSTUSERID.Trim();
            string strHostPwd = userContext.HOSTPWD.Trim();
            string strPort = userContext.PORT.Trim();
            string strSSLEnabled = userContext.SSLENABLED.Trim();

            string strBody = "";

            strBody = "Please open the link below to key your new password.\n\n" + strUrl.Trim();
            MailMessage mm = new MailMessage(); ;

            /* Prepare from and to address */
            mm.To.Add(new MailAddress(UserId));
            mm.From = new MailAddress(CurrentUserID);

            /* Fill subject of the mail*/
            mm.Subject = "Kaizos - Password recovery";

            /* Attach content of the mail */
            mm.Body = strBody;

            /* Configure Smtp rely server with port */
            SmtpClient smtp = new SmtpClient(strHostAddress, Convert.ToInt32(strPort));

            /* Enable SSL if required */
            if (strSSLEnabled.ToUpper().Equals("YES"))
                smtp.EnableSsl = true;
            else
                smtp.EnableSsl = false;

            /* Disable default credential and give explicit credentail to access SMTP host */
            smtp.UseDefaultCredentials = false;

            /* Smtp host credentails */
            smtp.Credentials = new NetworkCredential(strHostUserID, strHostPwd);

            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            /* Send the message */
            smtp.Send(mm);
        }
        catch (Exception e)
        {
            bSend = false;
        }

        return bSend;
    }

    public bool IsValidZidcode(string Zipcode)
    {
        bool result = true;
        return result;
    
    }

    /// <summary>
    /// To get the list reference carriers
    /// </summary>
    /// <returns>List of carriers</returns>
    public List<string> GetAllRefCarrierList()
    {
        List<string> result = new List<string>();

        KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
        var tblCarrierNames = context.uSP_GET_REF_CARRIER_LIST().ToList();

        foreach (var names in tblCarrierNames)
        {
            string t = "";
            t = names.CARRIER_NAME.Trim();
            result.Add(t);
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
        char[] chrHQzipcode = strZipcode.ToCharArray();
        string strformat = string.Empty;
        bool result = false;
        foreach (char c in chrHQzipcode)
        {
            if (char.IsDigit(c))
            {
                strformat = strformat + "N";


            }
            else if (char.IsLetter(c))
            {
                strformat = strformat + "A";
            }
        }
        if (strActualFormat.IndexOf(',') > 0)
        {
            string[] sFormat = strActualFormat.Split(',');
            foreach (string s in sFormat)
            {
                if (s.Length == strformat.Length)
                {
                    result = s.Contains(strformat);
                    if (result)
                    {
                        break;
                    }
                }
            }

        }
        else
        {
            if (strActualFormat.Length == strformat.Length)
            {
                result = strActualFormat.Contains(strformat);
            }
        }
        return result;
    }

    public string FrtoEn(string name)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR");

        //string before = "Voix ambiguë d'un cœur, qui au zéphyr préfère les \"jattes de kiwis\" d'été !";

        string decoded = Encoding.ASCII.GetString(Encoding.GetEncoding("Cyrillic").GetBytes(name));

        string pattern = "[\\p{P}]";
        string replacement = " ";
        Regex rgx = new Regex(pattern);

        decoded = rgx.Replace(decoded, replacement);

        return decoded;
    }

   /// <summary>
   /// 
   /// </summary>
   /// <param name="To"></param>
   /// <param name="CurrentUserID"></param>
   /// <param name="strBody"></param>
   /// <param name="Subject"></param>
   /// <returns></returns>
    public bool sendMail(string To, string CurrentUserID, string strBody, string Subject)
    {
        bool bSend = true;
        try
        {
            KaizosEntities.KaizosEntities context = new KaizosEntities.KaizosEntities();
            var userContext = context.uSP_GET_MAIL_DETAIL().SingleOrDefault();
            BKeyValue bKeyvalue = new BKeyValue();

            /* Get mail rely server information from config file */
            string strHostAddress = userContext.HOSTADDRESS.Trim();
            string strHostUserID = userContext.HOSTUSERID.Trim();
            string strHostPwd = userContext.HOSTPWD.Trim();
            string strPort = userContext.PORT.Trim();
            string strSSLEnabled = userContext.SSLENABLED.Trim();

            MailMessage mm = new MailMessage(); ;
            mm.IsBodyHtml = true;

            /* Prepare from and to address */
            mm.To.Add(new MailAddress(To));
            mm.From = new MailAddress(CurrentUserID);

            /* Fill subject of the mail*/
            mm.Subject = Subject;

            /* Attach content of the mail */
            mm.Body = strBody;

            /* Configure Smtp rely server with port */
            SmtpClient smtp = new SmtpClient(strHostAddress, Convert.ToInt32(strPort));

            /* Enable SSL if required */
            if (strSSLEnabled.ToUpper().Equals("YES"))
                smtp.EnableSsl = true;
            else
                smtp.EnableSsl = false;

            /* Disable default credential and give explicit credentail to access SMTP host */
            smtp.UseDefaultCredentials = false;

            /* Smtp host credentails */
            smtp.Credentials = new NetworkCredential(strHostUserID, strHostPwd);

            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            /* Send the message */
            smtp.Send(mm);
        }
        catch (Exception e)
        {
            bSend = false;
        }

        return bSend;
    }

  }
}
