$(function () {
    //PROJECT LIST
    $("#getProjects").on("click", function (e) {
        debugger;
        e.preventDefault();
        $.ajax({
            type: "GET",
            url: "/Projects/GetAllProjects",
            //dataType: JSON,
            success: function (htmldata) {
                $("#HomeContainer").hide(); 
                $("#taskContainer").hide(); 
                $("#ProjectContainer").html(htmldata).show();
                // Initialize DataTable after content is loaded
             
            },
            error: function (err) {
                alert("Failed to load project content.");
                console.error("Error loading projects:", err);
            }
        });
       
    });


    $(document).on('click', ".openProjectModal", function (e) {
        debugger;
        e.preventDefault();
        var id = $(this).data("id");

        $.ajax({
            type: "GET",
            url: "Projects/CreateProject" + (id ? "?id=" + id : ""),
            success: function (htmlContent) {
                $("#projectModalBodyContent").html(htmlContent);
            },
            error: function (err) {
                Swal.fire({
                    title: 'Error!',
                    text: 'Internal Server Error',
                    icon: 'error'
                });
                console.error("Error loading view:", err);
            }
        });
        // Dynamically set modal title
        var title = id ? "Update Project" : "Create Project";
        $("#modalTitle").text(title);
        $('#createOrEditProject').modal('show');

    });
    });