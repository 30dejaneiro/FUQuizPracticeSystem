$("#toastify").delay(1500).slideUp(500);

function checkDelete(id, text) {
    switch (text) {
        case "student": deleteItem(id, text, "DeleteStudent", "Index"); break;
        case "subject": deleteItem(id, text, "DeleteSubject", "SubjectList"); break;
    }
}

function deleteItem(id, text, url, goBack) {
    let answer = confirm(`Do you want to delete ${text} ${id}`)
    if (answer === true) {
        $.ajax({
            url: "/Admin/" + url,
            dataType: "json",
            type: "POST",
            data: { id: id },
            success: function (res) {
                if (res.mess === true) {
                    document.getElementById(text + id).style.display = "none";
                }
                window.location.href = "/Admin/" + goBack;
            }
        })
    }
}

