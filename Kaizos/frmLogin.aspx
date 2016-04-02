<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmLogin.aspx.cs" Inherits="Kaizos.frmLogin" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>
<html lang="fr">
<head id="Head2" runat="server">
	<title>Kaizos - Login</title>
	<!-- Meta-data -->
	<meta charset='utf-8' />
	<meta name='description' content="Solution d'expédition" />
	<meta name='keywords' content="" />
	<meta name='author' content='IPS Europe' />
	<meta name='copyright' content='&copy; 2011 IPS Europe' />
	<meta name='language' content='fr' />
	<!-- Design -->
	<link rel='shortcut icon' href='favicon.ico' />
	<link href='css/std.css' type='text/css' rel='stylesheet' media='screen' charset='utf-8' />
	<!--[if IE]><link href='css/ie.css' type='text/css' rel='stylesheet' media='screen' charset='utf-8' /><![endif]-->
	<!--[if lte IE 7]><link href='css/ie7-.css' type='text/css' rel='stylesheet' media='screen' charset='utf-8' /><![endif]-->
	<!--[if lte IE 8]><link href='css/ie8-.css' type='text/css' rel='stylesheet' media='screen' charset='utf-8' /><![endif]-->
</head>

<body class="home">
	<div id="container">
		<div id="head">
			<a id="logo" href="index.htm"><img src="img/main/logo_kaizos.png" alt="Logo Kaizos" /></a>
			<a id="login" href="frmLogin.aspx">LOGIN</a>

			<ul id="menu">
				<li><a href="index.htm" class="current">Accueil</a></li>
				<li><a href="services.htm">Services</a></li>
				<li><a href="help.htm">Conseils</a>
					<ul>
						<li><a href="help.htm#open_account">Créer votre compte</a></li>
						<li><a href="help.htm#invoicing_payment">Facturation et paiement</a></li>
						<li><a href="help.htm#insurance">Assurance</a></li>
						<li><a href="help.htm#tips">Conseils utiles</a></li>
					</ul>
				</li>
				<li><a href="contact.htm">Contact</a></li>
			</ul>
			<div id="head_actions">
				<div>
					<a href='demo.htm'>Démo<br />Application</a>
					<a href='register.htm'>Créer compte<br />& expédier</a>
				</div>
			</div>
		</div>
		<div id="content">
			<div id="column1">
				<!-- EDITABLE CONTENT STARTS HERE -->
				<form id="Form1" class="form1" runat="server">
				<asp:ScriptManager ID="ScriptManager1" runat="server" />

                <fieldset id="Fieldset1" runat="server">
                    <legend>
                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalString, LoginLogin %>"></asp:Label>
                    </legend>
                
                    <label for="txtEmail"><asp:Label ID="lblEmail" runat="server" Text="<%$ Resources:LocalString, LoginEmail %>"></asp:Label></label>
					<asp:TextBox ID="txtEmail" runat="server" ValidationGroup="grpLogin"></asp:TextBox>
                
                    <label for="txtPassword"><asp:Label ID="lblPassword" runat="server" Text="<%$ Resources:LocalString, LoginPassword%>"></asp:Label></label>
                    <asp:TextBox ID="txtPassword" runat="server" ValidationGroup="grpLogin" TextMode="Password"></asp:TextBox>
                
                    <div id="buttons">
						<asp:Button ID="btnLogin" runat="server" Text="<%$ Resources:LocalString, LoginbtnLogin%>" onclick="Button1_Click" ValidationGroup="grpLogin" /> <br />
                
                        <asp:HyperLink ID="hypEndCustomer" runat="server" NavigateUrl="frmEndCustomer.aspx" >
                            <asp:Label ID="lblSignUp" runat="server" Text="<%$ Resources:LocalString, LoginSignUp%>"></asp:Label>
                        </asp:HyperLink> - 
                        <asp:HyperLink ID="hybPWDRecovery" runat="server" NavigateUrl="frmPasswordRecovery.aspx" >
                            <asp:Label ID="lblForgotPassword" runat="server" Text="<%$ Resources:LocalString, LoginForgotPassword%>"></asp:Label>
                        </asp:HyperLink>
                    </div>
                </fieldset>

            <input id="btnMessage" type="button" runat="server" style="display: none"/>
            
            <asp:modalpopupextender id="TOSAcceptModalPopupExtender" runat="server"  
                targetcontrolid="btnMessage" popupcontrolid="MessagePanel"
		         DropShadow="false">
            </asp:modalpopupextender>

            <asp:panel class="customerModelWindow" id="MessagePanel" style="display: none" runat="server">
            
                <div class="TitlebarLeft">
                    <asp:Label ID="lblInformation" Text="<%$ Resources:LocalString, LoginMessageHeader %>" runat="server"  CssClass="clsLabelHeader"/>
                </div>
        
                <div class="customerDivModalWindow">
                    <asp:Label ID="lblMessage" runat="server" />
                </div>
                <table>
                     <tr>
                        <td align="right">
                            <asp:Button id = "BtnOK" Text = "Yes" runat = "server" class="clsMessgeButton" onclick="BtnOK_Click"/>
                            <asp:Button id = "btnCancel" Text = "No" runat = "server" class="clsMessgeButton"/>
                        </td>
                    </tr>
                </table>
            </asp:panel>

            <div class="divSummaryLogin">
                <asp:Label ID="valEmpty" Text="<%$ Resources:LocalString, ValidationEmptyLogin %>" runat ="server" Visible= "false"></asp:Label>
                <asp:Label ID="valInvalid" Text="<%$ Resources:LocalString, ValidationInvalidLogin %>" runat ="server" Visible= "false"></asp:Label>
                <asp:Label ID="valLength" Text="<%$ Resources:LocalString, ValidationLengthLogin %>" runat ="server" Visible= "false"></asp:Label>
                <asp:Label ID="valNotAvailable" Text="<%$ Resources:LocalString, valNotAvailableLogin %>" runat ="server" Visible= "false"></asp:Label>

                <asp:CustomValidator ID="val_Login" runat="server" 
                                ControlToValidate="txtEmail" 
                                EnableClientScript="False" 
                                ValidateEmptyText="True"
                                ValidationGroup="grpLogin" 
                        CssClass="clsErrorMessage" onservervalidate="val_Login_ServerValidate">
                </asp:CustomValidator>
            </div>
    </form>
<!-- EDITABLE CONTENT STOPS HERE -->
			</div>
			<div id="column2">
				<div class="white_bloc">
					<h3>
						CONTACTEZ-NOUS :<h3>
							<ul>
								<li>Tél : 01 30 15 78 29</li>
								<li>E-mail : <a href="mailto:commercial@kaizos.com">commercial@kaizos.com</a></li>
							</ul>
				</div>
				<div id="shipping_solution">
					THE BEST SHIPPING
					<br />
					SOLUTION FOR EACH<br />
					DESTINATION
                </div>
			</div>
		</div>
	</div>
	<hr class="break" />
	<div id="foot">
		<div id="foot_content">
			<a id="website" href="http://www.kaizos.com">www.kaizos.com</a>
			<div id="legal">
				<ul>
					<li><a href="mentions_legales.htm"> Mentions légales</a></li>
				</ul>
				<p>
					Copyright &reg; 2011 Kaizos. Tous droits réservés</p>
			</div>
		</div>
	</div>
</body>
</html>
