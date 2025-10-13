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
                if ($.fn.DataTable.isDataTable("#myProjectTable")) {
                    $("#myProjectTable").DataTable().destroy();
                }
                $("#myProjectTable").DataTable({
                    paging: true,
                    searching: true,
                    ordering: true,
                    info: true
                });
                            
            },
            error: function (err) {
                alert("Failed to load project content.");
                console.error("Error loading projects:", err);
            }
        });
       
    });

    //open Modal
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
    
    //CreateOrEdit
    $(document).on('click', '#submitProjectId', function (e) {
        debugger;
        e.preventDefault();
       // var id = $(this).data("id");

        //var url = $(this).data("url"); // e.g. "/Tasks/Create"
        var url = "/Projects/Create";
        var form = $('#createOrEditProjectForm'); // form's id
        var formData = form.serialize(); // convert form fields to query string

        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            success: function (response) {
                if (response.success) {
                    // Close modal
                    $('#createOrEditProjectTask').modal('hide');
                    Swal.fire("Success!", "Task created successfully", "success");
                    // Optionally clear form
                    form[0].reset();

                    // Reload task table
                  //  loadTasks();

                }
                else {
                    // If server returns validation errors, display them
                    $("#projectModalBodyContent").html(response);

                    // Rebind validation to newly injected form
                    $.validator.unobtrusive.parse("#createOrEditProjectForm");
                    Swal.fire("error!", "Invalid Form", "error");

                }
            },
            error: function (err) {
                //$('#createOrEditProjectTask').modal('hide');
                $.validator.unobtrusive.parse("#createOrEditProjectForm");
                Swal.fire("error!", "Invalid Form", "error");
                console.error("Error submitting form:", err);

            }
        });
    });

    //Delete
    $(document).on('click', '.deleteProject', function (e) {
        debugger;
        e.preventDefault();
        var id = $(this).data("id");
        Swal.fire({
            title: 'Are you sure?',
            text: "This task will be permanently deleted!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: "POST",
                    url: "/Projects/DeleteConfirmed",
                    data: { id: id },
                    success: function (response) {
                        if (response.success) {
                            Swal.fire("Deleted!", "Project has been deleted.", "success");
                            // Optionally, refresh the project list
                            // $("#getProjects").click();
                        } else {
                            Swal.fire("Error!", "Failed to delete the project.", "error");
                        }
                    },
                    error: function (err) {
                        Swal.fire("Error!", "Internal Server Error", "error");
                        console.error("Error deleting project:", err);
                    }
                });
            }
        });
    });

});