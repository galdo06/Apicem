<%@ Page Language="C#" MasterPageFile="~/App_Resources/master.master" AutoEventWireup="true" CodeBehind="scientificnameselect.aspx.cs" Inherits="ScientificNameSelect_Page"
    Title="Nombre Científico | N@TURA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceholder" runat="server">
    <asp:HiddenField ID="hfProjectID" runat="server" />
    <h1 class="title-regular">Seleccionar Nombre Científico</h1>
    <p>
        Seleccionar el Nombre Científico que deseas atar a tu Nombre Común:&nbsp;
      <strong>
          <asp:Label ID="lblProjectName" runat="server"></asp:Label>
      </strong>
    </p>
    <asp:ScriptManager ID="scriptManager" runat="server" />
    <asp:UpdatePanel ID="upd" runat="server">
        <ContentTemplate>
            <asp:Literal EnableViewState="false" runat="server" ID="ltlMessage"></asp:Literal>
            <asp:Panel ID="pnlCommonName" runat="server">
                <strong>Nombre Común Actual:</strong>
                <asp:Label runat="server" ID="lblCommonName" /><br />
            </asp:Panel>
            <asp:Panel ID="pnlScientificName" runat="server">
                <strong>Nombre Científico Actual:</strong>
                <asp:Label runat="server" ID="lblScientificName" /><br />
            </asp:Panel>
            <div class="clearfix align-right">
                <asp:TextBox ID="txtFilter" runat="server" CssClass="text" AutoPostBack="true" />
                <asp:LinkButton ID="lbFilter" runat="server" SkinID="LinkButton" Text="Filtrar" />
            </div>
            <div class="grid-viewer grid_19 clearfix">
                <asp:GridView ID="gridViewScientificNames" runat="server" DataSourceID="odsScientificNameListing" SkinID="GridView" ClientIDMode="Static" AllowPaging="True"
                    DataKeyNames="scientificNameId" PageSize="15" OnRowCommand="gridViewScientificNames_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="7%" ControlStyle-Width="100%">
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlEdit" Width="100%" Height="100%" HorizontalAlign="Center">
                                    <asp:LinkButton runat="server" CommandName="lbSelectScientificName" CommandArgument='<%# Eval("ScientificNameId")%>'
                                        CssClass="GridIcon ico-edit"></asp:LinkButton>
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderTemplate>
                                Seleccionar
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ControlStyle-Width="93%" HeaderStyle-Width="93%" DataField="ScientificNameDesc" HeaderText="Nombre Científico" SortExpression="ScientificNameDesc" />
                    </Columns>
                </asp:GridView>
            </div>
            <div class="clearfix">
                <asp:ObjectDataSource ID="odsScientificNameListing"      runat="server" DataObjectTypeName="Eisk.BusinessEntities.ScientificName"
                    DeleteMethod="DeleteScientificName" EnablePaging="True" InsertMethod="CreateNewScientificName" OldValuesParameterFormatString="original_{0}"
                    SelectCountMethod="GetTotalCountForAllScientificNames" SelectMethod="GetScientificNameByFilter" SortParameterName="orderby"
                    TypeName="Eisk.BusinessLogicLayer.ScientificNameBLL" UpdateMethod="scientificNameId" OnSelecting="odsScientificNameListing_Selecting">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtFilter" Name="scientificName" PropertyName="Text"
                            Type="String" />
                        <asp:Parameter Name="orderBy" Type="String" />
                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                        <asp:Parameter Name="maximumRows" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
            <div class="clearfix align-right">
                <asp:Button CausesValidation="false" ID="btnBack" runat="server" Text="Volver" OnClick="ButtonGoToListPage_Click"
                    SkinID="AltButton" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
