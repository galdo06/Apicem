<%@ Page Language="C#" MasterPageFile="~/App_Resources/master.master" AutoEventWireup="true" CodeBehind="projects.aspx.cs" Inherits="Eisk.Web.Project"
    Title="Proyectos | N@TURA" MetaKeywords="proyecto"
    MetaDescription="Lista de proyectos. Seleccione la opción apropiada para insertar, ver, editar o eliminar un proyecto." %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <script type="text/javascript">
        //function selectall(frmId, check) {
        //    var frm = document.getElementById(frmId);

        //    var i = frm.all[0].rows.length;
        //    //var e = document.frm.elements;
        //    var name = new Array();
        //    var value = new Array();
        //    var j = 0;
        //    for (var k = 1; k < i; k++) {
        //        var checkbox = frm.all[0].rows[k].children[0].children[0].children[0].children[0];
        //        if (checkbox) {
        //            if (checkbox.id == fieldName) {
        //                if (checkbox.checked == true) {
        //                    value[j] = checkbox.value;
        //                    j++;
        //                }
        //            }
        //        }
        //    }
        //    checkSelect(frmId);
        //}

        //function selectCheck(frmId, check) {
        //    var frm = document.getElementById(frmId);
        //    var i = frm.all[0].rows.length;

        //    for (var k = 1; k < i; k++) {
        //        if (frm.all[0].rows[k].children[0].children[0].children[0].children[0].id == fieldName) {
        //            frm.all[0].rows[k].children[0].children[0].children[0].children[0].checked = check;
        //        }
        //    }
        //    //selectall();
        //}

        //function selectallMe(frmId, id) {
        //    var frmChkAll = document.getElementById(id);

        //    if (frmChkAll.checked == true) {
        //        selectCheck(frmId, true);
        //    }
        //    else {
        //        selectCheck(frmId, false);
        //    }
        //}

        //function checkSelect(frmId) {
        //    var frm = document.getElementById(frmId);

        //    var i = frm.all[0].rows.length;
        //    var berror = true;
        //    for (var k = 1; k < i; k++) {
        //        var checkbox = frm.all[0].rows[k].children[0].children[0].children[0].children[0];
        //        if (checkbox.id == fieldName) {
        //            if (checkbox.checked == false) {
        //                berror = false;
        //                break;
        //            }
        //        }
        //    }

        //    var chkSelectAll = document.getElementById('chkSelectAll');
        //    if (berror == false) {
        //        chkSelectAll.checked = false;
        //    }
        //    else {
        //        chkSelectAll.checked = true;
        //    }
        //}

        var fieldName = 'chkProjectSelector';

        function selectall(frmId, check) {
            var frm = document.getElementById(frmId);

            var i = frm.rows.length;
            //var e = document.frm.elements;
            var name = new Array();
            var value = new Array();
            var j = 0;
            for (var k = 1; k < i; k++) {
                var checkbox = frm.rows[k].children[0].children[0].children[0].children[0];
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
            var i = frm.rows.length;

            for (var k = 1; k < i; k++) {
                if (frm.rows[k].children[0].children[0].children[0].children[0].id == fieldName) {
                    frm.rows[k].children[0].children[0].children[0].children[0].checked = check;
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

            var i = frm.rows.length;
            var berror = true;
            for (var k = 1; k < i; k++) {
                var checkbox = frm.rows[k].children[0].children[0].children[0].children[0];
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

        function checkEnterPressed(evt, control) {
            var charCode = evt.keyCode || evt.which;
            if (charCode == 13) {
                __doPostBack('ctl00$BodyContentPlaceholder$lbFilter', '');
                return false;
            }
            return true;
        }

        function SetCursorToTextEnd(textControl) {
            if (textControl != null && textControl.value.length > 0) {
                if (textControl.createTextRange) {
                    var FieldRange = textControl.createTextRange();
                    FieldRange.moveStart('character', textControl.value.length);
                    FieldRange.collapse();
                    FieldRange.select();
                }
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceholder" runat="server">
    <h1 class="title-regular">Proyectos</h1>
    <p>
        Lista de proyectos. Seleccione la opción apropiada para insertar, ver, editar o eliminar un proyecto.
    </p>
    <asp:ScriptManager ID="scriptManager" runat="server" />
    <asp:UpdatePanel ID="upd" runat="server">
        <ContentTemplate>
            <asp:Literal EnableViewState="false" runat="server" ID="ltlMessage"></asp:Literal>
            <div class="clearfix align-right">
                <asp:TextBox ID="txtFilter" runat="server" CssClass="text" AutoPostBack="true" onfocus="SetCursorToTextEnd(this);" />
                <asp:LinkButton ID="lbFilter" runat="server" SkinID="LinkButton" Text="Filtrar" />
            </div>
            <div class="grid-viewer grid_19 clearfix">
                <asp:GridView ID="gridViewProjects" runat="server" DataSourceID="odsProjectListing" SkinID="GridView" ClientIDMode="Static" AllowPaging="True" DataKeyNames="ProjectId" PageSize="15" OnRowDataBound="gridViewProjects_RowDataBound">
                    <Columns>
                        <asp:TemplateField ControlStyle-Width="5%" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlCheckBox" Width="100%" Height="100%">
                                    <asp:CheckBox runat="server" ID="chkProjectSelector" name="chkName" onClick="selectall('gridViewProjects','chkSelectAll')" />
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:CheckBox runat="server" ID="chkSelectAll" ClientIDMode="Static" name="allCheck" onclick="selectallMe('gridViewProjects','chkSelectAll')" /><%--SelectAll('gridViewProjects','chkSelectAll')--%>
                            </HeaderTemplate>
                            <ControlStyle Width="5%" />
                            <HeaderStyle Width="5%" />
                        </asp:TemplateField>
                        <%--<asp:BoundField ControlStyle-Width="41%" HeaderStyle-Width="41%" DataField="projectName" HeaderText="Project" SortExpression="projectName" />--%>
                        <asp:TemplateField HeaderStyle-Width="41%">
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlOrganism" Width="100%" Height="100%">
                                    <%#   GetProjectName((int)Eval("ProjectId")) %>
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderTemplate>
                                Proyecto
                            </HeaderTemplate>
                            <HeaderStyle Width="41%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="22%">
                            <ItemTemplate>
                                <%--     <asp:Panel runat="server" ID="pnlCategorizationCertification" Width="100%" Height="100%">
                                    <asp:Button ID="btnCategorizationCertification" runat="server" Text="Categorization Certification"
                                        OnClick="btnCategorizationCertification_Click" CommandArgument='<%# Eval("ProjectId") %>'
                                        SkinID="GreenButton" />
                                </asp:Panel>--%>
                            </ItemTemplate>
                            <HeaderStyle Width="22%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="22%">
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlTreeLocation" Width="100%" Height="100%">
                                    <asp:Button ID="btnTreeLocation" runat="server" Text="Corte y Poda"
                                        OnClick="btnTreeLocation_Click" CommandArgument='<%# Eval("ProjectId") %>'
                                        SkinID="GreenButton" />
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderStyle Width="22%" />
                        </asp:TemplateField>
                        <%--<asp:BoundField ControlStyle-Width="22%" HeaderStyle-Width="22%" DataField="createdDate" HeaderText="Creado En" SortExpression="createdDate" DataFormatString="{0:MMM/dd/yyyy h:mm:ss tt}" />--%>
                        <%--<asp:BoundField ControlStyle-Width="22%" HeaderStyle-Width="22%" DataField="editedDate" HeaderText="Editado En" SortExpression="editedDate" DataFormatString="{0:MMM/dd/yyyy h:mm:ss tt}" />--%>
                        <asp:TemplateField HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlView" Width="100%" Height="100%">
                                    <a href="<%# Page.GetRouteUrl("project-details", new {edit_mode = "view", project_id = Eval("ProjectId")}) %>"
                                        class="GridIcon ico-view"></a>
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderTemplate>
                                Ver
                            </HeaderTemplate>
                            <HeaderStyle Width="5%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlEdit" Width="100%" Height="100%">
                                    <a href="<%# Page.GetRouteUrl("project-details", new {edit_mode = "edit", project_id = Eval("ProjectId")}) %>"
                                        class="GridIcon ico-edit"></a>
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderTemplate>
                                Editar
                            </HeaderTemplate>
                            <HeaderStyle Width="5%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="clearfix">
                <asp:ObjectDataSource ID="odsProjectListing" runat="server" DataObjectTypeName="Eisk.BusinessEntities.Project"
                    DeleteMethod="DeleteProject" EnablePaging="True" InsertMethod="CreateNewProject" OldValuesParameterFormatString="original_{0}"
                    SelectCountMethod="GetTotalCountForAllProjectsInGroupByFilter" SelectMethod="GetProjectsInGroupByFilter" SortParameterName="orderby" OnSelected="odsProjectListing_Selected"
                    TypeName="Eisk.BusinessLogicLayer.ProjectBLL" UpdateMethod="projectId" OnSelecting="odsProjectListing_Selecting">
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
                <asp:LinkButton SkinID="LinkButton" runat="server" ID="lbtAddNewProject" Text="Añadir Proyecto"
                    OnClick="ButtonAddNewProject_Click" />
                <asp:LinkButton SkinID="AltLinkButton" OnClientClick="return confirm('¿Está seguro?');"
                    runat="server" ID="buttonDeleteSelected" Text="Eliminar Seleccionados" OnClick="ButtonDeleteSelected_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
