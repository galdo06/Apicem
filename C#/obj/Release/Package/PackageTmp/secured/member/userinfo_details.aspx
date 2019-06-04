<%@ Page Language="C#" MasterPageFile="~/App_Resources/master.master" AutoEventWireup="false"
    Inherits="UserInfo_Details" Title="Usuario | N@TURA" MetaKeywords="user,details"
    MetaDescription="Detalles del Usuario. Esta información es importante el la generación de reportes."
    CodeBehind="userinfo_details.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#BodyContentPlaceholder_formViewUserInfo_txtZipCode").mask("99999?-9999");
            $("#BodyContentPlaceholder_formViewUserInfo_txtPhone").mask("(999) 999-9999");
            $("#BodyContentPlaceholder_formViewUserInfo_txtCellPhone").mask("(999) 999-9999");
            $("#BodyContentPlaceholder_formViewUserInfo_txtPhoneExtension").mask("?9999");
            $("#BodyContentPlaceholder_formViewUserInfo_txtFax").mask("(999) 999-9999");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceholder" runat="Server">
    <asp:Literal EnableViewState="False" runat="server" ID="ltlMessage"></asp:Literal>
    <asp:FormView SkinID="FormView" ID="formViewUserInfo" runat="server" DataSourceID="odsUserInfo_Details"
        DataKeyNames="UserID" EnableViewState="true" OnItemUpdating="FormViewUserInfo_ItemUpdating"
        OnItemUpdated="formViewUserInfo_ItemUpdated" OnItemInserting="FormViewUserInfo_ItemInserting"
        OnItemInserted="formViewUserInfo_ItemInserted" OnDataBound="FormViewUserInfo_DataBound">
        <ItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">Ver Detalles de Usuario</span> </h1>
    <p>
        Detalles del Usuario. Esta información es importante el la generación de reportes.
    </p>
            <div class="grid_10 inline alpha">
                <strong>Nombre:</strong>
                <%# Eval("FirstName") %>
                <br />
                <strong>Segundo Nombre:</strong>
                <%# Eval("MiddleName") %>
                <br />
                <strong>Apellido:</strong>
                <%# Eval("LastName") %>
                <br />
                <strong>Segundo Apellido:</strong>
                <%# Eval("SecondLastName") %><br />
                <strong>Título:</strong>
                <%# Eval("Title") %><br />
                <strong>Licencia:</strong>
                <%# Eval("License") %><br />
            </div>
            <div class="grid_9 inline omega">
                <strong>Dirección:</strong>
                <%# Eval("Address1") %><br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <%# Eval("Address2") %><br />
                <strong>Ciudad:</strong>
                <%# Eval("City") %><br />
                <strong>Estado:</strong>
                <%# Eval("State") %><br />
                <strong>Zip Code:</strong>
                <%# Eisk.Web.App_Logic.Helpers.Utility.FormatUSZipCode((string)Eval("ZipCode")) %><br />
                <strong>Teléfono:</strong>
                <%# Eisk.Web.App_Logic.Helpers.Utility.FormatUSPhoneNumber((string)Eval("Phone")) %><br />
                <strong>Extensión:</strong>
                <%# Eval("PhoneExtension") %><br />
                <strong>Celular:</strong>
                <%# Eisk.Web.App_Logic.Helpers.Utility.FormatUSPhoneNumber((string)Eval("CellPhone")) %><br />
                <strong>Fax:</strong>
                <%# Eisk.Web.App_Logic.Helpers.Utility.FormatUSPhoneNumber((string)Eval("Fax")) %><br />
                <br />
            </div>
            <hr />
            <asp:Button ID="Button2" runat="server" Text="Editar" OnClick="ButtonEdit_Click" SkinID="Button" />
        </ItemTemplate>
        <EditItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">Información de Usuario</span> </h1>
                <p>
        Detalles del Usuario. Esta información es importante el la generación de reportes.
    </p>
            <div class="grid_10 inline alpha">
                <label>
                    Nombre:</label>
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox ID="txtFirstName" runat="server" CssClass="text" MaxLength="100" Text='<%# Bind("FirstName") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server"
                    CssClass="validator" ControlToValidate="txtFirstName" Display="Dynamic"><br />Required</asp:RequiredFieldValidator><br />
                <label>
                    Segundo Nombre:</label><br />
                <asp:TextBox ID="txtMiddleName" runat="server" CssClass="text" MaxLength="100" Text='<%# Bind("MiddleName") %>'></asp:TextBox><br />
                <label>
                    Apellido:</label>
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox ID="txtLastName" runat="server" CssClass="text" MaxLength="100" Text='<%# Bind("LastName") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                    CssClass="validator" ControlToValidate="txtLastName" Display="Dynamic"><br />Required</asp:RequiredFieldValidator><br />
                <label>
                    Segundo Apellido:</label><br />
                <asp:TextBox ID="txtSecondLastName" runat="server" CssClass="text" MaxLength="100" Text='<%# Bind("SecondLastName") %>'></asp:TextBox><br />
                <label>
                    Título:</label><br />
                <asp:TextBox ID="txtTitle" runat="server" CssClass="text" MaxLength="100" Text='<%# Bind("Title") %>'></asp:TextBox><br />
                <label>
                    Licencia:</label><br />
                <asp:TextBox ID="txtLicense" runat="server" CssClass="text" MaxLength="100" Text='<%# Bind("License") %>'></asp:TextBox><br />

            </div>
            <div class="grid_9 inline omega">
                <label>
                    Dirección:</label><br />
                <asp:TextBox ID="txtAddress1" runat="server" CssClass="text" MaxLength="200" Text='<%# Bind("Address1") %>'></asp:TextBox><br />
                <asp:TextBox ID="txtAddress2" runat="server" CssClass="text" MaxLength="200" Text='<%# Bind("Address2") %>'></asp:TextBox><br />
                <br />
                <label>
                    Ciudad:</label><br />
                <asp:TextBox ID="txtCity" runat="server" CssClass="text" MaxLength="100" Text='<%# Bind("City") %>'></asp:TextBox><br />
                <label>
                    Estado:</label><br />
                <asp:TextBox ID="txtState" runat="server" CssClass="text" MaxLength="100" Text='<%# Bind("State") %>'></asp:TextBox><br />
                <label>
                    Zip Code:</label><br />
                <asp:TextBox ID="txtZipCode" runat="server" CssClass="text" MaxLength="10" Text='<%# Eisk.Web.App_Logic.Helpers.Utility.FormatUSZipCode((string)Eval("ZipCode")) %>'></asp:TextBox>
                <asp:RegularExpressionValidator ID="revZipCode" runat="server"
                    CssClass="validator" ControlToValidate="txtZipCode" Display="Dynamic" ValidationExpression="\d{5}(-\d{4})?"><br />Incorrect Zip Code format</asp:RegularExpressionValidator><br />
                <label>
                    Teléfono:</label><br />
                <asp:TextBox ID="txtPhone" runat="server" CssClass="text" MaxLength="15" Text='<%# Eisk.Web.App_Logic.Helpers.Utility.FormatUSPhoneNumber((string)Eval("Phone")) %>'></asp:TextBox>
                <asp:RegularExpressionValidator ID="revPhone" runat="server"
                    CssClass="validator" ControlToValidate="txtPhone" Display="Dynamic" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"><br />Incorrect Phone Number format</asp:RegularExpressionValidator><br />
                <label>
                    Extensión:</label><br />
                <asp:TextBox ID="txtPhoneExtension" runat="server" CssClass="text" MaxLength="5" Width="50px" Text='<%# Bind("PhoneExtension") %>'></asp:TextBox>
                <asp:RegularExpressionValidator ID="revPhoneExtension" runat="server"
                    CssClass="validator" ControlToValidate="txtPhoneExtension" Display="Dynamic" ValidationExpression="^\d*"><br />Numbers only allowed</asp:RegularExpressionValidator><br />
                <label>
                    Celular:</label>
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox ID="txtCellPhone" runat="server" CssClass="text" MaxLength="15" Text='<%# Eisk.Web.App_Logic.Helpers.Utility.FormatUSPhoneNumber((string)Eval("CellPhone")) %>'></asp:TextBox>
                <asp:RegularExpressionValidator ID="revCellPhone" runat="server"
                    CssClass="validator" ControlToValidate="txtCellPhone" Display="Dynamic" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"><br />Incorrect Cell Phone Number format</asp:RegularExpressionValidator><br />
                <label>
                    Fax:</label>
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox ID="txtFax" runat="server" CssClass="text" MaxLength="15" Text='<%# Eisk.Web.App_Logic.Helpers.Utility.FormatUSPhoneNumber((string)Eval("Fax")) %>'></asp:TextBox>
                <asp:RegularExpressionValidator ID="revFax" runat="server"
                    CssClass="validator" ControlToValidate="txtFax" Display="Dynamic" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"><br />Incorrect Fax Number format</asp:RegularExpressionValidator><br />
                <hr />
                <p>
                    <asp:Button ID="btnSave" runat="server" Text="Guardar" OnClick="ButtonSave_Click" SkinID="Button" />
                    <asp:Button ID="btnBack" CausesValidation="false" runat="server" Text="Volver" OnClick="ButtonGoToListPage_Click"
                        SkinID="AltButton" />
                </p>
                <em>Los campos requeridos están marcados con un <span class="required-field-indicator">*</span></em>
            </div>
        </EditItemTemplate>


    </asp:FormView>
    <asp:ObjectDataSource ID="odsUserInfo_Details"      runat="server" SelectMethod="GetCurrentUserUserInfo"
        InsertMethod="CreateNewUserInfo" UpdateMethod="UpdateUserInfo" DataObjectTypeName="Eisk.BusinessEntities.UserInfo"
        TypeName="Eisk.BusinessLogicLayer.UserInfoBLL" OnSelecting="OdsUserInfo_Details_Selecting"
        OnInserted="OdsUserInfo_Details_Inserted">
        <UpdateParameters>
            <asp:ControlParameter Name="UserID" ControlID="formViewUserInfo" />
            <asp:Parameter Name="FirstName" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="MiddleName" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="LastName" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="SecondLastName" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="Title" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="License" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="Address1" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="Address2" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="City" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="State" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="ZipCode" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="Phone" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="PhoneExtension" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="CellPhone" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="Fax" ConvertEmptyStringToNull="true" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="UserInfo" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="FirstName" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="MiddleName" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="LastName" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="SecondLastName" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="Title" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="License" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="Address1" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="Address2" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="City" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="State" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="ZipCode" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="Phone" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="PhoneExtension" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="CellPhone" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="Fax" ConvertEmptyStringToNull="true" />
        </InsertParameters>
    </asp:ObjectDataSource>
</asp:Content>
