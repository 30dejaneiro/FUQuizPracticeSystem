const listQuestion = $("#listQuestion");
const practiceQuestion = [...$(".practice__ques")];
const countDown = $("#countDown");
const lastButton = [...$(".test__ques_action")];
const numberTest = $("#numberTest");
const viewResut = $("#viewResut");
let lastNumber = 0;
let totalQuestion = $(".practice__ques").last()[0].classList[1].slice(4);

var result = [];

let element = [];

for (let index = 0; index < totalQuestion; index++) {
    element.push(`<div id="chooseQues${index + 1}" class="test__question-wrapper">
                        <button class="test__question-button" onclick="changeQuestion(${index + 1})">${index + 1}</button></div>`);
}

if (totalQuestion > 70) {
    listQuestion.css("height", "77vh");
}
if (viewResut.length == 0) {
    lastButton[lastButton.length - 1].style.display = "none"
} else {
    viewResut.css("display", "flex");
}

listQuestion.html(element.join``);

$(`#chooseQues1`).addClass("test__question--active");

function changeQuestion(number) {
    $('.test__question-wrapper').removeClass('test__question--active');
    $('.practice__ques').css("display", "none");
    $(`#chooseQues${number}`).addClass("test__question--active");
    let list = [...$(`.ques${number}`)];

    if (list.length == 0) {
        let i = 1;
        while (i < 4) {
            if ([...$(`.ques${number - i}`)].length != 0) {
                let list1 = [...$(`.ques${number - i}`)];
                list1[list1.length - 1].style.display = "flex";
                break;
            }
            i++;
        }
    } else {
        list[list.length - 1].style.display = "flex";
    }
}


function nextTestQues(number) {
    changeQuestion(number++);
}

function showResult(answer) {
    const answerBlock = $(".ques" + (answer - 1) + " #answerBlock");
    answerBlock.css("display", "block");
}


function handleChange(src, QuestionId) {
    var data = localStorage.getItem("data");
    const questions = [...$(".test__question-wrapper")];

    if (data != null) {
        let newObj = JSON.parse(data);
        let findItem = newObj.findIndex(item => item.QuestionId === QuestionId);
        if (findItem == -1) {
            newObj.push({ QuestionId, UserAnswer: src.value })
        } else {
            newObj[findItem].UserAnswer = src.value;
        }
        console.log(questions[QuestionId - 1])
        questions[QuestionId - 1].classList.add("test__question--choose")
        localStorage.setItem("data", JSON.stringify(newObj));
    } else {
        let arr = [];
        arr.push({ QuestionId, UserAnswer: src.value })
        localStorage.setItem("data", JSON.stringify(arr));
    }
}

function submitTest(code, timeOut) {
    let score = 0;
    let answer = false;
    var data = JSON.parse(localStorage.getItem("data"));
    if (timeOut == false) {
        answer = confirm("Do you really want to submit?");
    }
    if (answer == true || timeOut == true) {
        result.forEach((item) => {
            if (item.UserAnswer === item.answer) {
                score += (100 / totalQuestion);
            }
        })
        $.ajax({
            url: "/Test/CheckQuestion",
            dataType: "json",
            type: "POST",
            data: { score: score, code: code, data, totalQuestion: totalQuestion },
            success: function (res) {
                if (res.mess === true) {
                    window.location.href = "/Test/Index";
                    alert((timeOut == true ? "You're run out of time" : "You're done") + "! You can go to profile to see your score");
                } else {
                    window.location.href = "/NotFound/Index";
                }
                localStorage.clear();
            }
        })
    }
}

if (countDown.length > 0) {
    let timer2 = countDown.html();
    let interval = setInterval(() => {
        var timer = timer2.split(':');
        var hour = parseInt(timer[0], 10);
        var minutes = parseInt(timer[1], 10);
        var seconds = parseInt(timer[2], 10);
        --seconds;
        minutes = (seconds < 0) ? --minutes : minutes;
        hour = (minutes < 0) ? --hour : hour;
        if (seconds == 0 && minutes == 0 && hour == 0) {
            clearInterval(interval);
            let code = window.location.search;
            submitTest(code.slice(6), true);
        };
        seconds = (seconds < 0) ? 59 : seconds;
        seconds = (seconds < 10) ? '0' + seconds : seconds;
        minutes = (minutes < 0) ? 59 : minutes;
        minutes = (minutes < 10) ? '0' + minutes : minutes;
        countDown.html(hour + ':' + minutes + ':' + seconds);
        timer2 = hour + ':' + minutes + ':' + seconds;
    }, 1000);
}