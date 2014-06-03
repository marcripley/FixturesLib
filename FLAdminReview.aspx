<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FLAdminReview.aspx.cs" Inherits="FLAdminReview" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<form runat="server">

<Ajax:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />

<asp:SqlDataSource ID="dsCategories" 
                        runat="server" 
                        SelectCommand="SELECT CategoryID, CategoryName FROM flCategories WHERE ParentID IS NULL" 
                        ConnectionString="<%$ ConnectionStrings:MBData2005_DEV %>" 
                        DataSourceMode="DataSet" />

    <asp:SqlDataSource ID="dsSubCategories" 
                    runat="server"    
                    ConnectionString="<%$ ConnectionStrings:MBData2005_DEV %>" 
                    DataSourceMode="DataSet" />

     <asp:SqlDataSource ID="dsTags" 
                    runat="server"
                    SelectCommand="SELECT TagId, Tags FROM flTags"     
                    ConnectionString="<%$ ConnectionStrings:MBData2005_DEV %>" 
                    DataSourceMode="DataSet" />


<h2 align="center">Fixtures Pending Approval</h2>
<p></p>
<asp:Label ID="lblMessage" runat="server" />

<asp:Table ID="tblMain" runat="server" Width="80%" HorizontalAlign="Center">
    <asp:TableRow>
        <asp:TableCell ID="tcList" runat="server" HorizontalAlign="Left" BorderStyle="None" VerticalAlign="Top">
           
            <asp:GridView ID="gvPending" runat="server" AutoGenerateColumns="false" CellPadding="3" ShowHeader="false" BorderStyle="None">
                <Columns>
                    <asp:TemplateField HeaderImageUrl="Images" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#"~/FLAdminReview.aspx?JID="+Eval("lJobNumber")+"&TID="+Eval("lTaskNumber") %>' Target="_self">
                            <asp:Label id="lblhl" runat="server" Text='<%# Eval("txtJobNumber") + " | " + Eval("txtJobName") + " | " + Eval("txtEmployeeName") + " | " + Eval("CreatedDate") %>' />
                        </asp:HyperLink>
                    </ItemTemplate>    
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:TableCell>
        

        <asp:TableCell ID="tcData" runat="server" HorizontalAlign="Left" VerticalAlign="Middle">
            
            <h3><asp:Label ID="lblProjectName" runat="server" cssClass="flDetails" Font-Size="24px" ForeColor="Maroon" /></h3><br />

            <h3><asp:Label ID="Label1" runat="server" Text="Labor Details" CssClass="flDetails" Font-Size="24px" /></h3><br />
           
            <asp:Table ID="tblDetails" runat="server"><asp:TableRow ID="trLaborgv" runat="server"><asp:TableCell>
            <asp:GridView ID="gvLaborDetails" runat="server" ShowFooter="true" AutoGenerateColumns="false"  
                OnRowDataBound="gvLaborDetails_RowDataBound" cellspacing="10" BorderStyle="Solid" GridLines="Both" Width="100%">
                <Columns>
                <asp:BoundField HeaderText="Department" DataField="txtDepartmentDescription" />
                <asp:BoundField HeaderText="Fac" DataField="txtFacilityShortName" ItemStyle-HorizontalAlign="Center" />
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
            </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow ID="trProjDetails" runat="server"><asp:TableCell>

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
            </asp:TableCell></asp:TableRow>
            <asp:TableRow ID="trComments" runat="server"><asp:TableCell>
            <asp:TextBox ID="txtComments" runat="server" Rows="4" TextMode="MultiLine" Width="90%" />
            
            </asp:TableCell></asp:TableRow>

            <asp:TableRow ID="trCheckboxes" runat="server"><asp:TableCell>
            <asp:CheckBox ID="cbApprove"  runat="server" Text="Approve" OnCheckedChanged="cbApprove_OnCheckChanged" AutoPostBack="true" />&nbsp;&nbsp;
            <asp:CheckBox ID="cbDecline" runat="server" Text="Decline" OnCheckedChanged="cbDecline_OnCheckChanged" AutoPostBack="true" />
            <p></p>
            </asp:TableCell></asp:TableRow>
            <asp:TableRow ID="trCategoryDD" runat="server"><asp:TableCell>
            Assign Category:<Ajax:ComboBox ID="ddCategory" 
                            runat="server" 
                            DropDownStyle="DropDown" 
                            AutoCompleteMode="None"
                            CaseSensitive="false"
                            RenderMode="Inline"
                            ItemInsertLocation="Append" 
                            DataSourceID="dsCategories"
                             DataTextField="CategoryName" DataValueField="CategoryID" AutoPostBack="true" OnSelectedIndexChanged="ddCategory_SelectedIndexChanged" Height="20px" Width="181px"
                            ListItemHoverCssClass="ComboBoxListItemHover">
                            </Ajax:ComboBox>
                 </asp:TableCell></asp:TableRow>
                 
                 <asp:TableRow ID="trSubCatDD" runat="server"><asp:TableCell>
               &nbsp;&nbsp; Assign SubCategory:               
                     <Ajax:ComboBox ID="ddSubCategory" 
                                    runat="server" 
                            DropDownStyle="DropDown" 
                            AutoCompleteMode="None"
                            CaseSensitive="false"
                            RenderMode="Inline"
                            ItemInsertLocation="Append" 
                            DataSourceID="dsSubCategories"
                             DataTextField="CategoryName" DataValueField="CategoryID" AutoPostBack="true" Height="20px" Width="181px"
                            ListItemHoverCssClass="ComboBoxListItemHover">
                            </Ajax:ComboBox>
                            <p></p>
</asp:TableCell></asp:TableRow>

<asp:TableRow ID="trTagscbl" runat="server"><asp:TableCell>
                            Select Tags (min 3 Required):<asp:CheckBoxList ID="cblTags" runat="server" DataSourceID="dsTags" RepeatColumns="4" RepeatDirection="Horizontal" DataTextField="Tags" DataValueField="TagID" />
                            <p></p>
                            </asp:TableCell></asp:TableRow>

                            <asp:TableRow ID="trUploads" runat="server"><asp:TableCell>
                            Upload Primary Image:<asp:FileUpload ID="PrimaryfileUpload" runat="server" /><br />
                            
                            Upload Additional Images:<ajax:AjaxFileUpload ID="AjaxFileUpload1"
                                                ThrobberID="myThrobber"
                                                ContextKeys="fred"
                                                MaximumNumberOfFiles="10"
                                                runat="server"/>
                                                <p></p>
                                                </asp:TableCell></asp:TableRow>

                                                <asp:TableRow ID="trButtons" runat="server"><asp:TableCell>
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_OnClick" />
                            </asp:TableCell></asp:TableRow>
        </asp:Table>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>

</form>
</asp:Content>

