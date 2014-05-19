$(document).ready(function() {
	$('img.flFeaturedImage').each(function(i) {
		$(this).parent().children('.fixturesProjTitle').children('p').html($(this).attr("title"));
	}).hover(function() {
    $(this).fadeOut(20, function() {
      $(this).attr({
        src: $(this).attr('data-other-src'), 'data-other-src': $(this).attr('src') 
      }) 
      $(this).fadeIn(200);
    })
  });
  
  preload([
    'images/passportSD.jpg',
    'images/stanfordSD.jpg',
    'images/twitterSD.jpg',
    'images/c3energySD.jpg'
  ]);

// If the user scrolls below the post details, they will fix to the top of the window
var flDetails = $("#flDetailsSection"), pos = flDetails.offset();
$(window).scroll(function () {
    if ($(this).scrollTop() > pos.top && $(flDetails.css('position') == 'static')) {
        $(flDetails).css('position', 'fixed');
    } else {
        $(flDetails).css('position', 'static');
    }
})
});

function preload(arrayOfImages) {
    $(arrayOfImages).each(function () {
        $('<img />').attr('src',this).appendTo('body').css('display','none');
    });
}

//bind('mouseenter mouseleave', 

        // Show captions
		// MB ******* HEAVILY CUSTOMIZED (messed up) FOR MISSION BELL SITE
        if (settings.useCaptions) {

            $.each(slides, function (key, value) {
			
				// MB Added the anchor element as a necessity for populating the captions, and added the "Read more" link to ALL captions, using the aforementioned anchor href.
                var $slide = $(value);
				// MB next 2 lines are custom additions
				var $mbSlideChildLink = $slide.children('a');
				var mbSCLhref = $mbSlideChildLink.attr('href');
                var $slideChild = $mbSlideChildLink.children('img:first-child');
				var title = $slideChild.attr('title');

                if (title) {
                    var $caption = $('<p class="bjqs-caption">' + title + ' <a href="' + mbSCLhref + '" class="readmore">Read more...</a></p>'); // MB readmore link is custom addition
                    $caption.appendTo($slide);
                }

            });

        }