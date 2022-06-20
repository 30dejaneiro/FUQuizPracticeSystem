const listQues = [...$(".practice__ques")];
const totalQues = [...$(".practice__answer-list")];
const afterCheck = [...$(".practice__answer-action")];
const beforeCheck = [...$(".practice__ques_action")];
const showScripts = [...$(".practice__answer-content")];
var showScriptBtn = $("#showScript");

let i = 0;
let wrongQues = [];
var userAnswerList = [];
listQues.forEach(item => {
    item.style.display = "none";
})

listQues[i].style.display = "flex";
toggleActionButton(false);

function handleChange(src) {
    let data = $(`#${src.parentNode.id}`).attr("data-image").split("&");
    let imgUrl = data[0].length != 0 ? `/Assets/Images/question/Part${data[2]}/Level${data[3]}/${data[0]}` : null;
    let soundURl = data[1].length != 0 ? `/Assets/Sound/Part${data[2]}/Level${data[3]}/${data[1]}` : null;
    let arr = src.parentNode.id.slice(4, -1);
    let checkId = userAnswerList.findIndex(ele => ele.id === arr);
    if (checkId != -1) {
        userAnswerList[checkId].userAns = src.value;
    } else {
        userAnswerList.push({ name: src.parentNode.id, userAns: src.value, img: imgUrl, sound: soundURl, id: arr });
    }
}

function nextQues() {
    listQues[i].style.display = "none";
    listQues[++i].style.display = "flex";

    toggleActionButton(false);

    if (i === listQues.length - 1) {
        const wrongQuesResult = $("#wrongQuesResult");
        if (showScriptBtn != null) {
            showScriptBtn.css('display', "none");
        }
        $("#result").html(`You completed the practice with ${totalQues.length - wrongQues.length}/${totalQues.length} correct questions.`);
        const element = wrongQues.map(item => `<tr>
                                <td>${item.img != null ? `<img src="${item.img}" alt="" class= "practice__content-img" >` : ''}</td>
                                <td>${item.sound != null ? `<audio class="sounds" controls src="${item.sound}"></audio>` : ''}</td>
                                <td><span class="practice__result-correct-text">Correct Answer: <span style="color : green">${item.answer}</span></span></td>
                                <td><span class="practice__result-correct-text">Your answer: <span style="color : red">${item.userAns}</span></span></td>
                            </tr>`);
        wrongQuesResult.html(element.join``);
        $("#resultContainer").css('display', "flex");
    }
}

function checkAnswer(id) {
    let count = 0;
    if (userAnswerList.length == 0) {
        var answer = confirm("You are not choose answers! Do you really want to check question?");
        if (answer == true) {
            toggleActionButton(true);
            return;
        }
    } else {
        userAnswerList.forEach(item => {
            $.ajax({
                url: "/Practice/CheckQuestion",
                dataType: "json",
                type: "POST",
                data: { id: item.id, userAnswer: item.userAns },
                success: function (res) {
                    if (res.mess) {
                        document.getElementById(`ques${item.id + item.userAns}`).classList.add("success-radio");
                    } else {
                        document.getElementById(`ques${item.id + item.userAns}`).classList.add("wrong-radio");
                        wrongQues.push({ ...item, answer: res.correctAnswer});
                    }
                    document.getElementById(`ques${item.id + res.correctAnswer}`).classList.add("success-radio");
                }
            })
        })
        userAnswerList = [];
        toggleActionButton(true);
    }
};

function toggleActionButton(flag) {
    afterCheck.forEach(item => item.style.display = flag === true ? "flex" : "none");
    beforeCheck.forEach(item => item.style.display = flag === true ? "none" : "flex");
}

function reLearn() {
    window.setTimeout(() => {
        window.location.reload(true);
    }, 200);
}