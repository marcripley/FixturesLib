jQuery(document).ready(function ($) {
//Commented out code below for dynamically creating additional slideshows
//    var rowCount = document.getElementById('MainContent_gvPosts').rows.length;
//    for (row = 0; row < document.getElementById('MainContent_gvPosts').rows.length; row++) {
//        $('#slideshow' + row).bjqs({
    $('div.slideshow0').bjqs({
        'width': 940,
        'height': 450,
        'automatic': false,
        'showcontrols': true,
        'centercontrols': true, // center controls verically
        'nexttext': 'Next', // Text for 'next' button (can use HTML)
        'prevtext': 'Prev', // Text for 'previous' button (can use HTML)
        'showmarkers': true, // Show individual slide markers
        'centermarkers': true, // Center markers horizontally
        'responsive': true,
        'usecaptions': true
    });
});



//$(document).ready(function () {
//    //    var s = $("#sticker");
//    //    var pos = s.position();
//    //    $(window).scroll(function () {
//    //        var windowpos = $(window).scrollTop();
//    //        s.html("Distance from top:" + pos.top + "<br />Scroll position: " + windowpos);
//    //        if (windowpos >= pos.top) {
//    //            s.addClass("stick");
//    //        } else {
//    //            s.removeClass("stick");
//    //        }
//    //    });
//    //});

//    // If the user scrolls below the post details, they will fix to the top of the window
//    var flDetails = $("#flDetailsSection"), pos = flDetails.offset();

//    $(window).scroll(function () {
//        if ($(this).scrollTop() > pos.top && $(flDetails.css('position') == 'static')) {
//            $(flDetails).css('position', 'fixed');
//        } else {
//            $(flDetails).css('position', 'static');
//        }
//    })
//});