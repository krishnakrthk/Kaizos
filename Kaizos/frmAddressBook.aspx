<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAddressBook.aspx.cs" Inherits="Kaizos.frmAddressBook"MasterPageFile="~/NewSite.master"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<asp:ScriptManager ID="ScriptManager1" runat="server" />

<div class="errorMsg" runat="server" id="errMsg">
	<asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>
	<asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
	<asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
	<asp:Label ID="valAccept" Text="<%$ Resources:LocalString, ValidationAccept %>" runat ="server" Visible= "false"></asp:Label>
	<asp:Label ID="valAlready" Text="<%$ Resources:LocalString, ValidationAlready %>" runat ="server" Visible= "false"></asp:Label>
	<asp:Label ID="valInvalidTimeFormat" Text="<%$ Resources:LocalString, ValidationInvalidTimeFormat %>" runat ="server" Visible= "false"></asp:Label>
	<asp:Label ID="ValLessThan" Text="<%$ Resources:LocalString, ValLessThan %>" runat ="server" Visible= "false"></asp:Label>
	<asp:CustomValidator ID="val_AddressBook" runat="server" 
					ControlToValidate="txtName" 
					EnableClientScript="False" 
					ValidateEmptyText="True"
					ValidationGroup="grpAddressBook" onservervalidate="val_AddressBook_ServerValidate" 
				 ></asp:CustomValidator>
