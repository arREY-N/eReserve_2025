function SetDefaultTimeAndDate() {
    const now = new Date();
    const requestDateTime = document.getElementById("requestDateTime");

    const formattedDateTime = now.toISOString().slice(0, 16);

    requestDateTime.value = formattedDateTime;
}

document.addEventListener("DOMContentLoaded", function () {
    SetDefaultTimeAndDate();  
});

function ValidateForm(form) {
    let fields = [];

    switch (form.id) {
        case "User":
            fields = ["Username", "Password", "FirstName", "MiddleName", "LastName", "DesignationID", "UserNumber", "UserEmail"];
            break;
        case "Facility":
            fields = ["FacilityName", "Floor", "Number", "Status"];
            break;
        case "Reservation":
            fields = ["ReserveDateTime", "RequestDateTime", "FacilityID", "Purpose"];
            break;
    }

    let valid = true;

    for (let i = 0; i < fields.length; i++) {
        const field = fields[i];
        const input = document.getElementById(field);

        if (!input.value || !input.value.trim()) {
            input.classList.add("is-invalid");
            input.focus();
            valid = false;
        } else {
            input.classList.remove("is-invalid");
        }
    }
    return valid;
}




