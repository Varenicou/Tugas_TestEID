$(document).ready(() => {

});

const handshake_login = () => {
    var username = $("#username").val();
    var password = $("#password").val();
    if(!username || !password) {
        Swal.fire({
            title: "Error",
            icon: "error",
            text: "Username dan Password tidak boleh kosong!"
        });
    } else {
        $.ajax({
            method: "POST",
            dataType: "json",
            data: JSON.stringify({
                username:username,
                password:password
            }),
            headers: {
                "Content-Type": "application/json"
            },
            url: "/api/api_login/handshake",
            beforeSend: () => {
                Swal.fire({
                    icon: "info",
                    text: "Verifying login... Please wait..."
                });
            },
            success: (data) => {
                if (data.status == "success") {
                    localStorage.setItem("api_token", data.token);
                    Swal.fire({
                        title: "Success",
                        icon: "success",
                        text: data.msg,
                        timer: 1500,
                        timerProgressBar: true,
                        willClose: () => {
                            window.location.assign("/homepage");
                        }
                    });
                } else {
                    Swal.fire({
                        title: "Error",
                        icon: "error",
                        text: data.msg
                    });
                }
            },
            error: (xhr) => {
                if(xhr.status == 400) {
                    var ret = JSON.parse(xhr.responseText);
                    Swal.fire({
                        title: "Error",
                        icon: ret.status,
                        text: ret.msg
                    });
                } else {
                    Swal.fire({
                        title: "Error",
                        icon: "error",
                        text: "Ada kesalahan pada saat melakukan Handshake dengan server."
                    });
                }
            }
        });
    }
}

const handshake_reg = () => {
    var username = $("#username").val();
    var password = $("#password").val();
    if(!username || !password) {
        Swal.fire({
            title: "Error",
            icon: "error",
            text: "Username dan Password tidak boleh kosong!"
        });
    } else {
        $.ajax({
            method: "POST",
            dataType: "json",
            data: JSON.stringify({
                username:username,
                password:password
            }),
            headers: {
                "Content-Type": "application/json"
            },
            url: "/api/api_login/register",
            beforeSend: () => {
                Swal.fire({
                    icon: "info",
                    text: "Registering Account... Please wait..."
                });
            },
            success: (data) => {
                if(data.status == "success") {
                    Swal.fire({
                        title: "Success",
                        icon: "success",
                        text: data.msg
                    });
                } else {
                    Swal.fire({
                        title: "Error",
                        icon: data.status,
                        text: data.msg
                    });
                }
            },
            error: (xhr) => {
                if(xhr.status == 400) {
                    var ret = JSON.parse(xhr.responseText);
                    Swal.fire({
                        title: "Error",
                        icon: ret.status,
                        text: ret.msg
                    });
                } else {
                    Swal.fire({
                        title: "Error",
                        icon: "error",
                        text: "Ada kesalahan pada saat melakukan Handshake dengan server."
                    });
                }
            }
        });
    }
}