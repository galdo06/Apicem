<%@ Page Language="C#" MasterPageFile="~/App_Resources/default.master" AutoEventWireup="false"
    Inherits="Organism_Details" Title="Organism Details Page | EISK" MetaKeywords="ASP.NET, Organism,Details"
    MetaDescription="In this page, you will be able to see details of an employee."
    CodeBehind="organism_details.aspx.cs" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContentPlaceholder" runat="Server">
    <asp:Literal EnableViewState="False" runat="server" ID="ltlMessage"></asp:Literal>
    <asp:FormView SkinID="FormView" ID="formViewOrganism" runat="server" DataSourceID="odsOrganism_Details"
        DataKeyNames="OrganismId" EnableViewState="true" OnItemUpdating="FormViewOrganism_ItemUpdating"
        OnItemUpdated="formViewOrganism_ItemUpdated" OnItemInserting="FormViewOrganism_ItemInserting" 
        OnItemInserted="formViewOrganism_ItemInserted" OnDataBound="FormViewOrganism_DataBound"> 
        <EmptyDataTemplate>
            <h1 class="title-regular clearfix">No Organism Found</h1>
            <div class="notice">
                Sorry, no Organism available with this ID.
            </div>
            <asp:Button ID="btnBack" CausesValidation="false" runat="server" Text="Back to listing page"
                OnClick="ButtonGoToListPage_Click" SkinID="Button" />
        </EmptyDataTemplate>
        <ItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">View Organism</span> <span class="grid_3 omega align-right">
                    <asp:LinkButton ID="LinkButton1" OnClientClick="print()" runat="server" Text="Print Info"
                        CssClass="button small white"></asp:LinkButton></span></h1>
            <div class="grid_10 inline alpha">
                <strong>Organism Type:</strong>
                <%# Eval("OrganismType.OrganismTypeName") %><br />
                <strong>Organism Common Name:</strong>
                <%# Eval("CommonName") %><br />
                <strong>Organism Scientific Name:</strong>
                <%# Eval("ScientificName") %><br />
                <hr />
                <asp:Button CausesValidation="false" ID="btnBack" runat="server" Text="Back" OnClick="ButtonGoToListPage_Click"
                    SkinID="AltButton" />
                <asp:Button ID="Button2" runat="server" Text="Edit" OnClick="ButtonEdit_Click" SkinID="Button" />
            </div>
        </ItemTemplate>
        <InsertItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">Create New Organism</span>
            </h1>
            <div class="grid_10 alpha">
                <label>
                   Organism Type:</label><br />
                <asp:DropDownList ID="ddlOrganismType" runat="server" AppendDataBoundItems="true"
                    AutoPostBack="false" DataSourceID="odsOrganismTypeList" DataTextField="OrganismTypeName"
                    DataValueField="OrganismTypeID" EnableViewState="true">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsOrganismTypeList" runat="server" TypeName="Eisk.BusinessLogicLayer.OrganismTypeBLL"
                    EnableViewState="true" SelectMethod="GetAllOrganismTypesList" />
            </div>
            <div class="grid_10 inline alpha">
                Common Name:
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox ID="txtCommonName" Text='<%# Bind("CommonName") %>' runat="server" CssClass="text" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCommonName" runat="server"
                    CssClass="validator" ControlToValidate="txtCommonName" Display="Dynamic">&nbsp;Required</asp:RequiredFieldValidator>
                <br />
                Scientific Name:
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox ID="txtScientificName" Text='<%# Bind("ScientificName") %>' runat="server" CssClass="text" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvScientificName" runat="server"
                    CssClass="validator" ControlToValidate="txtScientificName" Display="Dynamic">&nbsp;Required</asp:RequiredFieldValidator>
                <br />
                <hr width="600px" />
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
                <span class="grid_15 alpha">Edit Organism</span>
            </h1>
            <asp:Literal EnableViewState="False" runat="server" ID="ltlMessage"></asp:Literal>
            <div class="grid_10 alpha">
                <label>
                    Organism Type:</label><br />
                <asp:DropDownList ID="ddlOrganismType" runat="server" AppendDataBoundItems="true"
                    AutoPostBack="false" DataSourceID="odsOrganismTypeList" DataTextField="OrganismTypeName" 
                    DataValueField="OrganismTypeID" EnableViewState="true">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsOrganismTypeList" runat="server" TypeName="Eisk.BusinessLogicLayer.OrganismTypeBLL"
                    EnableViewState="true" SelectMethod="GetAllOrganismTypesList" />
            </div>
            <div class="grid_10 inline alpha">
                Common Name:
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox ID="txtCommonName" Text='<%# Bind("CommonName") %>' runat="server" CssClass="text" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCommonName" runat="server"
                    CssClass="validator" ControlToValidate="txtCommonName" Display="Dynamic"><br />Required</asp:RequiredFieldValidator>
                <br />
                Scientific Name:
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox ID="txtScientificName" Text='<%# Bind("ScientificName") %>' runat="server" CssClass="text" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvScientificName" runat="server"
                    CssClass="validator" ControlToValidate="txtScientificName" Display="Dynamic"><br />Required</asp:RequiredFieldValidator>
                <br />
                <hr width="600px" />
                <p>
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="ButtonSave_Click" SkinID="Button" />
                    <asp:Button ID="btnBack" CausesValidation="false" runat="server" Text="Back" OnClick="ButtonGoToListPage_Click"
                        SkinID="AltButton" />
                </p>
                <em>Required fields are marked with <span class="required-field-indicator">*</span></em>
            </div>
        </EditItemTemplate>
    </asp:FormView>
    <asp:ObjectDataSource ID="odsOrganism_Details" runat="server" SelectMethod="GetOrganismByOrganismId2"
        InsertMethod="CreateNewOrganism" UpdateMethod="UpdateOrganism" DataObjectTypeName="Eisk.BusinessEntities.Organism"
        TypeName="Eisk.BusinessLogicLayer.OrganismBLL" OnSelecting="OdsOrganism_Details_Selecting"
        OnInserted="OdsOrganism_Details_Inserted">
        <UpdateParameters>
            <asp:ControlParameter Name="OrganismId" ControlID="formViewOrganism" />
            <asp:Parameter Name="Organism" ConvertEmptyStringToNull="true" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="CommonName" ConvertEmptyStringToNull="true" />
        </InsertParameters>
        <SelectParameters>
            <asp:RouteParameter Name="organismId" RouteKey="organism_id" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

