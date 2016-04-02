<%@ Page Title="" Language="C#" MasterPageFile="~/NewSite.master" AutoEventWireup="true" CodeBehind="frmMasterServiceTypeUpdate.aspx.cs" Inherits="Kaizos.frmMasterServiceTypeUpdate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
<script type="text/javascript" language="javascript">
    function test() {
        alert('test');
    }

    function set_readonly_state(obj, state) {
        if (state) {
            obj.setAttribute('readonly', state);
        } else {
            obj.removeAttribute('readonly');
        }
    } 
function Enable( i, j ,id,sid) {
   
    var Parent = document.getElementById(id);
//    alert('row');
//    alert(i);
//    alert('col');
//    alert(j);
//    alert('serid');
//    alert(sid);
//    alert('row.length');
//    alert(Parent.rows.length);
    var items = Parent.getElementsByTagName('input');
    var c = 2;
    var oldi = i;
    var oldj = j;
    var k;
    for ( k = 10; ; k = k + 10)
    {
    
        if (i <= k) {
            i = i - c;
            break;
        }
        else{
    
          c= c+2;
        }


  }

  if (oldi == i) {
      i = i - c;

  }

//  alert('ik');
//  alert(i);
//  if (k > Parent.rows.length && i > Parent.rows.length) {
//     c=c+2
//     i = i - c;
////     alert('ik');
////     alert(i);
//  }
  
    j = j - 1;
    try
    {
        
        var chk = Parent.rows[i].cells[j].getElementsByTagName('input');
//        alert(chk[0].type);
//        alert(chk[0].id);
       
        if (sid>0)
        {
            if (chk[0].checked) {
                txt = Parent.rows[i - 4].cells[j].getElementsByTagName('a');
                txt[0].style.visibility = "hidden";
                txt = Parent.rows[i - 4].cells[j].getElementsByTagName('input');
                txt[0].style.visibility = "hidden"
                // Parent.rows[i - 4].cells[j].style.visibility = "hidden"
            }
            else {
                txt = Parent.rows[i - 4].cells[j].getElementsByTagName('a');
                txt[0].style.visibility = "visible"
                txt = Parent.rows[i - 4].cells[j].getElementsByTagName('input');
                txt[0].style.visibility = "visible"
                //  Parent.rows[i - 4].cells[j].style.visibility = "visible"
            }
        
        } 
        var txt = Parent.rows[i - 6].cells[j].getElementsByTagName('input');
         txt[0].disabled = chk[0].checked;                  
         txt = Parent.rows[i - 5].cells[j].getElementsByTagName('input');
         txt[0].disabled = chk[0].checked;

        
         txt = Parent.rows[i - 3].cells[j].getElementsByTagName('input');
         txt[0].disabled = chk[0].checked;
         txt = Parent.rows[i - 2].cells[j].getElementsByTagName('textarea');
      
         txt[0].disabled = chk[0].checked;
//         alert(txt[0].type);  
         txt = Parent.rows[i - 1].cells[j].getElementsByTagName('input');
         txt[0].disabled = chk[0].checked;     
}
     catch (e) {
       
       
     }

 
// Parent = document.getElementById('MainContent_tbCarrierService');
}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <asp:GridView ID="gvCarrierService1" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="gridHeader" RowStyle-CssClass="gridRow" AlternatingRowStyle-CssClass="gridAlternate" 
              ShowHeader="false"  GridLines="None" onrowcommand="gvCarrierService1_RowCommand" Visible="false">
              <Columns>
              </Columns>
    
</asp:GridView>
 <asp:PlaceHolder ID="plnCarrierService" runat="server">
 
     <asp:Table ID="tbCarrierService" runat="server">
     </asp:Table>
  
 </asp:PlaceHolder>
   <asp:Button ID="btnSubmit" runat="server" Text="Submit" onclick="btnSubmit_Click"  ValidationGroup="grpMasterType" />
    <table class="clsTable" style="display: none">
        <tr class="gridHeader">
            <th>
            </th>
            <th>
            </th>
            <th>
                <asp:Label ID="lblRoadHeader" runat="server"></asp:Label>
            </th>
            <th>
                <asp:Label ID="lblAirB410Header" runat="server"></asp:Label>
            </th>
            <th>
                <asp:Label ID="lblAirB412Header" runat="server"></asp:Label>
            </th>
            <th>
                <asp:Label ID="lblAirB418Header" runat="server"></asp:Label>
            </th>
        </tr>
    </table>
   
