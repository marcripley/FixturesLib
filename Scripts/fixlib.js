jQuery(document).ready(function ($) {
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