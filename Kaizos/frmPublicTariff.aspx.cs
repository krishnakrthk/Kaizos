using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

using log4net;
using log4net.Config;

using System.ServiceModel;

using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;


namespace Kaizos
{
    public partial class frmPublicTariff : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmPublicTariff));

        string OriginFlag = "";
        string ShipTypeFlag = "";
        string MasterServiceNameFlag = "";
        string CaptionFlag = "";

        protected SEnumPublicTariffType GetTariffType(string TariffCode)
        {
            SEnumPublicTariffType sEnumPublicTariffType = new SEnumPublicTariffType();

            if (TariffCode.Trim().Equals("Export"))
                sEnumPublicTariffType = SEnumPublicTariffType.Export;
            else if (TariffCode.Trim().Equals("Import"))
                sEnumPublicTariffType = SEnumPublicTariffType.Import;
            else if (TariffCode.Trim().Equals("Domestic"))
                sEnumPublicTariffType = SEnumPublicTariffType.Domestic;

            return sEnumPublicTariffType;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmPublicTariff").ToString();
                try
                {
                    KaizosServiceAgent proxy = new KaizosServiceAgent();

                    /* Fill public tariff list in drop down list */
                    List<string> sTariffNameList= proxy.GetPublicTariffNames();
                    ddlTariffReference.DataSource = sTariffNameList;
                    ddlTariffReference.DataBind();


                    /* 1. Fill public tariff grid */
                    if (ddlTariffReference.Items.Count > -1)
                    {
                        ddlTariffReference.SelectedIndex = 0;

                        List<SPublicTariffSearchResult> sPublicTariffSearchResult = proxy.GetPublicTariff(ddlTariffReference.SelectedValue);
                        
                        if (sPublicTariffSearchResult.Count != 0)
                        {
                            gv_PublicTariff.DataSource = sPublicTariffSearchResult;
                            gv_PublicTariff.DataBind();
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
                        KaizosSession.Current.ReturnURL = "frmPublicTariff.aspx";
                        KaizosSession.Current.ErrorMessage = string.Format(GetGlobalResourceObject("LocalString", "PublicTariffFailure").ToString(), ddlTariffReference.SelectedValue);
                        Server.Transfer("frmResult.aspx", false);
                    }
                }
            }
        }

        protected void gv_PublicTariff_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                #region Hide all non value text boxes

                TextBox txtNewDomestic = (TextBox)e.Row.FindControl("txtDomestic");
                
                if (txtNewDomestic.Text == "-1")
                {
                    txtNewDomestic.Visible = false;
                    e.Row.Cells[4].Style.Add("border-left-width", "0px");
                    e.Row.Cells[4].Style.Add("border-top-width", "0px");
                    e.Row.Cells[4].Style.Add("border-bottom-width", "0px");
                }

                TextBox txtNewEurope = (TextBox)e.Row.FindControl("txtEurope");
                if (txtNewEurope.Text == "-1")
                {
                    txtNewEurope.Visible = false;
                    e.Row.Cells[5].Style.Add("border-left-width", "0px");
                    e.Row.Cells[5].Style.Add("border-right-width", "0px");
                    e.Row.Cells[5].Style.Add("border-top-width", "0px");
                    e.Row.Cells[5].Style.Add("border-bottom-width", "0px");
                }

                TextBox txtNewInternational = (TextBox)e.Row.FindControl("txtInternational");
                if (txtNewInternational.Text == "-1")
                {
                    txtNewInternational.Visible = false;
                    e.Row.Cells[6].Style.Add("border-left-width", "0px");
                    e.Row.Cells[6].Style.Add("border-top-width", "0px");
                    e.Row.Cells[6].Style.Add("border-bottom-width", "0px");

                }

                #endregion


                Label lblNewOriginCaption = (Label)e.Row.FindControl("lblOriginCaption");
                    Label lblNewlblShipType         = (Label)e.Row.FindControl("lblShipType");
                    Label lblNewMasterServiceName   = (Label)e.Row.FindControl("lblMasterServiceName");


                    if (lblNewOriginCaption.Text != OriginFlag)
                    {
                        OriginFlag = lblNewOriginCaption.Text;
                    }
                    else
                    {
                        lblNewOriginCaption.Visible = false;
                        e.Row.Cells[0].Style.Add("border-left-width", "0px");
                        e.Row.Cells[0].Style.Add("border-top-width", "0px");
                        e.Row.Cells[0].Style.Add("border-bottom-width", "0px");
                    }


                    if (lblNewlblShipType.Text != ShipTypeFlag)
                    {
                        ShipTypeFlag            = lblNewlblShipType.Text;
                        MasterServiceNameFlag   = lblNewMasterServiceName.Text;
                    }
                    else
                    {
                        lblNewlblShipType.Visible = false;
                        e.Row.Cells[1].Style.Add("border-left-width", "0px");
                        e.Row.Cells[1].Style.Add("border-top-width", "0px");
                        e.Row.Cells[1].Style.Add("border-bottom-width", "0px");



                        if (lblNewMasterServiceName.Text != MasterServiceNameFlag)
                            MasterServiceNameFlag = lblNewMasterServiceName.Text;
                        else
                        {
                            lblNewMasterServiceName.Visible = false;
                            e.Row.Cells[2].Style.Add("border-left-width", "0px");
                            e.Row.Cells[2].Style.Add("border-top-width", "0px");
                            e.Row.Cells[2].Style.Add("border-bottom-width", "0px");
                        }
                    }

                
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            if (IsValid) /* 1.Valid all entered values are numerical */
            {
                /* 2. Form the tariff values */
                List<SPublicTariffSearchResult> sPublicTariffUpdatedValue= new List<SPublicTariffSearchResult>(); 

                for (int i = 0; i < gv_PublicTariff.Rows.Count; i++)
                {

                    SPublicTariffSearchResult sPublicTariffSearchResult = new SPublicTariffSearchResult();
                    
                    Label   lblNewOrigin                        = (Label)gv_PublicTariff.Rows[i].FindControl("lblOrigin");
                    Label   lblNewShipType                      = (Label)gv_PublicTariff.Rows[i].FindControl("lblShipType");
                    Label   lblNewlblMasterServiceName          = (Label)gv_PublicTariff.Rows[i].FindControl("lblMasterServiceName");
                    Label lblNewlblMasterWeightRange            = (Label)gv_PublicTariff.Rows[i].FindControl("lblWeightRange");
                    
                    TextBox txtNewDomestic                      = (TextBox)gv_PublicTariff.Rows[i].FindControl("txtDomestic");
                    TextBox txtNewEurope                        = (TextBox)gv_PublicTariff.Rows[i].FindControl("txtEurope");
                    TextBox txtNewInternational                 = (TextBox)gv_PublicTariff.Rows[i].FindControl("txtInternational");

                    TextBox txtNewMinWt                         = (TextBox)gv_PublicTariff.Rows[i].FindControl("txtMinWt");
                    TextBox txtNewMaxWt                         = (TextBox)gv_PublicTariff.Rows[i].FindControl("txtMaxWt");


                    sPublicTariffSearchResult.Caption           = lblNewOrigin.Text.Trim();  //captions
                    sPublicTariffSearchResult.Name              = ddlTariffReference.SelectedValue;
                    sPublicTariffSearchResult.ShipType          = GetTariffType(lblNewShipType.Text.Trim());
                    sPublicTariffSearchResult.MasterServiceName = lblNewlblMasterServiceName.Text.Trim();
                    sPublicTariffSearchResult.MinWt             = Convert.ToInt32(txtNewMinWt.Text);
                    sPublicTariffSearchResult.MaxWt             = Convert.ToInt32(txtNewMaxWt.Text);
                    sPublicTariffSearchResult.WtRangeCaption    = lblNewlblMasterWeightRange.Text.Trim();
                    sPublicTariffSearchResult.Dom               = float.Parse(txtNewDomestic.Text.Trim());
                    sPublicTariffSearchResult.Eur               = float.Parse(txtNewEurope.Text.Trim());
                    sPublicTariffSearchResult.Int               = float.Parse(txtNewInternational.Text.Trim());

                    sPublicTariffUpdatedValue.Add(sPublicTariffSearchResult);
                }
                
                /* 3. Create proxy and call business method to update public tariff */
                KaizosServiceAgent proxy = new KaizosServiceAgent();
                int result = proxy.UpdatePublicTariff(sPublicTariffUpdatedValue);

                /* 4. Get the result and move to confirmation page with return URL */
                string strDisplayMsg = "";

                if (result==0)
                    strDisplayMsg = string.Format(GetGlobalResourceObject("LocalString", "PublicTariffSuccess").ToString(), sPublicTariffUpdatedValue[0].Name);
                else
                    strDisplayMsg = string.Format(GetGlobalResourceObject("LocalString", "PublicTariffFailure").ToString(), sPublicTariffUpdatedValue[0].Name);

                string strReturnUrl                 = "frmPublicTariff.aspx";
                KaizosSession.Current.ErrorMessage  = strDisplayMsg;
                KaizosSession.Current.ReturnURL     = strReturnUrl;
                Server.Transfer("frmResult.aspx", false);

            }
        }

        protected bool isNumericValidation(string val, System.Globalization.NumberStyles NumberStyle)
        {
            Double result;
            return Double.TryParse(val, NumberStyle,
                System.Globalization.CultureInfo.CurrentCulture, out result);
        }

        //08FEB12HN Bug ID : 1132 
        protected bool isValidPercentage(string Value)
        {
            bool result = false;
            
            string DecimalPattern = @"^\d{0,3}(\.\d{0,2})?$";
            Regex strPassword = new Regex(DecimalPattern);
            result = strPassword.IsMatch(Value.Trim());
            if ((result) && (Convert.ToDouble(Value) > 100))
            {
                result = false;
            }

            return result;
        }
        
        //15FEB12RM : 1181
        protected void val_PublicTariff_ServerValidate(object source, ServerValidateEventArgs args)
        {
            
            string strError = valInvalid.Text.Trim() + "<br>";

            for (int i = 0; i < gv_PublicTariff.Rows.Count; i++)
            {
                
                TextBox txtNewDomestic          = (TextBox)gv_PublicTariff.Rows[i].FindControl("txtDomestic");
                TextBox txtNewEurope            = (TextBox)gv_PublicTariff.Rows[i].FindControl("txtEurope");
                TextBox txtNewInternational     = (TextBox)gv_PublicTariff.Rows[i].FindControl("txtInternational");
                Label lblNewOrigin              = (Label)gv_PublicTariff.Rows[i].FindControl("lblOrigin");
                Label lblNewShipType            = (Label)gv_PublicTariff.Rows[i].FindControl("lblShipType");
                Label lblNewlblMasterServiceName= (Label)gv_PublicTariff.Rows[i].FindControl("lblMasterServiceName");
                Label lblNewlblMasterWeightRange = (Label)gv_PublicTariff.Rows[i].FindControl("lblWeightRange");

                if (!(txtNewDomestic.Text == "-1" && txtNewDomestic.Visible.Equals(false)))
                {
                    bool bDomestic = isNumericValidation(txtNewDomestic.Text, System.Globalization.NumberStyles.Float);
                
                    if (!bDomestic)
                    {
                        strError = strError + "*" + String.Format("{0,30}", lblNewOrigin.Text.Trim()) + "|" +
                                                    String.Format("{0,10}", lblNewShipType.Text.Trim()) + "|" +
                                                    String.Format("{0,30}", lblNewlblMasterServiceName.Text.Trim()) + "|" +
                                                    String.Format("{0,10}", lblNewlblMasterWeightRange.Text.Trim()) + " => <b>" +
                                                    txtNewDomestic.Text.Trim() + "</b><br>";
                        args.IsValid = false;
                    }
                    else if (bDomestic)  //Check it should be negative [03FEB12RM]
                    {
                        if (float.Parse(txtNewDomestic.Text) < 0)
                        {
                            strError = strError + "*" + String.Format("{0,30}", lblNewOrigin.Text.Trim()) + "|" +
                            String.Format("{0,10}", lblNewShipType.Text.Trim()) + "|" +
                            String.Format("{0,30}", lblNewlblMasterServiceName.Text.Trim()) + "|" +
                            String.Format("{0,10}", lblNewlblMasterWeightRange.Text.Trim()) + " => <b>" +
                            txtNewDomestic.Text.Trim() + "</b><br>";
                            args.IsValid = false;
                        }
                        else if (!(isValidPercentage(txtNewDomestic.Text))) //Check it should max allow 2 decimals and less than 100 [08FEB12HN]Bug ID : 1132
                        {
                            strError = strError + "*" + String.Format("{0,30}", lblNewOrigin.Text.Trim()) + "|" +
                                String.Format("{0,10}", lblNewShipType.Text.Trim()) + "|" +
                                String.Format("{0,30}", lblNewlblMasterServiceName.Text.Trim()) + "|" +
                                String.Format("{0,10}", lblNewlblMasterWeightRange.Text.Trim()) + " => <b>" +
                                txtNewDomestic.Text.Trim() + "</b><br>";
                            args.IsValid = false;
                        }
                    }
                }

                if (!(txtNewEurope.Text == "-1" && txtNewEurope.Visible.Equals(false)))
                {
                    
                    bool bEurope = isNumericValidation(txtNewEurope.Text, System.Globalization.NumberStyles.Float);

                    if (!bEurope)
                    {

                        strError = strError + "*" + String.Format("{0,30}", lblNewOrigin.Text.Trim()) + "|" +
                                                    String.Format("{0,10}", lblNewShipType.Text.Trim()) + "|" +
                                                    String.Format("{0,30}", lblNewlblMasterServiceName.Text.Trim()) + "|" +
                                                    String.Format("{0,10}", lblNewlblMasterWeightRange.Text.Trim()) + "=> <b>" +
                                                    txtNewEurope.Text.Trim() + "</b><br>";
                        args.IsValid = false;
                    }
                    else if (bEurope) //Check it should be negative [03FEB12RM]
                    {
                        if (float.Parse(txtNewEurope.Text) < 0)
                        {
                            strError = strError + "*" + String.Format("{0,30}", lblNewOrigin.Text.Trim()) + "|" +
                            String.Format("{0,10}", lblNewShipType.Text.Trim()) + "|" +
                            String.Format("{0,30}", lblNewlblMasterServiceName.Text.Trim()) + "|" +
                            String.Format("{0,10}", lblNewlblMasterWeightRange.Text.Trim()) + "=> <b>" +
                            txtNewEurope.Text.Trim() + "</b><br>";
                            args.IsValid = false;
                        }
                        else if (!(isValidPercentage(txtNewEurope.Text))) //Check it should allow max 2 decimals and less than 100 [08FEB12HN]Bug ID : 1132
                        {
                            strError = strError + "*" + String.Format("{0,30}", lblNewOrigin.Text.Trim()) + "|" +
                                String.Format("{0,10}", lblNewShipType.Text.Trim()) + "|" +
                                String.Format("{0,30}", lblNewlblMasterServiceName.Text.Trim()) + "|" +
                                String.Format("{0,10}", lblNewlblMasterWeightRange.Text.Trim()) + " => <b>" +
                                txtNewEurope.Text.Trim() + "</b><br>";
                            args.IsValid = false;
                        }
                    }
                }

                if (!(txtNewInternational.Text == "-1" && txtNewInternational.Visible.Equals(false)))
                {
                    bool bInternational = isNumericValidation(txtNewInternational.Text, System.Globalization.NumberStyles.Float);
                    if (!bInternational)
                    {
                        strError = strError + "*" + String.Format("{0,30}", lblNewOrigin.Text.Trim()) + "|" +
                                                    String.Format("{0,10}", lblNewShipType.Text.Trim()) + "|" +
                                                    String.Format("{0,30}", lblNewlblMasterServiceName.Text.Trim()) + "|" +
                                                    String.Format("{0,10}", lblNewlblMasterWeightRange.Text.Trim()) + "=> <b>" +
                                                    txtNewInternational.Text.Trim() + "</b><br>";
                        args.IsValid = false;
                    }
                    else if (bInternational) //Check it should be negative [03FEB12RM]  
                    {
                        if (float.Parse(txtNewInternational.Text) < 0)
                        {
                            strError = strError + "*" + String.Format("{0,30}", lblNewOrigin.Text.Trim()) + "|" +
                            String.Format("{0,10}", lblNewShipType.Text.Trim()) + "|" +
                            String.Format("{0,30}", lblNewlblMasterServiceName.Text.Trim()) + "|" +
                            String.Format("{0,10}", lblNewlblMasterWeightRange.Text.Trim()) + "=> <b>" +
                            txtNewInternational.Text.Trim() + "</b><br>";
                            args.IsValid = false;
                        }
                        else if (!(isValidPercentage(txtNewInternational.Text))) //Check it should max allow 2 decimals and less than 100 [08FEB12HN]Bug ID : 1132
                        {
                            strError = strError + "*" + String.Format("{0,30}", lblNewOrigin.Text.Trim()) + "|" +
                                String.Format("{0,10}", lblNewShipType.Text.Trim()) + "|" +
                                String.Format("{0,30}", lblNewlblMasterServiceName.Text.Trim()) + "|" +
                                String.Format("{0,10}", lblNewlblMasterWeightRange.Text.Trim()) + " => <b>" +
                                txtNewInternational.Text.Trim() + "</b><br>";
                            args.IsValid = false;
                        }
                    }

                }


                if (!(args.IsValid))
                {
                    val_PublicTariff.ErrorMessage = strError;
                }


            }
        }

        //To fix bug 1091 [03FEB12RM]
        protected void ddlTariffReference_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* 1. Fill public tariff grid */
            if (ddlTariffReference.SelectedIndex != -1)
            {
                KaizosServiceAgent proxy = new KaizosServiceAgent();

                List<SPublicTariffSearchResult> sPublicTariffSearchResult = proxy.GetPublicTariff(ddlTariffReference.SelectedValue);

                if (sPublicTariffSearchResult.Count != 0)
                {
                    gv_PublicTariff.DataSource = sPublicTariffSearchResult;
                    gv_PublicTariff.DataBind();
                }
            }
        }

        protected void gv_PublicTariff_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //To fix 1181 
            if (e.Row.RowType == DataControlRowType.Header) 
            {
                e.Row.Cells[7].CssClass = "hiddencol";
                e.Row.Cells[8].CssClass = "hiddencol";
                e.Row.Cells[9].CssClass = "hiddencol"; //24feb12rm moved from 1st to last column
            
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderGridRow =new GridViewRow(0, 0, DataControlRowType.Header,DataControlRowState.Insert);
                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "";
                HeaderCell.ColumnSpan = 4;
                HeaderCell.CssClass = "gridPublicTariffRow";
                HeaderGridRow.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = GetGlobalResourceObject("LocalString", "GrossMarginPercentage").ToString(); //"Kaizos Purchase tariffs";
                HeaderCell.ColumnSpan = 3;
                HeaderCell.CssClass = "gridPublicTariffRow";
                HeaderGridRow.Cells.Add(HeaderCell);

                gv_PublicTariff.Controls[0].Controls.AddAt(0, HeaderGridRow);

            }

        }

        protected string FormatCaption(string code)
        {
            string result = code;
            if (code.Trim().ToUpper().Equals("DOM"))
                return GetGlobalResourceObject("LocalString", "DOMCaption").ToString(); //"France";
            else if (code.Trim().ToUpper().Equals("COUNTRYX"))
                return GetGlobalResourceObject("LocalString", "COUNRTYXCaption").ToString();  //"Country X";
            return result;
        }
    }
}