const headerItem = [...document.querySelectorAll(".header__nav-item")];
const headerNavActive = document.querySelector(".header__nav-active");

headerItem.forEach((item) => {
    let tabIndex = 0;
    let path = window.location.pathname.split('/')[1];
    if (path === 'Home') {
        tabIndex = 0;
    } else if (path == "Test") {
        tabIndex = 1;
    } else {
        tabIndex = 2;
    }

    if (tabIndex < headerItem.length) {
        headerItem.forEach(item => item.classList.remove('header__nav-active'));
        if (headerItem[tabIndex]) {
            headerItem[tabIndex].classList.add("header__nav-active");
        } else {
            headerItem[0].classList.add("header__nav-active");
        }
    } else {
        headerItem.forEach(item => item.classList.remove('header__nav-active'));
    }
});

toggleNavList = () => {
    if ($('.header__nav-moblie').css('display') == 'block') {
        $(".header__nav-moblie").css('display', "none");
    } else {
        $(".header__nav-moblie").css('display', "block");
    }

}


