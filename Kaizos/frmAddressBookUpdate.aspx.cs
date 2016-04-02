using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.ServiceModel;
using System.ServiceModel.Channels;
using KaizosServiceInvokers.KaizosServiceReference;


using log4net;
using log4net.Config;

namespace Kaizos
{
    public partial class frmAddressBookUpdate : System.Web.UI.Page
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmAddressBookUpdate));

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmAddressBookUpdate").ToString();
            }
            if (KaizosSession.Current.UserType.Trim() == "CS")
            {
                KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageNotAuthorize").ToString();
                Server.Transfer("frmResult.aspx", false);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;
            try
            {
                if (IsValid)
                {
                    KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                    List<SAddressBook> sAddressList;
                    sAddressList = proxy.GetAddressBookSearch(txtSearchName.Text.Trim(), "B").ToList();
                    gvAddressBookList.DataSource = sAddressList.ToArray();
                    gvAddressBookList.DataBind();
                }
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [23JAN12SM] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "AddressBookListFailure").ToString();
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [23JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "btnSearch_Click", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "AddressBookListFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }

        protected void val_AddressBook_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            string strError = "";
            if (txtSearchName.Text.Equals(""))
            {
                strError = strError + "*" + lblSearchName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
                args.IsValid = false;
            }

            if (!(args.IsValid))
            {
                val_AddressBook.ErrorMessage = strError;
            }

        }

        protected void gvAddressBookList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvAddressBookList.EditIndex = e.NewEditIndex;
                FillGridView();
            }
            catch (Exception error)
            {
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string userName = User.Identity.Name;
                    string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                    logger.Debug(errorMessage);
                }

            }
        }

        protected void FillGridView()
        {
            try
            {
                 KaizosServiceContractClient proxy = new KaizosServiceContractClient();
                 List<SAddressBook> sAddressList;
                 sAddressList = proxy.GetAddressBookSearch(txtSearchName.Text.Trim(), "B").ToList();
                 gvAddressBookList.DataSource = sAddressList.ToArray();
                 gvAddressBookList.DataBind();
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [23JAN12SM] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;


                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "AddressBookListFailure").ToString();
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [23JAN12SM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "btnSearch_Click", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "AddressBookListFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
            }

        }


        public bool validation(tempaddress temp , string header)
        {
            bool result = true;
            KaizosServiceContractClient context = new KaizosServiceContractClient();
            string[] Headerarry = header.Split('!');

            string strError = "";
            if (temp.AddressType == SEnumAddressType.Company)
            {
                if (temp.CompanyName.Trim() == "")
                {
                    strError = strError + "*" + Headerarry[0] + " " + valEmpty.Text.Trim() + "<br>";
                    result = false;  
                }
                else 
                {
                    if (!(context.isAlphaNumericValidation(temp.CompanyName.Trim())))
                    {
                        strError = strError + "*" + Headerarry[0] + " " + valInvalid.Text.Trim() + "<br>";
                         result = false;  
                    }
                }
           }

               if (temp.Name.Trim().Equals(""))
                {
                    strError = strError + "*" + Headerarry[1] + " " + valEmpty.Text.Trim() + "<br>";
                      result = false;  
                }
                else
                {
                    if (!(context.isAlphaNumericValidation((temp.Name.Trim()))))
                    {
                        strError = strError + "*" + Headerarry[1] + " " + valInvalid.Text.Trim() + "<br>";
                          result = false;  
                    }
                }

                if (temp.Address1.Equals(""))
                    {
                        strError = strError + "*" + Headerarry[2] + " " + valEmpty.Text.Trim() + "<br>";
                        result = false;  
                    }

                    if (temp.ZipCode.Trim().Equals(""))
                    {
                        strError = strError + "*" + Headerarry[3] + " " + valEmpty.Text.Trim() + "<br>";
                        result = false;  
                    }

                    if (temp.City.Trim().Equals(""))
                    {
                        strError = strError + "*" + Headerarry[4] + " " + valEmpty.Text.Trim() + "<br>";
                        result = false;  
                    }
                    else
                    {
                        if (!(context.isAlphaNumericValidation(temp.City.Trim())))
                        {
                            strError = strError + "*" +Headerarry[4] + " " + valInvalid.Text.Trim() + "<br>";
                            result = false;  
                        }
                    }

                    if (!temp.State.Equals(""))
                    {
                        if (!(context.isAlphaNumericValidation(temp.State.Trim())))
                        {
                            strError = strError + "*" + Headerarry[5]  + " " + valInvalid.Text.Trim() + "<br>";
                            result = false;  
                        }
                    }


            
                if (temp.Country.Trim().Equals(""))
                {
                    strError = strError + "*" + Headerarry[6] + " " + valEmpty.Text.Trim() + "<br>";
                    result = false;  
                }

                if (temp.Email.Trim().Length != 0)
                {
                    if (context.ValidateEmail(temp.Email.Trim()) != 0)
                    {
                        strError = strError + "*" +Headerarry[7] + " " + valInvalid.Text.Trim() + "<br>";
                        result = false;  
                    }
                }

                if (!temp.FaxNo.Trim().Equals(""))
                {
                    if (!(context.isNumericValidation(temp.FaxNo.Trim(), System.Globalization.NumberStyles.Integer)))
                    {
                        strError = strError + "*" + Headerarry[8] + " " + valInvalid.Text.Trim() + "<br>";
                        result = false;  
                    }
                }

                if (temp.TelephoneNo.Trim().Equals(""))
                {
                    strError = strError + "*" + Headerarry[9] + " " + valEmpty.Text.Trim() + "<br>";
                    result = false;  
                }
                else
                {
                    if (!(context.isNumericValidation(temp.TelephoneNo.Trim(), System.Globalization.NumberStyles.Integer)))
                    {
                        strError = strError + "*" + Headerarry[9].Trim() + " " + valInvalid.Text.Trim() + "<br>";
                        result = false;  
                    }
                }

                if (temp.LastPickupMondayToThursday.Trim().Equals(""))
                {
                    strError = strError + "*" + Headerarry[10].Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    result = false;  
                }
                else if (context.ValidateTime(temp.LastPickupMondayToThursday.Trim()) != 0)
                {
                    strError = strError + "*" + Headerarry[10].Trim() + " " + valInvalidTimeFormat.Text.Trim() + "<br>";
                    result = false;  
                }

                if (temp.LastPickupFriday.Equals(""))
                {
                    strError = strError + "*" + Headerarry[11].Trim() + " " + valEmpty.Text.Trim() + "<br>";
                    result = false;  
                }
                else if (context.ValidateTime(temp.LastPickupFriday.Trim()) != 0)
                {
                    strError = strError + "*" + Headerarry[11].Trim() + " " + valInvalidTimeFormat.Text.Trim() + "<br>";
                    result = false;  
                }


                lblError.Text = strError;
                if (result == false)
                {
                    lblError.Visible = true;
                }
                else
                    lblError.Visible = false;

                  return result;
        }


        protected void gvAddressBookList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                    int result = 1;

                    string Header = gvAddressBookList.HeaderRow.Cells[0].Text + "!" + gvAddressBookList.HeaderRow.Cells[1].Text + "!" + gvAddressBookList.HeaderRow.Cells[5].Text+"!";
                    Header = Header + gvAddressBookList.HeaderRow.Cells[8].Text + "!" + gvAddressBookList.HeaderRow.Cells[9].Text + "!" + gvAddressBookList.HeaderRow.Cells[10].Text + "!";
                    Header = Header + gvAddressBookList.HeaderRow.Cells[11].Text + "!" + gvAddressBookList.HeaderRow.Cells[12].Text + "!" + gvAddressBookList.HeaderRow.Cells[4].Text + "!";
                    Header = Header + gvAddressBookList.HeaderRow.Cells[3].Text + "!" + gvAddressBookList.HeaderRow.Cells[13].Text + "!" + gvAddressBookList.HeaderRow.Cells[14].Text + "!" + gvAddressBookList.HeaderRow.Cells[16].Text + "!";

                    tempaddress sAddressBook = new tempaddress();

                    sAddressBook.AddressID = ((TextBox)gvAddressBookList.Rows[e.RowIndex].Cells[0].FindControl("txtAddressID")).Text.Trim();

                    if (((DropDownList)gvAddressBookList.Rows[e.RowIndex].Cells[1].FindControl("ddlAddressType")).SelectedValue.Trim() == "Company")
                        sAddressBook.AddressType = SEnumAddressType.Company;
                    else
                        sAddressBook.AddressType = SEnumAddressType.Residential;

                    sAddressBook.CompanyName = ((TextBox)gvAddressBookList.Rows[e.RowIndex].Cells[2].FindControl("txtCompanyName")).Text.Trim();
                    sAddressBook.Name = ((TextBox)gvAddressBookList.Rows[e.RowIndex].Cells[3].FindControl("txtName")).Text.Trim();
                    sAddressBook.TelephoneNo = ((TextBox)gvAddressBookList.Rows[e.RowIndex].Cells[4].FindControl("txtTelephoneNo")).Text.Trim();
                    sAddressBook.FaxNo = ((TextBox)gvAddressBookList.Rows[e.RowIndex].Cells[5].FindControl("txtFaxNo")).Text.Trim();
                    sAddressBook.Address1 = ((TextBox)gvAddressBookList.Rows[e.RowIndex].Cells[6].FindControl("txtAddress1")).Text.Trim();
                    sAddressBook.Address2 = ((TextBox)gvAddressBookList.Rows[e.RowIndex].Cells[7].FindControl("txtAddress2")).Text.Trim();
                    sAddressBook.Address3 = ((TextBox)gvAddressBookList.Rows[e.RowIndex].Cells[8].FindControl("txtAddress3")).Text.Trim();
                    sAddressBook.ZipCode = ((TextBox)gvAddressBookList.Rows[e.RowIndex].Cells[9].FindControl("txtZipcode")).Text.Trim();
                    sAddressBook.City = ((TextBox)gvAddressBookList.Rows[e.RowIndex].Cells[10].FindControl("txtCity")).Text.Trim();
                    sAddressBook.State = ((TextBox)gvAddressBookList.Rows[e.RowIndex].Cells[11].FindControl("txtState")).Text.Trim();
                    sAddressBook.Country = ((DropDownList)gvAddressBookList.Rows[e.RowIndex].Cells[12].FindControl("ddlCountry")).SelectedValue.Trim();
                    sAddressBook.Email = ((TextBox)gvAddressBookList.Rows[e.RowIndex].Cells[13].FindControl("txtEmail")).Text.Trim();
                    sAddressBook.LastPickupMondayToThursday = ((TextBox)gvAddressBookList.Rows[e.RowIndex].Cells[14].FindControl("txtLastPickupMondayToThursday")).Text.Trim();
                    sAddressBook.LastPickupFriday = ((TextBox)gvAddressBookList.Rows[e.RowIndex].Cells[15].FindControl("txtLastPickupFriday")).Text.Trim();
                    sAddressBook.Comments = ((TextBox)gvAddressBookList.Rows[e.RowIndex].Cells[16].FindControl("txtComments")).Text.Trim();

                    if (((DropDownList)gvAddressBookList.Rows[e.RowIndex].Cells[18].FindControl("ddlShipPreference")).SelectedValue.Trim() == "Fastest")
                        sAddressBook.ShipPreference = SEnumShipPreference.Fastest;
                    else if (((DropDownList)gvAddressBookList.Rows[e.RowIndex].Cells[18].FindControl("ddlShipPreference")).SelectedValue.Trim() == "MostCompetitive")
                        sAddressBook.ShipPreference = SEnumShipPreference.MostCompetitive;
                    else
                        sAddressBook.ShipPreference = SEnumShipPreference.NamedCarrier;

                    sAddressBook.AddressUsedFor = SEnumDeliveryType.Both;
                    
                    sAddressBook.LastUpdated = DateTime.Now;
                    sAddressBook.AccountNo = KaizosSession.Current.AccountNo;
                    
                if(validation(sAddressBook,Header))
                {
                    result= UpdatingAddressBook(sAddressBook);
                    gvAddressBookList.EditIndex = -1;
                    FillGridView();
                }  
                    
                   
                    /*cancel edit mode */
                    
            }
            catch (Exception error)
            {
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string userName = User.Identity.Name;
                    string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                    logger.Debug(errorMessage);
                   
                }

            }
        }

        

        //protected void val_AddressUpdateBook_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    args.IsValid = true;
        //    string strError = "";
        //    KaizosServiceContractClient context = new KaizosServiceContractClient();
        //    tempaddress ss = new tempaddress();
        //    try
        //    {
        //        //if (!optResidential.Checked)
        //        //{
        //            if (ss.CompanyName.Equals(""))
        //            {
        //                strError = strError + "*" +"company"+ " " + valEmpty.Text.Trim() + "<br>";
        //                args.IsValid = false;
        //            }
        //            else
        //            {
        //               if (!(context.isAlphaNumericValidation(ss.CompanyName.Trim())))
        //                {
        //                    strError = strError + "*" + "company" + " " + valInvalid.Text.Trim() + "<br>";
        //                    args.IsValid = false;
        //                }
        //            }
        //            #region valid
        //            // }

        //        //if (txtName.Text.Equals(""))
        //        //{
        //        //    strError = strError + "*" + lblName.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
        //        //    args.IsValid = false;
        //        //}

        //        //else
        //        //{
        //        //    if (!(context.isAlphaNumericValidation(txtName.Text.Trim())))
        //        //    {
        //        //        strError = strError + "*" + lblName.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
        //        //        args.IsValid = false;
        //        //    }
        //        //}

        //        //if (txtAddress1.Text.Equals(""))
        //        //{
        //        //    strError = strError + "*" + lblAddress1.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
        //        //    args.IsValid = false;
        //        //}

        //        //if (txtZipcode.Text.Equals(""))
        //        //{
        //        //    strError = strError + "*" + lblZipcode.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
        //        //    args.IsValid = false;
        //        //}

        //        //if (txtCity.Text.Equals(""))
        //        //{
        //        //    strError = strError + "*" + lblCity.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
        //        //    args.IsValid = false;
        //        //}
        //        //else
        //        //{
        //        //    if (!(context.isAlphaNumericValidation(txtCity.Text.Trim())))
        //        //    {
        //        //        strError = strError + "*" + lblCity.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
        //        //        args.IsValid = false;
        //        //    }
        //        //}

        //        //if (!txtState.Text.Equals(""))
        //        //{
        //        //    if (!(context.isAlphaNumericValidation(txtState.Text.Trim())))
        //        //    {
        //        //        strError = strError + "*" + lblState.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
        //        //        args.IsValid = false;
        //        //    }
        //        //}

        //        //if (ddlCountry.Text.Equals(""))
        //        //{
        //        //    strError = strError + "*" + lblCountry.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
        //        //    args.IsValid = false;
        //        //}

        //        //if (txtEmail.Text.Trim().Length != 0)
        //        //{
        //        //    if (context.ValidateEmail(txtEmail.Text.Trim()) != 0)
        //        //    {
        //        //        strError = strError + "*" + lblEmail.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
        //        //        args.IsValid = false;
        //        //    }
        //        //}

        //        //if (!txtFaxNumber.Text.Equals(""))
        //        //{
        //        //    if (!(context.isNumericValidation(txtFaxNumber.Text.Trim(), System.Globalization.NumberStyles.Integer)))
        //        //    {
        //        //        strError = strError + "*" + lblFaxNumber.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
        //        //        args.IsValid = false;
        //        //    }
        //        //}

        //        //if (txtPhoneNumber.Text.Equals(""))
        //        //{
        //        //    strError = strError + "*" + lblPhoneNumber.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
        //        //    args.IsValid = false;
        //        //}
        //        //else
        //        //{
        //        //    if (!(context.isNumericValidation(txtPhoneNumber.Text.Trim(), System.Globalization.NumberStyles.Integer)))
        //        //    {
        //        //        strError = strError + "*" + lblPhoneNumber.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
        //        //        args.IsValid = false;
        //        //    }
        //        //}

        //        //if (txtLastPickupMT.Text.Equals(""))
        //        //{
        //        //    strError = strError + "*" + lblLastPickupMT.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
        //        //    args.IsValid = false;
        //        //}
        //        //else if (context.ValidateTime(txtLastPickupMT.Text.Trim()) != 0)
        //        //{
        //        //    strError = strError + "*" + lblLastPickupMT.Text.Trim() + " " + valInvalidTimeFormat.Text.Trim() + "<br>";
        //        //    args.IsValid = false;
        //        //}

        //        //if (txtLastPickupF.Text.Equals(""))
        //        //{
        //        //    strError = strError + "*" + lblLastPickupF.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
        //        //    args.IsValid = false;
        //        //}
        //        //else if (context.ValidateTime(txtLastPickupF.Text.Trim()) != 0)
        //        //{
        //        //    strError = strError + "*" + lblLastPickupF.Text.Trim() + " " + valInvalidTimeFormat.Text.Trim() + "<br>";
        //        //    args.IsValid = false;
        //        //}

        //        ////if (chkEnableShippingPreference.Checked)
        //        ////{
        //        ////    if (optShipPreference3.Checked)
        //        ////    {
        //        ////        if (txtNamedCarrier.Text.Equals(""))
        //        ////        {
        //        ////            strError = strError + "*" + lblNamedCarrier.Text.Trim() + " " + valEmpty.Text.Trim() + "<br>";
        //        ////            args.IsValid = false;
        //        ////        }
        //        ////    }
        //        ////}
        //         #endregion
        //        if (!(args.IsValid))
        //        {
        //            val_AddressBook.ErrorMessage = strError;
        //        }
        //    }
        //    /* Introduced faultexception handling and logging detailed exception into log4net file [23JAN12SM] */
        //    catch (FaultException<SGeneralFault> ex)
        //    {
        //        string ErrorDetails = ex.Detail.Details;
        //        string MethodName = ex.Detail.Issue;


        //        string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
        //        logger.Debug(ErrMsg);

        //        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
        //        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "AddressBookValidationFailure").ToString();
        //        Server.Transfer("frmResult.aspx", false);

        //    }
        //    catch (Exception error)
        //    {
        //        /* Generalized exception handling and logging detailed exception into log4net file [05JAN12RM] */
        //        if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
        //        {
        //            string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "val_AddressBook_ServerValidate", ErrorLog.ExtractError(error));
        //            logger.Debug(ErrMsg);

        //            KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
        //            KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "AddressBookValidationFailure").ToString();
        //            Server.Transfer("frmResult.aspx", false);
        //        }
        //    }
        //}


        protected void gvAddressBookList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            lblError.Visible = false;
            gvAddressBookList.EditIndex = -1;
            FillGridView();
        }

        protected void gvAddressBookList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            KaizosServiceContractClient proxy = new KaizosServiceContractClient();
            if (gvAddressBookList.EditIndex == -1 && e.Row.RowType == DataControlRowType.DataRow)
            {
                
                CheckBox chkBox = (CheckBox)e.Row.FindControl("chkEnableShippingPreferenceItem");
              //CheckBox chkBox = (CheckBox)e.Row.FindControl("chkEnableShippingPreferenceItem");
                if (((Label)e.Row.Cells[18].FindControl("lblEnableShipPreference")).Text.Trim().Equals(string.Empty))
                {
                    chkBox.Checked = true;
                }

                AjaxControlToolkit.ReorderList rlShippingPreferenceItem = (AjaxControlToolkit.ReorderList)e.Row.Cells[19].FindControl("rlShippingPreference");


                List<ShippingPreference> lstShippingPreferance = new List<ShippingPreference>();
                ShippingPreference sp = new ShippingPreference();
                sp.Id = 1;
                sp.ShippingPreferenceType = "the most competitive";
                sp.priority = 1;
                lstShippingPreferance.Add(sp);
                sp = new ShippingPreference();
                sp.Id = 2;
                sp.priority = 2;
                sp.ShippingPreferenceType = "the fastest";
                lstShippingPreferance.Add(sp);
                sp = new ShippingPreference();
                sp.Id = 3;
                sp.ShippingPreferenceType = "named carrier";
                sp.priority = 3;
                lstShippingPreferance.Add(sp);
                rlShippingPreferenceItem.DataSource = lstShippingPreferance;
                rlShippingPreferenceItem.DataBind();

                if (chkBox.Checked)
                {
                    rlShippingPreferenceItem.Visible = true;
                }
                else
                {
                    rlShippingPreferenceItem.Visible = false;

                }
            }
            if (gvAddressBookList.EditIndex == e.Row.RowIndex && e.Row.RowType == DataControlRowType.DataRow)
            {

                // Address Type
                DropDownList ddlAddressType = (DropDownList)e.Row.Cells[1].FindControl("ddlAddressType");
                ddlAddressType.Items.Add("Company");
                ddlAddressType.Items.Add("Residential");
                ddlAddressType.SelectedValue = ((Label)e.Row.Cells[1].FindControl("lblAddressType1")).Text.Trim();

                // Country code
                List<SCountryTable> sCountryTable = new List<SCountryTable>();
                DropDownList ddlCountry = (DropDownList)e.Row.Cells[12].FindControl("ddlCountry");
                sCountryTable = proxy.FillCountryCombo().ToList();
                ddlCountry.DataSource = sCountryTable;
                ddlCountry.DataTextField = "CodeName";
                ddlCountry.DataValueField = "CountryCode";
                ddlCountry.DataBind();
                ddlCountry.SelectedValue = ((Label)e.Row.Cells[12].FindControl("lblCountry1")).Text.Trim();

                //Shipping Preference
                DropDownList ddlShipPreference = (DropDownList)e.Row.Cells[18].FindControl("ddlShipPreference");
                ddlShipPreference.Items.Add("Fastest");
                ddlShipPreference.Items.Add("MostCompetitive");
                ddlShipPreference.Items.Add("NamedCarrier");
                ddlShipPreference.SelectedValue = ((Label)e.Row.Cells[18].FindControl("lblShipPreference1")).Text.Trim();

                CheckBox chkBox = (CheckBox)e.Row.FindControl("chkEnableShippingPreferenceEdit");
                if (((Label)e.Row.Cells[18].FindControl("lblShipPreference1")).Text.Trim().Equals(string.Empty))
                {
                    chkBox.Checked = true;
                }
                    AjaxControlToolkit.ReorderList rlShippingPreferenceItem = (AjaxControlToolkit.ReorderList)e.Row.Cells[19].FindControl("rlShippingPreferenceEdit");

                
                List<ShippingPreference> lstShippingPreferance = new List<ShippingPreference>();
                ShippingPreference sp = new ShippingPreference();
                sp.Id = 1;
                sp.ShippingPreferenceType = "the most competitive";
                sp.priority = 1;
                lstShippingPreferance.Add(sp);
                sp = new ShippingPreference();
                sp.Id = 2;
                sp.priority = 2;
                sp.ShippingPreferenceType = "the fastest";
                lstShippingPreferance.Add(sp);
                sp = new ShippingPreference();
                sp.Id = 3;
                sp.ShippingPreferenceType = "named carrier";
                sp.priority = 3;
                lstShippingPreferance.Add(sp);
                rlShippingPreferenceItem.DataSource = lstShippingPreferance;
                rlShippingPreferenceItem.DataBind();
                rlShippingPreferenceItem.Visible = false;
                if (chkBox.Checked)
                {
                    rlShippingPreferenceItem.Visible = true;
                }
                else
                {
                    rlShippingPreferenceItem.Visible = false;

                }
            }
        }

        protected void gvAddressBookList_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
        }

        protected void gvAddressBookList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int result = 1;
            KaizosServiceContractClient proxy = new KaizosServiceContractClient();
            result = proxy.DeleteAddress(((Label)gvAddressBookList.Rows[e.RowIndex].Cells[0].FindControl("lblAddressID")).Text.Trim());
            /*cancel edit mode */
            gvAddressBookList.EditIndex = -1;
            FillGridView();
        }


        protected void chkEnableShippingPreference_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            GridViewRow row = checkBox.NamingContainer as GridViewRow;
            AjaxControlToolkit.ReorderList rlShippingPreferenceItem = (AjaxControlToolkit.ReorderList)row.Cells[19].FindControl("rlShippingPreferenceEdit");
            if (checkBox.Checked)
            {
                rlShippingPreferenceItem.Visible = true;

            }
            else
            {
                rlShippingPreferenceItem.Visible = false;
            }

        }


        public int UpdatingAddressBook(tempaddress temp)
        {
            SAddressBook sAddressBook = new SAddressBook();

            sAddressBook.AddressID = temp.AddressID;
            sAddressBook.AddressType = temp.AddressType;

            sAddressBook.CompanyName = temp.CompanyName;
            sAddressBook.Name = temp.Name;
            sAddressBook.TelephoneNo = temp.TelephoneNo;
            sAddressBook.FaxNo = temp.FaxNo;
            sAddressBook.Address1 = temp.Address1;
            sAddressBook.Address2 = temp.Address2;
            sAddressBook.Address3 = temp.Address3;
            sAddressBook.ZipCode = temp.ZipCode;
            sAddressBook.City = temp.City;
            sAddressBook.State = temp.State;
            sAddressBook.Country = temp.Country;
            sAddressBook.Email = temp.Email;
            sAddressBook.LastPickupMondayToThursday = temp.LastPickupMondayToThursday;
            sAddressBook.LastPickupFriday = temp.LastPickupFriday;
            sAddressBook.Comments = temp.Comments;
            sAddressBook.ShipPreference = temp.ShipPreference;
            sAddressBook.AddressUsedFor = temp.AddressUsedFor;
            sAddressBook.LastUpdated = temp.LastUpdated;
            sAddressBook.AccountNo = temp.AccountNo;
            int result = 0;
            KaizosServiceContractClient proxy = new KaizosServiceContractClient();
            result = proxy.UpdateAddress(sAddressBook);
            return result;
        }

    }

    public class tempaddress
    {
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string TelephoneNo { get; set; }
        public string FaxNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string LastPickupMondayToThursday { get; set; }
        public string LastPickupFriday { get; set; }
        public string Comments { get; set; }
        public string AddressID { get; set; }
        public string AccountNo { get; set; }


        public DateTime LastUpdated { get; set; }
        public SEnumAddressType AddressType { get; set; }
        public SEnumShipPreference ShipPreference { get; set; }
         public SEnumDeliveryType AddressUsedFor { get; set; }
    } 
     
    

}