$(function () {
    // PROJECT LIST button
    $(document).on("click", "#getTaskItems", function (e) {
        e.preventDefault();
        loadTaskItems();
    });
    // load partial then fetch data
    function loadTaskItems() {
        debugger;
        $('.modal-backdrop').remove();
        $('body').removeClass('modal-open').css('padding-right', '');

        $.get("/TaskItems/LoadTaskItems")
            .done(function (htmlContent) {
                $("#HomeContainer").hide();
                $("#taskContainer").hide();
                $("#ProjectContainer").hide();


                $("#TaskItemsContainer").html(htmlContent).show();
                fetchTaskItemsData();
            })
            .fail(function (err) {
                Swal.fire({ title: 'Error!', text: 'Error loading projects', icon: 'error' });

                console.error("Error loading projects partial:", err);
                $("#TaskItemsContainer").html('<div class="text-center p-5 text-danger"><h4>Error loading Task Items Container</h4></div>');
                toggleLoader(false);
            });
    }
    function fetchTaskItemsData() {
        debugger;
        toggleLoader(true);

        // Destroy any previous DataTable instance
        if ($.fn.DataTable.isDataTable('#myTaskItemsTable')) {
            try { $('#myTaskItemsTable').DataTable().clear().destroy(); } catch (ex) { console.warn('DataTable destroy failed', ex); }
        }
        // Initialize DataTable with server-side processing
        $('#myTaskItemsTable').DataTable({
            autoWidth: false,
            serverSide: true,
            processing: true,
            dom: '<"top"<"top-left"f><"top-right"l>>rt<"bottom"<"bottom-left"i><"bottom-right"p>>',
            pageLength: 5,
            lengthMenu: [5, 10, 25, 50, 75, 100],
            
            ajax: {
                url: '/TaskItems/GetTaskItemsDataTable',
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
                { data: 'taskName' },
                { data: 'description' },
                { data: 'startDate' },
                { data: 'endDate' },   
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
});