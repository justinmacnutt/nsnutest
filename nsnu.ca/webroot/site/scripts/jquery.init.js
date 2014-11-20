$j(document).ready(function () {
    /*var ec_date;
    $j('.ec_lure_eventDate, .ec_details_date time, .ec_list_eventDate').each(function () {
    ec_date = $j(this).html().split('-');
    $j(this).html(ec_date[1]+' '+ec_date[0]+', '+ec_date[2]);
    });*/
    $j('.inside').addClass($j('#backgroundClass').attr('class'));

    $j('.btn_menu').click(function (e) {
        e.preventDefault();
        $j(this).toggleClass('active');
        $j('#ow_mainNav').toggleClass('active');
    });

    $j('.home #content .login h1.loginToggle').click(function (e) {
        e.preventDefault();
        $j('.home #content .login').toggleClass('active');
        $j('.home #content .col1').toggleClass('login-active');
    });

    $j('#ow_mainNav ul li').each(function () {
        if ($j('ul li', $j(this)).length) {
            if ($j('a', $j(this)).hasClass('current')) {
                $j(this).addClass('nav_open');
            }
            $j('> a', $j(this)).append('<span href="#" class="btn_open">Open</span>');
            $j('.btn_open', $j(this)).click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                $j(this).parent().parent().toggleClass('nav_open');
            });
        }
    });

    if (typeof OneWeb.Admin === "undefined") {
        $j('body').addClass('public');
        //slider
	if ($j('#header_main .slider').length) {
	        $j('#header_main .slider .slide').hide();
        	$j('#header_main .slider .slide_nav li').each(function () {
	            $j(this).click(function (e) {
        	        advanceSlider($j(this));
	            });
	        });
	        advanceSlider($j('#header_main .slider .slide_nav li:first-child'));
        	var el;
	        $j('#header_main').swipe({
        	    swipeRight: function (e, dir, dist, dur, fingerCount) {
	                el = $j('#header_main .slider .slide_nav li.active');
        	        if (el.index() - 1 >= 0) {
                	    advanceSlider($j($j('#header_main .slider .slide_nav li').get(el.index() - 1)));
	                }
        	    },
	            swipeLeft: function (e, dir, dist, dur, fingerCount) {
        	        el = $j('#header_main .slider .slide_nav li.active')
                	if (el.index() + 1 < $j('#header_main .slider .slide_nav li').length) {
	                    advanceSlider($j($j('#header_main .slider .slide_nav li').get(el.index() + 1)))
        	        }
	            }
        	});
	}
    } else {
        $j('body').addClass('private');
    };

});

function advanceSlider(item) {
    var id = item.attr('id').replace('nav_', '');
    $j('#header_main .slider .slide').fadeOut();
    $j('#header_main .slider .slide.' + id).fadeIn();
    $j('#header_main .slider .slide_nav li').removeClass('active');
    item.addClass('active');
}

/*$j(window).load(function () {
    var output = '<pre>';
    $j('*').each(function () {
        if ($j(this).width() > $j(window).width()) {
            output += this.nodeName + '\r\n';
            output += '  id: ' + $j(this).attr('id') + '\r\n';
            output += '  class: ' + $j(this).attr('class') + '\r\n';
            output += '  style: ' + $j(this).attr('style') + '\r\n';
            output += '  width: ' + $j(this).width() + '\r\n';
        };
    });
    output += '</pre>';
    $j('body').html(output);
});*/




