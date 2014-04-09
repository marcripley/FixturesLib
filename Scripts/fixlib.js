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

//  // If the user scrolls below the post details, they will fix to the top of the window
//  var flDetails = $("#flDetailsSection"), pos = flDetails.offset();
//  $(window).scroll(function () {
//      if ($(this).scrollTop() > pos.top && $(flDetails.css('position') == 'static')) {
//          $(flDetails).css('position', 'fixed');
//      } else {
//          $(flDetails).css('position', 'static');
//      }
//  })
//});

function preload(arrayOfImages) {
    $(arrayOfImages).each(function () {
        $('<img />').attr('src',this).appendTo('body').css('display','none');
    });
}

//bind('mouseenter mouseleave', 