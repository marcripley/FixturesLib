<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FLAdminReview.aspx.cs" Inherits="FLAdminReview" MaintainScrollPositionOnPostback="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<script type="text/javascript">

    function validateCombobox() {
        var id = document.getElementById('<%=ddCategory.ClientID %>');
        var inputs = id.getElementsByTagName('input');
        var i;
                    
        for (i = 0; i < inputs.length; i++) {
            if (inputs[i].type == 'text') {
                if (inputs[i].value != "" && inputs[i].value != null) {
                    alert(inputs[i].value);
                    return true;
                }
                else {
                    alert("null value");
                    return false;
                }

                //break;
            }
        }
    }


    function validatecb() {
    if (document.getElementById('cbPostedStatus').checked){
    alert("checked");
    return true;    
    }
    else
    {
    alert("false");
    return false;
    }
    }
    
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<form runat="server">

<Ajax:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />

<%--Drop down list datasources--%>
    <asp:SqlDataSource ID="dsCategories" 
                        runat="server" 
                        SelectCommand="SELECT CategoryID, CategoryName FROM dbo.flCategories WHERE ParentID IS NULL" 
                        ConnectionString="<%$ ConnectionStrings:MBData2005 %>" 
                        DataSourceMode="DataSet" />

    <asp:SqlDataSource ID="dsSubCategories" 
                    runat="server"    
                    ConnectionString="<%$ ConnectionStrings:MBData2005 %>" 
                    DataSourceMode="DataSet" />

     <asp:SqlDataSource ID="dsTags" 
                    runat="server"
                    SelectCommand="SELECT TagId, Tags FROM dbo.flTags"     
                    ConnectionString="<%$ ConnectionStrings:MBData2005 %>" 
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

        <h3 class="AdminTitle"><asp:Label ID="lblPendingTitle" runat="server" Font-Underline="true" /></h3><br /> 

            <asp:GridView ID="gvPending" runat="server" AutoGenerateColumns="false" CellPadding="3" ShowHeader="false" BorderStyle="None">
                <Columns>
                    <asp:TemplateField>
                    <ItemTemplate>
                        <div class="flReviewList">
                        <asp:HyperLink ID="HyperLink1" runat="server" CssClass="list" NavigateUrl='<%#"~/FLAdminReview.aspx?PostID="+Eval("PostID") %>' Target="_self">
                            <asp:Label id="lblhl" runat="server" Text='<%# Eval("txtJobNumber") + " | " + Eval("txtTaskNumber") + " | " + Eval("txtJobName") + " | By: " + Eval("txtEmployeeName") + " | On: " + Eval("CreatedDate") %>' />
                        </asp:HyperLink></div>
                    </ItemTemplate>    
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>


            <br />
            <br />
            <br />
            <h3 class="AdminTitle"><asp:Label ID="lblHistory" runat="server" Font-Underline="true" /></h3><br />

            <asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="false" CellPadding="3" ShowHeader="false" BorderStyle="None">
                <Columns>
                    <asp:TemplateField>
                    <ItemTemplate>
                        <div class="flReviewList">
                        <asp:HyperLink ID="HyperLink1" runat="server" CssClass="list" NavigateUrl='<%#"~/FLAdminReview.aspx?PostID="+Eval("PostID") %>' Target="_self">
                            <asp:Label id="lblhl" runat="server" Text='<%# Eval("txtJobNumber") + " | " + Eval("txtTaskNumber") + " | " + Eval("txtJobName") + " | By: " + Eval("txtEmployeeName") + " | On: " + Eval("PostedDate") %>' />
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
                            OnRowDataBound="gvLaborDetails_RowDataBound" BorderStyle="Solid" GridLines="Both" CellSpacing="12" Width="100%">
                            <Columns>
                            <asp:BoundField HeaderText="Department" DataField="txtDepartmentDescription" />
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
                        <asp:Table ID="tblProjDetails" runat="server" CssClass="flProjDetails">
                            <asp:TableRow><asp:TableCell><h3 class="flProjectHeader">Project Details</h3></asp:TableCell></asp:TableRow>
                            <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Job Number:&nbsp;<b><asp:Label ID="lblJobNum" runat="server" /></b></asp:TableCell></asp:TableRow>
                            <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Job Location:&nbsp;<asp:Label ID="lblJobCity" runat="server" /></asp:TableCell></asp:TableRow>
                            <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;General Contractor:&nbsp;<asp:Label ID="lblGC" runat="server" /></asp:TableCell></asp:TableRow>
                            <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Architect:&nbsp;<asp:Label ID="lblArhitect" runat="server" /></asp:TableCell></asp:TableRow>
                        </asp:Table>                    
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow ID="trComments" runat="server">
                    <asp:TableCell>
                        <asp:Label ID="lblComments" runat="server" Text="Comments:" Font-Size="14" /><br />
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
                                    <div class="flLabels">
                                        <asp:Label ID="lblAC" runat="server" Text="Assign Category:" /> 
                                    </div>
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
                                    <div class="flLabels">
                                        <asp:Label ID="lblASC" runat="server" Text="Assign Subcategory:" />
                                    </div>       
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
                        <div class="flLabels">
                            <asp:label ID="lblSelectTags" runat="server" Text="Select Tags (3 min):" />
                        </div>
                        <div style="overflow-y:scroll; width:650px; height:100px">
                        <asp:CheckBoxList ID="cblTags" 
                                        runat="server" 
                                        DataSourceID="dsTags" 
                                        RepeatColumns="7" 
                                        CellPadding="2" 
                                        RepeatDirection="Horizontal" 
                                        DataTextField="Tags" 
                                        DataValueField="TagID" Font-Size="14px" 
                                        OnDataBound="cblTags_OnDataBound"/></div>
                    </asp:TableCell>
                </asp:TableRow>


                <asp:TableRow ID="trUploads" runat="server">
                    <asp:TableCell>
                        <div class="flLabels">      
                        <asp:Label runat="server" ID="lblCurrPrimFile" Visible="false" Font-Underline="true" Text="Existing Image(s): " />
                        <asp:Button ID="btnAddImages" runat="server" Text="Add Images" OnClick="btnAddImages_Onclick" Visible="false" />
                        <asp:Label ID="lblAddImagesNote" runat="server" Text="**Adding images will replace the existing images listed below." ForeColor="Gray" /><br />
                        <%--<asp:TextBox ID="txtCurrPrimFile" runat="server" Visible="false" Width="500" Rows="7" TextMode="MultiLine" BorderColor="AliceBlue" />--%>
                       <asp:Label ID="txtCurrPrimFile" runat="server" Visible="false" Width="600" Font-Size="14px" />
                        <asp:Table ID="tblAdditionalImages" runat="server">
                            <asp:TableRow>
                                <asp:TableCell></asp:TableCell><asp:TableCell>Small Image</asp:TableCell><asp:TableCell>Large Image</asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="Label1" runat="server" Text="Primary Image:" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:FileUpload ID="PrimaryfileUpload" runat="server" ForeColor="Red" BorderColor="AliceBlue" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:FileUpload ID="PrimaryfileUploadl" runat="server" ForeColor="Red" BorderColor="AliceBlue" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="Label8" runat="server" Text="Upload Image:" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:FileUpload ID="FileUpload2s" runat="server" BorderColor="AliceBlue" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:FileUpload ID="FileUpload2l" runat="server" BorderColor="AliceBlue" />
                                </asp:TableCell>
                            </asp:TableRow>
                             <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="Label9" runat="server" Text="Upload Image:" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:FileUpload ID="FileUpload3s" runat="server" BorderColor="AliceBlue" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:FileUpload ID="FileUpload3l" runat="server" BorderColor="AliceBlue" />
                                </asp:TableCell>
                            </asp:TableRow>
                             <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="Label10" runat="server" Text="Upload Image:" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:FileUpload ID="FileUpload4s" runat="server" BorderColor="AliceBlue" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:FileUpload ID="FileUpload4l" runat="server" BorderColor="AliceBlue" />
                                </asp:TableCell>
                            </asp:TableRow>
                             <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="Label11" runat="server" Text="Upload Image:" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:FileUpload ID="FileUpload5s" runat="server" BorderColor="AliceBlue" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:FileUpload ID="FileUpload5l" runat="server" BorderColor="AliceBlue" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                        
                        </div>                       
                    </asp:TableCell>
                </asp:TableRow>

            <asp:TableRow ID="trPostedcb" runat="server">
                <asp:TableCell>
                    <div class="flLabels">
                    <br />
                        <asp:CheckBox ID="cbPostedStatus" runat="server" Text="Ready To Post" Font-Size="18px" ForeColor="Maroon" /> 
                        <asp:CheckBox ID="cbArchive" runat="server" Text="Archive" Font-Size="18px" ForeColor="Maroon" Visible="false" /> 
                    </div>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow ID="trButtons" runat="server">
                <asp:TableCell>
                    <br /><asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_OnClick" Font-Size="16px" OnClientClick="return validatecb();" />
                </asp:TableCell>
            </asp:TableRow>

        </asp:Table>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>

</form>
</asp:Content>