 
function BindMenuItems() {

    // region

    $('#menu ul').hide();
    LoadSelectedMenuItemOrNode();

    $('#menu li a').click(
    function () {
        var href = $(this).attr('href'); /*parent navigation support*/
        //if (href == window.location.pathname) return false;

        //if a MenuItem with a hyperlink is clicked, re-direct to that location
        if (IsRedirectionUrl(href))/*parent navigation support*/
            window.location = href; /*parent navigation support*/

        var clickedMenuItem = $(this).next();
        //if the clicked MenuItem nodes are already visible, no need to take any action
        if ((clickedMenuItem.is('ul')) && (clickedMenuItem.is(':visible'))) {
            return false;
        }

        //if the clicked MenuItem nodes are not visible, then hide the previous MenuItem nodes and make the clicked MenuItem nodes visible
        if ((clickedMenuItem.is('ul')) && (!clickedMenuItem.is(':visible'))) {
            $('#menu ul:visible').slideUp('normal');

            if (!IsRedirectionUrl(href))/*parent navigation support*/
                clickedMenuItem.slideDown('normal'); /*parent navigation support*/

            return false;
        }
    }
    );

    $('#menu li a').mouseenter(
    function () {
        var href = $(this).attr('href'); /*parent navigation support*/
        //if (href == window.location.pathname) return false;

        //if a MenuItem with a hyperlink is clicked, re-direct to that location
        if (IsRedirectionUrl(href))/*parent navigation support*/
            return false; /*parent navigation support*/

        var clickedMenuItem = $(this).next();
        //if the clicked MenuItem nodes are already visible, no need to take any action
        if ((clickedMenuItem.is('ul')) && (clickedMenuItem.is(':visible'))) {
            return false;
        }

        //if the clicked MenuItem nodes are not visible, then hide the previous MenuItem nodes and make the clicked MenuItem nodes visible
        if ((clickedMenuItem.is('ul')) && (!clickedMenuItem.is(':visible'))) {
            //$('#menu ul:visible').slideUp('normal');

            if (!IsRedirectionUrl(href))/*parent navigation support*/
                clickedMenuItem.slideDown('normal'); /*parent navigation support*/

            return false;
        }
    }
    );

    $('#menu').mouseleave(
    function () {

        var path = location.pathname;
        var currentVisibleMenuItem = $('#menu ul:visible');
        var item = $(currentVisibleMenuItem).find("a[href='" + path + "']")[0];

        //if sub-menu item is selected currently & in visible area
        if (item) {
            return false;
        }

            //else if menu item is selected currently & in visible area
        else if (currentVisibleMenuItem.parents('li').find("a[href='" + path + "']")[0]) {/*parent navigation support*/
            return false; /*parent navigation support*/
        } /*parent navigation support*/

            //otherwise we've made a menu visible that is not selected (i.e. in current page location) 
        else {
            LoadSelectedMenuItemOrNode();
        }
    }
    );

    // endregion 

    // region

    $('#menu2 ul').hide();
    LoadSelectedMenuItemOrNode();

    $('#menu2 li a').click(
    function () {
        var href = $(this).attr('href'); /*parent navigation support*/
        //if (href == window.location.pathname) return false;

        //if a MenuItem with a hyperlink is clicked, re-direct to that location
        if (IsRedirectionUrl(href))/*parent navigation support*/
            window.location = href; /*parent navigation support*/

        var clickedMenuItem = $(this).next();
        //if the clicked MenuItem nodes are already visible, no need to take any action
        if ((clickedMenuItem.is('ul')) && (clickedMenuItem.is(':visible'))) {
            return false;
        }

        //if the clicked MenuItem nodes are not visible, then hide the previous MenuItem nodes and make the clicked MenuItem nodes visible
        if ((clickedMenuItem.is('ul')) && (!clickedMenuItem.is(':visible'))) {
            $('#menu2 ul:visible').slideUp('normal');

            if (!IsRedirectionUrl(href))/*parent navigation support*/
                clickedMenuItem.slideDown('normal'); /*parent navigation support*/

            return false;
        }
    }
    );

    $('#menu2 li a').mouseenter(
    function () {
        var href = $(this).attr('href'); /*parent navigation support*/
        //if (href == window.location.pathname) return false;

        //if a MenuItem with a hyperlink is clicked, re-direct to that location
        if (IsRedirectionUrl(href))/*parent navigation support*/
            return false; /*parent navigation support*/

        var clickedMenuItem = $(this).next();
        //if the clicked MenuItem nodes are already visible, no need to take any action
        if ((clickedMenuItem.is('ul')) && (clickedMenuItem.is(':visible'))) {
            return false;
        }

        //if the clicked MenuItem nodes are not visible, then hide the previous MenuItem nodes and make the clicked MenuItem nodes visible
        if ((clickedMenuItem.is('ul')) && (!clickedMenuItem.is(':visible'))) {
            //$('#menu ul:visible').slideUp('normal');

            if (!IsRedirectionUrl(href))/*parent navigation support*/
                clickedMenuItem.slideDown('normal'); /*parent navigation support*/

            return false;
        }
    }
    );

    $('#menu2').mouseleave(
    function () {

        var path = location.pathname;
        var currentVisibleMenuItem = $('#menu2 ul:visible');
        var item = $(currentVisibleMenuItem).find("a[href='" + path + "']")[0];

        //if sub-menu2 item is selected currently & in visible area
        if (item) {
            return false;
        }

            //else if menu2 item is selected currently & in visible area
        else if (currentVisibleMenuItem.parents('li').find("a[href='" + path + "']")[0]) {/*parent navigation support*/
            return false; /*parent navigation support*/
        } /*parent navigation support*/

            //otherwise we've made a menu2 visible that is not selected (i.e. in current page location) 
        else {
            LoadSelectedMenuItemOrNode();
        }
    }
    );

    // endregion 

    // region

    $('#menu3 ul').hide();
    LoadSelectedMenuItemOrNode();

    $('#menu3 li a').click(
    function () {
        var href = $(this).attr('href'); /*parent navigation support*/
        //if (href == window.location.pathname) return false;

        //if a MenuItem with a hyperlink is clicked, re-direct to that location
        if (IsRedirectionUrl(href))/*parent navigation support*/
            window.location = href; /*parent navigation support*/

        var clickedMenuItem = $(this).next();
        //if the clicked MenuItem nodes are already visible, no need to take any action
        if ((clickedMenuItem.is('ul')) && (clickedMenuItem.is(':visible'))) {
            return false;
        }

        //if the clicked MenuItem nodes are not visible, then hide the previous MenuItem nodes and make the clicked MenuItem nodes visible
        if ((clickedMenuItem.is('ul')) && (!clickedMenuItem.is(':visible'))) {
            $('#menu3 ul:visible').slideUp('normal');

            if (!IsRedirectionUrl(href))/*parent navigation support*/
                clickedMenuItem.slideDown('normal'); /*parent navigation support*/

            return false;
        }
    }
    );

    $('#menu3 li a').mouseenter(
    function () {
        var href = $(this).attr('href'); /*parent navigation support*/
        //if (href == window.location.pathname) return false;

        //if a MenuItem with a hyperlink is clicked, re-direct to that location
        if (IsRedirectionUrl(href))/*parent navigation support*/
            return false; /*parent navigation support*/

        var clickedMenuItem = $(this).next();
        //if the clicked MenuItem nodes are already visible, no need to take any action
        if ((clickedMenuItem.is('ul')) && (clickedMenuItem.is(':visible'))) {
            return false;
        }

        //if the clicked MenuItem nodes are not visible, then hide the previous MenuItem nodes and make the clicked MenuItem nodes visible
        if ((clickedMenuItem.is('ul')) && (!clickedMenuItem.is(':visible'))) {
            //$('#menu ul:visible').slideUp('normal');

            if (!IsRedirectionUrl(href))/*parent navigation support*/
                clickedMenuItem.slideDown('normal'); /*parent navigation support*/

            return false;
        }
    }
    );

    $('#menu3').mouseleave(
    function () {

        var path = location.pathname;
        var currentVisibleMenuItem = $('#menu3 ul:visible');
        var item = $(currentVisibleMenuItem).find("a[href='" + path + "']")[0];

        //if sub-menu3 item is selected currently & in visible area
        if (item) {
            return false;
        }

            //else if menu3 item is selected currently & in visible area
        else if (currentVisibleMenuItem.parents('li').find("a[href='" + path + "']")[0]) {/*parent navigation support*/
            return false; /*parent navigation support*/
        } /*parent navigation support*/

            //otherwise we've made a menu3 visible that is not selected (i.e. in current page location) 
        else {
            LoadSelectedMenuItemOrNode();
        }
    }
    );

    // endregion 
    // region

    $('#menu4 ul').hide();
    LoadSelectedMenuItemOrNode();

    $('#menu4 li a').click(
    function () {
        var href = $(this).attr('href'); /*parent navigation support*/
        //if (href == window.location.pathname) return false;

        //if a MenuItem with a hyperlink is clicked, re-direct to that location
        if (IsRedirectionUrl(href))/*parent navigation support*/
            window.location = href; /*parent navigation support*/

        var clickedMenuItem = $(this).next();
        //if the clicked MenuItem nodes are already visible, no need to take any action
        if ((clickedMenuItem.is('ul')) && (clickedMenuItem.is(':visible'))) {
            return false;
        }

        //if the clicked MenuItem nodes are not visible, then hide the previous MenuItem nodes and make the clicked MenuItem nodes visible
        if ((clickedMenuItem.is('ul')) && (!clickedMenuItem.is(':visible'))) {
            $('#menu4 ul:visible').slideUp('normal');

            if (!IsRedirectionUrl(href))/*parent navigation support*/
                clickedMenuItem.slideDown('normal'); /*parent navigation support*/

            return false;
        }
    }
    );

    $('#menu4 li a').mouseenter(
    function () {
        var href = $(this).attr('href'); /*parent navigation support*/
        //if (href == window.location.pathname) return false;

        //if a MenuItem with a hyperlink is clicked, re-direct to that location
        if (IsRedirectionUrl(href))/*parent navigation support*/
            return false; /*parent navigation support*/

        var clickedMenuItem = $(this).next();
        //if the clicked MenuItem nodes are already visible, no need to take any action
        if ((clickedMenuItem.is('ul')) && (clickedMenuItem.is(':visible'))) {
            return false;
        }

        //if the clicked MenuItem nodes are not visible, then hide the previous MenuItem nodes and make the clicked MenuItem nodes visible
        if ((clickedMenuItem.is('ul')) && (!clickedMenuItem.is(':visible'))) {
            //$('#menu ul:visible').slideUp('normal');

            if (!IsRedirectionUrl(href))/*parent navigation support*/
                clickedMenuItem.slideDown('normal'); /*parent navigation support*/

            return false;
        }
    }
    );

    $('#menu4').mouseleave(
    function () {

        var path = location.pathname;
        var currentVisibleMenuItem = $('#menu4 ul:visible');
        var item = $(currentVisibleMenuItem).find("a[href='" + path + "']")[0];

        //if sub-menu4 item is selected currently & in visible area
        if (item) {
            return false;
        }

            //else if menu4 item is selected currently & in visible area
        else if (currentVisibleMenuItem.parents('li').find("a[href='" + path + "']")[0]) {/*parent navigation support*/
            return false; /*parent navigation support*/
        } /*parent navigation support*/

            //otherwise we've made a menu4 visible that is not selected (i.e. in current page location) 
        else {
            LoadSelectedMenuItemOrNode();
        }
    }
    );

    // endregion 
}

