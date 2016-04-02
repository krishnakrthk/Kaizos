using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text;
using System.Text.RegularExpressions;

using log4net;
using log4net.Config;

using System.ServiceModel;

using Kaizos;
using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;


namespace Kaizos
{
    public partial class frmZoneCreationUpdate : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmZoneCreationUpdate));

        string  ModeFlag        = "";
        int     ZoneID          = 0;
        string zn, ca, zn1; //[KM 21MAR12]
        int a; //[KM 21MAR12]
        
        
        protected void Page_Load(object sender, EventArgs e)
        {
			errorMsg.Attributes["style"] = "display: none;";
            if (!IsPostBack)
            {
                FillServiceTypeCheckBox();
                FillCountryListBox();

                if (string.IsNullOrEmpty(KaizosSession.Current.ModeFlag)) 
                    ModeFlag = "";
                else
                    ModeFlag = KaizosSession.Current.ModeFlag;
                

                /* Check control comes with CREATE ZONE OR UPDATE ZONE */
                if (ModeFlag.Equals("Update")) //Request.QueryString["ModeFlag"] != null
                {
                    Page.Title = GetGlobalResourceObject("LocalString", "frmZoneUpdate").ToString();
                    //ModeFlag = Request.QueryString["ModeFlag"].ToString();
                    ZoneID = Convert.ToInt32(KaizosSession.Current.ZoneID);
                    //ZoneID   = Convert.ToInt32(Request.QueryString["ZoneID"].ToString());

                    KaizosSession.Current.ModeFlag = "";
                    KaizosSession.Current.ZoneID = "";

                    ViewState["ZoneID"] = ZoneID;
                    ViewState["ModeFlag"]   = ModeFlag;
                    btnCreate.Text          = "Update";

                    try
                    {
                        KaizosServiceAgent proxy = new KaizosServiceAgent();
                        SZone sZone = proxy.GetZoneDetails(ZoneID);
                        FillScreenValue(sZone);
                        ViewState["ZoneNameOriginal"] = txtZoneName.Text.Trim(); //to fix 1257 [06MAR12RM]
                    }
                    catch (FaultException<SGeneralFault> sGeneralFault)
                    {
                        SGeneralFault fault = sGeneralFault.Detail;
                        string userName = User.Identity.Name;
                        string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + fault.Issue + fault.Details;
                        Server.Transfer("frmResult.aspx?DisplayMsg=" + errorMessage);
                        logger.Debug(errorMessage);
                    }
                    catch (Exception error)
                    {
                        string userName = User.Identity.Name;
                        string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                        Server.Transfer("frmResult.aspx?DisplayMsg=" + errorMessage);
                        logger.Debug(errorMessage);
                    }
                }
                else
                {
                    Page.Title = GetGlobalResourceObject("LocalString", "frmZoneCreation").ToString();
                    ModeFlag    = "Create";
                    ZoneID      = 0;
                    ViewState["ZoneID"] = ZoneID;
                    ViewState["ModeFlag"] = ModeFlag;
                    btnCreate.Text = "Create";
                    rblGeographical.SelectedIndex = 0; //05mar12RM  to fix 1253
                    //rblGeographical.SelectedIndex = -1;
                }
            }
            else
            {
                ModeFlag    = ViewState["ModeFlag"].ToString();
                ZoneID = Convert.ToInt32(ViewState["ZoneID"].ToString());
            }

        }

        protected void FillCountryListBox()
        {
             /* 1.Get Service type from TARIFF_MASTER table */
            KaizosServiceAgent proxy            = new KaizosServiceAgent();
            List<SComboText> sComboText         = new List<SComboText>();
            SComboTableField sComboTableField   = new SComboTableField();

            //To fill Origin drop down list
            sComboTableField.FieldName          = "COUNTRY_CODE";
            sComboTableField.TableName          = "COUNTRY";
            sComboText = proxy.FillCombo(sComboTableField).ToList();

            for (int i = 0; i < sComboText.Count; i++)
            {
                ddlCountry.Items.Add(sComboText[i].ComboText);
            }
        }

        protected void FillServiceTypeCheckBox()
        {

            /* 1.Get Service type from TARIFF_MASTER table */
            KaizosServiceAgent proxy            = new KaizosServiceAgent();
            List<SComboText> sComboText         = new List<SComboText>();
            SComboTableField sComboTableField   = new SComboTableField();

            //To fill Origin drop down list
            sComboTableField.FieldName = "MASTER_SERVICE_NAME";
            sComboTableField.TableName = "MASTER_SERVICE";
            sComboText = proxy.FillCombo(sComboTableField).ToList();

            for (int i = 0; i < sComboText.Count; i++)
            {
                cblServiceType.Items.Add(new ListItem(sComboText[i].ComboText, sComboText[i].ComboText));
            }

        }

        protected string GetSelectedMasterServiceName()
        {
            string result="";

            for(int i=0;i<cblServiceType.Items.Count;i++)
            {
                if (cblServiceType.Items[i].Selected) result = result + cblServiceType.Items[i].Value + ",";
            }

            if (result.Length >0) result = result.Substring(0,(result.Length-1));

            return result;
        }

        protected string GetSelectedDirection()
        {
            string result = "";

            for (int i = 0; i < cblDirection.Items.Count; i++)
            {
                if (cblDirection.Items[i].Selected) result = result + cblDirection.Items[i].Value + ",";
            }

            if (result.Length > 0) result = result.Substring(0, (result.Length - 1));

            return result;
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
			if (IsValid) //ValidateScreenControls()
			{
                try
                {
                    KaizosServiceAgent proxy = new KaizosServiceAgent();

                    SZone sZone = new SZone();

                    sZone.TariffReference = txtTariffReference.Text.Trim();
                    sZone.ZoneName = txtZoneName.Text.Trim();
                    sZone.Direction = GetSelectedDirection();
                    sZone.MasterServiceName = GetSelectedMasterServiceName();
                    sZone.GeographicalCoverage = rblGeographical.SelectedValue;
                    int k = 1;

                    //sZone.CountryCode           = "";
                    if (ddlCountry.Enabled)
                    {
                        sZone.CountryCode = ddlCountry.SelectedValue;
                    }
                    else //[19MAR KM]
                    {
                        sZone.CountryCode = "";
                    }

                    sZone.CoverageList = txtCountryZip.Text;
                    if (ModeFlag == "Update")
                    {
                        //string countr =ViewState["countrylist"].ToString();
                        //string zonecountry = txtCountryZip.Text;
                        //string country  =zonecountry.Replace(countr, "");
                        //if(country.StartsWith(","))
                        //{
                        //    country = country.Substring(1, country.Length - 1);
                        //}
                        string g = proxy.Getvalidatezone(txtCountryZip.Text, sZone.TariffReference, sZone.GeographicalCoverage, ZoneID);
                        if (g == "NODATA")
                        {
                            valzone.Visible = false;
                            int result = proxy.CreateZone(sZone, ModeFlag, ZoneID);
                        }
                        else
                        {
                            string error = Resources.LocalString.valzone.Trim();
                            string[] error1 = error.Split('-');
                            valzone.Text = error1[0].ToString() + " " + g + " " + error1[1].ToString();
                            k = 2;
                        }
                    }
                    else
                    {

                        int result = proxy.CreateZone(sZone, ModeFlag, ZoneID);
                    }
                    if (k == 2)
                    {
                        valzone.Visible = true;
                        errorMsg.Attributes["style"] = "display: block;";
                    }
                    else
                    {
                        string strReturnUrl = "frmZoneSearch.aspx";

                        if (ModeFlag.Equals("Create"))  //31JAN12RM bug id 1123
                            KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "ZoneCreationSuccess").ToString(), sZone.ZoneName);
                        else
                            KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "ZoneUpdateSuccess").ToString(), sZone.ZoneName);

                        KaizosSession.Current.ReturnURL = strReturnUrl;
                        Server.Transfer("frmResult.aspx", false);
                    }
                }
                catch (Exception error)
                {
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        /* Detail exception into log file */
                        string userName = User.Identity.Name;
                        string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                        logger.Debug(errorMessage);

                        /* User friendly message */
                        KaizosSession.Current.ReturnURL = "frmZoneCreationUpdate.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "ZoneCreationFailure").ToString(), txtZoneName.Text);
                        Server.Transfer("frmResult.aspx", false);
                    }
                }
			} else { errorMsg.Attributes["style"] = "display: block;"; }
        }

        protected bool ValidateScreenControls()
        {
            bool result = true;
            return result;
        }

        protected void FillScreenValue(SZone sZone)
        {
            try
            {
                if (sZone != null)
                {
                
                /* 1. Tariff Reference */
                    txtTariffReference.Text = sZone.TariffReference;

                /* 2. Zone Name */
                    txtZoneName.Text = sZone.ZoneName;

                /* 3. Direction */
                List<String> direction = sZone.Direction.Split(',').ToList();

                for (int i = 0; i < cblDirection.Items.Count; i++)
                {
                    cblDirection.Items[i].Selected = false;

                    for (int j = 0; j < direction.Count; j++)
                    {
                        if (cblDirection.Items[i].Text == direction[j].ToString())
                        {
                            cblDirection.Items[i].Selected = true;
                            break;
                        }
                    }

                }


                /* 4. Master Service Name */
                List<String> masterService = sZone.MasterServiceName.Split(',').ToList();

                for (int i = 0; i < cblServiceType.Items.Count; i++)
                {
                    cblServiceType.Items[i].Selected = false;

                    for (int j = 0; j < masterService.Count; j++)
                    {
                        if (cblServiceType.Items[i].Text == masterService[j].ToString())
                        {
                            cblServiceType.Items[i].Selected = true;
                            break;
                        }
                    }

                }


                /* 5. Geographical Coverage */
                if (sZone.GeographicalCoverage == "Country")
                {
                    rblGeographical.Items[0].Selected = true;

                    //To fix 1253 [05MAR12RM]
                    lblZipCode.Visible = true;
                    txtCountryZip.Visible = true;

                    /* 7. Country code / zip code */
                    txtCountryZip.Text = sZone.CoverageList;

                    lblCountry.Visible = false;
                    ddlCountry.Visible = false;
                    
                }
                else if (sZone.GeographicalCoverage == "ZipCode")
                {
                    rblGeographical.Items[1].Selected = true;

                    //To fix 1253 [05MAR12RM]
                    lblCountry.Visible = true;
                    ddlCountry.Visible = true;

                    /* 6. Country */ // [19MAR12KM]
                    int index = ddlCountry.Items.IndexOf(ddlCountry.Items.FindByValue(sZone.CountryCode));
                    ddlCountry.SelectedIndex = index;
                    txtCountryZip.Text = sZone.CoverageList;

                    lblZipCode.Visible = false;
                    // txtCountryZip.Visible = false;

                }
                

                }
            }
            catch (Exception error)
            {
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    /* Detail exception into log file */
                    string userName = User.Identity.Name;
                    string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                    logger.Debug(errorMessage);

                    /* User friendly message */
                    KaizosSession.Current.ReturnURL = "frmZoneCreationUpdate.aspx";
                    KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "ZoneCreationFailure").ToString(), txtZoneName.Text);
                    Server.Transfer("frmResult.aspx", false);
                }
            }

        }

        protected bool IsKeyExistsInGivenList(string[] Key, string DelimitedText)
        {
            bool result = false;
            
            string[] List = DelimitedText.Split(',');

            for (int i = 0; i < Key.Length; i++)
            {

                for (int j = 0; j < List.Length; j++)
                {
                    if (Key[i].Trim().ToUpper() == List[j].Trim().ToUpper())
                    {
                        result = true;
                        break;
                    }
                }

                if (result==true) break;
            }

            return result;
        }

        protected string AlreadyAssignedZipCountry(string[] Key, string DelimitedText)
        {
            string result = "";

            string[] List = DelimitedText.Split(',');

            for (int i = 0; i < Key.Length; i++)
            {

                for (int j = 0; j < List.Length ; j++)
                {
                    if (Key[i].Trim().ToUpper() == List[j].Trim().ToUpper())
                    {
                        result = result + Key[i] + ",";
                        continue;
                    }
                }
            }

            if (result.Length > 0) result = result.Substring(0, result.Length - 1);

            return result;
        }

        protected string RemoveDuplicateZipCountry(string DelimitedText)
        {
            string result = "";



            return result;
        }

        //To fix bug 1073[06FEB12RM]
        protected bool IsValidTariffRef(string strTariff)
        {

            string PasswordPattern = @"^[a-zA-Z0-9_]+$";

            Regex strPassword = new Regex(PasswordPattern);

            return (strPassword.IsMatch(strTariff.Trim()));
        }

        protected void val_Shipment_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;

            string strError = "";

            #region 1. Mandatory field check

            if (txtTariffReference.Text.Trim().Equals(""))
            {
                strError = strError + "*" + lblTariffReferencE.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            //  To fix bug 1073[06FEB12RM]
            if (args.IsValid)
            {
                if (!IsValidTariffRef(txtTariffReference.Text.Trim()))
                {
                    strError = strError + "*" + lblTariffReferencE.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
            }


            if (txtZoneName.Text.Trim().Equals(""))
            {
                strError = strError + "*" + lblZone.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            if (cblDirection.SelectedIndex == -1)
            {
                strError = strError + "*" + lblDirection.Text.Trim() + " " + valAccept.Text.Trim() + "<br>";
                args.IsValid = false;

            }

             if (cblServiceType.SelectedIndex == -1)
            {
                strError = strError + "*" + lblServiceType.Text.Trim() + " " + valAccept.Text.Trim() + "<br>";
                args.IsValid = false;

            }
            
            if (rblGeographical.SelectedIndex == -1)
            {
                strError = strError + "*" + lblGeographical.Text.Trim() + " " + valAccept.Text.Trim() + "<br>";
                args.IsValid = false;

            }

            if (rblGeographical.SelectedIndex == 1)  //1253 [05MAR12RM]
            {
                if (ddlCountry.SelectedIndex == -1)
                {
                    strError = strError + "*" + lblCountry.Text.Trim() + " " + valAccept.Text.Trim() + "<br>";
                    args.IsValid = false;

                }
            }

            if (rblGeographical.SelectedIndex == 0) //1253 [05MAR12RM]
            {
                if (txtCountryZip.Text.Trim().Equals(""))
                {
                    strError = strError + "*" + lblZipCode.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
            }

            if (rblGeographical.SelectedIndex == 0)  //1253 [05MAR12RM]
            {
                if (txtCountryZip.Text.Trim().Length > 500000) //1067 [24FEB12RM]
                {
                    strError = strError + "*" + lblZipCode.Text.Trim() + " " + GetGlobalResourceObject("LocalString", "ValLessThan").ToString() + " 500000<br>";
                    args.IsValid = false;
                }
            }

            #endregion

            KaizosServiceAgent proxy = new KaizosServiceAgent();
            List<SComboText> sComboText         = new List<SComboText>();
            SComboTableField sComboTableField   = new SComboTableField();

            #region 2.If all values provided check given Tariff Reference exits 

            if (args.IsValid)
            {
                /* 1.Get Service type from TARIFF_MASTER table */
                sComboTableField.FieldName          = "TARIFF_REFERENCE";
                sComboTableField.TableName          = "TARIFF_MAST";
                List<String> TariffRefList          = proxy.GetFieldValue(sComboTableField).ToList();
                
                bool bTariffExists =TariffRefList.Any(s => s.Equals(txtTariffReference.Text.Trim(), StringComparison.OrdinalIgnoreCase));

                if (!bTariffExists)
                {
                    strError = strError + "*" + lblTariffReferencE.Text.Trim() + " " + valNotExist.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
            }

            #endregion

            #region 3. Zone name already exists for the given Tariff Reference

            if (args.IsValid && ViewState["ModeFlag"].ToString().Trim().Equals("Create"))
            {
                List<String> ZoneName = proxy.GetTariffZoneName(txtTariffReference.Text.Trim());

                bool bZoneNameExists = ZoneName.Any(s => s.Equals(txtZoneName.Text.Trim(), StringComparison.OrdinalIgnoreCase));

                if (bZoneNameExists)
                {
                    strError = strError + "*" + lblZone.Text.Trim() + " " + valAlreadyExist.Text.Trim() + "<br>";
                    args.IsValid = false;
                }
            }
            

            #endregion

            #region 4. Validate listed countries are valid

            if (args.IsValid)
            {
                bool bCountryListValid = false;

                if (rblGeographical.SelectedValue.Equals("Country"))
                {

                    sComboTableField.FieldName          = "COUNTRY_CODE";
                    sComboTableField.TableName          = "COUNTRY";
                    List<String> CountryCodeList        = proxy.GetFieldValue(sComboTableField).ToList();


                    string[] countries = txtCountryZip.Text.Split(',');
                    
                    string InvalidCountryList = "";

                    for (int i = 0; i < countries.Length; i++)
                    {
                        /* Check given list of countries one by one */
                        bool bTariffExists = CountryCodeList.Any(s => s.Equals(countries[i].Trim(), StringComparison.OrdinalIgnoreCase));
                        
                        /* If doesn't exists include it in invalid country list */
                        if (!bTariffExists) InvalidCountryList = InvalidCountryList + countries[i] + ",";
                    }

                    if (InvalidCountryList.Length!=0)
                    {
                        InvalidCountryList = InvalidCountryList.Substring(0, InvalidCountryList.Length - 1);
                        strError = strError + "*" + lblCountry.Text.Trim() + " [" + InvalidCountryList + "] " + valInvalid.Text.Trim() + "<br>";
                        args.IsValid = false;
                    }

                }
                
              }

            #endregion

            #region 5. Validate list of Zip codes are valid for the selected country.
            //To be add after provided zip code details database from YOHANN 
            #endregion

            #region 6. Validate countries or zip codes provided already exist in another ZONE for same TARIFF_REFERENCE + DIRECTION + SERVICE_TYPE

            if (args.IsValid && ViewState["ModeFlag"].ToString().Trim().Equals("Create"))
            {
                List<SZone> sZoneCoverageList = proxy.GetZoneCoverageList(txtTariffReference.Text.Trim());
                string[] DirectionList = GetSelectedDirection().Split(',');
                string[] MasterServiceNameList = GetSelectedMasterServiceName().Split(',');
                string[] CountryZipList = txtCountryZip.Text.Split(',');

                string ErrorMessage = "";

                for (int i = 0; i < sZoneCoverageList.Count; i++)
                {
                    SZone sZone = sZoneCoverageList[i];

                    if (IsKeyExistsInGivenList(DirectionList, sZoneCoverageList[i].Direction) &&
                        IsKeyExistsInGivenList(MasterServiceNameList, sZoneCoverageList[i].MasterServiceName))
                    {

                        string AssignedZipCountry = AlreadyAssignedZipCountry(CountryZipList, sZoneCoverageList[i].CoverageList);

                        if (AssignedZipCountry.Length > 0) ErrorMessage = ErrorMessage + AssignedZipCountry + " [" + sZoneCoverageList[i].ZoneName + "] <br>";
                    }

                }

                if (ErrorMessage.Length != 0)
                {
                    strError = "";
                    strError = strError + "*" + lblZipCode.Text.Trim() + " " + valAlreadyExist.Text.Trim() + "<br>" + ErrorMessage;
                    args.IsValid = false;
                }
            }
            else if (args.IsValid && ViewState["ModeFlag"].ToString().Trim().Equals("Update"))  //08FEB12RM [KM 21MAR12]
            {
                List<SZone> sZoneCoverageList = proxy.GetZoneCoverageList(txtTariffReference.Text.Trim());
                string[] DirectionList = GetSelectedDirection().Split(',');
                string[] MasterServiceNameList = GetSelectedMasterServiceName().Split(',');
                string[] CountryZipList = txtCountryZip.Text.Split(',');

                string ErrorMessage = "";


                for (int i = 0; i < sZoneCoverageList.Count; i++)
                {
                    //if (sZoneCoverageList[i].ZoneName.ToUpper().Equals(txtZoneName.Text.Trim().ToUpper()))
                    string ZoneNameOriginal = "";

                    if (ViewState["ZoneNameOriginal"] != null)
                    {
                        ZoneNameOriginal = ViewState["ZoneNameOriginal"].ToString();
                    }

                    zn = txtZoneName.Text.ToString();

                    a = 0;
                    if (ZoneNameOriginal.ToUpper().Equals(zn.ToUpper()))
                    {
                        for (int j = 0; j < sZoneCoverageList.Count; j++)
                        {
                            if (zn.ToUpper().Equals(sZoneCoverageList[j].ZoneName.ToUpper()))
                            {
                                sZoneCoverageList.RemoveAt(j);
                                a = 0;
                            }
                        }

                    }
                    else
                    {
                        // if (sZoneCoverageList[i].ZoneName.ToUpper().Equals(ZoneNameOriginal.ToUpper()))
                        for (int j = 0; j < sZoneCoverageList.Count; j++)
                        {
                            if (zn.ToUpper().Equals(sZoneCoverageList[j].ZoneName.ToUpper()))
                            {
                                a = 1;
                                break;

                            }

                        }

                    }



                    if (a == 1)
                    {
                        break;
                    }
                    if (a == 0)
                    {
                        if (sZoneCoverageList[i].ZoneName.ToUpper().Equals(ZoneNameOriginal.ToUpper()))
                        {
                            sZoneCoverageList.RemoveAt(i);
                            break;

                        }
                    }
                }


                /*for (int i = 0; i < sZoneCoverageList.Count; i++)
                {
                    SZone sZone = sZoneCoverageList[i];

                    if (IsKeyExistsInGivenList(DirectionList, sZoneCoverageList[i].Direction) &&
                        IsKeyExistsInGivenList(MasterServiceNameList, sZoneCoverageList[i].MasterServiceName))
                    {

                        string AssignedZipCountry = AlreadyAssignedZipCountry(CountryZipList, sZoneCoverageList[i].CoverageList);

                        if (AssignedZipCountry.Length > 0) ErrorMessage = ErrorMessage + AssignedZipCountry + " [" + sZoneCoverageList[i].ZoneName + "] <br>";
                    }

                }*/

                if (a != 0)
                {
                    strError = "";
                    //strError = strError + "*" + lblZipCode.Text.Trim() + " " + valAlreadyExist.Text.Trim() + "<br>" + ErrorMessage;
                    strError = "The Zone " + zn.ToString() + " is already Exists";

                    args.IsValid = false;
                }
            }

            

            #endregion

                         

             if (!(args.IsValid))
             {
                 val_Shipment.ErrorMessage = strError;
             }
             else
             {
                 //Remove repeated country codes or zip codes in the given list.
             }
        }

        //To fix 1253 [05MAR12RM] [19MAR12KM]
        protected void rblGeographical_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCountry.Visible = true;
            ddlCountry.Visible = true;

            lblZipCode.Visible = true;
            txtCountryZip.Visible = true;

            if (rblGeographical.SelectedIndex == 0)
            {
                lblCountry.Visible = false;
                ddlCountry.Visible = false;
                lblZipCode.Text = "Country";

                //lblZipCode.Visible      = false;
                //txtCountryZip.Visible   = false;
            }
            else if (rblGeographical.SelectedIndex == 1)
            {
                //lblZipCode.Visible      = false;
                //txtCountryZip.Visible   = false;
                lblZipCode.Text = "ZipCode";
            }
        }

        protected void txtTariffReference_TextChanged(object sender, EventArgs e)
        {

        }
    }
}