<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FLAdminReview.aspx.cs" Inherits="FLAdminReview" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<script type = "text/javascript">
    function uploadComplete(sender) {
        $get("<%=lblUploadMsg.ClientID%>").innerHTML = "File Uploaded Successfully";
    }
    function uploadError(sender) {
        $get("<%=lblUploadMsg.ClientID%>").innerHTML = "File upload failed.";
    } 
</script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<form runat="server">
<Ajax:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />

<%--Drop down list datasources--%>
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

<div id="MainTitle" runat="server" class="flAdminHeader">
<h2><asp:label ID="lblMainTitle" runat="server" /></h2></div>
<p></p>
<div id="AdminMessage" runat="server" class="flAdminMessage">
<h3><asp:Label ID="lblMessage" runat="server" Visible="false" ForeColor="Maroon" /></h3></div>

<asp:Table ID="tblMain" runat="server" Width="90%" HorizontalAlign="Center">
    <asp:TableRow>
    <%--Contains List of Pending items for Committee and List of Approved items for Admin--%>
        <asp:TableCell ID="tcList" runat="server" HorizontalAlign="Left" BorderStyle="None" VerticalAlign="Top">
            <asp:GridView ID="gvPending" runat="server" AutoGenerateColumns="false" CellPadding="3" ShowHeader="false" BorderStyle="None">
                <Columns>
                    <asp:TemplateField>
                    <ItemTemplate>
                        <div class="flReviewList">
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#"~/FLAdminReview.aspx?JID="+Eval("lJobNumber")+"&TID="+Eval("lTaskNumber") %>' Target="_self">
                            <asp:Label id="lblhl" runat="server" Text='<%# Eval("txtJobNumber") + " | " + Eval("txtTaskNumber") + " | " + Eval("txtJobName") + " | " + Eval("txtEmployeeName") + " | " + Eval("CreatedDate") %>' />
                        </asp:HyperLink></div>
                    </ItemTemplate>    
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:TableCell>
        

        <%--Right Hand side of the page containing Job/Labor Details and FL fields--%>
        <asp:TableCell ID="tcData" runat="server" HorizontalAlign="Left" VerticalAlign="Middle">
            
            <h3 class="flProjectTitle"><asp:Label ID="lblProjectName" runat="server" /></h3><br />

            <h3 class="flLaborHeader"><asp:Label ID="lblLaborDetailsHeader" runat="server" Text="Labor Details" /></h3><br />
           
            <asp:Table ID="tblDetails" runat="server" Width="100%">
                <%--Labor Details table--%>
                <asp:TableRow ID="trLaborgv" runat="server">
                    <asp:TableCell>
                        <asp:GridView ID="gvLaborDetails" runat="server" ShowFooter="true" AutoGenerateColumns="false"  
                            OnRowDataBound="gvLaborDetails_RowDataBound" BorderStyle="Solid" GridLines="Both" CellSpacing="10" Width="100%">
                            <Columns>
                            <asp:BoundField HeaderText="Department" DataField="txtDepartmentDescription" />
                            <asp:BoundField HeaderText="Fac" DataField="txtFacilityShortName" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField HeaderText="Work Order Desc" DataField="txtWorkOrderDescription" FooterText="Total Hours" FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Center" /> 
                            <asp:TemplateField HeaderText="Budget Hours" HeaderStyle-Wrap="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" FooterStyle-HorizontalAlign="Center" FooterStyle-Font-Bold="true" >
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

                <%--Project Details--%>
                <asp:TableRow ID="trProjDetails" runat="server">
                    <asp:TableCell>
                        <div class="flDetails" id="flDetailsProject">
                        <asp:Table ID="tblProjDetails" runat="server">
                            <asp:TableRow><asp:TableCell><h3 class="flProjectHeader">Project Details</h3></asp:TableCell></asp:TableRow>
                            <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Job Number:&nbsp;<b><asp:Label ID="lblJobNum" runat="server" /></b></asp:TableCell></asp:TableRow>
                            <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Job Location:&nbsp;<asp:Label ID="lblJobCity" runat="server" /></asp:TableCell></asp:TableRow>
                            <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;General Contractor:&nbsp;<asp:Label ID="lblGC" runat="server" /></asp:TableCell></asp:TableRow>
                            <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Architect:&nbsp;<asp:Label ID="lblArhitect" runat="server" /></asp:TableCell></asp:TableRow>
                            <%--<asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Job Close Date:&nbsp;<asp:Label ID="lblJobCloseDate" runat="server" /></asp:TableCell></asp:TableRow>--%>
                        </asp:Table>
                        </div>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow ID="trComments" runat="server">
                    <asp:TableCell>
                        <asp:Label ID="lblComments" runat="server" Text="Comments:" Font-Size="14" ForeColor="Silver" /><br />
                        <asp:TextBox ID="txtComments" runat="server" Rows="5" TextMode="MultiLine" Width="100%" /> 
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow ID="trCheckboxes" runat="server">
                    <asp:TableCell>
                    <div class="flDetails">
                        <asp:CheckBox ID="cbApprove"  runat="server" Text="Approve" OnCheckedChanged="cbApprove_OnCheckChanged" AutoPostBack="true" Font-Size="16px"/>&nbsp;&nbsp;
                        <asp:CheckBox ID="cbDecline" runat="server" Text="Decline" OnCheckedChanged="cbDecline_OnCheckChanged" AutoPostBack="true" Font-Size="16px" />
                    </div>
                    </asp:TableCell>
                </asp:TableRow>


                <asp:TableRow ID="trCategoryDD" runat="server">
                    <asp:TableCell>
                        <asp:Table ID="tblddlists" runat="server" CellPadding="15">
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblAC" runat="server" Text="Assign Category:" Font-Size="14px" /> 
                                    <Ajax:ComboBox ID="ddCategory" 
                                                    runat="server"  
                                                    ItemInsertLocation="Append" 
                                                    DataSourceID="dsCategories"
                                                    DataTextField="CategoryName" 
                                                    DataValueField="CategoryID" 
                                                    AutoPostBack="true" 
                                                    OnSelectedIndexChanged="ddCategory_SelectedIndexChanged" 
                                                    CssClass="CustomComboBoxStyle" 
                                                    AppendDataBoundItems="true" 
                                                    OnItemInserted="ddCategory_OnItemInserted">
                                    </Ajax:ComboBox>
                                </asp:TableCell>
                                    
                                <asp:TableCell>
                                    <asp:Label ID="lblASC" runat="server" Text="Assign Subcategory:" Font-Size="14px" />       
                                     <Ajax:ComboBox ID="ddSubCategory" 
                                                    runat="server"  
                                                    ItemInsertLocation="Append" 
                                                    DataSourceID="dsSubCategories"
                                                    DataTextField="CategoryName" 
                                                    DataValueField="CategoryID" 
                                                    AutoPostBack="true" 
                                                    CssClass="CustomComboBoxStyle"  
                                                    OnItemInserted="ddSubCategory_OnItemInserted"/>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                     </asp:TableCell>
                </asp:TableRow>


                <asp:TableRow ID="trTagscbl" runat="server">
                    <asp:TableCell>
                        <asp:label ID="lblSelectTags" runat="server" Text="Select Tags (3 min):" Font-Size="14px" />
                        <asp:CheckBoxList ID="cblTags" runat="server" DataSourceID="dsTags" RepeatColumns="8" CellPadding="5" RepeatDirection="Horizontal" DataTextField="Tags" DataValueField="TagID"  Font-Size="14px"/>
                    </asp:TableCell>
                </asp:TableRow>


                <asp:TableRow ID="trUploads" runat="server">
                    <asp:TableCell>
                        <asp:Label ID="lblUploadPrim" runat="server" Text="Upload Primary Image:" Font-Size="14px" />&nbsp;
                        <asp:FileUpload ID="PrimaryfileUpload" runat="server" /><br />
                    
                        <asp:Label ID="lblUploadMltp" runat="server" Text="Upload Additional Images:" Font-Size="14px" />
                        <ajax:AjaxFileUpload ID="AsyncFileUpload" 
                                            OnClientUploadError="uploadError" 
                                            OnClientUploadComplete="uploadComplete"
                                            UploaderStyle="Modern" 
                                            CompleteBackColor="White" 
                                            UploadingBackColor="#CCFFFF" 
                                            ThrobberID="imgLoader"
                                            ContextKeys="fred"
                                            MaximumNumberOfFiles="10" 
                                            
                                            runat="server"/>
                            <asp:Image ID="imgLoader" runat="server" ImageUrl="/images/ProgressBar.gif" /> 
                            <br /> 
                            <asp:Label ID="lblUploadMsg" runat="server" Text="" />
                    </asp:TableCell>
                </asp:TableRow>


            <asp:TableRow ID="trButtons" runat="server">
                <asp:TableCell>
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_OnClick" Font-Size="14px" />
                </asp:TableCell>
            </asp:TableRow>

        </asp:Table>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>

</form>
</asp:Content>

