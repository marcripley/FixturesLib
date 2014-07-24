<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <%--Javascript used to populate the Tags textbox with the checklist of items selected--%>
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
                            ConnectionString="<%$ ConnectionStrings:MBData2005 %>" 
                            DataSourceMode="DataSet" />

      <%--SubCategory Drop down select comman is populated in code behind after Category has been selected--%>
        <asp:SqlDataSource ID="dsSubCategories" 
                            runat="server"    
                            ConnectionString="<%$ ConnectionStrings:MBData2005 %>" 
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

                <%--Tags checkbox list uses ajax & Javascript above--%>
                <asp:TableCell>
                    <div class="fixturesFormDrop">              
                        <label for="recipient"> TAGS </label> &nbsp;                                       
                        <asp:TextBox ID="tags0" runat="server" ReadOnly="true" Width="250" Height="18px" />
                        <Ajax:PopupControlExtender ID="PopupControlExtender111" 
                                                runat="server" 
                                                TargetControlID="tags0" 
                                                PopupControlID="Panel1" 
                                                Position="Bottom" />
                        <input type="hidden" name="hidVal" id="hidVal" runat="server" />
                        <asp:Panel ID="Panel1" runat="server" >
                            <asp:CheckBoxList ID="chkList" runat="server" Height="20" onclick="CheckItem(this)" BackColor="white" />
                        </asp:Panel>
                    </div>
                </asp:TableCell>

                <%--Simple Text to allow Job number data entry with or without "j"--%>
                <asp:TableCell>
                    <div class="fixturesTextInput">
                        <label for="recipient">JOB NUMBER&nbsp;</label>
                        <asp:TextBox ID="jobNumber0" runat="server" Height="18px" Width="90" />
                    </div>
                </asp:TableCell>

                <%--Submit and Clear buttons **In next phase, use ajax cascading dropdowns 
                    to eleminate submit if not using tags or job number as filter --%>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:Button ID="btnSubmit" runat="server" Text="Search" OnClick="btnSubmit_OnClick" />&nbsp;
                    <asp:Button ID="btnClear" runat="server" Text="Clear Search" OnClick="btnClear_OnClick" />
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow ID="trBlank" runat="server">
                <asp:TableCell ColumnSpan="5" runat="server">&nbsp;</asp:TableCell>
            </asp:TableRow>
            
            <asp:TableRow ID="trlblMessage" runat="server">
                <asp:TableHeaderCell ColumnSpan="5" runat="server">
                    <asp:Label ID="lblMessage" runat="server" visible="false" ForeColor="#660000" Font-Size="15px" />
                </asp:TableHeaderCell>
            </asp:TableRow>
        </asp:Table>
                

        <%--Gridview calls flPostSearch stored procedure and pulls primary image for selection--%> 
        <asp:GridView ID="gvPosts" runat="server" AutoGenerateColumns="false" Visible="true" ShowHeader="false" GridLines="None" Width="95%" 
                        HorizontalAlign="Center" OnRowDataBound="gvPosts_OnRowDataBound">
            <Columns>
                <asp:TemplateField HeaderImageUrl="Images" ItemStyle-HorizontalAlign="center">
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#"~/FixtureDetails.aspx?PostID="+Eval("PostID") %>' Target="_self">
                            <div class="img-wrap">
                            <asp:Image ID="imgOriginal" runat="server" Height="450" Width="940" AlternateText="No Image" />
                                <div class="img-overlay">
                                    <h3><asp:Label ID="lblOverlaytitle" runat="server" Text='<%# Eval("txtJobName") %>' ForeColor="Black" Font-Size="14px" Font-Bold="false" /></h3>
                                </div>
                            </div>
                            <asp:Label ID="lblimage1" runat="server" Visible="false" Text='<%# Eval("Img1") %>'  />                   
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>  
            </Columns>
        </asp:GridView>

</form>
</div>
</asp:Content>
