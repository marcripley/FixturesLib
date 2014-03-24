<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<div id="fixturesFilter">
		<form name="fixturesSelect" action="index.php" method="post" id="fixturesSelect">
			<div class="fixturesFormDrop">
				<label for="recipient">CATEGORY</label>
				<select name="recipient" class="fixturesInput">
					<option value="general">Desk</option>
					<option value="sales">Paneling</option>
					<option value="pm">Window Frame</option>
					<option value="purch">Bar Counter</option>
					<option value="acct">Bench</option>
				</select>
			</div>
			<div class="fixturesFormDrop">
				<label for="recipient">SUBCATEGORY</label>
				<select name="recipient" class="fixturesInput">
					<option value="general">Reception</option>
					<option value="sales">Service Counter</option>
					<option value="pm">Bank Fixture</option>
					<option value="purch" action="single.aspx" method="get">Nurse Station</option>
					<option value="acct">Circular</option>
				</select>
			</div>

			<div class="fixturesTextInput">
				<label for="recipient">TAGS</label>
				<div class="fixturesFormItem">
					<input type="text" name="tags" value="" class="fixturesInput" />
				</div>
				<a href="fixturesSeeAll">See All Tags</a>
			</div>
			<div class="fixturesTextInput">
				<label for="recipient">JOB NUMBER</label>
				<div class="fixturesFormItem">
					<input type="text" name="tags" value="" class="fixturesInput" />
				</div>
				<a href="fixturesSeeAll">See All Jobs</a>
			</div>
			<div class="fixturesTextInput">
				<label for="recipient">DATE RANGE</label>
				<div class="fixturesFormItem">
					<input type="text" name="tags" value="" class="fixturesInput" />
				</div>
				<a href="fixturesSeeAll">See All Jobs</a>
			</div>
		</form>
		<div class="clear"></div>
	</div>
</asp:Content>
