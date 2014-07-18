<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" EnableViewState="true" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <%--Javascript used to populate the Tags textbox with the checklist items selected--%>
    <script type = "text/javascript">
        function CheckItem(checkBoxList) {
            var options = checkBoxList.getElementsByTagName('input');
            var arrayOfCheckBoxLabels = checkBoxList.getElementsByTagName("label");
            var s = "";

            //Loop through selected checklist items
            for (i = 0; i < options.length; i++) {
                var opt = options[i];
                if (opt.checked) {
                    s = s + ", " + arrayOfCheckBoxLabels[i].innerText;
                }
            }
            if (s.length > 0) {
                s = s.substring(2, s.length);
            }
            //Populates textboxes
            var TxtBox_Tags = document.getElementById("<%=tags0.ClientID%>");
            TxtBox_Tags.value = s;
            document.getElementById('<%=hidVal.ClientID %>').value = s;
        }        
    </script>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<div class="fixturesFilter">
<form id="fixturesSelect" runat="server">

    <Ajax:ToolkitScriptManager runat="server" ID="ScriptManager1" />


    <%--Images Displayed in Table below--%>
        <asp:Table ID="tblDropdowns" runat="server" Width="85%" HorizontalAlign="Center">
            <asp:TableRow><asp:TableCell><p>&nbsp;</p></asp:TableCell></asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <div class="fixturesFormDrop">
                    <label for="recipient">CATEGORY</label>
                    <asp:DropDownList ID="categoryList0" 
                                        runat="server"
                                        Height="20px" 
                                        Width="181px">
                        </asp:DropDownList>
                        <Ajax:CascadingDropDown ID="cddCategories" 
                                                TargetControlID="categoryList0" 
                                                PromptText="Select Category" 
                                                PromptValue="0" 
                                                ServicePath="~/ServiceCS.asmx" 
                                                ServiceMethod="GetCategories" 
                                                runat="server" 
                                                Category="CategoryID" 
                                                LoadingText="Loading..." />
                        </div>
                </asp:TableCell>

                <%--SubCategory List Populated by Category Selected in code behind--%>
                <asp:TableCell>
                    <div class="fixturesFormDrop">
                    <label for="recipient">SUBCATEGORY</label>
                    <asp:DropDownList ID="subcategoryList0" 
                                        runat="server"  
                                        DataTextField="CategoryName" 
                                        DataValueField="CategoryID" 
                                        Height="20px"
                                        OnSelectedIndexChanged="subcategoryList_SelectedIndexChanged"   
                                        Width="180px" 
                                        AutoPostBack="True">             
                    </asp:DropDownList>
                    <Ajax:CascadingDropDown ID="cddSubCategories" 
                                            TargetControlID="subcategoryList0"   
                                            ServicePath="~/ServiceCS.asmx" 
                                            ServiceMethod="GetSubCategories" 
                                            runat="server" 
                                            Category="CategoryID" 
                                            ParentControlID="categoryList0" 
                                            LoadingText="Loading..." />
                    </div>
                </asp:TableCell>

                <asp:TableCell>
                    <div class="fixturesFormDrop">              
                    <label for="recipient"> TAGS </label> &nbsp;                                       
                    <asp:TextBox ID="tags0" runat="server" ReadOnly="true" Width="250" Height="18px" />
     
                        <Ajax:PopupControlExtender ID="PopupControlExtender111" runat="server" TargetControlID="tags0" PopupControlID="Panel1" Position="Bottom" />
                        <input type="hidden" name="hidVal" id="hidVal" runat="server" />
                        <asp:Panel ID="Panel1" runat="server">
                            <asp:CheckBoxList ID="chkList" runat="server" Height="20" onclick="CheckItem(this)" />
                        </asp:Panel>
                        </div>
                </asp:TableCell><asp:TableCell>
                    <div class="fixturesTextInput">
                        <label for="recipient">JOB NUMBER&nbsp;</label>
                        <asp:TextBox ID="jobNumber0" runat="server" Height="18px" Width="90" />
                    </div>
                </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                    <asp:Button ID="btnSubmit" runat="server" Text="Search" OnClick="btnSubmit_OnClick" />&nbsp;
                    <asp:Button ID="btnClear" runat="server" Text="Clear Search" OnClick="btnClear_OnClick" />
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow ID="trBlank" runat="server">
                <asp:TableCell ColumnSpan="5" runat="server">&nbsp;</asp:TableCell>
            </asp:TableRow>
            
            <asp:TableRow ID="trlblMessage" runat="server">
                <asp:TableCell ColumnSpan="5" runat="server">
                    <h2 id="h2_lblMessage" align="center"><asp:Label ID="lblMessage" runat="server" visible="false" ForeColor="#660000" Font-Size="15px" /></h2>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>

        <div id="wrapper">
  <div id="mainContent">


               <asp:UpdatePanel ID="panelimages" runat="server">
               <ContentTemplate>

        <asp:GridView ID="gvPosts" runat="server" AutoGenerateColumns="false" Visible="true" ShowHeader="false" GridLines="None" Width="940" Height="470" HorizontalAlign="Center" 
                OnRowDataBound="gvPosts_OnRowDataBound" DataKeyNames="PostID" BorderStyle="Solid" OnSelectedIndexChanging="gvPosts_OnSelectedIndexChanging">
            <Columns>       
                <asp:TemplateField HeaderImageUrl="Images" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%--<asp:Label ID="lblimage1" runat="server" Visible="false" Text='<%# Eval("Img1") %>'  />--%>
                        <div class="slider" id="slideshow<%# Container.DataItemIndex %>">
                        
                            <asp:ListView ID="lvPics" runat="server" AutoGenerateColumns="false" Visible="true" ShowHeader="false">
                                <LayoutTemplate>
                                    <ul class="bjqs">
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                        <%--<div class="img-overlay">
                                            <h3 align="center"><asp:Label ID="lblOverlaytitle" runat="server" Text='<%# Eval("txtJobName") %>' ForeColor="White" /></h3>
                                        </div>--%>
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li>
                                        <asp:LinkButton ID="lbImageDetails" runat="server" OnClick="lbImageDetails_OnClick" Text="select">
                                        <img src='<%# Eval("flImageThumb") %>' alt="no image" width="940" height="450" title='<%# Eval("txtJobName") %>' />
                                        </asp:LinkButton>
                                    </li>

                                </ItemTemplate>
                                <%--<EmptyDataTemplate>
                                    <p>Sorry, no images have been uploaded for this post yet.</p>
                                </EmptyDataTemplate>--%>
                            </asp:ListView>                                              
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>

            </ContentTemplate>
            </asp:UpdatePanel>
        </div>


            <section id="flDetailsSection">

            <%--<asp:UpdatePanel ID="updatepanel2" runat="server">
            <ContentTemplate>--%>

                <h2 id="H1" class="flDetailsHeader"><asp:Label ID="lblProjectName" runat="server" /></h2>

                <div class="flDetails" id="flDetailsOrg">

                <asp:Table ID="tblOrganizationInfo" runat="server" Visible="false">
                    <asp:TableRow><asp:TableCell><h3>Organization</h3></asp:TableCell></asp:TableRow>
                    <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Category:&nbsp;<span><asp:Label ID="lblCategory" runat="server" /></span></asp:TableCell></asp:TableRow>
                    <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Subcategory:&nbsp;<span><asp:Label ID="lblSubCat" runat="server" /></span></asp:TableCell></asp:TableRow>
                    <asp:TableRow><asp:TableCell ID="tcTags" runat="server">&nbsp;&nbsp;&nbsp;Tags:&nbsp;<asp:HyperLink ID="hlTags" runat="server" /></asp:TableCell></asp:TableRow>
                </asp:Table>

                </div>

                <h3><asp:Label ID="lblLaborDetailsHeader" runat="server" Text="Labor Details" CssClass="flDetails" Font-Size="24px" Visible="false" /></h3><br />

            
            <asp:UpdatePanel ID="upLaborDetails" runat="server" Visible="false">
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
 
                    <ajax:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" 
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
                    </ajax:CollapsiblePanelExtender>
                </ContentTemplate>
            </asp:UpdatePanel>

                <div class="flDetails" id="flDetailsProject">
                <asp:Table ID="tblProjDetails" runat="server" CellPadding="1" Visible="false">
                    <asp:TableRow><asp:TableCell><h3>Project Details</h3></asp:TableCell></asp:TableRow>
                    <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Job Number:&nbsp;<b><asp:Label ID="lblJobNum" runat="server" /></b></asp:TableCell></asp:TableRow>
                    <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Job Location:&nbsp;<asp:Label ID="lblJobCity" runat="server" /></asp:TableCell></asp:TableRow>
                    <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;General Contractor:&nbsp;<asp:Label ID="lblGC" runat="server" /></asp:TableCell></asp:TableRow>
                    <asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Architect:&nbsp;<asp:Label ID="lblArhitect" runat="server" /></asp:TableCell></asp:TableRow>
                    <%--<asp:TableRow><asp:TableCell>&nbsp;&nbsp;&nbsp;Job Close Date:&nbsp;<asp:Label ID="lblJobCloseDate" runat="server" /></asp:TableCell></asp:TableRow>--%>
                </asp:Table>
            </div>

            <div class="flDetails" id="flDetailsComments">
            <h3><asp:Label ID="lblCommentslbl" runat="server" Text="Comments:" CssClass="flDetails" Visible="false" /></h3>
            <asp:Label ID="lblComments" runat="server" CssClass="flComments" />
        </div>

        <%--</ContentTemplate>
       <%-- <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="lbImageDetails" EventName="Click" />
        </Triggers>-
        </asp:UpdatePanel>--%>
      </section>

    </div>

        </form>
    </div>
</asp:Content>