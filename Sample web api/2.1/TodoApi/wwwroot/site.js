const uri = 'api/employee/';

$(document).ready(function () {
    table = $('#myTable').DataTable({
        "columnDefs": [
            {
                "targets": 0,
                "visible": false,
                "searchable": false
            },
            {
                "targets": 9,
                "data": null,
                "render": function (data, type, row) {
                    var id = data[0];
                    var html = "<td><button onclick='editItem(this," + id + ")'>Edit</button></td>"
                    return html;
                }
            },
            {
                "targets": 10,
                "data": null,
                "render": function (data, type, row) {
                    var id = data[0];
                    var html = "<td><button onclick='deleteItem(this," + id + ")'>Delete</button></td>"
                    return html;
                }
            }
        ]
    });
    getData();
});

function getData() {
    $.ajax({
        type: 'GET',
        url: uri + 'GetAll',
        cache: false,
        success: function (data) {
            var table = $('#myTable').DataTable();
            table.clear();
            $.each(data, function (key, item) {
                paintTable(table, item);
            });
        }
    });
}

function addItem() {
    var item = getItem('add');
    if (validateEmail('add') && validatePhone('add')) {
        $.ajax({
            type: 'POST',
            accepts: 'application/json',
            url: uri + 'Create',
            contentType: 'application/json',
            data: JSON.stringify(item),
            error: function (jqXHR, textStatus, errorThrown) {
                alert("add失败，email重复");
            },
            success: function (result) {
                var table = $('#myTable').DataTable();
                paintTable(table, result);
                getData();
                alert('add成功');
            }
        });
    }
}

function paintTable(table, item) {
    table.row.add([
        item.id,
        item.firstName,
        item.lastName,
        item.gender,
        item.birth,
        item.email,
        item.address,
        item.phone,
        item.department,
    ]).draw();
}

function editItem(obj, id) {
    var table = $('#myTable').DataTable();
    var rowIndex = table.cell($(obj).parent()).index().row;
    var parameters = new Array('id', 'firstname', 'lastname', 'gender', 'birth', 'email', 'address', 'phone', 'department');
    for (var i = 0; i < parameters.length; i++) {
        $('#edit-' + parameters[i]).val(table.data()[rowIndex][i]);
    }
    $('#spoiler').css({ 'display': 'block' });
}

function saveItem() {
    var item = getItem('edit');
    if (confirm("真的要修改吗")) {
        $.ajax({
            url: uri + 'Update/' + $('#edit-id').val(),
            type: 'PUT',
            accepts: 'application/json',
            contentType: 'application/json',
            data: JSON.stringify(item),
            async: false,
            error: function (jqXHR, textStatus, errorThrown) {
                alert("edit失败，请检查phone和email");
            },
            success: function (result) {
                alert('edit成功');
                getData();
                closeInput();
            }
        });
    }
    return false;
}

function deleteItem(obj, id) {
    if (confirm("真的要删除吗")) {
        $.ajax({
            url: uri + '/Delete/' + id,
            type: 'DELETE',
            success: function (result) {
                var table = $('#myTable').DataTable();
                table.row($(obj).parents('tr')).remove().draw();
            }
        });
    }
}

function closeInput() {
    $('#spoiler').css({ 'display': 'none' });
}

function validatePhone(str) {
    var phone = '';
    if (str == 'add') phone = $('#add-phone').val();
    else phone = $('#edit-phone').val();
    if (!(/^1[34578]\d{9}$/.test(phone))) {
        $('#phoneError').html('这个Phone号有毒！');
        return false;
    } else {
        $("#phoneError").html("");
        return true;
    }
}

function validateEmail(str) {
    var email = '';
    if (str == 'add') email = $('#add-email').val();
    else email = $('#edit-email').val();
    if (!(/^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/.test(email))) {
        $("#emailError").html("这个Email号有毒！");
        //alert("这个Email号有毒！");
        return false;
    } else {
        $('#emailError').html('');
        return true;
    }
}

function getItem(str) {
    //alert('getItem');
    var item = {
        'id': $('#' + str + '-id').val(),
        'firstName': $('#' + str + '-firstname').val(),
        'lastName': $('#' + str + '-lastname').val(),
        'gender': $('#' + str + '-gender').val(),
        'birth': $('#' + str + '-birth').val(),
        'email': $('#' + str + '-email').val(),
        'address': $('#' + str + '-address').val(),
        'phone': $('#' + str + '-phone').val(),
        'department': $('#' + str + '-department').val()
    };
    return item;
}
