<%@ Page Language="C#" MasterPageFile="~/App_Resources/master.master" AutoEventWireup="true" CodeBehind="users.aspx.cs" Inherits="Eisk.Web.User"
    Title="Usuarios | N@TURA" MetaKeywords="usuario"
    MetaDescription="Lista de usuarios. Seleccione la opción apropiada para insertar, ver, editar o eliminar un usuario." %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
        <script type="text/javascript">
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
    <h1 class="title-regular">Usuarios</h1>
    <p>
        Lista de usuarios. Seleccione la opción apropiada para insertar, ver, editar o eliminar un usuario.
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
                <asp:GridView ID="gridViewUsers" runat="server" DataSourceID="odsUserListing" SkinID="GridView" ClientIDMode="Static" AllowPaging="True" DataKeyNames="userId" PageSize="15">
                    <Columns>
                        <asp:BoundField ControlStyle-Width="32%" HeaderStyle-Width="32%" DataField="userName" HeaderText="Nombre" SortExpression="userName" />
                        <asp:TemplateField HeaderStyle-Width="17%">
                            <ItemTemplate>
                                <%#  (GetRole((System.Guid)Eval("userId"))).RoleName %>
                            </ItemTemplate>
                            <HeaderTemplate>
                                Rol
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="17%">
                            <ItemTemplate>
                                <%#  (GetGroup((System.Guid)Eval("userId"))).GroupName %>
                            </ItemTemplate>
                            <HeaderTemplate>
                                Nombre del Grupo
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ControlStyle-Width="24%" HeaderStyle-Width="24%" DataField="createdDate" HeaderText="Creado En" SortExpression="createdDate" DataFormatString="{0:MMM/dd/yyyy h:mm:ss tt}" />
                        <asp:TemplateField HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="pnlView" Width="100%" Height="100%">
                                    <a href="<%# Page.GetRouteUrl("user-details", new {edit_mode = "view", user_id = Eval("userId")}) %>"
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
                                    <a href="<%# Page.GetRouteUrl("user-details", new {edit_mode = "edit", user_id = Eval("userId")}) %>"
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
                <asp:ObjectDataSource ID="odsUserListing"      runat="server" DataObjectTypeName="Eisk.BusinessEntities.User"
                    DeleteMethod="DeleteUser" EnablePaging="True" InsertMethod="CreateNewUser" OldValuesParameterFormatString="original_{0}"
                    SelectCountMethod="GetTotalCountForAllUsers" SelectMethod="GetUsersByFilter" SortParameterName="orderby" OnSelected="odsUserListing_Selected"
                    TypeName="Eisk.BusinessLogicLayer.UserBLL" UpdateMethod="userId">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtFilter" Name="userName" PropertyName="Text"
                            Type="String" />
                        <asp:Parameter Name="orderBy" Type="String" />
                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                        <asp:Parameter Name="maximumRows" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
            <div class="clearfix align-right">
                <asp:LinkButton SkinID="LinkButton" runat="server" ID="lbtAddNewUser" Text="Añadir Usuario"
                    OnClick="ButtonAddNewUser_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
