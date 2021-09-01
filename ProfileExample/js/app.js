$('#btnTest').click(function() {
    const decTaxRate = .0925;

    let decHours = $('#txtHours').val();
    let decRate = $('#txtPayRate').val();

    console.log(decHours * decRate);
}); 