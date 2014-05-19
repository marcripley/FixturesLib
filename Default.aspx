<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <%--Added by: Jenise Marcus 5/9/14 - Used for multiple select dropdown checklist--%>
    <script type = "text/javascript">
        function CheckItem(checkBoxList) {
            var options = checkBoxList.getElementsByTagName('input');
            var arrayOfCheckBoxLabels = checkBoxList.getElementsByTagName("label");
            var s = "";

            //populates textbox of with the selected checklist items
            for (i = 0; i < options.length; i++) {
                var opt = options[i];
                if (opt.checked) {
                    s = s + ", " + arrayOfCheckBoxLabels[i].innerText;
                }
            }
            if (s.length > 0) {
                s = s.substring(2, s.length);
            }
            var TxtBox = document.getElementById("<%=tags0.ClientID%>");
            TxtBox.value = s;
            document.getElementById('<%=hidVal.ClientID %>').value = s;
        }
    </script>
</asp:Content>


<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <form id="fixturesSelect" runat="server">

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

        <%--<div class="fixturesFormDrop">
           

        </div>--%>
        <div class="clear">

        <asp:Label ID="topOfPage" runat="server" CssClass="MyStyle" visible="false" />
          

           <%-- <ContentTemplate>--%>
                <div class="fixturesFormDrop">
                    <label for="recipient"> &nbsp;&nbsp;&nbsp; CATEGORY&nbsp;&nbsp;</label>
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
                
                    <label for="recipient"> &nbsp; SUBCATEGORY</label>&nbsp;
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
                                        <asp:ListItem Text="Select" Value="0" />
                                        <asp:ListItem Text="All" Value="1" />
                    </asp:DropDownList>
          

               <%-- Added by: Jenise Marcus 5/8/14 - Changed Tags textbox to dropdown checkbox list for multiple Tag selections --%>               
                    <label for="recipient"> TAGS </label> &nbsp;&nbsp;                                         
                        <asp:TextBox ID="tags0" runat="server" ReadOnly="true" Width="200" Height="20px" />
                        <%--<asp:TextBox ID="tags0" runat="server" Height="20px"></asp:TextBox>--%>

       
                     <Ajax:PopupControlExtender ID="PopupControlExtender111" runat="server" TargetControlID="tags0" PopupControlID="Panel1" Position="Bottom">
                    </Ajax:PopupControlExtender>

                        <input type="hidden" name="hidVal" id="hidVal" runat="server" />
                        <asp:Panel ID="Panel1" runat="server">
                            <asp:CheckBoxList ID="chkList" 
                                runat="server" 
                                Height="20" onclick="CheckItem(this)">                                                                                                                                                                        
                            </asp:CheckBoxList>
                         </asp:Panel>
                        <%--<asp:AutoCompleteExtender ID="tags0_AutoCompleteExtender" runat="server" 
                            ServiceMethod="GetCompletionList" TargetControlID="tags0" UseContextKey="True" MinimumPrefixLength="2">
                        </asp:AutoCompleteExtender>--%>
                </div>


                <div class="fixturesTextInput">
                    <label for="recipient">JOB NUMBER &nbsp;&nbsp;&nbsp;</label>
                    <asp:TextBox ID="jobNumber0" runat="server" Height="20px" OnTextChanged="jobNumberText_TextChanged"></asp:TextBox>
                    
                    
                  <%--  button added by jenise marcus 5/8/14--%> 
                    &nbsp;<asp:Button ID="btnSubmit" runat="server" Text="Search" OnClick="btnSubmit_OnClick" />
                         </div>          

                <asp:Label ID="bottomOfPage" runat="server" CssClass="MyStyle" visible="false" />
           <%--</ContentTemplate>--%>

        

    </div>


    <div class="fixturesMain">
    <p></p><asp:Label ID="lbltest" runat="server" />
    
        <%--Jenise Marcus - 5/9/14 - display initial search results - Testing Only--%>
        <asp:GridView ID="gv" runat="server" AutoGenerateColumns="false" Visible="true" ShowHeader="false" >
        <Columns>
            <asp:TemplateField HeaderImageUrl="Images">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#"~/FixtureDetails.aspx?JID="+Eval("lJobNumber")+"&TID="+Eval("lTaskNumber") %>' Target="_self">
                        <img src='<%# Eval("Img1") %>' alt="no image" data-other-src="images/c3energySD.jpg" width="940" height="450" class="flFeaturedImage" title="C3 Energy | Other, Planter..." /><span class="ficturesProjTitle"></span>                   
                    </asp:HyperLink>
                    
                </ItemTemplate>
            </asp:TemplateField>  
        </Columns>
       </asp:GridView>

    </div>
    

    <div class="fixturesMain">
    
    
    </div>


    </form>
</asp:Content>
