<%@ Page Language="C#" MasterPageFile="~/App_Resources/master.master" AutoEventWireup="true"
    Inherits="Eisk.Web.Tl_Upload_CSV" Title="Árbol | N@tura"
    CodeBehind="tl_upload_csv.aspx.cs" %>

<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 158px;
            height: 205px;
        }

        .auto-style2 {
            width: 157px;
            height: 206px;
        }
    </style>
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
    <script type='text/javascript'>
        var fieldName = 'chkReportSelector';
        var lbtGenerateReports = "BodyContentPlaceholder_lbtGenerateReports";
        var time = 20000;

        function GetRadioButtonListSelectedValue(radioButtonList) {
            var pnlState = $("#<%= pnlState.ClientID%>");
            var pnlNad83 = $("#<%= pnlNad83.ClientID%>");

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

        function hideDiv() {
            document.getElementById("loading").className = "loading-invisible";
        };

        function showDiv() {
            document.getElementById("loading").className = "loading-visible";

            setTimeout(function () { hideDiv(); }, time);
        };

        var loadingDiv = document.getElementById("loading");
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceholder" runat="Server">
    <%--    <asp:ScriptManager ID="ScriptManager2" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upd" runat="server">
        <ContentTemplate>--%>
    <script type="text/javascript">
        function getConfirm(message) {
            var ans = confirm(message.replace("###", "\n") + '\n¿Está usted seguro de querer hacer estos cambios? ');
            if (ans == true) {
                showDiv();
                __doPostBack('ctl00$BodyContentPlaceholder$btnUpload', '');
            }
            else {
                hideDiv();
            }
        }
    </script>
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
        <span class="grid_15 alpha">
            <asp:Literal ID="Literal1" runat="server" Text='Editar Detalles del Árbol'></asp:Literal>
        </span>
        <span class="grid_3 omega align-right"></span>
    </h1>
    <asp:Literal EnableViewState="false" runat="server" ID="ltlMessage"></asp:Literal>
    <p>
        Puede imporar un documento tipo CSV con la posición de los árboles. Seleccione el formato en el que importará los puntos:
    </p>
    <div>
        <asp:RadioButtonList ID="rblPosition" runat="server" RepeatDirection="Horizontal" AutoPostBack="false" onclick="GetRadioButtonListSelectedValue(this);">
            <asp:ListItem Text="NAD 83 (Planos Estatales de PR)&nbsp;&nbsp;" Selected="True" Value="0" />
            <asp:ListItem Text="Latitud/Longitud" Value="1" />
        </asp:RadioButtonList>
        <asp:FileUpload ID="FileUploadControl" Width="600px" runat="server" />
        &nbsp;&nbsp;&nbsp;
                <asp:Button runat="server" ID="btnUpload" ClientIDMode="Static" Style="display: none;" OnClick="btnUpload_Click" />
        <asp:Button runat="server" ID="btnValidate" ClientIDMode="Static" Text="Upload" OnClick="btnValidate_Click" />
        <br />
        <br />
        Ejemplo:
                <br />
        <br />
        <asp:Panel ID="pnlNad83" Style="display: block;" runat="server"> 
            <asp:Image runat="server" AlternateText="" Style="border: solid 1px blue; width: auto;" ID="imgStatePlanes" CssClass="auto-style1" ImageUrl='<%#  "~/App_Resources/images/StatePlanes.png" %>' />
        </asp:Panel>
        <asp:Panel ID="pnlState" Style="display: none;" runat="server">
            <asp:Image runat="server" AlternateText="" Style="border: solid 1px blue; width: auto;" ID="imgLatLon" CssClass="auto-style1" ImageUrl='<%# "~/App_Resources/images/LatLon.png" %>' />
        </asp:Panel>
    </div>
    <%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>

