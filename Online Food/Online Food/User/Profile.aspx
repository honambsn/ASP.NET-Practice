<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Online_Food.User.Profile" %>

<%@ Import Namespace="Online_Food" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%
        string imageUrl = Session["imageUrl"].ToString();
    %>

    <section class="book_section layout-padding">
        <div class="container">
            <div class="heading_container">
                <h2>User Information</h2>
            </div>

            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-body">
                            <div class="card-title mb-4">
                                <div class="d-flex justify-content-start">
                                    <!-- Image container for user image -->
                                    <div class="image-container">
                                        <!-- Example: Image tag, replace with dynamic source -->
                                        <img src="<%=Utils.GetImageUrl(imageUrl) %>" id="imgProfile" style="width: 150px; height: 150px;"
                                            class="img-thumbnail" />
                                        <div class="middle pt-2">
                                            <a href="Registration.aspx?id=<%Response.Write(Session["UserID"]); %>"
                                                class="btn btn-warning">
                                                <i class="fa fa-pencil"></i>Edit Details
                                            </a>
                                        </div>
                                    </div>

                                    <div class="userData ml-3">
                                        <h2 class="d-block" style="font-size: 1.5rem; font-weight: bold">
                                            <a href="javascript:void(0);"><%Response.Write(Session["name"]); %></a>
                                        </h2>
                                        <h6 class="d-block">
                                            <a href="javascript:void(0)">
                                                <asp:Label ID="lblUsername" runat="server" ToolTip="Unique Username">
                                                    @<%Response.Write(Session["username"]); %>
                                                </asp:Label>
                                            </a>
                                        </h6>

                                        <h6 class="d-block">
                                            <a href="javascript:void(0)">
                                                <asp:Label ID="lblEmail" runat="server" ToolTip="User Email">
                                                    @<%Response.Write(Session["email"]); %>
                                                </asp:Label>
                                            </a>
                                        </h6>

                                        <h6 class="d-block">
                                            <a href="javascript:void(0)">
                                                <asp:Label ID="lblCreatedDate" runat="server" ToolTip="Account Created On">
                                                    @<%Response.Write(Session["createdDate"]); %>
                                                </asp:Label>
                                            </a>
                                        </h6>
                                    </div>

                                </div>
                            </div>

                            <div class="row">
                                <div class="col-12">
                                    <ul class="nav nav-tabs mb-4" id="myTab" role="tablist">
                                        <li class="nav-item">
                                            <a class="nav-link active text-info" id="basicInfo-tab" data-toggle="tab" href="#basicInfo"
                                                role="tab" aria-controls="basicInfo" aria-selected="true"><i class="fa fa-id-badge mr-2"></i>Information</a>
                                        </li>

                                        <li class="nav-item">
                                            <a class="nav-link active text-info" id="connectedServices-tab" data-toggle="tab" href="#connectedServices"
                                                role="tab" aria-controls="connectedServices" aria-selected="false"><i class="fa fa-clock-o mr-2"></i>Purchased History</a>
                                        </li>
                                    </ul>

                                    <div class="tab-content ml-1" id="myTabContent">
                                        <%--Basic User Information Starts--%>
                                        <div class="tab-pane fade show active" id="basicInfo" role="tabpanel" aria-labelledby="basicInfo-tab">
                                            <asp:Repeater ID="rUserProfile" runat="server">
                                                <ItemTemplate>

                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <labal style="font-weight: bold;">Full Name</labal>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("Name")  %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <labal style="font-weight: bold;">Username</labal>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("Username")  %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <labal style="font-weight: bold;">Mobile No.</labal>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("Mobile")  %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <labal style="font-weight: bold;">Email</labal>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("Email")  %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <labal style="font-weight: bold;">Post Code</labal>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("PostCode")  %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <labal style="font-weight: bold;">Address</labal>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("Address")  %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <%--Basic User Information Ends--%>

                                        <%--Order History Starts--%>
                                        <div class="tab-pane fade" id="connectedServices" role="tabpanel" aria-labelledby="connectedServices-tab">
                                            <%--<asp:Repeater ID="rPurchasedHistory" runat="server">
                                                <ItemTemplate>
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <labal style="font-weight: bold;">Order ID</labal>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("OrderID")  %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <labal style="font-weight: bold;">Order Date</labal>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("OrderDate")  %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <labal style="font-weight: bold;">Total Amount</labal>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("TotalAmount")  %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <labal style="font-weight: bold;">Payment Status</labal>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("PaymentStatus")  %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                </ItemTemplate>
                                            </asp:Repeater>--%>
                                        </div>
                                        <%--Order History End--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Close card-body -->
                    </div>
                    <!-- Close card -->
                </div>
                <!-- Close col-12 -->
            </div>
            <!-- Close row -->
        </div>
        <!-- Close container -->
    </section>
</asp:Content>
