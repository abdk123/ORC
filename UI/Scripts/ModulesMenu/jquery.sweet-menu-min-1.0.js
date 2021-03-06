jQuery(document).ready(function() {
    jQuery.fn.sweetMenu = function(c) {
        var a = jQuery.extend({
                top: 80,
                position: "fixed",
                iconSize: 0,
                iconSizeWidth: 0,
                iconSizeHeight: 0,
                padding: 10,
                verticalSpacing: 10,
                duration: 200,
                easing: "linear",
                anchorClass: "sweetMenuAnchor",
                openFinished: function() {
                },
                closeFinished: function() {
                },
                icons: []
            }, c);
        jQuery(this).children("li").children("a").addClass(a.anchorClass);
        jQuery(this).children("li").children("a").wrapInner("<span>");
        if (a.iconSize > 0) {
            a.iconSizeWidth = a.iconSize;
            a.iconSizeHeight = a.iconSize;
        }
        a.tabWidth =
            a.iconSizeWidth + a.padding * 2;
        a.height = a.iconSizeHeight + a.padding * 2;
        a.paddingString = a.padding + "px " + a.tabWidth + "px " + a.padding + "px " + a.padding + "px";
        jQuery(this).css({ padding: "0", margin: "0", left: "0px", top: a.top + "px" });
        a.position == "fixed" || a.position == "absolute" ? jQuery(this).css({ position: a.position }) : jQuery(this).css({ position: "fixed" });
        jQuery(this).children("li").css({ padding: "0", margin: "0", "list-style": "none", display: "block", position: "relative", "margin-bottom": a.verticalSpacing, height: a.height });
        a.width =
            0;
        jQuery(this).children("li").children("a").each(function() { if (jQuery(this).width() > a.width) a.width = jQuery(this).width(); });
        jQuery(this).children("li").children("a").css({ display: "block", padding: a.paddingString, "min-height": a.iconSizeHeight, "white-space": "nowrap", width: a.width });
        a.marginLeft = jQuery(this).outerWidth() - a.tabWidth;
        jQuery(this).children("li").children("a").css({ position: "absolute", left: "-" + a.marginLeft + "px" });
        a.backgroundPosition = a.width + a.padding * 2 + "px 50%";
        jQuery(this).children("li").children("a").each(function(b) {
            a.icons[b] !=
                "undefined" && jQuery(this).css({ "background-image": 'url("' + a.icons[b] + '")', "background-repeat": "no-repeat", "background-position": a.backgroundPosition });
            jQuery(this).mouseover(function() { jQuery(this).stop().animate({ left: "0px" }, a.duration, a.easing, function() { typeof a.openFinished == "function" && a.openFinished(); }); });
            jQuery(this).mouseout(function() { jQuery(this).stop().animate({ left: "-" + a.marginLeft + "px" }, a.duration, a.easing, function() { typeof a.closeFinished == "function" && a.closeFinished(); }); });
        });
        return jQuery(this);
    };
});