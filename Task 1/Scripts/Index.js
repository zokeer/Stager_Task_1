var api_func = function () {
    //var table = $("#subnetTable").DataTable({
    //    "processing": true,
    //    "ajax": {
    //        "url": "/api/SubnetApi/Get",
    //        "type": "GET",
    //        "dataSrc": function (json) {
    //            console.log(json);
    //            return json;
    //        }
    //    },
    //    "columns": [
    //        {
    //            "type": "string",
    //            "data": "Id"
    //        },
    //        {
    //            "type": "string",
    //            "data": "MaskedAddress"
    //        }
    //    ],
    //    "columnDefs": [
    //        {
    //            "targets": 2,
    //            "data": null,
    //            "defaultContent": "<button id='deleteButton'>Удалить</button>"
    //        },
    //        {
    //            "targets": 3,
    //            "data": null,
    //            "defaultContent": "<button id='editButton'>Изменить</button>"
    //        }
    //    ],
    //    "order": []
    //});

    //$("#subnetTable tbody").on("click",
    //    "#deleteButton",
    //    function() {
    //        var data = table.row($(this).parents('tr')).data();
    //        if (confirm("Вы точно хотите удалить подсеть с идентификатором: " + data.Id + "?")) {
    //            $.post("/api/SubnetApi/DeleteSubnet",
    //                "=" + data.Id,
    //                function(data) {
    //                    $('#log_label').text(data);
    //                    table.ajax.reload();
    //                });
    //        }
    //    });

    //$("#subnetTable tbody").on("click",
    //    "#editButton",
    //    function() {
    //        var data = table.row($(this).parents("tr")).data();
    //        $("#editModal").css("display", "flex");
    //        $("#editModal").click(function(event) {
    //            if (event.target == $("#editModal")[0]) {
    //                $("#editModal").css("display", "none");
    //            }
    //        });
    //        $("#submit_editted_subnet").click(function () {
    //            if (confirm("Вы точно хотите изменить подсеть с идентификатором: " + data.Id +
    //                "\nНа подсеть: " + $("#edit_id").val() + ", " + $("#edit_address").val()
    //                + "/" + $("#edit_mask").val())) {
    //                $.post("/api/SubnetApi/EditSubnet",
    //                    "=" + JSON.stringify({
    //                        old_id: data.Id,
    //                        new_id: $("#edit_id").val(),
    //                        address: $("#edit_address").val(),
    //                        mask: $("#edit_mask").val()
    //                    }),
    //                    function(data) {
    //                        $("#log_label").text(data);
    //                        $("#editModal").css("display", "none");
    //                        table.ajax.reload();
    //                    });
    //            }
    //        });
    //    });

    //$("#submit_new_subnet").click(function() {
    //    $.post("/api/SubnetApi/CreateSubnet",
    //        "=" +
    //        JSON.stringify({
    //            "id": $("#new_id").val(),
    //            "address": $("#new_address").val(),
    //            "mask": $("#new_mask").val()
    //        }),
    //        function(data) {
    //            $("#log_label").text(data);
    //            table.ajax.reload();
    //        });
    //});

    //var coverage_table = $("#coverageTable").DataTable({
    //    "processing": true,
    //    "ajax": {
    //        "url": "/api/SubnetApi/GetCoverage",
    //        "type": "GET",
    //        "dataSrc": function (json) {
    //            console.log(json);
    //            return json;
    //        }
    //    },
    //    bAutoWidth : false,
    //    "columns": [
    //        { data: "KeyId" },
    //        { data: "KeyMaskedAddress" },
    //        { data: "Children" }
    //    ]
    //});

    //$("#getCoverage").click(function () {
    //    coverage_table.ajax.reload();
    //    $("#coverage_table_wrapper").css("display", "block");
    //    coverage_table.columns.adjust().draw();
        
    //});
}

