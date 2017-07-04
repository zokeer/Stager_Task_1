$(document).ready(function () {
    $('#subnetTable').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/SubnetContainer/Get",
            "type": "GET"
        },
        "columns": [
            { "data": "Id" },
            { "data": "MaskedAddress" }
        ]
    });
});