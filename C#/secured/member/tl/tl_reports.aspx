<%@ Page Language="C#" MasterPageFile="~/App_Resources/master.master" AutoEventWireup="true" CodeBehind="tl_reports.aspx.cs" Inherits="Eisk.Web.Report"
    Title="Reportes | N@TURA" MetaKeywords="reporte" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <script type="text/javascript">
        var fieldName = 'chkReportSelector';
        var lbtGenerateReports = "BodyContentPlaceholder_lbtGenerateReports";
        var time = 20000;

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

        function hideDiv() {
            document.getElementById("loading").className = "loading-invisible";
        };

        function showDiv() {
            document.getElementById("loading").className = "loading-visible";

            setTimeout(function () { hideDiv(); }, time);
        };

        var loadingDiv = document.getElementById("loading");
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
            position: absolute;
            top: 50%;
            left: 50%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceholder" runat="server">
    <asp:ScriptManager ID="scriptManager" runat="server" />
    <div id="loading" class="loading-invisible">
        <p>
            <img class="loading" src='<%#  webPath + "/secured/member/tl/loading2.gif" %>' alt="Loading..." />
        </p>
    </div>
    <script type="text/javascript">
        document.getElementById("loading").className = "loading-visible";

        var apiLocation = document.domain == 'localhost' ? "" : "/web";
        var imgLoc = apiLocation + "/secured/member/tl/loading2.gif";

        // reset img src
        $('.loading').attr('src', imgLoc);

        var oldLoad = window.onload;
        var newLoad = oldLoad ? function () { hideDiv.call(this); oldLoad.call(this); } : hideDiv;

        window.onload = newLoad;
    </script>
    <h1 class="title-regular clearfix">
        <span class="grid_10 inline alpha">
            <div class="clearfix align-left">
                Reportes
            </div>
        </span>
    </h1>
    <script language="javascript" type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        var postBackElement;

        prm.add_endRequest(EndRequest);
        prm.add_initializeRequest(InitializeRequest);

        function InitializeRequest(sender, args) {
            postBackElement = args.get_postBackElement();
            if (postBackElement.id == lbtGenerateReports) {
                showDiv();
            }
        }

        function EndRequest(sender, args) {
            // if (postBackElement.id == lbtGenerateReports && $("#BodyContentPlaceholder_hfReports") && $("#BodyContentPlaceholder_hfReports")[0].value != "") {
            //window.location.replace($("#BodyContentPlaceholder_hfReports")[0].value);
            //var handle = window.open($("#BodyContentPlaceholder_hfReports")[0].value, "mywindow", "menubar=0,resizable=0,location=0,status=0,scrollbars=1,width=200,height=500");
            if ($("#hfReports")[0].value)
            {
                window.location.href = 'http://localhost:53193' + $("#hfReports")[0].value;
                time = 5000;
            }
            //hideDiv();
            // }
            // else 
            setTimeout(function () {
                hideDiv();
            }, 2000);
        }
    </script>
    <asp:UpdatePanel ID="upd" runat="server">
        <ContentTemplate>
            <asp:Literal EnableViewState="false" runat="server" ID="ltlMessage"></asp:Literal>
            <div class="grid_10 inline alpha" style="width: 100%;">
                <asp:LinkButton ID="lbGenerateCSV" OnClick="lbGenerateCSV_Click" runat="server" OnClientClick="showDiv();" Text="Generar CSV de todos los árboles en el proyecto"></asp:LinkButton>
                <br />
                <asp:LinkButton ID="lbGenerateCSVProtec" OnClick="lbGenerateCSVProtec_Click" runat="server" OnClientClick="showDiv();" Text="Generar CSV de árboles a Protegerse o Podarse en el proyecto"></asp:LinkButton>
                <br />
            </div>
            <asp:Literal EnableViewState="false" runat="server" ID="Literal1"></asp:Literal>
            <div class="grid_10 inline alpha" style="width: 100%;">
                <br />
                <strong>
                    <asp:Label runat="server" Text="Seleccionar los Reportes a Generar:" Style="text-decoration: underline; font-size: 15px;"></asp:Label></strong>
                <br />
                <br />
            </div>
            <asp:HiddenField ID="hfReports" runat="server" ClientIDMode="Static" EnableViewState="false" />
            <div class="grid_10 inline alpha">
                <div class="clearfix align-left" style="margin-bottom: 10px;">
                    <label>Crear Tabla de Contenido: </label>
                    <asp:CheckBox ID="cbCreateIndex" runat="server" Checked="true" AutoPostBack="true"></asp:CheckBox>
                </div>
                <div class="clearfix align-left" style="margin-bottom: 10px;">
                    <label>Cabecera en cada Reporte: </label>
                    <asp:CheckBox ID="cbHeaderOnEachReport" runat="server" AutoPostBack="true"></asp:CheckBox>
                </div>
            </div>
            <div class="grid-viewer grid_19 clearfix">
                <asp:GridView ID="gridViewReports" runat="server" DataSourceID="odsReportListing" SkinID="GridView" ClientIDMode="Static" AllowPaging="True" DataKeyNames="repId" PageSize="15" OnRowDataBound="gridViewReports_RowDataBound">
                    <Columns>
                        <asp:TemplateField ControlStyle-Width="5%" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlCheckBox" Width="100%" Height="100%">
                                    <asp:CheckBox runat="server" ID="chkReportSelector" name="chkName" onClick="selectall('gridViewReports','chkSelectAll')" />
                                </asp:Panel>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:CheckBox runat="server" ID="chkSelectAll" ClientIDMode="Static" name="allCheck" onclick="selectallMe('gridViewReports','chkSelectAll')" /><%--SelectAll('gridViewProjects','chkSelectAll')--%>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ControlStyle-Width="95%" HeaderStyle-Width="95%" DataField="RepName" HeaderText="Report Name" />
                    </Columns>
                </asp:GridView>
            </div>
            <div class="clearfix align-right">
            </div>
            <div class="clearfix">
                <asp:ObjectDataSource ID="odsReportListing" runat="server" DataObjectTypeName="Eisk.BusinessEntities.Rep"
                    EnablePaging="True" OldValuesParameterFormatString="original_{0}"
                    SelectCountMethod="GetTotalCountForAllReps" SelectMethod="GetAllReps" SortParameterName="orderby"
                    TypeName="Eisk.BusinessLogicLayer.RepBLL" UpdateMethod="repId">
                    <SelectParameters>
                        <asp:Parameter Name="orderBy" Type="String" />
                        <asp:Parameter Name="maximumRows" Type="Int32" />
                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
            <div class="grid_10 inline alpha">
                <div class="clearfix align-left">
                    <%--<asp:Button ID="Button3" CausesValidation="false" runat="server" Text="Volver" OnClick="ButtonGoToViewPage_Click"
                        SkinID="AltButton" />--%>
                    &nbsp;
                </div>
            </div>
            <div class="grid_9 inline omega">
                <div class="clearfix align-right">
                    <asp:LinkButton SkinID="LinkButton" runat="server" ID="lbtGenerateReports" Text="Generar Reportes" OnClick="lbtGenerateReports_Click" OnClientClick="showDiv();$('#hfReports').val('');" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    -
</asp:Content>