var asmx_func = function() {

    var sync_func = function() {
        $.get("../ASMX/SubnetContainerWebService.asmx/Get",
            function(response) {
                var subnets = response.childNodes[0].children;
                $("#subnetTable tbody").remove();
                $("#subnetTable").append("<tbody></tbody>");
                for (var i = 0; i < subnets.length; i++) {
                    $("#subnetTable > tbody").append("<tr>" +
                        "<td>" +
                        subnets[i].children[0].textContent +
                        "</td>" +
                        "<td>" +
                        subnets[i].children[1].textContent +
                        "/" +
                        subnets[i].children[2].textContent +
                        "</td>" +
                        "<td>" +
                        "<button class='deleteButton'>Удалить</button>" +
                        "</td>" +
                        "<td>" +
                        "<button class='editButton'>Изменить</button>" +
                        "</td>" +
                        "</tr>");
                }
            });

        $.get("../ASMX/SubnetContainerWebService.asmx/GetCoverage",
            function(response) {
                var subnets = response.childNodes[0].children;
                $("#coverageTable tbody").remove();
                $("#coverageTable").append("<tbody></tbody>");
                for (var i = 0; i < subnets.length; i++) {
                    var key = subnets[i].getElementsByTagName("Key")[0];
                    var value = subnets[i].getElementsByTagName("Value")[0];
                    var covered_subnets = value.getElementsByTagName("Subnet");
                    var array_ids = "";
                    console.info(covered_subnets);
                    for (var j = 0; j < covered_subnets.length; j++) {
                        array_ids = array_ids + covered_subnets[j].getElementsByTagName("Id")[0].textContent + ", ";
                    }
                    console.info(array_ids);
                    $("#coverageTable > tbody").append("<tr>" +
                        "<td>" +
                        key.children[0].textContent +
                        "</td>" +
                        "<td>" +
                        key.children[1].textContent +
                        "/" +
                        key.children[2].textContent +
                        "</td>" +
                        "<td>" +
                        array_ids +
                        "</td>" +
                        "</tr>");
                }
            }
        );
    }

    sync_func();

    $("#submit_new_subnet").click(function() {
        $.post("../ASMX/SubnetContainerWebService.asmx/Create",
            {
                "id": $("#new_id").val(),
                "address": $("#new_address").val(),
                "mask": $("#new_mask").val()
            },
            function(response) {
                $("#log_label").text(response.childNodes[0].childNodes[0].data);
                sync_func();
            });
    });

    $("#subnetTable").on("click",
        ".deleteButton",
        function() {
            var data = $(this).parents("tr")[0];
            var id = data.children[0].textContent;
            if (confirm("Вы точно хотите удалить подсеть с идентификатором: " + id + "?")) {
                $.post("../ASMX/SubnetContainerWebService.asmx/Delete",
                    {
                        "id": id
                    },
                    function(response) {
                        $("#log_label").text(response.childNodes[0].childNodes[0].data);
                        sync_func();
                    });
            }
        });


    $("#subnetTable").on("click",
        ".editButton",
        function() {
            var data = $(this).parents("tr")[0];
            var old_id = data.children[0].textContent;
            $("#editModal").css("display", "flex");
            $("#editModal").click(function(event) {
                if (event.target == $("#editModal")[0]) {
                    $("#editModal").css("display", "none");
                }
            });
            $("#submit_editted_subnet").click(function() {
                if (confirm("Вы точно хотите изменить подсеть с идентификатором: " +
                    old_id +
                    "\nНа подсеть: " +
                    $("#edit_id").val() +
                    ", " +
                    $("#edit_address").val() +
                    "/" +
                    $("#edit_mask").val())) {
                    $.post("../ASMX/SubnetContainerWebService.asmx/Edit",
                        {
                            old_id: old_id,
                            new_id: $("#edit_id").val(),
                            address: $("#edit_address").val(),
                            mask: $("#edit_mask").val()
                        },
                        function(response) {
                            $("#log_label").text(response.childNodes[0].childNodes[0].data);
                            $("#editModal").css("display", "none");
                            sync_func();
                        });
                }
            });
        });

    $("#getCoverage").click(function() {
        $("#coverage_table_wrapper").css("display", "block");
    });
}

$(document).ready(asmx_func);