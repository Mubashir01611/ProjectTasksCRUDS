$(function () {
      // PROJECT LIST button
    $(document).on("click", "#getProjects", function (e) {
        e.preventDefault();
        loadProjects();
    });

    // load partial then fetch data
    function loadProjects() {
        $('.modal-backdrop').remove();
        $('body').removeClass('modal-open').css('padding-right', '');

        $.get("/Projects/LoadProjects")
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

    function fetchProjectData() {
        toggleLoader(true);
        debugger;
        // Destroy any previous DataTable instance
        if ($.fn.DataTable.isDataTable('#myProjectTable')) {
            try { $('#myProjectTable').DataTable().clear().destroy(); } catch (ex) { console.warn('DataTable destroy failed', ex); }
        }

        // Initialize DataTable with server-side processing
        $('#myProjectTable').DataTable({
            serverSide: true,
            processing: true,
            ajax: {
                url: '/Projects/GetProjectsDataTable',
                type: 'POST',
                dataType: 'json',
                data: function (d) {
                    // d is the DataTables request payload
                    // You can add custom parameters here if needed
                    return d;
                },
                beforeSend: function () { toggleLoader(true); },
                complete: function () { toggleLoader(false); }
            },
            columns: [
                { data: 'projectName' },
                { data: 'description' },
                { data: 'startDate' },
                { data: 'endDate' },
                { data: 'budget' },
                { data: 'clientName' },
                { data: 'manager' },
                { 
                    data: 'status',
                    render: function (data) {
                        return `<span class="badge ${data ? 'bg-success' : 'bg-danger'}">${data ? 'Active' : 'Inactive'}</span>`;
                    }
                },
                {
                    data: 'id',
                    orderable: false,
                    searchable: false,
                    render: function (id, type, row) {
                        return `
                            <a href="#" class="fa fa-pencil openProjectModal text-decoration-none me-1" data-id="${id}"></a>
                            <a href="#" class="fa fa-eye openProjectDetailModal text-decoration-none text-dark me-1" data-id="${id}"></a>
                            <a href="#" class="fa fa-trash text-danger deleteProject" data-id="${id}"></a>
                        `;
                    }
                }
            ],
            order: [[0, 'asc']]
        });
    }

    // open project modal (unchanged)
    $(document).on('click', ".openProjectModal", function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        $.ajax({
            type: "GET",
            url: "Projects/CreateProject" + (id ? "?id=" + id : ""),
            success: function (htmlContent) {
                $("#projectModalBodyContent").html(htmlContent);
            },
            error: function (err) {
                Swal.fire({ title: 'Error!', text: 'Internal Server Error', icon: 'error' });
                console.error("Error loading view:", err);
            }
        });
        var title = id ? "Update Project" : "Create Project";
        $("#projectModalTitle").text(title);
        $('#createOrEditProject').modal('show');
    });
    //CreateOrEdit
    $(document).on('click', '#submitProjectId', function (e) {
       
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


        $(document).on('click', '.openProjectDetailModal', function (e) {
            e.preventDefault();
            var id = $(this).data("id");
            $.ajax({
                type: "GET",
                url: "Projects/Details" + (id ? "?id=" + id : ""),
                success: function (htmlContent)
                {
                    $("#projectModalBodyContent").html(htmlContent);
                },
                error: function (err)
                {
                    Swal.fire({ title: 'Error', text: 'Internal Server Error', icon: 'error', });
                    console.log("error loading view", err);
                }
            }); $("#modalTitle").text("Task Detail"); $('#createOrEditProject').modal('show'); 
        });
         
    });


    
    $(document).on('click', '.openProjectDetailModal', function (e) {
      
        e.preventDefault();
        var id = $(this).data("id");
        $.ajax({
            type: "GET",
            url: "Projects/Details" + (id ? "?id=" + id : ""),
            success: function (htmlContent) {
                $("#projectModalBodyContent").html(htmlContent);
            },
            error: function (err) {
                Swal.fire({
                    title: 'Error',
                    text: 'Internal Server Error',
                    icon: 'error',
                });
                console.log("error loading view", err);
            }
        });
        $("#modalTitle").text("Task Detail");
        $('#createOrEditProject').modal('show');
    });
    
    $(document).on('click', '.deleteProject', function (e) {
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

})