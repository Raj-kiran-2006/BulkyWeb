$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/Product/GetAll' },
        "autoWidth": false,
        "language": {
            "emptyTable": "No products have been added yet."
        },
        "columns": [
            { data: 'title', "width": "23%", "className": "align-middle fw-semibold" },
            { data: 'isbn', "width": "17%", "className": "align-middle text-nowrap text-muted" },
            { data: 'price', "width": "11%", "className": "align-middle text-nowrap" },
            { data: 'author', "width": "17%", "className": "align-middle" },
            { data: 'category.name', "width": "14%", "className": "align-middle" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="d-flex justify-content-center align-items-center gap-2">
                        <a href="/Admin/Product/Upsert?id=${data}" class="btn btn-outline-primary btn-sm product-action-button">
                            <i class="bi bi-pencil-square" aria-hidden="true"></i> Edit
                        </a>
                        <form method="post" action="/Admin/Product/Delete" class="delete-product-form m-0">
                            <input type="hidden" name="Id" value="${data}">
                            <button type="submit" class="btn btn-outline-danger btn-sm product-action-button">
                                <i class="bi bi-trash-fill" aria-hidden="true"></i> Delete
                            </button>
                        </form>
                    </div>`
                },
                "width": "18%",
                "className": "text-center align-middle text-nowrap"
            }
        ]

    });
}

$(document).on('submit', '.delete-product-form', function (event) {
    event.preventDefault();

    const form = this;
    Swal.fire({
        title: 'Delete this product?',
        text: 'This action cannot be undone.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#dc3545',
        confirmButtonText: 'Yes, delete it',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed) {
            form.submit();
        }
    });
});