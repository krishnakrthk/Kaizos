using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.ServiceModel;
using System.ServiceModel.Channels;

using log4net;
using log4net.Config;

using KaizosServiceInvokers.KaizosServiceReference;
 
namespace Kaizos
{
    public partial class frmAuthorizedUserList : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmAuthorizedUserList));
        //int irow = 0;

        public class UserList
        {
            public string AccountNo { get; set; }
            public string ContactName { get; set; }
            public string EmailId { get; set; }
            public string TelephoneNo { get; set; }
            public string UserType { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (KaizosSession.Current.UserType == "RF")
                    {
                        KaizosServiceContractClient proxy = new KaizosServiceContractClient();

                        List<SAuthorized> sAuthorized = new List<SAuthorized>();
                        SCustomer sCustomer = new SCustomer();
                        List<UserList> uList = new List<UserList>();
                        List<UserList> sortedUList = new List<UserList>();
                        UserList user = new UserList();
                        sCustomer = proxy.GetCustomer(KaizosSession.Current.UserId.Trim());

                        user.AccountNo = sCustomer.AccountNo.Trim();
                        user.ContactName = sCustomer.Name.Trim();
                        ViewState["CompanyName"] = sCustomer.CompanyName.Trim(); 
                        user.EmailId = sCustomer.Email.Trim();
                        user.TelephoneNo = sCustomer.TelephoneNo.Trim();
                        user.UserType = sCustomer.UserType == SEnumUserType.Referent ? "Y" : "N";
                        uList.Add(user);

                        sAuthorized = proxy.GetAuthorizedList(KaizosSession.Current.AccountNo.Trim()).ToList();

                        for (int i = 0; i < sAuthorized.Count; i++)
                        {
                            UserList Auser = new UserList();
                            Auser.AccountNo = sAuthorized[i].AccountNo.Trim();
                            Auser.ContactName = sAuthorized[i].ContactName.Trim();
                            Auser.EmailId = sAuthorized[i].Email.Trim();
                            Auser.TelephoneNo = sAuthorized[i].TelephoneNo.Trim();
                            Auser.UserType = "N";
                            uList.Add(Auser);
                        }
                        ViewState["SortDirection"] = "ASC";
                        ViewState["SortExpression"] = "ContactName";
                       //List<UserList> sortedUList = new List<UserList>();
                        if (uList.Count > 2)
                        {
                            if (GetSortDirection("ContactName").Equals("ASC"))
                            {


                                sortedUList = uList.OrderByDescending(u => u.UserType).ThenBy(u => u.ContactName).ToList();
                            }
                            else
                            {
                                sortedUList = uList.OrderByDescending(u => u.UserType).ThenByDescending(u => u.ContactName).ToList();
                            }
                        }
                        gv_AZL.DataSource = sortedUList.ToArray();
                        gv_AZL.DataBind();
                        Session["DictatedDV"] = gv_AZL;
                        // Save new values in ViewState.

                    }
                    else
                    {
                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MessageNotAuthorize").ToString();
                        Server.Transfer("frmResult.aspx", false);
                    }
                }
                /* Introduced faultexception handling and logging detailed exception into log4net file [27JAN12SM] */
                catch (FaultException<SGeneralFault> ex)
                {
                    string ErrorDetails = ex.Detail.Details;
                    string MethodName = ex.Detail.Issue;


                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Service, MethodName, ErrorDetails);
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "AZUserListLoadFailure").ToString();
                    Server.Transfer("frmResult.aspx", false);

                }
                catch (Exception error)
                {
                    /* Generalized exception handling and logging detailed exception into log4net file [27JAN12SM] */
                    if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                    {
                        string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId.Trim(), ErrorSource.Client, "Page_Load", ErrorLog.ExtractError(error));
                        logger.Debug(ErrMsg);

                        KaizosSession.Current.ReturnURL = "frmWelcomePage.aspx";
                        KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "AZUserListLoadFailure").ToString();
                        Server.Transfer("frmResult.aspx", false);

                    }
                }
            }
        }
        // 21-Feb -2012 HV Bug 1154
        private static int CompareASCUser(UserList x, UserList y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater. 
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (x.UserType.Equals("Y"))
                {
                    return -1;
                }
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the 
                    // lengths of the two strings.
                    //
                    int retval = x.ContactName.CompareTo(y.ContactName);
                    return retval;
                }
            }
        }
        // 21-Feb -2012 HV Bug 1154
        private static int CompareDescUser(UserList x, UserList y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater. 
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (x.UserType.Equals("Y"))
                {
                    return -1;
                }
                if (y == null)
               
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the 
                    // lengths of the two strings.
                    //
                    int retval = y.ContactName.CompareTo(x.ContactName);
                    return retval;
                }
            }
        }
        // 21-Feb -2012 HV Bug 1154
        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        protected void gv_AZL_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {

        }

        protected void gv_AZL_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void gv_AZL_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "GoEdit")
            {
                GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;

                string AccountNo = row.Cells[0].Text;
                string Name = row.Cells[1].Text;
                string Email = row.Cells[2].Text;
                string PhoneNo = row.Cells[3].Text;
                KaizosSession.Current.AZName = ViewState["CompanyName"].ToString();
                KaizosSession.Current.AZEmail = Email;
                KaizosSession.Current.AZPhoneNo = PhoneNo;
                KaizosSession.Current.AZAccountNo =AccountNo; 
               // Server.Transfer("frmAuthorizedUpdate.aspx?Name=" + Name + "&Email=" + Email + "&PhoneNo=" + PhoneNo + "&AccountNo=" + AccountNo);
                Server.Transfer("frmAuthorizedUpdate.aspx");
            }
        }

        protected void gv_AZL_RowCreated(object sender, GridViewRowEventArgs e)
        {
        }

        // 21-Feb -2012 HV Bug 1154
        protected void gv_AZL_Sorting(object sender, GridViewSortEventArgs e)
        {
            // 21-Feb -2012 HV Bug 1154
            KaizosServiceContractClient proxy = new KaizosServiceContractClient();

            List<SAuthorized> sAuthorized = new List<SAuthorized>();
            SCustomer sCustomer = new SCustomer();
            List<UserList> uList = new List<UserList>();
            UserList user = new UserList();
            sCustomer = proxy.GetCustomer(KaizosSession.Current.UserId.Trim());

            user.AccountNo = sCustomer.AccountNo.Trim();
            user.ContactName = sCustomer.Name.Trim();
            user.EmailId = sCustomer.Email.Trim();
            user.TelephoneNo = sCustomer.TelephoneNo.Trim();
            user.UserType = sCustomer.UserType == SEnumUserType.Referent ? "Y" : "N";

            uList.Add(user);

            sAuthorized = proxy.GetAuthorizedList(KaizosSession.Current.AccountNo.Trim()).ToList();

            for (int i = 0; i < sAuthorized.Count; i++)
            {
                UserList Auser = new UserList();
                Auser.AccountNo = sAuthorized[i].AccountNo.Trim();
                Auser.ContactName = sAuthorized[i].ContactName.Trim();
                Auser.EmailId = sAuthorized[i].Email.Trim();
                Auser.TelephoneNo = sAuthorized[i].TelephoneNo.Trim();
                Auser.UserType = "N";
                uList.Add(Auser);
            }
             List<UserList> sortedUList = new List<UserList>();
            if (uList.Count > 2)
            {
                if (GetSortDirection("ContactName").Equals("ASC"))
                {


                    sortedUList = uList.OrderByDescending(u => u.UserType).ThenBy(u => u.ContactName).ToList();
                }
                else
                {
                    sortedUList = uList.OrderByDescending(u => u.UserType).ThenByDescending(u => u.ContactName).ToList();
                }
            }
            gv_AZL.DataSource = sortedUList.ToArray();
            gv_AZL.DataBind();
        }

        protected void BindData(string sortExpression, string SortDirection)
        {
        }

        protected void gv_AZL_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //  if (irow == 0)
                //{
                e.Row.Cells[6].Visible = false;
                Button btnEdit = e.Row.FindControl("dlRow") as Button;
                Image imgUser = e.Row.FindControl("imgEdit") as Image;
                if (e.Row.Cells[6].Text.Equals("Y"))
                {

                    btnEdit.Visible = false;
                    imgUser.Visible = true;

                }
                else
                {
                    imgUser.Visible = false;
                }
                // irow = 1;
                // }
            }
        }
       
    }
}