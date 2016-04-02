using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using log4net;
using log4net.Config;

using System.ServiceModel;

using KaizosServiceInvokers.KaizosServiceReference;
using KaizosServiceInvokers;


namespace Kaizos
{

    public class MasterServiceUpdate
    {
        public string CarrierName { get; set; }

        /* Road */
        public int RoadServiceID { get; set; }
        public string RoadMasterServiceName { get; set; }
        public string RoadServiceName { get; set; }
        public string RoadServiceCode { get; set; }
        public string RoadDeliveryDelay { get; set; }
        public string RoadDeliveryDeadLine { get; set; }
        public bool RoadActive { get; set; }

        /* AirB410 */
        public int AirB410ServiceID { get; set; }
        public string AirB410MasterServiceName { get; set; }
        public string AirB410ServiceName { get; set; }
        public string AirB410ServiceCode { get; set; }
        public string AirB410DeliveryDelay { get; set; }
        public string AirB410DeliveryDeadLine { get; set; }
        public bool AirB410Active { get; set; }

        /* AirB412 */
        public int AirB412ServiceID { get; set; }
        public string AirB412MasterServiceName { get; set; }
        public string AirB412ServiceName { get; set; }
        public string AirB412ServiceCode { get; set; }
        public string AirB412DeliveryDelay { get; set; }
        public string AirB412DeliveryDeadLine { get; set; }
        public bool AirB412Active { get; set; }

        /* AirB418 */
        public int AirB418ServiceID { get; set; }
        public string AirB418MasterServiceName { get; set; }
        public string AirB418ServiceName { get; set; }
        public string AirB418ServiceCode { get; set; }
        public string AirB418DeliveryDelay { get; set; }
        public string AirB418DeliveryDeadLine { get; set; }
        public bool AirB418Active { get; set; }

    }

