// Функция работает с WebApi.
var api_func = function () {
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
                    "=" + data.Id,
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
                        "=" + JSON.stringify({
                            old_id: data.Id,
                            new_id: $("#edit_id").val(),
                            address: $("#edit_address").val(),
                            mask: $("#edit_mask").val()
                        }),
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
            "=" +
            JSON.stringify({
                "id": $("#new_id").val(),
                "address": $("#new_address").val(),
                "mask": $("#new_mask").val()
            }),
            function(data) {
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

// Фукнция работает с ASMX.
var asmx_func = function () {
    // Функция, которая заполняет таблицу подсетей и минимального покрытия.
    var sync_func = function() {
        // Заполнение таблицы подсетей. Обращается к ASMX и парсит полученные данные.
        $.get("../ASMX/SubnetContainerWebService.asmx/Get",
            function(response) {
                var nodes = response.evaluate("/Subnets//Subnet", response, null, XPathResult.ANY_TYPE, null);
                var subnet = nodes.iterateNext();
                $("#subnetTable tbody").remove();
                $("#subnetTable").append("<tbody></tbody>");
                while (subnet) {
                    $("#subnetTable > tbody").append("<tr>" +
                        "<td>" +
                        subnet.childNodes[1].textContent +
                        "</td>" +
                        "<td>" +
                        subnet.childNodes[3].textContent +
                        "/" +
                        subnet.childNodes[5].textContent +
                        "</td>" +
                        "<td>" +
                        "<button class='deleteButton'>Удалить</button>" +
                        "</td>" +
                        "<td>" +
                        "<button class='editButton'>Изменить</button>" +
                        "</td>" +
                        "</tr>");
                    subnet = nodes.iterateNext();
                };
            });

        // Заполнение таблицы минимального покрытия. Обращается к ASMX и парсит полученные данные.
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
                    for (var j = 0; j < covered_subnets.length; j++) {
                        array_ids = array_ids + covered_subnets[j].getElementsByTagName("Id")[0].textContent + ", ";
                    }
                    $("#coverageTable > tbody").append("<tr>" +
                        "<td>" +
                        key.getElementsByTagName("Id")[0].textContent +
                        "</td>" +
                        "<td>" +
                        key.getElementsByTagName("Address")[0].textContent +
                        "/" +
                        key.getElementsByTagName("Mask")[0].textContent +
                        "</td>" +
                        "<td>" +
                        array_ids +
                        "</td>" +
                        "</tr>");
                }
            }
        );
    }

    // Вызываем функцию синхронизации данных в таблицах в начале работы.
    sync_func();

    //Обработчик нажатия на кнопку создания новой подсети.
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

    // Обработчик нажатия на любую из кнопок "Удалить".
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

    // Обработчик нажатия на любую из кнопок "Изменить". Включает в себя обработку модального окна.
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

    // +Обработчик кнопки, которая показывает таблицу минимального покрытия.
    $("#getCoverage").click(function () {
        sync_func();
        $("#coverage_table_wrapper").css("display", "block");
    });
}

// Аргумент функции ready - это функция, которая берёт данные. Возможны два варианта:
// 1)api_func - работает с WebApi.
// 2)asmx_func - работает с ASMX.
$(document).ready(asmx_func);