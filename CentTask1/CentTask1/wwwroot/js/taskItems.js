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
    const modal = document.getElementById('#taskItemModal');
        let entries = [];
        let editingIndex = -1;


    // open modal for task item
    $(document).on('click', '.openTaskItemModal', function (e) {
        debugger;
        e.preventDefault();
        //modal.style.display = 'flex';
        //var title = id ? "Update Task Item" : "Create Task Item";
        $("#taskItemModalTitle").text("Create Task Item");
        $('#taskItemModal').modal('show');
        resetForm();
    });
        
        function closeModal() {
            modal.style.display = 'none';
        entries = [];
        renderTable();
        resetForm();
        }
    function resetForm() {
        debugger;
            document.getElementById('taskName').value = '';
        document.getElementById('description').value = '';
        document.getElementById('startDate').value = '';
        document.getElementById('endDate').value = '';
        document.getElementById('status').value = '';
        clearErrors();
        editingIndex = -1;
        document.getElementById('addBtn').textContent = '+';
        }
        function clearErrors() {
            document.querySelectorAll('.error').forEach(el => el.textContent = '');
        }
        function validateForm() {
            clearErrors();
        let valid = true;
        const taskName = document.getElementById('taskName').value.trim();
        const desc = document.getElementById('description').value.trim();
        const startDate = document.getElementById('startDate').value;
        const endDate = document.getElementById('endDate').value;
        const status = document.getElementById('status').value;

        if (!taskName) {document.getElementById('taskNameError').textContent = 'Required'; valid = false; }
        if (!desc) {document.getElementById('descError').textContent = 'Required'; valid = false; }
        if (!startDate) {document.getElementById('startDateError').textContent = 'Required'; valid = false; }
        if (!endDate) {document.getElementById('endDateError').textContent = 'Required'; valid = false; }
        if (!status) {document.getElementById('statusError').textContent = 'Required'; valid = false; }
        return valid;
    }

    //add task item to list
    $(document).on('click', '#addItem', function (e) {
            if (!validateForm()) return;
        const entry = {
            taskName: document.getElementById('taskName').value.trim(),
        description: document.getElementById('description').value.trim(),
        startDate: document.getElementById('startDate').value,
        endDate: document.getElementById('endDate').value,
        status: document.getElementById('status').value
            };
            if (editingIndex >= 0) {
            entries[editingIndex] = entry;
        editingIndex = -1;
        document.getElementById('addBtn').textContent = '+';
            } else {
            entries.push(entry);
            }
        renderTable();
        resetForm();
        

    });
        function renderTable() {
            const tbody = document.getElementById('tableBody');
        tbody.innerHTML = '';
            entries.forEach((entry, index) => {
                const row = document.createElement('tr');
        row.innerHTML = `
        <td>${entry.taskName}</td>
        <td>${entry.description}</td>
        <td>${entry.startDate}</td>
        <td>${entry.endDate}</td>
        <td>${entry.status}</td>
        <td>
            <button class="action-btn edit-btn" onclick="editEntry(${index})">✏️</button>
            <button class="action-btn delete-btn" onclick="deleteEntry(${index})">🗑️</button>
        </td>
        `;
        tbody.appendChild(row);
            });
        document.getElementById('totalMHS').textContent = entries.length.toFixed(1);
        document.getElementById('laborTotal').textContent = (entries.length * 100).toFixed(2); // Example calculation
        document.getElementById('submitBtn').disabled = entries.length === 0;
        }
        function editEntry(index) {
            const e = entries[index];
        document.getElementById('taskName').value = e.taskName;
        document.getElementById('description').value = e.description;
        document.getElementById('startDate').value = e.startDate;
        document.getElementById('endDate').value = e.endDate;
        document.getElementById('status').value = e.status;
        editingIndex = index;
        document.getElementById('addBtn').textContent = 'Update';
        }
        function deleteEntry(index) {
            if (confirm('Remove this entry?')) {
            entries.splice(index, 1);
        renderTable();
            }
        }
        function submitEntries() {
            if (entries.length === 0) return;
        // Simulate submission
        console.log('Submitting Task Items:', entries);
        alert(`Submitted ${entries.length} entries!\nTotal MHS: ${document.getElementById('totalMHS').textContent}\nLabor Total: $${document.getElementById('laborTotal').textContent}`);
            // In real app: send to backend via fetch()
            // fetch('/TaskItems/CreateMultiple', { method: 'POST', body: JSON.stringify(entries), headers: { 'Content-Type': 'application/json' } })
            closeModal();
        } 
});