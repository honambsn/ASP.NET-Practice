﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Contacts.aspx.cs" Inherits="Online_Food.Admin.Contacts" %>

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

        function closePanel() {
            document.getElementById('<%= pReply.ClientID %>').style.display = 'none';
        }

        function validateAndConfirm() {
            var replyMessage = document.getElementById('<%= txtReplyMsg.ClientID %>').value.trim();

            // Check if the text field is empty
            if (replyMessage === "") {
                alert("Please enter a reply message.");
                return false; // Prevent the form from submitting
            }

            // If the field is not empty, show the confirmation
            return confirm('Do you want to update this reply?');
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
                                                                        CssClass="badge bg-info" CommandArgument='<%#Eval("FeedbackID") %>'
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
                                            <asp:Panel ID="pReply" runat="server">
                                                <h4 class="sub-title">Reply</h4>
                                                <div>
                                                    <div class="form-group">
                                                        <label for="txtReplyMsg">Reply Message</label>
                                                        <div>
                                                            <asp:TextBox ID="txtReplyMsg" runat="server" CssClass="form-control"
                                                                placeholder="Enter Reply Message" TextMode="MultiLine" Style="height: 200px;"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvReplyMsg" runat="server"
                                                                ErrorMessage="Message is required" ForeColor="Red" Display="Dynamic"
                                                                SetFocusOnError="true" ControlToValidate="txtReplyMsg" Font-Bold="true" InitialValue="" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="txtFeedbackID">Feedback ID</label>
                                                        <div>
                                                            <asp:TextBox ID="txtFeedbackID" runat="server" CssClass="form-control" ReadOnly="true"/>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="txtAdminName">Admin Name</label>
                                                        <div>
                                                            <asp:TextBox ID="txtAdminName" runat="server" CssClass="form-control"/>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="txtReplyDate">Reply Date</label>
                                                        <div>
                                                            <asp:TextBox ID="txtReplyDate" runat="server" CssClass="form-control" ReadOnly="true"/>
                                                        </div>
                                                    </div>

                                                    <div class="pb-5">
                                                        <asp:LinkButton ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-success" OnClick="btnAdd_Click"
                                                            OnClientClick="return validateAndConfirm();" CommandArgument='<%#Eval("FeedbackID") %>' CommandName="add" Visible="true"/>
                                                        &nbsp;

                                                        <asp:LinkButton ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnUpdate_Click"
                                                            OnClientClick="return validateAndConfirm();" CommandArgument='<%#Eval("FeedbackID") %>' CommandName="update" Visible="false"/>
                                                        &nbsp;
                                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="btnCancel_Click" CausesValidation="false"
                                                            OnClientClick="return confirm('Do you want to cancel this reply?');" />
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
