<%@ Page Language="C#" MasterPageFile="~/App_Resources/master.master" AutoEventWireup="false"
    Inherits="ChangePassword_Details" Title="Nombre Común | N@TURA"
    CodeBehind="changepassword.aspx.cs" %>

<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContentPlaceholder" runat="Server">
    <div class="FormView">
        <h1 class="title-regular clearfix">
            <span class="grid_15 alpha">Cambiar Contraseña</span>
        </h1>
        <asp:Literal EnableViewState="False" runat="server" ID="ltlMessage"></asp:Literal>
        <div class="grid_10 inline alpha" style="width: 450px;">
            Contraseña Actual:
                <span class="required-field-indicator">*</span><br /> 
            <asp:TextBox ID="txtActualPassword" runat="server" CssClass="text" MaxLength="15" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvActualPassword" runat="server"
                CssClass="validator" ControlToValidate="txtActualPassword" Display="Dynamic"><br />Requerido</asp:RequiredFieldValidator>
            <br />
            Nueva Contraseña:
                <span class="required-field-indicator">*</span><br />
            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="text" MaxLength="15" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server"
                CssClass="validator" ControlToValidate="txtNewPassword" Display="Dynamic"><br />Requerido</asp:RequiredFieldValidator>
            <br />
            Reingresar Nueva Contraseña:
                <span class="required-field-indicator">*</span><br />
            <asp:TextBox ID="txtNewPassword2" runat="server" CssClass="text" MaxLength="15" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvNewPassword2" runat="server"
                CssClass="validator" ControlToValidate="txtNewPassword2" Display="Dynamic"><br />Requerido</asp:RequiredFieldValidator>
            <br />
            <hr width="600px" />
            <p>
                <asp:Button ID="btnSave" runat="server" Text="Guardar" OnClick="ButtonSave_Click" SkinID="Button" />
                <asp:Button ID="btnBack" CausesValidation="false" runat="server" Text="Volver" OnClick="ButtonGoToListPage_Click"
                    SkinID="AltButton" />
            </p>
            <em>Los campos requeridos están marcados con un <span class="required-field-indicator">*</span></em>
        </div>
    </div>
</asp:Content>

