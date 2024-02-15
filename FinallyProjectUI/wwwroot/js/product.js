function Delete(url) {
    swal({
        title: "Öğeyi silmek istiyorsanız Tamam'a tıklayın",
        text: "Tamam'a tıklarsanız kalıcı olarak silinecektir",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(reloadPage(),3000);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    });
}
function reloadPage() {
    window.location.reload();
}