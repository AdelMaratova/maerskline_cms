(function($) {
    "use strict";

    jQuery(document).ready(function($) {

        // Header Slide
        $(".header-slider").owlCarousel({
            items: 1,
            loop: true,
            dots: true,
            nav: true,
            navText: ['<i class="fa fa-angle-double-left"></i>', '<i class="fa fa-angle-double-left"></i>'],
            autoplay: true,
            autoplayTimeout: 4000,
            animateIn: 'pulse',
            animateOut: 'fadeOut',
            smartSpeed: 250
        });

        // Header Slide items with animate.css
        var owl = $('.header-slider');
        owl.owlCarousel();
        owl.on('translate.owl.carousel', function(event) {
            $('.header-single-slider h1').removeClass('animated').hide();
            $('.header-single-slider p').removeClass('animated').hide();
            $('.header-single-slider .boxed-btn').removeClass('animated').hide();
        });

        owl.on('translated.owl.carousel', function(event) {
            $('.header-single-slider h1').addClass('animated fadeInUp').show();
            $('.header-single-slider p').addClass('animated fadeInDown').show();
            $('.header-single-slider .boxed-btn').addClass('animated fadeInDown').show();
        });

        // Testimonial Carousel
        $(".testimonial-carousel").owlCarousel({
            items: 2,
            loop: true,
            dots: true,
            nav: false,
            margin: 30,
            autoplay: true,
            autoplayTimeout: 3000,
            autoplayHoverPause: true,
            responsive: {
                0: {
                    items: 1
                },
                700: {
                    items: 1,
                },
                900: {
                    items: 2
                }
            }
        });

        // partner Carousel
        $(".partner-carousel").owlCarousel({
            loop: true,
            dots: false,
            nav: false,
            margin: 30,
            autoplay: true,
            autoplayTimeout: 3000,
            responsive: {
                0: {
                    items: 1
                },
                700: {
                    items: 2,
                },
                1000: {
                    items: 5
                }
            }
        });

        // Exclusive Carousel for 4 column
        $(".exclusive-carousel.four-column").owlCarousel({
            items: 4,
            loop: true,
            dots: true,
            nav: false,
            margin: 30,
            autoplay: true,
            autoplayTimeout: 3000,
            responsive: {
                0: {
                    items: 1
                },
                700: {
                    items: 2,
                },
                1000: {
                    items: 4
                }
            }
        });

        // Exclusive Carousel for 3 column
        $(".exclusive-carousel.three-column").owlCarousel({
            items: 3,
            loop: true,
            dots: true,
            nav: false,
            margin: 30,
            autoplay: true,
            autoplayTimeout: 3000,
            responsive: {
                0: {
                    items: 1
                },
                700: {
                    items: 2
                },
                1000: {
                    items: 3
                }
            }
        });

        // Exclusive Carousel for 2 column
        $(".exclusive-carousel.two-column").owlCarousel({
            items: 2,
            loop: true,
            dots: true,
            nav: false,
            margin: 30,
            autoplay: true,
            autoplayTimeout: 3000,
            responsive: {
                0: {
                    items: 1
                },
                700: {
                    items: 2
                },
                1000: {
                    items: 2
                }
            }
        });

        // MagnificPopup
        $('.gallery-items').each(function() {
            $(this).magnificPopup({
                delegate: 'div.single-item>a, .gallery-load>a',
                mainClass: 'mfp-zoom-in',
                type: 'image',
                tLoading: '',
                gallery: { enabled: true },
                removalDelay: 300,

            });
        });
        
        $('.project').each(function() {
            $(this).magnificPopup({
                delegate: '.portfolio-post>a.popup',
                mainClass: 'mfp-zoom-in',
                type: 'image',
                tLoading: '',
                gallery: { enabled: true },
                removalDelay: 300,

            });
        });

        /* --------------------------------------
            LOAD MORE GALLERY
        -------------------------------------- */

        $(".gallery-load").slice(0, 6).show();
        $("#gallery-load-pro").click(function(e) {
            e.preventDefault();
            $("#gallery-ti-port-load").addClass("ti-port-load-show");
            $("#gallery-ti-port-load").animate({
                    display: "block"
                }, 500,
                function() {
                    // Animation complete.
                    $(".gallery-load:hidden").slice(0, 3).slideDown();
                    if ($(".gallery-load:hidden").length === 0) {
                        $("#gallery-load-pro").text("No more");
                    }
                    $("#gallery-ti-port-load").removeClass("ti-port-load-show");
                }
            );

        });

        /* --------------------------------------
            LOAD MORE Features
        -------------------------------------- */

        $(".features-box").slice(0, 6).show();
        $("#feature-load-pro").click(function(e) {
            e.preventDefault();
            $("#feature-ti-port-load").addClass("feature-ti-port-load-show");
            $("#feature-ti-port-load").animate({
                    display: "block"
                }, 500,
                function() {
                    // Animation complete.
                    $(".features-box:hidden").slice(0, 3).slideDown();
                    if ($(".features-box:hidden").length === 0) {
                        $("#feature-load-pro").text("No more");
                    }
                    $("#feature-ti-port-load").removeClass("feature-ti-port-load-show");
                }
            );
        });

        /* --------------------------------------
            Scroll UP
        -------------------------------------- */

        $(window).on('scroll', function() {
            if ($(this).scrollTop() > 100) {
                $('.scrollup').fadeIn();
            } else {
                $('.scrollup').fadeOut();
            }
        });

        $('.scrollup').on('click', function() {
            $("html, body").animate({
                scrollTop: 0
            }, 600);
            return false;
        });

        // Search
        var changeClass = function(name) {
            $('#search').removeAttr('class').addClass(name);
        }


        /* --------------------------------------
            Smooth Scroll
        -------------------------------------- */
        $(function() {

            var $window = $(window);
            var scrollTime = 0.5;
            var scrollDistance = 250;
            $window.on("mousewheel DOMMouseScroll", function(event) {
                event.preventDefault();
                var delta = event.originalEvent.wheelDelta / 120 || -event.originalEvent.detail / 3;
                var scrollTop = $window.scrollTop();
                var finalScroll = scrollTop - parseInt(delta * scrollDistance);
                TweenMax.to($window, scrollTime, {
                    scrollTo: { y: finalScroll, autoKill: true },
                    ease: Power1.easeOut,
                    autoKill: true,
                    overwrite: 5
                });
            });
        });

        // Gallery hover Effect
        $(' #gallery-items > div.single-item ').each(function() { $(this).hoverdir(); });


        /*------------------------------------
            Sticky Menu 
        --------------------------------------*/
        var windows = $(window);
        var stick = $(".header-sticky");
        windows.on('scroll', function() {
            var scroll = windows.scrollTop();
            if (scroll < 10) {
                stick.removeClass("sticky");
            } else {
                stick.addClass("sticky");
            }
        });

        /*------------------------------------
            jQuery MeanMenu 
        --------------------------------------*/
        $('.mobile-menu-active').meanmenu({
            meanScreenWidth: "991",
            meanMenuContainer: '.mobile-menu'
        });

        /* last  2 li child add class */
        $('ul.menu > li').slice(-2).addClass('last-elements');

    });


    jQuery(window).on('load', function() {

        // Sticky Nav
        $(".sticky-nav").sticky({ topSpacing: 0 });

        // // Preloader
        jQuery(".preloader").fadeOut('slow');

    });


}(jQuery));