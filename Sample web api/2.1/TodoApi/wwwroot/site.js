const uri = 'api/employee/';
let todos = null;
/*
function getCount(data) {
    const el = $('#counter');
    let name = 'to-do';
    if (data) {
        if (data > 1) {
            name = 'to-dos';
        }
        el.text(data + ' ' + name);
    } else {
        el.html('No ' + name);
    }
}
*/
function ValidatePhone(str) {
    var phone = "";
    if (str == "add") phone = $('#add-phone').val();
    else phone = $('#edit-phone').val()
    if (!(/^1[34578]\d{9}$/.test(phone))) {
        alert("这个手机号有毒！");
    }
}

$(document).ready(function () {
    getData();
    $('#myTable').DataTable();
});

function getData() {
    $.ajax({
        type: 'GET',
        url: uri + 'GetAll',
        cache: false,
        success: function (data) {
            var table = $('#myTable').DataTable();
            $.each(data, function (key, item) {
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
                    '<td><button onclick="editItem(this, ' + item.id + ')">Edit</button></td>',
                    '<td><button onclick="deleteItem(this, ' + item.id + ')">Delete</button></td>'
                ]).draw();
            });
            todos = data;
        }
    });
}

function addItem() {
    //alert('addItem');
    const item = {
        'firstName': $('#add-firstname').val(),
        'lastName': $('#add-lastname').val(),
        'gender': $('#add-gender').val(),
        'birth': $('#add-birth').val(),
        'email': $('#add-email').val(),
        'address': $('#add-address').val(),
        'phone': $('#add-phone').val(),
        'department': $('#add-department').val()
    };


    $.ajax({
        type: 'POST',
        accepts: 'application/json',
        url: uri + 'Create',
        contentType: 'application/json',
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("add失败，请检查phone和email");
        },
        success: function (result) {
            var table = $('#myTable').DataTable();
            table.row.add([
                result.id,
                result.firstName,
                result.lastName,
                result.gender,
                result.birth,
                result.email,
                result.address,
                result.phone,
                result.department,
                '<td><button onclick="editItem(this, ' + item.id + ')">Edit</button></td>',
                '<td><button onclick="deleteItem(this, ' + item.id + ')">Delete</button></td>'
            ]).draw();
            window.location.reload();
        }
    });
}

function saveItem() {
    const item = {
        'id': $('#edit-id').val(),
        'firstName': $('#edit-firstname').val(),
        'lastName': $('#edit-lastname').val(),
        'gender': $('#edit-gender').val(),
        'birth': $('#edit-birth').val(),
        'email': $('#edit-email').val(),
        'address': $('#edit-address').val(),
        'phone': $('#edit-phone').val(),
        'department': $('#edit-department').val()
    };
    if (confirm("真的要修改吗")) {
        $.ajax({
            url: uri + '/Update/' + $('#edit-id').val(),
            type: 'PUT',
            accepts: 'application/json',
            contentType: 'application/json',
            data: JSON.stringify(item),
            async: false,
            success: function (result) {
                window.location.reload();
                getData();
            }
        });
    }

    closeInput();
    return false;
}

function deleteItem(obj, id) {
    if (confirm("真的要删除吗")) {
        $.ajax({
            url: uri + '/Delete/' + id,
            type: 'DELETE',
            success: function (result) {
                //alert("删了");
                var table = $('#myTable').DataTable();
                table.row($(obj).parents('tr')).remove().draw();
            }
        });
    }

}

function editItem(obj, id) {
    //alert("hahah");
    $.each(todos, function (key, item) {
        if (item.id === id) {
            $("#edit-id").val(item.id);
            $('#edit-firstname').val(item.firstName);
            $('#edit-lastname').val(item.lastName);
            $('#edit-gender').val(item.gender);
            $('#edit-birth').val(item.birth);
            $('#edit-email').val(item.email);
            $('#edit-address').val(item.address);
            $('#edit-phone').val(item.phone);
            $('#edit-department').val(item.department);
        }
    });
    $('#spoiler').css({ 'display': 'block' });
}

function closeInput() {
    $('#spoiler').css({ 'display': 'none' });
}
