<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FixtureDetails.aspx.cs" Inherits="FixtureDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<form runat="server">
 <p></p>
    <asp:Table ID="tblMain" runat="server">
        <asp:TableRow runat="server">

            <asp:TableCell runat="server"><p>click on an image to enlarge it</p>
                <asp:GridView ID="gvImages" runat="server" ShowHeader="false" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("flImageThumb") %>' rel="shadowbox[c3-energy]">
                                    <img src='<%# Eval("flImageLarge") %>' alt="no image" width="940" height="450" />              
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:TableCell>


            <asp:TableCell VerticalAlign="Top">
                <asp:Table ID="tblOrganizationInfo" runat="server" CssClass="flDetails">
                    <asp:TableHeaderRow><asp:TableHeaderCell><h3>Organization</h3></asp:TableHeaderCell></asp:TableHeaderRow>
                    <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Category:<asp:Label ID="Label5" runat="server" /></asp:TableCell></asp:TableRow>
                    <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Subcategory:<asp:Label ID="Label6" runat="server" /></asp:TableCell></asp:TableRow>
                    <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Tags:<asp:Label ID="Label7" runat="server" /></asp:TableCell></asp:TableRow>
                </asp:Table>
                <p></p>


            <h2 id="flTitle"><asp:Label ID="lblProjectName" runat="server" /></h2>

            

            <h3><asp:Label ID="lblLD" runat="server" Text="Labor Details" /></h3>

            <asp:GridView ID="gvLaborDetails" runat="server" ShowFooter="true" AutoGenerateColumns="false" CssClass="flDetails" GridLines="None" OnRowDataBound="gvLaborDetails_RowDataBound">
                <Columns>
                <asp:BoundField HeaderText="Department" DataField="txtDepartmentDescription" />
                <asp:BoundField HeaderText="Fac" DataField="txtFacilityShortName" />
                <asp:BoundField HeaderText="Work Order Desc" DataField="txtWorkOrderDescription" FooterText="Total Hours" /> 
                <asp:TemplateField HeaderText="Budget Hours">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "sWorkOrderHoursBudget") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="ProjSum" runat="server" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Actual Hours">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "sWorkOrderHoursActual") %>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="ActualSum" runat="server" />
                    </FooterTemplate>
                </asp:TemplateField>                 
                    <%--<asp:BoundField HeaderText="Budget Hours" DataField="sWorkOrderHoursBudget" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="Actual Hours" DataField="sWorkOrderHoursActual" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField><FooterTemplate>
                    
                    <asp:Label ID="ActualSum" runat="server" />
                </FooterTemplate></asp:TemplateField>--%>
                </Columns>
            </asp:GridView><p></p>

     

            <div class="flDetails" id="flDetailsProject">

            <asp:Table ID="tblProjDetails" runat="server" CssClass="flDetails">
                <asp:TableHeaderRow><asp:TableHeaderCell><h3>Project Details</h3></asp:TableHeaderCell></asp:TableHeaderRow>
                <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Job Number:<b><asp:Label ID="lblJobNum" runat="server" /></b></asp:TableCell></asp:TableRow>
                <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Job Location:<asp:Label ID="lblJobCity" runat="server" /></asp:TableCell></asp:TableRow>
                <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;General Contractor:<asp:Label ID="lblGC" runat="server" /></asp:TableCell></asp:TableRow>
                <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Architect:<asp:Label ID="lblArhitect" runat="server" /></asp:TableCell></asp:TableRow>
                <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Job Close Date:<asp:Label ID="lblJobCloseDate" runat="server" /></asp:TableCell></asp:TableRow>
            </asp:Table>

            </div>

            <div class="flDetails" id="flDetailsComments">
            <h2>><asp:Label ID="lblCommentslbl" runat="server" Text="Comments" /></h2
            <asp:Label ID="lblComments" runat="server" />
        </div>
        </asp:TableCell>

    </asp:TableRow>
</asp:Table>

</form>
</asp:Content>

