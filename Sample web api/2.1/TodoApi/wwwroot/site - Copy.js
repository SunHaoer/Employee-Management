const uri = 'api/employee/';
//let todos = null;
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
            table.clear();
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
            
            //todos = data;
        }
    });
}

function addItem() {
    //alert('addItem');
    /*
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
    */
    var item = getItem('add');
    /*
    if (ValidateEmail('add') && ValidatePhone('add')) {
        alert('可以添加');
    } else {
        alert('不可以添加');
    }
    */
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
            //window.location.reload();
            getData();
        }
    });
}

function saveItem() {
    /*
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
    */
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
                //window.location.reload();
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
    var table = $('#myTable').DataTable();
    var rowIndex = table.cell($(obj).parent()).index().row;
    //alert(rowIndex + " " + id);
    var parametes = new Array('id', 'firstname', 'lastname', 'gender', 'birth', 'email', 'address', 'phone', 'department');
    for (var i = 0; i < parametes.length; i++) {
        $('#edit-' + parametes[i]).val(table.data()[rowIndex][i]);
    }
    /*
    $('#edit-id').val(table.data()[rowIndex][0]);
    $('#edit-firstname').val(table.data()[rowIndex][1]);
    $('#edit-lastname').val(table.data()[rowIndex][2]);
    $('#edit-gender').val(table.data()[rowIndex][3]);
    $('#edit-birth').val(table.data()[rowIndex][4]);
    $('#edit-email').val(table.data()[rowIndex][5]);
    $('#edit-address').val(table.data()[rowIndex][6]);
    $('#edit-phone').val(table.data()[rowIndex][7]);
    $('#edit-department').val(table.data()[rowIndex][8]);
    */
    /*
    var tr = $(obj).parents('tr');
    $('#edit-id').val(tr.find('td:nth(0)').text());
    $('#edit-firstname').val(tr.find('td:nth(1)').text());
    $('#edit-lastname').val(tr.find('td:nth(2)').text());
    $('#edit-gender').val(tr.find('td:nth(3)').text());
    $('#edit-birth').val(tr.find('td:nth(4)').text());
    $('#edit-email').val(tr.find('td:nth(5)').text());
    $('#edit-address').val(tr.find('td:nth(6)').text());
    $('#edit-phone').val(tr.find('td:nth(7)').text());
    $('#edit-department').val(tr.find('td:nth(8)').text());
    */
    /*
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
    */
    $('#spoiler').css({ 'display': 'block' });
}

function closeInput() {
    $('#spoiler').css({ 'display': 'none' });
}

function ValidatePhone(str) {
    var phone = '';
    if (str == 'add') phone = $('#add-phone').val();
    else phone = $('#edit-phone').val();
    if (!(/^1[34578]\d{9}$/.test(phone))) {
        alert("这个手机号有毒！");
        return false;
    }
}

function ValidateEmail(str) {
    var email = '';
    if (str == 'add') email = $('#add-email').val();
    else email = $('#edit-email').val();
    if (!(/^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/.test(email))) {
        alert("这个Email号有毒！");
        return false;
    }
}

function getItem(str) {
    alert('getItem');
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