function LoadSelectedMenuItemOrNode() {

    //get the current location
    var path = location.pathname;

    // region

    //get the target sub-menu
    var item = $("#menu").find("a[href='" + path + "']");
    //hide existing visible menu item
    $('#menu ul:visible').slideUp('normal');
    //expand target menu
    var menuItem;
    menuItem = $(item).parents('li').children('ul');/*parent navigation support*/
    if (!menuItem)/*parent navigation support*/
        menuItem = $(item).parents('ul');
    menuItem.slideDown('normal');
    //apply 'selected' style to target sub-menu
    item.addClass('SelectedNode');

    // endregion 

    // region

    var item2 = $("#menu2").find("a[href='" + path + "']");
    //hide existing visible menu item
    $('#menu2 ul:visible').slideUp('normal');
    //expand target menu
    var menuItem2;
    menuItem2 = $(item2).parents('li').children('ul');/*parent navigation support*/
    if (!menuItem2)/*parent navigation support*/
        menuItem2 = $(item2).parents('ul');
    menuItem2.slideDown('normal');
    //apply 'selected' style to target sub-menu
    item2.addClass('SelectedNode');

    // endregion 

    // region

    var item3 = $("#menu3").find("a[href='" + path + "']");
    //hide existing visible menu item
    $('#menu3 ul:visible').slideUp('normal');
    //expand target menu
    var menuItem3;
    menuItem3 = $(item3).parents('li').children('ul');/*parent navigation support*/
    if (!menuItem3)/*parent navigation support*/
        menuItem3 = $(item3).parents('ul');
    menuItem3.slideDown('normal');
    //apply 'selected' style to target sub-menu
    item3.addClass('SelectedNode');

    // endregion 

};

function IsRedirectionUrl(url) {
    if (!url)
        return true;


    if ((url == '#') || (url.endsWith('?')))
        return false;

    return true;
}

String.prototype.endsWith = function (pattern) {
    var d = this.length - pattern.length;
    return d >= 0 && this.lastIndexOf(pattern) === d;
};

$(document).ready(function () { BindMenuItems(); });