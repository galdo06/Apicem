<%@ Page Language="C#" MasterPageFile="~/App_Resources/master.master" AutoEventWireup="false"
    Inherits="Project_Details" Title="Proyecto | N@TURA"
    CodeBehind="project_details.aspx.cs" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <script type="text/javascript" src='<%# ResolveUrl ("~/Scripts/jquery.tipTip.minified.js") %>'></script>
    <link href='<%# ResolveUrl ("~/Content/tipTip/tipTip.css") %>' rel="stylesheet" />
    <script type="text/javascript">
        $(function () {
            $("#BodyContentPlaceholder_formViewProject_txtZipCode").mask("99999?-9999");
            $(".tiptip").tipTip({ maxWidth: "auto", edgeOffset: 10, defaultPosition: "right" });
        });
    </script>

</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContentPlaceholder" runat="Server">
    <asp:Literal EnableViewState="False" runat="server" ID="ltlMessage"></asp:Literal>
    <asp:FormView SkinID="FormView" ID="formViewProject" runat="server" DataSourceID="odsProject_Details"
        DataKeyNames="ProjectId" EnableViewState="true" OnItemUpdating="FormViewProject_ItemUpdating"
        OnItemUpdated="formViewProject_ItemUpdated" OnItemInserting="FormViewProject_ItemInserting"
        OnItemInserted="formViewProject_ItemInserted" OnDataBound="FormViewProject_DataBound" OnDataBinding="formViewProject_DataBinding">
        <ItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">Detalles Generales del Proyecto</span> <span class="grid_3 omega align-right">
                    <asp:LinkButton ID="LinkButton1" OnClientClick="print()" runat="server" Text="Imprimir"
                        CssClass="button small white"></asp:LinkButton></span></h1>
            <div class="inline">
                <div class="grid_10 inline alpha">
                    <strong>Nombre del Proyecto:</strong>
                    <asp:Label ID="lblProjectName" Text='<%# Eval("ProjectInfoes") == null ? "" : projectInfo.ProjectName %>' runat="server" />
                    <br />
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
                    <strong>Cliente:</strong>
                    <asp:Label ID="Label2" Text='<%# Eval("ProjectInfoes") == null ? "" : projectInfo.Client  %>' runat="server" />
                    <br />
                    <strong>Dirección:</strong>
                    <asp:Label ID="Label3" Text='<%# Eval("ProjectInfoes") == null ? "" : projectInfo.Address1  %>' runat="server" />
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="Label4" Text='<%# Eval("ProjectInfoes") == null ? "" : projectInfo.Address2  %>' runat="server" />
                    <br />
                    <strong>Ciudad:</strong>
                    <asp:Label ID="Label5" Text='<%# Eval("ProjectInfoes") == null ? "" : projectInfo.City.CityName  %>' runat="server" />
                    <br />
                    <strong>Estado:</strong>
                    <asp:Label ID="Label6" Text='<%# Eval("ProjectInfoes") == null ? "" : projectInfo.State  %>' runat="server" />
                    <br />
                    <strong>Zip Code:</strong>
                    <asp:Label ID="Label7" Text='<%# Eval("ProjectInfoes") == null ? "" : projectInfo.ZipCode  %>' runat="server" />
                    <br />
                    <br />
                </div>
                <div class="grid_9 inline omega">
                    <strong>Descripción:</strong>
                    <%# Eval("ProjectInfoes") == null ? "" : projectInfo.Description %>
                    <br />
                    <strong>Comentarios:</strong>
                    <%# Eval("ProjectInfoes") == null ? "" : projectInfo.Comments %>
                    <br />
                </div>
            </div>
            <br />
            <br />
            <hr />
            <asp:Button CausesValidation="false" ID="btnBack" runat="server" Text="Volver" OnClick="ButtonGoToListPage_Click"
                SkinID="AltButton" />
            <asp:Button ID="Button2" runat="server" Text="Editar" OnClick="ButtonEdit_Click" SkinID="Button" />
        </ItemTemplate>
        <EditItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">Editar Detalles Generales del Proyecto</span> </h1>
            <div class="grid_10 inline alpha">
                Nombre del Proyecto:
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox ID="txtProjectName" Text='<%# Eval("ProjectInfoes") == null ? "" : projectInfo.ProjectName %>' runat="server" CssClass="text" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ErrorMessage="Required"
                    CssClass="validator" ControlToValidate="txtProjectName" Display="Dynamic" />
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
                <label>
                    Dirección:</label><br />
                <asp:TextBox ID="txtAddress1" runat="server" CssClass="text" MaxLength="200" Text='<%# Eval("ProjectInfoes") == null ? "" : projectInfo.Address1 %>'></asp:TextBox>
                <asp:TextBox ID="txtAddress2" runat="server" CssClass="text" MaxLength="200" Text='<%# Eval("ProjectInfoes") == null ? "" : projectInfo.Address2 %>'></asp:TextBox><br />
                <label>
                    Ciudad:</label><br />
                <asp:DropDownList ID="ddlCity" runat="server" AppendDataBoundItems="true"
                    AutoPostBack="false" DataSourceID="odsCityList" DataTextField="CityName"
                    DataValueField="CityID" EnableViewState="true">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsCityList" runat="server" TypeName="Eisk.BusinessLogicLayer.CityBLL"
                    EnableViewState="true" SelectMethod="GetAllCities" />
                <br />
                <label>
                    Estado:</label><br />
                <asp:TextBox ID="txtState" runat="server" CssClass="text" MaxLength="100" Text='<%# Eval("ProjectInfoes") == null ? "" : projectInfo.State %>'></asp:TextBox><br />
                <label>
                    Zip Code:</label><br />
                <asp:TextBox ID="txtZipCode" runat="server" CssClass="text" MaxLength="10" Text='<%# Eval("ProjectInfoes") == null ? "" : Eisk.Web.App_Logic.Helpers.Utility.FormatUSZipCode( projectInfo.ZipCode ) %>'></asp:TextBox>
                <asp:RegularExpressionValidator ID="revZipCode" runat="server"
                    CssClass="validator" ControlToValidate="txtZipCode" Display="Dynamic" ValidationExpression="\d{5}(-\d{4})?"><br />Formato incorrecto del Zip Code</asp:RegularExpressionValidator><br />
                <br />
                <br />
                <br />
                <label>
                    Cliente:</label><br />
                <asp:TextBox ID="txtClient" Text='<%# Eval("ProjectInfoes") == null ? "" : projectInfo.Client %>' runat="server" CssClass="text" MaxLength="100"></asp:TextBox><br />
            </div>
            <div class="grid_9 inline omega">
                <label>
                    Descripción:</label><br />
                <asp:TextBox ID="txtDescription" Text='<%# Eval("ProjectInfoes") == null ? "" : projectInfo.Description %>' runat="server" TextMode="MultiLine"></asp:TextBox>
                <label>
                    Comentarios:</label><br />
                <asp:TextBox ID="txtComments" Text='<%# Eval("ProjectInfoes") == null ? "" : projectInfo.Comments %>' runat="server" TextMode="MultiLine"></asp:TextBox>
                <hr />
                <p>
                    <asp:Button ID="Button1" runat="server" Text="Guardar" OnClick="ButtonSave_Click" SkinID="Button" />
                    <asp:Button ID="Button3" CausesValidation="false" runat="server" Text="Volver" OnClick="ButtonGoToListPage_Click"
                        SkinID="AltButton" />
                </p>
                <em>Los campos requeridos están marcados con un <span class="required-field-indicator">*</span></em>
            </div>
        </EditItemTemplate>
    </asp:FormView>
    <asp:ObjectDataSource ID="odsProject_Details" runat="server" SelectMethod="GetProjectByProjectId2"
        InsertMethod="CreateNewProject" UpdateMethod="UpdateProject" DataObjectTypeName="Eisk.BusinessEntities.Project"
        TypeName="Eisk.BusinessLogicLayer.ProjectBLL" OnSelecting="OdsProject_Details_Selecting"
        OnInserted="OdsProject_Details_Inserted">
        <UpdateParameters>
            <asp:ControlParameter Name="ProjectId" ControlID="formViewProject" />
            <asp:Parameter Name="Project" ConvertEmptyStringToNull="true" />
            <asp:Parameter Name="ProjectInfo" ConvertEmptyStringToNull="true" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="ProjectName" ConvertEmptyStringToNull="true" />
        </InsertParameters>
        <SelectParameters>
            <asp:RouteParameter Name="projectId" RouteKey="project_id" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

