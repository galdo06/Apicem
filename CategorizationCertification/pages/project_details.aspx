<%@ Page Language="C#" MasterPageFile="~/App_Resources/default.master" AutoEventWireup="false"
    Inherits="Project_Details" Title="Project Details Page | EISK" MetaKeywords="ASP.NET, Project,Details"
    MetaDescription="In this page, you will be able to see details of an employee."
    CodeBehind="project_details.aspx.cs" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContentPlaceholder" runat="Server">
    <asp:Literal EnableViewState="False" runat="server" ID="ltlMessage"></asp:Literal>
    <asp:FormView SkinID="FormView" ID="formViewProject" runat="server" DataSourceID="odsProject_Details"
        DataKeyNames="ProjectId" EnableViewState="true" OnItemUpdating="FormViewProject_ItemUpdating"
        OnItemUpdated="formViewProject_ItemUpdated" OnItemInserting="FormViewProject_ItemInserting"
        OnItemInserted="formViewProject_ItemInserted" OnDataBound="FormViewProject_DataBound">
        <EmptyDataTemplate>
            <h1 class="title-regular clearfix">No Project Found</h1>
            <div class="notice">
                Sorry, no Project available with this ID.
            </div>
            <asp:Button ID="btnBack" CausesValidation="false" runat="server" Text="Back to listing page"
                OnClick="ButtonGoToListPage_Click" SkinID="Button" />
        </EmptyDataTemplate>
        <ItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">View Project</span> <span class="grid_3 omega align-right">
                    <asp:LinkButton ID="LinkButton1" OnClientClick="print()" runat="server" Text="Print Info"
                        CssClass="button small white"></asp:LinkButton></span></h1>
            <div class="grid_10 inline alpha">
                <strong>Project:</strong>
                <%# Eval("ProjectName") %><br />
                <hr />
                <asp:Button CausesValidation="false" ID="btnBack" runat="server" Text="Back" OnClick="ButtonGoToListPage_Click"
                    SkinID="AltButton" />
                <asp:Button ID="Button2" runat="server" Text="Edit" OnClick="ButtonEdit_Click" SkinID="Button" />
            </div>
        </ItemTemplate>
        <InsertItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">Insert Project</span> </h1>
            <div class="grid_10 inline alpha">
                Project:
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox ID="txtProject" Text='<%# Bind("ProjectName") %>' runat="server" CssClass="text" Width="585px" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ErrorMessage="Required"
                    CssClass="validator" ControlToValidate="txtProject" Display="Dynamic" />
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
                <span class="grid_15 alpha">Edit Project</span> </h1>
            <div class="grid_10 inline alpha">
                Project:
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox ID="txtProject" Text='<%# Bind("ProjectName") %>' runat="server" CssClass="text" Width="585px" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ErrorMessage="Required"
                    CssClass="validator" ControlToValidate="txtProject" Display="Dynamic" />
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
    <asp:ObjectDataSource ID="odsProject_Details" runat="server" SelectMethod="GetProjectByProjectId2"
        InsertMethod="CreateNewProject" UpdateMethod="UpdateProject" DataObjectTypeName="Eisk.BusinessEntities.Project"
        TypeName="Eisk.BusinessLogicLayer.ProjectBLL" OnSelecting="OdsProject_Details_Selecting"
        OnInserted="OdsProject_Details_Inserted">
        <UpdateParameters>
            <asp:ControlParameter Name="ProjectId" ControlID="formViewProject" />
            <asp:Parameter Name="Project" ConvertEmptyStringToNull="true" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="ProjectName" ConvertEmptyStringToNull="true" />
        </InsertParameters>
        <SelectParameters>
            <asp:RouteParameter Name="projectId" RouteKey="project_id" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

