$(document).ready(function SendToServer() {

    var permissions = localStorage.getItem("permissions");
    if (permissions != null) {
        var permissionList = JSON.stringify({ 'Permissions': permissions });

        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            url: '/Proxy/RenderPermissions',
            data: permissionList,
            success: function (msg) {
                console.info(msg);
            },
            failure: function (response) {
                console.error(response);
            }
        });
    }
});