using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;

using log4net;
using log4net.Config;

using System.ServiceModel;

using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;

namespace Kaizos
{
    public partial class frmDeliveryDelayImport : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmDeliveryDelayImport));

        string strServiceID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmDeliveryDelayImport").ToString();             
                //strServiceID = Request.QueryString["ServiceID"]; //[06MAR12RM]
                strServiceID = KaizosSession.Current.ServiceID;
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {

                /* 1.create proxy */
                KaizosServiceAgent proxy = new KaizosServiceAgent();

                //string strImageName = "sample";
                if (FileUpload1.PostedFile != null && FileUpload1.PostedFile.FileName != "")
                {

                    byte[] imageSize = new byte[FileUpload1.PostedFile.ContentLength];

                    HttpPostedFile uploadedImage = FileUpload1.PostedFile;

                    uploadedImage.InputStream.Read(imageSize, 0, (int)FileUpload1.PostedFile.ContentLength);


                    //int result = proxy.ImportDeliveryDelay(Convert.ToInt32(strServiceID), imageSize);
                    List<SFileImportStatus> sFileImportStatus = proxy.ImportDeliveryDelay(Convert.ToInt32(KaizosSession.Current.ServiceID), imageSize);


                    string ImportResult = "";

                    if (sFileImportStatus.Count == 1)
                    {
                        ImportResult = sFileImportStatus[0].ErrorDescription;
                    }
                    else
                    {

                        //Imported successfully except following data
                        string Temp = "<h1>" + GetGlobalResourceObject("LocalString", "FileImportStatus").ToString() + "</h1> <br>";

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

                    string strReturnUrl = "frmMasterServiceTypeUpdate.aspx";
                    KaizosSession.Current.ErrorMessage = ImportResult;
                    KaizosSession.Current.ReturnURL = strReturnUrl;
                    Server.Transfer("frmResult.aspx", false);

                    //if (result == 0)
                    //    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "TariffImportSuccess").ToString();//"Imported Sucessfully";
                    //else
                    //    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "TariffImportFailure").ToString();//"Problem while importing";

                    //KaizosSession.Current.ReturnURL = "frmMasterServiceTypeUpdate.aspx";

                    //Server.Transfer("frmResult.aspx",false);

                }
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [18JAN12RM] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;

                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmMasterServiceTypeUpdate.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "TariffImportFailure").ToString();
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [18JAN12RM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "btnUpload_Click()", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmMasterServiceTypeUpdate.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "TariffImportFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);

                }
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
        }
    }
}