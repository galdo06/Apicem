<%@ Page Language="C#" MasterPageFile="~/App_Resources/default.master" AutoEventWireup="true" CodeBehind="organismselect.aspx.cs" Inherits="OrganismSelect_Page"
    Title="Organisms Page | TreeLocation" MetaKeywords="scientific,name"
    MetaDescription="In this page you will be able to view the list of all organisms. Click on the appropriate buttons to view, insert or update a organism." %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceholder" runat="server">
    <asp:HiddenField ID="hfProjectID" runat="server" />
    <h1 class="title-regular">Select Organisms In Project</h1>
    <p>
        Select the organisms that you want to add to your project:&nbsp;
      <strong>
          <asp:Label ID="lblProjectName" runat="server"></asp:Label>
      </strong>
    </p>
    <asp:ScriptManager ID="scriptManager" runat="server" />
    <asp:UpdatePanel ID="upd" runat="server">
        <ContentTemplate>
            <asp:Literal EnableViewState="false" runat="server" ID="ltlMessage"></asp:Literal>
            <div class="">
                <div class="clearfix align-left" style="float: left">
                    <strong>Organism Type:&nbsp;</strong>
                    <asp:DropDownList ID="ddlOrganismType" runat="server" AppendDataBoundItems="true"
                        AutoPostBack="true" DataSourceID="odsOrganismTypeList" DataTextField="OrganismTypeName"
                        DataValueField="OrganismTypeID" EnableViewState="true" OnSelectedIndexChanged="ddlOrganismType_SelectedIndexChanged">
                        <asp:ListItem Text="ALL" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ObjectDataSource ID="odsOrganismTypeList" runat="server" TypeName="Eisk.BusinessLogicLayer.OrganismTypeBLL"
                        EnableViewState="true" SelectMethod="GetAllOrganismTypesList" />
                </div>
                <div class="clearfix align-right" style="float: right">
                    <asp:TextBox ID="txtFilter" runat="server" CssClass="text" AutoPostBack="true" />
                    <asp:LinkButton ID="lbFilter" runat="server" SkinID="LinkButton" Text="Filter" />
                </div>
            </div>
            <div class="grid-viewer grid_19 clearfix">
                <asp:GridView ID="gridViewOrganisms" runat="server" DataSourceID="odsOrganismListing" SkinID="GridView" ClientIDMode="Static" AllowPaging="True"
                    DataKeyNames="organismId" PageSize="15" OnRowCommand="gridViewOrganisms_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="7%">
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlEdit" Width="100%" Height="100%" HorizontalAlign="Center">
                                    <asp:LinkButton runat="server" CommandName="lbSelectOrganism" CommandArgument='<%# Eval("OrganismId")%>'
                                        CssClass="GridIcon ico-edit"></asp:LinkButton>
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderTemplate>
                                Select
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ControlStyle-Width="34%" HeaderStyle-Width="34%" DataField="CommonName" HeaderText="Common Name" SortExpression="organism" />
                        <asp:BoundField ControlStyle-Width="34%" HeaderStyle-Width="34%" DataField="ScientificName" HeaderText="Scientific Name" SortExpression="organism" />
                        <asp:TemplateField HeaderStyle-Width="17%">
                            <ItemTemplate>
                                <%# GetOrganismTypeName(Convert.ToInt32(Eval("OrganismTypeId"))) %>
                            </ItemTemplate>
                            <HeaderTemplate>
                                Type
                            </HeaderTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
            <div class="clearfix">
                <asp:ObjectDataSource ID="odsOrganismListing" runat="server" DataObjectTypeName="Eisk.BusinessEntities.Organism"
                    DeleteMethod="DeleteOrganism" EnablePaging="True" InsertMethod="CreateNewOrganism" OldValuesParameterFormatString="original_{0}"
                    SelectCountMethod="GetTotalCountForAllOrganismsSelect" SelectMethod="GetOrganismSelectByFilter" SortParameterName="orderby"
                    TypeName="Eisk.BusinessLogicLayer.OrganismBLL" UpdateMethod="organismId" OnSelecting="odsOrganismListing_Selecting">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtFilter" Name="organism" PropertyName="Text"
                            Type="String" />
                        <asp:ControlParameter ControlID="ddlOrganismType" Name="organismTypeID" PropertyName="Text"
                            Type="Int32" />
                        <asp:Parameter Name="orderBy" Type="String" />
                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                        <asp:Parameter Name="maximumRows" Type="Int32" />
                        <asp:Parameter Name="projectID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
            <div class="clearfix align-right">
                <asp:Button CausesValidation="false" ID="btnBack" runat="server" Text="Back" OnClick="ButtonGoToListPage_Click"
                    SkinID="AltButton" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
