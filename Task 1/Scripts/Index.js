var sync_func = function() {
    var table = $('#subnetTable').DataTable({
        "processing": true,
        "ajax": {
            "url": "/SubnetContainer/Get",
            "type": "GET"
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
        "order": [],
    });

    $('#subnetTable tbody').on('click',
        '#deleteButton',
        function() {
            var data = table.row($(this).parents('tr')).data();
            $.post("/SubnetContainer/DeleteSubnet", { id: data.Id })
                .done(function(data) {
                    table.ajax.reload();
                });
        });

    $('#subnetTable tbody').on('click',
        '#editButton',
        function () {
            var data = table.row($(this).parents('tr')).data();
            $('#editModal').css("display", "flex");
            $('#editModal').click(function (event) {
                if (event.target == $('#editModal')[0]) {
                    $('#editModal').css("display", "none");
                }
            });
            $('#submit_editted_subnet').click(function() {
                $.post("/SubnetContainer/EditSubnet",
                        {
                            old_id: data.Id,
                            new_id: $('#edit_id').val(),
                            address: $('#edit_address').val(),
                            mask: $('#edit_mask').val()
                        })
                    .done(function (data) {
                        $('#editModal').css("display", "none");
                        table.ajax.reload();
                    });
            });
        });

    $('#submit_new_subnet').click(function() {
        $.post("/SubnetContainer/CreateSubnet",
                {
                    id: $('#new_id').val(),
                    address: $('#new_address').val(),
                    mask: $('#new_mask').val()
                })
            .done(function(data) {
                table.ajax.reload();
            });
    });

    $('#getCoverage').click(function () {
        $('#coverage_table_wrapper').css('display', 'block');
        $('#coverageTable').DataTable({
            "processing": true,
            "ajax": {
                "url": "/SubnetContainer/GetCoverage",
                "type": "GET"
            },
            columns: [
                { data: "KeyId" },
                { data: "KeyMaskedAddress" },
                { data: "Children" }
            ]
        });
    });
}

$(document).ready(sync_func);