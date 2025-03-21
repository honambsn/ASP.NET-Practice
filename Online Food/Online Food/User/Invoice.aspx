<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Invoice.aspx.cs" Inherits="Online_Food.User.Invoice" %>

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
    <section class="book_section layout_padding">
        <div class="container">
            <div class="heading_container">
                <div class="align-self-end">
                    <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
                </div>
            </div>
        </div>

        <div class="container">
            <asp:Repeater ID="rOrderItem" runat="server">
                <HeaderTemplate>
                    <table class="table table-responsive-sm table-bordered table-hover" id="tblInvoice">
                        <thead class="bg-dark text-white">
                            <tr>
                                <th>Sr.No</th>
                                <th>Order Number</th>
                                <th>Item Name</th>
                                <th>Unit Price</th>
                                <th>Quantity</th>
                                <th>Total Price</th>
                            </tr>
                        </thead>
                        <tbody>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td> <%# Eval("Srno") %></td>
                        <td> <%# Eval("OrderNo") %></td>
                        <td> <%# Eval("Name") %></td>
                        <td> <%# string.IsNullOrEmpty( Eval("Price").ToString() ) ? "" : "$" + Eval("Price") %></td>
                        <td> <%# Eval("Quantity") %></td>
                        <td> <%# Eval("TotalPrice") %></td>
                        
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>

            <div class="text-center">
                <asp:LinkButton ID="lbDownloadInvoice" runat="server" CssClass="btn btn-info">
                    <i class="fa fa-file-pdf-o mr-2"></i> Download Invoice
                </asp:LinkButton>
            </div>

        </div>
    </section>
</asp:Content>
