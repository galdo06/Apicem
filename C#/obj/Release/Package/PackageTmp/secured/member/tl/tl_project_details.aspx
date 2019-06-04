<%@ Page Language="C#" MasterPageFile="~/App_Resources/master.master" AutoEventWireup="false"
    Inherits="Tl_Project_Details" Title="Corte y Poda | N@TURA" MetaKeywords="Corte,Poda"
    MetaDescription="Detalles de Corte y Poda para el Proyecto."
    CodeBehind="tl_project_details.aspx.cs" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <script type="text/javascript" src='<%# ResolveUrl ("~/Scripts/jquery.tipTip.minified.js") %>'></script>
    <link href='<%# ResolveUrl ("~/Content/tipTip/tipTip.css") %>' rel="stylesheet" />
    <style type="text/css">
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
    </style>
    <script type="text/javascript">
        var timeoutId;

        $(function () {
            var apiLocation = document.domain == 'localhost' ? "" : "/web";
            var imgLoc = apiLocation + "/App_Themes/Default/images/extras/CrystalClearInfoIcon.png";

            // reset img src
            $('.qaimg').attr('src', imgLoc);

            //$("#BodyContentPlaceholder_formViewTlProject_txtX").mask("999999?.999999999999");
            //$("#BodyContentPlaceholder_formViewTlProject_txtY").mask("999999?.999999999999");
            //$("#BodyContentPlaceholder_formViewTlProject_txtLat").mask("999999?.999999999999");
            //$("#BodyContentPlaceholder_formViewTlProject_txtLon").mask("999999?.999999999999");
            $("#BodyContentPlaceholder_formViewTlProject_txtParkings").mask("9?9999");
            $("#BodyContentPlaceholder_formViewTlProject_txtLots").mask("9?9999");
            //$("#BodyContentPlaceholder_formViewTlProject_txtAcres").mask("9?9999.99");
            $("#BodyContentPlaceholder_formViewTlProject_txtDistanceBetweenTrees").mask("9?9999");
            $(".tiptip").tipTip({ maxWidth: "auto", edgeOffset: 10, defaultPosition: "right" });



            $("#divSocialInterest").load(apiLocation + "/App_Resources/html/SocialInterest.html");
            $("#divParkings").load(apiLocation + "/App_Resources/html/Parkings.html");
            $("#divDistance").load(apiLocation + "/App_Resources/html/Distance.html");
            $("#divPreviouslyImpacted").load(apiLocation + "/App_Resources/html/PreviouslyImpacted.html");
            $.each($(".acres"), function (index, value) {
                $(value).load(apiLocation + "/App_Resources/html/Acres.html");
            });
            $.each($(".segregation"), function (index, value) {
                $(value).load(apiLocation + "/App_Resources/html/Segregation.html");
            });
            $.each($(".unifamiliar"), function (index, value) {
                $(value).load(apiLocation + "/App_Resources/html/Unifamiliar.html");
            });
            $.each($(".multifamiliar"), function (index, value) {
                $(value).load(apiLocation + "/App_Resources/html/Multifamiliar.html");
            });
            $.each($(".commercial"), function (index, value) {
                $(value).load(apiLocation + "/App_Resources/html/Commercial.html");
            });

            $.each($(".lotConserv"), function (index, value) {
                $(value).load(apiLocation + "/App_Resources/html/LotConserv.html");
            });
            $.each($(".lotCut"), function (index, value) {
                $(value).load(apiLocation + "/App_Resources/html/LotCut.html");
            });


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

        function rblPosition_click(_checkBox) {
            var val = "";
            val = $('#rblPosition').find('input:checked').val();
            if (val != "" && val != undefined && val == 1) {
                $('#pnlState').show();
                $('#pnlNad83').hide();
            } else {
                $("#rblPosition").attr('disabled', '');
                $('#pnlState').hide();
                $('#pnlNad83').show();
            }
        }

        function rblPreviouslyImpacted_click(_checkBox) {
            var val = "";
            val = $('#rblPreviouslyImpacted').find('input:checked').val();
            if (val != "" && val != undefined && val == 1) {
                $("#ddlDistanceBetweenTrees").attr('disabled', 'disabled');
                $('#divDistanceBetweenTrees').hide();
            } else {
                $("#ddlDistanceBetweenTrees").attr('disabled', '');
                $('#divDistanceBetweenTrees').show();
            }
        }

        function rblCalc_click(_checkBox) {
            var val = "";
            val = $('#rblCalc').find('input:checked').val();
            if (val != "" && val != undefined && val <= 1) {
                $('#divPerimeter').hide();
                $('#divLots').show();
            }
            else {
                $('#divPerimeter').show();
                $('#divLots').hide();
            }
        }

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

        function dropdownwrapSlideToggle() {
            $(this).nextAll('.dropdownwrap:first').slideToggle();
        }
        function ValidateRangeY(sender, args) {
            var min = '<%#  System.Configuration.ConfigurationManager.AppSettings["YMinValue"] %>'
            var max = '<%#  System.Configuration.ConfigurationManager.AppSettings["YMaxValue"] %>'
            var value = $("#BodyContentPlaceholder_formViewTlProject_txtY").val();
            args.IsValid = validateTextNumericInRange(value, min, max) || parseFloat(value) == 0;
        }
        function ValidateRangeX(sender, args) {
            var min = '<%#  System.Configuration.ConfigurationManager.AppSettings["XMinValue"] %>'
            var max = '<%#  System.Configuration.ConfigurationManager.AppSettings["XMaxValue"] %>'
            var value = $("#BodyContentPlaceholder_formViewTlProject_txtX").val();
            args.IsValid = validateTextNumericInRange(value, min, max) || parseFloat(value) == 0;
        }
        function ValidateRangeLat(sender, args) {
            var min = '<%#  System.Configuration.ConfigurationManager.AppSettings["LatMinValue"] %>';
            var max = '<%#  System.Configuration.ConfigurationManager.AppSettings["LatMaxValue"] %>'
            var value = $("#BodyContentPlaceholder_formViewTlProject_txtLat").val();
            args.IsValid = validateTextNumericInRange(value, min, max) || parseFloat(value) == 0;
        }
        function ValidateRangeLon(sender, args) {
            var min = '<%#  System.Configuration.ConfigurationManager.AppSettings["LonMinValue"] %>';
            var max = '<%#  System.Configuration.ConfigurationManager.AppSettings["LonMaxValue"] %>';
            var value = $("#BodyContentPlaceholder_formViewTlProject_txtLon").val();
            args.IsValid = validateTextNumericInRange(value, min, max) || parseFloat(value) == 0;
        }

        function validateTextNumericInRange(textInput, min, max) {
            var value = Number(textInput);
            return (!isNaN(value) && value >= min && value <= max);
        }
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContentPlaceholder" runat="Server">
    <asp:ScriptManager ID="ScriptManager2" runat="server">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="upd" runat="server">
        <ContentTemplate>
            <asp:Literal EnableViewState="False" runat="server" ID="ltlMessage"></asp:Literal>
            <asp:FormView SkinID="FormView" ID="formViewTlProject" runat="server" DataSourceID="odsProject_Details"
                DataKeyNames="ProjectId" EnableViewState="true" OnItemUpdating="FormViewTlProject_ItemUpdating"
                OnItemUpdated="FormViewTlProject_ItemUpdated" OnItemInserting="FormViewTlProject_ItemInserting"
                OnItemInserted="FormViewTlProject_ItemInserted" OnDataBound="FormViewTlProject_DataBound" OnDataBinding="FormViewTlProject_DataBinding">
                <ItemTemplate>
                    <h1 class="title-regular clearfix">
                        <span class="grid_15 alpha">Detalles de Corte y Poda para el Proyecto</span> <span class="grid_3 omega align-right"></span>
                    </h1>
                    <div class="grid_10 inline alpha" style="width: 465px;">
                        <strong>
                            <asp:Label runat="server" Text="Datos Generales del Proyecto:" Style="text-decoration: underline; font-size: 15px;"></asp:Label></strong>
                        <br />
                        <br />
                        <strong>Tipo de Desarrollo:</strong>
                        <asp:Label ID="lblCalc" Text='<%# projectInfoTreeLocation == null ? "" : Eisk.BusinessLogicLayer.ProjectInfoTreeLocationBLL.GetTipoDesarrollo(projectInfoTreeLocation.Mitigation ) %>' runat="server" />
                        <br />
                        <strong>Proyecto de Interés Social:</strong>
                        <asp:Label ID="lblSocialInterest" Text='<%# ( projectInfoTreeLocation == null ? "No" : (projectInfoTreeLocation.SocialInterest ==null || !(bool)projectInfoTreeLocation.SocialInterest ? "No":"Si"))  %>' runat="server" />
                        <br />
                        <strong>Total de Cuerdas en el Proyecto:</strong>
                        <asp:Label ID="Label2" Text='<%# projectInfoTreeLocation == null ? "0.00" : projectInfoTreeLocation.Acres.ToString() %>' runat="server" />
                        <br />
                        <strong>Estacionamientos:</strong>
                        <asp:Label ID="lblParking" Text='<%# projectInfoTreeLocation == null ? 0 : projectInfoTreeLocation.Parkings  %>' runat="server" />
                        <br />
                        <strong>Mitigación por Perímetro o Solares:</strong>
                        <asp:Label ID="Label7" Text='<%# Eisk.BusinessLogicLayer.ProjectInfoTreeLocationBLL.GetMitigacion(projectInfoTreeLocation)  %>' runat="server" />
                        <br />
                        <div id="divPerimeter" <%# "style='display:" + (projectInfoTreeLocation == null ? "block" : (projectInfoTreeLocation.Mitigation > 1 ? "block" : "none" )) + "';" %>>
                            <strong>Proyecto Previamente Impactado:</strong>
                            <asp:Label ID="lblPreviouslyImpacted" Text='<%# ( projectInfoTreeLocation == null ? "No" : (projectInfoTreeLocation.PreviouslyImpacted ==null || !(bool)projectInfoTreeLocation.PreviouslyImpacted ? "No":"Si")) %>' runat="server" />
                            <br />
                            <div id="divDistanceBetweenTrees" <%# "style='display:" + (projectInfoTreeLocation == null ? "block" : (projectInfoTreeLocation.PreviouslyImpacted ==null || !(bool)projectInfoTreeLocation.PreviouslyImpacted ? "block" : "none" )) + "';" %>>
                                <strong>Distancia entre Árboles:</strong>
                                <asp:Label ID="lblDistanceBetweenTrees" Text='<%# projectInfoTreeLocation == null ? 0 : projectInfoTreeLocation.DistanceBetweenTrees  %>' runat="server" />
                                <br />
                            </div>
                            <br />
                        </div>
                        <div id="divLots" <%# "style='display:" + (projectInfoTreeLocation == null ? "none" : (projectInfoTreeLocation.Mitigation > 1 ? "none" : "block" )) + "';" %>>
                            <br />
                            <strong>
                                <asp:Label runat="server" Text="Solares:" Style="text-decoration: underline; font-size: 15px;"></asp:Label></strong>
                            <br />
                            <br />
                            <strong>Total de Solares:</strong>
                            <asp:Label ID="lblLots" Text='<%# projectInfoTreeLocation == null ? "0" : (projectInfoTreeLocation.Lots0 != null ? (projectInfoTreeLocation.Lots0 + projectInfoTreeLocation.Lots1 + projectInfoTreeLocation.Lots2 + projectInfoTreeLocation.Lots3).ToString() : "0" )  %>' runat="server" />
                            <br />
                            <strong>Solares con al menos un árbol a conservarse:</strong>
                            <asp:Label ID="lblLots0" Text='<%# projectInfoTreeLocation == null ? "0" : (projectInfoTreeLocation.Lots0 != null ? projectInfoTreeLocation.Lots0.ToString() : "0" )  %>' runat="server" />
                            <br />
                            <strong>Solares con un (1) árbol a sembrarse según tamaño de cabida del solar:</strong>
                            <asp:Label ID="lblLots1" Text='<%# projectInfoTreeLocation == null ? "0" : (projectInfoTreeLocation.Lots1 != null ? projectInfoTreeLocation.Lots1.ToString() : "0" )  %>' runat="server" />
                            <br />
                            <strong>Solares con dos (2) árbol a sembrarse según tamaño de cabida del solar:</strong>
                            <asp:Label ID="lblLots2" Text='<%# projectInfoTreeLocation == null ? "0" : (projectInfoTreeLocation.Lots2 != null ? projectInfoTreeLocation.Lots2.ToString() : "0" )  %>' runat="server" />
                            <br />
                            <strong>Solares con tres (3) árbol a sembrarse según tamaño de cabida del solar:</strong>
                            <asp:Label ID="lblLots3" Text='<%# projectInfoTreeLocation == null ? "0" : (projectInfoTreeLocation.Lots3 != null ? projectInfoTreeLocation.Lots3.ToString() : "0" )  %>' runat="server" />
                            <br />
                            <br />
                        </div>
                        <strong>
                            <asp:Label runat="server" Text="Localización del Proyecto" Style="text-decoration: underline; font-size: 15px;"></asp:Label></strong>
                        <br />
                        <br />
                        <strong>X:</strong>
                        <asp:Label ID="lblX" Text='<%# projectInfoTreeLocation == null ? "0.0" : String.Format("{0:0.####################################}",projectInfoTreeLocation.X)  %>' runat="server" />
                        <br />
                        <strong>Y:</strong>
                        <asp:Label ID="lblY" Text='<%# projectInfoTreeLocation == null ? "0.0" : String.Format("{0:0.####################################}",projectInfoTreeLocation.Y)  %>' runat="server" />
                        <br />
                        <strong>Latitud:</strong>
                        <asp:Label ID="lblLatitud" Text='<%# projectInfoTreeLocation == null ? "0.0" : String.Format("{0:0.####################################}",projectInfoTreeLocation.Lat)  %>' runat="server" />
                        <br />
                        <strong>Longitud:</strong>
                        <asp:Label ID="lblLongitud" Text='<%# projectInfoTreeLocation == null ? "0.0" : String.Format("{0:0.####################################}",projectInfoTreeLocation.Lon)  %>' runat="server" />
                        <br />
                        <br />
                    </div>
                    <div class="grid_9 inline omega" style="width: 250px;">
                        <br />
                        <strong>Creado Por:</strong>
                        <asp:Label ID="Label3" Text='<%#  Eval("CreatorUserID") == null ? "" : creator.UserName %>' runat="server" />
                        <br />
                        <strong>Creado En:</strong>
                        <asp:Label ID="Label4" Text='<%# Eval("CreatedDate", "{0:MMM/dd/yyyy h:mm:ss tt}")%>' runat="server" />
                        <br />
                        <strong>Editado Por:</strong>
                        <asp:Label ID="Label5" Text='<%#  Eval("EditorUserID") == null ? "" : editor.UserName %>' runat="server" />
                        <br />
                        <strong>Editado En:</strong>
                        <asp:Label ID="Label6" Text='<%# Eval("EditedDate", "{0:MMM/dd/yyyy h:mm:ss tt}")%>' runat="server" />
                        <br />
                        <br />
                    </div>
                    <br />
                    <br />
                    <hr />
                    <%--<asp:Button CausesValidation="false" ID="btnBack" runat="server" Text="Volver a Proyectos" OnClick="ButtonGoToListPage_Click" SkinID="AltButton" />--%>
                    <asp:Button ID="btnEditTreeLocation" runat="server" Text="Editar Detalles de Corte y Poda" OnClick="ButtonEditProjectInfoTreeLocation_Click" SkinID="Button" />
                </ItemTemplate>
                <EditItemTemplate>
                    <h1 class="title-regular clearfix">
                        <span class="grid_15 alpha">Editar Detalles de Corte y Poda</span> </h1>
                    <div class="grid_10 inline alpha" style="width: 100%;">
                        <strong>Nombre del Proyecto:</strong>
                        <asp:Label ID="Label1" Text='<%# projectInfo.ProjectName %>' runat="server" />
                        <br />
                        <strong>Editado Por:</strong>
                        <asp:Label ID="lblEditedUserId" Text='<%#  Eval("EditorUserID") == null ? "" : editor.UserName %>' runat="server" />
                        <br />
                        <strong>Editado En:</strong>
                        <asp:Label ID="lblEditedDate" Text='<%# Eval("EditedDate", "{0:MMM/dd/yyyy h:mm:ss tt}")%>' runat="server" />
                        <br />
                        <br />
                        <br />
                        <strong>Datos Generales del Proyecto:</strong>
                        <br />
                        <br />
                        <label>Tipo de Desarrollo:</label>
                        <asp:RadioButtonList ID="rblCalc" runat="server" RepeatDirection="Vertical" ClientIDMode="Static"
                            Style="margin-bottom: 0px;" SelectedValue="<%# projectInfoTreeLocation == null ? 3 : projectInfoTreeLocation.Mitigation %>">
                            <asp:ListItem Value="0" onclick="rblCalc_click(this); return true;">
                                Segregaciones (Mitigación por solares):&nbsp;
                                <img alt="Info: Segregaciones" class="dropdown qaimg"   style="height: 15px; width: 15px;" />
                                <div class='notice segregation dropdownwrap'></div>
                            </asp:ListItem>
                            <asp:ListItem Value="1" onclick="rblCalc_click(this); return true;">
                                Residenciales Unifamiliares (Mitigación por solares):&nbsp;
                                <img alt="Info: Residenciales Unifamiliares" class="dropdown qaimg"   style="height: 15px; width: 15px;" />
                                <div id="divUnifamiliar" class='notice unifamiliar dropdownwrap'></div>
                            </asp:ListItem>
                            <asp:ListItem Value="2" onclick="rblCalc_click(this); return true;">
                                Residenciales Multifamiliares (Mitigación por perímetro):&nbsp;
                                <img alt="Info: Residenciales Multifamiliares" class="dropdown qaimg"  style="height: 15px; width: 15px;" />
                                <div id="divMultifamiliar" class='notice multifamiliar dropdownwrap'></div>
                            </asp:ListItem>
                            <asp:ListItem Value="3" onclick="rblCalc_click(this); return true;">
                                Desarrollo Comercial, Industrial o Institucional (Mitigación por perímetro):&nbsp;
                                <img alt="Info: Desarrollo Comercial, Industrial o Institucional" class="dropdown qaimg"  style="height: 15px; width: 15px;" />
                                <div id="divCommercial" class='notice commercial dropdownwrap'></div>
                            </asp:ListItem>
                        </asp:RadioButtonList>
                        <br />
                        <label>Proyecto de Interés Social:</label>
                        <img alt="Info: Proyecto de Interés Social" class="dropdown qaimg" style="height: 15px; width: 15px;" />
                        <div id="divSocialInterest" class='notice dropdownwrap'>
                        </div>
                        <asp:RadioButtonList ID="rblSocialInterest" ClientIDMode="Static" runat="server" RepeatDirection="Vertical" Style="margin-bottom: 0px;" SelectedValue='<%# ( projectInfoTreeLocation == null ? "1" : (projectInfoTreeLocation.SocialInterest ==null || !(bool)projectInfoTreeLocation.SocialInterest ? "0":"1")) %>'>
                            <asp:ListItem Text="No" Value="0" />
                            <asp:ListItem Text="Sí&nbsp;" Value="1" />
                        </asp:RadioButtonList>
                        <br />
                        <label>Total de Cuerdas en el Proyecto:</label>
                        <img alt="Info: Total de Cuerdas en el Proyecto" class="dropdown qaimg" style="height: 15px; width: 15px;" />
                        <div id="divAcres1" class='acres dropdownwrap'></div>
                        <br />
                        <asp:TextBox ID="txtAcres" runat="server" Width="40px" CssClass="text" MaxLength="10" Text='<%# projectInfoTreeLocation == null ? "0.00" : projectInfoTreeLocation.Acres.ToString()  %>'></asp:TextBox>
                        <asp:CompareValidator CssClass="validator" ID="cvAcres" ControlToValidate="txtAcres" runat="server" Operator="DataTypeCheck" Type="Currency" Display="Dynamic"><br />Favor de validar.</asp:CompareValidator>
                        <br />
                        <br />
                        <label class="tiptip" title="R.C.P. Sección 47.5.4 Requisitos de siembra por concepto de estacionamiento:<br /><br />Cuando un solar requiera cuatro (4) o más espacios de estacionamiento al aire<br />libre, se sembrará un (1) árbol por cada cuatro (4) espacios de estacionamiento.">
                            Estacionamientos:</label>
                        <img alt="Info: Estacionamientos" class="dropdown qaimg" style="height: 15px; width: 15px;" />
                        <div id="divParkings" class='notice dropdownwrap'></div>
                        <br />
                        <asp:TextBox ID="txtParkings" Width="40px" runat="server" CssClass="text" Text='<%# projectInfoTreeLocation == null ? "0" : projectInfoTreeLocation.Parkings.ToString()  %>'></asp:TextBox>
                        <br />
                        <div id="divPerimeter" <%# "style='display:" + (projectInfoTreeLocation == null ? "block" : (projectInfoTreeLocation.Mitigation > 1 ? "block" : "none" )) + "';" %>>
                            <br />
                            <strong>Mitigación por Perímetro:</strong><br />
                            <br />
                            <label>Proyecto Previamente Impactado:</label>
                            <img alt="Info: Proyecto Previamente Impactado" class="dropdown qaimg" style="height: 15px; width: 15px;" />
                            <div id="divPreviouslyImpacted" class='notice dropdownwrap'>
                            </div>
                            <asp:RadioButtonList ID="rblPreviouslyImpacted" ClientIDMode="Static" runat="server" RepeatDirection="Vertical" Style="margin-bottom: 0px;" SelectedValue='<%# ( projectInfoTreeLocation == null ? "0" : (projectInfoTreeLocation.PreviouslyImpacted ==null || !(bool)projectInfoTreeLocation.PreviouslyImpacted ? "0":"1")) %>'>
                                <asp:ListItem Text="No (Esta opción utiliza el Total de Cuerdas en el Proyecto para calcular la mitigación)" Value="0" onclick="rblPreviouslyImpacted_click(this); return true;" />
                                <asp:ListItem Text="Sí&nbsp;" Value="1" onclick="rblPreviouslyImpacted_click(this); return true;" />
                            </asp:RadioButtonList>
                            <br />
                            <div id="divDistanceBetweenTrees" <%# "style='display:" + (projectInfoTreeLocation == null ? "block" : (projectInfoTreeLocation.PreviouslyImpacted ==null || !(bool)projectInfoTreeLocation.PreviouslyImpacted ? "block" : "none" )) + "';" %>>

                                <label>
                                    Distancia entre Árboles:&nbsp;</label><img alt="Info: Distancia entre Árboles" class="dropdown qaimg" style="height: 15px; width: 15px;" />
                                <div id="divDistance" class='notice dropdownwrap'></div>
                                <br />
                                <asp:DropDownList ID="ddlDistanceBetweenTrees" ClientIDMode="Static" runat="server" Enabled='<%# ( projectInfoTreeLocation == null ? true : (projectInfoTreeLocation.PreviouslyImpacted ==null || !(bool)projectInfoTreeLocation.PreviouslyImpacted ? true:false)) %>' AutoPostBack="false">
                                    <asp:ListItem Text="10'" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="20'" Value="20"></asp:ListItem>
                                    <asp:ListItem Text="30'" Value="30"></asp:ListItem>
                                </asp:DropDownList>
                                <br />
                            </div>
                        </div>
                        <div id="divLots" <%# "style='display:" + (projectInfoTreeLocation == null ? "none" : (projectInfoTreeLocation.Mitigation > 1 ? "none" : "block" )) + "';" %>>
                            <br />
                            <strong>Mitigación por Solares:</strong><br />
                            <br />
                            Solares con al menos un árbol a conservarse (Acción Propuesta: Protección o Poda):
                            <img alt="Info: Cálcular Mitigación por Segregación" class="dropdown qaimg" style="height: 15px; width: 15px;" />
                            <br />
                            <div class='lotConserv dropdownwrap'></div>
                            <asp:TextBox ID="txtLots0" Width="40px" runat="server" CssClass="text" Text='<%# projectInfoTreeLocation == null ? "0" : (projectInfoTreeLocation.Lots0 != null ? projectInfoTreeLocation.Lots0.ToString() : "0" )  %>'></asp:TextBox>
                            <asp:RangeValidator ID="rvLots0" runat="server" MinimumValue="0" MaximumValue="99999999999999999999" Type="Double"
                                CssClass="validator" ControlToValidate="txtLots0" Display="Dynamic">Entrar un número válido.<br /></asp:RangeValidator>
                            <br />
                            Solares con un (1) árbol a sembrarse según tamaño de cabida del solar:
                            <img alt="Info: Cálcular Mitigación por Segregación" class="dropdown qaimg" style="height: 15px; width: 15px;" />
                            <br />
                            <div class='lotCut dropdownwrap'></div>
                            <asp:TextBox ID="txtLots1" Width="40px" runat="server" CssClass="text" Text='<%# projectInfoTreeLocation == null ? "0" : (projectInfoTreeLocation.Lots1 != null ? projectInfoTreeLocation.Lots1.ToString() : "0" )  %>'></asp:TextBox>
                            <asp:RangeValidator ID="rvLots1" runat="server" MinimumValue="0" MaximumValue="99999999999999999999" Type="Double"
                                CssClass="validator" ControlToValidate="txtLots1" Display="Dynamic"><br />Entrar un número válido.</asp:RangeValidator>
                            <br />
                            Solares con dos (2) árboles a sembrarse según tamaño de cabida del solar:
                            <img alt="Info: Cálcular Mitigación por Segregación" class="dropdown qaimg" style="height: 15px; width: 15px;" />
                            <br />
                            <div class='lotCut dropdownwrap'></div>
                            <asp:TextBox ID="txtLots2" Width="40px" runat="server" CssClass="text" Text='<%# projectInfoTreeLocation == null ? "0" : (projectInfoTreeLocation.Lots2 != null ? projectInfoTreeLocation.Lots2.ToString() : "0" )  %>'></asp:TextBox>
                            <asp:RangeValidator ID="rvLot2" runat="server" MinimumValue="0" MaximumValue="99999999999999999999" Type="Double"
                                CssClass="validator" ControlToValidate="txtLots2" Display="Dynamic"><br />Entrar un número válido.</asp:RangeValidator><br />
                            Solares con tres (3) árboles a sembrarse según tamaño de cabida del solar:
                            <img alt="Info: Cálcular Mitigación por Segregación" class="dropdown qaimg" style="height: 15px; width: 15px;" />
                            <br />
                            <div class='lotCut dropdownwrap'></div>
                            <asp:TextBox ID="txtLots3" Width="40px" runat="server" CssClass="text" Text='<%# projectInfoTreeLocation == null ? "0"  : (projectInfoTreeLocation.Lots3 != null ? projectInfoTreeLocation.Lots3.ToString() : "0" )  %>'></asp:TextBox>
                            <asp:RangeValidator ID="rvLot3" runat="server" MinimumValue="0" MaximumValue="99999999999999999999" Type="Double"
                                CssClass="validator" ControlToValidate="txtLots3" Display="Dynamic"><br />Entrar un número válido.</asp:RangeValidator>
                            <br />
                        </div>
                        <br />
                        <strong>Localización del Proyecto:</strong><br />
                        <br />
                        <asp:RadioButtonList ID="rblPosition" ClientIDMode="Static" runat="server" RepeatDirection="Vertical" Style="margin-bottom: 0px;">
                            <asp:ListItem Text="Calcular localización utilizando datum geodésico NAD 83 (Planos Estatales de PR)&nbsp;&nbsp;" Selected="True" Value="0" onclick="rblPosition_click(this); return true;" />
                            <asp:ListItem Text="Calcular localización utilizando Latitud/Longitud" Value="1" onclick="rblPosition_click(this); return true;" />
                        </asp:RadioButtonList>
                        <br />
                        <div id="pnlNad83" style='display: block'>
                            <label>
                                Y:</label><br />
                            <asp:TextBox ID="txtY" runat="server" CssClass="text" Text='<%# projectInfoTreeLocation == null ? "0.0" : String.Format("{0:0.####################################}",projectInfoTreeLocation.Y)  %>'></asp:TextBox>
                            <asp:CustomValidator CssClass="validator" ID="cvY" ControlToValidate="txtY" runat="server" ClientValidationFunction="ValidateRangeY" Display="Dynamic"><br />Favor de validar.</asp:CustomValidator><br />
                            <label>
                                X:</label><br />
                            <asp:TextBox ID="txtX" runat="server" CssClass="text" Text='<%# projectInfoTreeLocation == null ? "0.0" : String.Format("{0:0.####################################}",projectInfoTreeLocation.X)  %>'></asp:TextBox>
                            <asp:CustomValidator CssClass="validator" ID="cvX" ControlToValidate="txtX" runat="server" ClientValidationFunction="ValidateRangeX" Display="Dynamic"><br />Favor de validar.</asp:CustomValidator><br />
                        </div>
                        <div id="pnlState" style='display: none'>
                            <label>
                                Latitud:</label><br />
                            <asp:TextBox ID="txtLat" runat="server" CssClass="text" Text='<%# projectInfoTreeLocation == null ? "0.0" : String.Format("{0:0.####################################}",projectInfoTreeLocation.Lat)  %>'></asp:TextBox>
                            <asp:CustomValidator CssClass="validator" ID="cvLat" ControlToValidate="txtLat" runat="server" ClientValidationFunction="ValidateRangeLat" Display="Dynamic"><br />Favor de validar.</asp:CustomValidator><br />
                            <label>
                                Longitud:</label><br />
                            <asp:TextBox ID="txtLon" runat="server" CssClass="text" Text='<%# projectInfoTreeLocation == null ? "0.0" : String.Format("{0:0.####################################}",projectInfoTreeLocation.Lon)  %>'></asp:TextBox>
                            <asp:CustomValidator CssClass="validator" ID="cvLon" ControlToValidate="txtLon" runat="server" ClientValidationFunction="ValidateRangeLon" Display="Dynamic"><br />Favor de validar.</asp:CustomValidator><br />
                        </div>
                        <br />
                    </div>
                    <%--            <div class="grid_9 inline omega">
                <asp:Panel ID="pnlDates" runat="server" Visible='<%#  Eval("CreatorUserID") != null %>'>
                </asp:Panel>
                <br />
                <br />
            </div>--%>
                    <hr />
                    <p>
                        <asp:Button ID="Button1" runat="server" Text="Guardar" OnClick="ButtonSave_Click" SkinID="Button" />
                        <asp:Button ID="Button3" CausesValidation="false" runat="server" Text="Volver" OnClick="ButtonGoToViewPage_Click"
                            SkinID="AltButton" />
                    </p>
                    <em>Los campos requeridos están marcados con un <span class="required-field-indicator">*</span></em>


                </EditItemTemplate>
            </asp:FormView>
            <asp:ObjectDataSource ID="odsProject_Details" runat="server" SelectMethod="GetProjectByProjectId2"
                InsertMethod="CreateNewProject" UpdateMethod="UpdateProject" DataObjectTypeName="Eisk.BusinessEntities.Project"
                TypeName="Eisk.BusinessLogicLayer.ProjectBLL" OnSelecting="OdsProject_Details_Selecting"
                OnInserted="OdsProject_Details_Inserted">
                <UpdateParameters>
                    <asp:ControlParameter Name="ProjectId" ControlID="formViewProject" />
                    <asp:Parameter Name="Project" ConvertEmptyStringToNull="true" />
                    <asp:Parameter Name="ProjectInfo" ConvertEmptyStringToNull="true" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="ProjectName" ConvertEmptyStringToNull="true" />
                </InsertParameters>
                <SelectParameters>
                    <asp:RouteParameter Name="projectId" RouteKey="project_id" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

