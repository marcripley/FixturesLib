<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <form id="fixturesSelect" runat="server">
    <div class="fixturesFormDrop">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>

    </div>
    <div class="clear">

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <label for="recipient">
                    &nbsp;&nbsp;&nbsp; CATEGORY&nbsp;&nbsp; </label><asp:DropDownList ID="categoryList0" 
                    runat="server" AutoPostBack="True" 
                    onselectedindexchanged="categoryList_SelectedIndexChanged" Height="20px" 
                    Width="181px">
                </asp:DropDownList>
                <div class="fixturesFormDrop">
                    <label for="recipient">
                    SUBCATEGORY</label>&nbsp;
                    <asp:DropDownList ID="subcategoryList0" runat="server" DataTextField="name" 
                        Height="20px" OnSelectedIndexChanged="subcategoryList_SelectedIndexChanged" 
                        Width="180px" AutoPostBack="True">
                    </asp:DropDownList>
                </div>
                <div class="fixturesTextInput">
                    <label for="recipient">
                    TAGS</label>
                    <div class="fixturesFormItem">
                        <asp:TextBox ID="tags0" runat="server" Height="20px"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="tags0_AutoCompleteExtender" runat="server" 
                            CompletionInterval="1000" CompletionSetCount="1" EnableCaching="true" 
                            MinimumPrefixLength="1" ServiceMethod="GetCompletionList" 
                            TargetControlID="tags0" />
                        &nbsp;</div>
                    <a href="fixturesSeeAll">See All Tags</a>
                </div>
                <div class="fixturesTextInput">
                    <label for="recipient">
                    JOB NUMBER</label><asp:TextBox ID="jobNumber0" runat="server" Height="20px" 
                        OnTextChanged="jobNumberText_TextChanged"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="fixturesSeeAll">See All Jobs</a> &nbsp;</div>
                <asp:Button ID="executeButton0" runat="server" OnClick="executeButton_Click" 
                    Text="Execute Query" Width="112px" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</asp:Content>
