<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="Online_Food.Admin.Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%--<div class="pcoded-innter-content pt-0">
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

    </div>--%>


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
                                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary mt-md-4" OnClick="btnSearch_Click" />
                                            </div>
                                        </div>
                                </div>
                                <div class="card-block">
                                    <div class="row">

                                        <div class="col-12 mobile-inputs">
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
