﻿var DataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    DataTable = $('#tblData').DataTable({
        "ajax": {
            url: "Frequency/GetAll",
            type: "GET",
            dataType: "json"
        },
        "columns": [
            { "data": "name", "width": "50%" },
            { "data": "frequencyCount", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="Frequency/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                                    <i class="far fa-edit"></i> Edit
                                </a>
                                &nbsp;
                                <a onclick=Delete("Frequency/Delete/${data}") class="btn btn-danger  text-white" style="cursor:pointer; width:100px;">
                                    <i class="far fa-trash-alt"></i> Delete
                                </a>
                            </div>
                            `;
                }, "width": "30%"
            }
        ],
        "language": {
            "emptyTable": "No Records found."
        },
        "width": "100%"
    });
};

function Delete(url) {
    swal({
        title: "Are you sure you want to delete?",
        text: "You will not be able to restore the content!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6b55",
        confirmButtonText: "Yes, delete it!",
        closeOnconfirm: true
    }, function () {
        $.ajax({
            type: 'DELETE',
            url: url,
            success: function (data) {
                if (data.success) {
                    ShowMessage(data.message);
                    DataTable.ajax.reload();
                } else {
                    toastr.error(data.message);
                }
            }
        })
    });
}

function ShowMessage(msg) {
    toastr.success(msg);
}