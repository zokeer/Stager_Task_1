var sync_func = function() {
    var table = $('#subnetTable').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/SubnetContainer/Get",
            "type": "GET"
        },
        "columns": [
            { "data": "Id" },
            { "data": "MaskedAddress" }
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
        ]
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
        function() {
            var data = table.row($(this).parents('tr')).data();
            console.info(data.Id + "edit");
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
}

$(document).ready(sync_func);