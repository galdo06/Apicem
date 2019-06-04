<%@ Page Language="C#" MasterPageFile="~/App_Resources/master.master" AutoEventWireup="true" CodeBehind="notifications.aspx.cs" Inherits="Eisk.Web.Notification"
    Title="Nom. Comunes | N@TURA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <style type="text/css">
        .dropdownwrap {
            height: auto;
            width: 99%;
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

        $(function () {
            $('.dropdown').bind({
                click: function () {
                    if (this.mouseleaveBind == null || this.mouseleaveBind) {
                        $(this).nextAll('.dropdownwrap:first').slideDown('400');
                        this.mouseleaveBind = false;
                    } else {
                        $(this).nextAll('.dropdownwrap:first').slideUp('400');
                        this.mouseleaveBind = true;
                    }
                }
            });

        });

        var fieldName = 'chkNotificationSelector';

        function selectall(frmId, check) {
            var frm = document.getElementById(frmId);

            var i = frm.rows.length;
            //var e = document.frm.elements;
            var name = new Array();
            var value = new Array();
            var j = 0;
            for (var k = 1; k < i; k++) {
                if (frm.rows[k].children[0].children.length > 0) {
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
            }
            checkSelect(frmId);
        }

        function selectCheck(frmId, check) {
            var frm = document.getElementById(frmId);
            var i = frm.rows.length;

            for (var k = 1; k < i; k++) {
                if (frm.rows[k].children[0].children.length > 0) {
                    var checkbox = frm.rows[k].children[0].children[0].children[0].children[0];
                    if (checkbox) {
                        if (checkbox.id == fieldName) {
                            checkbox.checked = check;
                        }
                    }
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
                if (frm.rows[k].children[0].children.length > 0) {
                    var checkbox = frm.rows[k].children[0].children[0].children[0].children[0];
                    if (checkbox.id == fieldName) {
                        if (checkbox.checked == false) {
                            berror = false;
                            break;
                        }
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
    <h1 class="title-regular">Notificaciones</h1>
    <p>
        Notificaciones de importancia. Favor de revisarlas con regularidad:
    </p>
    <asp:ScriptManager ID="scriptManager" runat="server" />
    <asp:UpdatePanel ID="upd" runat="server">
        <ContentTemplate>

            <asp:Literal EnableViewState="false" runat="server" ID="ltlMessage"></asp:Literal>
            <div class="clearfix align-right">
                <asp:TextBox ID="txtFilter" runat="server" CssClass="text" AutoPostBack="true"
                    onfocus="SetCursorToTextEnd(this);" onkeydown="javascript:var ret = checkEnterPressed(event,this); this.focus(); return ret;" />
                <asp:LinkButton ID="lbFilter" runat="server" SkinID="LinkButton" Text="Filtrar" />
            </div>
            <div class="grid-viewer grid_19 clearfix">
                <asp:GridView ID="gridViewNotifications" runat="server" Style="width: 100%;"
                    DataSourceID="odsNotificationListing" SkinID="GridView" ClientIDMode="Static" AllowPaging="True" DataKeyNames="NotificationID" PageSize="15" OnRowDataBound="gridViewNotifications_RowDataBound">
                    <Columns>
                        <asp:BoundField HeaderStyle-Width="18%" DataField="editedDate" HeaderText="Fecha" SortExpression="editedDate" DataFormatString="{0:MMM/dd/yyyy}" ItemStyle-VerticalAlign="Top" />
                        <asp:TemplateField HeaderStyle-Width="82%">
                            <ItemTemplate>
                                <asp:LinkButton Text='<%# Eval("NotificationSubject")  %>' class="dropdown" OnClientClick="return false;" runat="server" />
                                <div id="divPreviouslyImpacted" class='dropdownwrap'>
                                    <br />
                                    <div class='notice'>
                                        <asp:Literal Text='<%# Eval("NotificationDesc")  %>' runat="server" />
                                    </div>
                                </div>
                            </ItemTemplate>
                            <HeaderTemplate>
                                Título
                            </HeaderTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="clearfix">
                <asp:ObjectDataSource ID="odsNotificationListing" runat="server" DataObjectTypeName="Eisk.BusinessEntities.Notification"
                    EnablePaging="True" InsertMethod="CreateNewNotification" OldValuesParameterFormatString="original_{0}" OnSelecting="odsNotificationListing_Selecting"
                    SelectCountMethod="GetTotalCountForAllNotifications" SelectMethod="GetNotificationByFilter" SortParameterName="orderby" OnSelected="odsNotificationListing_Selected"
                    TypeName="Eisk.BusinessLogicLayer.NotificationBLL" UpdateMethod="organismId">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtFilter" Name="searchText" PropertyName="Text"
                            Type="String" />
                        <asp:Parameter Name="orderBy" Type="String" />
                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                        <asp:Parameter Name="maximumRows" Type="Int32" />
                        <asp:Parameter Name="groupID" Type="Int32" />
                        <asp:Parameter Name="userID" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
