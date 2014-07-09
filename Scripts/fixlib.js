jQuery(document).ready(function ($) {
    var rowCount = document.getElementById('MainContent_gvPosts').rows.length;
    for (row = 0; row < document.getElementById('MainContent_gvPosts').rows.length; row++) {
        $('#slideshow' + row).bjqs({
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
    }
});