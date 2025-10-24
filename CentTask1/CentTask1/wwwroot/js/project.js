$(function () {
    
    // PROJECT LIST
    $(document).on("click", "#getProjects", function (e) {
        e.preventDefault();
        loadProjects();
    });

    //project load function(list)
    function loadProjects() {
    
        // fetch partial
        $.get("/Projects/LoadProjects")
            .done(function (htmlContent) {
                $("#HomeContainer").hide();
                $("#taskContainer").hide();

                $("#ProjectContainer").html(htmlContent).show();

                // now the overlay element (if in partial) exists — show it
                //toggleLoader(true);

                fetchProjectData();
            })
            .fail(function (err) {
                console.error("Error loading projects partial:", err);
                $("#ProjectContainer").html('<div class="text-center p-5 text-danger"><h4>Error loading projects</h4></div>');
                toggleLoader(false);
            });
    }
    //project fetch function(list)
    function fetchProjectData() {
        debugger;
        toggleLoader(true);

        //// Watchdog to avoid stuck overlay
        //var overlayWatch = setTimeout(function () {
        //    console.warn('[fetchProjectData] project overlay watchdog fired — hiding overlay.');
        //    hideProjectOverlay();
        //}, 8000);

        $.ajax({
            type: "GET",
            url: "/Projects/GetAllProjects",
            dataType: "json",
            success: function (data) {
                var tableBody = $("#myProjectTable tbody");
                tableBody.empty(); // Clear existing rows
                data.forEach(function (project) {
                    var row = `
                    <tr>
                    <td>${project.projectName}</td>
                    <td>${project.description ?? ''}</td>
                    <td>${project.startDate ? project.startDate.substring(0, 10) : ''}</td>
                    <td>${project.endDate ? project.endDate.substring(0, 10) : ''}</td>
                    <td>${project.budget ?? ''}</td>
                    <td>${project.clientName ?? ''}</td>
                    <td>${project.manager ?? ''}</td>
                    <td>
                        <span class="badge ${project.status ? 'bg-success' : 'bg-danger'}">
                        ${project.status ? 'Active' : 'Inactive'}
                        </span>
                    </td>
                    <td>
                        <a href="#" class="fa fa-pencil openProjectModal text-decoration-none me-1"  data-id="${project.id}"></a>
                        <a href="#" class="fa fa-eye  text-decoration-none openProjectDetailModal  text-dark me-1" data-id="${project.id}"></a>
                        <a href="#" class="fa fa-trash text-danger deleteProject" data-id="${project.id}"></a>
                    </td>
                    </tr>
                    `
                    tableBody.append(row);
                });

                if ($.fn.DataTable.isDataTable('#myProjectTable')) {
                    try { $('#myProjectTable').DataTable().destroy(); } catch (ex) { console.warn('Error destroying project DataTable', ex); }
                }

                toggleLoader(false);
                $('#myProjectTable').DataTable({
                    paging: true,
                    searching: true,
                    ordering: true,
                    info: true,
                });

                // short fallback hide if needed
               
            },
            error: function (err) {
                alert("Failed to load project content.");
                console.error("Error loading projects:", err);
                clearTimeout(overlayWatch);
                toggleLoader(false);
            }
        });
    }

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
        $("#projectModalTitle").text(title);
        $('#createOrEditProject').modal('show');

    });
    

    //CreateOrEdit
    $(document).on('click', '#submitProjectId', function (e) {
        debugger;
        e.preventDefault();
       // var id = $(this).data("id");

        var url = $(this).data("url"); // e.g. "/Tasks/Create"
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
                    Swal.fire("Success!", "Task created successfully", "success");
                    $('#createOrEditProject').modal('hide');

                    // Optionally clear form
                    form[0].reset();
                loadProjects();
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
                            loadProjects();
                        }
                        else {
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

    $(document).on('click', '.openProjectDetailModal', function (e) {
        debugger;
        e.preventDefault();
        var id = $(this).data("id");
        $.ajax({
            type: "GET",
            url: "Projects/Details" + (id ? "?id=" + id : ""),

            error: function (err) {
                Swal.fire({
                    title: 'Error',
                    text: 'Internal Server Error',
                    icon: 'error',
                });
                console.log("error loading view", err);
            }
        });
        $("#projectModalTitle").text("Project Detail");
        $('#createOrEditProject').modal('show'); 
    });

});