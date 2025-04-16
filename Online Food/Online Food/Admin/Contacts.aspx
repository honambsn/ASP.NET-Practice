<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Contacts.aspx.cs" Inherits="Online_Food.Admin.Contacts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        window.onload = function () {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById('<%=lblMsg.ClientID %>').style.display = 'none';
            }, seconds * 1000);
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pcoded-innter-content pt-0">
        <div class="align-align-self-end">
            <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
        </div>

        <div class="main-body">
            <div class="page-wrapper">
                <div class="page-body">
                    <div class="row">

                        <div class="col-sm-12">
                            <div class="card">
                                <div class="card-header">
                                    <%--<h5>Statestics</h5>
                                <div class="card-header-left ">
                                </div>
                                <div class="card-header-right">
                                    <ul class="list-unstyled card-option">
                                        <li><i class="icofont icofont-simple-left "></i></li>
                                        <li><i class="icofont icofont-maximize full-card"></i></li>
                                        <li><i class="icofont icofont-minus minimize-card"></i></li>
                                        <li><i class="icofont icofont-refresh reload-card"></i></li>
                                        <li><i class="icofont icofont-error close-card"></i></li>
                                    </ul>
                                </div>--%>
                                </div>
                                <div class="card-block">
                                    <div class="row">
                                        <%--<div class="col-sm-6 col-md-4 col-lg-4">
                                        <h4 class="sub-title">Category</h4>
                                        <div>
                                            <div class="form-group">
                                                <label>Category Name</label>
                                                <div>
                                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Enter Category Name" required=""></asp:TextBox>
                                                    <asp:HiddenField ID="hdnId" runat="server" Value="0" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label>Category Image</label>
                                                <div>
                                                    <asp:FileUpload ID="fuCategoryImage" runat="server" CssClass="form-control" onchange="Image Preview(this);" />
                                                </div>
                                            </div>
                                            <div class="form-check pl-4">
                                                <asp:CheckBox ID="cbIsActive" runat="server" Text="&nbsp; IsActive" CssClass="form-check-input" />
                                            </div>
                                            <div class="pb-5">
                                                <asp:Button ID="btnAddOrUpdate" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAddOrUpdate_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnClear_Click" />
                                            </div>
                                            <div>
                                                <asp:Image ID="imgCategory" runat="server" CssClass="img-thumbnail" />
                                            </div>
                                        </div>
                                    </div>--%>


                                        <div class="col-12 mobile-inputs">
                                            <h4 class="sub-title">Contact List</h4>
                                            <div class="card-block table-border-style">
                                                <div class="table-responsive">

                                                    <asp:Repeater ID="rContacts" runat="server" OnItemCommand="rContacts_ItemCommand">
                                                        <HeaderTemplate>
                                                            <table class="table data-table-export table-hover nowrap">
                                                                <thead>
                                                                    <tr>
                                                                        <th class="table-plus">SrNo</th>
                                                                        <th>User Name</th>
                                                                        <th>Email</th>
                                                                        <th>Subject</th>
                                                                        <th>Message</th>
                                                                        <th>Created Date</th>
                                                                        <th class="datatable-nosort">Delete</th>
                                                                    </tr>

                                                                </thead>
                                                                <tbody>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="table-plus"><%# Eval("SrNo") %></td>
                                                                <td><%# Eval("Name") %> </td>
                                                                <td><%# Eval("Email") %> </td>
                                                                <td><%# Eval("Subject") %> </td>
                                                                <td><%# Eval("Message") %> </td>
                                                                <td><%# Eval("CreatedDate") %></td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnkDelete" Text="Delete" runat="server" CommandName="delete"
                                                                        CssClass="badge bg-danger" CommandArgument='<%#Eval("ContactID") %>'
                                                                        OnClientClick="return confirm('Do you want to delete this record?');">
                                                                        <i class="ti-trash"></i>
                                                                    </asp:LinkButton>

                                                                    <asp:LinkButton ID="lnkReply" Text="Reply" runat="server" CommandName="reply"
                                                                        CssClass="badge bg-warning" CommandArgument='<%#Eval("ContactID") %>'
                                                                        OnClientClick="return confirm('Do you want to reply this feedback?');">
                                                                        <i class="ti-comment-alt"></i>
                                                                    </asp:LinkButton>

                                                                    <!-- Panel for reply form, initially hidden -->
                                                                    <asp:Panel ID="pnlReplyForm" runat="server" Visible="false">
                                                                        <asp:TextBox ID="txtReplyMessage" runat="server" TextMode="MultiLine" Rows="4" Columns="50"
                                                                            Placeholder="Write your reply here..."></asp:TextBox>
                                                                        <asp:Button ID="btnSendReply" runat="server" Text="Send Reply" OnClick="SendReply" />
                                                                    </asp:Panel>

                                                                    <!-- Label to display success or error messages -->
                                                                    <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>

                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </tbody>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
