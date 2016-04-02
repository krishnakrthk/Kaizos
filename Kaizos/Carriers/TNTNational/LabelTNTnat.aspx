<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LabelTNTnat.aspx.cs" Inherits="TNTNATIONAL.carrier.LabelTNTnat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/CSS/Kaizos.css" rel="stylesheet" type="text/css" />
      <style type="text/css">
        #frm
        {
            width: 254px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

   <div class="clsCarrierMain">
        
        <table>
    <tr class="clsLabelLeft">
    <td>
       <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Button" />
    </td>
    </tr>
    <tr>
    <td class="clsLabelLeft">
    <asp:LinkButton ID="lbReturnUrl" runat="server" Text="Return" 
            PostBackUrl="~/frmcarrierLabelDisplay.aspx" ></asp:LinkButton>
    </td>
    </tr>
</table>
    </div>
    </form>
</body>
</html>
