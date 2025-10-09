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
             
            },
            error: function (err) {
                alert("Failed to load project content.");
                console.error("Error loading projects:", err);
            }
        });
       
    });
    });