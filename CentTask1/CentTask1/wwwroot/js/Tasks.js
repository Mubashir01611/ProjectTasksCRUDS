$(function () {
    /*
yeh func ajax sy partial view load krta hai 
or usko index ky div mai inject karta hai
or oper waly function ko use kr ky ik complete DataTable bna deta hai
 
*/
    function loadTasks() {

         //var url = $(this).data("url");
   //     var url = "Tasks/LoadTaskTable";
        //var url = '@Url.Action("LoadTaskTable", "Tasks")';
        $.get("Tasks/LoadTaskTable", function (htmlContent) {
            $("#HomeContainer").hide(); 
            $("#ProjectContainer").hide(); 
            $("#taskContainer").html(htmlContent).show();
            fetchTaskData();
        });
    }
    //Yeh func js ky zariye ajax sy data fetch kr k table ke body m dalta hai
    function fetchTaskData() {
        $.ajax({
            type: "GET",
            url: "Tasks/GetAllTasks",
            dataType: "json",  
            success: function (data) {
                console.log("Project Tasks Data",data); // for debugging
                var tbody = $("#myTable tbody");
                tbody.empty(); // clear old rows

                data.forEach(function (task) {
                    var row = `

                <tr>
                    <td>${task.taskName}</td>
                    <td>${task.description}</td>
                    <td>${task.startDate.substring(0, 10)}</td>
                    <td>${task.endDate.substring(0, 10)}</td>
                    <td>${task.priority}</td>
                    <td>${task.equipmentType}</td>
                    <td>${task.twr}</td>
                    <td>${task.projectName}</td>
                    <td>
                        <a href="#" class="fa fa-pencil openTaskModal text-decoration-none me-1" data-url="" data-id="${task.taskId}"></a>
                        <a href="#" class="fa fa-eye  text-decoration-none openTaskDetailModal  text-dark me-1" data-id="${task.taskId}"></a>
                        <a href="#" class="fa fa-trash text-danger deleteTask" data-id="${task.taskId}"></a>
                    </td>
                </tr>
            `;
                    tbody.append(row);
                });
                setTimeout(function () {
                    if ($.fn.DataTable.isDataTable('#myTable')) {
                        $('#myTable').DataTable().destroy();
                    }//use parameters here in datatable

                    $('#myTable').DataTable({
                        paging: true,
                        searching: true,
                        ordering: true,
                        info: true
                    });
                }, 100);
            },
            error: function (err) {
                console.error("Error loading view:", err);
            }
        });
    }

    //DataTable yeh jb ham sidebar sy click krty hain to load hota hai
    $(".sidebar-link").on("click", function (e) {
        debugger;
        e.preventDefault();
        loadTasks();
    });

    //modal detail
    $(document).on('click', '.openTaskDetailModal', function (e) {
        debugger;
        e.preventDefault();
        var id = $(this).data("id");
        $.ajax({
            type: "GET",
            url: "Tasks/Details" + (id ? "?id=" + id : ""),
            success: function (htmlContent) {
                $("#modalBodyContent").html(htmlContent);
            },
            error: function (err) {
                Swal.fire({
                    title:'Error',
                    text:'Internal Server Error',
                    icon:'error',
                });
                console.log("error loading view", err);
            }
        });
        $("#modalTitle").text("Task Detail");
        $('#createOrEditProjectTask').modal('show');
        //$('#createOrEditProjectTask').modal.footer('hide');
    });

    // Initialize project select2 inside task modal with defensive load fallback
    function initTaskProjectSelect(retryCount = 0) {
        var $modal = $('#createOrEditProjectTask');
        var $select = $modal.find('#projectSelect');

        if (!$select.length) {
            console.debug('[initTaskProjectSelect] no #projectSelect in modal');
            return;
        }

        // If select2 is not available, try to load it dynamically (once)
        if (typeof $.fn.select2 !== 'function') {
            console.warn('[initTaskProjectSelect] select2 not loaded yet (attempt ' + retryCount + ')');
            if (retryCount === 0) {
                // load Select2 script dynamically from CDN, then re-run init
                $.getScript('https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js')
                    .done(function () {
                        console.debug('[initTaskProjectSelect] select2 loaded dynamically, initializing...');
                        initTaskProjectSelect(1);
                    })
                    .fail(function () {
                        console.error('[initTaskProjectSelect] failed to load select2 dynamically');
                    });
            } else if (retryCount < 5) {
                // small retry loop in case of timing issues
                setTimeout(function () { initTaskProjectSelect(retryCount + 1); }, 200);
            } else {
                console.error('[initTaskProjectSelect] select2 never loaded after retries');
            }
            return;
        }

        // destroy existing select2 instance if any (prevents duplicate init)
        if ($select.hasClass('select2-hidden-accessible')) {
            try { $select.select2('destroy'); } catch (ex) { console.warn('error destroying select2', ex); }
        }

        $select.select2({
            theme: 'bootstrap5',
            placeholder: 'Search and select a project',
            allowClear: true,
            dropdownParent: $modal,
            ajax: {
                url: '/Tasks/GetProjectsForDropdown',
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return { term: params.term };
                },
                processResults: function (data) {
                    return { results: data.results };
                }
            }
        });

        console.debug('[initTaskProjectSelect] initialized select2 on #projectSelect');
    }

    //modal open (load partial then initialize select2)
    $(document).on('click', ".openTaskModal", function (e) {
        debugger;
        e.preventDefault();
        var id = $(this).data("id");

        $.ajax({
            type: "GET",
            url: "Tasks/CreateProjectTask" + (id ? "?id=" + id : ""),
            success: function (htmlContent) {
                $("#modalBodyContent").html(htmlContent);

                // re-parse unobtrusive validation
                if ($.validator && $.validator.unobtrusive) {
                    $.validator.unobtrusive.parse('#createOrEditProjectTaskForm');
                }

                // initialize Select2 (with retry if Select2 wasn't ready yet)
                initTaskProjectSelect();
            },
            error: function (err) {
                Swal.fire({ title: 'Error!', text: 'Internal Server Error', icon: 'error' });   
                console.error("Error loading view:", err);
            }
        });
        // Dynamically set modal title
        var title = id ? "Update Task" : "Create Task";
        $("#modalTitle").text(title);
        $('#createOrEditProjectTask').modal('show');
    });


    //CreateOrEdit
    $(document).on('click', '#submitProjectTaskId', function (e) {
        debugger;

        e.preventDefault();
      //  var id = $(this).data("id");

        //var url = $(this).data("url"); // e.g. "/Tasks/Create"
        var url = "/Tasks/Create";
        var form = $('#createOrEditProjectTaskForm'); // form's id
        var formData = form.serialize(); // convert form fields to query string

        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            success: function (response) {
                if (response.success) {
                    // Close modal
                    $('#createOrEditProjectTask').modal('hide');
                    Swal.fire( "Success!", "Task created successfully", "success" );
                    // Optionally clear form
                    form[0].reset();

                    // Reload task table
                    loadTasks();

                }
                else {
                    // If server returns validation errors, display them
                    $("#modalBodyContent").html(response);

                    // Rebind validation to newly injected form
                    $.validator.unobtrusive.parse("#createOrEditProjectTaskForm");
                Swal.fire("error!", "Invalid Form", "error");

                }
            },
            error: function (err) {
                    //$('#createOrEditProjectTask').modal('hide');
                    $.validator.unobtrusive.parse("#createOrEditProjectTaskForm");
                Swal.fire("error!", "Invalid Form", "error");
                console.error("Error submitting form:", err);

            }
        });
    });


    //delete
    $(document).on('click', '.deleteTask', function (e) {
        debugger;
        e.preventDefault();
        var id = $(this).data("id");

        Swal.fire({
            title: 'Are you sure?',
            text: "This task will be deleted!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: "POST",
                    url: "/Tasks/DeleteConfirmed",
                    data: { id: id },
                    success: function (response) {
                        if (!response.success) {
                            Swal.fire(
                                'Error!',
                                response.message || 'Something went wrong while deleting.',
                                'error'
                            );
                            return;
                        }
                        Swal.fire(
                            'Deleted!',
                            'Task has been deleted.',
                            'success'
                        );
                        loadTasks();
                    },
                    error: function () {
                        Swal.fire(
                            'Error!',
                            'Something went wrong while deleting.',
                            'error'
                        );
                    }
                });
            }
        });
    });

    //home
    $("#loadHome").on("click", function (e) {
        debugger;
        e.preventDefault();
        $.get("/Home/Index")
            .done(function (htmlContent) {
                $("#taskContainer").hide();   
                $("#ProjectContainer").hide();   
                $("#HomeContainer").html(htmlContent).show(); 
            })
            .fail(function () {
                alert("Failed to load Home content.");
            });
    });
});