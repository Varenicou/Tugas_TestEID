$(document).ready(() => {
    load_inventaris();
});

// Modal Definition
const md_upd_inventaris = new bootstrap.Modal(document.getElementById("md_upd_inventaris"), {
    backdrop: "static",
    focus: true
});

const load_inventaris = () => {
    var token = localStorage.getItem("api_token");
    $.ajax({
        method: "GET",
        dataType: "json",
        url: "/api/inventaris/get_inventaris",
        headers: {
            "Authorization": "Bearer " + token,
            "Content-Type":"application/json"
        },
        beforeSend: () => {
            Swal.fire({
                icon: "info",
                text: "Please wait... Fetching data...",
                showConfirmButton: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });
        },
        success: (data) => {
            if(data.status == "success") {
                var html_inventaris = "";
                if(data.data.length > 0) {
                    for(var i=0; i<data.data.length; i++) {
                        // Constant
                        var res = data.data[i];

                        html_inventaris += `
                            <tr>
                                <td>${(i+1)}</td>
                                <td>${res.item_id}</td>
                                <td>${res.item_name}</td>
                                <td>${res.item_desc ?? "-"}</td>
                        `;

                        if(res.item_img != null) {
                            html_inventaris += `
                                <td>
                                    <img src="${res.item_img}" style="width: 25%;">
                                </td>
                            `;
                        } else {
                            html_inventaris += `<td>-</td>`;
                        }

                        html_inventaris += `
                                <td>${res.item_stock_current}</td>
                                <td>
                                    <button type="button" class="btn btn-primary" onclick="upd_inventaris('${btoa(res.item_id)}')>
                                        <i class='bi bi-pencil'></i>
                                    </button>
                                    <button type="button" class="btn btn-danger" onclick="del_inventaris('${btoa(res.item_id)}')">
                                        <i class="bi bi-trash"></i>
                                    </button>
                                </td>
                            </tr>
                        `;
                    }

                    $("#dt_inventaris").html(html_inventaris);
                    Swal.close();
                } else {
                    Swal.close();
                }
            } else {
                Swal.fire({
                    title: "Error",
                    icon: "error",
                    text: data.msg
                });
            }
        },
        error: (xhr) => {
            if(xhr.status == 401) {
                Swal.fire({
                    title: "Error",
                    icon: "error",
                    text: "Sesi telah berakhir, silakan login kembali."
                });
            } else {
                Swal.fire({
                    title: "Error",
                    icon: "error",
                    text: "Ada Kesalahan pada saat menarik Data Inventaris."
                });
            }
        }
    });
}

const upd_inventaris = (item_id_64) => {
    var token = localStorage.getItem("api_token");
    $.ajax({
        method: "POST",
        dataType: "json",
        data: {item_id:atob(item_id_64)},
        url: "/api/inventaris/get_inventaris",
        headers: {
            "Authorization": "Bearer " + token,
            "Content-Type": "application/json"
        },
        beforeSend: () => {
            Swal.fire({
                icon: "info",
                text: "Memuat Data...",
                didOpen: () => {
                    Swal.showLoading();
                },
                showConfirmButton: false,
            });
        },
        success: (res) => {
            if(res.status == "success") {
                $("#frm_item_id").val(res.data.item_id);
                $("#frm_item_name").val(res.data.item_name);
                $("#frm_item_desc").val(res.data.item_desc ?? "");
                $("#frm_item_img").val(res.data.item_img ?? "");
                $("#frm_item_stock").val(res.data.item_stock);
                Swal.close();
                $("#btn_submit_inventaris").attr("onclick", "submit_upd_inventaris()");
                md_upd_inventaris.show();
            } else {
                Swal.fire({
                    title: "Error",
                    icon: res.status,
                    text: res.msg
                });
            }
        },
        error: (xhr) => {
            if(xhr.status == 401) {
                Swal.fire({
                    title: "Error",
                    icon: "error",
                    text: "Sesi telah berakhir, silakan login kembali."
                });
            } else if(xhr.status == 404) {
                Swal.fire({
                    title: "Error",
                    icon: "error",
                    text: "Data Tidak Ditemukan!"
                });
            } else {
                Swal.fire({
                    title: "Error",
                    icon: "error",
                    text: "Ada Kesalahan pada saat menarik Data Inventaris."
                });
            }
        }
    });
}

