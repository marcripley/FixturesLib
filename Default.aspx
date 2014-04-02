<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

		<form id="fixturesSelect" runat="server">
			<div class="fixturesFormDrop">
				<label for="recipient">CATEGORY</label>
                <asp:DropDownList ID="categoryList" runat="server" Height="20px" Width="175px">
                </asp:DropDownList>

				<label for="recipient">SUBCATEGORY</label>&nbsp;
                <asp:DropDownList ID="subcategoryList" runat="server" Height="20px" 
                    onselectedindexchanged="subcategoryList_SelectedIndexChanged" 
                    Width="177px">
                </asp:DropDownList>
			</div>

			<div class="fixturesTextInput">
				<label for="recipient">TAGS</label>
				<div class="fixturesFormItem">
					<asp:TextBox ID="tagsText" runat="server" Height="20px"></asp:TextBox>
&nbsp;</div>
				<a href="fixturesSeeAll">See All Tags</a>
			</div>
			<div class="fixturesTextInput">
				J<label for="recipient">OB NUMBER</label><asp:TextBox ID="jobNumberText" 
                    runat="server" Height="20px" ontextchanged="jobNumberText_TextChanged"></asp:TextBox>
&nbsp;<div class="fixturesFormItem">
					&nbsp;</div>
				<a href="fixturesSeeAll">See All Jobs</a>
			</div>
			<div class="fixturesTextInput">
				<label for="recipient">DATE RANGE</label>
				<div class="fixturesFormItem">
					</div>
                <asp:TextBox ID="TextBox1" runat="server" Height="20px"></asp:TextBox>
&nbsp;</div>
		<asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
            Text="Execute Query" Width="112px" />
        </form>
		<div class="clear"></div>
	</div>
</asp:Content>
