$(function () {
    //function toggleProjectLoader(show) {
    //    const $loader = $("#projectTableSpinner");
    //    if (!$loader.length) return; // element not found, skip

    //    if (show) {
    //        $loader.show();
    //    } else {
    //        $loader.hide();
    //    }
    
    // PROJECT LIST button
    $(document).on("click", "#getProjects", function (e) {
        e.preventDefault();
        loadProjects();
    });

    // show/hide table-scoped spinner
    //function showProjectTableSpinner() {
    //    const $spinner = $("#projectTableSpinner");
    //    if ($spinner.length) {
    //        $spinner.show().attr('aria-hidden', 'false');
    //    } else {
    //        // fallback: use layout/global spinner if per-table spinner not present
    //        toggleLoader(true);
    //    }
    //}
    //function hideProjectTableSpinner() {
    //    const $spinner = $("#projectTableSpinner");
    //    if ($spinner.length) {
    //        $spinner.hide().attr('aria-hidden', 'true');
    //    } else {
    //        toggleLoader(false);
    //    }
    //}

    // load partial then fetch data
    function loadProjects() {
        // cleanup leftover backdrops
        $('.modal-backdrop').remove();
        $('body').removeClass('modal-open').css('padding-right', '');

        $.get("/Projects/LoadProjects")
            .done(function (htmlContent) {
                $("#HomeContainer").hide();
                $("#taskContainer").hide();

                $("#ProjectContainer").html(htmlContent).show();
                debugger;
                // fetch data after partial injected
                fetchProjectData();
                //toggleProjectLoader(false);
            })
            .fail(function (err) {
                console.error("Error loading projects partial:", err);
                $("#ProjectContainer").html('<div class="text-center p-5 text-danger"><h4>Error loading projects</h4></div>');
                // ensure any spinner hidden
                //hideProjectTableSpinner();
            });
    }

    function fetchProjectData() {
        debugger;
        toggleLoader(true);
        //toggleProjectLoader(false);
        // show spinner scoped to table area
        //showProjectTableSpinner();

        //// watchdog in case DataTables.initComplete doesn't fire
        //const watchdog = setTimeout(function () {
        //    console.warn('project fetch watchdog fired — hiding spinner to avoid stuck UI');
        //    hideProjectTableSpinner();
        //}, 8000);

        $.ajax({
            type: "GET",
            url: "/Projects/GetAllProjects",
            dataType: "json",
            success: function (data) {
                var tableBody = $("#myProjectTable tbody");
                tableBody.empty();
                data.forEach(function (project) {
                    var row = `
                    <tr>
                        <td>${project.projectName ?? ''}</td>
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
                            <a href="#" class="fa fa-pencil openProjectModal text-decoration-none me-1" data-id="${project.id}"></a>
                            <a href="#" class="fa fa-eye openProjectDetailModal text-decoration-none text-dark me-1" data-id="${project.id}"></a>
                            <a href="#" class="fa fa-trash text-danger deleteProject" data-id="${project.id}"></a>
                        </td>
                    </tr>
                    `;
                    tableBody.append(row);
                });
                //toggleProjectLoader(false);
                //hideProjectTableSpinner();
                toggleLoader(false);
                // destroy existing DataTable instance if present
                if ($.fn.DataTable.isDataTable('#myProjectTable')) {
                    try { $('#myProjectTable').DataTable().clear().destroy(); } catch (ex) { console.warn('DataTable destroy failed', ex); }
                }

                // initialize DataTable and hide spinner in initComplete
                $('#myProjectTable').DataTable({
                    paging: true,
                    searching: true,
                    ordering: true,
                    info: true,
                    //initComplete: function () {
                    //    clearTimeout(watchdog);
                    //}
                });

                // short fallback hide
                //setTimeout(function () {
                //    //clearTimeout(watchdog);
                //    hideProjectTableSpinner();
                //}, 1500);
            },
            error: function (err) {
                console.error("Error loading projects:", err);
                $("#ProjectContainer").html('<div class="text-center p-5 text-danger"><h4>Error loading projects</h4></div>');
                //clearTimeout(watchdog);
                //hideProjectTableSpinner();
            }
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

    // keep remaining handlers (create/edit/delete/detail) unchanged...
});