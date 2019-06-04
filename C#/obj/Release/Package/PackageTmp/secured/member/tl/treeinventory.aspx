<%@ Page Language="C#" MasterPageFile="~/App_Resources/master.master" AutoEventWireup="true" CodeBehind="treeinventory.aspx.cs" Inherits="Eisk.Web.TreeInventory"
    Title="Inventario | N@TURA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <link href="/App_Themes/Default/css/html.css" rel="stylesheet" />
    <style type="text/css">
        /* Info - Start*/
        .dropdownwrap {
            height: auto;
            width: 740px;
            float: left;
            margin: 0px 0px 0px 0px;
            display: none;
        }

        .dropdown {
            position: relative;
            top: 3px;
        }
        /* Info - END */
    </style>
    <script type="text/javascript">
        //var fieldName = 'chkTreeDetailsSelector';

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

        var fieldName = 'chkTreeDetailsSelector';

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

        function EditTreeActionProposedID(actionProposedID, projectOrganismID, userID) {
            var apiLocation = document.domain == 'localhost' ? "" : "/web";
            $.ajax({
                type: "POST",
                url: apiLocation + "/secured/member/tl/treelocation.aspx/EditTreeActionProposedID",
                data: "{actionProposedID:'" + actionProposedID + "',projectOrganismID:'" + projectOrganismID + "',userID:'" + userID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: errorFn
            });
        }

        function errorFn(result) {
            alert(result);
        }

        function confirmChangeActionProposed(ddl) {
            var selectedText = ddl.options[ddl.selectedIndex].text;
            if (confirm("Are you sure you want to change ALL this Trees to " + selectedText + "?")) {
                document.getElementById('BodyContentPlaceholder_ddlActionProposed').selectedIndex = ddl.selectedIndex;
                return true;
            }
            else {
                ddl.selectedIndex = 0;
                return false;
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

        function ret() {
            __doPostBack('<%= ddlActionProposed.ClientID  %>', '');
        }

        var chkLittoral_click = function (target) {
            if (target.checked) {
                $('#chkMaritimeZone').attr('checked', false);
            }
        };
        var chkMaritimeZone_click = function (target) {
            if (target.checked) {
                $('#chkLittoral').attr('checked', false);
            }
        };

        function OnChkBox_Click(isRange) {
            if (isRange) {
                $('.dropdownwrap').slideDown('400');
            } else {
                $('.dropdownwrap').slideUp('400');
            }
        };

        function Clone(value) {
            $('#pnlClone').toggle(value);
            $('#pnlLoading').toggle(value);
            $("#pnlLoading").height($(document).height());
            $("#pnlLoading").width($(document).width());
        };

        $(function () {
            $("#pnlLoading").height($(document).height());
            $("#pnlLoading").width($(document).width());
        });
    </script>
    <style type="text/css">
        /*this is what we want the div to look like    when it is not showing*/
        div.loading-invisible { /*make invisible*/
            display: none;
        }

        /*this is what we want the div to look like when it IS showing*/
        div.loading-visible { /*make visible*/
            display: block; /*position it 200px down the screen*/
            position: absolute;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            text-align: center; /*in supporting browsers, make it      a little transparent*/
            background: #fff;
            filter: alpha(opacity=75); /* internet explorer */
            -khtml-opacity: 0.75; /* khtml, old safari */
            -moz-opacity: 0.75; /* mozilla, netscape */
            opacity: 0.75; /* fx, safari, opera */
            border-top: 1px solid #ddd;
            border-bottom: 1px solid #ddd;
        }

        /*this is what we want the div to look like when it IS showing*/
        .loading { /*make visible*/
            display: block; /*position it 200px down the screen*/
            position: relative;
            margin-left: auto;
            margin-right: auto;
            filter: alpha(opacity=1); /* internet explorer */
            -khtml-opacity: 1; /* khtml, old safari */
            -moz-opacity: 1; /* mozilla, netscape */
            opacity: 1; /* fx, safari, opera */
            width: 500px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceholder" runat="server">
    <h1 class="title-regular">Inventario de Árboles</h1>
    <p>
        Lista de Árboles. Seleccione la opción apropiada para insertar, ver, editar o eliminar un árbol.
    </p>
    <asp:Panel ID="pnlLoading" runat="server" ClientIDMode="Static" CssClass="loading-visible" Style="display: none; z-index: 100;">
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlClone" ClientIDMode="Static" CssClass='notice loading' Style="display: none; width: 750px; z-index: 101;">
        <h1 class="title-regular">Copiar Características de un Árbol a Otros</h1>
        <asp:Panel runat="server" ID="pnlCloneError" CssClass='error' Style="display: none; width: 715px;">
            <asp:Label ID="lblCloneError" runat="server" />
        </asp:Panel>
        <div class="grid_10 inline alpha" style="width: 100%;">
            <asp:RadioButton ID="rbtCloneSelect" Value="0" Text="Copiar las características a los árboles previamente seleccionados (checkboxes)" GroupName="CloneFrom" onclick="OnChkBox_Click(0);" Checked="true" runat="server" /><br />
            <asp:RadioButton ID="rbtCloneRange" Value="1" Text="Copiar las características a los árboles en un rango" GroupName="CloneFrom" onclick="OnChkBox_Click(1);" runat="server" />
        </div>
        <div class="clearfix">
        </div>
        <br />
        <div class="grid_10 inline alpha">
            <asp:Panel runat="server" CssClass='clearfix success'>
                <strong>Número del Árbol a ser Copiado:</strong><br />
                <asp:TextBox ID="txtCloneBase" ClientIDMode="Static" runat="server" MaxLength="5" Width="65px" CssClass="text"></asp:TextBox>
                ''
                    &nbsp;                        
                        <asp:Button ID="btnCloneBase" ClientIDMode="Static" runat="server" Text="Buscar" SkinID="Button" OnClick="btnCloneBase_Click" Style="width:80px;" />
                        <asp:Button ID="btnCloneBaseClear" Visible="false" ClientIDMode="Static" runat="server" Text="Limpiar" SkinID="AltButton" OnClick="btnCloneBaseClear_Click" />
                <br />
            </asp:Panel>
        </div>
        <asp:Panel runat="server" ID="pnlCloneRange" ClientIDMode="Static" CssClass="dropdownwrap grid_9 inline omega">
            <asp:Panel runat="server" CssClass='clearfix success'>
                <div class="grid_10 inline alpha" style="width: 160px;">
                    <div class="clearfix align-left">
                        <strong>Número del Árbol Desde:</strong>
                        <asp:TextBox ID="txtCloneFrom" ClientIDMode="Static" runat="server" MaxLength="5" Width="65px" CssClass="text"></asp:TextBox>
                        <div style="padding-right: 10px; padding-top: 10px; float: right">-</div>
                    </div>
                </div>
                <div class="grid_9 inline omega" style="width: 150px;">
                    <div class="clearfix align-left">
                        <strong>Número del Árbol Hasta:</strong>
                        <asp:TextBox ID="txtCloneTo" ClientIDMode="Static" runat="server" MaxLength="5" Width="65px" CssClass="text"></asp:TextBox>
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>
        <div class="clearfix">
        </div>
        <div class="grid_10 inline alpha">
            <strong>Características del Árbol a ser Copiado:</strong>
            <br />
            <label>Número: </label>
            <asp:Label ID="lblNumber" runat="server" Style="font-style: italic;" />
            <br />
            <label>Nombre Común: </label>
            <asp:Label ID="lblNombreComun" runat="server" Style="font-style: italic;" />
            <br />
            <asp:Panel ID="pnlCloneVaras" Visible="false" runat="server">
                <label>Varas: </label>
                <asp:Label ID="lblVaras" runat="server" Style="font-style: italic;" />
                <br />
            </asp:Panel>
            <asp:Panel ID="pnlCloneDap" runat="server">
                <label>D.A.P: </label>
                <asp:Label ID="lblDap" runat="server" Style="font-style: italic;" />
                <br />
            </asp:Panel>
            <label>Altura (pies): </label>
            <asp:Label ID="lblAltura" runat="server" Style="font-style: italic;" />
            <br />
            <label>Acción Propuesta: </label>
            <asp:Label ID="lblAcciónPropuesta" runat="server" Style="font-style: italic;" />
            <br />
            <label>Condición: </label>
            <asp:Label ID="lblCondicion" runat="server" Style="font-style: italic;" />
            <br />
            <label>En la Servidumbre de Vigilancia de Litoral: </label>
            <asp:Label ID="lblLitoral" runat="server" Style="font-style: italic;" />
            <br />
            <label>En Zona Marítimo Terrestre: </label>
            <asp:Label ID="lblMaritimoTerrestre" runat="server" Style="font-style: italic;" />
            <br />
            <br />
        </div>
        <div class="grid_9 inline omega">
            <div class="clearfix align-left">
                <strong>Características a Copiar:</strong>
                <br />
                <asp:CheckBox ID="chkCloneNombre" runat="server" Value="2" Selected="True" Text="Nombre Común - Nombre Científico" /><br />
                <asp:Panel ID="pnlCloneVarasChk" Visible="false" runat="server">
                    <asp:CheckBox ID="chkCloneVaras" runat="server" Value="4" Selected="True" Text="Varas" /><br />
                </asp:Panel>
                <asp:Panel ID="pnlCloneDapChk" runat="server">
                    <asp:CheckBox ID="chkCloneDap" runat="server" Value="5" Selected="True" Text="D.A.P" /><br />
                </asp:Panel>
                <asp:CheckBox ID="chkAltura" runat="server" Value="7" Selected="True" Text="Altura (pies)" /><br />
                <asp:CheckBox ID="chkAcciónPropuesta" runat="server" Value="8" Selected="True" Text="Acción Propuesta" /><br />
                <asp:CheckBox ID="chkCondicion" runat="server" Value="9" Selected="True" Text="Condición" /><br />
                <asp:CheckBox ID="chkLitoral" runat="server" Value="10" Selected="True" Text="Árbol en la Servidumbre de Vigilancia de Litoral" /><br />
                <asp:CheckBox ID="chkMaritimoTerrestre" runat="server" Value="11" Selected="True" Text="Árbol en la Zona Marítimo Terrestre" /><br />
                <asp:CheckBox ID="chkComentarios" runat="server" Value="12" Selected="True" Text="Comentarios" /><br />
            </div>
        </div>
        <div class="clearfix">
        </div>
        <div class="grid_10 inline alpha">
            <div class="clearfix align-left">
                <br />
                <asp:LinkButton SkinID="AltLinkButton"
                    runat="server" ID="btnClear" Text="Limpiar Campos" OnClick="btnClear_Click" />
            </div>
        </div>
        <div class="grid_9 inline omega">
            <div class="clearfix align-right">
                <br />
                <asp:LinkButton SkinID="LinkButton" runat="server" ID="btnCloneSelected" Text="Copiar" OnClientClick="return confirm('¿Está seguro?');"
                    OnClick="btnCloneSelected_Click" />
                <asp:LinkButton ID="Button3" CausesValidation="false" runat="server" Text="Volver" OnClientClick="Clone(false); return false;"
                    SkinID="AltLinkButton" />
            </div>
        </div>
        <br />
        <br />
        <br />
    </asp:Panel>
    <asp:ScriptManager ID="scriptManager" runat="server" />
    <asp:UpdatePanel ID="upd" runat="server">
        <ContentTemplate>
            <asp:Literal EnableViewState="false" runat="server" ID="ltlMessage"></asp:Literal>
            <div class="grid_10 inline alpha">
                <div class="clearfix align-left">
                    <div style="width: 50%; float: left">
                        <label>Ordenar por</label>
                        <asp:DropDownList ID="ddlOrderBy" runat="server" Style="width: 150px;"
                            AutoPostBack="true">
                            <asp:ListItem Text="Número" Value="Number"></asp:ListItem>
                            <asp:ListItem Text="Nombre Común" Value="Project_Organisms.Organism.CommonName.CommonNameDesc"></asp:ListItem>
                            <asp:ListItem Text="Nombre Científico" Value="Project_Organisms.Organism.ScientificName.ScientificNameDesc"></asp:ListItem>
                            <asp:ListItem Text="Acción" Value="ActionProposed.ActionProposedID"></asp:ListItem>
                            <asp:ListItem Text="D.A.P." Value="Dap"></asp:ListItem>
                            <asp:ListItem Text="Altura" Value="Height"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div style="width: 50%; float: right">
                        <label>
                            Condición</label><br />
                        <asp:DropDownList ID="ddlCondition" runat="server" AppendDataBoundItems="true"
                            AutoPostBack="true" DataSourceID="odsConditionList" DataTextField="ConditionDesc" Width="150px"
                            DataValueField="ConditionID" EnableViewState="true">
                            <asp:ListItem Text="<--ALL-->" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:ObjectDataSource ID="odsConditionList" runat="server" TypeName="Eisk.BusinessLogicLayer.ConditionBLL"
                            EnableViewState="true" SelectMethod="GetAllConditions" />
                    </div>
                </div>
            </div>
            <div class="grid_9 inline omega">
                <div class="clearfix align-left ">
                    <div style="width: 50%; float: left">
                        <label>Acción Propuesta</label><br />
                        <select id="ddlActionProposed" runat="server" onchange=' ret();this.style.backgroundColor=this.options[this.selectedIndex].style.backgroundColor;'>
                            <option label="<--ALL-->" style="background-color: #fff;" value="0"></option>
                            <option label="Corte y Remoción" style="background-color: #FF4A4A;" value="1"></option>
                            <option label="Protección" style="background-color: rgb(51, 204, 0);" value="2"></option>
                            <option label="Poda" style="background-color: rgb(255, 127, 39);" value="3"></option>
                            <option label="Transplantar" style="background-color: rgb(255, 255, 51);" value="4"></option>
                            <option label="Determinar Luego" style="background-color: rgb(63, 205, 255);" value="5"></option>
                        </select>
                        <%--                        
                            <
                            asp:DropDownList 
                            ID="ddlActionProposed" 
                            runat="server" 
                            AppendDataBoundItems="true" 
                            Style="width: 150px;"
                            AutoPostBack="true" 
                            DataSourceID="odsActionProposedList" 
                            DataTextField="ActionProposedDesc"
                            DataValueField="ActionProposedID"
                            >
                            <asp:ListItem Text="<--ALL-->" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        --%>
                        <asp:ObjectDataSource ID="odsActionProposedList" runat="server" TypeName="Eisk.BusinessLogicLayer.ActionProposedBLL"
                            EnableViewState="true" SelectMethod="GetAllActionProposeds" />
                    </div>
                    <div style="width: 50%; float: right;">
                        &nbsp;&nbsp;<label style="color: red;">Cambiar lo Filtrado a</label>
                        <br />
                        &nbsp;&nbsp;
                    <asp:DropDownList ID="ddlActionProposedChange" runat="server" AppendDataBoundItems="true" Style="width: 150px;" OnDataBound="ddlActionProposedChange_DataBound"
                        AutoPostBack="true" DataSourceID="odsActionProposedList" DataTextField="ActionProposedDesc" onchange='confirmChangeActionProposed(this);'
                        DataValueField="ActionProposedID" OnSelectedIndexChanged="ddlActionProposedChange_SelectedIndexChanged">
                        <asp:ListItem Text="<--SELECT->" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="grid_10 inline alpha">
                <div class="clearfix align-left">
                    <asp:CheckBox ID="rbReverse" Text="Reversar" runat="server" Checked="true" AutoPostBack="true"></asp:CheckBox>
                    <br />
                    <asp:CheckBox ID="cbCepas" Text="Cepas" runat="server" AutoPostBack="true"></asp:CheckBox>
                    <br />
                </div>
            </div>
            <div class="grid_9 inline omega">
                <div class="clearfix align-left">
                    <asp:CheckBox ID="chkLittoral" ClientIDMode="Static" runat="server" AutoPostBack="true" onclick="chkLittoral_click(this);" Text="Árboles en la Servidumbre de Vigilancia de Litoral" Value="vl" />
                    <br />
                    <asp:CheckBox ID="chkMaritimeZone" ClientIDMode="Static" runat="server" AutoPostBack="true" onclick="chkMaritimeZone_click(this);" Text="Árboles en la Zona Marítimo Terrestre " Value="mt" />
                    <br />
                </div>
            </div>
            <div class="grid_10 inline alpha">
                <div class="clearfix align-left ">
                    <div style="width: 100%; float: right;">
                        <label>&nbsp;Árboles por Página&nbsp;</label>
                        <asp:DropDownList ID="ddlPageSize" runat="server" Style="width: 150px;" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged"
                            AutoPostBack="true">
                            <asp:ListItem Text="15" Value="15"></asp:ListItem>
                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="grid_9 inline omega">
                <div class="clearfix align-left">
                    <asp:TextBox ID="txtFilter" runat="server" CssClass="text" TabIndex="1" Style="width: 260px;" onfocus="SetCursorToTextEnd(this);" onkeydown="javascript:var ret = checkEnterPressed(event,this); this.focus(); return ret;" />
                    <%-- Hack Fix enter button problem --%>
                    <asp:TextBox ID="DummyTextBox" runat="server" Style="visibility: hidden; display: none;" />
                    <asp:LinkButton ID="lbFilter" TabIndex="2" runat="server" SkinID="LinkButton" Text="Filtrar" UseSubmitBehavior="true" ViewStateMode="Inherit" OnClick="lbFilter_Click" />
                </div>
            </div>
            <div class="grid-viewer grid_19 clearfix">
                <asp:GridView ID="gridViewTreeDetails" runat="server" DataSourceID="odsTreeDetailsListing" SkinID="GridView" ClientIDMode="Static"
                    AllowPaging="True"
                    DataKeyNames="TreeDetailsID"
                    OnRowCommand="gridViewTreeDetails_RowCommand"
                    OnRowDataBound="gridViewTreeDetails_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="3%" ItemStyle-VerticalAlign="Middle">
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlCheckBox" Width="100%" Height="100%">
                                    <asp:CheckBox runat="server" ID="chkTreeDetailsSelector" name="chkName" onClick="selectall('gridViewTreeDetails','chkSelectAll')" />
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:CheckBox runat="server" ID="chkSelectAll" ClientIDMode="Static" name="allCheck" onclick="selectallMe('gridViewTreeDetails','chkSelectAll')" />
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ControlStyle-Width="3%" HeaderStyle-Width="3%" DataField="number" HeaderText="#" ItemStyle-VerticalAlign="Middle" />
                        <asp:TemplateField HeaderStyle-Width="61%" SortExpression="CommonNameDesc" ItemStyle-VerticalAlign="Middle">
                            <ItemTemplate>
                                <table style="margin-bottom: 0px; padding: 0px;">
                                    <tr>
                                        <td style='width: 360px; padding: 0;'>
                                            <asp:Label ID="lblCommonName" runat="server" Style="width: 100%;" Text='<%# Eval("Project_Organisms.Organism.CommonName.CommonNameDesc") %>' />
                                            /<asp:Label ID="lblScientificName" runat="server" Style="width: 100%; margin-left: 5px;" Font-Italic="true" Text='<%# Eval("Project_Organisms.Organism.ScientificName.ScientificNameDesc") %>' />
                                        </td>
                                        <td style='width: 120px; padding: 0; display: <%# (int)Eval("Condition.ConditionID") == 1 ? "none":"block" %>'>
                                            <strong>(Condición<asp:Label ID="Label3" runat="server" Style="width: 100%; margin-left: 5px;" Text='<%# Eval("Condition.ConditionDesc") %>' />)</strong>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" ID="pnlDap" Style="padding: 0px;" Visible='<%# !(Convert.ToInt32( Eval("Varas")) > 0) %>'>

                                    <table style="margin-bottom: 0px; padding: 0px;">
                                        <tr>
                                            <td style='width: 180px; display: <%# (Boolean)Eval("Littoral")? "block":"none" %>'>
                                                <strong><i>En Litoral</i></strong>
                                            </td>
                                            <td style='width: 180px; display: <%# (Boolean)Eval("MaritimeZone") ? "block":"none" %>'>
                                                <strong><i>En Zona Marítimo Terrestre</i></strong>
                                            </td>
                                            <td style='width: 80px;'>
                                                <strong>Altura: </strong>&nbsp;<asp:Label ID="lblHeight" runat="server" Text='<%# Eval("Height","{0:0.####################################}") %>' />
                                            </td>
                                            <td style='width: 80px;'>
                                                <strong>D.A.P:</strong>&nbsp;<asp:Label ID="lblDap" runat="server" Text='<%# Eval("Dap","{0:0.####################################}") %>' />
                                            </td>
                                            <td style='width: 80px;'>
                                                <strong>Troncos:</strong>&nbsp;<asp:Label ID="lblDapCounter" runat="server" Text='<%# Eval("Dap_Counter","{0:0.####################################}") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel runat="server" ID="pnlVaras" Style="padding: 0px;" Visible='<%# (Convert.ToInt32( Eval("Varas")) > 0) %>'>
                                    <table style="margin-bottom: 0px; padding: 0px;">
                                        <tr>
                                            <td style='width: 180px; display: <%# (Boolean)Eval("Littoral")? "block":"none" %>'>
                                                <strong><i>En Litoral</i></strong>
                                            </td>
                                            <td style='width: 180px; display: <%# (Boolean)Eval("MaritimeZone") ? "block":"none" %>'>
                                                <strong><i>En Zona Marítimo Terrestre</i></strong>
                                            </td>
                                            <td style='width: 80px;'><strong>Altura: </strong>&nbsp;<asp:Label ID="Label1" runat="server" Text='<%# Eval("Height","{0:0.####################################}") %>' /></td>
                                            <td style='width: 120px; text-align: left; padding-left: 0px;'><strong>Cepa:</strong>&nbsp;<asp:Label ID="lblvaras" runat="server" Text='<%# Eval("Varas","{0:0.####################################}") %>' />&nbsp;varas
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderTemplate>
                                Nombre Común/Científico
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="23%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <select onchange='this.style.backgroundColor=this.options[this.selectedIndex].style.backgroundColor; <%# "EditTreeActionProposedID(this.options[this.selectedIndex].value, " + Eval("Project_Organisms.ProjectOrganismID") + ",editorID); return true; " %>' style='background-color: #<%#   getColor((Eisk.BusinessEntities.ActionProposed )(Eval("ActionProposed"))) %>;'>
                                    <option label="Corte y Remoción" style="background-color: #FF4A4A;" <%# Convert.ToInt32( Eval("ActionProposed.ActionProposedID")) == 1 ?  " selected " : "" %> value="1"></option>
                                    <option label="Protección" style="background-color: rgb(51, 204, 0);" <%# Convert.ToInt32( Eval("ActionProposed.ActionProposedID")) == 2 ?  " selected " : "" %> value="2"></option>
                                    <option label="Poda" style="background-color: rgb(255, 127, 39);" <%# Convert.ToInt32( Eval("ActionProposed.ActionProposedID")) == 3 ?  " selected " : "" %> value="3"></option>
                                    <option label="Transplantar" style="background-color: rgb(255, 255, 51);" <%# Convert.ToInt32( Eval("ActionProposed.ActionProposedID")) == 4 ?   " selected " : "" %> value="4"></option>
                                    <option label="Determinar Luego" style="background-color: rgb(63, 205, 255);" <%# Convert.ToInt32( Eval("ActionProposed.ActionProposedID")) == 5 ?  " selected " : "" %> value="5"></option>
                                </select>
                            </ItemTemplate>
                            <HeaderTemplate>
                                Acción Propuesta
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <%--  <strong>View</strong>--%>
                                <a href="<%# Page.GetRouteUrl("tl", new { Project_id = RouteData.Values["Project_id"] }) +  "?poid=" + Eval("Project_Organisms.ProjectOrganismID") + "&lat=0&lon=0"%>"
                                    class="GridIcon2 ico-view" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                Ver
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <%-- <strong>Edit</strong>--%>
                                <asp:ImageButton runat="server" CommandName="lb" CommandArgument='<%# Eval("Project_Organisms.ProjectOrganismID")  %>' ImageUrl="~/App_Themes/Default/images/listing/ico-edit.png" AlternateText=""
                                    CssClass="GridIcon2" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                Editar
                            </HeaderTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="clearfix">
                <asp:ObjectDataSource ID="odsTreeDetailsListing" runat="server" DataObjectTypeName="Eisk.BusinessEntities.TreeDetail"
                    DeleteMethod="DeleteTreeDetails" EnablePaging="True" InsertMethod="CreateNewTreeDetails" OldValuesParameterFormatString="original_{0}"
                    SelectCountMethod="GetTotalCountForAllTreeDetailsInProjectByFilter" SelectMethod="GetTreeDetailsInProjectByFilter" OnSelected="odsTreeDetailsListing_Selected"
                    TypeName="Eisk.BusinessLogicLayer.TreeDetailBLL" UpdateMethod="TreeDetailsID" OnSelecting="odsTreeDetailsListing_Selecting">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtFilter" Name="treeDetail" PropertyName="Text"
                            Type="String" />
                        <asp:ControlParameter ControlID="ddlOrderBy" Name="orderBy" PropertyName="SelectedValue"
                            Type="String" />
                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                        <%--<asp:Parameter Name="maximumRows" Type="Int32" />--%>
                        <asp:Parameter Name="projectID" Type="Int32" />
                        <asp:ControlParameter ControlID="ddlPageSize" Name="maximumRows" PropertyName="SelectedValue"
                            Type="Int32" />
                        <asp:ControlParameter ControlID="ddlActionProposed" Name="actionProposed" PropertyName="Value"
                            Type="Int32" />
                        <asp:ControlParameter ControlID="ddlCondition" Name="condition" PropertyName="SelectedValue"
                            Type="Int32" />
                        <asp:ControlParameter ControlID="rbReverse" Name="isReverse" PropertyName="Checked"
                            Type="Boolean" />
                        <asp:ControlParameter ControlID="cbCepas" Name="isCepas" PropertyName="Checked"
                            Type="Boolean" />
                        <asp:ControlParameter ControlID="chkLittoral" Name="isLittoral" PropertyName="Checked"
                            Type="Boolean" />
                        <asp:ControlParameter ControlID="chkMaritimeZone" Name="isMaritimeZone" PropertyName="Checked"
                            Type="Boolean" />
                        <asp:ControlParameter ControlID="ddlActionProposedChange" Name="actionProposedChange" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
            <div class="grid_10 inline alpha">
                <div class="clearfix align-left">
                    <asp:LinkButton CausesValidation="false" runat="server" Text="Copiar Características de un Árbol a Otros" OnClick="btnClear_Click"
                        SkinID="LinkButton" />
                </div>
            </div>
            <div class="clearfix align-right">
                <%  if (ProjectCenterSet())
                    { %>
                <asp:LinkButton SkinID="LinkButton" runat="server" ID="lbtAddNewTreeDetails" Text="Añadir Nuevo Árbol"
                    OnClick="ButtonAddNewTreeDetails_Click" />

                <% } %>
                <asp:LinkButton SkinID="AltLinkButton" OnClientClick="return confirm('¿Está seguro?');"
                    runat="server" ID="buttonDeleteSelected" Text="Eliminar Árboles" OnClick="ButtonDeleteSelected_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
