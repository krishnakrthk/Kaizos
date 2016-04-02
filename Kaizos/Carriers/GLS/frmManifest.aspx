<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmManifest.aspx.cs" Inherits="GLSFinal.Carrier.frmManifest" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="~/CSS/Kaizos.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
     <div class="clsCarrierMain">
      <table>
    <tr class="clsLabelLeft">
    <td>
    <rsweb:ReportViewer ID="ReportViewer2" runat="server" Font-Names="Verdana" 
            Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
            style="z-index: 1; left: 27px; top: 53px; position: absolute; height: 400px; width: 354px" 
            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
            <LocalReport ReportPath="Carriers\GLS\ManifestReport.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
         </td>
    </tr>
    <tr>
    <td class="clsLabelLeft">
    <asp:LinkButton ID="lbReturnUrl" runat="server" Text="Return" 
            PostBackUrl="~/frmcarrierLabelDisplay.aspx" ></asp:LinkButton>
    </td>
    </tr>
</table>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            SelectMethod="getLabel" TypeName="GLSFinal.Carrier.GLSCarrier">
        </asp:ObjectDataSource>
    
    </div>
    </form>
</body>
</html>
