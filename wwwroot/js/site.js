//
document.addEventListener('DOMContentLoaded', function () {
    const searchForm = document.querySelector('form.d-flex');
    const searchInput = searchForm.querySelector('input[name="searchTerm"]');

    searchForm.addEventListener('submit', function (e) {
        if (searchInput.value.trim() === "") {
            e.preventDefault();
            alert("Por favor ingrese un término de búsqueda.");
        }
    });
});


/*  mostrar/ocultar el contenedor de filtros*/
document.addEventListener('DOMContentLoaded', function () {
    const applyFiltersBtn = document.getElementById('applyFiltersBtn');

    applyFiltersBtn.addEventListener('click', function () {
        const genreId = document.getElementById('Genres').value;
        const producerId = document.getElementById('Producers').value;

        // Redirigir a la ruta con los parámetros de filtro
        const url = `/Home/FilteredSearch?genreId=${genreId}&producerId=${producerId}`;
        window.location.href = url;
    });
});
