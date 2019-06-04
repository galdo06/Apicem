<%@ Page Language="C#" MasterPageFile="~/App_Resources/master.master" AutoEventWireup="true" Inherits="Public_Log_On"
    Title="Iniciar Sesión | N@TURA"
    CodeBehind="log-in.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceholder" runat="Server">
    <style type="text/css">
        .dropdownwrap {
            height: auto;
            width: 740px;
            float: left;
            margin: 0px 0px 0px 0px;
            display: none;
        }

        .dropdown {
            position: relative;
            top: 3px;
        }
    </style>
    <script type="text/javascript">
        var apiLocation;
        $(function () { 

        });

        function lnkClick(lnk) {
            if (lnk.mouseleaveBind == null || lnk.mouseleaveBind) {
                $('#divSocialInterest').slideDown('400');
                lnk.mouseleaveBind = false;
            } else {
                $('#divSocialInterest').slideUp('400');
                lnk.mouseleaveBind = true;
            }
        };

        function chkClick(chk) {
            $('#divVTermsalidator').slideUp('400');
        };

        function btnClick() {
            if (!$('#chkTerms')[0].checked) {
                $('#divVTermsalidator').slideDown('400');
                return false;
            }
            return true;
        };

    </script>
    <h1 class="title-regular clearfix">N@TURA</h1>
    <asp:Literal runat="server" EnableViewState="False" ID="labelMessage"></asp:Literal>

    <div id="divVTermsalidator" style="display: none;" class='notice'>Debe aceptar los Términos y Codiciones para continuar</div>
    <p>
        N@TURA es una solución orientada a faclitar a los Profesionales de Siembra y Forestación de Puerto Rico la obtención y procesamiento de datos en sus distintas tareas de trabajo. Entre su Nombre de Usuario y Contraseña para iniciar.
    </p>
    <asp:Login ID="Login1" OnAuthenticate="Login1_Authenticate" OnLoggedIn="Login1_LoggedIn" runat="server">
        <LayoutTemplate>
            <p class="inline">
                <label for="name">Nombre de Usuario</label>
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox runat="server" name="name" ID="UserName" type="text" class="text" />
                <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="Required"
                    CssClass="validator" ControlToValidate="UserName" Display="Dynamic" ValidationGroup="Login1"></asp:RequiredFieldValidator><br />
                <label for="password">Contraseña</label>
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox runat="server" ID="Password" type="password" class="text" />
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Required"
                    CssClass="validator" ControlToValidate="Password" Display="Dynamic" ValidationGroup="Login1"></asp:RequiredFieldValidator><br />
                <br />
                <label for="check1">
                    <asp:CheckBox runat="server" title="remember" type="checkbox" name="checkboxRemember" ClientIDMode="Static" ID="chkTerms" onclick="chkClick(this);" />
                    <asp:LinkButton OnClientClick="lnkClick(this); return false;" Text="Acepto los Términos y Condiciones" runat="server" />
                    <br />
                    <asp:CheckBox runat="server" title="remember" type="checkbox" name="checkboxRemember" ID="chkRememberMe" Text="Recordarme"
                        value="" />
                </label>
                <br />
            </p>
            <p>
                <asp:Button ID="LoginButton" CommandName="Login" SkinID="Button" runat="server" Text="Iniciar" ValidationGroup="Login1" OnClientClick="return btnClick();" />
            </p>
            <hr />
            <div id="divSocialInterest" clientidmode="Static" runat="server" class='success dropdownwrap'></div>
        </LayoutTemplate>
    </asp:Login>

</asp:Content>
