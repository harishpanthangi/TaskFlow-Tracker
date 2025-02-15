//function confirmDelete(uniqueId, isDeleteClicked) {
//    var deletespan = 'deletespan_' + uniqueId;
//    var confirmDeleteSpan = 'confirmDeleteSpan_' + uniqueId;

//    if (isDeleteClicked) {
//        $('#' + deletespan).hide();
//        $('#' + confirmDeleteSpan).show();
//    }
//    else {
//        $('#' + confirmDeleteSpan).hide();
//        $('#' + deletespan).show();
//    }

//}
function confirmDelete(uniqueId) {
    bootbox.confirm({
        title:"<h5 class='text-danger'>Warning</h5>",
        message: "Are you sure you want to delete this?",
        buttons: {
            confirm: {
                label: 'Yes',
                className: 'btn-danger'
            },
            cancel: {
                label: 'No',
                className: 'btn-primary'
            }
        },
        callback: function (result) {
            if (result) {
                // Perform the delete operation
                deleteCategory(uniqueId);
            }
        }
    });
    setTimeout(function () {
        let modalHeader = $('.modal-header');
        let title = modalHeader.find('.modal-title');
        let closeButton = modalHeader.find('.bootbox-close-button');

        // Move the title before the close button
        closeButton.detach().appendTo(modalHeader);
    }, 50);
    $('.modal-header').css('display', 'flex'); // Make header flexbox
    $('.modal-header .modal-title').css('flex-grow', '1'); // Push title to the left
}

function deleteCategory(categoryId) {
    $.ajax({
        url: '/api/Category/DeleteCategory?categoryID=' + categoryId, // Update with your actual delete endpoint
        type: 'POST',
        success: function (response) {
            if (response.success) {
                bootbox.alert("Category deleted successfully!", function () {
                    location.reload(); // Refresh page or remove the deleted row
                });
            } else {
                bootbox.alert("Error: " + response.message);
            }
        },
        error: function () {
            bootbox.alert("An error occurred while deleting.");
        }
    });
}

//Project
function confirmProjectDelete(uniqueId) {
    bootbox.confirm({
        title: "<h5 class='text-danger'>Warning</h5>",
        message: "Are you sure you want to delete this?",
        buttons: {
            confirm: {
                label: 'Yes',
                className: 'btn-danger'
            },
            cancel: {
                label: 'No',
                className: 'btn-primary'
            }
        },
        callback: function (result) {
            if (result) {
                // Perform the delete operation
                deleteProject(uniqueId);
            }
        }
    });
    setTimeout(function () {
        let modalHeader = $('.modal-header');
        let title = modalHeader.find('.modal-title');
        let closeButton = modalHeader.find('.bootbox-close-button');

        // Move the title before the close button
        closeButton.detach().appendTo(modalHeader);
    }, 50);
    $('.modal-header').css('display', 'flex'); // Make header flexbox
    $('.modal-header .modal-title').css('flex-grow', '1'); // Push title to the left
}

function deleteProject(projectId) {
    var dataValue = { ProjectId:projectId };
    $.ajax({
        url: deleteProjectUrl,
        type: 'POST',
        data: dataValue,
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                bootbox.alert("Project deleted successfully!", function () {
                    location.reload(); // Refresh page or remove the deleted row
                });
            } else {
                bootbox.alert("Error: " + response.message);
            }
        },
        error: function () {
            bootbox.alert("An error occurred while deleting.");
        }
    });
}

// Task Deletion

function confirmTaskDelete(uniqueId) {
    bootbox.confirm({
        title: "<h5 class='text-danger'>Warning</h5>",
        message: "Are you sure you want to delete this?",
        buttons: {
            confirm: {
                label: 'Yes',
                className: 'btn-danger'
            },
            cancel: {
                label: 'No',
                className: 'btn-primary'
            }
        },
        callback: function (result) {
            if (result) {
                // Perform the delete operation
                deleteTask(uniqueId);
            }
        }
    });
    setTimeout(function () {
        let modalHeader = $('.modal-header');
        let title = modalHeader.find('.modal-title');
        let closeButton = modalHeader.find('.bootbox-close-button');

        // Move the title before the close button
        closeButton.detach().appendTo(modalHeader);
    }, 50);
    $('.modal-header').css('display', 'flex'); // Make header flexbox
    $('.modal-header .modal-title').css('flex-grow', '1'); // Push title to the left
}

function deleteTask(taskId) {
    var dataValue = { TaskId: taskId };
    $.ajax({
        url: TaskDeleteUrl,
        type: 'POST',
        data: dataValue,
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                bootbox.alert("Project deleted successfully!", function () {
                    location.reload(); // Refresh page or remove the deleted row
                });
            } else {
                bootbox.alert("Error: " + response.message);
            }
        },
        error: function () {
            bootbox.alert("An error occurred while deleting.");
        }
    });
}

//TasksDone function

function fxsave_tskdn_data(ProjectId, Description, Screen, attachment, TaskID, AdminID) {
    if (ProjectId != "" || null) {
        //alert("jjj")
        var formData = new FormData();  // Create FormData object to send files
        formData.append("ProjectID", ProjectId);
        formData.append("Description", Description);
        formData.append("Screen", Screen);
        formData.append("Attachment", attachment);
        formData.append("TaskID", TaskID);
        formData.append("AdminID", AdminID);
        $.ajax({
            type: "POST",
            url: TaskDoneUrl,
            data: formData,
            contentType: false,
            processData: false,
            dataType: "json",
            success: function (data2) {
                alert("jjj")
                if (data2.status == "success") {
                    $("#success").html("<div class='alert alert-success alert-dismissable'><i class='fa fa-info-circle fa-fw'></i><button class='close' aria-hidden='true' data-dismiss='alert' type='button'>×</button>" + data2.message + "</div>");
                    $("#Loder").modal('hide');
                }

                else {
                    $("#warning").html("<div class='alert alert-danger alert-dismissable'><i class='fa fa-info-circle fa-fw'></i><button class='close' aria-hidden='true' data-dismiss='alert' type='button'>×</button>" + data2.message + "</div>");
                    $("#Loder").modal('hide');
                }
            }


        });
    }
}