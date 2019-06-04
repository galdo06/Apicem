<%@ Page Language="C#" MasterPageFile="~/App_Resources/default.master" AutoEventWireup="true" CodeBehind="projects.aspx.cs" Inherits="Projects_Page"
    Title="Projects Page | TreeLocation" MetaKeywords="scientific,name"
    MetaDescription="In this page you will be able to view the list of all projects. Click on the appropriate buttons to view, insert or update a project." %>

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

        var fieldName = 'chkProjectSelector';

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
    <h1 class="title-regular">Projects</h1>
    <p>
        In this page you will be able to view the list of all projects. 
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
                <asp:GridView ID="gridViewProjects" runat="server" DataSourceID="odsProjectListing" SkinID="GridView" ClientIDMode="Static" AllowPaging="True"
                    DataKeyNames="projectId" PageSize="15">
                    <Columns>
                        <asp:TemplateField ControlStyle-Width="3%" HeaderStyle-Width="3%">
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlCheckBox" Width="100%" Height="100%">
                                    <asp:CheckBox runat="server" ID="chkProjectSelector" name="chkName" onClick="selectall('gridViewProjects','chkSelectAll')" />
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:CheckBox runat="server" ID="chkSelectAll" ClientIDMode="Static" name="allCheck" onclick="selectallMe('gridViewProjects','chkSelectAll')" /><%--SelectAll('gridViewProjects','chkSelectAll')--%>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="70%">
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlOrganism" Width="100%" Height="100%">
                                    <a href="<%# Page.GetRouteUrl("organismsinproject", new { project_id = Eval("projectId")}) %>"><%#  Eval("projectName") %></a>
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderTemplate>
                                Project
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ControlStyle-Width="17%" HeaderStyle-Width="87%" DataField="EditedDate" HeaderText="Last Edited Date" SortExpression="EditedDate" />
                        <asp:TemplateField HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlView" Width="100%" Height="100%">
                                    <a href="<%# Page.GetRouteUrl("project-details", new {edit_mode = "view", project_id = Eval("projectId")}) %>"
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
                                    <a href="<%# Page.GetRouteUrl("project-details", new {edit_mode = "edit", project_id = Eval("projectId")}) %>"
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
                <asp:ObjectDataSource ID="odsProjectListing" runat="server" DataObjectTypeName="Eisk.BusinessEntities.Project"
                    DeleteMethod="DeleteProject" EnablePaging="True" InsertMethod="CreateNewProject" OldValuesParameterFormatString="original_{0}"
                    SelectCountMethod="GetTotalCountForAllProjects" SelectMethod="GetProjectByFilter" SortParameterName="orderby"
                    TypeName="Eisk.BusinessLogicLayer.ProjectBLL" UpdateMethod="projectId">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtFilter" Name="project" PropertyName="Text"
                            Type="String" />
                        <asp:Parameter Name="orderBy" Type="String" />
                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                        <asp:Parameter Name="maximumRows" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
            <div class="clearfix align-right">
                <asp:LinkButton SkinID="LinkButton" runat="server" ID="lbtAddNewProject" Text="Add New Project"
                    OnClick="ButtonAddNewProject_Click" />
                <asp:LinkButton SkinID="AltLinkButton" OnClientClick="return confirm('Are you sure you want to delete all items?');"
                    runat="server" ID="buttonDeleteSelected" Text="Delete Selected" OnClick="ButtonDeleteSelected_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
