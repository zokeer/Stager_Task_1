var sync_func = function () {
    var table = $("#subnetTable").DataTable({
        "processing": true,
        "ajax": {
            "url": "/api/SubnetApi/Get",
            "type": "GET",
            "dataSrc": function (json) {
                console.log(json);
                return json;
            }
        },
        "columns": [
            {
                "type": "string",
                "data": "Id"
            },
            {
                "type": "string",
                "data": "MaskedAddress"
            }
        ],
        "columnDefs": [
            {
                "targets": 2,
                "data": null,
                "defaultContent": "<button id='deleteButton'>Удалить</button>"
            },
            {
                "targets": 3,
                "data": null,
                "defaultContent": "<button id='editButton'>Изменить</button>"
            }
        ],
        "order": []
    });

    $("#subnetTable tbody").on("click",
        "#deleteButton",
        function() {
            var data = table.row($(this).parents('tr')).data();
            if (confirm("Вы точно хотите удалить подсеть с идентификатором: " + data.Id + "?")) {
                $.post("/api/SubnetApi/DeleteSubnet",
                    { id: data.Id },
                    function(data) {
                        $('#log_label').text(data);
                        table.ajax.reload();
                    });
            }
        });

    $("#subnetTable tbody").on("click",
        "#editButton",
        function() {
            var data = table.row($(this).parents("tr")).data();
            $("#editModal").css("display", "flex");
            $("#editModal").click(function(event) {
                if (event.target == $("#editModal")[0]) {
                    $("#editModal").css("display", "none");
                }
            });
            $("#submit_editted_subnet").click(function () {
                if (confirm("Вы точно хотите изменить подсеть с идентификатором: " + data.Id +
                    "\nНа подсеть: " + $("#edit_id").val() + ", " + $("#edit_address").val()
                    + "/" + $("#edit_mask").val())) {
                    $.post("/api/SubnetApi/EditSubnet",
                        {
                            old_id: data.Id,
                            new_id: $("#edit_id").val(),
                            address: $("#edit_address").val(),
                            mask: $("#edit_mask").val()
                        },
                        function(data) {
                            $("#log_label").text(data);
                            $("#editModal").css("display", "none");
                            table.ajax.reload();
                        });
                }
            });
        });

    $("#submit_new_subnet").click(function() {
        $.post("/api/SubnetApi/CreateSubnet",
            JSON.stringify({
                id: $("#new_id").val(),
                address: $("#new_address").val(),
                mask: $("#new_mask").val()
            }),
            function(data) {
                console.info(typeof data);
                $("#log_label").text(data);
                table.ajax.reload();
            });
    });

    var coverage_table = $("#coverageTable").DataTable({
        "processing": true,
        "ajax": {
            "url": "/api/SubnetApi/GetCoverage",
            "type": "GET",
            "dataSrc": function (json) {
                console.log(json);
                return json;
            }
        },
        bAutoWidth : false,
        "columns": [
            { data: "KeyId" },
            { data: "KeyMaskedAddress" },
            { data: "Children" }
        ]
    });

    $("#getCoverage").click(function () {
        coverage_table.ajax.reload();
        $("#coverage_table_wrapper").css("display", "block");
        coverage_table.columns.adjust().draw();
        
    });
}

$(document).ready(sync_func);