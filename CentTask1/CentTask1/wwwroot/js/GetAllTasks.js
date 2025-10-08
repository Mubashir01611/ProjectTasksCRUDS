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

                var tbody = $("#myTable tbody");
                tbody.empty(); // clear old rows

                data.forEach(function (task) {
                    var row = `
                <tr>
                    <td>${task.name}</td>
                    <td>${task.description}</td>
                    <td>${task.startDate}</td>
                    <td>${task.dueDate}</td>
                    <td>${task.priority}</td>
                    <td>${task.assignedTo}</td>
                    <td>${task.equipmentType}</td>
                    <td>${task.twr}</td>
                    <td>
                        <a href="#" class="fa fa-pencil openTaskModal text-decoration-none me-1" data-url="" data-id="${task.id}"></a>
                        <a href="#" class="fa fa-eye  text-decoration-none openTaskDetailModal  text-dark me-1" data-id="${task.id}"></a>
                        <a href="#" class="fa fa-trash text-danger deleteTask" data-id="${task.id}"></a>
                    </td>
                </tr>
            `;
                    tbody.append(row);
                });
                setTimeout(function () {
                    if ($.fn.DataTable.isDataTable('#myTable')) {
                        $('#myTable').DataTable().destroy();
                    }

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
        $('#createOrEditProjectTask').modal.footer('hide');
    });

    //modal open
    $(document).on('click', '.openTaskModal', function (e) {
        debugger;
        e.preventDefault();
        var id = $(this).data("id");
        // var url = $(this).data("url");

        $.ajax({
            type: "GET",
            url: "Tasks/CreateProjectTask" + (id ? "?id=" + id : ""),
            success: function (htmlContent) {
                $("#modalBodyContent").html(htmlContent);
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
        var title = id ? "Update Task" : "Create Task";
        $("#modalTitle").text(title);
        $('#createOrEditProjectTask').modal('show');
    });


    //CreateOrEdit
    $(document).on('click', '#submitProjectTaskId', function (e) {
        debugger;

        e.preventDefault();
        var id = $(this).data("id");

        //var url = $(this).data("url"); // e.g. "/Tasks/Create"
        var url = "/Tasks/Create";
        var form = $('#createOrEditProjectTaskForm'); // form's id
        var formData = form.serialize(); // convert form fields to query string

        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            success: function (response) {

                // Close modal
                $('#createOrEditProjectTask').modal('hide');

                // Optionally clear form
                form[0].reset();

                // Reload task table
                loadTasks();
            },
            error: function (err) {
                console.error("Error submitting form:", err);
                alert("Something went wrong while submitting the form.");
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
                    url: "/Tasks/DeleteConfirmed",
                    data: { id: id },
                    success: function () {
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

    //DataTable yeh jb ham sidebar sy click krty hain to load hota hai
    $(".sidebar-link").on("click", function (e) {
        debugger;
        e.preventDefault();
        loadTasks();
    });

    //home
    $("#loadHome").on("click", function (e) {
        debugger;
        e.preventDefault();
        $.get("Home/Index")
            .done(function (htmlContent) {
                $("#taskContainer").hide();   
                $("#HomeContainer").html(htmlContent).show(); 
            })
            .fail(function () {
                alert("Failed to load Home content.");
            });
    });
});
