$(function () {
    // Sidebar Tasks button
    //changing the class temporary
    $(document).on("click", "#getTasks", function (e) {
        debugger;
        e.preventDefault();
        loadTasks(); 
    });

    function loadTasks() {
      
        $.ajax({
            type: "GET",
            url: "Tasks/LoadTaskTable",
            success: function (htmlContent) {
                $("#HomeContainer").hide();
                $("#ProjectContainer").hide();
                //toggleLoader(false);
                $("#taskContainer").html(htmlContent).show();

                // Fetch data and initialize table
                fetchTaskData();
            },
            error: function (err) {
                toggleLoader(false);
                Swal.fire({ title: 'Error!', text: 'Error loading tasks', icon: 'error' });
                console.error("Error loading partial view:", err);
                //toggleLoader(false);
                $("#taskContainer").html(
                    '<div class="text-center p-5 text-danger"><h4>Error loading tasks</h4></div>'
                );
            },
           
        });

    }

    function fetchTaskData() {
        toggleLoader(true);
        if ($.fn.DataTable.isDataTable('#myTable')) {
            $('#myTable').DataTable().clear().destroy();
        }
        $('#myTable').DataTable({
            serverSide: true,
            processing: true,
            dom: '<"top-left"lf>rt<"bottom"<"bottom-left"i><"bottom-right"p>>',

            //dom: 'Blfrtip', // Add this line to enable buttons
            //buttons: [
            //    //'excel', 'pdf', 'print'
            //    //'print'
            //],
            pageLength: 5,
            lengthMenu: [5, 10, 25, 50, 100],
            ajax: {
                url: '/Tasks/GetTasksDataTable',
                type: 'POST',
                dataType: 'json',
                data: function (d) { return d; },
                beforeSend: function () { toggleLoader(true); },
                complete: function () { toggleLoader(false); }
            },
            columns: [
                { data: 'taskName' },
                { data: 'description' },
                { data: 'startDate' },
                { data: 'endDate' },
                { data: 'priority' },
                { data: 'equipmentType' },
                { data: 'twr' },
                { data: 'projectName' },
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
                                statusText = 'Not Started';
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
                            <a href="#" class="fa fa-pencil text-decoration-none me-1 openTaskModal" data-url="Tasks/EditProjectTaskForm" data-id="${id}"></a>
                            <a href="#" class="fa fa-eye text-decoration-none openTaskDetailModal text-dark me-1" data-id="${id}"></a>
                            <a href="#" class="fa fa-trash text-danger deleteTask" data-id="${id}"></a>
                        `;
                    }
                }
            ],
            order: [[0, 'asc']]
        });
    }

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
        $('.createOrEditModal').modal('show'); 
    });

    // Initialize project select2 inside task modal with defensive load fallback
    function initTaskProjectSelect(retryCount = 0) {
        var $modal = $('.createOrEditModal');
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
        // Check for selected value (edit mode)
        var selectedId = $select.data('selected-id') || $select.val();
        var selectedText = $select.data('selected-text') || $select.find('option:selected').text();


        if (selectedId && selectedText) {
            var option = new Option(selectedText, selectedId, true, true);
            $select.append(option).trigger('change');
            console.debug('[initTaskProjectSelect] manually selected project:', selectedText);
        }
        console.debug('[initTaskProjectSelect] initialized select2 on #projectSelect');
    }

    //modal open dynamically for create and edit
    $(document).on('click', ".openTaskModal", function (e) {
        debugger;
        e.preventDefault();
        var id = $(this).data("id");
        var url = $(this).data("url"); // e.g. "/Tasks/CreateProjectTask" or "/Tasks/EditProjectTask"
        $.ajax({
            type: "GET",
            url: url + (id ? "?id=" + id : ""),
            success: function (htmlContent) {
                $("#modalBodyContent").html(htmlContent);

                // re-parse unobtrusive validation
                if ($.validator && $.validator.unobtrusive) {
                    $.validator.unobtrusive.parse('.createOrEditForm');
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
        $('.createOrEditModal').modal('show');
    });


    //CreateOrEdit submit
    $(document).on('click', '.submitForm', function (e) {
        debugger;

        e.preventDefault();
        var $btn = $(this);
        var url = $btn.data("url"); // e.g. "/Tasks/Create" or "/Tasks/Edit"
        var formId = $btn.data("form-id"); // e.g. "createOrEditProjectTaskForm" or "editProjectTaskForm"
        var $form = $('#' + formId);// e.g. "#createOrEditProjectTaskForm"
        var formData = $form.serialize();
        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            success: function (response) {
                if (response.success) {
                    // Close modal
                    $('.createOrEditModal').modal('hide');
                    Swal.fire( "Success!", "Task created successfully", "success" );
                    // Optionally clear form
                    $form[0].reset();

                    // Reload task table
                    loadTasks();

                }
                else {
                    // If server returns validation errors, display them
                    $("#modalBodyContent").html(response);

                    // Rebind validation to newly injected form
                    $.validator.unobtrusive.parse(".createOrEditForm");
                Swal.fire("error!", "Invalid Form", "error");

                }
            },
            error: function (err) {
                    //$('#createOrEditProjectTask').modal('hide');
                $.validator.unobtrusive.parse(".createOrEditForm");
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