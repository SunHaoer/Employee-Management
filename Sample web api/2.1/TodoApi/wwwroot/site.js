const uri = 'api/employee/';
let todos = null;

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

$(document).ready(function () {
    getData();
    $('#myTable').DataTable();
    $('.my-form').on('submit', function () {
        //alert("1111");
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
        if (!(/^1[34578]\d{9}$/.test($('#edit-phone').val()))) {
            alert("手机号码有误，请重填");
        } else {
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
            closeInput();
        }
        return false;
    });
});

function getData() {
    $.ajax({
        type: 'GET',
        url: uri + 'GetAll',
        cache: false,
        success: function (data) {
            //$('#myTable').empty();
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
                    '<td><button onclick="editItem(' + item.id + ')">Edit</button></td>',
                    '<td><button onclick="deleteItem(' + item.id + ')">Delete</button></td>'
                ]).draw();
            });
            todos = data;
        }
    });
}

function addItem() {
    alert('addItem');
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
    if (!(/^1[34578]\d{9}$/.test($('#add-phone').val()))) {
        alert("手机号码有误，请重填");
    } else {
        $.ajax({
            type: 'POST',
            accepts: 'application/json',
            url: uri + 'Create',
            contentType: 'application/json',
            data: JSON.stringify(item),
            error: function (jqXHR, textStatus, errorThrown) {
                alert("该邮箱已存在");
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
                    '<td><button onclick="editItem(' + item.id + ')">Edit</button></td>',
                    '<td><button onclick="deleteItem(' + item.id + ')">Delete</button></td>'
                ]).draw();
                //alert('success');
                window.location.reload();
                /*
                $('#add-firstname').val(),
                $('#add-lastname').val(),
                $('#add-gender').val(),
                $('#add-birth').val(),
                $('#add-email').val(),
                $('#add-department').val()
                */
            }
        });
    }

}

function deleteItem(id) {
    $.ajax({
        url: uri + '/Delete/' + id,
        type: 'DELETE',
        success: function (result) {
            var table = $('#myTable').DataTable();
            table.row($(result)).remove;
            alert("delete");
            window.location.reload();
            //getData();
            
        }
    });
}

function editItem(id) {
    alert("editItem");
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
