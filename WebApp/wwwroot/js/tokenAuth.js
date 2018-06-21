
var tokenKey = "accessToken";
$('#submitLogin').click(function (e) {
    e.preventDefault();
    var isChecked = $("input:checkbox").is(":checked") ? 1 : 0;
    var loginData = {
        grant_type: 'password',
        username: $('#emailLogin').val(),
        password: $('#passwordLogin').val(),
        rememberMe: isChecked
    };

    $.ajax({
        type: 'POST',
        url: '/token',
        data: loginData
    }).success(function (data) {

        // save sessionStorage token
        sessionStorage.setItem(tokenKey, data.access_token);
        console.log(data.access_token);
    }).fail(function (data) {
        console.log(data);
    });
});

$('#logOut').click(function (e) {
    e.preventDefault();
    sessionStorage.removeItem(tokenKey);
    location.replace("/Account/Login");
});

$('#CodeAPI').click(function (e) {
    e.preventDefault();
    $.ajax({
        type: 'GET',
        url: '/api/CodeAPI',
        beforeSend: function (xhr) {

            var token = sessionStorage.getItem(tokenKey);
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        },
        success: function (data) {
            alert(data);
        },
        fail: function (data) {
            console.log(data);
        }
    });
});
$('#getDataByRole').click(function (e) {
    e.preventDefault();
    $.ajax({
        type: 'GET',
        url: '/api/values/getrole',
        beforeSend: function (xhr) {

            var token = sessionStorage.getItem(tokenKey);
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        },
        success: function (data) {
            alert(data);
        },
        fail: function (data) {
            console.log(data);
        }
    });
});