<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" EnableViewState="true" %>
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

        <Ajax:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    
    <%--Drop down list Datasources--%>
        <asp:SqlDataSource ID="dsCategories" 
                            runat="server" 
                            SelectCommand="SELECT CategoryID, CategoryName FROM flCategories WHERE ParentID IS NULL" 
                            ConnectionString="<%$ ConnectionStrings:MBData2005_DEV %>" 
                            DataSourceMode="DataSet" />

        <asp:SqlDataSource ID="dsSubCategories" 
                            runat="server"    
                            ConnectionString="<%$ ConnectionStrings:MBData2005_DEV %>" 
                            DataSourceMode="DataSet" />

    <%--Images Displayed in Table below--%>
        <asp:Table ID="tblDropdowns" runat="server" Width="85%" HorizontalAlign="Center">
            <asp:TableRow><asp:TableCell><p>&nbsp;</p></asp:TableCell></asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <div class="fixturesFormDrop">
                    <label for="recipient">CATEGORY</label>
                    <asp:DropDownList ID="categoryList0" 
                                        DataSourceID="dsCategories"
                                        DataTextField="CategoryName" 
                                        DataValueField="CategoryID"
                                        runat="server" 
                                        AutoPostBack="True" 
                                        OnSelectedIndexChanged="categoryList_SelectedIndexChanged" 
                                        Height="20px" 
                                        Width="181px" 
                                        AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select" Value="0" />
                                        <asp:ListItem Text="All" Value="1" />
                        </asp:DropDownList>
                        </div>
                </asp:TableCell>

                <%--SubCategory List Populated by Category Selected in code behind--%>
                <asp:TableCell>
                    <div class="fixturesFormDrop">
                    <label for="recipient">SUBCATEGORY</label>
                    <asp:DropDownList ID="subcategoryList0" 
                                        runat="server" 
                                        DataSourceID="dsSubCategories"
                                        DataTextField="CategoryName" 
                                        DataValueField="CategoryID" 
                                        Height="20px"
                                        OnSelectedIndexChanged="subcategoryList_SelectedIndexChanged" 
                                        Width="180px" 
                                        AutoPostBack="True" 
                                        AppendDataBoundItems="true">             
                    </asp:DropDownList>
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
        


        <asp:GridView ID="gvPosts" runat="server" AutoGenerateColumns="false" Visible="true" ShowHeader="false" GridLines="None" Width="940" HorizontalAlign="Center" 
                OnRowDataBound="gvPosts_OnRowDataBound" DataKeyNames="PostID">
            <Columns>       
                <asp:TemplateField HeaderImageUrl="Images" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%--<asp:Label ID="lblimage1" runat="server" Visible="false" Text='<%# Eval("Img1") %>'  />--%>
                        <div id="slideshow0">
                        
                            <asp:ListView ID="lvPics" runat="server" AutoGenerateColumns="false" Visible="true" ShowHeader="false">
                                <LayoutTemplate>
                                    <ul class="bjqs">
                                        <%--<div id="slider">
                                      <a href="#" class="control_next">>></a>
                                      <a href="#" class="control_prev"><</a></div>--%>
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />

                                        <%--<div class="img-overlay">
                                            <h3 align="center"><asp:Label ID="lblOverlaytitle" runat="server" Text='<%# Eval("txtJobName") %>' ForeColor="White" /></h3>
                                        </div>--%>

                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li>
                                        <img src='<%# Eval("flImageThumb") %>' alt="no image" width="940" height="450" title='<%# Eval("txtJobName") %>' />
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



        <%--<asp:GridView ID="gvPosts" runat="server" AutoGenerateColumns="false" Visible="true" ShowHeader="false" GridLines="None" Width="940" 
                        HorizontalAlign="Center" OnRowDataBound="gvPosts_OnRowDataBound">
            <Columns>
                <asp:TemplateField HeaderImageUrl="Images" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#"~/FixtureDetails.aspx?PostID="+Eval("PostID") %>' Target="_self">
                            <div class="img-wrap">
                            <asp:Image ID="imgOriginal" runat="server" Height="300" Width="550" AlternateText="No Image" />
                                <div class="img-overlay">
                                    <h3 align="center"><asp:Label ID="lblOverlaytitle" runat="server" Text='<%# Eval("txtJobName") %>' ForeColor="White" /></h3>
                                </div>
                            </div>
                            <asp:Label ID="lblimage1" runat="server" Visible="false" Text='<%# Eval("Img1") %>'  />                   
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>  
            </Columns>
        </asp:GridView>--%>

        </form>
    </div>
</asp:Content>