<%@ Page Language="C#" MasterPageFile="~/App_Resources/master.master" AutoEventWireup="false"
    Inherits="Group_Details" Title="Grupo | N@TURA"
    CodeBehind="group_details.aspx.cs" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContentPlaceholder" runat="Server">
    <asp:Literal EnableViewState="False" runat="server" ID="ltlMessage"></asp:Literal>
    <asp:FormView SkinID="FormView" ID="formViewGroup" runat="server" DataSourceID="odsGroup_Details"
        DataKeyNames="GroupId" EnableViewState="true" OnItemUpdating="FormViewGroup_ItemUpdating"
        OnItemUpdated="formViewGroup_ItemUpdated" OnItemInserting="FormViewGroup_ItemInserting"
        OnItemInserted="formViewGroup_ItemInserted" OnDataBound="FormViewGroup_DataBound" OnDataBinding="formViewGroup_DataBinding">
        <ItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">Ver Grupo</span> <span class="grid_3 omega align-right">
                    <asp:LinkButton ID="LinkButton1" OnClientClick="print()" runat="server" Text="Imprimir"
                        CssClass="button small white"></asp:LinkButton></span></h1>
            <div class="grid_10 inline alpha">
                <strong>Nombre de Grupo:</strong>
                <asp:Label ID="Label1" Text='<%# Eval("GroupName") %>' runat="server" />
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
        <EditItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">Editar Grupo</span> </h1>
            <div class="grid_10 inline alpha">
                Nombre de Grupo:
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox ID="txtGroup" Text='<%# Bind("GroupName") %>' runat="server" CssClass="text" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvGroup" runat="server" ErrorMessage="Required"
                    CssClass="validator" ControlToValidate="txtGroup" Display="Dynamic" />
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
                <p>
                    <asp:Button ID="btnSave" runat="server" Text="Guardar" OnClick="ButtonSave_Click" SkinID="Button" />
                    <asp:Button ID="btnBack" CausesValidation="false" runat="server" Text="Volver" OnClick="ButtonGoToListPage_Click"
                        SkinID="AltButton" />
                </p>
                <em>Los campos requeridos están marcados con un <span class="required-field-indicator">*</span></em>
            </div>
        </EditItemTemplate>
    </asp:FormView>
    <asp:ObjectDataSource ID="odsGroup_Details"        runat="server" SelectMethod="GetGroupBygroupId2"
        InsertMethod="CreateNewGroup" UpdateMethod="UpdateGroup" DataObjectTypeName="Eisk.BusinessEntities.Group"
        TypeName="Eisk.BusinessLogicLayer.GroupBLL" OnSelecting="OdsGroup_Details_Selecting"
        OnInserted="OdsGroup_Details_Inserted">
        <UpdateParameters>
            <asp:ControlParameter Name="GroupId" ControlID="formViewGroup" />
            <asp:Parameter Name="Group" ConvertEmptyStringToNull="true" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="GroupName" ConvertEmptyStringToNull="true" />
        </InsertParameters>
        <SelectParameters>
            <asp:RouteParameter Name="GroupId" RouteKey="group_id" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

