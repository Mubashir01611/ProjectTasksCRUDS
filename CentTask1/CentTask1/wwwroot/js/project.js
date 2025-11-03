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
        
        // Destroy any previous DataTable instance
        if ($.fn.DataTable.isDataTable('#myProjectTable')) {
            try { $('#myProjectTable').DataTable().clear().destroy(); } catch (ex) { console.warn('DataTable destroy failed', ex); }
        }
        // Initialize DataTable with server-side processing
        $('#myProjectTable').DataTable({
            autoWidth: false,
            serverSide: true,
            processing: true,
            dom: '<"top"<"top-left"f><"top-right"l>>rt<"bottom"<"bottom-left"i><"bottom-right"p>>',
            pageLength: 5,
            lengthMenu: [5, 10, 25, 50, 75,100],
 // Add this line to enable buttons

            ///commented this code for styling purposes
//            buttons: [
//                {
//                    extend: 'print',
//                    text: 'Print All',
//                    action: function (e, dt, button, config) {
//                        $.ajax({
//                            url: '/Projects/GetProjectsDataTable',
//                            type: 'POST',
//                            data: {
//                                draw: 1,
//                                start: 0,
//                                length: -1 // ✅ -1 means "fetch all" (custom logic needed in backend)
//                            },
//                            success: function (response) {
//                                // Create a temporary table for printing
//                                let tableHtml = '<table class="table table-bordered"><thead><tr>' +
//                                    '<th>Project Name</th><th>Description</th><th>Start Date</th><th>End Date</th>' +
//                                    '<th>Budget</th><th>Client Name</th><th>Manager</th><th>Status</th></tr></thead><tbody>';

//                                response.data.forEach(row => {
//                                    tableHtml += `<tr>
//                            <td>${row.projectName}</td>
//                            <td>${row.description}</td>
//                            <td>${row.startDate}</td>
//                            <td>${row.endDate}</td>
//                            <td>${row.budget}</td>
//                            <td>${row.clientName}</td>
//                            <td>${row.manager}</td>
//                            <td>${row.status ? 'Active' : 'Inactive'}</td>
//                        </tr>`;
//                                });

//                                tableHtml += '</tbody></table>';

//                                // Open print window
//                                let win = window.open('', '', 'height=700,width=900');
//                                win.document.write('<html><head><title>All Projects</title>');
//                                win.document.write('<link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css"/>');
//                                win.document.write('</head><body>');
//                                win.document.write(tableHtml);
//                                win.document.write('</body></html>');
//                                win.document.close();
//                                win.print();
//                            }
//                        });
//                    }
//                }
//            ]
//,

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
                        let badgeClass = '';
                        let statusText = '';

                        switch (data) {
                            case 0:
                                badgeClass = 'bg-secondary';
                                statusText = 'Not Selected';
                                break;
                            case 1:
                                badgeClass = 'bg-dark';
                                statusText = 'Pending';
                                break;
                            case 2:
                                badgeClass = 'bg-info text-dark';
                                statusText = 'Started';
                                break;
                            case 3:
                                badgeClass = 'bg-warning text-dark';
                                statusText = 'In Progress';
                                break;
                            case 4:
                                badgeClass = 'bg-success';
                                statusText = 'Completed';
                                break;
                            default:
                                badgeClass = 'bg-light text-dark';
                                statusText = 'Unknown';
                                break;
                        }

                        return `<span class="badge ${badgeClass}">${statusText}</span>`;
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