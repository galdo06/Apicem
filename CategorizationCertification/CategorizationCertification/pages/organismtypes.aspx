<%@ Page Language="C#" MasterPageFile="~/App_Resources/default.master" AutoEventWireup="true" CodeBehind="organismtypes.aspx.cs" Inherits="OrganismType_Page"
    Title="Organism Types Page | TreeLocation" MetaKeywords="organism,name"
    MetaDescription="In this page you will be able to view the list of all organism types. Click on the appropriate buttons to view, insert or update a organism type." %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <script type="text/javascript">
        //function SelectAll(frmId, id) {
        //    var frm = document.getElementById(frmId);
        //    for (i = 1; i < frm.rows.length; i++) {
        //        var checkbox = frm.rows[i].cells[0].children[0].childNodes[1];
        //        if (checkbox != null)
        //            checkbox.checked = document.getElementById(id).checked;
        //    }
        //};

        var fieldName = 'chkOrganismTypeSelector';

        function selectall(frmId, check) {
            var frm = document.getElementById(frmId);

            var i = frm.all[0].rows.length;
            //var e = document.frm.elements;
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
            //selectall();
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
    <h1 class="title-regular">Organism Types</h1>
    <p>
        In this page you will be able to view the list of all organism types. Click on the appropriate
        buttons to view, insert or update a organism type.
    </p>
    <asp:ScriptManager ID="scriptManager" runat="server" />
    <asp:UpdatePanel ID="upd" runat="server">
        <ContentTemplate>
            <asp:Literal EnableViewState="false" runat="server" ID="ltlMessage"></asp:Literal>
            <div class="clearfix align-right">
                <asp:TextBox ID="txtFilter" runat="server" CssClass="text" AutoPostBack="true" />
                <asp:LinkButton ID="lbFilter" runat="server" SkinID="LinkButton" Text="Filter" />
            </div>
            <div class="grid-viewer grid_19 clearfix">
                <asp:GridView ID="gridViewOrganismTypes" runat="server" DataSourceID="odsOrganismTypeListing" SkinID="GridView" ClientIDMode="Static" AllowPaging="True" DataKeyNames="organismTypeId" PageSize="15" OnRowDataBound="gridViewOrganismTypes_RowDataBound">
                    <Columns>
                        <asp:TemplateField ControlStyle-Width="5%" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlCheckBox" Width="100%" Height="100%">
                                    <asp:CheckBox runat="server" ID="chkOrganismTypeSelector" name="chkName" onClick="selectall('gridViewOrganismTypes','chkSelectAll')" />
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:CheckBox runat="server" ID="chkSelectAll" ClientIDMode="Static" name="allCheck" onclick="selectallMe('gridViewOrganismTypes','chkSelectAll')" /><%--SelectAll('gridViewOrganismTypes','chkSelectAll')--%>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ControlStyle-Width="85%" HeaderStyle-Width="85%" DataField="organismTypeName" HeaderText="Organism Type" SortExpression="organismTypeName" />
                        <asp:TemplateField HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlView" Width="100%" Height="100%">
                                    <a href="<%# Page.GetRouteUrl("organismtype-details", new {edit_mode = "view", organismtype2_id = Eval("organismTypeId")}) %>"
                                      
                                        class="GridIcon ico-view"></a>
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderTemplate>
                                View
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlEdit" Width="100%" Height="100%">
                                    <a href="<%# Page.GetRouteUrl("organismtype-details", new {edit_mode = "edit", organismtype2_id = Eval("organismTypeId")}) %>"
                                        class="GridIcon ico-edit"></a>
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderTemplate>
                                Edit
                            </HeaderTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="clearfix">
                <asp:ObjectDataSource ID="odsOrganismTypeListing" runat="server" DataObjectTypeName="Eisk.BusinessEntities.OrganismType"
                    DeleteMethod="DeleteOrganismType" EnablePaging="True" InsertMethod="CreateNewOrganismType" OldValuesParameterFormatString="original_{0}"
                    SelectCountMethod="GetTotalCountForAllOrganismTypes" SelectMethod="GetOrganismTypeByFilter" SortParameterName="orderby"
                    TypeName="Eisk.BusinessLogicLayer.OrganismTypeBLL" UpdateMethod="organismTypeId">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtFilter" Name="organismType" PropertyName="Text"
                            Type="String" />
                        <asp:Parameter Name="orderBy" Type="String" />
                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                        <asp:Parameter Name="maximumRows" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
            <div class="clearfix align-right">
                <asp:LinkButton SkinID="LinkButton" runat="server" ID="lbtAddNewOrganismType" Text="Add New OrganismType"
                    OnClick="ButtonAddNewOrganismType_Click" />
                <asp:LinkButton SkinID="AltLinkButton" OnClientClick="return confirm('Are you sure you want to delete all items?');"
                    runat="server" ID="buttonDeleteSelected" Text="Delete Selected" OnClick="ButtonDeleteSelected_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
