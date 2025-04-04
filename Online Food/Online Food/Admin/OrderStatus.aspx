<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="OrderStatus.aspx.cs" Inherits="Online_Food.Admin.OrderStatus" %>

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
                                        <div class="col-sm-6 col-md-8 col-lg-8">
                                            <h4 class="sub-title">Order List</h4>

                                            <div class="card-block table-border-style">
                                                <div class="table-responsive">

                                                    <asp:Repeater ID="rOrderStatus" runat="server" OnItemCommand="rOrderStatus_ItemCommand">
                                                        <headertemplate>
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
                                                        </headertemplate>
                                                        <itemtemplate>
                                                            <tr>
                                                                <td class="table-plus"><%# Eval("OrderNo") %></td>
                                                                <td>
                                                                    <%# Eval("OrderDate") %>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'
                                                                        CssClass='<%# Eval("Status").ToString() == "Delievered" ? "badge badge-success" : "badge badge-warning" %>'></asp:Label>
                                                                </td>
                                                                <td><%# Eval("Name") %></td>
                                                                <td><%# Eval("TotalPrice") %></td>
                                                                <td><%# Eval("PaymentMode") %></td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CssClass="badge badge-primary"
                                                                        CommandArgument='<%#Eval("OrderDetailsID") %>' CommandName="edit">
                                                                        <i class="ti-pencil"></i>
                                                                    </asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </itemtemplate>
                                                        <footertemplate>
                                                            </tbody>
                                                            </table>
                                                        </footertemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>

                                        </div>


                                        <div class="col-sm-6 col-md-4 col-lg-4 mobile-inputs">
                                            <asp:Panel ID="pUpdateOrderStatus" runat="server">
                                                <h4 class="sub-title">Update Status</h4>
                                                <div>
                                                    <div class="form-group">
                                                        <label>Order Status</label>
                                                        <div>
                                                            <asp:DropDownList ID="ddlOrderStatus" runat="server" CssClass="form-control">
                                                                <asp:ListItem Value="0">Select Status</asp:ListItem>
                                                                <asp:ListItem>Pending</asp:ListItem>
                                                                <asp:ListItem>Dispatched</asp:ListItem>
                                                                <asp:ListItem>Delivered</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvDdlOrderStatus" runat="server" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ControlToValidate="ddlOrderStatus"
                                                                ErrorMessage="Order status is required" SetFocusOnError="true" Dislay="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
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
