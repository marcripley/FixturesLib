<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <form id="fixturesSelect" runat="server">
    <div class="fixturesFormDrop">
        <label for="recipient">
            CATEGORY</label>
        <asp:DropDownList ID="categoryList"  
            onselectedindexchanged="categoryList_SelectedIndexChanged" AutoPostBack="False" runat="server">
        </asp:DropDownList>
        <label for="recipient">
            SUBCATEGORY</label>&nbsp;
        <asp:DropDownList ID="subcategoryList" runat="server" Height="20px" OnSelectedIndexChanged="subcategoryList_SelectedIndexChanged"
            Width="177px" DataTextField="name">
        </asp:DropDownList>
    </div>
    <div class="fixturesTextInput">
        <label for="recipient">
            TAGS</label>
        <div class="fixturesFormItem">
            <asp:TextBox ID="tags" runat="server" Height="20px"></asp:TextBox>
            &nbsp;</div>
        <a href="fixturesSeeAll">See All Tags</a>
    </div>
    <div class="fixturesTextInput">
        <label for="recipient">JOB NUMBER</label><asp:TextBox ID="jobNumber" runat="server"
            Height="20px" OnTextChanged="jobNumberText_TextChanged"></asp:TextBox>
        &nbsp;<div class="fixturesFormItem">
            &nbsp;</div>
        <a href="fixturesSeeAll">See All Jobs</a> &nbsp;</div>
    <asp:Button ID="executeButton" runat="server" OnClick="executeButton_Click" Text="Execute Query"
        Width="112px" />
    <div class="clear">

        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>

    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="tags"
         MinimumPrefixLength="1" EnableCaching="true" CompletionSetCount="1" CompletionInterval="1000"
         ServiceMethod="GetCompletionList" />
    </div>
    </form>
</asp:Content>
