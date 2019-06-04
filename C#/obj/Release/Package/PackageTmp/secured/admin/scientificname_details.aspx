<%@ Page Language="C#" MasterPageFile="~/App_Resources/master.master" AutoEventWireup="false"
    Inherits="ScientificName_Details" Title="Nombre Científico | N@TURA" 
    CodeBehind="scientificname_details.aspx.cs" %>

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
    <asp:FormView SkinID="FormView" ID="formViewScientificName" runat="server" DataSourceID="odsScientificName_Details"
        DataKeyNames="ScientificNameId" EnableViewState="true" OnItemUpdating="FormViewScientificName_ItemUpdating"
        OnItemUpdated="formViewScientificName_ItemUpdated" OnItemInserting="FormViewScientificName_ItemInserting"
        OnItemInserted="formViewScientificName_ItemInserted" OnDataBound="FormViewScientificName_DataBound" OnDataBinding="formViewScientificName_DataBinding">
        <ItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">Ver Nombre Científico</span> <span class="grid_3 omega align-right">
                    <asp:LinkButton ID="LinkButton1" OnClientClick="print()" runat="server" Text="Imprimir"
                        CssClass="button small white"></asp:LinkButton></span></h1>
            <div class="grid_10 inline alpha">
                <strong>Nombre Científico:</strong>
                <asp:Label ID="Label1" Text='<%# Eval("ScientificNameDesc") %>' runat="server" />
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
            </div>
        </ItemTemplate>
        <InsertItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">Insertar Nombre Científico</span> </h1>
            <div class="grid_10 inline alpha">
                Nombre Científico:
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox ID="txtScientificName" Text='<%# Bind("ScientificNameDesc") %>' runat="server" CssClass="text" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ErrorMessage="Required"
                    CssClass="validator" ControlToValidate="txtScientificName" Display="Dynamic" />
                <br />
                <asp:RegularExpressionValidator ID="revFirstName" runat="server" ErrorMessage="Expecting a scientific name with the first letter capitalized, the rest lowercase."
                    ValidationExpression="^[A-Z]+[a-z\s]*$"
                    CssClass="validator" ControlToValidate="txtScientificName" Display="Dynamic" />
                <br />
                <hr />
                <p>
                    <asp:Button ID="btnSave" runat="server" Text="Guardar" OnClick="ButtonSave_Click" SkinID="Button" />
                    <asp:Button ID="btnBack" CausesValidation="false" runat="server" Text="Volver" OnClick="ButtonGoToListPage_Click"
                        SkinID="AltButton" />
                </p>
                <em>Los campos requeridos están marcados con un <span class="required-field-indicator">*</span></em>
            </div>
        </InsertItemTemplate>
        <EditItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">Editar Nombre Científico</span> </h1>
            <div class="grid_10 inline alpha">
                Nombre Científico:
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox ID="txtScientificName" Text='<%# Bind("ScientificNameDesc") %>' runat="server" CssClass="text" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ErrorMessage="Required"
                    CssClass="validator" ControlToValidate="txtScientificName" Display="Dynamic" />
                <br />
                <asp:RegularExpressionValidator ID="revFirstName" runat="server" ErrorMessage="Expecting a scientific name with the first letter capitalized, the rest lowercase."
                    ValidationExpression="^[A-Z]+[a-z\s]*$"
                    CssClass="validator" ControlToValidate="txtScientificName" Display="Dynamic" />
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
    <asp:ObjectDataSource ID="odsScientificName_Details"       runat="server" SelectMethod="GetScientificNameByscientificNameId2"
        InsertMethod="CreateNewScientificName" UpdateMethod="UpdateScientificName" DataObjectTypeName="Eisk.BusinessEntities.ScientificName"
        TypeName="Eisk.BusinessLogicLayer.ScientificNameBLL" OnSelecting="OdsScientificName_Details_Selecting"
        OnInserted="OdsScientificName_Details_Inserted">
        <UpdateParameters>
            <asp:ControlParameter Name="ScientificNameId" ControlID="formViewScientificName" />
            <asp:Parameter Name="ScientificName" ConvertEmptyStringToNull="true" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="ScientificNameDesc" ConvertEmptyStringToNull="true" />
        </InsertParameters>
        <SelectParameters>
            <asp:RouteParameter Name="scientificNameId" RouteKey="scientificname_id" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

