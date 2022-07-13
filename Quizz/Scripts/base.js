$("#toastify").delay(1500).slideUp(500);

function checkDelete(id, text) {
    switch (text) {
        case "student": deleteItem(id, text, "/Admin/DeleteStudent", "/Admin/Index"); break;
        case "question": deleteItem(id, text, "/Admin/DeleteQuestion", "/Admin/QuestionList"); break;
        case "subject": deleteItem(id, text, "/Admin/DeleteSubject", "/Admin/SubjectList"); break;
        case "bank": deleteItem(id, text, "/Admin/DeleteBank", "/Admin/BankList"); break;
        case "test": deleteItem(id, text, "/Admin/DeleteTest", "/Admin/TestList"); break;
        case "examQues": deleteItem(id, text, "/Admin/DeleteExamQuestion", "/Admin/ExamQuestions"); break;
    }
}

function checkDeleteUser(id, text, acc) {
    switch (text) {
        case "mybank": deleteItem1(id, text, "/Profile/DeleteBank", "/Profile/MyBank", acc); break;
        case "subject": deleteItem1(id, text, "/Profile/DeleteSubject", "/Profile/Subjects", acc); break;
        case "question": deleteItem1(id, text, "/Profile/DeleteQuestion", "/Profile/Questions", acc); break;
    }
}

function deleteItem1(id, text, url, goBack, acc) {
    let answer = confirm(`Do you want to delete ${text} ${id}`)
    if (answer === true) {
        $.ajax({
            url: url,
            dataType: "json",
            type: "POST",
            data: { id: id },
            success: function (res) {
                if (res.mess === true) {
                    document.getElementById(text + id).style.display = "none";
                }
                window.location.href = goBack + '/' + acc;
            }
        })
    }
}

function deleteItem(id, text, url, goBack) {
    let answer = confirm(`Do you want to delete ${text} ${id}`)
    if (answer === true) {
        $.ajax({
            url: url,
            dataType: "json",
            type: "POST",
            data: { id: id },
            success: function (res) {
                if (res.mess === true) {
                    document.getElementById(text + id).style.display = "none";
                }
                window.location.href = goBack;
            }
        })
    }
}

