// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function toggleLoader(show) {
    const $loader = $("#Spinner");
    if (!$loader.length) return; // element not found, skip

    if (show) {
        $loader.show();
    } else {
        $loader.hide();
    }
}