<%@ Page Language="C#" MasterPageFile="~/App_Resources/default.master" AutoEventWireup="false"
    Inherits="OrganismType_Details" Title="OrganismType Details Page | EISK" MetaKeywords="ASP.NET, OrganismType,Details"
    MetaDescription="In this page, you will be able to see details of an employee."
    CodeBehind="organismtype_details.aspx.cs" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContentPlaceholder" runat="Server">
    <asp:Literal EnableViewState="False" runat="server" ID="ltlMessage"></asp:Literal>
    <asp:FormView SkinID="FormView" ID="formViewOrganismType" runat="server" DataSourceID="odsOrganismType_Details"
        DataKeyNames="OrganismTypeId" EnableViewState="true" OnItemUpdating="FormViewOrganismType_ItemUpdating"
        OnItemUpdated="formViewOrganismType_ItemUpdated" OnItemInserting="FormViewOrganismType_ItemInserting"
        OnItemInserted="formViewOrganismType_ItemInserted" OnDataBound="FormViewOrganismType_DataBound">
        <EmptyDataTemplate>
            <h1 class="title-regular clearfix">No Organism Type Found</h1>
            <div class="notice">
                Sorry, no Organism Type available with this ID.
            </div>
            <asp:Button ID="btnBack" CausesValidation="false" runat="server" Text="Back to listing page"
                OnClick="ButtonGoToListPage_Click" SkinID="Button" />
        </EmptyDataTemplate>
        <ItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">View Organism Type</span> <span class="grid_3 omega align-right">
                    <asp:LinkButton ID="LinkButton1" OnClientClick="print()" runat="server" Text="Print Info"
                        CssClass="button small white"></asp:LinkButton></span></h1>
            <div class="grid_10 inline alpha">
                <strong>Organism Type:</strong>
                <%# Eval("OrganismTypeName") %><br />
                <hr />
                <asp:Button CausesValidation="false" ID="btnBack" runat="server" Text="Back" OnClick="ButtonGoToListPage_Click"
                    SkinID="AltButton" />
                <asp:Button ID="Button2" runat="server" Text="Edit" OnClick="ButtonEdit_Click" SkinID="Button" />
            </div>
        </ItemTemplate>
        <InsertItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">Insert Organism Type</span> </h1>
            <div class="grid_10 inline alpha">
                Organism Type:
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox ID="txtOrganismType" Text='<%# Bind("OrganismTypeName") %>' runat="server" CssClass="text" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ErrorMessage="Required"
                    CssClass="validator" ControlToValidate="txtOrganismType" Display="Dynamic" />
                <br />
                <hr />
                <p>
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="ButtonSave_Click" SkinID="Button" />
                    <asp:Button ID="btnBack" CausesValidation="false" runat="server" Text="Back" OnClick="ButtonGoToListPage_Click"
                        SkinID="AltButton" />
                </p>
                <em>Required fields are marked with <span class="required-field-indicator">*</span></em>
            </div>
        </InsertItemTemplate>
        <EditItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">Edit Organism Type</span> </h1>
            <div class="grid_10 inline alpha">
                Organism Type:
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox ID="txtOrganismType" Text='<%# Bind("OrganismTypeName") %>' runat="server" CssClass="text"  MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ErrorMessage="Required"
                    CssClass="validator" ControlToValidate="txtOrganismType" Display="Dynamic" />
                <br />
                <hr />
                <p>
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="ButtonSave_Click" SkinID="Button" />
                    <asp:Button ID="btnBack" CausesValidation="false" runat="server" Text="Back" OnClick="ButtonGoToListPage_Click"
                        SkinID="AltButton" />
                </p>
                <em>Required fields are marked with <span class="required-field-indicator">*</span></em>
            </div>
        </EditItemTemplate>
    </asp:FormView>
    <asp:ObjectDataSource ID="odsOrganismType_Details" runat="server" SelectMethod="GetOrganismTyeByOrganismTypeId2"
        InsertMethod="CreateNewOrganismType" UpdateMethod="UpdateOrganismType" DataObjectTypeName="Eisk.BusinessEntities.OrganismType"
        TypeName="Eisk.BusinessLogicLayer.OrganismTypeBLL" OnSelecting="OdsOrganismType_Details_Selecting"
        OnInserted="OdsOrganismType_Details_Inserted">
        <UpdateParameters>
            <asp:ControlParameter Name="OrganismTypeId" ControlID="formViewOrganismType" />
            <asp:Parameter Name="OrganismType" ConvertEmptyStringToNull="true" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="OrganismTypeName" ConvertEmptyStringToNull="true" />
        </InsertParameters>
        <SelectParameters>
            <asp:RouteParameter Name="organismTypeId" RouteKey="organismtype2_id" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