<table style="display:none" >
        
    <tr>
    <td>


    <asp:GridView ID="gvCarrierService" runat="server" AutoGenerateColumns="false" Visible="false"
        HeaderStyle-CssClass="gridHeader" RowStyle-CssClass="gridRow" 
        AlternatingRowStyle-CssClass="gridAlternate" 
        ShowHeader="false" 
        GridLines="None"  >
    <Columns>

    <asp:TemplateField>
     <ItemTemplate>

         <table>
             <tr class="rowGroup">
                 <td rowspan="6">
                     <asp:Label ID="lblCarrierName" runat="server" Text='<%#  Eval("CarrierName").ToString() %>'> </asp:Label>
                 </td>
                 <td>
                     <asp:Label ID="lblServiceNameCaption" Text="<%$ Resources:LocalString, ServiceNameCaption %>"
                         runat="server"></asp:Label>
                 </td>
                 <td>
                     <asp:TextBox ID="txtRoadServiceName" runat="server" Text='<%#  Eval("RoadServiceName").ToString() %>'
                         class="clsSummaryLabelLeft" MaxLength="60"> </asp:TextBox>
                 </td>
                 <td>
                     <asp:TextBox ID="txtAirB410ServiceName" runat="server" Text='<%#  Eval("AirB410ServiceName").ToString() %>'
                         class="clsSummaryLabelLeft" MaxLength="60"> </asp:TextBox>
                 </td>
                 <td>
                     <asp:TextBox ID="txtAirB412ServiceName" runat="server" Text='<%#  Eval("AirB412ServiceName").ToString() %>'
                         class="clsSummaryLabelLeft" MaxLength="60"> </asp:TextBox>
                 </td>
                 <td>
                     <asp:TextBox ID="txtAirB418ServiceName" runat="server" Text='<%#  Eval("AirB418ServiceName").ToString() %>'
                         class="clsSummaryLabelLeft" MaxLength="60"> </asp:TextBox>
                 </td>
             </tr>
             <tr>
                 <td>
                     <asp:Label ID="lblServiceCodeCaption" Text="<%$ Resources:LocalString, ServiceCodeCaption %>"
                         runat="server"></asp:Label>
                 </td>
                 <td>
                     <asp:TextBox ID="txtRoadServiceCode" runat="server" Text='<%#  Eval("RoadServiceCode").ToString() %>'
                         class="clsSummaryLabelLeft" MaxLength="30"> </asp:TextBox>
                     <asp:Label ID="lblRoadServiceCode" Visible="false" runat="server" Text='<%#  Eval("RoadServiceCode").ToString() %>'
                         class="clsSummaryLabelLeft"> </asp:Label>
                 </td>
                 <td>
                     <asp:TextBox ID="txtAirB410ServiceCode" runat="server" Text='<%#  Eval("AirB410ServiceCode").ToString() %>'
                         class="clsSummaryLabelLeft" MaxLength="30"> </asp:TextBox>
                     <asp:Label ID="lblAirB410ServiceCode" Visible="false" runat="server" Text='<%#  Eval("AirB410ServiceCode").ToString() %>'
                         class="clsSummaryLabelLeft"> </asp:Label>
                 </td>
                 <td>
                     <asp:TextBox ID="txtAirB412ServiceCode" runat="server" Text='<%#  Eval("AirB412ServiceCode").ToString() %>'
                         class="clsSummaryLabelLeft" MaxLength="30"> </asp:TextBox>
                     <asp:Label ID="lblAirB412ServiceCode" Visible="false" runat="server" Text='<%#  Eval("AirB412ServiceCode").ToString() %>'
                         class="clsSummaryLabelLeft"> </asp:Label>
                 </td>
                 <td>
                     <asp:TextBox ID="txtAirB418ServiceCode" runat="server" Text='<%#  Eval("AirB418ServiceCode").ToString() %>'
                         class="clsSummaryLabelLeft" MaxLength="30"> </asp:TextBox>
                     <asp:Label ID="lblAirB418ServiceCode" Visible="false" runat="server" Text='<%#  Eval("AirB418ServiceCode").ToString() %>'
                         class="clsSummaryLabelLeft"> </asp:Label>
                 </td>
             </tr>
             <tr>
                 <td>
                     <asp:Label ID="lblImportCaption" Text="<%$ Resources:LocalString, DeliveryDelayCaption %>"
                         runat="server"></asp:Label>
                 </td>
                 <td>
                     <asp:LinkButton ID="lblRoadDeliveryDelay" runat="server" CommandName="RoadDelay"
                         Text="Delivery Delay" ToolTip='<%# Eval("RoadDeliveryDelay") %>'></asp:LinkButton>
                     <asp:Button ID="btnRoadImport" runat="server" Text="Import" CommandName="RoadImport" />
                     <asp:Label ID="lblRoadServiceID" runat="server" Text='<%# Eval("RoadServiceID") %>'
                         Visible="false"></asp:Label>
                 </td>
                 <td>
                     <asp:LinkButton ID="lblAirB410DeliveryDelay" runat="server" CommandName="AirB410Delay"
                         Text="Delivery Delay" ToolTip='<%# Eval("AirB410DeliveryDelay") %>'></asp:LinkButton>
                     <asp:Button ID="btnAirB410Import" runat="server" Text="Import" CommandName="AirB410Import" />
                     <asp:Label ID="lblAirB410ServiceID" runat="server" Text='<%# Eval("AirB410ServiceID") %>'
                         Visible="false"></asp:Label>
                 </td>
                 <td>
                     <asp:LinkButton ID="lblAirB412DeliveryDelay" runat="server" CommandName="AirB412Delay"
                         Text="Delivery Delay" ToolTip='<%# Eval("AirB412DeliveryDelay") %>'></asp:LinkButton>
                     <asp:Button ID="btnAirB412Import" runat="server" Text="Import" CommandName="AirB412Import"
                         ToolTip="" />
                     <asp:Label ID="lblAirB412ServiceID" runat="server" Text='<%# Eval("AirB412ServiceID") %>'
                         Visible="false"></asp:Label>
                 </td>
                 <td>
                     <asp:LinkButton ID="lblAirB418DeliveryDelay" runat="server" CommandName="AirB418Delay"
                         Text="Delivery Delay" ToolTip='<%# Eval("AirB418DeliveryDelay") %>'></asp:LinkButton>
                     <asp:Button ID="btnAirB418Import" runat="server" Text="Import" CommandName="AirB418Import" />
                     <asp:Label ID="lblAirB418ServiceID" runat="server" Text='<%# Eval("AirB418ServiceID") %>'
                         Visible="false"></asp:Label>
                 </td>
             </tr>
             <tr>
                 <td>
                     <asp:Label ID="lblDeadLineCaption" Text="<%$ Resources:LocalString, DeliveryDeadLineCaption %>"
                         runat="server"></asp:Label>
                 </td>
                 <td>
                     <asp:TextBox ID="txtRoadDeliveryDeadLine" runat="server" Text='<%# Eval("RoadDeliveryDeadLine") %>'></asp:TextBox>
                 </td>
                 <td>
                     <asp:TextBox ID="txtAirB410DeliveryDeadLine" runat="server" Text='<%# Eval("AirB410DeliveryDeadLine") %>'></asp:TextBox>
                 </td>
                 <td>
                     <asp:TextBox ID="txtAirB412DeliveryDeadLine" runat="server" Text='<%# Eval("AirB412DeliveryDeadLine") %>'></asp:TextBox>
                 </td>
                 <td>
                     <asp:TextBox ID="txtAirB418DeliveryDeadLine" runat="server" Text='<%# Eval("AirB418DeliveryDeadLine") %>'></asp:TextBox>
                 </td>
             </tr>
             <tr>
                 <td>
                     <asp:Label ID="lblActive" Text="<%$ Resources:LocalString, DisabledCaption %>" runat="server"></asp:Label>
                 </td>
                 <td>
                     <asp:CheckBox ID="chkRoadActive" runat="server" Checked=' <%# Eval("RoadActive") %>' />
                 </td>
                 <td>
                     <asp:CheckBox ID="chkAirB410Active" runat="server" Checked=' <%# Eval("AirB410Active") %>' />
                 </td>
                 <td>
                     <asp:CheckBox ID="chkAirB412Active" runat="server" Checked=' <%# Eval("AirB412Active") %>' />
                 </td>
                 <td>
                     <asp:CheckBox ID="chkAirB418Active" runat="server" Checked=' <%# Eval("AirB418Active") %>' />
                 </td>
             </tr>
         </table>
     </ItemTemplate>
    </asp:TemplateField>
    
    </Columns>
    </asp:GridView>
    </td> </tr> </table>
</asp:Content>



