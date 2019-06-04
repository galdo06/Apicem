<%@ Page Language="C#" MasterPageFile="~/App_Resources/default.master" AutoEventWireup="true" CodeBehind="organismsinproject.aspx.cs" Inherits="OrganismsInProject_Page"
    Title="Organisms In Project Page | TreeLocation" MetaKeywords="scientific,name"
    MetaDescription="In this page you will be able to view the list of all organisms. Click on the appropriate buttons to view, insert or update a organism." %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <script type="text/javascript">
        var fieldName = 'chkOrganismSelector';

        function selectall(frmId, check) {
            var frm = document.getElementById(frmId);

            var i = frm.all[0].rows.length;
            var name = new Array();
            var value = new Array();
            var j = 0;
            for (var k = 1; k < i; k++) {
                var checkbox = frm.all[0].rows[k].children[0].children[0].children[0].children[0];
                if (checkbox) {
                    if (checkbox.id == fieldName) {
                        if (checkbox.checked == true) {
                            value[j] = checkbox.value;
                            j++;
                        }
                    }
                }
            }
            checkSelect(frmId);
        }

        function selectCheck(frmId, check) {
            var frm = document.getElementById(frmId);
            var i = frm.all[0].rows.length;

            for (var k = 1; k < i; k++) {
                if (frm.all[0].rows[k].children[0].children[0].children[0].children[0].id == fieldName) {
                    frm.all[0].rows[k].children[0].children[0].children[0].children[0].checked = check;
                }
            }
        }

        function selectallMe(frmId, id) {
            var frmChkAll = document.getElementById(id);

            if (frmChkAll.checked == true) {
                selectCheck(frmId, true);
            }
            else {
                selectCheck(frmId, false);
            }
        }

        function checkSelect(frmId) {
            var frm = document.getElementById(frmId);

            var i = frm.all[0].rows.length;
            var berror = true;
            for (var k = 1; k < i; k++) {
                var checkbox = frm.all[0].rows[k].children[0].children[0].children[0].children[0];
                if (checkbox.id == fieldName) {
                    if (checkbox.checked == false) {
                        berror = false;
                        break;
                    }
                }
            }

            var chkSelectAll = document.getElementById('chkSelectAll');
            if (berror == false) {
                chkSelectAll.checked = false;
            }
            else {
                chkSelectAll.checked = true;
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceholder" runat="server">
    <asp:HiddenField ID="hfProjectID" runat="server" />
    <h1 class="title-regular clearfix">
        <span class="grid_15 alpha">Organisms In Project</span>
        <span class="grid_3 omega align-right">
            <asp:LinkButton ID="btnExportData" runat="server" Text="Export Project"
                CssClass="button  big  white" Height="25px" OnClick="btnExportData_Click"></asp:LinkButton>
        </span>
    </h1>
    <p>
        List of organisms in project:&nbsp;
      <strong>
          <asp:Label ID="lblProjectName" runat="server"></asp:Label>
      </strong>
    </p>
    <asp:ScriptManager ID="scriptManager" runat="server" />
    <asp:UpdatePanel ID="upd" runat="server">
        <ContentTemplate>
            <asp:Literal EnableViewState="false" runat="server" ID="ltlMessage"></asp:Literal>
            <div>
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
                    DataKeyNames="organismId" PageSize="15">
                    <Columns>
                        <asp:TemplateField ControlStyle-Width="3%" HeaderStyle-Width="3%">

                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlCheckBox" Width="100%" Height="100%">
                                    <asp:CheckBox runat="server" ID="chkOrganismSelector" name="chkName" onClick="selectall('gridViewOrganisms','chkSelectAll')" />
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:CheckBox runat="server" ID="chkSelectAll" ClientIDMode="Static" name="allCheck" onclick="selectallMe('gridViewOrganisms','chkSelectAll')" /><%--SelectAll('gridViewOrganisms','chkSelectAll')--%>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ControlStyle-Width="3%" HeaderStyle-Width="3%">
                            <ItemTemplate>
                                <%# new Eisk.BusinessLogicLayer.OrganismBLL().GetTotalCountForAllOrganismsInProject(this.txtFilter.Text,null,0,1,Convert.ToInt32(this.ddlOrganismType.SelectedValue),Convert.ToInt32(Page.RouteData.Values["project_id"] as string)) -Container.DataItemIndex   %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ControlStyle-Width="33%" HeaderStyle-Width="33%" DataField="CommonName" HeaderText="Common Name" SortExpression="organism" />
                        <asp:BoundField ControlStyle-Width="33%" HeaderStyle-Width="33%" DataField="ScientificName" HeaderText="Scientific Name" SortExpression="organism" />
                        <asp:TemplateField HeaderStyle-Width="18%">
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
                    DeleteMethod="DeleteProject_Organisms" EnablePaging="True" InsertMethod="CreateNewOrganism" OldValuesParameterFormatString="original_{0}" 
                    SelectCountMethod="GetTotalCountForAllOrganismsInProject" SelectMethod="GetOrganismInProjectByFilter" SortParameterName="orderby"
                    TypeName="Eisk.BusinessLogicLayer.OrganismBLL" UpdateMethod="organismId" OnSelecting="odsOrganismListing_Selecting">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtFilter" Name="organism" PropertyName="Text" Type="String" />
                        <asp:ControlParameter ControlID="ddlOrganismType" Name="organismTypeID" PropertyName="Text"
                            Type="Int32" />
                        <asp:Parameter Name="orderBy" DefaultValue="EditedDate" Type="String" />
                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                        <asp:Parameter Name="maximumRows" Type="Int32" />
                        <asp:Parameter Name="projectID" Type="Int32" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="projectID" Type="Int32" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
            </div>
            <div class="clearfix align-right">
                <asp:LinkButton SkinID="LinkButton" runat="server" ID="lbtAddNewOrganism" Text="Add Organism To Project"
                    OnClick="ButtonAddNewOrganism_Click" />
                <asp:LinkButton SkinID="AltLinkButton" OnClientClick="return confirm('Are you sure you want to delete all items?');"
                    runat="server" ID="buttonDeleteSelected" Text="Remove Selected Organism From Project" OnClick="ButtonDeleteSelected_Click" />
                <asp:Button CausesValidation="false" ID="btnBack" runat="server" Text="Back" OnClick="ButtonGoToListPage_Click"
                    SkinID="AltButton" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
