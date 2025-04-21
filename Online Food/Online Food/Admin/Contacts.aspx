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

        function hidePanel() {
            var panel = document.getElementById('<%= pReply.ClientID %>');
            panel.style.display = 'none';
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

                                        <div class="col-sm-6 col-md-8 col-lg-8">


                                            <%--<div class="card-block table-border-style">
                                                <div class="table-responsive">

                                                    <asp:Repeater ID="rOrderStatus" runat="server" OnItemCommand="rOrderStatus_ItemCommand">
                                                        <HeaderTemplate>
                                                            <table class="table data-table-export table-hover nowrap">
                                                                <thead>
                                                                    <tr>
                                                                        <th class="table-plus">Order No.</th>
                                                                        <th>Order Date</th>
                                                                        <th>Status</th>
                                                                        <th>Product Name</th>
                                                                        <th>Total Price</th>
                                                                        <th>Payment Mode</th>
                                                                        <th class="datatable-nosort">Edit</th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="table-plus"><%# Eval("OrderNo") %></td>
                                                                <td><%# Eval("OrderDate") %></td>
                                                                <td>
                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'
                                                                        CssClass='<%# Eval("Status").ToString() == "Delivered" ? "badge badge-success" : "badge badge-warning" %>'>
                                                                    </asp:Label>
                                                                </td>
                                                                <td><%# Eval("ProductName") %></td>
                                                                <td><%# Eval("TotalPrice") %></td>
                                                                <td><%# Eval("PaymentMode") %></td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CssClass="badge badge-primary"
                                                                        CommandArgument='<%#Eval("OrderDetailsID") %>' CommandName="edit">
                                                                        <i class="ti-pencil"></i>
                                                                    </asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </tbody>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>--%>

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


                                        <div class="col-sm-6 col-md-4 col-lg-4 mobile-inputs">
                                            <asp:Panel ID="pReply" runat="server" Visible="false">
                                                <h4 class="sub-title">Reply</h4>
                                                <div>
                                                    <div class="form-group">
                                                        <label for="txtReplyMsg">Reply Message</label>
                                                        <div>
                                                            <asp:TextBox ID="txtReplyMsg" runat="server" CssClass="form-control"
                                                                placeholder="Enter Reply Message" required="" TextMode="MultiLine" Style="height: 200px;"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvReplyMsg" runat="server"
                                                                ErrorMessage="Message is required" ForeColor="Red" Display="Dynamic"
                                                                SetFocusOnError="true" ControlToValidate="txtReplyMsg" Font-Bold="true" InitialValue="">

                                                            </asp:RequiredFieldValidator>
                                                            <asp:HiddenField ID="hdnId" runat="server" Value="0" />
                                                        </div>
                                                    </div>

                                                    <div class="pb-5">
                                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnUpdate_Click" CausesValidation="true" />
                                                        &nbsp;
                                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnCancel_Click" CausesValidation="false"  OnClientClick="hidePanel(); return false;" />
                                                    </div>

                                                </div>



                                            </asp:Panel>
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
