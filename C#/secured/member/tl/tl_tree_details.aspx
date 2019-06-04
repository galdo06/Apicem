<%@ Page Language="C#" MasterPageFile="~/App_Resources/master.master" AutoEventWireup="false"
    Inherits="Tl_Tree_Details" Title="Árbol | N@tura"
    CodeBehind="tl_tree_details.aspx.cs" %>

<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

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

        div.loading-visible { /*make visible*/
            display: block; /*position it 200px down the screen*/
            position: absolute;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            text-align: center; /*in supporting browsers, make it      a little transparent*/
            background: #000;
            filter: alpha(opacity=85); /* internet explorer */
            -khtml-opacity: 0.85; /* khtml, old safari */
            -moz-opacity: 0.85; /* mozilla, netscape */
            opacity: 0.85; /* fx, safari, opera */
            border-top: 1px solid #ddd;
            border-bottom: 1px solid #ddd;
        }

        /*this is what we want the div to look like when it IS showing*/
        .loading { /*make visible*/
            /*display: block;*/ /*position it 200px down the screen*/
            position: absolute;
            top: 200px;
            border: 1px solid #ddd;
            background-color: white;
            margin-right: auto;
            margin-left: auto;
            margin-bottom: auto;
            height: 280px;
            width: 240px;
        }
    </style>
    <script type="text/javascript">
        function populating(source, eventArgs) {
            ShowLoading();
        }
        function populated(source, eventArgs) {
            CloseLoading();
        }

        function YourMethodHere(source, eventArgs) {
            $("#BodyContentPlaceholder_formViewTlProject_Organism_txtCommonName").val(trim(eventArgs._text.split("  -  ")[0]));
            $("#BodyContentPlaceholder_formViewTlProject_Organism_txtScientificName").val(trim(eventArgs._text.split("  -  ")[1]));
            $("#BodyContentPlaceholder_formViewTlProject_Organism_hfOrganism").val(eventArgs._value);
        }
    </script>
    <script type="text/javascript">

        $(function () {
            $("#BodyContentPlaceholder_formViewTlProject_txtParkings").mask("9?9999");
            $("#BodyContentPlaceholder_formViewTlProject_txtLots").mask("9?9999");
            $("#BodyContentPlaceholder_formViewTlProject_txtDistanceBetweenTrees").mask("9?9999");

            mountTrunks();
        });

        function mountTrunks() { 
            var _trunks = $('#hfDap').val();

            var hfDapArray = _trunks.split(",");

            $('#txtDapCounter').val(hfDapArray.length.toString());

            var dapTotal = 0;
            for (var i = 0; i < hfDapArray.length; i++) {
                var value = parseFloat(hfDapArray[i]);
                if (!isNaN(value) && value != 0) {
                    dapTotal += parseFloat(hfDapArray[i]);

                    $("#POITable").find('tbody')
                        .append($('<tr>')
                            .append($('<td>')
                                .append($('<span>')
                                    .text(hfDapArray[i])
                                )
                            )
                            .append($('<td>')
                                .append(
                                    $('<a>')
                                    .addClass('GridIcon ico-delete')
                                    .click(
                                    function () {
                                        deleteRow(this);
                                    })
                                )
                            )
                        );
                }
            }

            var dap = parseFloat(Math.round(Math.sqrt(Math.pow(dapTotal, 2) / parseFloat(hfDapArray.length)) * 100) / 100);
            $('#txtDap').val(dap);
            $('#txtDapAdd').val("");

            if (dapTotal > 0)
                document.getElementById('POITable').deleteRow(1);

            cbCepa_Click();

            $('#txtY').focus();
        }

        function btnAddTrunk_Click() {
            var value = $('#txtDapAdd').val();
            if (isNumber(value)) {
                var valueFloat = parseFloat(value);
                if (valueFloat >= 4 && (valueFloat < 99999)) {
                    $('#lblError').hide();
                    var hfDapVal = ($('#hfDap').val() == "0" || $('#hfDap').val() == "") ? "" : $('#hfDap').val();
                    var hfDapNewVal = hfDapVal == "" ? valueFloat.toString() : hfDapVal + ',' + valueFloat.toString();

                    $('#hfDap').val(hfDapNewVal);

                    var hfDapArray = hfDapNewVal.split(",");

                    $('#txtDapCounter').val(hfDapArray.length.toString());

                    mountTrunks();

                    ValidatorEnable($('#rvDap')[0], true);

                    $('#txtDapAdd').focus();

                    return;
                }
            }
            
            $('#lblError').show();
            txtY
        }

        function deleteRow(row) {
            var i = row.parentNode.parentNode.rowIndex;
            document.getElementById('POITable').deleteRow(i);


            var hfDapVal = $('#hfDap').val();
            var hfDapArray = hfDapVal.split(",");
            hfDapArray.splice(i - 1, 1);

            var daps = "";
            if (hfDapArray.length > 1) {
                for (var i = 0; i < hfDapArray.length; i++) {
                    if (parseFloat(hfDapArray[i]) != 0) {
                        if (i == 0)
                            daps = parseFloat(hfDapArray[i]);
                        else
                            daps += "," + parseFloat(hfDapArray[i]);
                    }
                }
                $('#hfDap').val(daps);

            }
            else if (hfDapArray.length == 1)
                $('#hfDap').val(hfDapArray[0]);
            else {
                $('#hfDap').val('');
                $('#txtDap').val('0');
                $('#txtDapCounter').val('1');

                $("#POITable").find('tbody').append('<tr><td colspan="2"><div class="notice">No hay D.A.P.</div></td></tr>');
                return;
            }

            $('#txtDapCounter').val(hfDapArray.length.toString());

            var dapTotal = 0;
            for (var i = 0; i < hfDapArray.length; i++) {
                if (parseFloat(hfDapArray[i]) != 0)
                    dapTotal += parseFloat(hfDapArray[i]);
            }

            var dap = parseFloat(Math.round(Math.sqrt(Math.pow(dapTotal, 2) / parseFloat(hfDapArray.length)) * 100) / 100);
            $('#txtDap').val(dap);
        }

        function btnClose_Click() {
            $('#divAddDap').slideUp('400');
            mouseleaveBind = true;
        }

        function cbCepa_Click() {

            var checked = $('#cbCepa')[0].checked;
             
            {
                oVal = document.getElementById("rvDap");
                oVal.enabled = !checked;
                //ValidatorEnable($('#rvDap')[0], !checked);  
                ValidatorUpdateDisplay($('#rvDap')[0]);
            }
            ValidatorEnable($('#rvDapCounter')[0], !checked);
            ValidatorEnable($('#rfvVaras')[0], checked);
            ValidatorEnable($('#rvVaras')[0], checked);

            if (checked) {
                $('#pnlDap').hide();
                $('#pnlVaras').show();
            }
            else {
                $('#pnlDap').show();
                $('#pnlVaras').hide();
            }
        }




        function isNumber(n) {
            return !isNaN(parseFloat(n)) && isFinite(n);
        }

        function trim(n) {
            return n.replace(/^\s+|\s+$/g, '');
        }

        function ShowLoading() {
            document.getElementById("imgLoading").style.display = 'block';
        }

        function CloseLoading() {
            document.getElementById("imgLoading").style.display = 'none';
        }

        function ValidateRangeY(sender, args) {
            var min = '<%#  System.Configuration.ConfigurationManager.AppSettings["YMinValue"] %>'
            var max = '<%#  System.Configuration.ConfigurationManager.AppSettings["YMaxValue"] %>'
            var value = $("#txtY").val();
            args.IsValid = validateTextNumericInRange(value, min, max) || parseFloat(value) == 0;
        }
        function ValidateRangeX(sender, args) {
            var min = '<%#  System.Configuration.ConfigurationManager.AppSettings["XMinValue"] %>'
            var max = '<%#  System.Configuration.ConfigurationManager.AppSettings["XMaxValue"] %>'
            var value = $("#txtX").val();
            args.IsValid = validateTextNumericInRange(value, min, max) || parseFloat(value) == 0;
        }
        function ValidateRangeLat(sender, args) {
            var min = '<%#  System.Configuration.ConfigurationManager.AppSettings["LatMinValue"] %>';
            var max = '<%#  System.Configuration.ConfigurationManager.AppSettings["LatMaxValue"] %>'
            var value = $("#txtLat").val();
            args.IsValid = validateTextNumericInRange(value, min, max) || parseFloat(value) == 0;
        }
        function ValidateRangeLon(sender, args) {
            var min = '<%#  System.Configuration.ConfigurationManager.AppSettings["LonMinValue"] %>';
            var max = '<%#  System.Configuration.ConfigurationManager.AppSettings["LonMaxValue"] %>';
            var value = $("#txtLon").val();
            args.IsValid = validateTextNumericInRange(value, min, max) || parseFloat(value) == 0;
        }

        function validateTextNumericInRange(textInput, min, max) {
            var value = parseFloat(textInput);
            return (!isNaN(value) && value >= min && value <= max);
        }

    </script>
    <script type='text/javascript'>
        /* Info - Start*/
        $(function () {
            $("#divLittoral").load(webPath + "/App_Resources/html/Litoral.html");
            $("#divMaritimeZone").load(webPath + "/App_Resources/html/MaritimeZone.html");
            $("#divDap").load(webPath + "/App_Resources/html/Dap.html");

            $(".dropdownwrap").hide();
            timeoutId = null;
            $('.dropdown').bind({
                mouseover: fcn_mouseover,
                mouseleave: fcn_mouseleave,
                click: function () {
                    if (this.mouseleaveBind == null || this.mouseleaveBind) {
                        $(this).nextAll('.dropdownwrap:first').slideDown('400');
                        $(this).unbind('mouseleave mouseover');
                        this.mouseleaveBind = false;
                    } else {
                        $(this).nextAll('.dropdownwrap:first').slideUp('400');
                        $(this).bind({
                            mouseover: fcn_mouseover,
                            mouseleave: fcn_mouseleave
                        });
                        this.mouseleaveBind = true;
                    }
                }
            });
        });

        function fcn_mouseover() {
            var that = this;
            if (!timeoutId) {
                timeoutId = window.setTimeout(function () {
                    timeoutId = null;
                    $(that).nextAll('.dropdownwrap:first').slideDown('400');
                }, 600);
            }
        }
        function fcn_mouseleave() {
            var that = this;
            if (timeoutId) {
                window.clearTimeout(timeoutId);
                timeoutId = null;
            }
            else {
                timeoutId = window.setTimeout(function () {
                    $(that).nextAll('.dropdownwrap:first').slideUp('400');
                }, 400);
            }
        }

        /* Info - END*/
    </script>
    <script type='text/javascript'>
        function GetRadioButtonListSelectedValue(radioButtonList) {
            var pnlState = $("#<%=((Panel)formViewTlProject_Organism.FindControl("pnlState")).ClientID%>");
            var pnlNad83 = $("#<%=((Panel)formViewTlProject_Organism.FindControl("pnlNad83")).ClientID%>");

            for (var i = 0; i < radioButtonList.rows[0].cells.length; ++i) {
                if (radioButtonList.rows[0].cells[i].firstChild.checked) {
                    if (radioButtonList.rows[0].cells[i].firstChild.value == '0') {
                        pnlNad83.show();
                        pnlState.hide();
                    }
                    else {
                        pnlState.show();
                        pnlNad83.hide();
                    }
                }
            }
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceholder" runat="Server">
    <%--   <asp:ScriptManager ID="ScriptManager2" runat="server">
       
    </asp:ScriptManager>--%>

    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="../CommonName_WS.asmx" />
        </Services>
    </ajaxToolkit:ToolkitScriptManager>
    <asp:Literal EnableViewState="False" runat="server" ID="ltlMessage"></asp:Literal>
    <asp:FormView SkinID="formViewTlProject_Organism" ID="formViewTlProject_Organism" runat="server" DataSourceID="odsProject_Organism_Details"
        DataKeyNames="ProjectOrganismID" EnableViewState="true" OnItemUpdating="FormViewTlProject_Organism_ItemUpdating"
        OnItemUpdated="FormViewTlProject_Organism_ItemUpdated" OnItemInserting="FormViewTlProject_Organism_ItemInserting"
        OnItemInserted="FormViewTlProject_Organism_ItemInserted" OnDataBound="FormViewTlProject_Organism_DataBound" OnDataBinding="FormViewTlProject_Organism_DataBinding">
        <EditItemTemplate>
            <h1 class="title-regular clearfix">
                <span class="grid_15 alpha">
                    <asp:Literal ID="Literal1" runat="server" Text='<%# treeDetail == null ? "Nuevo Árbol" : "Editar Detalles del Árbol " +  treeDetail.Number.ToString() %>'></asp:Literal>
                </span>
                <span class="grid_3 omega align-right">
                    <img src='<%#  webPath + "/App_Themes/Default/images/extras/loader-modal.gif" %> ' id="imgLoading" style="z-index: 2; display: none;">
                </span>
                &nbsp;&nbsp;</h1>
            <asp:Panel ID="pnlButtons1" runat="server" Visible="false">
                <asp:Button ID="btnTree0" runat="server" SkinID="Button" Visible="false" Width="240px" />
                <asp:Button ID="btnTree1" runat="server" SkinID="Button" Visible="false" Width="240px" />
                <asp:Button ID="btnTree2" runat="server" SkinID="Button" Visible="false" Width="240px" />
                <br />
            </asp:Panel>
            <asp:Panel ID="pnlButtons2" runat="server" Visible="false" Style="margin-top: 5px;">
                <asp:Button ID="btnTree3" runat="server" SkinID="Button" Visible="false" Width="240px" />
                <asp:Button ID="btnTree4" runat="server" SkinID="Button" Visible="false" Width="240px" />
                <asp:Button ID="btnTree5" runat="server" SkinID="Button" Visible="false" Width="240px" />
                <br />
            </asp:Panel>
            <br />
            <div class="grid_10 inline alpha">
                <strong>Número del Árbol:</strong>
                <asp:Label ID="lblNumber" Text='<%# treeDetail == null ? Page.RouteData.Values["number"] as string : treeDetail.Number.ToString() %>' runat="server" />
                <br />
                <br />
                <%--                <strong>X:</strong>
                <asp:Label ID="lblX" runat="server" CssClass="text" Text='<%# treeDetail == null ? Page.RouteData.Values["X"] as string : String.Format("{0:0.####################################}", treeDetail.X)  %>'></asp:Label><br />
                <strong>Y:</strong>
                <asp:Label ID="lblY" runat="server" CssClass="text" Text='<%# treeDetail == null ? Page.RouteData.Values["Y"] as string : String.Format("{0:0.####################################}", treeDetail.Y)  %>'></asp:Label><br />
                <asp:Panel runat="server" ID="pnlLatLng" Visible="false">
                    <strong>Latitud:</strong>
                    <asp:Label ID="lblLat" runat="server" CssClass="text" Text='<%# treeDetail == null ? Page.RouteData.Values["Lat"] as string : String.Format("{0:0.####################################}",  treeDetail.Lat)  %>'></asp:Label><br />
                    <strong>Longitud:</strong>
                    <asp:Label ID="lblLon" runat="server" CssClass="text" Text='<%# treeDetail == null ? Page.RouteData.Values["Lon"] as string : String.Format("{0:0.####################################}", treeDetail.Lon)  %>'></asp:Label><br />
                </asp:Panel>--%>
                <asp:RadioButtonList ID="rblPosition" runat="server" RepeatDirection="Horizontal" onclick="GetRadioButtonListSelectedValue(this);">
                    <asp:ListItem Text="NAD 83 (Planos Estatales de PR)&nbsp;&nbsp;" Selected="True" Value="0" />
                    <asp:ListItem Text="Latitud/Longitud" Value="1" />
                </asp:RadioButtonList>
                <asp:Panel ID="pnlNad83" Style="display: block;" runat="server">
                    <label>
                        Y:</label><br />
                    <asp:TextBox ID="txtY" runat="server" CssClass="text" ClientIDMode="Static" Text='<%# treeDetail == null ?  Page.RouteData.Values["Y"] as string  : String.Format("{0:0.####################################}",treeDetail.Y)  %>'></asp:TextBox>
                    <asp:CustomValidator CssClass="validator" ID="cvY" ControlToValidate="txtY" runat="server" ClientValidationFunction="ValidateRangeY" Display="Dynamic"><br />Favor de validar.</asp:CustomValidator>
                    <br />
                    <label>
                        X:</label><br />
                    <asp:TextBox ID="txtX" runat="server" CssClass="text" ClientIDMode="Static" Text='<%# treeDetail == null ?  Page.RouteData.Values["X"] as string  : String.Format("{0:0.####################################}",treeDetail.X)  %>'></asp:TextBox>
                    <asp:CustomValidator CssClass="validator" ID="cvX" ControlToValidate="txtX" runat="server" ClientValidationFunction="ValidateRangeX" Display="Dynamic"><br />Favor de validar.</asp:CustomValidator>
                    <br />
                </asp:Panel>
                <asp:Panel ID="pnlState" Style="display: none;" runat="server">
                    <label>
                        Latitud:</label><br />
                    <asp:TextBox ID="txtLat" runat="server" CssClass="text" ClientIDMode="Static" Text='<%# treeDetail == null ? Page.RouteData.Values["Lat"] as string : String.Format("{0:0.####################################}",treeDetail.Lat)  %>'></asp:TextBox>
                    <asp:CustomValidator CssClass="validator" ID="cvLat" ControlToValidate="txtLat" runat="server" ClientValidationFunction="ValidateRangeLat" Display="Dynamic"><br />Favor de validar.</asp:CustomValidator>
                    <br />
                    <label>
                        Longitud:</label><br />
                    <asp:TextBox ID="txtLon" runat="server" CssClass="text" ClientIDMode="Static" Text='<%# treeDetail == null ? Page.RouteData.Values["Lon"] as string : String.Format("{0:0.####################################}",treeDetail.Lon)  %>'></asp:TextBox>
                    <asp:CustomValidator CssClass="validator" ID="cvLon" ControlToValidate="txtLon" runat="server" ClientValidationFunction="ValidateRangeLon" Display="Dynamic"><br />Favor de validar.</asp:CustomValidator>
                    <br />
                </asp:Panel>
                <br />
                <label>Nombre Común:</label><br />
                <asp:TextBox ID="txtCommonName" Text='<%# organism != null ? organism.CommonName.CommonNameDesc : "" %>' runat="server" CssClass="text" MaxLength="100"></asp:TextBox>
                <asp:HiddenField ID="hfOrganism" runat="server" Value='<%# organism != null ? organism.OrganismID.ToString() : "" %>'></asp:HiddenField>
                <ajax:AutoCompleteExtender
                    runat="server"
                    ID="aceCommonName"
                    TargetControlID="txtCommonName"
                    ServicePath="../AutoComplete.asmx"
                    ServiceMethod="GetOrganismByFilter"
                    MinimumPrefixLength="1"
                    CompletionInterval="1000"
                    CompletionSetCount="30"
                    OnClientPopulating="populating"
                    OnClientPopulated="populated"
                    OnClientItemSelected="YourMethodHere" />
                <asp:RequiredFieldValidator ID="rfvCommonName" runat="server"
                    CssClass="validator" ControlToValidate="txtCommonName" Display="Dynamic">
<br />Requerido</asp:RequiredFieldValidator>
                <br />
                <label>Nombre Científico:</label><br />
                <asp:TextBox ID="txtScientificName" Text='<%# organism != null ? organism.ScientificName.ScientificNameDesc : "" %>' Style="background-color: #E3E9EF;" runat="server" CssClass="text" Enabled="false" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvScientificName" runat="server"
                    CssClass="validator" ControlToValidate="txtScientificName" Display="Dynamic">
<br />Requerido</asp:RequiredFieldValidator>
                <br />
                <label>
                    Es Cepa:</label><br />
                <asp:CheckBox ID="cbCepa" runat="server" CssClass="text" onchange="cbCepa_Click(this); return false;" ClientIDMode="Static"
                    Checked='<%# treeDetail == null ? false : (treeDetail.Varas > 0)  %>'></asp:CheckBox>
                <br />
                <div id="pnlVaras" <%# treeDetail == null || (treeDetail.Varas == 0) ? "style='display:none;'" : "style='display:block;'"%>>
                    <label>
                        Varas:</label><br />
                    <asp:TextBox ID="txtVaras" runat="server" CssClass="text" Width="100px"
                        Text='<%# treeDetail == null ? "0" : treeDetail.Varas.ToString()  %>'>
                    </asp:TextBox>
                    #
                    <br />
                    <asp:RangeValidator ID="rvVaras" runat="server" ClientIDMode="Static" MinimumValue="1" MaximumValue="1000" Type="Integer"
                        CssClass="validator" ControlToValidate="txtVaras" Display="Dynamic">Entrar un número válido
<br /></asp:RangeValidator>
                    <asp:RequiredFieldValidator ID="rfvVaras" runat="server" ClientIDMode="Static"
                        CssClass="validator" ControlToValidate="txtVaras" Display="Dynamic">Requerido
<br /></asp:RequiredFieldValidator>
                </div>
                <div id="pnlDap" <%# treeDetail == null || (treeDetail.Varas == 0) ? "style='display:block;'":"style='display:none;'"%>>
                    <div style="width: 100%; float: left">
                        <div style="width: 40%; float: left">
                            <label>
                                D.A.P (pulgadas):</label>
                            <img alt="Info: D.A.P" class="dropdown" src='<%#  webPath + "/App_Themes/Default/images/extras/CrystalClearInfoIcon.png" %>' style="height: 15px; width: 15px;" />
                            <br />
                            <asp:TextBox ID="txtDap" runat="server" Width="118px" ClientIDMode="Static" CssClass="text" Style="background-color: #E3E9EF;" Enabled="false"
                                Text='<%# treeDetail == null ? "0" :  treeDetail.Dap.ToString()   %>'>
                            </asp:TextBox>
                            ''
                            <asp:HiddenField ID="hfDap" ClientIDMode="Static" Value='<%# treeDetail == null ? "0" : GetDaps() %>' runat="server"></asp:HiddenField>
                            <asp:RequiredFieldValidator ID="rfvDap" runat="server" ClientIDMode="Static"
                                CssClass="validator" ControlToValidate="txtDap" Display="Dynamic">Requerido
<br /></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvDap" runat="server" MinimumValue="4" ClientIDMode="Static" MaximumValue="1000" Type="Double" Enabled='<%# treeDetail != null && (treeDetail.Varas > 0) ? false :  true %>'
                                CssClass="validator" ControlToValidate="txtDap" Display="Dynamic">
<br />Entrar un número válido. D.A.P debe ser mayor o igual a 4.
<br /></asp:RangeValidator><br />
                            <div id="divDap" class='notice dropdownwrap' style="display: block; z-index: 1; position: absolute;"></div>
                        </div>
                        <div style="width: 60%; float: right">
                            <label>
                                Troncos:</label><br />
                            <asp:TextBox ID="txtDapCounter" runat="server" Width="102px" CssClass="text" Style="background-color: #E3E9EF;" Enabled="false" ClientIDMode="Static" Text='<%# treeDetail == null ? "1" : treeDetail.Dap_Counter.ToString()  %>'></asp:TextBox>
                            #
                            <asp:RangeValidator ID="rvDapCounter" runat="server" ClientIDMode="Static" MinimumValue="1" MaximumValue="1000" Type="Integer" Enabled='<%# treeDetail != null && (treeDetail.Varas > 0) ? false :  true %>'
                                CssClass="validator" ControlToValidate="txtDapCounter" Display="Dynamic">
<br />Entrar un número válido
<br /></asp:RangeValidator><br />
                        </div>
                    </div>
                    <div style="text-align: left">
                        <asp:TextBox ID="txtDapAdd" ClientIDMode="Static" runat="server" MaxLength="5" Width="115px" CssClass="text"></asp:TextBox>
                        ''
                    &nbsp;                        
                        <asp:Button ID="btnAddDap" ClientIDMode="Static" runat="server" OnClientClick="btnAddTrunk_Click(this); return false;" Text="Añadir D.A.P" CausesValidation="false" SkinID="Button" />
                        &nbsp;<asp:Label ID="lblError" runat="server" CssClass="validator" ClientIDMode="Static" Style="display: none;"><br />Entrar un número válido. D.A.P debe ser mayor o igual a 4.</asp:Label>
                    </div>
                    <div id="POItablediv">
                        <table id="POITable" class="GridView">
                            <tr>
                                <td class="HeaderStyle" style="width: 50px;">D.A.P</td>
                                <td class="HeaderStyle" style="width: 60px;"></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="notice">
                                        No hay D.A.P.
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div style="width: 100%; float: left">
                    <label>
                        Altura (pies):</label><br />
                    <asp:TextBox ID="txtHeight" runat="server" Width="100px" CssClass="text" Text='<%# treeDetail == null ? "0" : String.Format("{0:0.##}", treeDetail.Height) %>'></asp:TextBox>
                    '<br />
                    <asp:RangeValidator ID="rvHeight" runat="server" MinimumValue="1" MaximumValue="1000" Type="Integer"
                        CssClass="validator" ControlToValidate="txtHeight" Display="Dynamic">Entrar un número válido<br /></asp:RangeValidator>
                    <asp:RequiredFieldValidator ID="rfvHeight" runat="server"
                        CssClass="validator" ControlToValidate="txtHeight" Display="Dynamic">Requerido<br /></asp:RequiredFieldValidator>
                </div>
                <br />
                <br />
                <div style="width: 100%; float: left">
                    <br />
                    <asp:CheckBox ID="chkLittoral" ClientIDMode="Static" runat="server" onclick="chkLittoral_click(this);" Checked='<%# treeDetail == null ? false : treeDetail.Littoral %>' Text="Árbol en la Servidumbre de Vigilancia de Litoral" Value="vl" ToolTip="Sección 47.5.6 Requisitos de siembra por corte de árboles en la servidumbre de vigilancia de litoral " />
                    <img alt="Info: Litoral" class="dropdown" src='<%#  webPath + "/App_Themes/Default/images/extras/CrystalClearInfoIcon.png" %>' style="height: 15px; width: 15px;" />
                    <div id="divLittoral" class='notice dropdownwrap'></div>
                    <br />
                    <asp:CheckBox ID="chkMaritimeZone" ClientIDMode="Static" runat="server" onclick="chkMaritimeZone_click(this);" Checked='<%# treeDetail == null ? false : treeDetail.MaritimeZone %>' Text="Árbol en la Zona Marítimo Terrestre " Value="mt" ToolTip="Sección 47.5.7 Requisitos de siembra por corte de árboles en la Zona Marítimo Terrestre " />
                    <img alt="Info: Zona Marítimo Terrestre" class="dropdown" src='<%#  webPath + "/App_Themes/Default/images/extras/CrystalClearInfoIcon.png" %>' style="height: 15px; width: 15px;" />
                    <div id="divMaritimeZone" class='notice dropdownwrap'></div>
                    <br />
                    <br />
                </div>
            </div>
            <div class="grid_9 inline omega">
                <label>
                    Comentarios:</label><br />
                <asp:TextBox ID="txtCommentary" runat="server" CssClass="text" Text='<%# treeDetail == null ? "" : treeDetail.Commentary %>'
                    MaxLength='<%# Convert.ToInt32(ConfigurationManager.AppSettings["CommentaryMaxLength"])  %>' TextMode="MultiLine">
                </asp:TextBox><br />
                <asp:Panel ID="pnlDates" runat="server" Visible='<%# treeDetail != null %>'>
                    <strong>Creado Por:</strong>
                    <asp:Label ID="lblEditedUserId" Text='<%#  treeDetail == null ? "" : creator.UserName %>' runat="server" />
                    <br />
                    <strong>Date Created:</strong>
                    <asp:Label ID="lblEditedDate" Text='<%#  treeDetail == null ? "" : treeDetail.CreatedDate.ToString("MMM/dd/yyyy h:mm:ss tt")%>' runat="server" />
                    <br />
                    <strong>Editado Por:</strong>
                    <asp:Label ID="Label1" Text='<%#  treeDetail == null ? "" :  editor.UserName %>' runat="server" />
                    <br />
                    <strong>Editado En:</strong>
                    <asp:Label ID="Label2" Text='<%# treeDetail == null ? "" : treeDetail.EditedDate.ToString("MMM/dd/yyyy h:mm:ss tt") %>' runat="server" />
                </asp:Panel>
                <br />
                <div style="width: 100%; float: left">
                    <div style="width: 60%; float: left">
                        <label>
                            Seleccionar Acción Propuesta:</label><br />
                        <asp:RadioButtonList ID="rblActionProposed" runat="server" AppendDataBoundItems="true" RepeatDirection="Vertical"
                            AutoPostBack="false" DataSourceID="odsActionProposedList" DataTextField="ActionProposedDesc"
                            DataValueField="ActionProposedID" EnableViewState="true" OnDataBound="rblActionProposed_DataBound">
                        </asp:RadioButtonList>
                        <asp:ObjectDataSource ID="odsActionProposedList" runat="server" TypeName="Eisk.BusinessLogicLayer.ActionProposedBLL"
                            EnableViewState="true" SelectMethod="GetAllActionProposeds" />
                    </div>
                    <div style="width: 40%; float: left">
                        <label>
                            Seleccionar Condición:</label><br />
                        <asp:RadioButtonList ID="rblCondition" runat="server" AppendDataBoundItems="true" RepeatDirection="Vertical"
                            AutoPostBack="false" DataSourceID="odsConditionList" DataTextField="ConditionDesc"
                            DataValueField="ConditionID" EnableViewState="true" OnDataBound="rblCondition_DataBound">
                        </asp:RadioButtonList>
                        <asp:ObjectDataSource ID="odsConditionList" runat="server" TypeName="Eisk.BusinessLogicLayer.ConditionBLL"
                            EnableViewState="true" SelectMethod="GetAllConditions" />
                    </div>
                </div>
            </div>
            <hr />
            <p>
                <asp:Button ID="Button1" runat="server" Text="Guardar" OnClick="ButtonSave_Click" SkinID="Button" />
                <asp:Button ID="Button3" CausesValidation="false" runat="server" Text="Volver" OnClick="ButtonGoToViewPage_Click"
                    SkinID="AltButton" />
            </p>
            <em>Los campos requeridos están marcados con un <span class="required-field-indicator">*</span></em>
            <asp:Panel runat="server" ID="pnlLoading" CssClass="loading-visible" Visible="false">
            </asp:Panel>
        </EditItemTemplate>
    </asp:FormView>
    <asp:ObjectDataSource ID="odsProject_Organism_Details" runat="server" SelectMethod="GetProject_OrganismsByProjectOrganismID"
        InsertMethod="CreateNewProject_Organisms" UpdateMethod="UpdateProject_Organisms" DataObjectTypeName="Eisk.BusinessEntities.Project_Organisms"
        TypeName="Eisk.BusinessLogicLayer.Project_OrganismsBLL" OnSelecting="OdsProject_Organism_Details_Selecting"
        OnInserted="OdsProject_Organism_Details_Inserted">
        <UpdateParameters>
            <asp:ControlParameter Name="projectOrganismID" ControlID="formViewTlProject_Organism" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="projectOrganismID" ConvertEmptyStringToNull="true" />
        </InsertParameters>
        <SelectParameters>
            <asp:RouteParameter Name="projectOrganismID" RouteKey="project_organism_id" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

