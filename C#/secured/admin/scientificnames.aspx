<%@ Page Language="C#" MasterPageFile="~/App_Resources/master.master" AutoEventWireup="true" CodeBehind="scientificnames.aspx.cs" Inherits="Eisk.Web.ScientificName"
    Title="Nombres Científicos | N@TURA" MetaKeywords="scientific,name"
    MetaDescription="Lista de nombres científicos. Seleccione la opción apropiada para insertar, ver, editar o eliminar un nombre científico." %>

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

        var fieldName = 'chkScientificNameSelector';

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
    <h1 class="title-regular">Nombres Científicos</h1>
    <p>
        Lista de nombres científicos. Seleccione la opción apropiada para insertar, ver, editar o eliminar un nombre científico.
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
                <asp:GridView ID="gridViewScientificNames" runat="server" DataSourceID="odsScientificNameListing" SkinID="GridView" ClientIDMode="Static" AllowPaging="True" DataKeyNames="scientificNameId" PageSize="15" OnRowDataBound="gridViewScientificNames_RowDataBound">
                    <Columns>
                        <asp:TemplateField ControlStyle-Width="5%" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlCheckBox" Width="100%" Height="100%">
                                    <asp:CheckBox runat="server" ID="chkScientificNameSelector" name="chkName" onClick="selectall('gridViewScientificNames','chkSelectAll')" />
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:CheckBox runat="server" ID="chkSelectAll" ClientIDMode="Static" name="allCheck" onclick="selectallMe('gridViewScientificNames','chkSelectAll')" /><%--SelectAll('gridViewScientificNames','chkSelectAll')--%>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ControlStyle-Width="41%" HeaderStyle-Width="41%" DataField="scientificNameDesc" HeaderText="Nombre Científico" SortExpression="scientificNameDesc" />
                        <asp:BoundField ControlStyle-Width="22%" HeaderStyle-Width="22%" DataField="createdDate" HeaderText="Creado En" SortExpression="createdDate" DataFormatString="{0:MMM/dd/yyyy h:mm:ss tt}" />
                        <asp:BoundField ControlStyle-Width="22%" HeaderStyle-Width="22%" DataField="editedDate" HeaderText="Editado En" SortExpression="editedDate" DataFormatString="{0:MMM/dd/yyyy h:mm:ss tt}" />
                        <asp:TemplateField HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlView" Width="100%" Height="100%">
                                    <a href="<%# Page.GetRouteUrl("scientificname-details", new {edit_mode = "view", project_id = (this.RouteData.Values["project_id"] as string), scientificname_id = Eval("scientificNameId")}) %>"
                                        class="GridIcon ico-view"></a>
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderTemplate>
                                Ver
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlEdit" Width="100%" Height="100%">
                                    <a href="<%# Page.GetRouteUrl("scientificname-details", new {edit_mode = "edit", project_id = (this.RouteData.Values["project_id"] as string), scientificname_id = Eval("scientificNameId")}) %>"
                                        class="GridIcon ico-edit"></a>
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderTemplate>
                                Editar
                            </HeaderTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="clearfix">
                <asp:ObjectDataSource ID="odsScientificNameListing"       runat="server" DataObjectTypeName="Eisk.BusinessEntities.ScientificName"
                    DeleteMethod="DeleteScientificName" EnablePaging="True" InsertMethod="CreateNewScientificName" OldValuesParameterFormatString="original_{0}"
                    SelectCountMethod="GetTotalCountForAllScientificNames" SelectMethod="GetScientificNameByFilter" SortParameterName="orderby" OnSelected="odsScientificNameListing_Selected"
                    TypeName="Eisk.BusinessLogicLayer.ScientificNameBLL" UpdateMethod="scientificNameId">
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
                <asp:LinkButton SkinID="LinkButton" runat="server" ID="lbtAddNewScientificName" Text="Añadir Nombre Científico"
                    OnClick="ButtonAddNewScientificName_Click" />
                <asp:LinkButton SkinID="AltLinkButton" OnClientClick="return confirm('¿Está seguro?');"
                    runat="server" ID="buttonDeleteSelected" Text="Eliminar Seleccionados" OnClick="ButtonDeleteSelected_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
