String.prototype.toEnglishNumbers = function () {
    var find = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];
    var replace = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
    var replaceString = this;
    var regex;
    for (var i = 0; i < find.length; i++) {
        regex = new RegExp(find[i], "g");
        replaceString = replaceString.replace(regex, replace[i]);
    }
    return replaceString;
};

//Convert English Numbers To Persian Numbers In Runtime
$(document).ready(function () {

    ConvertNumberToPersion();
});

function ConvertNumberToPersion() {
    persian = { 0: '۰', 1: '۱', 2: '۲', 3: '۳', 4: '۴', 5: '۵', 6: '۶', 7: '۷', 8: '۸', 9: '۹' };
    function traverse(el) {
        if (el.nodeType == 3) {
            var list = el.data.match(/[0-9]/g);
            if (list != null && list.length != 0) {
                for (var i = 0; i < list.length; i++)
                    el.data = el.data.replace(list[i], persian[list[i]]);
            }
        }
        for (var i = 0; i < el.childNodes.length; i++) {
            traverse(el.childNodes[i]);
        }
    }
    traverse(document.body);
}

$('.dropdown-menu a.dropdown-toggle').on('mouseenter', function (e) {
    if ($(window).width() > 992) {
        if (!$(this).next().hasClass('show-submenu')) {
            $(this).parents('.dropdown-menu').first().find('.show-submenu').removeClass("show-submenu");
        }
        var $subMenu = $(this).next(".dropdown-menu");
        $subMenu.toggleClass('show-submenu');


        $(this).parents('li.nav-item.dropdown.show').on('hidden.bs.dropdown', function (e) {
            $('.dropdown-submenu .show').removeClass("show-submenu");
        });


        return false;
    }
});

$('.dropdown-submenu').on('mouseleave', function (e) {
    if ($(window).width > 992) {
        if (!$(this).next().hasClass('show-submenu')) {
            $(this).parents('.dropdown-menu').first().find('.show-submenu').removeClass("show-submenu");
        }
        var $subMenu = $(this).next(".dropdown-menu");
        $subMenu.toggleClass('show-submenu');


        $(this).parents('li.nav-item.dropdown.show').on('hidden.bs.dropdown', function (e) {
            $('.dropdown-submenu .show').removeClass("show-submenu");
        });


        return false;
    }
});

$('.dropdown-menu a.dropdown-toggle').on('click', function (e) {
    if ($(window).width() < 992) {
        if (!$(this).next().hasClass('show-submenu')) {
            $(this).parents('.dropdown-menu').first().find('.show-submenu').removeClass("show-submenu");
        }
        var $subMenu = $(this).next(".dropdown-menu");
        $subMenu.toggleClass('show-submenu');


        $(this).parents('li.nav-item.dropdown.show').on('hidden.bs.dropdown', function (e) {
            $('.dropdown-submenu .show').removeClass("show-submenu");
        });


        return false;
    }
});

$(".openBtn").click(function () {
    $("#searchOverlay").css("display", "block");
});

$(".closebtn").click(function () {
    $("#searchOverlay").css("display", "none");
});
