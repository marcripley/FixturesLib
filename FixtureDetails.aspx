<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FixtureDetails.aspx.cs" Inherits="FixtureDetails" EnableViewState="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<form runat="server">

    <asp:Table ID="tblMain" runat="server" Width="95%" HorizontalAlign="Center">
        <asp:TableRow><asp:TableCell>&nbsp;</asp:TableCell></asp:TableRow>
        <asp:TableRow><asp:TableCell><span style="color:Maroon;font-style:italic;Font-Size:16px">Click on an image to enlarge it</span></asp:TableCell></asp:TableRow>
        
        <asp:TableRow runat="server">
            <asp:TableCell runat="server">
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
                <h2 id="flTitle" class="flDetailsHeader"><asp:Label ID="lblProjectName" runat="server" /></h2>

                <div class="flDetails" id="flDetailsOrg">

                <asp:Table ID="tblOrganizationInfo" runat="server">
                    <asp:TableRow><asp:TableCell><h3>Organization</h3></asp:TableCell></asp:TableRow>
                    <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Category:&nbsp;<span><asp:Label ID="lblCategory" runat="server" /></span></asp:TableCell></asp:TableRow>
                    <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Subcategory:&nbsp;<span><asp:Label ID="lblSubCat" runat="server" /></span></asp:TableCell></asp:TableRow>
                    <asp:TableRow><asp:TableCell ID="tcTags" runat="server">&nbsp;&nbsp;&nbsp;Tags:&nbsp;<asp:HyperLink ID="hlTags" runat="server" /></asp:TableCell></asp:TableRow>
                </asp:Table>
              
                </div>

            
            <%--<div class="flDetails" id="flDetailsLabor">--%>
            <h3><asp:Label ID="Label1" runat="server" Text="Labor Details" CssClass="flDetails" Font-Size="24px" /></h3><br />

            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pHeader" runat="server">
                        <h3><asp:Image ID="Image1" runat="server" />
                        <asp:Label ID="lblText" runat="server" Font-Size="16px" ForeColor="Maroon" /></h3>
                    </asp:Panel>

                    <asp:Panel ID="pBody" runat="server" CssClass="cpBody">
                        <asp:GridView ID="gvLaborDetails" runat="server" ShowFooter="true" AutoGenerateColumns="false"  
                            OnRowDataBound="gvLaborDetails_RowDataBound" cellspacing="10" BorderStyle="Solid" GridLines="Both" Width="100%">
                            <Columns>
                            <asp:BoundField HeaderText="Department" DataField="txtDepartmentDescription" />
                            <asp:BoundField HeaderText="Work Order Desc" DataField="txtWorkOrderDescription" FooterText="Total Hours" FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Center" /> 
                            <asp:TemplateField HeaderText="Budget Hours"  HeaderStyle-Wrap="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" FooterStyle-HorizontalAlign="Center" FooterStyle-Font-Bold="true" >
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "sWorkOrderHoursBudget") %>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="ProjSum" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Actual Hours" HeaderStyle-Wrap="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" FooterStyle-HorizontalAlign="Center" FooterStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "sWorkOrderHoursActual") %>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="ActualSum" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>                 
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
 
                    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" 
                                            runat="server" 
                                            TargetControlID="pBody" 
                                            CollapseControlID="pHeader" 
                                            ExpandControlID="pHeader"
                                            Collapsed="true" 
                                            TextLabelID="lblText" 
                                            ImageControlID="Image1" 
                                            CollapsedText="Click to Show Labor Details.." 
                                            ExpandedText="Click to Hide Labor Details.." 
                                            ExpandedImage="~/images/collapse.jpg"
                                            CollapsedImage="~/images/expand.jpg"
                                            CollapsedSize="0">
                    </asp:CollapsiblePanelExtender>
                </ContentTemplate>
            </asp:UpdatePanel>


            <div class="flDetails" id="flDetailsProject">
                <asp:Table ID="tblProjDetails" runat="server" CellPadding="1">
                    <asp:TableRow><asp:TableCell><h3>Project Details</h3></asp:TableCell></asp:TableRow>
                    <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Job Number:&nbsp;<b><asp:Label ID="lblJobNum" runat="server" /></b></asp:TableCell></asp:TableRow>
                    <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Job Location:&nbsp;<asp:Label ID="lblJobCity" runat="server" /></asp:TableCell></asp:TableRow>
                    <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;General Contractor:&nbsp;<asp:Label ID="lblGC" runat="server" /></asp:TableCell></asp:TableRow>
                    <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Architect:&nbsp;<asp:Label ID="lblArhitect" runat="server" /></asp:TableCell></asp:TableRow>
                    <%--<asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Job Close Date:&nbsp;<asp:Label ID="lblJobCloseDate" runat="server" /></asp:TableCell></asp:TableRow>--%>
                </asp:Table>
            </div>

            <div class="flDetails" id="flDetailsComments">
            <h3><asp:Label ID="lblCommentslbl" runat="server" Text="Comments:" CssClass="flDetails" /></h3>
            <asp:Label ID="lblComments" runat="server" CssClass="flComments" />
        </div>
        </asp:TableCell>

    </asp:TableRow>
</asp:Table>

</form>
</asp:Content>

