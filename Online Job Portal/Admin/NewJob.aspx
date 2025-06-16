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
                    <asp:DropDownList ID="ddlJobType" runat="server" CssClass="form-control"
                        AppendDataBoundItems="true" DataTextField="JobType" DataValueField="JobType" Style="cursor: pointer;">
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="JobType is required"
                        ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small" ControlToValidate="ddlJobType"
                        InitialValue="0"></asp:RequiredFieldValidator>
                </div>
            </div>


            <div class="row mr-lg-5 ml-lg-5 mb-3">
                <div class="col-md-6 pt-3">
                    <label for="txtCompany" style="font-weight: 600">Comapany/Organizarion</label>
                    <asp:TextBox ID="txtCompany" runat="server" CssClass="form-control"
                        placeholder="Enter Comapany/Organizarion" required TextMode="MultiLine">
                    </asp:TextBox>
                </div>

                <div class="col-md-6 pt-3">
                    <label for="fuCompanyLogo" style="font-weight: 600">Comapany/Organizarion</label>
                    <asp:FileUpload ID="fuCompanyLogo" runat="server" CssClass="form-control" ToolTip=".jpg, .jpeg, .png extension only" />
                </div>
            </div>


            <div class="row mr-lg-5 ml-lg-5 mb-3">
                <div class="col-md-6 pt-3">
                    <label for="txtWebsite" style="font-weight: 600">Website</label>
                    <asp:TextBox ID="txtWebsite" runat="server" CssClass="form-control"
                        placeholder="Enter Website" TextMode="Url">
                    </asp:TextBox>
                </div>

                <div class="col-md-6 pt-3">
                    <label for="txtEmail" style="font-weight: 600">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Enter email"
                        TextMode="Email">
                    </asp:TextBox>
                </div>
            </div>


            <div class="row mr-lg-5 ml-lg-5 mb-3">
                <div class="col-md-12 pt-3">
                    <label for="txtAddress" style="font-weight: 600">Address</label>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Enter Work Location"
                        TextMode="MultiLine" OnKeyUp="resizeTextArea(this)"
                        Style="overflow-y: hidden; resize: none; min-height: 80px;" required>
                    </asp:TextBox>
                </div>
            </div>


            <div class="row mr-lg-5 ml-lg-5 mb-3">
                <div class="col-md-6 pt-3">
                    <label for="txtWebsite" style="font-weight: 600">Country</label>
                    <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control w-100"
                        AppendDataBoundItems="true" DataTextField="CountryName" DataValueField="CountryName">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please select a country."
                        ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small" ControlToValidate="ddlCountry"
                        InitialValue="0"></asp:RequiredFieldValidator>
                </div>

                <div class="col-md-6 pt-3">
                    <label for="txtState" style="font-weight: 600">State</label>
                    <asp:TextBox ID="txtState" runat="server" CssClass="form-control" placeholder="Enter State" required>
                    </asp:TextBox>
                </div>
            </div>

            <div class="row mr-lg-5 ml-lg-5 mb-3 pt-4">
                <div class="col-md-3 col-md-offset-2 mb-3">
                    <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary btn-block" BackColor="#7200cf" Text="Add Job"
                         OnClick="btnAdd_Click"/>
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
