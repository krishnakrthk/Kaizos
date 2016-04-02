<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAddressBookUpdate.aspx.cs" Inherits="Kaizos.frmAddressBookUpdate" MasterPageFile="~/NewSite.master"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<div id="Div1" runat="server" class="search">
    <div class="fieldBloc">
        <asp:Label ID="lblSearchName" runat="server" Text="<%$ Resources:LocalString, AddressBookSearchName %>"></asp:Label> 
        <asp:TextBox ID="txtSearchName" runat="server" MaxLength="100"></asp:TextBox>
        
        <asp:Button ID="btnSearch" runat="server" 
            Text="<%$ Resources:LocalString, Search%>" onclick="btnSearch_Click" ValidationGroup="grpAddressBook"/>
    </div>
     </div>    


    <asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmpty %>" runat ="server" Visible= "false"></asp:Label>

    <asp:CustomValidator ID="val_AddressBook" runat="server" 
        ControlToValidate="txtSearchName"  EnableClientScript="False" 
        ValidateEmptyText="True" ValidationGroup="grpAddressBook" 
        CssClass="clsErrorMessage" onservervalidate="val_AddressBook_ServerValidate" > </asp:CustomValidator>
            
    <asp:GridView ID="gvAddressBookList" runat="server" AutoGenerateColumns="false"
                HeaderStyle-CssClass="gridHeader"
                AllowSorting="True" CaptionAlign="Left" 
                onrowediting="gvAddressBookList_RowEditing" 
                onrowupdating="gvAddressBookList_RowUpdating" 
                onrowcancelingedit="gvAddressBookList_RowCancelingEdit" 
                onrowdatabound="gvAddressBookList_RowDataBound" 
                onrowdeleted="gvAddressBookList_RowDeleted" 
                onrowdeleting="gvAddressBookList_RowDeleting">
               <Columns>
                   <asp:TemplateField HeaderText="<%$ Resources:LocalString, AddressBookListCompanyName %>" >
                        <ItemTemplate>
                            <asp:Label ID="lblCompanyName" runat="server" Text='<%#Eval("CompanyName").ToString() %>'></asp:Label>
                            <asp:Label ID="lblAddressID" runat="server" Text='<%#Eval("AddressID").ToString() %>' Visible = "false"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtCompanyName" runat="server" Text='<%#Eval("CompanyName").ToString() %>' MaxLength = "60"></asp:TextBox>                
                            <asp:TextBox ID="txtAddressID" runat="server" Text='<%#Eval("AddressID").ToString() %>' Visible = "false"></asp:TextBox>                
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, AddressBookListContactName %>" >
                        <ItemTemplate>
                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name").ToString() %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtName" runat="server" Text='<%#Eval("Name").ToString() %>' MaxLength = "100"></asp:TextBox>                
                        </EditItemTemplate>
                    </asp:TemplateField>

              
                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, AddressBookListAddressType %>" >
                        <ItemTemplate>
                            <asp:Label ID="lblAddressType" runat="server" Text='<%#Eval("AddressType").ToString() %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblAddressType1" runat="server" Text='<%#Eval("AddressType").ToString() %>' Visible="false"></asp:Label>
                            <asp:DropDownList ID="ddlAddressType" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, AddressBookListTelNo %>" >
                        <ItemTemplate>
                            <asp:Label ID="lblTelephoneNo" runat="server" Text='<%#Eval("TelephoneNo").ToString() %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtTelephoneNo" runat="server" Text='<%#Eval("TelephoneNo").ToString() %>' MaxLength = "20"></asp:TextBox>                
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, AddressBookListFaxNo %>" >
                        <ItemTemplate>
                            <asp:Label ID="lblFaxNo" runat="server" Text='<%#Eval("FaxNo").ToString() %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtFaxNo" runat="server" Text='<%#Eval("FaxNo").ToString() %>' MaxLength = "20"></asp:TextBox>                
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, AddressBookListAddress1 %>" >
                        <ItemTemplate>
                            <asp:Label ID="lblAddress1" runat="server" Text='<%#Eval("Address1").ToString() %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtAddress1" runat="server" Text='<%#Eval("Address1").ToString() %>' MaxLength = "50"></asp:TextBox>                
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, AddressBookListAddress2 %>" >
                        <ItemTemplate>
                            <asp:Label ID="lblAddress2" runat="server" Text='<%#Eval("Address2").ToString() %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtAddress2" runat="server" Text='<%#Eval("Address2").ToString() %>' MaxLength = "50"></asp:TextBox>                
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, AddressBookListAddress3%>" >
                        <ItemTemplate>
                            <asp:Label ID="lblAddress3" runat="server" Text='<%#Eval("Address3").ToString() %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtAddress3" runat="server" Text='<%#Eval("Address3").ToString() %>' MaxLength = "50"></asp:TextBox>                
                        </EditItemTemplate>
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, AddressBookListZipcode%>" >
                        <ItemTemplate>
                            <asp:Label ID="lblZipcode" runat="server" Text='<%#Eval("Zipcode").ToString() %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtZipcode" runat="server" Text='<%#Eval("Zipcode").ToString() %>' MaxLength = "12"></asp:TextBox>                
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, AddressBookListCity%>" >
                        <ItemTemplate>
                            <asp:Label ID="lblCity" runat="server" Text='<%#Eval("City").ToString() %>' ></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtCity" runat="server" Text='<%#Eval("City").ToString() %>' MaxLength = "50"></asp:TextBox>                
                        </EditItemTemplate>
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, AddressBookListState%>" >
                        <ItemTemplate>
                            <asp:Label ID="lblState" runat="server" Text='<%#Eval("State").ToString() %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtState" runat="server" Text='<%#Eval("State").ToString() %>' MaxLength = "50"></asp:TextBox>                
                        </EditItemTemplate>
                    </asp:TemplateField>
                   
                   <asp:TemplateField HeaderText="<%$ Resources:LocalString, AddressBookListCountry%>" >
                        <ItemTemplate>
                            <asp:Label ID="lblCountry" runat="server" Text='<%#Eval("Country").ToString() %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblCountry1" runat="server" Text='<%#Eval("Country").ToString() %>' Visible="false"></asp:Label>
                            <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack = "true"/>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, AddressBookListEmail%>" >
                        <ItemTemplate>
                            <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("Email").ToString() %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEmail" runat="server" Text='<%#Eval("Email").ToString() %>' MaxLength = "60"></asp:TextBox>                
                        </EditItemTemplate>
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="<%$ Resources:LocalString, AddressBookListLastPickUpMonThur%>" >
                        <ItemTemplate>
                            <asp:Label ID="lblLastPickupMondayToThursday" runat="server" Text='<%#Eval("LastPickupMondayToThursday").ToString() %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtLastPickupMondayToThursday" runat="server" Text='<%#Eval("LastPickupMondayToThursday").ToString() %>' MaxLength = "5"></asp:TextBox>                
                        </EditItemTemplate>
                    </asp:TemplateField>
                   
                   <asp:TemplateField HeaderText="<%$ Resources:LocalString, AddressBookListLastPickUpFriday%>" >
                        <ItemTemplate>
                            <asp:Label ID="lblLastPickupFriday" runat="server" Text='<%#Eval("LastPickupFriday").ToString() %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtLastPickupFriday" runat="server" Text='<%#Eval("LastPickupFriday").ToString() %>' MaxLength = "5"></asp:TextBox>                
                        </EditItemTemplate>
                    </asp:TemplateField>
                   
                   <asp:TemplateField HeaderText="<%$ Resources:LocalString, AddressBookListComments%>" >
                        <ItemTemplate>
                            <asp:Label ID="lblComments" runat="server" Text='<%#Eval("Comments").ToString() %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtComments" runat="server" Text='<%#Eval("Comments").ToString() %>' MaxLength = "300"></asp:TextBox>                
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                   <asp:TemplateField HeaderText="<%$ Resources:LocalString, AddressBookListShipPreference%>" >
                        <ItemTemplate>
                            <asp:CheckBox ID="chkEnableShippingPreferenceItem" runat="server"  Enabled="false" AutoPostBack="false"/>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:CheckBox ID="chkEnableShippingPreferenceEdit"  oncheckedchanged="chkEnableShippingPreference_CheckedChanged" AutoPostBack="true" runat="server" />            
                        </EditItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="<%$ Resources:LocalString, AddressBookEnableShippingPreference%>" >
                        <ItemTemplate>
                            <asp:Label ID="lblEnableShipPreference" runat="server" Text='<%#Eval("ShipPreference").ToString() %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblShipPreference1" runat="server" Text='<%#Eval("ShipPreference").ToString() %>' Visible="false"></asp:Label>
                            <asp:DropDownList ID="ddlShipPreference" runat="server"></asp:DropDownList>               
                        </EditItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="<%$ Resources:LocalString, AddressBookListComments%>" >
                       <ItemTemplate>
                      <asp:ReorderList ID="rlShippingPreference" runat="server" clientIdmode="AutoID"   
                                    DragHandleAlignment="Left" 
                                    ItemInsertLocation="Beginning"
                                    DataKeyField="Id"
                                    AllowReorder="false" PostBackOnReorder="false">                              
                            <ItemTemplate> 
                                    <asp:Label ID="lblShippingPreferenceType" runat="server" Text='<%# Eval("ShippingPreferenceType") %>'></asp:Label>                  
                            </ItemTemplate> 
                       </asp:ReorderList>   
                        </ItemTemplate>
                         <EditItemTemplate>
                      <asp:ReorderList ID="rlShippingPreferenceEdit" runat="server" clientIdmode="AutoID"   
                                    DragHandleAlignment="Left" 
                                    ItemInsertLocation="Beginning"
                                    DataKeyField="Id"
                                    AllowReorder="true" PostBackOnReorder="false">                              
                            <ItemTemplate> 
                                    <asp:Label ID="lblShippingPreferenceType" runat="server" Text='<%# Eval("ShippingPreferenceType") %>'></asp:Label>                  
                            </ItemTemplate> 
                       </asp:ReorderList>   
                    </EditItemTemplate>
                        </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="Edit" Text="<%$ Resources:LocalString, CRAEdit%>"></asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="btnUpdate" runat="server" CausesValidation="true" CommandName="Update" Text="<%$ Resources:LocalString, CRAUpdate%>" ></asp:LinkButton>
                            <asp:LinkButton id="btnCancel" runat="server" CausesValidation="false" CommandName="Cancel" Text="<%$ Resources:LocalString, CRACancel%>"></asp:LinkButton>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="Delete" Text="Delete"></asp:LinkButton>
                        </ItemTemplate>
                   </asp:TemplateField>

               </Columns>
    </asp:GridView>
    <asp:Label id="lblError" runat="server" Visible="false" CssClass="clsErrorMessage" />
    <asp:Label ID="valNumber" Text="<%$ Resources:LocalString, ValidationNumber %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalid %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valAccept" Text="<%$ Resources:LocalString, ValidationAccept %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valAlready" Text="<%$ Resources:LocalString, ValidationAlready %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="valInvalidTimeFormat" Text="<%$ Resources:LocalString, ValidationInvalidTimeFormat %>" runat ="server" Visible= "false"></asp:Label>
<asp:Label ID="ValLessThan" Text="<%$ Resources:LocalString, ValLessThan %>" runat ="server" Visible= "false"></asp:Label>
 <!--   </div>-->



</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="HeadContent">
    <style type="text/css">
        .clsGridRow
        {}
    </style>
</asp:Content>