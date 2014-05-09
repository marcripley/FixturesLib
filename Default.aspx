<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
    <div class="fixturesFormDrop">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    </div>

    <div class="clear">
        <asp:Label ID="topOfPage" runat="server" CssClass="MyStyle" visible="false" />
        <asp:UpdatePanel runat="server">

            <ContentTemplate>
            <div class="fixturesFormDrop">
                <label for="recipient">
                    &nbsp;&nbsp;&nbsp; CATEGORY&nbsp;&nbsp;
                </label>
                <asp:DropDownList ID="categoryList0" runat="server" AutoPostBack="True" DataValueField="CategoryID" OnSelectedIndexChanged="categoryList_SelectedIndexChanged"
                    Height="20px" Width="181px">
                </asp:DropDownList>
                
                <label for="recipient">
                    &nbsp; SUBCATEGORY</label>&nbsp;
                <asp:DropDownList ID="subcategoryList0" runat="server" DataTextField="name" DataValueField="categoryId" Height="20px"
                    OnSelectedIndexChanged="subcategoryList_SelectedIndexChanged" Width="180px" AutoPostBack="True">
                </asp:DropDownList>
           


               <%-- Added by: Jenise Marcus 5/8/14 - Changed Tags textbox to dropdown checkbox list for multiple Tag selections --%>               
                    <label for="recipient"> TAGS </label> &nbsp;&nbsp;                                         
                        <asp:TextBox ID="tags0" runat="server" ReadOnly="true" Width="200" Height="20px" />
                        <%--<asp:TextBox ID="tags0" runat="server" Height="20px"></asp:TextBox>--%>
                        <ajaxToolkit:PopupControlExtender ID="PopupControlExtender111" runat="server" TargetControlID="tags0" PopupControlID="Panel111" Position="Bottom" />
                        <input type="hidden" name="hidVal" id="hidVal" runat="server" />
                        <asp:Panel ID="Panel111" runat="server" ScrollBars="Vertical" Width="200px" Height="180"  BorderWidth="1">
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
                    <asp:FilteredTextBoxExtender ID="jobNumber0_FilteredTextBoxExtender" runat="server" TargetControlID="jobNumber0" FilterType="Numbers">
                    </asp:FilteredTextBoxExtender></div>

                    
                  <%--  button added by jenise marcus 5/8/14--%> 
                    &nbsp;<asp:Button ID="btnSubmit" runat="server" Text="Search" OnClick="btnSubmit_OnClick" />
                                   

                <asp:Label ID="bottomOfPage" runat="server" CssClass="MyStyle" visible="false" />
            </ContentTemplate>

        </asp:UpdatePanel>

    </div>


    <div>
        <%--Jenise Marcus - 5/9/14 - display initial search results - Testing Only--%>
        <asp:GridView ID="gv" runat="server" AutoGenerateColumns="true" Visible="false">
        <Columns>
            <asp:BoundField DataField="lJobNumber" HeaderText="Job#" />
            <asp:BoundField DataField="lTaskNumber" HeaderText="Task#" />          
            <asp:TemplateField HeaderText="imagelink">
                <ItemTemplate>
                    <asp:HyperLink ID="hlid" Text='<%# Eval("lJobNumber") %>' runat="server" NavigateUrl="~/Default.aspx" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
       </asp:GridView>
    </div>
    

    <div>
       <%-- Jenise Marcus 5/9/14 - retrieve job details - Testing Only--%>
       
    </div>
    

    </form>
</asp:Content>
