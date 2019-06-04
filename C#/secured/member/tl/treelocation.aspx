<%@ Page Language="C#" MasterPageFile="~/App_Resources/master2.master" AutoEventWireup="false"
    Inherits="TreeLocation_Page" Title="Plano | N@TURA"
    CodeBehind="treelocation.aspx.cs" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <!-- Online Scripts -->
    <%--<script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBOKuXQOB7hK42T17hytHJazGHgPB8g5Cg&sensor=false" type="text/javascript"></script>--%>
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBOKuXQOB7hK42T17hytHJazGHgPB8g5Cg&sensor=false&libraries=drawing" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.16/jquery-ui.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.16/i18n/jquery-ui-i18n.min.js" type="text/javascript"></script>
    <script src="http://google-maps-utility-library-v3.googlecode.com/svn/trunk/infobubble/src/infobubble.js" type="text/javascript"></script>


    <!--  -->
    <!-- Project Scripts -->
    <script type="text/javascript" src='<%# ResolveUrl ("~/App_Resources/client-scripts/tl/es-PR.js") %>'></script>
    <script type="text/javascript" src='<%# ResolveUrl ("~/App_Resources/client-scripts/tl/GoogleMaps-General.js") %>'></script>
    <script type="text/javascript" src='<%# ResolveUrl ("~/App_Resources/client-scripts/tl/GoogleMaps-Marker.js" ) %>'></script>
    <script type="text/javascript" src='<%# ResolveUrl ("~/App_Resources/client-scripts/tl/GoogleMaps-Polyline.js" ) %>'></script>
    <script type="text/javascript" src='<%# ResolveUrl ("~/App_Resources/client-scripts/tl/proj4js-compressed.js") %>'></script>
    <script type="text/javascript" src='<%# ResolveUrl ("~/App_Resources/client-scripts/tl/MarkerWithLabel.js") %>'></script>
    <!--  -->
    <!-- Local Scripts -->
    <script language="javascript" type="text/javascript">

        $(function () {

            var apiLocation = document.domain == 'localhost' ? "url(" : "url(/web";
            var imgLoc = apiLocation + "/App_Themes/Default/images/eisk-header-grad-long.png)";
            $('#divFind').css("background-image", imgLoc);
        });

        function enterKeyPressedOnTxt(e) {
            var key;
            if (window.event)
                key = window.event.keyCode; //IE
            else
                key = e.which; //firefox     

            if (key == 13)
                findMarker();

            return (key != 13);
        }


        //use it:
        function _spBodyOnLoadWrapper() {
            Initialize();
            return false;
        }

        function ShowLoading() {
            document.getElementById("imgLoading").style.display = 'block';
        }

        function CloseLoading() {
            document.getElementById("imgLoading").style.display = 'none';
        }

        String.prototype.replaceAll = function (find, replace) {
            var str = this;
            return str.replace(new RegExp(find, 'g'), replace);
        };

        String.prototype.repeat = function (num) {
            return new Array(num + 1).join(this);
        }

        function findMarker() {
            var numero = $('#<%= txtFilter.ClientID %>').val();
            var isValid = validateTextNumeric(numero);

            if (!isValid) {
                alert(numero + " no es un valor válido. Favor de entrar un número.");
                return;
            }
            var marker = getMarker(numero);

            if (marker)
                SetCurrentMarker(marker, true);
            else
                alert("El árbol número " + numero + " no existe.");
        }

        function toggleSetDraggable() {
            var lbDraggable = $('#<%= lbDraggable.ClientID %>');
            lbDraggable.removeClass(currentMarker.getDraggable() ? "white" : "red");
            lbDraggable.addClass(currentMarker.getDraggable() ? "red" : "white");
            lbDraggable.text(currentMarker.getDraggable() ? "Locked" : 'Unlocked');
            SetCurrentMarker(currentMarker, true, true);
        }

        function setDraggableFalse() {
            var lbDraggable = $('#<%= lbDraggable.ClientID %>');
            lbDraggable.removeClass("white");
            lbDraggable.addClass("red");
            lbDraggable.text('Locked');
        }

        function validateTextNumeric(textInput) {
            var value = Number(textInput);
            return !isNaN(value);
        }

        function alert1() {
            $('.gmnoprint').css({ opacity: 1 });
        }

        //document.getElementById('imgPrev').src = "http://localhost:53193/App_Resources/images/Circle3FCDFF.png"
    </script>
    <style type="text/css">
        #container {
            width: 100%;
            height: 100%;
            position: relative;
        }

        .infoi {
            width: 200px;
            height: 50px;
            position: absolute;
            top: 230px;
            left: 10px;
            z-index: 10;
        }

        .button_add {
            cursor: pointer; /* make the cursor like hovering over an <a> element */
        }

        html, body, #map_canvas {
            margin: 0;
            padding: 0;
            height: 100%;
        }

        .labels11 {
            color: black;
            font-family: "Lucida Grande", "Arial", sans-serif;
            font-size: 11px;
            text-align: center;
            width: 35px;
            height: 61px;
            white-space: nowrap;
        }

        .labels15 {
            color: black;
            font-family: "Lucida Grande", "Arial", sans-serif;
            font-size: 15px;
            font-weight: bold;
            text-align: center;
            width: 35px;
            height: 61px;
            white-space: nowrap; 
        }
        
        div.gmnoprint {
            opacity: 1 !important;
        }
        span.labels15 {
            height: 20px !important;
            cursor: pointer !important;
        } 
    </style>
    <!--  -->
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContentPlaceholder" runat="Server">
    <div class="container_tl clearfix ContentArea">
        <div style="width: 100%; min-height: 650px; height: 670px; background-color: aqua;">

            <div id="container">
                <div id="map_canvas" style="width: 100%; height: 100%; z-index: 1;">
                </div>
                <div class="infoi" style="display: none;">
                    <img id="imgPrev" class="button_add" style="position: absolute; top: 5px; left: 10px;" onclick="setDraggableFalse(); setPreviousCurrent()" />
                    <img id="imgCurr" class="button_add" style="position: absolute; top: 15.5px; left: 87px; width: 42px;" onclick="setCurrent()" />
                    <img id="imgCurr2" class="button_add" style="position: absolute; top: 0px; left: 69.5px; height: 76px;" onclick="setCurrent()" />
                    <img id="imgNext" class="button_add" style="position: absolute; top: 5px; right: -5px;" onclick="setDraggableFalse(); setNextCurrent()" />
                    <div id="divPrev" class="button_add labels11" style="position: absolute; top: 27px; left: 15px; height: 64px; width: 64px; font-size: 14px;" onclick="setDraggableFalse(); setPreviousCurrent()"></div>
                    <div id="divCurr" class="button_add labels15" style="position: absolute; top: 27px; left: 83.5px; height: 48px; width: 48px;" onclick="setCurrent()"></div>
                    <div id="divNext" class="button_add labels11" style="position: absolute; top: 27px; right: 0px; height: 64px; width: 64px; font-size: 14px;" onclick="setDraggableFalse(); setNextCurrent()"></div>
                    <div id="divFind" style="width: 215px; height: 110px; border: 1px ridge rgba(96, 95, 93, 0.46);">
                        <asp:TextBox ID="txtFilter" runat="server" CssClass="text" TabIndex="1" MaxLength="4" Style="width: 60px; height: 15px; position: absolute; top: 68px; left: 30px;" />
                        <asp:LinkButton ID="lbFilter" TabIndex="2" runat="server" SkinID="LinkButton" Text="Buscar" Style="width: 40px; position: absolute; top: 76px; right: 15px; height: 10px;"
                            OnClientClick="findMarker(); return false;" />
                        <asp:LinkButton ID="lbDraggable" TabIndex="2" runat="server" SkinID="RedLinkButton" Text="Locked" Style="width: 50px; height: 10px; position: absolute; top: -19px; right: 46.5px;"
                            OnClientClick="toggleSetDraggable(); return false;" />
                    </div>
                </div>
                <%--<div class="infoi">
                </div>--%>
            </div>
        </div>

        <asp:Literal EnableViewState="False" runat="server" ID="ltlMessage"></asp:Literal>
        <%--   <asp:FormView SkinID="FormView" ID="formViewUser" runat="server" DataSourceID="odsProject_Details"
            DataKeyNames="ProjectId">
            <ItemTemplate>
                <ul id="menu" class="collapsible" style="background-color: white; position: absolute; left: 0px; top: 62px; z-index: 2;">
                    <li><a href="#">Área de Usuario</a>
                        <ul>
                            <li>
                                <a href="<%# this.ResolveUrl("~/secured/member/userinfo_details.aspx") %>">Información de Usuario</a>
                            </li> 
                        </ul>
                    </li>
                </ul>
            </ItemTemplate>
        </asp:FormView>
        <div  class="FormView" ID="formViewProjects" runat="server"> 
                <ul id="menu4" class="collapsible" style="background-color: white; position: absolute; left: 197px; top: 62px; z-index: 2;">
                    <li><a href="../secured/member/projects.aspx">Proyectos</a>
                    </li>
                </ul> 
        </div>--%>
        <asp:FormView SkinID="FormView" ID="formViewProject" runat="server" DataSourceID="odsProject_Details"
            DataKeyNames="ProjectId">
            <ItemTemplate>
                <ul id="menu" class="collapsible" style="background-color: white; position: absolute; left: 0px; top: 62px; z-index: 2;">
                    <li><a href="<%# Page.GetRouteUrl("project-details", new { edit_mode = "view", project_id = RouteData.Values["project_id"] }) %>">...Volver</a>
                    </li>
                </ul>
            </ItemTemplate>
        </asp:FormView>
        <%--        <asp:FormView SkinID="FormView" ID="formViewProject" runat="server" DataSourceID="odsProject_Details"
            DataKeyNames="ProjectId">
            <ItemTemplate>
                <ul id="menu3" class="collapsible" style="background-color: white; position: absolute; left: 394px; top: 62px; z-index: 2;">
                    <li><a href="#">Proyecto</a>
                        <ul>
                            <li>
                                <a href="<%# Page.GetRouteUrl("project-details", new { edit_mode = "edit", project_id = RouteData.Values["project_id"] }) %>">Detalles Generales</a>
                            </li>
                        </ul>
                    </li>
                </ul>
            </ItemTemplate>
        </asp:FormView>--%>
        <asp:FormView SkinID="FormView" ID="formViewTreeLocation" runat="server" DataSourceID="odsProject_Details"
            DataKeyNames="ProjectId">
            <ItemTemplate>
                <ul id="menu2" class="collapsible" style="background-color: white; position: absolute; left: 197px; top: 62px; z-index: 2;">
                    <li><a href="#">Corte y Poda</a>
                        <ul>
                            <li>
                                <a href="<%# Page.GetRouteUrl("tl-project-details", new { edit_mode = "edit", project_id = RouteData.Values["project_id"] }) %>">Detalles de Corte y Poda</a>
                            </li>
                            <li>
                                <a href="<%# Page.GetRouteUrl("tl-treeinventory", new { project_id = RouteData.Values["project_id"] }) %>">Inventario de Árboles</a>
                            </li>
                            <li>
                                <a href="<%# Page.GetRouteUrl("tl-upload-csv", new { project_id = RouteData.Values["project_id"] }) %>">Importar Árboles</a>
                            </li>
                            <li>
                                <a href="<%# Page.GetRouteUrl("commonnames", new { Project_id = RouteData.Values["project_id"]  }) %>">Nom. Comunes Árboles</a>
                            </li>
                            <li>
                                <a href="<%# Page.GetRouteUrl("tl-reports", new { project_id = RouteData.Values["project_id"] }) %>">Reportes</a>
                            </li>
                        </ul>
                    </li>
                </ul>
            </ItemTemplate>
        </asp:FormView>
        <div class="FormView" id="formViewProjects" runat="server">
            <ul id="menu3" class="collapsible" style="background-color: white; position: absolute; left: 394px; top: 62px; z-index: 2;">
                <li><a href="#">Opciones</a>
                    <ul>
                        <li>
                            <a id="btnSetProjectCenter" href="#" onclick="javascript:setProjectCenter(); return false;">Centrar el Proyecto</a>
                        </li>
                        <li>
                            <a id="btnSetPerimeterFromSIP" href="#" onclick="javascript:setPerimeterFromSIP(); return false;">Traer Perímetro del SIP</a>
                        </li>
                    </ul>
                </li>
            </ul>
        </div>
        <table>
            <tbody>
                <tr>
                    <td>
                        <img src='<%#  webPath + "/App_Themes/Default/images/extras/loader-modal.gif" %> ' id="imgLoading"
                            style="position: absolute; left: 592px; top: 67px; z-index: 2; display: none;">
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <asp:ObjectDataSource ID="odsProject_Details" runat="server" SelectMethod="GetProjectByProjectId"
        InsertMethod="CreateNewProject" UpdateMethod="UpdateProject" DataObjectTypeName="Eisk.BusinessEntities.Project"
        TypeName="Eisk.BusinessLogicLayer.ProjectBLL">
        <SelectParameters>
            <asp:RouteParameter Name="projectId" RouteKey="project_id" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

