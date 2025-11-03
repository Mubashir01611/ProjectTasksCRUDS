$(function () {
    $(document).ready(function () { 
        // Toggle Sidebar
        $("#toggleSidebar").on("click", function () {
            $("#sidebar").toggleClass("collapsed");
            $(this).find("i").toggleClass("fa-angle-double-right fa-angle-double-left");
        });

        // Active Link Highlight
        $(".sidebar .nav-link").on("click", function () {
            $(".sidebar .nav-link").removeClass("active");
            $(this).addClass("active");
        });
    });
});