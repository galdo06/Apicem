<%@ Page Language="C#" MasterPageFile="~/App_Resources/master.master" AutoEventWireup="false"
    Inherits="CommonName_Details" Title="Nombre Común | N@TURA"
    CodeBehind="commonname_details.aspx.cs" %>

<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContentPlaceholder" runat="Server">
    <asp:ScriptManager ID="ScriptManager2" runat="server">
        <Services>
            <asp:ServiceReference Path="CommonName_WS.asmx" />
        </Services>
    </asp:ScriptManager>
    
    
  <%--  <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
          <Services>
            <asp:ServiceReference Path="../CommonName_WS.asmx" />
        </Services>
 </ajaxToolkit:ToolkitScriptManager>--%>
    <asp:Literal EnableViewState="False" runat="server" ID="ltlMessage"></asp:Literal>
    <asp:FormView SkinID="FormView" ID="formViewCommonName" runat="server" DataSourceID="odsOrganism_Details"
        DataKeyNames="OrganismId" EnableViewState="true" OnItemUpdating="FormViewCommonName_ItemUpdating"
        OnItemUpdated="formViewCommonName_ItemUpdated" OnItemInserting="FormViewCommonName_ItemInserting"
        OnItemInserted="formViewCommonName_ItemInserted" OnDataBound="FormViewCommonName_DataBound" OnDataBinding="formViewCommonName_DataBinding">
        <ItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">Ver Nombre Común de Árbol</span> <span class="grid_3 omega align-right">
                    <asp:LinkButton ID="LinkButton1" OnClientClick="print()" runat="server" Text="Imprimir"
                        CssClass="button small white"></asp:LinkButton></span></h1>
            <div class="grid_10 inline alpha">
                <strong>Nombre Común:</strong>
                <%# Eval("CommonName.CommonNameDesc") %><br />
                <strong>Nombre Científico:</strong>
                <%# Eval("ScientificName.ScientificNameDesc") %><br />
                <hr />
                <asp:Button CausesValidation="false" ID="btnBack" runat="server" Text="Volver" OnClick="ButtonGoToListPage_Click"
                    SkinID="AltButton" />
                <asp:Button ID="Button2" runat="server" Text="Editar" OnClick="ButtonEdit_Click" SkinID="Button" />
            </div>
        </ItemTemplate>
        <EditItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">Editar Nombre Común de Árbol</span>
            </h1>
            <asp:Literal EnableViewState="False" runat="server" ID="ltlMessage"></asp:Literal>
            <div class="grid_10 inline alpha" style="width: 450px;">
                Nombre Común:
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox ID="txtCommonName" Text='<%# Page.RouteData.Values["organism_id"] as string != "0" ? organism.CommonName.CommonNameDesc : "" %>' runat="server" CssClass="text" MaxLength="100"></asp:TextBox>
                <ajax:AutoCompleteExtender
                    runat="server"
                    ID="aceCommonName"
                    TargetControlID="txtCommonName"
                    ServicePath="AutoComplete.asmx"
                    ServiceMethod="GetCommonNameByFilter"
                    MinimumPrefixLength="1"
                    CompletionInterval="1000"
                      
                    CompletionSetCount="20" />
                <asp:RequiredFieldValidator ID="rfvCommonName" runat="server"
                    CssClass="validator" ControlToValidate="txtCommonName" Display="Dynamic"><br />Required</asp:RequiredFieldValidator>
                <br />
                Nombre Científico:
                <span class="required-field-indicator">*</span><br />
                <asp:TextBox ID="txtScientificName" Text='<%# Page.RouteData.Values["organism_id"] as string != "0" ? organism.ScientificName.ScientificNameDesc : "" %>' runat="server" CssClass="text" Enabled="false" MaxLength="100"></asp:TextBox>
                <asp:Button ID="btnSelect" runat="server" Text="Seleccionar" OnClick="ButtonSelect_Click" SkinID="Button" CausesValidation="false" />
                <ajax:AutoCompleteExtender
                    runat="server"
                    ID="aceScientificName"
                    TargetControlID="txtScientificName"
                    ServicePath="AutoComplete.asmx"
                    ServiceMethod="GetScientificNameByFilter"
                    MinimumPrefixLength="1"
                    CompletionInterval="1000"
                      
                    CompletionSetCount="20" />
                <asp:RequiredFieldValidator ID="rfvScientificName" runat="server" InitialValue="<--SELECCIONAR-->"
                    CssClass="validator" ControlToValidate="txtScientificName" Display="Dynamic"><br />Required</asp:RequiredFieldValidator>
                <br />
                <hr width="600px" />
                <p>
                    <asp:Button ID="btnSave" runat="server" Text="Guardar" OnClick="ButtonSave_Click" SkinID="Button" />
                    <asp:Button ID="btnBack" CausesValidation="false" runat="server" Text="Volver" OnClick="ButtonGoToListPage_Click"
                        SkinID="AltButton" />
                </p>
                <em>Los campos requeridos están marcados con un <span class="required-field-indicator">*</span></em>
            </div>
        </EditItemTemplate>
    </asp:FormView>
    <asp:ObjectDataSource ID="odsOrganism_Details"      runat="server" SelectMethod="GetOrganismByOrganismId2"
        InsertMethod="CreateNewOrganism" UpdateMethod="UpdateOrganism" DataObjectTypeName="Eisk.BusinessEntities.Organism"
        TypeName="Eisk.BusinessLogicLayer.OrganismBLL" OnSelecting="OdsOrganism_Details_Selecting"
        OnInserted="OdsOrganism_Details_Inserted">
        <UpdateParameters>
            <asp:ControlParameter Name="OrganismId" ControlID="formViewOrganism" />
            <asp:Parameter Name="Organism" ConvertEmptyStringToNull="true" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="Organism" ConvertEmptyStringToNull="true" />
        </InsertParameters>
        <SelectParameters>
            <asp:RouteParameter Name="organismId" RouteKey="organism_id" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