</div>


    <fieldset id="Fieldset1" runat="server" class ="third">
        <legend>
            <asp:Label ID="Label3" runat="server" Text="Label">Address Book</asp:Label>
        </legend>
        <div class="fieldSection">

            <div class="fieldBloc">
                <label for="txtCompanyName"><asp:Label ID="lblCompanyName" runat="server" Text="<%$ Resources:LocalString, AddressBookCompanyName %>"></asp:Label></label>
                <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="60"></asp:TextBox>
            </div>

            <div class="fieldBloc">
                <label for="txtEmail"><asp:Label ID="lblEmail" runat="server" Text="<%$ Resources:LocalString, AddressBookEmail%>"></asp:Label></label>
                <asp:TextBox ID="txtEmail" runat="server" MaxLength="60"></asp:TextBox>
            </div>

            <div class="fieldBloc">
                <label for="txtName">
                    <asp:Label ID="lblName" runat="server" Text="<%$ Resources:LocalString, AddressBookName%>"></asp:Label> 
                </label>
                <asp:TextBox ID="txtName" runat="server" MaxLength="100"></asp:TextBox>
            </div>

            <div class="fieldBloc">
                <label for="txtPhoneNumber">
                    <asp:Label ID="lblPhoneNumber" runat="server" Text="<%$ Resources:LocalString, AddressBookPhoneNumber%>"></asp:Label> 
                </label>
                <asp:TextBox ID="txtPhoneNumber" runat="server" MaxLength="20"></asp:TextBox>
            </div>

            <div class="fieldBloc">
                <label for="txtFaxNumber">
                    <asp:Label ID="lblFaxNumber" runat="server" Text="<%$ Resources:LocalString, AddressBookFaxNumber%>"></asp:Label>
                </label>
                <asp:TextBox ID="txtFaxNumber" runat="server" MaxLength="20"></asp:TextBox>
            </div>

		    <div class="fieldBloc">
                <label for="optCommercial"><asp:Label ID="lblAddressType" runat="server" Text="<%$ Resources:LocalString, AddressBookAddressType%>"></asp:Label></label>
                <asp:RadioButton ID="optCommercial" runat="server" Text="<%$ Resources:LocalString, AddressBookCommercial%>" GroupName="grpAddressType" AutoPostBack="True" oncheckedchanged="optCommercial_CheckedChanged"></asp:RadioButton>
                <asp:RadioButton ID="optResidential" runat="server" Text="<%$ Resources:LocalString, AddressBookResidential%>" GroupName="grpAddressType" AutoPostBack="True" oncheckedchanged="optResidential_CheckedChanged"></asp:RadioButton>
            </div>

			<hr />
            <div class="fieldBloc">
                <label for="txtAddress1">
                    <asp:Label ID="lblAddress1" runat="server" Text="<%$ Resources:LocalString, AddressBookAddress1%>"></asp:Label> 
                </label>
                <asp:TextBox ID="txtAddress1" runat="server" MaxLength="50"></asp:TextBox>
            </div>

            <div class="fieldBloc">
                <label for="txtAddress2">
                    <asp:Label ID="lblAddress2" runat="server" Text="<%$ Resources:LocalString, AddressBookAddress2%>"></asp:Label> 
                </label>
                <asp:TextBox ID="txtAddress2" runat="server" MaxLength="50"></asp:TextBox>
            </div>
            <div class="fieldBloc">
                <label for="txtAddress3">
                    <asp:Label ID="lblAddress3" runat="server" Text="<%$ Resources:LocalString, AddressBookAddress3%>"></asp:Label> 
                </label>
                <asp:TextBox ID="txtAddress3" runat="server" MaxLength="50"></asp:TextBox>
            </div>     
			<div class="fieldBloc">
                <label for="txtCity">
                    <asp:Label ID="lblCity" runat="server" Text="<%$ Resources:LocalString, AddressBookCity%>"></asp:Label> 
                </label>
                <asp:TextBox ID="txtCity" runat="server" MaxLength="50"></asp:TextBox>
            </div>
            <div class="fieldBloc">
                <label for="txtZipcode">
                    <asp:Label ID="lblZipcode" runat="server" Text="<%$ Resources:LocalString, AddressBookZipcode%>"></asp:Label> 
                </label>
                <asp:TextBox ID="txtZipcode" runat="server" MaxLength="12"></asp:TextBox>
            </div>
           <div class="fieldBloc">
                <label for="txtZipcode">
                    <asp:Label ID="lblState" runat="server" Text="<%$ Resources:LocalString, AddressBookState%>"></asp:Label> 
                </label>
                <asp:TextBox ID="txtState" runat="server" MaxLength="50"></asp:TextBox>
            </div>

            <div class="fieldBloc">
                <label for="ddlCountry">
                    <asp:Label ID="lblCountry" runat="server" Text="<%$ Resources:LocalString, AddressBookCountry%>"></asp:Label> 
                </label>
                <asp:DropDownList ID="ddlCountry" runat="server"></asp:DropDownList>
            </div>


			<hr />
			<div class="fieldBloc">
                <label for="txtLastPickupMT">
                    <asp:Label ID="lblLastPickupMT" runat="server" Text="<%$ Resources:LocalString, AddressBookLastPickupMtoF%>"></asp:Label>
               </label>
               <asp:TextBox ID="txtLastPickupMT" runat="server" MaxLength="5"></asp:TextBox>
            </div>
 
            <div class="fieldBloc">
                <label for="txtLastPickupF">
                    <asp:Label ID="lblLastPickupF" runat="server" Text="<%$ Resources:LocalString, AddressBookLastPickupF%>"></asp:Label>
                </label>
                <asp:TextBox ID="txtLastPickupF" runat="server" MaxLength="5"></asp:TextBox>
            </div>
        </div>
        <div class="fieldSection">

            <div class="fieldBloc">
                <label for="mtxtComments">
                    <asp:Label ID="lblCompany" runat="server" Text="<%$ Resources:LocalString, AddressBookComments%>">"></asp:Label> 
                 </label>
                 <asp:TextBox ID="mtxtComments" runat="server" TextMode="MultiLine" 
                                MaxLength="300"></asp:TextBox>
            </div>

            <div class="fieldBloc">
                <label for="optShippingAddress"><asp:Label ID="lblAddressUserFor" runat="server" Text="<%$ Resources:LocalString, AddressBookAddressUsedFor%>"></asp:Label></label>
                <asp:RadioButton ID="optShippingAddress" runat="server" Text="<%$ Resources:LocalString, AddressBookShippingAddress%>" GroupName="grpAddressUsedFor" ></asp:RadioButton>
                <asp:RadioButton ID="optDeliveryAddress" runat="server" Text="<%$ Resources:LocalString, AddressBookDeliveryAddress%>" GroupName="grpAddressUsedFor" ></asp:RadioButton>
				<asp:RadioButton ID="optBoth" runat="server" Text="<%$ Resources:LocalString, AddressBookBoth%>" GroupName="grpAddressUsedFor" ></asp:RadioButton>
             </div>

			<hr />
			<div class="fieldBloc">
                <label for="chkEnableShippingPreference">
                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalString, AddressBookEnableShippingPreference%>"></asp:Label> 
                </label>
                <asp:CheckBox ID="chkEnableShippingPreference" runat="server" 
                    oncheckedchanged="chkEnableShippingPreference_CheckedChanged" 
                    AutoPostBack="True"/>
            </div>
             <div class="fieldBloc">
                <label for="optShipPreference1"><asp:Label ID="lblShipPreference" runat="server" Text="<%$ Resources:LocalString, AddressBookShippingPreference%>"></asp:Label></label>
                <asp:RadioButton ID="optShipPreference1" runat="server" Text="<%$ Resources:LocalString, AddressBookMostCompetitive%>" GroupName="grpShipPreference" oncheckedchanged="optShipPreference1_CheckedChanged" AutoPostBack="True" ></asp:RadioButton>
				<asp:RadioButton ID="optShipPreference2" runat="server" Text="<%$ Resources:LocalString, AddressBookFastest%>" GroupName="grpShipPreference" oncheckedchanged="optShipPreference2_CheckedChanged" AutoPostBack="True" ></asp:RadioButton>
				<asp:RadioButton ID="optShipPreference3" runat="server" Text="<%$ Resources:LocalString, AddressBookNamedCarrier%>" GroupName="grpShipPreference" oncheckedchanged="optShipPreference3_CheckedChanged" AutoPostBack="True" ></asp:RadioButton>
            </div>

            <div class="fieldBloc">
                <label for="ddlNamedCarrier">
                    <asp:Label ID="lblNamedCarrier" runat="server" Text="<%$ Resources:LocalString, AddressBooklblNamedCarrier%>" Visible = "false"></asp:Label> 
                </label>
                <asp:DropDownList ID="ddlNamedCarrier" runat="server" Visible = "false"></asp:DropDownList>
            </div>

        </div>    
    </fieldset>

    <div id ="buttons">
        <asp:Label ID="Label16" runat="server" Font-Size="Smaller" Text="*Mandatory fields"></asp:Label>
        <asp:Button ID="Button1" runat="server" Text="<%$ Resources:LocalString, AddAddress%>" onclick="Button1_Click" ValidationGroup="grpAddressBook" />
        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalString, AllCancel%>" onclick="btnCancel_Click"/>
    </div>
    


