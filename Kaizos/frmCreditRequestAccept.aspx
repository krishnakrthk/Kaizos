<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCreditRequestAccept.aspx.cs" Inherits="Kaizos.frmCreditRequestAccept" MasterPageFile="~/NewSite.master"%>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="grdAuthorizedList" runat="server" AutoGenerateColumns="false" DataKeyNames="AccountNo"
        HeaderStyle-CssClass="gridHeader" 
        onrowcommand="grdAuthorizedList_RowCommand" 
        onrowediting="grdAuthorizedList_RowEditing" 
        onrowupdating="grdAuthorizedList_RowUpdating" 
        onrowcancelingedit="grdAuthorizedList_RowCancelingEdit">
        <Columns>
            <asp:TemplateField HeaderText="<%$ Resources:LocalString, CRACompanyName %>" >
                <ItemTemplate>
                    <asp:Label ID="lblCompanyName" runat="server" Text='<%#Eval("CompanyName").ToString() %>'></asp:Label>
                    <asp:Label ID="lblAccountNo" runat="server" Text='<%#Eval("AccountNo").ToString() %>' Visible = "false"></asp:Label>
                    <asp:TextBox ID="txtAccountNo" runat="server" Text='<%#Eval("AccountNo").ToString() %>' Visible = "false"></asp:TextBox>                
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ Resources:LocalString, CRAWishedBudgetAmount %>" >
                <ItemTemplate>
                    <asp:Label ID="lblWishedBudgetAmount" runat="server" Text='<%#Eval("WishedBudgetAmount").ToString() %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtWishedBudgetAmount" runat="server" Text='<%#Eval("WishedBudgetAmount").ToString() %>'></asp:TextBox>                
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ Resources:LocalString, CRABudgetAmount %>" >
                <ItemTemplate>
                    <asp:Label ID="lblBudgetAmount" runat="server" Text='<%#Eval("BudgetAmount").ToString() %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ Resources:LocalString, CRAInsuredCreditAmount%>" >
                <ItemTemplate>
                    <asp:Label ID="lblInsuredCreditAmount" runat="server" Text='<%#Eval("InsuredCreditAmount").ToString() %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtInsuredCreditAmount" runat="server" Text='<%#Eval("InsuredCreditAmount").ToString() %>'></asp:TextBox>                
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ Resources:LocalString, CRAPaymentDelayDays%>" >
                <ItemTemplate>
                    <asp:Label ID="lblPaymentDelayDays" runat="server" Text='<%#Eval("PaymentDelayDays").ToString() %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtPaymentDelayDays" runat="server" Text='<%#Eval("PaymentDelayDays").ToString() %>'></asp:TextBox>                
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ Resources:LocalString, CRAGrossMargin%>" >
                <ItemTemplate>
                    <asp:Label ID="lblGrossMargin" runat="server" Text='<%#Eval("GrossMargin").ToString() %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtGrossMargin" runat="server" Text='<%#Eval("GrossMargin").ToString() %>'></asp:TextBox>                
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ Resources:LocalString, CRAPaymentDelayMonth%>" >
                <ItemTemplate>
                    <asp:Label ID="lblPaymentDelayMonth" runat="server" Text='<%#Eval("PaymentDelayMonth").ToString() %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtPaymentDelayMonth" runat="server" Text='<%#Eval("PaymentDelayMonth").ToString() %>'></asp:TextBox>                
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ Resources:LocalString, CRACompensationRate%>" >
                <ItemTemplate>
                    <asp:Label ID="lblCompensationRate" runat="server" Text='<%#Eval("CompensationRate").ToString() %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtCompensationRate" runat="server" Text='<%#Eval("CompensationRate").ToString() %>'></asp:TextBox>                
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ Resources:LocalString, CRADepositAmount%>" >
                <ItemTemplate>
                    <asp:Label ID="lblDepositAmount" runat="server" Text='<%#Eval("DepositAmount").ToString() %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ Resources:LocalString, CRAAuthorizedCreditLimit%>" >
                <ItemTemplate>
                    <asp:Label ID="lblAuthorizedCreditLimit" runat="server" Text='<%#Eval("AuthorizedCreditLimit").ToString() %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ Resources:LocalString, CRAAuthorizedCreditAmount%>" >
                <ItemTemplate>
                    <asp:Label ID="lblAuthorizedCreditAmount" runat="server" Text='<%#Eval("AuthorizedCreditAmount").ToString() %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtAuthorizedCreditAmount" runat="server" Text='<%#Eval("AuthorizedCreditAmount").ToString() %>'></asp:TextBox>                
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ Resources:LocalString, CRAChkAccept%>" >
                <ItemTemplate>
                    <asp:CheckBox ID="chkCS" runat="server" Checked = 'false' Enabled = "false"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="Edit" Text="<%$ Resources:LocalString, CRAEdit%>"></asp:LinkButton>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:LinkButton ID="btnUpdate" runat="server" CausesValidation="true" CommandName="Update" Text="<%$ Resources:LocalString, CRAUpdate%>"></asp:LinkButton>
                    <asp:LinkButton id="btnCancel" runat="server" CausesValidation="false" CommandName="Cancel" Text="<%$ Resources:LocalString, CRACancel%>"></asp:LinkButton>
                </EditItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="HeadContent">
    
</asp:Content>


