const apiURL = "";

$.click("btnInsert", () => {
    $.ajax({
        type: "POST",
        url: apiURL,
        data: JSON.stringify({
            "make": $("inputMake").val(),
            "model": $("inputModel").val(),
            "year": $("inputYear").val(),
            "armored": $("inputArmored").val(),
            "VIN": $("inputVIN").val()
        }),
        success: (res) => {
            alert("SUCCESS: Insert was successful: " + res);
        },
        error: (res) => {
            alert("WARNING: An error occured: " + res);
        },
        contentType: "application/json",
        dataType: "json"
    });
});