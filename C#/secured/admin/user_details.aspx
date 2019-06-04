<%@ Page Language="C#" MasterPageFile="~/App_Resources/master.master" AutoEventWireup="false"
    Inherits="User_Details" Title="Usuario | N@TURA" MetaKeywords="usuario"
    CodeBehind="user_details.aspx.cs" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <script src='<%# ResolveUrl ("~/App_Resources/client-scripts/framework/jquery.ui.core.js") %>'
        type="text/javascript"></script>
    <script src='<%# ResolveUrl ("~/App_Resources/client-scripts/framework/jquery.ui.datepicker.js") %>'
        type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(".datepicker").datepicker({
                changeMonth: true,
                changeYear: true,
                showOn: "both",
                buttonImage: "../../App_Resources/images/ico-cal.png",
                buttonImageOnly: true,
                showAnim: 'slideDown'
            });
        });
    </script>
    <script type="text/javascript">

        $("#anim").change(function () {
            $("#datepicker").datepicker("option", "showAnim", $(this).val());
        });

    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContentPlaceholder" runat="Server">
    <asp:Literal EnableViewState="False" runat="server" ID="ltlMessage"></asp:Literal>
    <asp:FormView SkinID="FormView" ID="formViewUser" runat="server" DataSourceID="odsUser_Details"
        DataKeyNames="UserId" EnableViewState="true" OnItemUpdating="FormViewUser_ItemUpdating"
        OnItemUpdated="formViewUser_ItemUpdated" OnDataBound="FormViewUser_DataBound" OnDataBinding="formViewUser_DataBinding">
        <ItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">Ver Usuario</span> <span class="grid_3 omega align-right">
                    <asp:LinkButton ID="LinkButton1" OnClientClick="print()" runat="server" Text="Imprimir"
                        CssClass="button small white"></asp:LinkButton></span></h1>
            <div class="grid_10 inline alpha">
                <strong>Usuario:</strong>
                <asp:Label ID="Label1" Text='<%# Eval("UserName") %>' runat="server" />
                <br />
                <br />
                <strong>Rol:</strong>
                <asp:Label ID="Label2" Text='<%# new Eisk.BusinessLogicLayer.Role_UsersBLL().GetRole_UsersByUserID( (Guid)Eval("UserID"))[0].Role.RoleName %>' runat="server" />
                <br />
                <strong>Grupo:</strong>
                <asp:Label ID="Label3" Text='<%# new Eisk.BusinessLogicLayer.Group_UsersBLL().GetGroup_UsersByUserID( (Guid)Eval("UserID"))[0].Group.GroupName %>' runat="server" />
                <br />
                <asp:Panel ID="pnlDates" runat="server" Visible='<%#  Eval("CreatorUserID") != null %>'>
                    <br />
                    <strong>Creado Por:</strong>
                    <asp:Label ID="lblCreatedUserId" Text='<%#  Eval("CreatorUserID") == null ? "" : creator.UserName %>' runat="server" />
                    <br />
                    <strong>Creado En:</strong>
                    <asp:Label ID="lblCreatedDate" Text='<%# Eval("CreatedDate", "{0:MMM/dd/yyyy h:mm:ss tt}")%>' runat="server" />
                    <br />
                    <strong>Editado Por:</strong>
                    <asp:Label ID="lblEditedUserId" Text='<%#  Eval("EditorUserID") == null ? "" : editor.UserName %>' runat="server" />
                    <br />
                    <strong>Editado En:</strong>
                    <asp:Label ID="lblEditedDate" Text='<%# Eval("EditedDate", "{0:MMM/dd/yyyy h:mm:ss tt}")%>' runat="server" />
                    <br />
                    <br />
                </asp:Panel>
                <hr />
                <asp:Button CausesValidation="false" ID="btnBack" runat="server" Text="Volver" OnClick="ButtonGoToListPage_Click"
                    SkinID="AltButton" />
                <asp:Button ID="Button2" runat="server" Text="Editar" OnClick="ButtonEdit_Click" SkinID="Button" />
        </ItemTemplate>
        <EditItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">Editar Usuario</span> </h1>
            <div class="grid_10 inline alpha">
                <strong>Nombre de Usuario Actual:</strong>
                <asp:Label ID="Label1" Text='<%# Eval("UserName") %>' runat="server" />
                <br />
                <br />
                Nombre de Usuario:
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox ID="txtUser" Text='<%# Bind("UserName") %>' runat="server" CssClass="text" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="Requerido"
                    CssClass="validator" ControlToValidate="txtUser" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revUserName" runat="server"
                    CssClass="validator" ControlToValidate="txtUser" Display="Dynamic" ValidationGroup="CreateUserWizard1" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"><br />Invalid Email format</asp:RegularExpressionValidator><br />
                <label>
                    Seleccionar Rol:</label><br />
                <asp:DropDownList ID="ddlRoles" runat="server" AppendDataBoundItems="true"
                    AutoPostBack="false" DataSourceID="odsRoleList" DataTextField="RoleName"
                    DataValueField="RoleID" EnableViewState="true">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsRoleList"       runat="server" TypeName="Eisk.BusinessLogicLayer.RoleBLL"
                    EnableViewState="true" SelectMethod="GetAllRolesList" />
                <br />
                <label>
                    Seleccionar Grupo:</label><br />
                <asp:DropDownList ID="ddlGroups" runat="server" AppendDataBoundItems="true"
                    AutoPostBack="false" DataSourceID="odsGroupList" DataTextField="GroupName"
                    DataValueField="GroupID" EnableViewState="true">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsGroupList"       runat="server" TypeName="Eisk.BusinessLogicLayer.GroupBLL"
                    EnableViewState="true" SelectMethod="GetAllGroupsList" />
                <br />
                <br />
                <asp:Panel ID="pnlDates" runat="server" Visible='<%#  Eval("CreatorUserID") != null %>'>
                    <strong>Creado Por:</strong>
                    <asp:Label ID="lblCreatedUserId" Text='<%#  Eval("CreatorUserID") == null ? "" : creator.UserName %>' runat="server" />
                    <br />
                    <strong>Creado En:</strong>
                    <asp:Label ID="lblCreatedDate" Text='<%# Eval("CreatedDate", "{0:MMM/dd/yyyy h:mm:ss tt}")%>' runat="server" />
                    <br />
                    <strong>Editado Por:</strong>
                    <asp:Label ID="lblEditedUserId" Text='<%#  Eval("EditorUserID") == null ? "" : editor.UserName %>' runat="server" />
                    <br />
                    <strong>Editado En:</strong>
                    <asp:Label ID="lblEditedDate" Text='<%# Eval("EditedDate", "{0:MMM/dd/yyyy h:mm:ss tt}")%>' runat="server" />
                    <br />
                    <br />
                </asp:Panel>
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
    <asp:ObjectDataSource ID="odsUser_Details"       runat="server" SelectMethod="GetUserByUserID"
        UpdateMethod="UpdateUser" DataObjectTypeName="Eisk.BusinessEntities.User"
        TypeName="Eisk.BusinessLogicLayer.UserBLL" OnSelecting="OdsUser_Details_Selecting">
        <UpdateParameters>
            <asp:ControlParameter Name="UserId" ControlID="formViewUser" />
            <asp:Parameter Name="User" ConvertEmptyStringToNull="true" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="UserName" ConvertEmptyStringToNull="true" />
        </InsertParameters>
        <SelectParameters>
            <asp:RouteParameter Name="UserId" RouteKey="user_id" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