<asp:Button ID="btnMessage" Visible="false" runat="server" />
     <asp:modalpopupextender 
                                   id="MdlAddressExist" runat="server" 
                                   okcontrolid="ButtonDeleleOkay" 
                                   targetcontrolid="btnMessage" popupcontrolid="DivDeleteConfirmation" 
                                   backgroundcssclass="modalBackground">
                       </asp:modalpopupextender>
                        <asp:confirmbuttonextender id="lnkDelete_ConfirmButtonExtender" 
                                       runat="server" targetcontrolid="btnMessage" enabled="True" 
                                       displaymodalpopupid="MdlAddressExist">
                       </asp:confirmbuttonextender>

<asp:panel class="customerModelWindow" id="DivDeleteConfirmation" style="display: none" runat="server">
    <div class="TitlebarLeft">
        <asp:Label ID="lblInformation" Text="Information" runat="server"  CssClass="clsLabelHeader"/>
     </div>
        <center>
        <div class="customerDivModalWindow">
        <table >
            <tr>
                <td>
                      <asp:Label ID="lblMessage" runat="server" Text="test" />
                </td>
            </tr>
        </table>
        </div>
        <table>
             <tr>
                <td align="right">
                    <input id="ButtonDeleleOkay" type="button" value="ok" class="clsMessgeButton"/> 
                </td>
            </tr>
        </table>
        </center>
</asp:panel>

</asp:Content>