<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="NewJob.aspx.cs" Inherits="Online_Job_Portal.Admin.NewJob" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="background-image: url('.../Images/bg.jpg'); width: 100%; height: auto; background-repeat: no-repeat; background-size: cover; background-attachment: fixed;">
        <div class="container pt-4 pb-4">
            <div>
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </div>

            <h3 class="text-center">Add Job</h3>
            <div class="row mr-lg-5 ml-lg-5 mb-3">
                <div class="col-md-6 pt-3">
                    <label for="txtJobTitle" style="font-weight: 600">Job Title</label>
                    <asp:TextBox ID="txtJobTitle" runat="server" CssClass="form-control" placeholder="Enter Job Title" required>
                    </asp:TextBox>
                </div>

                <div class="col-md-6 pt-3">
                    <label for="txtNoOfPost" style="font-weight: 600">Number Of Post</label>
                    <asp:TextBox ID="txtNoOfPost" runat="server" CssClass="form-control" placeholder="Enter Number Of Position"
                        TextMode="Number" required>
                    </asp:TextBox>
                </div>
            </div>

            <div class="row mr-lg-5 ml-lg-5 mb-3">
                <div class="col-md-12 pt-3">
                    <label for="txtDescription" style="font-weight: 600">Description</label>
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" placeholder="Enter Job Description"
                        TextMode="MultiLine" OnKeyUp="resizeTextArea(this)"
                        Style="overflow-y: hidden; resize: none; min-height: 80px;" required>
                    </asp:TextBox>
                </div>
            </div>

            <div class="row mr-lg-5 ml-lg-5 mb-3">
                <div class="col-md-6 pt-3">
                    <label for="txtQualification" style="font-weight: 600">Qualification/Education Required</label>
                    <asp:TextBox ID="txtQualification" runat="server" CssClass="form-control"
                        placeholder="Enter Qualification/Education"
                        required>
                    </asp:TextBox>
                </div>

                <div class="col-md-6 pt-3">
                    <label for="txtExperience" style="font-weight: 600">Experience Required</label>
                    <asp:TextBox ID="txtExperience" runat="server" CssClass="form-control" placeholder="Ex: 2 Years, 1.5 Years"
                        TextMode="Number" required>
                    </asp:TextBox>
                </div>
            </div>

            <div class="row mr-lg-5 ml-lg-5 mb-3">
                <div class="col-md-6 pt-3">
                    <label for="txtSpecialization" style="font-weight: 600">Specialization Required</label>
                    <asp:TextBox ID="txtSpecialization" runat="server" CssClass="form-control"
                        placeholder="Enter txtSpecialization" required TextMode="MultiLine">
                    </asp:TextBox>
                </div>

                <div class="col-md-6 pt-3">
                    <label for="txtLastDate" style="font-weight: 600">Last Date To Apply</label>
                    <asp:TextBox ID="txtLastDate" runat="server" CssClass="form-control" placeholder="Enter Last Date To Apply"
                        TextMode="Date" required>
                    </asp:TextBox>
                </div>
            </div>

            <div class="row mr-lg-5 ml-lg-5 mb-3">
                <div class="col-md-6 pt-3">
                    <label for="txtSalary" style="font-weight: 600">Salary</label>
                    <asp:TextBox ID="txtSalary" runat="server" CssClass="form-control"
                        placeholder="Enter Salary" required TextMode="MultiLine">
                    </asp:TextBox>
                </div>

                <div class="col-md-6 pt-3">
                    <label for="txtLastDate" style="font-weight: 600">Job Type</label>
                    <asp:DropDownList ID="ddlJobType" runat="server" CssClass="form-control">
                        <asp:ListItem Value="0">Select Job Type </asp:ListItem>
                        <asp:ListItem Value="FullTime">Full Time</asp:ListItem>
                        <asp:ListItem Value="PartTime">Part Time</asp:ListItem>
                        <asp:ListItem Value="Contract">Contract</asp:ListItem>
                        <asp:ListItem Value="Internship">Internship</asp:ListItem>
                        <asp:ListItem Value="Temporary">Temporary</asp:ListItem>
                        <asp:ListItem Value="Freelance">Freelance</asp:ListItem>
                        <asp:ListItem Value="Remote">Remote</asp:ListItem>
                        <asp:ListItem Value="WorkFromHome">Work From Home</asp:ListItem>
                        <asp:ListItem Value="Onsite">Onsite</asp:ListItem>
                        <asp:ListItem Value="Hybrid">Hybrid</asp:ListItem>
                        <asp:ListItem Value="Commission">Commission</asp:ListItem>
                        <asp:ListItem Value="Volunteer">Volunteer</asp:ListItem>
                        <asp:ListItem Value="Other">Other</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function resizeTextArea(textarea) {
            textarea.style.height = "auto";
            textarea.style.height = (textarea.scrollHeight) + "px";
        }
    </script>
</asp:Content>