    public partial class frmMasterServiceTypeUpdate : BasePage
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmMasterServiceTypeUpdate));
        int intMSCounter = 0;
        protected void Page_Init(object sender, EventArgs e)
        {
            LoadCarrierServiceGrid(false, false);

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Page.Title = GetGlobalResourceObject("LocalString", "frmMasterServiceTypeUpdate").ToString();
                // LoadCarrierServiceGrid(false,false);



            }

        }

        protected List<String> GetMasterServiceName(List<SCarrierService> sCarrierService)
        {
            List<String> masterServiceList = new List<String>();

            for (int i = 0; i < sCarrierService.Count; i++)
            {
                bool bMasterServiceExists = masterServiceList.Any(s => s.Equals(sCarrierService[i].MasterServiceName.Trim(), StringComparison.OrdinalIgnoreCase));

                if (!bMasterServiceExists)
                {
                    masterServiceList.Add(sCarrierService[i].MasterServiceName.Trim());
                }
            }
            return masterServiceList;
        }

        protected List<String> GetCarrierList(List<SCarrierService> sCarrierService)
        {
            List<String> carrierList = new List<String>();

            for (int i = 0; i < sCarrierService.Count; i++)
            {
                bool bCarrierExists = carrierList.Any(s => s.Equals(sCarrierService[i].CarrierName.Trim(), StringComparison.OrdinalIgnoreCase));

                if (!bCarrierExists)
                {
                    carrierList.Add(sCarrierService[i].CarrierName.Trim());
                }
            }
            return carrierList;
        }

        protected DataTable FormatDataForGrid(List<SCarrierService> sCarrierService)
        {

            List<MasterServiceUpdate> masterServiceUpdate = new List<MasterServiceUpdate>();
            bool rowCreated = false;
            //Get carrier service list

            //Get Master Service List
            KaizosServiceAgent proxy = new KaizosServiceAgent();
            List<SCarrier> CarrierList = proxy.GetCarriers();
            List<SComboText> lstComboText = new List<SComboText>();
            List<SComboText> sComboText = new List<SComboText>();
            SComboTableField sComboTableField = new SComboTableField();
            sComboTableField.FieldName = "MASTER_SERVICE_NAME";
            sComboTableField.TableName = "MASTER_SERVICE";
            sComboText = proxy.FillCombo(sComboTableField).ToList();
            DataTable dtService = new DataTable();
            dtService.Columns.Add("ID");
            dtService.Columns.Add("Carrier");
            dtService.Columns.Add("FieldName");
            // int intMSCounter = 0;
            int ik = 1;
            foreach (SComboText objComboText in sComboText)
            {
                DataColumn dc = new DataColumn();
                dc.ColumnName = objComboText.ComboText;
                dc.Caption = "Service" + ik.ToString();

                if (sCarrierService.Count == 0)
                {
                    foreach (SCarrier sCarrier in CarrierList)
                    {
                        SCarrierService scarrierService = new SCarrierService();
                        scarrierService.ServiceID = 0;
                        scarrierService.CarrierName = sCarrier.CarrierName;
                        scarrierService.MasterServiceName = dc.ColumnName;
                        scarrierService.Priority = SEnumPriority.Economy;
                        scarrierService.ServiceName = "";
                        scarrierService.ServiceCode = "";
                        scarrierService.DeliveryDelayTable = "NoTable";
                        scarrierService.DeliveryDeadLine = "";
                        scarrierService.Active = SEnumFlag.No;
                        scarrierService.KeyCustomerService = SEnumFlag.No;
                        scarrierService.Information = "";
                        scarrierService.InfoType = "";
                        sCarrierService.Add(scarrierService);
                    }
                }
                else
                {

                    foreach (SCarrier sCarrier in CarrierList)
                    {
                        // Find a book by its ID.
                        SCarrierService result = sCarrierService.Find(
                        delegate(SCarrierService sk)
                        {
                            return sk.CarrierName == sCarrier.CarrierName;
                        }
                        );
                        if (result == null)
                        {
                            SCarrierService scarrierService = new SCarrierService();
                            scarrierService.ServiceID = 0;
                            scarrierService.CarrierName = sCarrier.CarrierName;
                            scarrierService.MasterServiceName = dc.ColumnName;
                            scarrierService.Priority = SEnumPriority.Economy;
                            scarrierService.ServiceName = "";
                            scarrierService.ServiceCode = "";
                            scarrierService.DeliveryDelayTable = "NoTable";
                            scarrierService.DeliveryDeadLine = "";
                            scarrierService.Active = SEnumFlag.No;
                            scarrierService.KeyCustomerService = SEnumFlag.No;
                            scarrierService.Information = "";
                            scarrierService.InfoType = "";
                            sCarrierService.Add(scarrierService);

                        }
                    }

                }


                dtService.Columns.Add(dc);
                intMSCounter = intMSCounter + 1;
                ik = ik + 1;
            }


            /* Loop Carrier Mast */
            int row = 0;
            int rowcountperservice = 0;
            int CurrentRow = 0;
            int temprow = 0;
            List<string> strMasterServiceNames = new List<string>();


            foreach (SCarrier sCarrier in CarrierList)
            {
                MasterServiceUpdate masterService = new MasterServiceUpdate();
                DataRow drServiceRow = dtService.NewRow();
                strMasterServiceNames = new List<string>();
                rowCreated = false;


                var resultService = from service in sCarrierService
                                    where service.CarrierName.ToUpper().Trim().Equals(sCarrier.CarrierName.ToUpper().Trim())
                                    select service;

                #region Populate Carrier Service
                if (!(resultService == null))
                {
                    // if(resultService.Count() == dtService.Columns.Count-1)

                    int temp = 0;
                    foreach (var result in resultService)
                    {


                        #region Column Looping

                        foreach (DataColumn dc in dtService.Columns)
                        {


                            #region Columnname = MasterService name
                            if (dc.ColumnName == result.MasterServiceName)
                            {
                                if (!strMasterServiceNames.Contains(dc.ColumnName))
                                {
                                    strMasterServiceNames.Add(dc.ColumnName);
                                    temp = temp + 1;
                                }


                                if (!rowCreated)
                                {
                                    CurrentRow = row;
                                    row = row + 1;
                                    drServiceRow = dtService.NewRow();
                                    drServiceRow["ID"] = row;
                                    drServiceRow["Carrier"] = result.CarrierName;
                                    drServiceRow["FieldName"] = "ServiceID";
                                    drServiceRow[dc.ColumnName] = result.ServiceID;
                                    dtService.Rows.Add(drServiceRow);

                                    row = row + 1;
                                    drServiceRow = dtService.NewRow();
                                    drServiceRow["ID"] = row;
                                    drServiceRow["Carrier"] = result.CarrierName;
                                    drServiceRow["FieldName"] = "ServiceCode";
                                    drServiceRow[dc.ColumnName] = result.ServiceCode;
                                    dtService.Rows.Add(drServiceRow);

                                    row = row + 1;
                                    drServiceRow = dtService.NewRow();
                                    drServiceRow["ID"] = row;
                                    drServiceRow["Carrier"] = string.Empty;// result.CarrierName;
                                    drServiceRow["FieldName"] = "ServiceName";
                                    drServiceRow[dc.ColumnName] = result.ServiceName;
                                    dtService.Rows.Add(drServiceRow);


                                    row = row + 1;
                                    drServiceRow = dtService.NewRow();
                                    drServiceRow["ID"] = row;
                                    drServiceRow["Carrier"] = string.Empty;// result.CarrierName;
                                    drServiceRow["FieldName"] = "Priority";
                                    drServiceRow[dc.ColumnName] = result.Priority;
                                    dtService.Rows.Add(drServiceRow);
                                    row = row + 1;
                                    drServiceRow = dtService.NewRow();
                                    drServiceRow["ID"] = row;
                                    drServiceRow["Carrier"] = string.Empty;// result.CarrierName;
                                    drServiceRow["FieldName"] = "Import";
                                    drServiceRow[dc.ColumnName] = result.DeliveryDelayTable;// "ImportDetails"; 
                                    dtService.Rows.Add(drServiceRow);

                                    row = row + 1;
                                    drServiceRow = dtService.NewRow();
                                    drServiceRow["ID"] = row;
                                    drServiceRow["Carrier"] = string.Empty;// result.CarrierName;
                                    drServiceRow["FieldName"] = "DeliveryDeadLine";
                                    drServiceRow[dc.ColumnName] = result.DeliveryDeadLine;
                                    dtService.Rows.Add(drServiceRow);
                                    row = row + 1;
                                    drServiceRow = dtService.NewRow();
                                    drServiceRow["ID"] = row;
                                    drServiceRow["Carrier"] = string.Empty;// result.CarrierName;
                                    drServiceRow["FieldName"] = "Information";
                                    drServiceRow[dc.ColumnName] = result.Information;
                                    dtService.Rows.Add(drServiceRow);

                                    row = row + 1;
                                    drServiceRow = dtService.NewRow();
                                    drServiceRow["ID"] = row;
                                    drServiceRow["Carrier"] = string.Empty;// result.CarrierName;
                                    drServiceRow["FieldName"] = "Carrier Service Code";
                                    drServiceRow[dc.ColumnName] = result.CarrierServiceCode;
                                    dtService.Rows.Add(drServiceRow);

                                    row = row + 1;
                                    drServiceRow = dtService.NewRow();
                                    drServiceRow["ID"] = row;
                                    drServiceRow["Carrier"] = string.Empty;// result.CarrierName;
                                    drServiceRow["FieldName"] = "Disable";
                                    if (result.Active == SEnumFlag.Yes)
                                    {
                                        drServiceRow[dc.ColumnName] = "No";
                                    }
                                    else
                                    {
                                        drServiceRow[dc.ColumnName] = "Yes";
                                    }
                                    dtService.Rows.Add(drServiceRow);
                                    rowcountperservice = row - 1;
                                    rowCreated = true;


                                }
                                else
                                {


                                    temprow = CurrentRow;

                                    dtService.Rows[CurrentRow][dc.ColumnName] = result.ServiceID;
                                    CurrentRow = CurrentRow + 1;
                                    dtService.Rows[CurrentRow][dc.ColumnName] = result.ServiceCode;
                                    CurrentRow = CurrentRow + 1;
                                    dtService.Rows[CurrentRow][dc.ColumnName] = result.ServiceName;
                                    CurrentRow = CurrentRow + 1;
                                    dtService.Rows[CurrentRow][dc.ColumnName] = result.Priority;
                                    CurrentRow = CurrentRow + 1;
                                    dtService.Rows[CurrentRow][dc.ColumnName] = result.DeliveryDelayTable;//"Import";
                                    CurrentRow = CurrentRow + 1;
                                    dtService.Rows[CurrentRow][dc.ColumnName] = result.DeliveryDeadLine;
                                    CurrentRow = CurrentRow + 1;
                                    dtService.Rows[CurrentRow][dc.ColumnName] = result.Information;
                                    CurrentRow = CurrentRow + 1;
                                    dtService.Rows[CurrentRow][dc.ColumnName] = result.CarrierServiceCode;
                                    CurrentRow = CurrentRow + 1;
                                    if (result.Active == SEnumFlag.Yes)
                                    {
                                        dtService.Rows[CurrentRow][dc.ColumnName] = "No";
                                    }
                                    else
                                    {
                                        dtService.Rows[CurrentRow][dc.ColumnName] = "Yes";
                                    }

                                    CurrentRow = temprow;
                                }
                                //store column index
                                break;
                            }

                            #endregion
                        }
                        #endregion


                    }
                    if (intMSCounter != temp)
                    {
                        temp = 0;
                        foreach (DataColumn dc in dtService.Columns)
                        {
                            if (dc.Ordinal > 2)
                            {
                                if (!strMasterServiceNames.Contains(dc.ColumnName))
                                {

                                    temprow = CurrentRow;
                                    dtService.Rows[CurrentRow][dc.ColumnName] = 0;
                                    CurrentRow = CurrentRow + 1;
                                    dtService.Rows[CurrentRow][dc.ColumnName] = "";
                                    CurrentRow = CurrentRow + 1;
                                    dtService.Rows[CurrentRow][dc.ColumnName] = "";
                                    CurrentRow = CurrentRow + 1;
                                    dtService.Rows[CurrentRow][dc.ColumnName] = "";
                                    CurrentRow = CurrentRow + 1;
                                    dtService.Rows[CurrentRow][dc.ColumnName] = "NoTable";//"Import";
                                    CurrentRow = CurrentRow + 1;
                                    dtService.Rows[CurrentRow][dc.ColumnName] = "";
                                    CurrentRow = CurrentRow + 1;
                                    dtService.Rows[CurrentRow][dc.ColumnName] = "";
                                    CurrentRow = CurrentRow + 1;
                                    dtService.Rows[CurrentRow][dc.ColumnName] = "";
                                    CurrentRow = CurrentRow + 1;
                                    dtService.Rows[CurrentRow][dc.ColumnName] = "Yes";
                                    CurrentRow = temprow;

                                }
                            }

                        }

                    }

                }
                else
                {


                    #region Column Looping

                    foreach (DataColumn dc in dtService.Columns)
                    {

                        #region Columnname = MasterService name
                        if (dc.Ordinal > 2)
                        {

                            if (!rowCreated)
                            {
                                CurrentRow = row;
                                row = row + 1;
                                drServiceRow = dtService.NewRow();
                                drServiceRow["ID"] = row;
                                drServiceRow["Carrier"] = sCarrier.CarrierName.ToUpper().Trim();
                                drServiceRow["FieldName"] = "ServiceID";
                                drServiceRow[dc.ColumnName] = 0;
                                dtService.Rows.Add(drServiceRow);
                                row = row + 1;
                                drServiceRow = dtService.NewRow();
                                drServiceRow["ID"] = row;
                                drServiceRow["Carrier"] = sCarrier.CarrierName.ToUpper().Trim();
                                drServiceRow["FieldName"] = "ServiceCode";
                                drServiceRow[dc.ColumnName] = "";
                                dtService.Rows.Add(drServiceRow);

                                row = row + 1;
                                drServiceRow = dtService.NewRow();
                                drServiceRow["ID"] = row;
                                drServiceRow["Carrier"] = string.Empty;// CarrierList[i].ToUpper().Trim();
                                drServiceRow["FieldName"] = "ServiceName";
                                drServiceRow[dc.ColumnName] = "";
                                dtService.Rows.Add(drServiceRow);

                                row = row + 1;
                                drServiceRow = dtService.NewRow();
                                drServiceRow["ID"] = row;
                                drServiceRow["Carrier"] = string.Empty;// CarrierList[i].ToUpper().Trim();
                                drServiceRow["FieldName"] = "Priority";
                                drServiceRow[dc.ColumnName] = "";
                                dtService.Rows.Add(drServiceRow);


                                row = row + 1;
                                drServiceRow = dtService.NewRow();
                                drServiceRow["ID"] = row;
                                drServiceRow["Carrier"] = string.Empty;// CarrierList[i].ToUpper().Trim();
                                drServiceRow[dc.ColumnName] = "NoTable";//"ImportDetails";
                                drServiceRow[dc.ColumnName] = "";
                                dtService.Rows.Add(drServiceRow);

                                row = row + 1;
                                drServiceRow = dtService.NewRow();
                                drServiceRow["ID"] = row;
                                drServiceRow["Carrier"] = string.Empty;// CarrierList[i].ToUpper().Trim();
                                drServiceRow["FieldName"] = "DeliveryDeadLine";
                                drServiceRow[dc.ColumnName] = "";
                                dtService.Rows.Add(drServiceRow);

                                row = row + 1;
                                drServiceRow = dtService.NewRow();
                                drServiceRow["ID"] = row;
                                drServiceRow["Carrier"] = string.Empty; //CarrierList[i].ToUpper().Trim();
                                drServiceRow["FieldName"] = "Information";
                                drServiceRow[dc.ColumnName] = "";
                                dtService.Rows.Add(drServiceRow);

                                row = row + 1;
                                drServiceRow = dtService.NewRow();
                                drServiceRow["ID"] = row;
                                drServiceRow["Carrier"] = string.Empty;// result.CarrierName;
                                drServiceRow["FieldName"] = "Carrier Service Code";
                                drServiceRow[dc.ColumnName] = "";
                                dtService.Rows.Add(drServiceRow);

                                row = row + 1;
                                drServiceRow = dtService.NewRow();
                                drServiceRow["ID"] = row;
                                drServiceRow["Carrier"] = string.Empty;// CarrierList[i].ToUpper().Trim();
                                drServiceRow["FieldName"] = "Disable";
                                drServiceRow[dc.ColumnName] = "Yes";
                                dtService.Rows.Add(drServiceRow);

                                //row = row + 1;
                                //drServiceRow = dtService.NewRow();
                                //drServiceRow["ID"] = row;
                                //drServiceRow["Carrier"] = CarrierList[i].ToUpper().Trim();
                                //drServiceRow[dc.ColumnName] = "";
                                //dtService.Rows.Add(drServiceRow);
                                rowcountperservice = row - 1;
                                rowCreated = true;


                            }
                            else
                            {
                                temprow = CurrentRow;
                                dtService.Rows[CurrentRow][dc.ColumnName] = 0;
                                CurrentRow = CurrentRow + 1;
                                dtService.Rows[CurrentRow][dc.ColumnName] = "";
                                CurrentRow = CurrentRow + 1;
                                dtService.Rows[CurrentRow][dc.ColumnName] = "";
                                CurrentRow = CurrentRow + 1;
                                dtService.Rows[CurrentRow][dc.ColumnName] = "";
                                CurrentRow = CurrentRow + 1;
                                dtService.Rows[CurrentRow][dc.ColumnName] = "NoTable";//"Import";
                                CurrentRow = CurrentRow + 1;
                                dtService.Rows[CurrentRow][dc.ColumnName] = "";
                                CurrentRow = CurrentRow + 1;
                                dtService.Rows[CurrentRow][dc.ColumnName] = "";
                                CurrentRow = CurrentRow + 1;
                                dtService.Rows[CurrentRow][dc.ColumnName] = "";
                                CurrentRow = CurrentRow + 1;
                                dtService.Rows[CurrentRow][dc.ColumnName] = "Yes";
                                CurrentRow = temprow;
                            }
                            //store column index
                            break;
                        }

                        #endregion
                    }
                    #endregion







                }

                //gvCarrierService1.DataSource = dtService;
                //gvCarrierService1.DataBind();
                #endregion



            }




            return dtService;
        }

        protected void LoadCarrierServiceGrid(bool isPost, bool isModified)
        {
            try
            {
                KaizosServiceAgent proxy = new KaizosServiceAgent();
                List<SCarrierService> sCarrierService = proxy.GetCarrierServiceForMaster();
                // List<MasterServiceUpdate> masterServiceUpdate = FormatDataForGrid(sCarrierService);
                //gvCarrierService1.DataSource = FormatDataForGrid(sCarrierService);
                //gvCarrierService1.DataBind();
                DataTable dtService = FormatDataForGrid(sCarrierService);
                tbCarrierService.Controls.Clear();
                //startRow = i;
                TableRow trHeader = new TableRow();
                foreach (DataColumn dc in dtService.Columns)
                {
                    TableHeaderCell tc = new TableHeaderCell();
                    Label lb = new Label();
                    lb.ID = "lblHeaderRowCol" + dc.Ordinal.ToString();

                    lb.Text = dc.ColumnName;
                    if (dc.Ordinal == 0)
                    {
                        tc.Visible = false;
                        //  tc.Style.Add(HtmlTextWriterStyle.Display, "none");
                    }
                    tc.Controls.Add(lb);
                    trHeader.Cells.Add(tc);

                }
                trHeader.CssClass = "gridHeader";
                tbCarrierService.CssClass = "clsTable";
                tbCarrierService.Rows.Add(trHeader);
                int i = 1;
                int startRow = i;
                int endRow = 0;
                int ClientTableRow = 0;
                foreach (DataRow dr in dtService.Rows)
                {
                    TableRow tr = new TableRow();
                    string sTemp = string.Empty;
                    string sCarrier = string.Empty;
                    string sServiceID = string.Empty;
                    string sServiceName = string.Empty;
                    ClientTableRow = ClientTableRow + 7;
                    bool filled = false;
                    foreach (DataColumn dc in dtService.Columns)
                    {

                        TableCell tc = new TableCell();
                        if (dc.Ordinal <= 2)
                        {

                            Label lb = new Label();
                            lb.ID = "lblRow" + i.ToString() + "Col" + dc.Ordinal.ToString();
                            lb.Text = dr[dc].ToString();
                            sTemp = lb.Text;
                            if (dc.ColumnName.Equals("Carrier"))
                            {
                                if (!dr[dc].ToString().Equals(string.Empty))
                                {
                                    sCarrier = dr[dc].ToString();
                                }
                                // tc.RowSpan = 6; 
                            }
                            if (dc.ColumnName.Equals("ID"))
                            {
                                tc.Visible = false;
                                tc.Style.Add(HtmlTextWriterStyle.Display, "none");
                            }
                            if (sTemp.Equals("ServiceID"))
                            {
                                tr.Visible = false;
                                //tr.Style.Add(HtmlTextWriterStyle.Display, "none");
                            }
                            if (sTemp.Equals("Priority"))
                            {
                                tr.Visible = false;
                                //tr.Style.Add(HtmlTextWriterStyle.Display, "none");
                            }

                            tc.CssClass = "clsProposedTD";
                            tc.Controls.Add(lb);
                            tr.Cells.Add(tc);

                        }
                        else
                        {
                            if (!sTemp.Equals("Import"))
                            {
                                if (!sTemp.Equals("Disable"))
                                {
                                    TextBox txtBox = new TextBox();
                                    txtBox.ID = "txtBoxRow" + i.ToString() + "Col" + dc.Ordinal.ToString();


                                    txtBox.Text = dr[dc].ToString();
                                    if (sTemp.Equals("ServiceID"))
                                    {
                                        sServiceID = dr[dc].ToString();
                                    }
                                    if (sTemp.Equals("ServiceName"))
                                    {
                                        sServiceName = dr[dc].ToString();
                                    }

                                    tc.Controls.Add(txtBox);
                                    tc.CssClass = "clsProposedTD";
                                    tr.Cells.Add(tc);
                                }
                                else
                                {

                                    CheckBox chkDisable = new CheckBox();

                                    chkDisable.ID = "chkRow" + i.ToString() + "Col" + dc.Ordinal.ToString();
                                    //chkDisable.Attributes.Add("onclick", "test()");
                                    TextBox txtBoxServiceID = (TextBox)tbCarrierService.Rows[i - 8].FindControl("txtBoxRow" + (i - 8).ToString() + "Col" + dc.Ordinal.ToString());

                                    // txtBoxServiceID.Enabled = false;
                                    chkDisable.Attributes.Add("onclick", "Enable(" + tbCarrierService.Rows.Count.ToString() + "," + dc.Ordinal.ToString() + "," + Server.HtmlDecode("'" + tbCarrierService.ClientID.ToString() + "'") + "," + txtBoxServiceID.Text + ")");
                                    if (dr[dc].ToString().Equals("Yes"))
                                    {


                                        int k = i;


                                        TextBox txtBoxServiceCode = (TextBox)tbCarrierService.Rows[k - 7].FindControl("txtBoxRow" + (k - 7).ToString() + "Col" + dc.Ordinal.ToString());
                                        txtBoxServiceCode.Enabled = false;
                                        TextBox txtBoxServiceName = (TextBox)tbCarrierService.Rows[k - 6].FindControl("txtBoxRow" + (k - 6).ToString() + "Col" + dc.Ordinal.ToString());
                                        txtBoxServiceName.Enabled = false;
                                        LinkButton lnkbt = (LinkButton)tbCarrierService.Rows[k - 4].FindControl("lnkBtRow" + (k - 4).ToString() + "Col" + dc.Ordinal.ToString());
                                        lnkbt.Style.Add(HtmlTextWriterStyle.Display, "none");
                                        Button bt = (Button)tbCarrierService.Rows[k - 4].FindControl("btRow" + (k - 4).ToString() + "Col" + dc.Ordinal.ToString());
                                        bt.Style.Add(HtmlTextWriterStyle.Display, "none");
                                        //TextBox txtBoxPriority = (TextBox)tbCarrierService.Rows[k - 5].FindControl("txtBoxRow" + (k - 5).ToString() + "Col" + dc.Ordinal.ToString());
                                        //txtBoxPriority.Enabled = false;
                                        TextBox txtBoxDeliveryDeadLine = (TextBox)tbCarrierService.Rows[k - 3].FindControl("txtBoxRow" + (k - 3).ToString() + "Col" + dc.Ordinal.ToString());
                                        txtBoxDeliveryDeadLine.Enabled = false;
                                        TextBox txtBoxInformation = (TextBox)tbCarrierService.Rows[k - 2].FindControl("txtBoxRow" + (k - 2).ToString() + "Col" + dc.Ordinal.ToString());
                                        txtBoxInformation.Enabled = false;
                                        TextBox txtBoxCarrierServiceCode = (TextBox)tbCarrierService.Rows[k - 1].FindControl("txtBoxRow" + (k - 1).ToString() + "Col" + dc.Ordinal.ToString());
                                        txtBoxCarrierServiceCode.Enabled = false;

                                        chkDisable.Checked = true;




                                    }
                                    else
                                    {
                                        chkDisable.Checked = false;
                                    }
                                    TextBox txtBoxInfo = (TextBox)tbCarrierService.Rows[i - 2].FindControl("txtBoxRow" + (i - 2).ToString() + "Col" + dc.Ordinal.ToString());
                                    txtBoxInfo.TextMode = TextBoxMode.MultiLine;
                                    txtBoxInfo.Rows = 6;
                                    tc.CssClass = "clsProposedTD";
                                    tc.Controls.Add(chkDisable);
                                    tr.Cells.Add(tc);
                                    startRow = i + 1;
                                    filled = false;
                                }
                            }
                            else
                            {

                                LinkButton lnkBtRow = new LinkButton();
                                lnkBtRow.ID = "lnkBtRow" + i.ToString() + "Col" + dc.Ordinal.ToString();
                                lnkBtRow.Text = "Update Deliver Delay";
                                lnkBtRow.Command += new CommandEventHandler(lnkBtRow_Command);
                                lnkBtRow.CommandName = "Update";
                                lnkBtRow.CommandArgument = dtService.Rows[startRow - 1][dc].ToString() + ";" + dtService.Rows[startRow - 1]["Carrier"].ToString() + ";" + dtService.Rows[startRow - 1 + 2]["FieldName"].ToString();
                                tc.Controls.Add(lnkBtRow);

                                Button btRow = new Button();
                                btRow.ID = "btRow" + i.ToString() + "Col" + dc.Ordinal.ToString();
                                btRow.Text = "Import Deliver Delay";
                                btRow.Command += new CommandEventHandler(btRow_Command);
                                btRow.CommandName = "Import";
                                btRow.CommandArgument = dtService.Rows[startRow - 1][dc].ToString();
                                tc.Controls.Add(btRow);
                                tc.CssClass = "clsProposedTD";
                                tr.Cells.Add(tc);




                            }
                        }

                    }
                    tbCarrierService.Rows.Add(tr);
                    i = i + 1;

                }

                #region commented code
                //foreach (DataColumn col in dtService.Columns)
                //{
                //    //Declare the bound field and allocate memory for the bound field.
                //    TemplateField bfield = new TemplateField();

                //    if (col.ColumnName == "ID" || col.ColumnName == "Carrier")
                //    {
                //        //Initalize the DataField value.
                //        bfield.HeaderTemplate = new GridViewTemplate(ListItemType.Header, col.ColumnName,col.Caption,isPost,isModified);
                //        bfield.ItemTemplate = new GridViewTemplate(ListItemType.Item, col.ColumnName, col.Caption, isPost,isModified );
                //        if (col.ColumnName == "ID")
                //        {
                //            bfield.Visible = false;
                //        }
                //    }
                //    else
                //    {
                //        //Initialize the HeaderText field value.
                //        bfield.HeaderTemplate = new GridViewTemplate(ListItemType.Header, col.ColumnName, col.Caption, isPost,isModified);
                //        bfield.ItemTemplate = new GridViewTemplate(ListItemType.Item, col.ColumnName, col.Caption, isPost,isModified);

                //    }
                //    //Add the newly created bound field to the GridView.
                //    gvCarrierService1.Columns.Add(bfield);

                //}

                ////Initialize the DataSource
                //gvCarrierService1.ShowHeader = true;
                //gvCarrierService1.DataSource = dtService;

                ////Bind the datatable with the GridView.
                //gvCarrierService1.DataBind();
                //gvCarrierService1.ShowHeader = true;
                ////foreach (GridViewRow r in gvCarrierService1.Rows)
                ////{

                ////    r.Cells[6].text = r.Cells.Count.ToString();
                ////    r.Cells[4].Enabled = false;
                ////    r.c
                ////}
                #endregion
            }
            catch (FaultException<SGeneralFault> sGeneralFault)
            {
                SGeneralFault fault = sGeneralFault.Detail;
                string userName = User.Identity.Name;
                string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + fault.Issue + fault.Details;
                Response.Write(errorMessage);
            }
            catch (Exception error)
            {
                string userName = User.Identity.Name;
                string errorMessage = "Error occured for [" + userName.ToUpper().Trim() + "][" + DateTime.Now.ToLongDateString() + "]:" + error.Message;
                Response.Write(errorMessage);
            }



        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            string ErrorMessage = string.Empty;
            try
            {

                List<SCarrierService> lstInsertCarrierService = new List<SCarrierService>();
                List<SCarrierService> lstUpdateCarrierService = new List<SCarrierService>();
                bool bupdate = false;
                bool binsert = true;
                // GridView MainContainer = (GridView)container.NamingContainer;
                for (int k = 9; k <= tbCarrierService.Rows.Count; k = k + 9)
                {

                    string strServiceCode = string.Empty;
                    int msRef = 2;
                    //int MasterServiceColumnStart = 2;//[3-1 id,carrier,fieldname]
                    for (int col = 1; col <= intMSCounter; col++)
                    {


                        int s = msRef + col;
                        CheckBox chkBox = (CheckBox)tbCarrierService.Rows[k].FindControl("chkRow" + k.ToString() + "Col" + s.ToString());

                        SCarrierService sCarrierService = new SCarrierService();


                        TextBox txtBoxServiceId = (TextBox)tbCarrierService.Rows[k - 8].FindControl("txtBoxRow" + (k - 8).ToString() + "Col" + s.ToString());
                        sCarrierService.ServiceID = Convert.ToInt32(txtBoxServiceId.Text);



                        TextBox txtBoxServiceCode = (TextBox)tbCarrierService.Rows[k - 7].FindControl("txtBoxRow" + (k - 7).ToString() + "Col" + s.ToString());
                        TextBox txtBoxServiceName = (TextBox)tbCarrierService.Rows[k - 6].FindControl("txtBoxRow" + (k - 6).ToString() + "Col" + s.ToString());
                        TextBox txtBoxDeliveryDeadLine = (TextBox)tbCarrierService.Rows[k - 3].FindControl("txtBoxRow" + (k - 3).ToString() + "Col" + s.ToString());
                        TextBox txtBoxInformation = (TextBox)tbCarrierService.Rows[k - 2].FindControl("txtBoxRow" + (k - 2).ToString() + "Col" + s.ToString());
                        TextBox txtBoxCarrierServiceCode = (TextBox)tbCarrierService.Rows[k - 1].FindControl("txtBoxRow" + (k - 1).ToString() + "Col" + s.ToString());


                        sCarrierService.CarrierName = ((Label)tbCarrierService.Rows[k - 7].FindControl("lblRow" + (k - 7).ToString() + "Col" + "1")).Text;
                        sCarrierService.MasterServiceName = ((Label)tbCarrierService.Rows[0].FindControl("lblHeaderRowCol" + s.ToString())).Text;
                        sCarrierService.ServiceName = txtBoxServiceName.Text;
                        sCarrierService.ServiceCode = txtBoxServiceCode.Text;



                        if (s > 3)
                        {
                            if (!sCarrierService.ServiceCode.Equals(string.Empty))
                            {
                                if (!chkBox.Checked)
                                {

                                    if (strServiceCode.Contains(sCarrierService.ServiceCode))
                                    {
                                        ErrorMessage = ErrorMessage + "Service Code :" + sCarrierService.ServiceCode + " Should be  Unique for the Carrier" + sCarrierService.CarrierName + "<br/>";
                                        binsert = false;
                                        bupdate = false;

                                    }
                                }
                            }
                        }
                        if (!sCarrierService.ServiceCode.Equals(string.Empty))
                        {
                            if (!chkBox.Checked)
                            {
                                strServiceCode = strServiceCode + "," + sCarrierService.ServiceCode;
                            }
                        }
                        sCarrierService.DeliveryDelayTable = string.Empty;
                        sCarrierService.DeliveryDeadLine = txtBoxDeliveryDeadLine.Text;
                        sCarrierService.Information = txtBoxInformation.Text;
                        sCarrierService.CarrierServiceCode = txtBoxCarrierServiceCode.Text;
                        if (txtBoxInformation.Text.ToLower().Contains("http") || txtBoxInformation.Text.ToLower().Contains("www"))
                            sCarrierService.InfoType = "URL";
                        else
                            sCarrierService.InfoType = "TEXT";

                        sCarrierService.KeyCustomerService = SEnumFlag.No;
                        sCarrierService.Priority = SEnumPriority.Economy;


                        if (chkBox.Checked)
                        {
                            sCarrierService.Active = SEnumFlag.No;
                        }
                        else
                        {
                            sCarrierService.Active = SEnumFlag.Yes;
                        }
                        if (sCarrierService.ServiceID > 0)
                        {
                            if (sCarrierService.ServiceName.Trim().Equals(string.Empty))
                            {
                                ErrorMessage = ErrorMessage + "Service Name " + "Should Not be Empty for " + sCarrierService.MasterServiceName + " for the Carrier" + sCarrierService.CarrierName + "<br/>";
                            }
                            if (sCarrierService.ServiceCode.Trim().Equals(string.Empty))
                            {
                                ErrorMessage = ErrorMessage + "Service Code " + "Should Not be Empty for " + sCarrierService.MasterServiceName + " for the Carrier" + sCarrierService.CarrierName + "<br/>";
                            }
                            if (sCarrierService.DeliveryDeadLine.Trim().Equals(string.Empty))
                            {
                                ErrorMessage = ErrorMessage + "DeliveryDeadLine " + "Should Not be Empty for " + sCarrierService.MasterServiceName + " for the Carrier" + sCarrierService.CarrierName + "<br/>";
                            }
                            if (!isValidTime(sCarrierService.DeliveryDeadLine))
                            {
                                ErrorMessage = ErrorMessage + "DeliveryDeadLine  Time is not in proper format for " + sCarrierService.MasterServiceName + " for the Carrier" + sCarrierService.CarrierName + "<br/>";
                            }
                            if (sCarrierService.CarrierServiceCode.Trim().Equals(string.Empty))
                            {
                                ErrorMessage = ErrorMessage + "CarrierServiceCode " + "Should Not be Empty for " + sCarrierService.MasterServiceName + " for the Carrier" + sCarrierService.CarrierName + "<br/>";
                            }
                            if (ErrorMessage.Equals(string.Empty))
                            {
                                lstUpdateCarrierService.Add(sCarrierService);
                                bupdate = true;
                            }
                            else
                            {
                                bupdate = false;
                                break;

                            }


                        }
                        else
                        {
                            if (!chkBox.Checked)
                            {
                                if (sCarrierService.ServiceName.Trim().Equals(string.Empty))
                                {
                                    ErrorMessage = ErrorMessage + "Service Name " + "Should Not be Empty for " + sCarrierService.MasterServiceName + " for the Carrier" + sCarrierService.CarrierName + "<br/>";
                                }
                                if (sCarrierService.ServiceCode.Trim().Equals(string.Empty))
                                {
                                    ErrorMessage = ErrorMessage + "Service Code " + "Should Not be Empty for " + sCarrierService.MasterServiceName + " for the Carrier" + sCarrierService.CarrierName + "<br/>";
                                }
                                if (sCarrierService.DeliveryDeadLine.Trim().Equals(string.Empty))
                                {
                                    ErrorMessage = ErrorMessage + "DeliveryDeadLine " + "Should Not be Empty for " + sCarrierService.MasterServiceName + " for the Carrier" + sCarrierService.CarrierName + "<br/>";
                                }
                                if (!isValidTime(sCarrierService.DeliveryDeadLine))
                                {
                                    ErrorMessage = ErrorMessage + "DeliveryDeadLine  Time is not in proper format for " + sCarrierService.MasterServiceName + " for the Carrier" + sCarrierService.CarrierName + "<br/>";
                                }
                                if (sCarrierService.CarrierServiceCode.Trim().Equals(string.Empty))
                                {
                                    ErrorMessage = ErrorMessage + "CarrierServiceCode " + "Should Not be Empty for " + sCarrierService.MasterServiceName + " for the Carrier" + sCarrierService.CarrierName + "<br/>";
                                }
                                if (ErrorMessage.Equals(string.Empty))
                                {

                                    lstInsertCarrierService.Add(sCarrierService);
                                    binsert = true;
                                }
                                else
                                {
                                    binsert = false;
                                    break;
                                }
                            }



                        }


                    }
                }
                KaizosServiceAgent proxy = new KaizosServiceAgent();
                if (!ErrorMessage.Equals(string.Empty))
                {
                    KaizosSession.Current.ErrorMessage = ErrorMessage;
                    binsert = false;
                    bupdate = false;
                }
                //gvCarrierService1.Controls.Clear();
                if (bupdate)
                {
                    proxy.UpdateCarrierService(lstUpdateCarrierService);

                    KaizosSession.Current.ErrorMessage = " Records sucessfully updated";
                }
                if (binsert)
                {
                    proxy.InsertCarrierService(lstInsertCarrierService);
                    KaizosSession.Current.ErrorMessage = " Records sucessfully updated";
                }
                KaizosSession.Current.ReturnURL = "frmMasterServiceTypeUpdate.aspx";
                //GetGlobalResourceObject("LocalString", "MasterServiceUnderDev").ToString();
                Server.Transfer("frmResult.aspx", false);//
                //if (!ErrorMessage.Equals(string.Empty))
                //{
                //    KaizosSession.Current.ErrorMessage = ErrorMessage;
                //}


                //LoadCarrierServiceGrid(true, true);
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [05JAN12RM] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;

                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmMasterServiceTypeUpdate.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MasterServiceUnderDev").ToString();
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [17FEB12RM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "gvCarrierService_RowCommand", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmMasterServiceTypeUpdate.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MasterServiceUnderDev").ToString();
                    Server.Transfer("frmResult.aspx", false);

                }
            }


        }

        protected void lnkBtRow_Command(object sender, CommandEventArgs e)
        {

            string[] s = e.CommandArgument.ToString().Split(';');
            if (Convert.ToInt32(s[0]) > 0)
            {
                KaizosSession.Current.ServiceID = s[0].ToString();
                KaizosSession.Current.CarrierName = s[1].ToString();
                KaizosSession.Current.ServiceName = s[2].ToString();
                Server.Transfer("frmDeliveryDelayUpdate.aspx");
            }
            LoadCarrierServiceGrid(false, false);

        }

        protected void btRow_Command(object sender, CommandEventArgs e)
        {
            if (Convert.ToInt32(e.CommandArgument.ToString()) > 0)
            {
                KaizosSession.Current.ServiceID = e.CommandArgument.ToString();
                Server.Transfer("frmDeliveryDelayImport.aspx");
            }
            LoadCarrierServiceGrid(false, false);
        }

        protected void gvCarrierService_RowCreated(object sender, GridViewRowEventArgs e)
        {


        }

        protected void gvCarrierService_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


            }

        }

        ////23FEB12RM
        //protected void gvCarrierService_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        #region Hide columns depends on existence of master service name in carrier_service table

        //        KaizosServiceAgent proxy = new KaizosServiceAgent();
        //        List<SCarrierService> sCarrierService = proxy.GetCarrierService();

        //        List<String> MasterServiceList = GetMasterServiceName(sCarrierService);

        //        #region Hide Road Service, if master service count in carrier_service table is 0

        //        if (MasterServiceList.Count < 1)  //Hide all road service name
        //        {
        //            //Service Name  
        //            TextBox txtNewRoadServiceName = (TextBox)e.Row.FindControl("txtRoadServiceName");
        //            txtNewRoadServiceName.Visible = false;

        //            //Service Code
        //            TextBox txtNewRoadServiceCode = (TextBox)e.Row.FindControl("txtRoadServiceCode");
        //            txtNewRoadServiceCode.Visible = false;

        //            //Delivery Delay Link
        //            LinkButton lblNewRoadDeliveryDelay = (LinkButton)e.Row.FindControl("lblRoadDeliveryDelay");
        //            lblNewRoadDeliveryDelay.Visible = false;
        //            Button btnNewRoadImport = (Button)e.Row.FindControl("btnRoadImport");
        //            btnNewRoadImport.Visible = false;

        //            //delivery deadline
        //            TextBox txtNewRoadDeliveryDeadLine = (TextBox)e.Row.FindControl("txtRoadDeliveryDeadLine");
        //            txtNewRoadDeliveryDeadLine.Visible = false;

        //            //Active
        //            CheckBox chkNewRoadActive = (CheckBox)e.Row.FindControl("chkRoadActive");
        //            chkNewRoadActive.Visible = false;

        //        }
        //        #endregion

        //        #region Hide Air B4 10 column, if master service count in carrier_service table is 1 (ie., only road available)

        //        if (MasterServiceList.Count < 2)  //Hide all AIRB418
        //        {
        //            //Service Name
        //            TextBox txtNewAirB410ServiceName = (TextBox)e.Row.FindControl("txtAirB410ServiceName");
        //            txtNewAirB410ServiceName.Visible = false;

        //            //Service Code
        //            TextBox txtNewAirB410ServiceCode = (TextBox)e.Row.FindControl("txtAirB410ServiceCode");
        //            txtNewAirB410ServiceCode.Visible = false;

        //            //Delivery Delay Link
        //            LinkButton lblNewAirB410DeliveryDelay = (LinkButton)e.Row.FindControl("lblAirB410DeliveryDelay");
        //            lblNewAirB410DeliveryDelay.Visible = false;
        //            Button btnNewAirB410Import = (Button)e.Row.FindControl("btnAirB410Import");
        //            btnNewAirB410Import.Visible = false;

        //            //delivery deadline
        //            TextBox txtNewAirB410DeliveryDeadLine = (TextBox)e.Row.FindControl("txtAirB410DeliveryDeadLine");
        //            txtNewAirB410DeliveryDeadLine.Visible = false;

        //            //Active
        //            CheckBox chkNewAirB410Active = (CheckBox)e.Row.FindControl("chkAirB410Active");
        //            chkNewAirB410Active.Visible = false;

        //        }
        //        #endregion

        //        #region Hide Air B4 12 column, if master service count in carrier_service table is 1 (ie., only road & air b4 10 available)

        //        if (MasterServiceList.Count < 3)  //Hide all AIRB418
        //        {

        //            //Service Name
        //            TextBox txtNewAirB412ServiceName = (TextBox)e.Row.FindControl("txtAirB412ServiceName");
        //            txtNewAirB412ServiceName.Visible = false;

        //            //Service Code
        //            TextBox txtNewAirB412ServiceCode = (TextBox)e.Row.FindControl("txtAirB412ServiceCode");
        //            txtNewAirB412ServiceCode.Visible = false;

        //            //Delivery Delay Link
        //            LinkButton lblNewAirB412DeliveryDelay = (LinkButton)e.Row.FindControl("lblAirB412DeliveryDelay");
        //            lblNewAirB412DeliveryDelay.Visible = false;
        //            Button btnNewAirB412Import = (Button)e.Row.FindControl("btnAirB412Import");
        //            btnNewAirB412Import.Visible = false;

        //            //delivery deadline
        //            TextBox txtNewAirB412DeliveryDeadLine = (TextBox)e.Row.FindControl("txtAirB412DeliveryDeadLine");
        //            txtNewAirB412DeliveryDeadLine.Visible = false;

        //            //Active
        //            CheckBox chkNewAirB412Active = (CheckBox)e.Row.FindControl("chkAirB412Active");
        //            chkNewAirB412Active.Visible = false;

        //        }
        //        #endregion

        //        #region Hide Air B4 18 column, if master service count in carrier_service table is 1 (ie., only road, air b4 10 & air b4 12 available)

        //        if (MasterServiceList.Count < 4)  //Hide all AIRB418
        //        {

        //            //Service Name
        //            TextBox txtNewAirB418ServiceName = (TextBox)e.Row.FindControl("txtAirB418ServiceName");
        //            txtNewAirB418ServiceName.Visible = false;

        //            //Service Code
        //            TextBox txtNewAirB418ServiceCode = (TextBox)e.Row.FindControl("txtAirB418ServiceCode");
        //            txtNewAirB418ServiceCode.Visible = false;

        //            //Delivery Delay Link
        //            LinkButton lblNewAirB418DeliveryDelay = (LinkButton)e.Row.FindControl("lblAirB418DeliveryDelay");
        //            lblNewAirB418DeliveryDelay.Visible = false;
        //            Button btnNewAirB418Import = (Button)e.Row.FindControl("btnAirB418Import");
        //            btnNewAirB418Import.Visible = false;

        //            //delivery deadline
        //            TextBox txtNewAirB418DeliveryDeadLine = (TextBox)e.Row.FindControl("txtAirB418DeliveryDeadLine");
        //            txtNewAirB418DeliveryDeadLine.Visible = false;

        //            //Active
        //            CheckBox chkNewAirB418Active = (CheckBox)e.Row.FindControl("chkAirB418Active");
        //            chkNewAirB418Active.Visible = false;

        //        }
        //        #endregion

        //        #endregion
        //    }
        //}

        protected void gvCarrierService1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                #region Try block


                if (e.CommandSource.GetType().ToString() == "System.Web.UI.WebControls.Button")  // if "paramenter" link is clicked
                {
                    Button btn = (Button)e.CommandSource;

                    GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                    int i = btn.ID.IndexOf('K');
                    TextBox txtServiceID = (TextBox)gvCarrierService1.Rows[gvr.RowIndex - 4].FindControl("tb1" + btn.ID.Substring(3, i - 2) + (gvr.RowIndex - 4).ToString());
                    int serviceID = Convert.ToInt32(txtServiceID.Text);
                    KaizosSession.Current.ServiceID = serviceID.ToString();
                    //Server.Transfer("frmDeliveryDelayImport.aspx"); //?ServiceID=" + serviceID.ToString());
                    Response.Redirect("frmDeliveryDelayImport.aspx");
                }


                if (e.CommandSource.GetType().ToString() == "System.Web.UI.WebControls.LinkButton")  // if "paramenter" link is clicked
                {
                    LinkButton btn = (LinkButton)e.CommandSource;

                    GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                    int i = btn.ID.IndexOf('K');
                    TextBox txtServiceID = (TextBox)gvCarrierService1.Rows[gvr.RowIndex - 4].FindControl("tb1" + btn.ID.Substring(4, i - 3) + (gvr.RowIndex - 4).ToString());
                    int serviceID = Convert.ToInt32(txtServiceID.Text);
                    KaizosSession.Current.ServiceID = serviceID.ToString();
                    KaizosSession.Current.CarrierName = ((HiddenField)gvCarrierService1.Rows[gvr.RowIndex - 4].FindControl("hdnCarrierName" + btn.ID.Substring(4, i - 3) + (gvr.RowIndex - 4).ToString())).Value;
                    KaizosSession.Current.ServiceName = ((TextBox)gvCarrierService1.Rows[gvr.RowIndex - 2].FindControl("tb1" + btn.ID.Substring(4, i - 3) + (gvr.RowIndex - 2).ToString())).Text;
                    //Server.Transfer("frmDeliveryDelayImport.aspx"); //?ServiceID=" + serviceID.ToString());
                    Response.Redirect("frmDeliveryDelayUpdate.aspx");
                }
                if (e.CommandName == "AirB410Import")  // if "paramenter" link is clicked
                {
                    Button btn = (Button)e.CommandSource;
                    GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                    Label lbl = (Label)gvCarrierService.Rows[gvr.RowIndex].FindControl("lblAirB410ServiceID");
                    int serviceID = Convert.ToInt32(lbl.Text);
                    KaizosSession.Current.ServiceID = serviceID.ToString();
                    //Server.Transfer("frmDeliveryDelayImport.aspx"); //?ServiceID=" + serviceID.ToString());
                    Response.Redirect("frmDeliveryDelayImport.aspx");
                }

                if (e.CommandName == "AirB412Import")  // if "paramenter" link is clicked
                {
                    Button btn = (Button)e.CommandSource;
                    GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                    Label lbl = (Label)gvCarrierService.Rows[gvr.RowIndex].FindControl("lblAirB412ServiceID");

                    int serviceID = Convert.ToInt32(lbl.Text);
                    KaizosSession.Current.ServiceID = serviceID.ToString();

                    //Server.Transfer("frmDeliveryDelayImport.aspx"); //?ServiceID=" + serviceID.ToString()); [06MAR12RM]
                    Response.Redirect("frmDeliveryDelayImport.aspx");
                }

                if (e.CommandName == "AirB418Import")  // if "paramenter" link is clicked
                {
                    Button btn = (Button)e.CommandSource;
                    GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                    Label lbl = (Label)gvCarrierService.Rows[gvr.RowIndex].FindControl("lblAirB418ServiceID");
                    int serviceID = Convert.ToInt32(lbl.Text);
                    KaizosSession.Current.ServiceID = serviceID.ToString();
                    //Server.Transfer("frmDeliveryDelayImport.aspx"); //?ServiceID=" + serviceID.ToString());  [06MAR12RM]
                    Response.Redirect("frmDeliveryDelayImport.aspx");
                }


                if (e.CommandName == "RoadDelay")  // if "paramenter" link is clicked
                {
                    LinkButton btn = (LinkButton)e.CommandSource;
                    GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                    Label lblNewRoadServiceID = (Label)gvCarrierService.Rows[gvr.RowIndex].FindControl("lblRoadServiceID");

                    //[06MAR12RM]
                    Label lblNewCarrierNameCaption = (Label)gvCarrierService.Rows[gvr.RowIndex].FindControl("lblCarrierName");
                    TextBox txtNewServiceNameCaption = (TextBox)gvCarrierService.Rows[gvr.RowIndex].FindControl("txtRoadServiceName");
                    KaizosSession.Current.CarrierName = lblNewCarrierNameCaption.Text.Trim();
                    KaizosSession.Current.ServiceName = txtNewServiceNameCaption.Text.Trim();
                    KaizosSession.Current.ServiceID = lblNewRoadServiceID.Text.Trim();

                    //int serviceID = Convert.ToInt32(lblNewRoadServiceID.Text);

                    //Server.Transfer("frmDeliveryDelayUpdate.aspx"); //?ServiceID=" + serviceID.ToString());
                    Response.Redirect("frmDeliveryDelayUpdate.aspx");
                }

                if (e.CommandName == "AirB410Delay")  // if "paramenter" link is clicked
                {
                    LinkButton btn = (LinkButton)e.CommandSource;
                    GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                    Label lblNewRoadServiceID = (Label)gvCarrierService.Rows[gvr.RowIndex].FindControl("lblAirB410ServiceID");


                    //[06MAR12RM]
                    Label lblNewCarrierNameCaption = (Label)gvCarrierService.Rows[gvr.RowIndex].FindControl("lblCarrierName");
                    TextBox txtNewServiceNameCaption = (TextBox)gvCarrierService.Rows[gvr.RowIndex].FindControl("txtAirB410ServiceName");
                    KaizosSession.Current.CarrierName = lblNewCarrierNameCaption.Text.Trim();
                    KaizosSession.Current.ServiceName = txtNewServiceNameCaption.Text.Trim();
                    KaizosSession.Current.ServiceID = lblNewRoadServiceID.Text.Trim();


                    //int serviceID = Convert.ToInt32(lblNewRoadServiceID.Text);
                    Server.Transfer("frmDeliveryDelayUpdate.aspx"); //?ServiceID=" + serviceID.ToString());
                }

                if (e.CommandName == "AirB412Delay")  // if "paramenter" link is clicked
                {
                    LinkButton btn = (LinkButton)e.CommandSource;
                    GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                    Label lblNewRoadServiceID = (Label)gvCarrierService.Rows[gvr.RowIndex].FindControl("lblAirB412ServiceID");


                    //[06MAR12RM]
                    Label lblNewCarrierNameCaption = (Label)gvCarrierService.Rows[gvr.RowIndex].FindControl("lblCarrierName");
                    TextBox txtNewServiceNameCaption = (TextBox)gvCarrierService.Rows[gvr.RowIndex].FindControl("txtAirB412ServiceName");
                    KaizosSession.Current.CarrierName = lblNewCarrierNameCaption.Text.Trim();
                    KaizosSession.Current.ServiceName = txtNewServiceNameCaption.Text.Trim();
                    KaizosSession.Current.ServiceID = lblNewRoadServiceID.Text.Trim();


                    //int serviceID = Convert.ToInt32(lblNewRoadServiceID.Text);

                    Server.Transfer("frmDeliveryDelayUpdate.aspx"); //?ServiceID=" + serviceID.ToString());
                }


                if (e.CommandName == "AirB418Delay")  // if "paramenter" link is clicked
                {
                    LinkButton btn = (LinkButton)e.CommandSource;
                    GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                    Label lblNewRoadServiceID = (Label)gvCarrierService.Rows[gvr.RowIndex].FindControl("lblAirB418ServiceID");


                    //[06MAR12RM]
                    Label lblNewCarrierNameCaption = (Label)gvCarrierService.Rows[gvr.RowIndex].FindControl("lblCarrierName");
                    TextBox txtNewServiceNameCaption = (TextBox)gvCarrierService.Rows[gvr.RowIndex].FindControl("txtAirB418ServiceName");
                    KaizosSession.Current.CarrierName = lblNewCarrierNameCaption.Text.Trim();
                    KaizosSession.Current.ServiceName = txtNewServiceNameCaption.Text.Trim();
                    KaizosSession.Current.ServiceID = lblNewRoadServiceID.Text.Trim();

                    //int serviceID = Convert.ToInt32(lblNewRoadServiceID.Text);

                    Server.Transfer("frmDeliveryDelayUpdate.aspx"); //?ServiceID=" + serviceID.ToString());
                }
                #endregion
            }
            /* Introduced faultexception handling and logging detailed exception into log4net file [05JAN12RM] */
            catch (FaultException<SGeneralFault> ex)
            {
                string ErrorDetails = ex.Detail.Details;
                string MethodName = ex.Detail.Issue;

                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
                logger.Debug(ErrMsg);

                KaizosSession.Current.ReturnURL = "frmMasterServiceTypeUpdate.aspx";
                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MasterServiceUnderDev").ToString();
                Server.Transfer("frmResult.aspx", false);

            }
            catch (Exception error)
            {
                /* Generalized exception handling and logging detailed exception into log4net file [17FEB12RM] */
                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
                {
                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "gvCarrierService_RowCommand", ErrorLog.ExtractError(error));
                    logger.Debug(ErrMsg);

                    KaizosSession.Current.ReturnURL = "frmMasterServiceTypeUpdate.aspx";
                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MasterServiceUnderDev").ToString();
                    Server.Transfer("frmResult.aspx", false);

                }
            }



        }

        //        protected void btnSubmit_Click(object sender, EventArgs e)
        //        {

        //            string ErrorMessage = string.Empty;
        //            try
        //            {

        //                List<SCarrierService> lstInsertCarrierService = new List<SCarrierService>();
        //                List<SCarrierService> lstUpdateCarrierService = new List<SCarrierService>();
        //                bool bupdate = false;
        //                bool binsert = true;
        //                // GridView MainContainer = (GridView)container.NamingContainer;
        //                for (int k = 8; k <= gvCarrierService1.Rows.Count; k = k + 9)
        //                {

        //                    string strServiceCode = string.Empty;
        //                    //int MasterServiceColumnStart = 2;//[3-1 id,carrier,fieldname]
        //                    for (int s = 1; s <= intMSCounter; s++)
        //                    {
        //                        CheckBox chkBox = (CheckBox)gvCarrierService1.Rows[k].FindControl("chkDis" + "Service" + s.ToString() + "K" + k.ToString());

        //                        SCarrierService sCarrierService = new SCarrierService();
        //                        TextBox txtBoxServiceId = (TextBox)gvCarrierService1.Rows[k - 8].FindControl("tb1" + "Service" + s.ToString() + "K" + (k - 8).ToString());
        //                        sCarrierService.ServiceID = Convert.ToInt32(txtBoxServiceId.Text);



        //                            TextBox txtBoxServiceCode = (TextBox)gvCarrierService1.Rows[k - 7].FindControl("tb1" + "Service" + s.ToString() + "K" + (k - 7).ToString());
        //                            TextBox txtBoxServiceName = (TextBox)gvCarrierService1.Rows[k - 6].FindControl("tb1" + "Service" + s.ToString() + "K" + (k - 6).ToString());
        //                            TextBox txtBoxDeliveryDeadLine = (TextBox)gvCarrierService1.Rows[k - 3].FindControl("tb1" + "Service" + s.ToString() + "K" + (k - 3).ToString());
        //                            TextBox txtBoxInformation = (TextBox)gvCarrierService1.Rows[k - 2].FindControl("tb1" + "Service" + s.ToString() + "K" + (k - 2).ToString());
        //                            TextBox txtBoxCarrierServiceCode = (TextBox)gvCarrierService1.Rows[k - 1].FindControl("tb1" + "Service" + s.ToString() + "K" + (k - 1).ToString());


        //                            sCarrierService.CarrierName = ((HiddenField)gvCarrierService1.Rows[k-7].FindControl("hdnCarrierName" + "Service" + s.ToString() + "K" + (k-7).ToString())).Value;
        //                            sCarrierService.MasterServiceName = ((HiddenField)gvCarrierService1.Rows[k].FindControl("hdnMS" + "Service" + s.ToString() + "K" + k.ToString())).Value;
        //                            sCarrierService.ServiceName = txtBoxServiceName.Text;
        //                            sCarrierService.ServiceCode = txtBoxServiceCode.Text;
        //                            strServiceCode = strServiceCode + "," + sCarrierService.ServiceCode; 
        //                            if(s>1)
        //                            {
        //                              if(strServiceCode.Contains(sCarrierService.ServiceCode))
        //                             {
        //                                 ErrorMessage = ErrorMessage + "Service Code :" + sCarrierService.ServiceCode + " Should be  Unique for the Carrier" + sCarrierService.CarrierName + "<br/>";
        //                                 break;
        //                              }
        //                            }
        //                            sCarrierService.DeliveryDelayTable = string.Empty;
        //                            sCarrierService.DeliveryDeadLine = txtBoxDeliveryDeadLine.Text;
        //                            sCarrierService.Information = txtBoxInformation.Text;
        //                            sCarrierService.CarrierServiceCode = txtBoxCarrierServiceCode.Text;
        //                            if (txtBoxInformation.Text.ToLower().Contains("http") || txtBoxInformation.Text.ToLower().Contains("www"))
        //                                sCarrierService.InfoType = "URL";
        //                            else
        //                                sCarrierService.InfoType = "TEXT";

        //                            sCarrierService.KeyCustomerService = SEnumFlag.No;
        //                            sCarrierService.Priority = SEnumPriority.Economy;

        //                          //  if(sCarrierService.ServiceName.


        //                            if (chkBox.Checked)
        //                            {
        //                                sCarrierService.Active = SEnumFlag.Yes;
        //                            }
        //                            else
        //                            {
        //                                sCarrierService.Active = SEnumFlag.No;
        //                            }
        //                            if (sCarrierService.ServiceID > 0)
        //                            {
        //                                if (sCarrierService.ServiceName.Trim().Equals(string.Empty))
        //                                {
        //                                    ErrorMessage = ErrorMessage + "Service Name " + "Should Not be Empty for " + sCarrierService.MasterServiceName + " for the Carrier" + sCarrierService.CarrierName + "<br/>";
        //                                }
        //                                if (sCarrierService.ServiceCode.Trim().Equals(string.Empty))
        //                                {
        //                                    ErrorMessage = ErrorMessage + "Service Code " + "Should Not be Empty for " + sCarrierService.MasterServiceName + " for the Carrier" + sCarrierService.CarrierName + "<br/>";
        //                                }
        //                                if (sCarrierService.DeliveryDeadLine.Trim().Equals(string.Empty))
        //                                {
        //                                    ErrorMessage = ErrorMessage + "DeliveryDeadLine " + "Should Not be Empty for " + sCarrierService.MasterServiceName + " for the Carrier" + sCarrierService.CarrierName + "<br/>";
        //                                }
        //                                if (!isValidTime(sCarrierService.DeliveryDeadLine))
        //                                {
        //                                    ErrorMessage = ErrorMessage + "DeliveryDeadLine  Time is not in proper format for " +  sCarrierService.MasterServiceName + " for the Carrier" + sCarrierService.CarrierName + "<br/>";
        //                                }
        //                                if (sCarrierService.CarrierServiceCode.Trim().Equals(string.Empty))
        //                                {
        //                                    ErrorMessage = ErrorMessage + "CarrierServiceCode " + "Should Not be Empty for " + sCarrierService.MasterServiceName + " for the Carrier" + sCarrierService.CarrierName + "<br/>";
        //                                }
        //                                if (ErrorMessage.Equals(string.Empty))
        //                                {
        //                                    lstUpdateCarrierService.Add(sCarrierService);
        //                                    bupdate = true;
        //                                }
        //                                else
        //                                {
        //                                    bupdate = false;
        //                                    break;

        //                                }


        //                            }
        //                            else
        //                            {
        //                                if (!chkBox.Checked)
        //                                {
        //                                    if (sCarrierService.ServiceName.Trim().Equals(string.Empty))
        //                                    {
        //                                        ErrorMessage = ErrorMessage + "Service Name " + "Should Not be Empty for " + sCarrierService.MasterServiceName + " for the Carrier" + sCarrierService.CarrierName + "<br/>";
        //                                    }
        //                                    if (sCarrierService.ServiceCode.Trim().Equals(string.Empty))
        //                                    {
        //                                        ErrorMessage = ErrorMessage + "Service Code " + "Should Not be Empty for " + sCarrierService.MasterServiceName + " for the Carrier" + sCarrierService.CarrierName + "<br/>";
        //                                    }
        //                                    if (sCarrierService.DeliveryDeadLine.Trim().Equals(string.Empty))
        //                                    {
        //                                        ErrorMessage = ErrorMessage + "DeliveryDeadLine " + "Should Not be Empty for " + sCarrierService.MasterServiceName + " for the Carrier" + sCarrierService.CarrierName + "<br/>";
        //                                    }
        //                                    if (!isValidTime(sCarrierService.DeliveryDeadLine))
        //                                    {
        //                                        ErrorMessage = ErrorMessage + "DeliveryDeadLine  Time is not in proper format for " + sCarrierService.MasterServiceName + " for the Carrier" + sCarrierService.CarrierName + "<br/>";
        //                                    }
        //                                    if (sCarrierService.CarrierServiceCode.Trim().Equals(string.Empty))
        //                                    {
        //                                        ErrorMessage = ErrorMessage + "CarrierServiceCode " + "Should Not be Empty for " + sCarrierService.MasterServiceName + " for the Carrier" + sCarrierService.CarrierName + "<br/>";
        //                                    }
        //                                    if (ErrorMessage.Equals(string.Empty))
        //                                    {

        //                                        lstInsertCarrierService.Add(sCarrierService);
        //                                        binsert = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        binsert = false;
        //                                        break;
        //                                    }
        //                                }



        //                            }


        //                    }
        //                }
        //                KaizosServiceAgent proxy = new KaizosServiceAgent();
        //                if (bupdate)
        //                {
        //                    proxy.UpdateCarrierService(lstUpdateCarrierService);
        //                    KaizosSession.Current.ErrorMessage = " Records sucessfully updated";
        //                }
        //                if (binsert)
        //                {
        //                    proxy.InsertCarrierService(lstInsertCarrierService);
        //                    KaizosSession.Current.ErrorMessage = " Records sucessfully updated";
        //                }

        //                if(!ErrorMessage.Equals(string.Empty))
        //                {
        //                     KaizosSession.Current.ErrorMessage = ErrorMessage;
        //                }
        //                KaizosSession.Current.ReturnURL = "frmMasterServiceTypeUpdate.aspx";
        //               //GetGlobalResourceObject("LocalString", "MasterServiceUnderDev").ToString();
        //                Server.Transfer("frmResult.aspx", false);//gvCarrierService1.Controls.Clear();

        //                //LoadCarrierServiceGrid(true, true);
        //            }
        //            /* Introduced faultexception handling and logging detailed exception into log4net file [05JAN12RM] */
        //            catch (FaultException<SGeneralFault> ex)
        //            {
        //                string ErrorDetails = ex.Detail.Details;
        //                string MethodName = ex.Detail.Issue;

        //                string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Service, MethodName, ErrorDetails);
        //                logger.Debug(ErrMsg);

        //                KaizosSession.Current.ReturnURL = "frmMasterServiceTypeUpdate.aspx";
        //                KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MasterServiceUnderDev").ToString();
        //                Server.Transfer("frmResult.aspx", false);

        //            }
        //            catch (Exception error)
        //            {
        //                /* Generalized exception handling and logging detailed exception into log4net file [17FEB12RM] */
        //                if (!error.GetBaseException().GetType().Name.Equals("ThreadAbortException"))
        //                {
        //                    string ErrMsg = ErrorLog.GetErrorLogMessage(KaizosSession.Current.UserId, ErrorSource.Client, "gvCarrierService_RowCommand", ErrorLog.ExtractError(error));
        //                    logger.Debug(ErrMsg);

        //                    KaizosSession.Current.ReturnURL = "frmMasterServiceTypeUpdate.aspx";
        //                    KaizosSession.Current.ErrorMessage = GetGlobalResourceObject("LocalString", "MasterServiceUnderDev").ToString();
        //                    Server.Transfer("frmResult.aspx", false);

        //                }
        //            }

        //            #region Commented Code
        //            ////Get Master Service List
        //            //KaizosServiceAgent proxy = new KaizosServiceAgent();

        //            //List<SComboText> sComboText = new List<SComboText>();
        //            //SComboTableField sComboTableField = new SComboTableField();
        //            //sComboTableField.FieldName = "MASTER_SERVICE_NAME";
        //            //sComboTableField.TableName = "MASTER_SERVICE";
        //            //sComboText = proxy.FillCombo(sComboTableField).ToList();

        //            //if (IsValid) /* IsValid 1.Valid all edit controls are filled and in correct format */
        //            //{
        //            //    /* 2. Form the tariff values */
        //            //    List<SCarrierService> sCarrierService = new List<SCarrierService>();

        //            //    for (int i = 0; i < gvCarrierService.Rows.Count; i++)
        //            //    {

        //            //        if (sComboText.Count > 0)  //Road
        //            //        {
        //            //            SCarrierService sCarrierServiceRoad = new SCarrierService();

        //            //            Label lblNewRoadServiceID = (Label)gvCarrierService.Rows[i].FindControl("lblRoadServiceID");
        //            //            sCarrierServiceRoad.ServiceID = Convert.ToInt32(lblNewRoadServiceID.Text.Trim());

        //            //            TextBox txtNewRoadServiceCode = (TextBox)gvCarrierService.Rows[i].FindControl("txtRoadServiceCode");
        //            //            sCarrierServiceRoad.ServiceCode = txtNewRoadServiceCode.Text.Trim();

        //            //            TextBox txtNewRoadDeliveryDeadLine = (TextBox)gvCarrierService.Rows[i].FindControl("txtRoadDeliveryDeadLine");
        //            //            sCarrierServiceRoad.DeliveryDeadLine = txtNewRoadDeliveryDeadLine.Text.Trim();

        //            //            TextBox txtNewRoadServiceName = (TextBox)gvCarrierService.Rows[i].FindControl("txtRoadServiceName");
        //            //            sCarrierServiceRoad.ServiceName = txtNewRoadServiceName.Text.Trim();


        //            //            sCarrierServiceRoad.MasterServiceName = lblRoadHeader.Text.Trim(); //23feb12
        //            //            sCarrierServiceRoad.KeyCustomerService = SEnumFlag.No;          //Dummy
        //            //            sCarrierServiceRoad.Priority = SEnumPriority.Economy; //Dummy

        //            //            CheckBox chkNewRoadActive = (CheckBox)gvCarrierService.Rows[i].FindControl("chkRoadActive");
        //            //            sCarrierServiceRoad.Active = chkNewRoadActive.Checked ? SEnumFlag.No : SEnumFlag.Yes; //if ticked in screen then Activate="N"

        //            //            Label lblNewCarrierName = (Label)gvCarrierService.Rows[i].FindControl("lblCarrierName");
        //            //            sCarrierServiceRoad.CarrierName = lblNewCarrierName.Text.Trim();

        //            //            sCarrierService.Add(sCarrierServiceRoad);
        //            //        }

        //            //        if (sComboText.Count > 1)  //AirB410
        //            //        {
        //            //            SCarrierService sCarrierServiceAirB410 = new SCarrierService();

        //            //            Label lblNewAirB410ServiceID = (Label)gvCarrierService.Rows[i].FindControl("lblAirB410ServiceID");
        //            //            sCarrierServiceAirB410.ServiceID = Convert.ToInt32(lblNewAirB410ServiceID.Text.Trim());

        //            //            TextBox txtNewAirB410ServiceCode = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB410ServiceCode");
        //            //            sCarrierServiceAirB410.ServiceCode = txtNewAirB410ServiceCode.Text.Trim();

        //            //            TextBox txtNewAirB410DeliveryDeadLine = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB410DeliveryDeadLine");
        //            //            sCarrierServiceAirB410.DeliveryDeadLine = txtNewAirB410DeliveryDeadLine.Text.Trim();

        //            //            TextBox txtNewAirB410ServiceName = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB410ServiceName");
        //            //            sCarrierServiceAirB410.ServiceName = txtNewAirB410ServiceName.Text.Trim();


        //            //            sCarrierServiceAirB410.MasterServiceName = lblAirB410Header.Text.Trim(); //23feb12
        //            //            sCarrierServiceAirB410.KeyCustomerService = SEnumFlag.No;          //Dummy
        //            //            sCarrierServiceAirB410.Priority = SEnumPriority.Economy; //Dummy

        //            //            CheckBox chkNewAirB410Active = (CheckBox)gvCarrierService.Rows[i].FindControl("chkAirB410Active");
        //            //            sCarrierServiceAirB410.Active = chkNewAirB410Active.Checked ? SEnumFlag.No : SEnumFlag.Yes; //if ticked in screen then Activate="N"

        //            //            Label lblNewCarrierName = (Label)gvCarrierService.Rows[i].FindControl("lblCarrierName");
        //            //            sCarrierServiceAirB410.CarrierName = lblNewCarrierName.Text.Trim();

        //            //            sCarrierService.Add(sCarrierServiceAirB410);
        //            //        }


        //            //        if (sComboText.Count > 2)  //AirB412
        //            //        {
        //            //            SCarrierService sCarrierServiceAirB412 = new SCarrierService();

        //            //            Label lblNewAirB412ServiceID = (Label)gvCarrierService.Rows[i].FindControl("lblAirB412ServiceID");
        //            //            sCarrierServiceAirB412.ServiceID = Convert.ToInt32(lblNewAirB412ServiceID.Text.Trim());

        //            //            TextBox txtNewAirB412ServiceCode = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB412ServiceCode");
        //            //            sCarrierServiceAirB412.ServiceCode = txtNewAirB412ServiceCode.Text.Trim();

        //            //            TextBox txtNewAirB412DeliveryDeadLine = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB412DeliveryDeadLine");
        //            //            sCarrierServiceAirB412.DeliveryDeadLine = txtNewAirB412DeliveryDeadLine.Text.Trim();

        //            //            TextBox txtNewAirB412ServiceName = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB412ServiceName");
        //            //            sCarrierServiceAirB412.ServiceName = txtNewAirB412ServiceName.Text.Trim();

        //            //            sCarrierServiceAirB412.MasterServiceName = lblAirB412Header.Text.Trim(); //23feb12
        //            //            sCarrierServiceAirB412.KeyCustomerService = SEnumFlag.No;          //Dummy
        //            //            sCarrierServiceAirB412.Priority = SEnumPriority.Economy; //Dummy

        //            //            CheckBox chkNewAirB412Active = (CheckBox)gvCarrierService.Rows[i].FindControl("chkAirB412Active");
        //            //            sCarrierServiceAirB412.Active = chkNewAirB412Active.Checked ? SEnumFlag.No : SEnumFlag.Yes; //if ticked in screen then Activate="N"

        //            //            Label lblNewCarrierName = (Label)gvCarrierService.Rows[i].FindControl("lblCarrierName");
        //            //            sCarrierServiceAirB412.CarrierName = lblNewCarrierName.Text.Trim();


        //            //            sCarrierService.Add(sCarrierServiceAirB412);
        //            //        }

        //            //        if (sComboText.Count > 3)  //AirB418
        //            //        {
        //            //            SCarrierService sCarrierServiceAirB418 = new SCarrierService();

        //            //            Label lblNewAirB418ServiceID = (Label)gvCarrierService.Rows[i].FindControl("lblAirB418ServiceID");
        //            //            sCarrierServiceAirB418.ServiceID = Convert.ToInt32(lblNewAirB418ServiceID.Text.Trim());

        //            //            TextBox txtNewAirB418ServiceCode = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB418ServiceCode");
        //            //            sCarrierServiceAirB418.ServiceCode = txtNewAirB418ServiceCode.Text.Trim();

        //            //            TextBox txtNewAirB418DeliveryDeadLine = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB418DeliveryDeadLine");
        //            //            sCarrierServiceAirB418.DeliveryDeadLine = txtNewAirB418DeliveryDeadLine.Text.Trim();

        //            //            TextBox txtNewAirB418ServiceName = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB418ServiceName");
        //            //            sCarrierServiceAirB418.ServiceName = txtNewAirB418ServiceName.Text.Trim();

        //            //            sCarrierServiceAirB418.MasterServiceName = lblAirB418Header.Text.Trim(); //23feb12
        //            //            sCarrierServiceAirB418.KeyCustomerService = SEnumFlag.No;          //Dummy
        //            //            sCarrierServiceAirB418.Priority = SEnumPriority.Economy; //Dummy

        //            //            CheckBox chkNewAirB418Active = (CheckBox)gvCarrierService.Rows[i].FindControl("chkAirB418Active");
        //            //            sCarrierServiceAirB418.Active = chkNewAirB418Active.Checked ? SEnumFlag.No : SEnumFlag.Yes; //if ticked in screen then Activate="N"

        //            //            Label lblNewCarrierName = (Label)gvCarrierService.Rows[i].FindControl("lblCarrierName");
        //            //            sCarrierServiceAirB418.CarrierName = lblNewCarrierName.Text.Trim();

        //            //            sCarrierService.Add(sCarrierServiceAirB418);
        //            //        }



        //            //    }

        //            //    ///* 5. Validaton: Check service code repeated */
        //            //    //if (!CheckRedundantValues(sCarrierService))
        //            //    //{
        //            //    //    return;
        //            //    //}

        //            //    /* 6.call business method using proxy and update the record */


        //                /* 7.Cancel edit mode */
        //              //  gvCarrierService.EditIndex = -1;

        //                /* 8.Load again updated DataBind */
        //            // }  
        //#endregion

        //        }



        //protected void btnSubmit1_Click(object sender, EventArgs e)
        //{
        //    //Get Master Service List
        //    KaizosServiceAgent proxy = new KaizosServiceAgent();

        //    List<SComboText> sComboText         = new List<SComboText>();
        //    SComboTableField sComboTableField   = new SComboTableField();
        //    sComboTableField.FieldName          = "MASTER_SERVICE_NAME";
        //    sComboTableField.TableName          = "MASTER_SERVICE";
        //    sComboText = proxy.FillCombo(sComboTableField).ToList();

        //    if (IsValid) /* IsValid 1.Valid all edit controls are filled and in correct format */
        //    {
        //        /* 2. Form the tariff values */
        //        List<SCarrierService> sCarrierService = new List<SCarrierService>();

        //        for (int i = 0; i < gvCarrierService.Rows.Count; i++)
        //        {

        //            if (sComboText.Count > 0)  //Road
        //            {
        //                SCarrierService sCarrierServiceRoad = new SCarrierService();

        //                Label lblNewRoadServiceID           = (Label)gvCarrierService.Rows[i].FindControl("lblRoadServiceID");
        //                sCarrierServiceRoad.ServiceID       = Convert.ToInt32(lblNewRoadServiceID.Text.Trim());

        //                TextBox txtNewRoadServiceCode       = (TextBox)gvCarrierService.Rows[i].FindControl("txtRoadServiceCode");
        //                sCarrierServiceRoad.ServiceCode     = txtNewRoadServiceCode.Text.Trim();

        //                TextBox txtNewRoadDeliveryDeadLine   = (TextBox)gvCarrierService.Rows[i].FindControl("txtRoadDeliveryDeadLine");
        //                sCarrierServiceRoad.DeliveryDeadLine = txtNewRoadDeliveryDeadLine.Text.Trim();

        //                TextBox txtNewRoadServiceName = (TextBox)gvCarrierService.Rows[i].FindControl("txtRoadServiceName");
        //                sCarrierServiceRoad.ServiceName = txtNewRoadServiceName.Text.Trim();


        //                sCarrierServiceRoad.MasterServiceName = lblRoadHeader.Text.Trim(); //23feb12
        //                sCarrierServiceRoad.KeyCustomerService = SEnumFlag.No;          //Dummy
        //                sCarrierServiceRoad.Priority = SEnumPriority.Economy; //Dummy

        //                CheckBox chkNewRoadActive = (CheckBox)gvCarrierService.Rows[i].FindControl("chkRoadActive");
        //                sCarrierServiceRoad.Active = chkNewRoadActive.Checked ? SEnumFlag.No : SEnumFlag.Yes; //if ticked in screen then Activate="N"

        //                Label lblNewCarrierName = (Label)gvCarrierService.Rows[i].FindControl("lblCarrierName");
        //                sCarrierServiceRoad.CarrierName = lblNewCarrierName.Text.Trim();

        //                sCarrierService.Add(sCarrierServiceRoad);
        //            }

        //            if (sComboText.Count > 1)  //AirB410
        //            {
        //                SCarrierService sCarrierServiceAirB410 = new SCarrierService();

        //                Label lblNewAirB410ServiceID = (Label)gvCarrierService.Rows[i].FindControl("lblAirB410ServiceID");
        //                sCarrierServiceAirB410.ServiceID = Convert.ToInt32(lblNewAirB410ServiceID.Text.Trim());

        //                TextBox txtNewAirB410ServiceCode = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB410ServiceCode");
        //                sCarrierServiceAirB410.ServiceCode = txtNewAirB410ServiceCode.Text.Trim();

        //                TextBox txtNewAirB410DeliveryDeadLine = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB410DeliveryDeadLine");
        //                sCarrierServiceAirB410.DeliveryDeadLine = txtNewAirB410DeliveryDeadLine.Text.Trim();

        //                TextBox txtNewAirB410ServiceName = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB410ServiceName");
        //                sCarrierServiceAirB410.ServiceName = txtNewAirB410ServiceName.Text.Trim();


        //                sCarrierServiceAirB410.MasterServiceName = lblAirB410Header.Text.Trim(); //23feb12
        //                sCarrierServiceAirB410.KeyCustomerService = SEnumFlag.No;          //Dummy
        //                sCarrierServiceAirB410.Priority = SEnumPriority.Economy; //Dummy

        //                CheckBox chkNewAirB410Active = (CheckBox)gvCarrierService.Rows[i].FindControl("chkAirB410Active");
        //                sCarrierServiceAirB410.Active = chkNewAirB410Active.Checked ? SEnumFlag.No : SEnumFlag.Yes; //if ticked in screen then Activate="N"

        //                Label lblNewCarrierName = (Label)gvCarrierService.Rows[i].FindControl("lblCarrierName");
        //                sCarrierServiceAirB410.CarrierName = lblNewCarrierName.Text.Trim();

        //                sCarrierService.Add(sCarrierServiceAirB410);
        //            }


        //            if (sComboText.Count > 2)  //AirB412
        //            {
        //                SCarrierService sCarrierServiceAirB412 = new SCarrierService();

        //                Label lblNewAirB412ServiceID = (Label)gvCarrierService.Rows[i].FindControl("lblAirB412ServiceID");
        //                sCarrierServiceAirB412.ServiceID = Convert.ToInt32(lblNewAirB412ServiceID.Text.Trim());

        //                TextBox txtNewAirB412ServiceCode = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB412ServiceCode");
        //                sCarrierServiceAirB412.ServiceCode = txtNewAirB412ServiceCode.Text.Trim();

        //                TextBox txtNewAirB412DeliveryDeadLine = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB412DeliveryDeadLine");
        //                sCarrierServiceAirB412.DeliveryDeadLine = txtNewAirB412DeliveryDeadLine.Text.Trim();

        //                TextBox txtNewAirB412ServiceName = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB412ServiceName");
        //                sCarrierServiceAirB412.ServiceName = txtNewAirB412ServiceName.Text.Trim();

        //                sCarrierServiceAirB412.MasterServiceName = lblAirB412Header.Text.Trim(); //23feb12
        //                sCarrierServiceAirB412.KeyCustomerService = SEnumFlag.No;          //Dummy
        //                sCarrierServiceAirB412.Priority = SEnumPriority.Economy; //Dummy

        //                CheckBox chkNewAirB412Active = (CheckBox)gvCarrierService.Rows[i].FindControl("chkAirB412Active");
        //                sCarrierServiceAirB412.Active = chkNewAirB412Active.Checked ? SEnumFlag.No : SEnumFlag.Yes; //if ticked in screen then Activate="N"

        //                Label lblNewCarrierName = (Label)gvCarrierService.Rows[i].FindControl("lblCarrierName");
        //                sCarrierServiceAirB412.CarrierName = lblNewCarrierName.Text.Trim();


        //                sCarrierService.Add(sCarrierServiceAirB412);
        //            }

        //            if (sComboText.Count > 3)  //AirB418
        //            {
        //                SCarrierService sCarrierServiceAirB418 = new SCarrierService();

        //                Label lblNewAirB418ServiceID = (Label)gvCarrierService.Rows[i].FindControl("lblAirB418ServiceID");
        //                sCarrierServiceAirB418.ServiceID = Convert.ToInt32(lblNewAirB418ServiceID.Text.Trim());

        //                TextBox txtNewAirB418ServiceCode = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB418ServiceCode");
        //                sCarrierServiceAirB418.ServiceCode = txtNewAirB418ServiceCode.Text.Trim();

        //                TextBox txtNewAirB418DeliveryDeadLine = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB418DeliveryDeadLine");
        //                sCarrierServiceAirB418.DeliveryDeadLine = txtNewAirB418DeliveryDeadLine.Text.Trim();

        //                TextBox txtNewAirB418ServiceName = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB418ServiceName");
        //                sCarrierServiceAirB418.ServiceName = txtNewAirB418ServiceName.Text.Trim();

        //                sCarrierServiceAirB418.MasterServiceName = lblAirB418Header.Text.Trim(); //23feb12
        //                sCarrierServiceAirB418.KeyCustomerService = SEnumFlag.No;          //Dummy
        //                sCarrierServiceAirB418.Priority = SEnumPriority.Economy; //Dummy

        //                CheckBox chkNewAirB418Active = (CheckBox)gvCarrierService.Rows[i].FindControl("chkAirB418Active");
        //                sCarrierServiceAirB418.Active = chkNewAirB418Active.Checked ? SEnumFlag.No : SEnumFlag.Yes; //if ticked in screen then Activate="N"

        //                Label lblNewCarrierName = (Label)gvCarrierService.Rows[i].FindControl("lblCarrierName");
        //                sCarrierServiceAirB418.CarrierName = lblNewCarrierName.Text.Trim();

        //                sCarrierService.Add(sCarrierServiceAirB418);
        //            }



        //        }

        //        ///* 5. Validaton: Check service code repeated */
        //        //if (!CheckRedundantValues(sCarrierService))
        //        //{
        //        //    return;
        //        //}

        //        /* 6.call business method using proxy and update the record */
        //        proxy.UpdateCarrierService(sCarrierService);

        //        /* 7.Cancel edit mode */
        //        gvCarrierService.EditIndex = -1;

        //        /* 8.Load again updated DataBind */
        //        LoadCarrierServiceGrid(false);

        //    }
        //}


        //23FEB12RM
        //protected bool CheckRedundantValues(List<SCarrierService> sCarrierService)
        //{
        //    val_Redundant.IsValid   = true;
        //    string strError         = "";

        //    for (int i = 0; i < sCarrierService.Count; i++)
        //    {

        //        string ServiceCodeSearchKey = sCarrierService[i].ServiceCode;
        //        int ServiceCodeCount = sCarrierService.Count(s => s.ServiceCode == ServiceCodeSearchKey);
        //        if (ServiceCodeCount > 1 && sCarrierService[i].ServiceCode.Trim().Length!=0)
        //        {
        //            strError = strError + "*" + ServiceCodeSearchKey.Trim() + " " + GetGlobalResourceObject("LocalString", "MasterServiceRepeatedIn").ToString() + " [" + sCarrierService[i].CarrierName + "][" + sCarrierService[i].MasterServiceName + "]" + "<br>";
        //            val_Redundant.IsValid = false;
        //        }

        //        string ServiceNameSearchKey = sCarrierService[i].ServiceName;
        //        int ServiceNameCount = sCarrierService.Count(s => s.ServiceName == ServiceNameSearchKey);
        //        if (ServiceNameCount > 1 && sCarrierService[i].ServiceName.Trim().Length!=0)
        //        {
        //            strError = strError + "*" + ServiceNameSearchKey.Trim() + " " + GetGlobalResourceObject("LocalString", "MasterServiceRepeatedIn").ToString() +  " ["+sCarrierService[i].CarrierName + "][" + sCarrierService[i].MasterServiceName + "]" + "<br>";
        //            val_Redundant.IsValid = false;
        //        }


        //        ServiceCodeCount = 0;
        //        ServiceNameCount = 0;
        //    }

        //    if (!val_Redundant.IsValid)
        //    {
        //        val_Redundant.ErrorMessage = strError;
        //    }

        //    return val_Redundant.IsValid;
        //}

        //23FEB12RM



        protected bool isValidTime(string Value)
        {
            bool result = true;
            KaizosServiceAgent proxy = new KaizosServiceAgent();
            if (proxy.ValidateTime(Value) == 1)
            {
                result = false;
            }

            return result;

        }

        //Added validation [21FEB12RM]
        //protected void val_MasterTypeUpdate_ServerValidate(object source, ServerValidateEventArgs args)
        //{

        //    args.IsValid    = true;
        //    string strError = "";

        //    //Get Master Service List
        //    KaizosServiceAgent proxy                = new KaizosServiceAgent();
        //    List<SCarrierService> sCarrierService   = proxy.GetCarrierService();
        //    List<String> MasterServiceList          = GetMasterServiceName(sCarrierService);


        //    for (int i = 0; i < gvCarrierService.Rows.Count; i++)
        //    {

        //        Label lblNewCarrierName = (Label)gvCarrierService.Rows[i].FindControl("lblCarrierName");

        //        #region Validate Road (1st column)

        //        if (MasterServiceList.Count > 0)
        //        {

        //            //1. Service Name is not empty
        //            Label lblNewServiceNameCaption = (Label)gvCarrierService.Rows[i].FindControl("lblServiceNameCaption");
        //            TextBox txtNewRoadServiceName = (TextBox)gvCarrierService.Rows[i].FindControl("txtRoadServiceName");

        //            if (txtNewRoadServiceName.Text.Trim().Length == 0)
        //            {
        //                args.IsValid = false;
        //                strError = strError + "* [" + lblNewCarrierName.Text.Trim() + "][" + lblRoadHeader.Text + "] " + lblNewServiceNameCaption.Text + " " + GetGlobalResourceObject("LocalString", "MasterTypeNotEmpty").ToString() + "<br>";
        //            }


        //            //2. Service code is not empty
        //            Label lblNewServiceCodeCaption = (Label)gvCarrierService.Rows[i].FindControl("lblServiceCodeCaption");
        //            TextBox txtNewRoadServiceCode = (TextBox)gvCarrierService.Rows[i].FindControl("txtRoadServiceCode");

        //            if (txtNewRoadServiceCode.Text.Trim().Length == 0)
        //            {
        //                args.IsValid = false;
        //                strError = strError + "* [" + lblNewCarrierName.Text.Trim() + "][" + lblRoadHeader.Text + "] " + lblNewServiceCodeCaption + " " + GetGlobalResourceObject("LocalString", "MasterTypeNotEmpty").ToString() + "<br>";
        //            }

        //            //3.delivery deadline is not empty HH:MM format.
        //            TextBox txtNewRoadDeliveryDeadLine = (TextBox)gvCarrierService.Rows[i].FindControl("txtRoadDeliveryDeadLine");

        //            Label lblNewDeadLineCaption = (Label) gvCarrierService.Rows[i].FindControl("lblDeadLineCaption");

        //            if (txtNewRoadDeliveryDeadLine.Text.Trim().Length == 0)
        //            {
        //                args.IsValid = false;
        //                strError = strError + "* [" + lblNewCarrierName.Text.Trim() + "][" + lblRoadHeader.Text + "]" + lblNewDeadLineCaption.Text + " " + GetGlobalResourceObject("LocalString", "MasterTypeNotEmpty").ToString() + "<br>";
        //            }
        //            else if (!isValidTime(txtNewRoadDeliveryDeadLine.Text))
        //            {
        //                args.IsValid = false;
        //                strError = strError + "* [" + lblNewCarrierName.Text.Trim() + "][" + lblRoadHeader.Text + "]" + lblNewDeadLineCaption.Text + " " + GetGlobalResourceObject("LocalString", "MastTypeInvlidFormat").ToString() + "<br>";
        //            }
        //        }

        //        #endregion

        //        #region Validate Air b4 10 (2nd column)
        //        if (MasterServiceList.Count > 1)
        //        {

        //            //1. Service Name is not empty
        //            Label lblNewServiceNameCaption = (Label)gvCarrierService.Rows[i].FindControl("lblServiceNameCaption");
        //            TextBox txtNewAirB410ServiceName = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB410ServiceName");

        //            if (txtNewAirB410ServiceName.Text.Trim().Length == 0)
        //            {
        //                args.IsValid = false;
        //                strError = strError + "* [" + lblNewCarrierName.Text.Trim() + "][" + lblAirB410Header.Text + "] " + lblNewServiceNameCaption.Text + " " + GetGlobalResourceObject("LocalString", "MasterTypeNotEmpty").ToString() + "<br>";
        //            }

        //            //2. Service code is not empty
        //            TextBox txtNewAirB410ServiceCode = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB410ServiceCode");
        //            Label lblNewServiceCodeCaption = (Label)gvCarrierService.Rows[i].FindControl("lblServiceCodeCaption");

        //            if (txtNewAirB410ServiceCode.Text.Trim().Length == 0)
        //            {
        //                args.IsValid = false;
        //                strError = strError + "* [" + lblNewCarrierName.Text.Trim() + "][" + lblAirB410Header.Text + "] " + lblNewServiceCodeCaption.Text + " " + GetGlobalResourceObject("LocalString", "MasterTypeNotEmpty").ToString() + "<br>";
        //            }

        //            //3.delivery deadline is not empty 
        //            TextBox txtNewAirB410DeliveryDeadLine = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB410DeliveryDeadLine");

        //            Label lblNewDeadLineCaption = (Label)gvCarrierService.Rows[i].FindControl("lblDeadLineCaption");

        //            if (txtNewAirB410DeliveryDeadLine.Text.Trim().Length == 0)
        //            {
        //                args.IsValid = false;
        //                strError = strError + "* [" + lblNewCarrierName.Text.Trim() + "][" + lblAirB410Header.Text + "]" + lblNewDeadLineCaption.Text + " " + GetGlobalResourceObject("LocalString", "MasterTypeNotEmpty").ToString() + "<br>";
        //            }
        //            else if (!isValidTime(txtNewAirB410DeliveryDeadLine.Text)) // should be in "HH:MM" format.
        //            {
        //                args.IsValid = false;
        //                strError = strError + "* [" + lblNewCarrierName.Text.Trim() + "][" + lblAirB410Header.Text + "]" + lblNewDeadLineCaption.Text + " " + GetGlobalResourceObject("LocalString", "MastTypeInvlidFormat").ToString() + "<br>";
        //            }
        //        }
        //        #endregion


        //        #region Validate Air b4 12 (3rd column)
        //        if (MasterServiceList.Count > 2)
        //        {

        //            //1. Service Name is not empty
        //            Label lblNewServiceNameCaption = (Label)gvCarrierService.Rows[i].FindControl("lblServiceNameCaption");
        //            TextBox txtNewAirB412ServiceName = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB412ServiceName");

        //            if (txtNewAirB412ServiceName.Text.Trim().Length == 0)
        //            {
        //                args.IsValid = false;
        //                strError = strError + "* [" + lblNewCarrierName.Text.Trim() + "][" + lblAirB412Header.Text + "] " + lblNewServiceNameCaption.Text + " " + GetGlobalResourceObject("LocalString", "MasterTypeNotEmpty").ToString() + "<br>";
        //            }


        //            //2. Service code is not empty
        //            TextBox txtNewAirB412ServiceCode = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB412ServiceCode");
        //            Label lblNewServiceCodeCaption = (Label)gvCarrierService.Rows[i].FindControl("lblServiceCodeCaption");

        //            if (txtNewAirB412ServiceCode.Text.Trim().Length == 0)
        //            {
        //                args.IsValid = false;
        //                strError = strError + "* [" + lblNewCarrierName.Text.Trim() + "][" + lblAirB412Header.Text + "] " + lblNewServiceCodeCaption.Text + " "+GetGlobalResourceObject("LocalString", "MasterTypeNotEmpty").ToString() + "<br>";
        //            }

        //            //3.delivery deadline is not empty 
        //            TextBox txtNewAirB412DeliveryDeadLine = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB412DeliveryDeadLine");
        //            Label lblNewDeadLineCaption = (Label)gvCarrierService.Rows[i].FindControl("lblDeadLineCaption");

        //            if (txtNewAirB412DeliveryDeadLine.Text.Trim().Length == 0)
        //            {
        //                args.IsValid = false;
        //                strError = strError + "* [" + lblNewCarrierName.Text.Trim() + "][" + lblAirB412Header.Text + "]" + lblNewDeadLineCaption.Text + " " +GetGlobalResourceObject("LocalString", "MasterTypeNotEmpty").ToString() + "<br>";
        //            }
        //            else if (!isValidTime(txtNewAirB412DeliveryDeadLine.Text)) // should be in "HH:MM" format.
        //            {
        //                args.IsValid = false;
        //                strError = strError + "* [" + lblNewCarrierName.Text.Trim() + "][" + lblAirB412Header.Text + "]" + lblNewDeadLineCaption.Text + " " + GetGlobalResourceObject("LocalString", "MastTypeInvlidFormat").ToString() + "<br>";
        //            }
        //        }

        //        #endregion

        //        #region Validate Air b4 18 (4th column)

        //        if (MasterServiceList.Count > 3)
        //        {

        //            //1. Service Name is not empty
        //            Label lblNewServiceNameCaption = (Label)gvCarrierService.Rows[i].FindControl("lblServiceNameCaption");
        //            TextBox txtNewAirB418ServiceName = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB418ServiceName");

        //            if (txtNewAirB418ServiceName.Text.Trim().Length == 0)
        //            {
        //                args.IsValid = false;
        //                strError = strError + "* [" + lblNewCarrierName.Text.Trim() + "][" + lblAirB418Header.Text + "] " + lblNewServiceNameCaption.Text + GetGlobalResourceObject("LocalString", "MasterTypeNotEmpty").ToString() + "<br>";
        //            }


        //            //2. Service code is not empty
        //            TextBox txtNewAirB418ServiceCode = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB418ServiceCode");
        //            Label lblNewServiceCodeCaption = (Label)gvCarrierService.Rows[i].FindControl("lblServiceCodeCaption");

        //            if (txtNewAirB418ServiceCode.Text.Trim().Length == 0)
        //            {
        //                args.IsValid = false;
        //                strError = strError + "* [" + lblNewCarrierName.Text.Trim() + "][" + lblAirB418Header.Text + "] " + lblNewServiceCodeCaption.Text + GetGlobalResourceObject("LocalString", "MasterTypeNotEmpty").ToString() + "<br>";
        //            }

        //            //3.delivery deadline is not empty 
        //            TextBox txtNewAirB418DeliveryDeadLine = (TextBox)gvCarrierService.Rows[i].FindControl("txtAirB418DeliveryDeadLine");
        //            Label lblNewDeadLineCaption = (Label)gvCarrierService.Rows[i].FindControl("lblDeadLineCaption");

        //            if (txtNewAirB418DeliveryDeadLine.Text.Trim().Length == 0)
        //            {
        //                args.IsValid = false;
        //                strError = strError + "* [" + lblNewCarrierName.Text.Trim() + "][" + lblAirB418Header.Text + "]" + lblNewDeadLineCaption.Text + GetGlobalResourceObject("LocalString", "MasterTypeNotEmpty").ToString() + "<br>";
        //            }
        //            else if (!isValidTime(txtNewAirB418DeliveryDeadLine.Text)) // should be in "HH:MM" format.
        //            {
        //                args.IsValid = false;
        //                strError = strError + "* [" + lblNewCarrierName.Text.Trim() + "][" + lblNewDeadLineCaption.Text + "]" + lblNewDeadLineCaption.Text + " " + GetGlobalResourceObject("LocalString", "MastTypeInvlidFormat").ToString() + "<br>";
        //            }
        //        }
        //        #endregion

        //    }

        //    if (!(args.IsValid))
        //    {
        //        val_MasterTypeUpdate.ErrorMessage = strError;
        //    }

        //}

        //protected void val_Redundant_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    args.IsValid = true;
        //}

        //    protected void myGrid_RowDataBound(object sender, GridViewRowEventArgs e)  

        //{  

        //        if (e.Row.RowType == DataControlRowType.DataRow)  

        //        {  

        //            Customer cust = e.Row.DataItem as Customer;  

        //            if (!cust.ShowURL)  

        //            {  

        //                LinkButton lnkWebURL = e.Row.FindControl("lnk") as LinkButton;  

        //                if (lnkWebURL != null)  

        //                {  

        //                    lnkWebURL.Visible = false;  

        //                }  

        //            }  

        //        }  

        //    } 


    }

    public class GridViewTemplate : Control, ITemplate
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(frmMasterServiceTypeUpdate));

        //A variable to hold the type of ListItemType.
        ListItemType _templateType;
        public string CommandName { get; set; }
        //A variable to hold the column name.
        string _columnName;
        string _caption;
        Button bt;
        LinkButton lnk;

        bool isPost;
        bool modify;
        //Constructor where we define the template type and column name.
        public GridViewTemplate(ListItemType type, string colname, string Caption, bool post, bool isModified)
        {
            //Stores the template type.
            _templateType = type;
            //CommandName = "Import";
            //Stores the column name.
            _columnName = colname;
            EnableViewState = true;
            _caption = Caption;
            isPost = post;
            modify = isModified;
        }

        void ITemplate.InstantiateIn(System.Web.UI.Control container)
        {
            switch (_templateType)
            {
                case ListItemType.Header:
                    //Creates a new label control and add it to the container.
                    Label lbl = new Label();    //Allocates the new label object.
                    lbl.ID = "lbl" + _caption;
                    // lbl.DataBinding += new EventHandler(lbl2_DataBinding); 
                    lbl.Text = _columnName;             //Assigns the name of the column in the lable.
                    container.Controls.Add(lbl);
                    //Adds the newly created label control to the container.
                    break;

                case ListItemType.Item:
                    //Creates a new text box control and add it to the container.
                    if (_columnName == "FieldName" || _columnName == "Carrier" || _columnName == "ID")
                    {
                        Label lbl2 = new Label();            //Allocates the new label object.
                        lbl2.Text = _columnName;

                        lbl2.DataBinding += new EventHandler(lbl2_DataBinding);


                        //Assigns the name of the column in the lable.
                        container.Controls.Add(lbl2);        //Adds the newly created label control to the container.
                        if (lbl2.Text == "ServiceID")
                        {
                            container.Visible = false;
                        }
                    }
                    else
                    {
                        Label lbl3 = new Label();            //Allocates the new label object.
                        //lbl2.Text = _columnName;
                        //GridViewRow a = (GridViewRow)container;
                        //  lbl3.ID = "lbl3" + _caption;
                        lbl3.DataBinding += new EventHandler(lbl3_DataBinding);
                        container.Controls.Add(lbl3);
                        Label lbl4 = new Label();            //Allocates the new label object.
                        //lbl2.Text = _columnName;
                        //GridViewRow a = (GridViewRow)container;
                        //  lbl4.ID = "lbl4" + _caption;
                        lbl4.DataBinding += new EventHandler(lbl4_DataBinding);
                        container.Controls.Add(lbl4);
                        Label lbl5 = new Label();            //Allocates the new label object.
                        //lbl2.Text = _columnName;
                        //GridViewRow a = (GridViewRow)container;
                        //  lbl5.ID = "lbl5" + _caption;
                        lbl5.DataBinding += new EventHandler(lbl5_DataBinding);
                        container.Controls.Add(lbl5);

                        TextBox tb1 = new TextBox(); //Allocates the new text box object.

                        // tb1.ID = "tb1" + _caption;

                        tb1.DataBinding += new EventHandler(tb1_DataBinding);
                        //Attaches the data binding event.
                        tb1.Columns = 4;                                        //Creates a column with size 4.
                        container.Controls.Add(tb1);

                        HiddenField hdn = new HiddenField();

                        //     hdn.ID = "hdn" + _caption;

                        hdn.DataBinding += new EventHandler(hdn_DataBinding);
                        container.Controls.Add(hdn);
                        HiddenField hdnMS = new HiddenField();

                        //    hdnMS.ID = "hdnMS" + _caption;

                        hdnMS.DataBinding += new EventHandler(hdnMS_DataBinding);
                        container.Controls.Add(hdnMS);
                        HiddenField hdnCarrierName = new HiddenField();

                        //    hdnCarrierName.ID = "hdnCarrierName" + _caption;

                        hdnCarrierName.DataBinding += new EventHandler(hdnCarrierName_DataBinding);
                        container.Controls.Add(hdnCarrierName);


                        CheckBox chkDis = new CheckBox();

                        //    chkDis.ID = "Chk1" + _caption;


                        // chkDis.CheckedChanged += new EventHandler(chkDis_Change);

                        chkDis.DataBinding += new EventHandler(chkDis_DataBinding);





                        //chkDis.DataBinding += new EventHandler(chkDis_DataBinding);
                        // chkDis.AutoPostBack = true;
                        container.Controls.Add(chkDis);

                        lnk = new LinkButton();

                        //    lnk.ID = "lnk1" + _caption;

                        lnk.Text = "Delivery Delay";
                        lnk.Click += new EventHandler(lnk_Click);
                        lnk.DataBinding += new EventHandler(lnk_DataBinding);
                        // lnk.Click += new EventHandler(lnk_Click);

                        container.Controls.Add(lnk);

                        bt = new Button();

                        //   bt.ID = "bt1" + _caption;

                        bt.Text = "Import";
                        bt.CausesValidation = false;
                        // 
                        bt.Click += new EventHandler(bt_Click);
                        bt.DataBinding += new EventHandler(bt_DataBinding);
                        container.Controls.Add(bt);
                    }                           //Adds the newly created textbox to the container.
                    break;

                case ListItemType.EditItem:
                    //As, I am not using any EditItem, I didnot added any code here.
                    break;

                case ListItemType.Footer:
                    CheckBox chkColumn = new CheckBox();
                    chkColumn.ID = "Chk";
                    container.Controls.Add(chkColumn);
                    break;
            }
        }

        /// <summary>
        /// This is the event, which will be raised when the binding happens.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        void tb1_DataBinding(object sender, EventArgs e)
        {
            TextBox txtdata = (TextBox)sender;
            GridViewRow container = (GridViewRow)txtdata.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, _columnName);
            int i = container.RowIndex;
            txtdata.ID = "tb1" + _caption + "K" + i.ToString();
            if (dataValue != DBNull.Value)
            {



                if (!isPost || modify)
                {
                    txtdata.Text = dataValue.ToString();
                }
                if (txtdata.Text.Trim().Equals(string.Empty))
                {
                    txtdata.Text = dataValue.ToString();
                }

                if (dataValue.ToString().Equals("Yes") || dataValue.ToString().Equals("No"))
                {
                    txtdata.Visible = false;


                }
                else if (dataValue.ToString().Contains("DELIVERY_DELAY_") || dataValue.ToString().Equals("NoTable"))
                {
                    txtdata.Visible = false;
                }

            }
        }
        void hdn_DataBinding(object sender, EventArgs e)
        {
            HiddenField hdndata = (HiddenField)sender;
            GridViewRow container = (GridViewRow)hdndata.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, _columnName);
            int i = container.RowIndex;

            hdndata.ID = "hdn" + _caption + "K" + i.ToString();

            if (dataValue != DBNull.Value)
            {

                hdndata.Value = dataValue.ToString();

                //if (!isPost)
                //{
                //    txtdata.Text = dataValue.ToString();
                //}

                //if (dataValue.ToString().Equals("Yes") || dataValue.ToString().Equals("No"))
                //{
                //    txtdata.Visible = false;


                //}
                //else if (dataValue.ToString().Contains("DELIVERY_DELAY_") || dataValue.ToString().Equals("NoTable"))
                //{
                //    txtdata.Visible = false;
                //}

            }
        }

        void hdnMS_DataBinding(object sender, EventArgs e)
        {
            HiddenField hdndata = (HiddenField)sender;
            GridViewRow container = (GridViewRow)hdndata.NamingContainer;
            hdndata.Value = this._columnName;
            int i = container.RowIndex;
            hdndata.ID = "hdnMS" + _caption + "K" + i.ToString();


        }
        void hdnCarrierName_DataBinding(object sender, EventArgs e)
        {
            HiddenField hdndata = (HiddenField)sender;
            GridViewRow container = (GridViewRow)hdndata.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, "Carrier");
            int i = container.RowIndex;
            hdndata.ID = "hdnCarrierName" + _caption + "K" + i.ToString();
            if (dataValue != DBNull.Value)
            {
                hdndata.Value = dataValue.ToString();
            }



        }

        void lbl2_DataBinding(object sender, EventArgs e)
        {
            Label lbldata = (Label)sender;
            GridViewRow container = (GridViewRow)lbldata.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, _columnName);

            if (dataValue != DBNull.Value)
            {
                lbldata.Text = dataValue.ToString();
                if (lbldata.Text == "ServiceID")
                {
                    container.Visible = false;
                }
            }
        }
        void lbl3_DataBinding(object sender, EventArgs e)
        {
            Label lbldata = (Label)sender;
            GridViewRow container = (GridViewRow)lbldata.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, "ID");
            int i = container.RowIndex;


            lbldata.ID = "lbl3" + _caption + "K" + i.ToString();

            lbldata.Visible = false;
            if (dataValue != DBNull.Value)
            {
                lbldata.Text = dataValue.ToString();
                if (lbldata.Text == "ServiceID")
                {
                    container.Visible = false;
                }
            }
        }
        void lbl4_DataBinding(object sender, EventArgs e)
        {
            Label lbldata = (Label)sender;
            GridViewRow container = (GridViewRow)lbldata.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, "Carrier");
            int i = container.RowIndex;

            lbldata.ID = "lbl4" + _caption + "K" + i.ToString();

            lbldata.Visible = false;
            if (dataValue != DBNull.Value)
            {
                lbldata.Text = dataValue.ToString();
                if (lbldata.Text == "ServiceID")
                {
                    container.Visible = false;
                }
            }
        }
        void lbl5_DataBinding(object sender, EventArgs e)
        {
            Label lbldata = (Label)sender;
            GridViewRow container = (GridViewRow)lbldata.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, "FieldName");
            int i = container.RowIndex;
            lbldata.Visible = false;

            lbldata.ID = "lbl5" + _caption + "K" + i.ToString();


            if (dataValue != DBNull.Value)
            {
                lbldata.Text = dataValue.ToString();
                if (lbldata.Text == "ServiceID")
                {
                    container.Visible = false;
                }
            }
        }

        void chkDis_DataBinding(object sender, EventArgs e)
        {
            CheckBox chkData = (CheckBox)sender;
            GridViewRow container = (GridViewRow)chkData.NamingContainer;
            GridView MainContainer = (GridView)container.NamingContainer;

            object dataValue = DataBinder.Eval(container.DataItem, _columnName);
            int i = container.RowIndex;
            chkData.ID = "chkDis" + _caption + "K" + i.ToString();
            if (dataValue != DBNull.Value)
            {

                //chkData.Text = dataValue.ToString();
                if (dataValue.ToString().Equals("Yes"))
                {

                    chkData.Checked = true;


                    for (int j = container.RowIndex - 1; j >= container.RowIndex - 7; j--)
                    {

                        GridViewRow container1 = MainContainer.Rows[j];
                        TextBox txtBox = (TextBox)container1.FindControl("tb1" + _caption + "K" + j.ToString());
                        txtBox.Enabled = false;
                    }

                    LinkButton lkbutton = (LinkButton)MainContainer.Rows[container.RowIndex - 4].FindControl("lnk1" + _caption + "K" + (container.RowIndex - 4).ToString());
                    lkbutton.Enabled = false;
                    Button btbutton = (Button)MainContainer.Rows[container.RowIndex - 4].FindControl("bt1" + _caption + "K" + (container.RowIndex - 4).ToString());
                    btbutton.Enabled = false;


                    //if (chkData.Checked)
                    //{
                    //    TextBox txtBox = (TextBox)MainContainer.Rows[container.RowIndex - 1].FindControl("tb1" + _caption);
                    //    txtBox.Enabled = false;

                    //}
                    container.BorderStyle = BorderStyle.Double;
                    container.BorderColor = System.Drawing.Color.Blue;
                }
                else
                {
                    if (dataValue.ToString().Equals("No"))
                    {

                        chkData.Checked = false;
                        //chkDis.AutoPostBack = true;


                        container.BorderStyle = BorderStyle.Double;
                        container.BorderColor = System.Drawing.Color.Blue;
                    }
                    else
                    {

                        chkData.Checked = false;

                        chkData.Visible = false;

                    }

                }

            }
            else
            {
                chkData.Visible = false;
                chkData.Checked = true;


            }
            if (chkData.Visible)
            {
                chkData.AutoPostBack = true;
                chkData.CheckedChanged += new EventHandler(chkDis_Change);
            }
        }

        void lnk_DataBinding(object sender, EventArgs e)
        {
            LinkButton lnkdata = (LinkButton)sender;
            GridViewRow container = (GridViewRow)lnkdata.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, _columnName);
            int i = container.RowIndex;
            lnkdata.ID = "lnk1" + _caption + "K" + i.ToString();
            if (dataValue != DBNull.Value)
            {

                if (dataValue.ToString().Contains("DELIVERY_DELAY_"))
                {
                    lnkdata.Text = dataValue.ToString();
                    lnkdata.Visible = true;
                }
                else
                {
                    lnkdata.Visible = false;
                }

            }
            else
            {
                lnkdata.Visible = false;
            }
        }

        void bt_DataBinding(object sender, EventArgs e)
        {
            Button btdata = (Button)sender;
            GridViewRow container = (GridViewRow)btdata.NamingContainer;
            object dataValue = DataBinder.Eval(container.DataItem, _columnName);
            //btdata.Click += new EventHandler(bt_Click);
            int i = container.RowIndex;
            btdata.ID = "bt1" + _caption + "K" + i.ToString();
            if (dataValue != DBNull.Value)
            {

                if (dataValue.ToString().Equals("NoTable") || dataValue.ToString().Contains("DELIVERY_DELAY_"))
                {
                    btdata.Visible = true;
                }
                else
                {
                    btdata.Visible = false;
                }


            }
            else
            {
                btdata.Visible = false;

            }
        }

        void bt_Click(object sender, EventArgs e)
        {
            this.CommandName = "Import";
            //Create command event arguments
            CommandEventArgs commandEventArgs = new CommandEventArgs(this.CommandName, bt);
            GridViewCommandEventArgs cmd = new GridViewCommandEventArgs(bt, commandEventArgs);
            //Bubble the command event to the container of MyTemplateField i.e. GridView
            RaiseBubbleEvent(bt, cmd);
        }

        void lnk_Click(object sender, EventArgs e)
        {
            this.CommandName = "Update Import";
            //Create command event arguments
            CommandEventArgs commandEventArgs = new CommandEventArgs(this.CommandName, lnk);
            //Bubble the command event to the container of MyTemplateField i.e. GridView
            RaiseBubbleEvent(lnk, commandEventArgs);
        }

        void chkDis_Change(object sender, EventArgs e)
        {
            CheckBox chkData = (CheckBox)sender;
            GridViewRow container = (GridViewRow)chkData.NamingContainer;
            GridView MainContainer = (GridView)container.NamingContainer;
            if (chkData.Checked)
            {
                for (int i = container.RowIndex - 1; i >= container.RowIndex - 7; i--)
                {
                    TextBox txtBox = (TextBox)MainContainer.Rows[i].FindControl("tb1" + _caption + "K" + i.ToString());
                    txtBox.Enabled = false;
                }

                LinkButton lkbutton = (LinkButton)MainContainer.Rows[container.RowIndex - 4].FindControl("lnk1" + _caption + "K" + (container.RowIndex - 4).ToString());
                lkbutton.Enabled = false;
                Button btbutton = (Button)MainContainer.Rows[container.RowIndex - 4].FindControl("bt1" + _caption + "K" + (container.RowIndex - 4).ToString());
                btbutton.Enabled = false;
            }
            else
            {
                for (int i = container.RowIndex - 1; i >= container.RowIndex - 7; i--)
                {
                    TextBox txtBox = (TextBox)MainContainer.Rows[i].FindControl("tb1" + _caption + "K" + i.ToString());
                    txtBox.Enabled = true;
                    HiddenField hdntemp = (HiddenField)MainContainer.Rows[i].FindControl("hdn" + _caption + "K" + i.ToString());
                    txtBox.Text = hdntemp.Value;
                }
                TextBox txtBoxserviceId = (TextBox)MainContainer.Rows[container.RowIndex - 8].FindControl("tb1" + _caption + "K" + (container.RowIndex - 8).ToString());
                if (Convert.ToInt32(txtBoxserviceId.Text) > 0)
                {
                    LinkButton lkbutton = (LinkButton)MainContainer.Rows[container.RowIndex - 4].FindControl("lnk1" + _caption + (container.RowIndex - 4).ToString());
                    lkbutton.Enabled = true;
                    Button btbutton = (Button)MainContainer.Rows[container.RowIndex - 4].FindControl("bt1" + _caption + (container.RowIndex - 4).ToString());
                    btbutton.Enabled = true;
                }
            }
            ////Create command event arguments
            //CommandEventArgs commandEventArgs = new CommandEventArgs(this.CommandName, chkDis);
            ////Bubble the command event to the container of MyTemplateField i.e. GridView
            //RaiseBubbleEvent(chkDis, commandEventArgs);

        }


    }



}

