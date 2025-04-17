<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Contacts.aspx.cs" Inherits="Online_Food.Admin.Contacts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .reply-form {
            max-height: 0;
            overflow: hidden;
            transition: max-height 0.3s ease-in-out;
        }

            .reply-form.show {
                max-height: 300px; /* hoặc tuỳ chỉnh theo nội dung */
            }
    </style>
    <script type="text/javascript">
        function showReplyForm(itemId) {
            var item = document.getElementById(itemId);
            if (item) {
                var panel = item.querySelector('.reply-form');
                if (panel) {
                    panel.style.display = 'block';
                    panel.style.maxHeight = '500px';
                    panel.style.transition = 'max-height 0.3s ease-in-out';
                }
            }
        }

        function hideMsgAfterSeconds(id) {
            setTimeout(function () {
                var lbl = document.getElementById(id);
                if (lbl) {
                    lbl.style.display = 'none';
                }
            }, 5000);
        }

        function toggleReplyForm(id) {
            var panel = document.getElementById(id);
            if (panel.classList.contains("show")) {
                panel.classList.remove("show");
            } else {
                panel.classList.add("show");
            }
        }

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
                                </div>
                                <div class="card-block">
                                    <div class="row">

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
                                                                    
                                                                    <asp:LinkButton ID="lnlReply" Text="Reply" runat="server" CommandName="reply"
                                                                        CssClass="badge bg-info" CommandArgument='<%#Eval("ContactID") %>'
                                                                        OnClientClick="return confirm('Do you want to reply this record?');">
                                                                        <i class="ti-comment-alt"></i>
                                                                    </asp:LinkButton>

                                                                    <%--<asp:LinkButton ID="lnkReply" Text="Reply" runat="server" CommandName="reply"
                                                                        CssClass="badge bg-warning" CommandArgument='<%#Eval("ContactID") %>'
                                                                        OnClientClick='<%# "showReplyForm(\"" + ((RepeaterItem)Container).ClientID + "\"); return false;" %>'>
                                                                        <i class="ti-comment-alt"></i>
                                                                    </asp:LinkButton>

                                                                    <!-- Panel for reply form, initially hidden -->
                                                                    <asp:Panel ID="pnlReplyForm" runat="server" Style="display: none;" CssClass="reply-form">
                                                                        <asp:TextBox ID="txtReplyMessage" runat="server" TextMode="MultiLine" Rows="4" Columns="50"
                                                                            Placeholder="Write your reply here..."></asp:TextBox>
                                                                        <asp:Button ID="btnSendReply" runat="server" Text="Send Reply" OnClick="SendReply" />
                                                                    </asp:Panel>--%>

                                                                    <!-- Label to display success or error messages -->
                                                                    <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="badge badge-success"></asp:Label>

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
