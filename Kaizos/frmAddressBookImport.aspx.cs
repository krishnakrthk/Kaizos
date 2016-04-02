using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using log4net;
using log4net.Config;

using System.ServiceModel;

using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;

namespace Kaizos
{
    public partial class frmAddressBookImport : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmAddressBookImport));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmAddressBookImport").ToString();
                if (KaizosSession.Current.UserType.Trim() == "CS")
                {
                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageNotAuthorize").ToString();
                    Server.Transfer("frmResult.aspx", false);
                }
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid)
                {
                    /* create proxy */
                    KaizosServiceAgent proxy = new KaizosServiceAgent();

                    if (FileUpload1.PostedFile != null && FileUpload1.PostedFile.FileName != "")
                    {
                        byte[] AddressBookFile = new byte[FileUpload1.PostedFile.ContentLength];

                        HttpPostedFile uploadedImage = FileUpload1.PostedFile;

                        uploadedImage.InputStream.Read(AddressBookFile, 0, (int)FileUpload1.PostedFile.ContentLength);

                        List<SFileImportStatus> sFileImportStatus = null;

                        string ImportResult = "";

                        sFileImportStatus = proxy.ImportAddressBook(AddressBookFile, KaizosSession.Current.AccountNo);

                        if (sFileImportStatus.Count == 1)
                        {
                            ImportResult = sFileImportStatus[0].ErrorDescription;
                        }
                        else
                        {
                            string Temp = "<h1>Imported successfully except following data</h1> <br>";
                            Temp = Temp + "<table border='1'>" +
                                            "<tr>" +
                                                "<td>" +
                                                   "Line #" +
                                                "</td>" +
                                                "<td>" +
                                                   "Field" +
                                                "</td>" +
                                                "<td>" +
                                                   "Error Description" +
                                                "</td>" +
                                            "</tr>";


                            for (int i = 1; i < sFileImportStatus.Count; i++)
                            {
                                Temp = Temp +

                                                "<tr>" +
                                                "<td>" +
                                                   sFileImportStatus[i].RowNumber.ToString() +
                                                "</td>" +
                                                "<td>" +
                                                   sFileImportStatus[i].FieldName +
                                                "</td>" +
                                                "<td>" +
                                                   sFileImportStatus[i].ErrorDescription +
                                                "</td>" +
                                            "</tr>";

                            }

                            Temp = Temp + "</table>";

                            ImportResult = Temp;
                        }

                        string strReturnUrl = "frmAddressBookList.aspx";
                        KaizosSession.Current.ErrorMessage = ImportResult;
                        KaizosSession.Current.ReturnURL = strReturnUrl;
                        Server.Transfer("frmResult.aspx", false);

                    }
                }
            }
                /* Introduced faultexception handling and logging detailed exception into log4net file [26JAN12SM] */
                catch (FaultException<SGeneralFault> ex)
                {
                    string ErrorDetails = ex.Detail.Details;
                    string MethodName = ex.Detail.Issue;


                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "AddressBookImportFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);

                }
                catch (Exception error)
                {
                    /* Generalized exception handling and logging detailed exception into log4net file [26JAN12SM] */
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "btnCreate_Click", ErrorLog.ExtractError(error));
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "AddressBookImportFailure").ToString();
                        Server.Transfer("frmResult.aspx", false);

                    }
                }
        
        }

        protected void val_AddressBookImport_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            string strError = "";
            KaizosServiceContractClient context = new KaizosServiceContractClient();
                
                if (System.IO.Path.GetExtension(FileUpload1.FileName) != ".csv")
                {
                    strError = strError + "*" + lblAddressBookFile.Text.Trim() + " " + valInvalid.Text.Trim() + "<br>";
                    args.IsValid = false;
                }

                if (!(args.IsValid))
                {
                    val_AddressBookImport.ErrorMessage = strError;
                }
        }
    }
}