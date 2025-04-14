<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="Online_Food.User.Contact" %>
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

      <!-- book section -->
  <section class="book_section layout_padding">
    <div class="container">
      <div class="heading_container">
          <div class="align-self-end">
              <asp:Label runat="server" ID="lblMsg"></asp:Label>
          </div>
        <h2>Send Your Query</h2>
      </div>

      <div class="row">
        <div class="col-md-6">
          <div class="form_container">
            
              <div>
                  <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Your Name"></asp:TextBox>
                  <asp:RequiredFieldValidator runat="server" ErrorMessage="Name is required" ID="rfvName"
                      ControlToValidate="txtName" ForeColor="Red" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
              </div>
              <div>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Your Email" TextMode="Email"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="Email is required" ID="rfvEmail"
                    ControlToValidate="txtEmail" ForeColor="Red" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
              </div>
              <div>
                <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" placeholder="Subject"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="Subject is required" ID="rfvSubject"
                    ControlToValidate="txtSubject" ForeColor="Red" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
              </div>
              <div>
                <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" placeholder="Enter your query/feedback"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="Message is required" ID="rfvMessage"
                    ControlToValidate="txtMessage" ForeColor="Red" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
              </div>
              
              <div class="btn_box">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-warning rounded-pill pl-4 pr-4 text-white" OnClick="btnSubmit_Click"/>
              </div>
          </div>
        </div>
        <div class="col-md-6">
          <div class="map_container ">
            <div id="googleMap"></div>
          </div>
        </div>
      </div>
    </div>
  </section>
  <!-- end book section -->
</asp:Content>
