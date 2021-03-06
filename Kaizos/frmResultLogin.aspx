﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmResultLogin.aspx.cs" Inherits="Kaizos.frmResultLogin" %>

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
				<li><a href="index.htm">Accueil</a></li>
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
                    <fieldset class="first">
                        <legend>
                            <asp:Label ID="lblResult" runat="server" Text="Result"></asp:Label> 
                        </legend>
                        <asp:Label runat="server" ID="lblDisplayMsg" Text=""></asp:Label><br /><br />
                            <asp:LinkButton ID="lbReturnUrl" runat="server" Text="Return" ></asp:LinkButton>
                
                    </fieldset>
                </form>
                <!-- EDITABLE CONTENT STOPS HERE -->
			</div>
			<div id="column2">
				<div class="white_bloc">
					<h3>
						CONTACTEZ-NOUS :</h3>
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

