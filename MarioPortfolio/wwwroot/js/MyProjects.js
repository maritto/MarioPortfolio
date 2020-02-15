

$(".project").children(".clickable").on("click", async function () {
    if ($(this).parent().children(".card-body").hasClass("hide-by-height")) {
        $(".project").each(function () {
            $(this).children(".card-body").addClass("hide-by-height");
            $(".fas").removeClass("fa-angle-double-down").addClass("fa-angle-double-up");
        });
        $(this).parent().children(".card-body").removeClass("hide-by-height");
        $(this).parent().removeClass("project-shrink");
    } else {
        $(this).parent().children(".card-body").addClass("hide-by-height");
        $(this).parent().addClass("project-shrink");
        $(".fas").removeClass("fa-angle-double-up").addClass("fa-angle-double-down");
    }
    await new Promise(res => setTimeout(res, 300));
    $('html,body').animate({
        scrollTop: $(this).offset().top
    }, 'slow');
})


$(".scroll-nav").on("click", function () {
    $('html,body').animate({
        scrollTop: $("." + $(this).attr("id")).offset().top
    }, 'slow');
});

$(document).on('scroll', function () {
    var dist = $(window).scrollTop() - $(window).height();
    if (dist > 0) {
        $(".go-back-up-btn").addClass("show");
    } else {
        $(".go-back-up-btn").removeClass("show");
    }
});

$(".go-back-up-btn").on("click", function () {
    $("html").animate({
        scrollTop: "0"
    }, "slow");
});