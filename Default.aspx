<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <form id="fixturesSelect" runat="server">
    <div class="fixturesFormDrop">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        <Scripts>
       
        </Scripts>
        </asp:ToolkitScriptManager>
    </div>
    <div class="clear">
        <asp:Label ID="topOfPage" runat="server" CssClass="MyStyle" visible="false" />
        <asp:UpdatePanel runat="server">

            <ContentTemplate>
                <label for="recipient">
                    &nbsp;&nbsp;&nbsp; CATEGORY&nbsp;&nbsp;
                </label>
                <asp:DropDownList ID="categoryList0" runat="server" AutoPostBack="True" OnSelectedIndexChanged="categoryList_SelectedIndexChanged"
                    Height="20px" Width="181px">
                </asp:DropDownList>
                <div class="fixturesFormDrop">
                    <label for="recipient">
                        SUBCATEGORY</label>&nbsp;
                    <asp:DropDownList ID="subcategoryList0" runat="server" DataTextField="name" Height="20px"
                        OnSelectedIndexChanged="subcategoryList_SelectedIndexChanged" Width="180px" AutoPostBack="True">
                    </asp:DropDownList>
                </div>
                <div class="fixturesTextInput">
                    <label for="recipient">
                        TAGS</label>
                    <div class="fixturesFormItem">
                        <asp:TextBox ID="tags0" runat="server" Height="20px"></asp:TextBox>
                        &nbsp;</div>
                    <a href="fixturesSeeAll">See All Tags</a>
                </div>
                <div class="fixturesTextInput">
                    <label for="recipient">
                        JOB NUMBER&nbsp;&nbsp;
                    </label>
                    <asp:TextBox ID="jobNumber0" runat="server" Height="20px" OnTextChanged="jobNumberText_TextChanged"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="jobNumber0_FilteredTextBoxExtender" 
                        runat="server" TargetControlID="jobNumber0" FilterType="Numbers">
                    </asp:FilteredTextBoxExtender>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="fixturesSeeAll">See All Jobs</a> &nbsp;</div>
        <asp:Label ID="bottomOfPage" runat="server" CssClass="MyStyle" visible="false" />
            </ContentTemplate>

        </asp:UpdatePanel>

    </div>
    </form>
</asp:Content>
