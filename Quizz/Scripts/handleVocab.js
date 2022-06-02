const starBtn = [...document.querySelectorAll(".star--normal")];
const starActive = [...document.querySelectorAll(".star--active")];

const textSpeech = [...document.querySelectorAll(".text__speech")];
const volumnBtn = [...document.querySelectorAll(".volumn__btn")];
starBtn.forEach((item, index) => {
    item.addEventListener("click", () => {
        starBtn[index].style.display = 'none';
        starActive[index].style.display = 'block';
    })
});

starActive.forEach((item, index) => {
    item.addEventListener("click", () => {
        starBtn[index].style.display = 'block';
        starActive[index].style.display = 'none';
    })
});

volumnBtn.forEach((item, index) => {
    item.addEventListener("click", () => {
        responsiveVoice.speak(textSpeech[index].innerHTML);
    })
});