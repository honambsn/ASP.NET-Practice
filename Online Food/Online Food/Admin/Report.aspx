<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="Online_Food.Admin.Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="pcoded-innter-content pt-0">
        <div class="main-body">
            <div class="page-wrapper">
                <div class="page-body">
                    <div class="row">

                        <div class="col-sm-12">
                            <div class="card">
                                <div class="card-header">
                                    <div class="container">
                                        <div class="form-group col-md-4">
                                            <label>From Date</label>
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ForeColor="Red" ErrorMessage="*"
                                                SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtFromDate"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                        </div>

                                        <div class="form-group col-md-4">
                                            <label>To Date</label>
                                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ForeColor="Red" ErrorMessage="*"
                                                SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtToDate"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                        </div>

                                        <div class="form-group col-md-4">
                                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary mt-md-4" OnClick="btnSearch_Click"/>
                                        </div>

        
                                    </div>
                                </div>
                                <div class="card-block">
                                    <div class="row">

                                        <div class="col-sm-6 col-md-8 col-lg-8">
                                            <h4 class="sub-title">Selling Report</h4>


                                            <div class="card-block table-border-style">
                                                <div class="table-responsive">

                                                    <asp:Repeater ID="rReport" runat="server">
                                                        <HeaderTemplate>
                                                            <table class="table data-table-export table-hover nowrap">
                                                                <thead>
                                                                    <tr>
                                                                        <th class="table-plus">SrNo</th>
                                                                        <th>Full Name</th>
                                                                        <th>Email</th>
                                                                        <th>Item Orders</th>
                                                                        <th>Total Cost</th>
                                                                    </tr>

                                                                </thead>
                                                                <tbody>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="table-plus"><%# Eval("SrNo") %></td>
                                                                <td><%# Eval("Name") %> </td>
                                                                <td><%# Eval("Email") %> </td>
                                                                <td><%# Eval("TotalOrders") %> </td>
                                                                <td><%# Eval("TotalPrice") %> </td>
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


                                        <%--                                    <div class="col-sm-6 col-md-4 col-lg-4 mobile-inputs">
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
                                    </div>--%>
                                    </div>
                                    
                                    <div class="row pl-4">
                                        <asp:Label ID="lblTotal" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
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
