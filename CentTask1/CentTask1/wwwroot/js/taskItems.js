$(function () {
    // PROJECT LIST button
    $(document).on("click", "#getTaskItems", function (e) {
        e.preventDefault();
        loadTaskItems();
    });
    // load partial then fetch data
    function loadTaskItems() {
        $('.modal-backdrop').remove();
        $('body').removeClass('modal-open').css('padding-right', '');

        $.get("/TaskItems/LoadTaskItems")
            .done(function (htmlContent) {
                $("#HomeContainer").hide();
                $("#taskContainer").hide();

                $("#ProjectContainer").html(htmlContent).show();
                fetchProjectData();
            })
            .fail(function (err) {
                Swal.fire({ title: 'Error!', text: 'Error loading projects', icon: 'error' });

                console.error("Error loading projects partial:", err);
                $("#ProjectContainer").html('<div class="text-center p-5 text-danger"><h4>Error loading projects</h4></div>');
                toggleLoader(false);
            });
    }

});