<!DOCTYPE HTML>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<title>Fixtures Library | Mission Bell</title>
<!-- jQuery //-->
<link rel="stylesheet" href="http://missionbell.com/js/slider/css/basic-jquery-slider.css" />
<script src="http://missionbell.com/js/slider/js/libs/jquery-1.6.2.min.js"></script>
<!-- HoverIntent and Basic jQuery Slider //-->
<script src="http://missionbell.com/js/jquery.hoverIntent.minified.js"></script>
<script src="http://missionbell.com/js/slider/js/basic-jquery-slider.js"></script>
<!-- Shadowbox with jQuery //-->
<link rel="stylesheet" type="text/css" href="http://missionbell.com/js/shadowbox/shadowbox.css">
<script type="text/javascript" src="http://missionbell.com/js/shadowbox/shadowbox.js"></script>
<!-- jQuery Validation plugin //-->
<script type="text/javascript" src="http://missionbell.com/js/validation/dist/jquery.validate.js"></script>
<!-- MB CSS and JS //-->
<!--<link rel="stylesheet" type="text/css" href="http://missionbell.com/css/style.css" />-->
<link rel="stylesheet" type="text/css" href="~/Styles/fixlib.css" />
<link rel="icon" type="image/png" href="http://missionbell.com/images/favicon.png">
<script type="text/javascript" src="http://missionbell.com/js/mb.js"></script>
<script type="text/javascript" src="fixlib.js"></script>
<!--[if IE]>
	<link rel="stylesheet" type="text/css" href="http://missionbell.com/css/ie.css" />
	<script src="http://html5shiv.googlecode.com/svn/trunk/html5.js"></script>
<![endif]-->
</head>
<body>

<header>
	<div id="branding">
		<hgroup id="logo"><h1 id="missionBell"><a href="http://missionbell.com/index.php"><span>Mission Bell Manufacturing</span></a></h1></hgroup>
		<hgroup id="tagline">
			<h2><a href="http://missionbell.com/index.php"><span>Working Wonders in Workspace</span><br />
			Architectual Millwork,<br />
			Woodwork and<br />
			Casework</a></h2>
		</hgroup>
	
		<div id="fixturesTitle">
			<h1>Fixtures <span>Library</span></h1>
		</div>
	</div>
	
</header>

<section>

	<!-- The logic for pulling the project's photos and drawings loads here from a SQL query -->
	<section class="flFeature">
		<!-- Thumbnail-drive image carousel -->
		<p>Click on an image to enlarge it</p>
		<ul>
			<li><a href="http://missionbell.com/projects/c3-energy/images/c3-energy7-940.jpg" rel="shadowbox[c3-energy]"><img src="http://missionbell.com/projects/c3-energy/images/c3-energy7-940x450.jpg" alt="C3 Energy" width="940" height="450" /></a></li>
			<li><a href="http://missionbell.com/projects/c3-energy/images/c3-energy3-940.jpg" rel="shadowbox[c3-energy]"><img src="http://missionbell.com/projects/c3-energy/images/c3-energy3-940x450.jpg" alt="C3 Energy" width="940" height="450" /></a></li>
			<li><a href="http://missionbell.com/projects/c3-energy/images/c3-energy2-940.jpg" rel="shadowbox[c3-energy]"><img src="http://missionbell.com/projects/c3-energy/images/c3-energy2-940x450.jpg" alt="C3 Energy" width="940" height="450" /></a></li>
			<li><a href="http://missionbell.com/projects/c3-energy/images/c3-energy5-940.jpg" rel="shadowbox[c3-energy]"><img src="http://missionbell.com/projects/c3-energy/images/c3-energy5-940x450.jpg" alt="C3 Energy" width="940" height="450" /></a></li>
			<li><a href="http://missionbell.com/projects/c3-energy/images/c3-energy4-940.jpg" rel="shadowbox[c3-energy]"><img src="http://missionbell.com/projects/c3-energy/images/c3-energy4-940x450.jpg" alt="C3 Energy" width="940" height="450" /></a></li>
		</ul>
	</section>

	<section id="flDetailsSection">
		<h2 id="flTitle"><!-- Job Title -->C3 Energy</h2>
		<aside class="flDetails" id="flDetailsLabor">
			<!-- This is the most vital portion of stats. It will pull the data initially from the MBdb on creation and fetch the most up-to-date data on query, but will also allow for manual edits. A version history and notification system will have to be put in place. -->
			<h3>Labor Details</h3>
			<ul>
				<!-- Allow breakdown of departments -->
				<li>Projected Hours: <span><!-- Dynamic -->700</span></li>
				<li>Actual Hours: <span><!-- Dynamic -->550</span></li>
		</aside>
		<aside class="flDetails" id="flDetailsProject">
			<h3>Project Details</h3>
			<ul>
				<li>Job Number: <span><strong><!-- Auto pull -->J12061</strong></span></li>
				<li>Job Name: <span><!-- Auto pull -->C3 TI 5th Flr</span></li>
				<li>Task Number: <span><strong><!-- Manual -->10, 23</strong></span></li>
				<li>Job Location: <span><!-- Auto pull -->Redwood City</span></li>
				<li>General Contractor: <span><!-- Auto pull -->Vance Brown</span></li>
				<li>Architect: <span><!-- Auto pull -->Studio O+A</span></li>
				<li>Job Close Date: <span><!-- Auto pull -->Dec. 3, 2012</span></li>
			</ul>
		</aside>
		<aside class="flDetails" id="flDetailsOrg">
			<h3>Organization</h3>
			<ul>
				<li>Category: <span><!-- Manual -->Decorative</span></li>
				<li>Subcategory: <span><!-- Manual -->Planter</span></li>
				<li>Tags: <!-- Tags loaded in from the db for quick viewing and editing. User MUST enter at least 3 tags to create a post. -->
					<a href="#" class="post-tag">planter</a>
					<a href="#" class="post-tag">shingles</a>
					<a href="#" class="post-tag">wood</a>
					<a href="#" class="post-tag">decorative</a>
					<a href="#" class="post-tag">green</a>
				</li>
			</ul>
		</aside>
		<aside class="flDetails" id="flDetailsComments">
			<h3>Comments</h2>
			<p>Comments about the fixtures item go here, as well as any comments about the job.</p>
		</aside>

	</section>
	
</section>
</body>