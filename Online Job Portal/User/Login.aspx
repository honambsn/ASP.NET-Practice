<%@ Page Title="" Language="C#" MasterPageFile="~/User/UserMaster.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Online_Job_Portal.User.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
        <div class="container pt-50 pb-40">
            <div class="row">
                <div class="col-12 pb-20">
                    <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
                </div>
                <div class="col-12">
                    <h2 class="contact-title text-center">Sign In</h2>
                </div>
                <div class="col-lg-6 mx-auto">
                    <div class="form-contact contact_form">
                        <div class="row">
                            <div class="col-12">
                                <div class="form-group">
                                    <label>Username</label>
                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" placeholder="Enter Unique Username" required></asp:TextBox>
                                    <%--<textarea class="form-control w-100" runat="server" name="message" id="message" cols="30" rows="9" onfocus="this.placeholder = ''" onblur="this.placeholder = 'Enter Message'" placeholder=" Enter Message" required></textarea>--%>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <%--<input class="form-control valid" runat="server" name="name" id="name" type="text" onfocus="this.placeholder = ''" onblur="this.placeholder = 'Enter your name'" placeholder="Enter your name" required>--%>
                                    <label>Password</label>
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter Password" required></asp:TextBox>
                                </div>
                            </div>
                            <%--<div class="col-12">
                                <div class="form-group">
                                    <label>Login Type</label>
                                    <asp:DropDownList ID="ddlLoginType" runat="server" CssClass="form-contact w-100"
                                        AppendDataBoundItems="true" DataTextField="TypeName" DataValueField="TypeName">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please select user type."
                                        ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small" ControlToValidate="ddlLoginType"
                                        InitialValue="0"></asp:RequiredFieldValidator>
                                    <%--<asp:DropDownList ID="ddlLoginType" runat="server" CssClass="form-control w-100"
                                        AppendDataBoundItems="true" DataTextField="TypeName" DataValueField="TypeName">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="UserType is required"
                                        ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small" ControlToValidate="ddlLoginType"
                                        InitialValue="0"></asp:RequiredFieldValidator>--%>
                            <%--<textarea class="form-control w-100" runat="server" name="message" id="message" cols="30" rows="9" onfocus="this.placeholder = ''" onblur="this.placeholder = 'Enter Message'" placeholder=" Enter Message" required></textarea>--%>
                            <div class="col-12">
                                <div class="form-group">
                                    <label>Login Type</label>
                                    <asp:DropDownList ID="ddlTypeName" runat="server" CssClass="form-contact w-100"
                                        AppendDataBoundItems="true" DataTextField="TypeName" DataValueField="TypeName">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please select a login type."
                                        ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small" ControlToValidate="ddlTypeName"
                                        InitialValue="0"></asp:RequiredFieldValidator>
                                </div>
                            </div>


                        </div>
                        <div class="form-group mt-3">
                            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="button button-contactForm boxed-btn mr-4"
                                OnClick="btnLogin_Click" />
                            <span class="clickLink"><a href="Register.aspx">New User? Click Here...</a></span>
                        </div>
                    </div>
                </div>
        </div>
        </div>
    </section>
</asp:Content>
