<%@ Page Language="C#" MasterPageFile="~/App_Resources/master.master" AutoEventWireup="false"
    Inherits="CreateUser" Title="Crear Usuario | N@TURA" 
    CodeBehind="createuser.aspx.cs" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyContentPlaceholder" runat="Server">
    <asp:CreateUserWizard ID="CreateUserWizard1" runat="server"  RequireEmail="False" OnCreatedUser="CreateUserWizard1_CreatedUser" OnCreateUserError="CreateUserWizard1_CreateUserError">
        <WizardSteps><%--ContinueDestinationPageUrl="../login/Default.aspx"--%>
            <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
                <ContentTemplate>
                    <h1 class="title-regular clearfix">New Application User
                    </h1>
                    <asp:Literal EnableViewState="False" runat="server" ID="ltlMessage"></asp:Literal>
                    <div class="grid_10 alpha">
                         <label>
                            Select Role:</label><br />
                        <asp:DropDownList ID="ddlRoles" runat="server" AppendDataBoundItems="true"
                            AutoPostBack="false" DataSourceID="odsRoleList" DataTextField="RoleName"
                            DataValueField="RoleID" EnableViewState="true" >
                        </asp:DropDownList>
                        <asp:ObjectDataSource ID="odsRoleList"       runat="server" TypeName="Eisk.BusinessLogicLayer.RoleBLL"
                            EnableViewState="true" SelectMethod="GetAllRolesList" />
                    </div>
                    <div class="grid_10 alpha">
                                  <label>
                            Select Group:</label><br />
                        <asp:DropDownList ID="ddlGroups" runat="server" AppendDataBoundItems="true"
                            AutoPostBack="false" DataSourceID="odsGroupList" DataTextField="GroupName"
                            DataValueField="GroupID" EnableViewState="true" >
                        </asp:DropDownList>
                        <asp:ObjectDataSource ID="odsGroupList"       runat="server" TypeName="Eisk.BusinessLogicLayer.GroupBLL"
                            EnableViewState="true" SelectMethod="GetAllGroupsList" />
                    </div>
                    <div class="grid_10 inline alpha">
                        <label>
                            Email:</label>
                        <span class="required-field-indicator">*</span><br />
                        <asp:TextBox ID="UserName" runat="server" CssClass="text" MaxLength="40"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvUserName" runat="server"
                            CssClass="validator" ControlToValidate="UserName" Display="Dynamic" ValidationGroup="CreateUserWizard1"><br />Required</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revUserName" runat="server"
                            CssClass="validator" ControlToValidate="UserName" Display="Dynamic" ValidationGroup="CreateUserWizard1" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"><br />Invalid Email format</asp:RegularExpressionValidator><br />
                        <label>
                            Password:</label>
                        <span class="required-field-indicator">*</span><br />
                        <asp:TextBox ID="Password" runat="server" CssClass="text" MaxLength="15" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ValidationGroup="CreateUserWizard1"
                            CssClass="validator" ControlToValidate="Password" Display="Dynamic"><br />Required</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revPassword" runat="server" ValidationGroup="CreateUserWizard1"
                            CssClass="validator" ControlToValidate="Password" Display="Dynamic" ValidationExpression="^.*(?=.{6,})(?=.*[a-z])(?=.*[A-Z])(?=.*[\d\W]).*$"><br />Password must be at least 8 characters, at least one uppercase letter, one lowercase letter and a number or special character</asp:RegularExpressionValidator><br />
                        <label>
                            Confirm Password:</label>
                        <span class="required-field-indicator">*</span><br />
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="text" MaxLength="15" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ValidationGroup="CreateUserWizard1"
                            CssClass="validator" ControlToValidate="txtConfirmPassword" Display="Dynamic"><br />Required</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revConfirmPassword" runat="server" ValidationGroup="CreateUserWizard1"
                            CssClass="validator" ControlToValidate="txtConfirmPassword" Display="Dynamic" ValidationExpression="^.*(?=.{6,})(?=.*[a-z])(?=.*[A-Z])(?=.*[\d\W]).*$"><br />Password must be at least 8 characters, at least one uppercase letter, one lowercase letter and a number or special character</asp:RegularExpressionValidator>
                        <asp:CompareValidator ID="cvConfirmPassword" runat="server" ValidationGroup="CreateUserWizard1"
                            CssClass="validator" ControlToValidate="txtConfirmPassword" Display="Dynamic" ControlToCompare="Password" ValueToCompare="txtConfirmPassword"><br />Passwords don't match</asp:CompareValidator><br />
                    </div>
                    <div class="grid_9 inline omega">
                        <label>
                            Password Question:</label>
                        <span class="required-field-indicator">*</span><br />
                        <asp:TextBox ID="Question" runat="server" CssClass="text" MaxLength="100"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvQuestion" runat="server" ValidationGroup="CreateUserWizard1"
                            CssClass="validator" ControlToValidate="Question" Display="Dynamic"><br />Required</asp:RequiredFieldValidator><br />
                        <label>
                            Password Answer:</label>
                        <span class="required-field-indicator">*</span><br />
                        <asp:TextBox ID="Answer" runat="server" CssClass="text" MaxLength="100"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvAnswer" runat="server" ValidationGroup="CreateUserWizard1" ErrorMessage="Required"
                            CssClass="validator" ControlToValidate="Answer" Display="Dynamic"><br />Required</asp:RequiredFieldValidator><br />
                        <p>
                            <asp:Button ID="CreateUser" runat="server" Text="Guardar" SkinID="Button" CommandName="MoveNext" ValidationGroup="CreateUserWizard1" />
                            <asp:Button ID="btnBack" CausesValidation="false" runat="server" Text="Volver" OnClick="ButtonGoToListPage_Click"
                                SkinID="AltButton" />

                        </p>
                        <em>Los campos requeridos están marcados con un <span class="required-field-indicator">*</span></em>
                    </div>
                </ContentTemplate>
                <CustomNavigationTemplate>
                </CustomNavigationTemplate>
            </asp:CreateUserWizardStep>
            <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
                <ContentTemplate>
                    <h1 class="title-regular clearfix">New Application User
                    </h1>
                    <div class="grid_10 alpha">
                        <strong>Email:</strong>
                        <asp:Label ID="lblEmail" runat="server" /><br />
                        <strong>User Type:</strong>
                        <asp:Label ID="lblUserType" runat="server" /><br />
                        <strong>Group:</strong>
                        <asp:Label ID="lblGroup" runat="server" /><br />
                        <hr />
                        Application User created successfully.
                        <hr />
                        <asp:Button CausesValidation="false" ID="btnBack" runat="server" Text="Volver" OnClick="ButtonGoToListPage_Click"
                            SkinID="AltButton" />
                    </div>
                </ContentTemplate>
            </asp:CompleteWizardStep>
        </WizardSteps>
    </asp:CreateUserWizard>
</asp:Content>