const submit_upd_inventaris = () => {
    var formData = new FormData();
    var item_id = $("#frm_item_id").val();
    var item_name = $("#frm_item_name").val();
    var item_desc = $("#frm_item_desc").val();
    var item_img = $("#frm_item_img")[0].files[0];
    var item_stock = $("#frm_item_stock").val();

    if(!item_name || !item_id || !item_img || !item_stock) {
        Swal.fire({
            title: "Error",
            icon: "error",
            text: "Terdapat Input yang kosong, Silakan lengkapi dan coba lagi."
        });
    } else {
        // FormData
        formData.append("item_id", item_id);
        formData.append("item_name", item_name);
        formData.append("item_desc", item_desc);
        formData.append("item_img", item_img);
        formData.append("item_stock", item_stock);

        // Session
        var token = localStorage.getItem("api_token");

        $.ajax({
            method: "POST",
            dataType: "json",
            data: formData,
            url: "/api/inventaris/upd_inventaris",
            processData: false,
            contentType: false,
            headers: {
                "Authorization": "Bearer " + token
            },
            beforeSend: () => {
                Swal.fire({
                    icon: "info",
                    text: "Memproses Data... Mohon tunggu...",
                    showConfirmButton: false,
                    didOpen: () => {
                        Swal.showLoading();
                    }
                });
            },
            success: (res) => {
                if(res.status == "success") {
                    Swal.fire({
                        title: "Success",
                        icon: res.status,
                        text: res.msg,
                        timer: 1500,
                        timerProgressBar: true,
                        allowOutsideClick: false,
                        showConfirmButton: false,
                        willClose: () => {
                            load_inventaris();
                        }
                    });
                } else {
                    Swal.fire({
                        title: "Error",
                        icon: res.status,
                        text: res.msg
                    });
                }
            },
            error: (xhr) => {
                if(xhr.status == 401) {
                    Swal.fire({
                        title: "Error",
                        icon: "error",
                        text: "Sesi telah berakhir, silakan login kembali."
                    });
                } else {
                    Swal.fire({
                        title: "Error",
                        icon: "error",
                        text: "Ada Kesalahan pada saat Pengkinian Data Inventaris."
                    });
                }
            }
        })
    }
}

const submit_add_inventaris = () => {
    var formData = new FormData();
    var item_id = $("#frm_item_id").val();
    var item_name = $("#frm_item_name").val();
    var item_desc = $("#frm_item_desc").val();
    var item_img = $("#frm_item_img")[0].files[0];
    var item_stock = $("#frm_item_stock").val();

    if(!item_name || !item_img || !item_stock) {
        Swal.fire({
            title: "Error",
            icon: "error",
            text: "Terdapat Input yang kosong, Silakan lengkapi dan coba lagi."
        });
    } else {
        // FormData
        formData.append("item_id", item_id);
        formData.append("item_name", item_name);
        formData.append("item_desc", item_desc);
        formData.append("item_img", item_img);
        formData.append("item_stock", item_stock);

        // Session
        var token = localStorage.getItem("api_token");

        $.ajax({
            method: "POST",
            dataType: "json",
            data: formData,
            url: "/api/inventaris/add_inventaris",
            processData: false,
            contentType: false,
            headers: {
                "Authorization": "Bearer " + token
            },
            beforeSend: () => {
                Swal.fire({
                    icon: "info",
                    text: "Memproses Data... Mohon tunggu...",
                    showConfirmButton: false,
                    didOpen: () => {
                        Swal.showLoading();
                    }
                });
            },
            success: (res) => {
                if(res.status == "success") {
                    Swal.fire({
                        title: "Success",
                        icon: res.status,
                        text: res.msg,
                        timer: 1500,
                        timerProgressBar: true,
                        allowOutsideClick: false,
                        showConfirmButton: false,
                        willClose: () => {
                            load_inventaris();
                        }
                    });
                } else {
                    Swal.fire({
                        title: "Error",
                        icon: res.status,
                        text: res.msg
                    });
                }
            },
            error: (xhr) => {
                if(xhr.status == 401) {
                    Swal.fire({
                        title: "Error",
                        icon: "error",
                        text: "Sesi telah berakhir, silakan login kembali."
                    });
                } else {
                    Swal.fire({
                        title: "Error",
                        icon: "error",
                        text: "Ada Kesalahan pada saat Pengkinian Data Inventaris."
                    });
                }
            }
        });
    }
}

const add_inventaris = () => {
    $("#btn_submit_inventaris").attr("onclick", "submit_add_inventaris()");
    md_upd_inventaris.show();
}

const del_inventaris = (item_id_64) => {
    Swal.fire({
        title: "Konfirmasi",
        icon: "warning",
        text: "Apakah anda yakin akan menghapus data ini?",
        showConfirmButton: true,
        showDenyButton: true,
        allowOutsideClick: false,
    }).then((a) => {
        if(a.isConfirmed) {
            var token = localStorage.getItem("api_token");
            $.ajax({
                method: "DELETE",
                dataType: "json",
                data: {
                    item_id:atob(item_id_64)
                },
                url: "/api/inventaris/del_inventaris",
                headers: {
                    "Authorization": "Bearer " + token,
                    "Conent-Type": "application/json"
                },
                beforeSend: () => {
                    Swal.fire({
                        icon: "info",
                        text: "Mohon Tunggu... Menghapus data...",
                        allowOutsideClick: false,
                        showConfirmButton: false,
                        didOpen: () => {
                            Swal.showLoading();
                        }
                    });
                },
                success: (res) => {
                    if(res.status == "success") {
                        Swal.fire({
                            title: "Success",
                            icon: "success",
                            text: res.msg,
                            timer: 1500,
                            timerProgressBar: true,
                            showConfirmButton: false,
                            willClose: () => {
                                load_inventaris();
                            }
                        });
                    } else {
                        Swal.fire({
                            title: "Error",
                            icon: res.status,
                            text: res.msg
                        });
                    }
                },
                error: (xhr) => {
                    if(xhr.status == 401) {
                        Swal.fire({
                            title: "Error",
                            icon: "error",
                            text: "Sesi telah berakhir, silakan login kembali."
                        });
                    } else {
                        Swal.fire({
                            title: "Error",
                            icon: "error",
                            text: "Ada Kesalahan pada saat Pengkinian Data Inventaris."
                        });
                    }
                }
            });
        }
    });
}