<%@ Page Title="" Language="C#" MasterPageFile="~/App_Resources/master.master" AutoEventWireup="true" CodeBehind="companies.aspx.cs" Inherits="Eisk.Web.Company" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceholder" runat="server">
    <p>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="CompanyId" DataSourceID="ObjectDataSource1" SkinID="GridView">
            <Columns>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                <asp:BoundField DataField="CompanyId" HeaderText="CompanyId" SortExpression="CompanyId" />
                <asp:BoundField DataField="CompanyName" HeaderText="CompanyName" SortExpression="CompanyName" />
                <asp:BoundField DataField="ContactPhone" HeaderText="ContactPhone" SortExpression="ContactPhone" />
                <asp:BoundField DataField="ContactEmail" HeaderText="ContactEmail" SortExpression="ContactEmail" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="ObjectDataSource1"       runat="server" DataObjectTypeName="Eisk.BusinessEntities.Company" DeleteMethod="DeleteCompany"
          EnablePaging="True" OldValuesParameterFormatString="original_{0}" SelectCountMethod="GetTotalCountForAllCompanies"
            SelectMethod="GetAllCompaniesPaged" SortParameterName="orderby" TypeName="Eisk.BusinessLogicLayer.CompanyBLL" UpdateMethod="UpdateCompany">
            <SelectParameters>
                <asp:Parameter Name="orderBy" Type="String" />
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </p>
</asp:Content>
