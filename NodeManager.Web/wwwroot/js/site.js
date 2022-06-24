$(document).ready(function () {
    $('.chosen-select').chosen({ width: "250px" });
});

var blockAllNodes = document.querySelectorAll('div.node'); // Получаем список все блоков документа
var countAllNodes = blockAllNodes.length;                  // Считаем количество блок
alert(countAllNodes);                                      // Выводим на экран результат
var arrNodes = Array.from(blockAllNodes);
 

//alert(typeof (arr));
//console.log(typeof (arr));
//alert(typeof (arr[0]));
//console.log(arr);
//alert(arr[0]);
//displayBlock();
//console.log(arr[0]);

var current_page = 1;
var records_per_page = 12;

function prevPage() {
    if (current_page > 1) {
        current_page--;
        changePage(current_page);
    }
}

function nextPage() {
    if (current_page < countAllPages()) {
        current_page++;
        changePage(current_page);
    }
}

function changePage(page) {
    var btn_next = document.getElementById("btn_next");
    var btn_prev = document.getElementById("btn_prev");
    var listing_table = document.getElementById("listingTable");
    var page_span = document.getElementById("page");

    // Validate page
    if (page < 1) page = 1;
    if (page > countAllPages()) page = countAllPages();

    listing_table.innerHTML = "";

    for (var i = (page - 1) * records_per_page; i < (page * records_per_page) && i < arrNodes.length; i++) {
        listing_table.innerHTML += arrNodes[i].outerHTML;
    }
    page_span.innerHTML = page + "/" + countAllPages();

    if (page == 1) {
        btn_prev.style.visibility = "hidden";
    }
    else {
        btn_prev.style.visibility = "visible";
    }

    if (page == countAllPages()) {
        btn_next.style.visibility = "hidden";
    }
    else {
        btn_next.style.visibility = "visible";
    }
}

function countAllPages() {
    return Math.ceil(arrNodes.length / records_per_page);
}

window.onload = function () {
    changePage(1);
};


//function displayBlock() {
    
//    for (var i = 0; i < 12; i++) {
//        arr[i]
//    }

//}

function changeCountItems(count) {
    //records_per_page = document.getElementById("countPageItem").value;
    records_per_page = count;
    console.log(records_per_page);
    countAllPages();
    console.log(countAllPages());
    changePage(1);
    console.log(changePage(1));
}

let form = document.querySelector('.subscription');
let enteredText = document.querySelector('.search_by_name');

form.onsubmit = function (evt) {
    evt.preventDefault();
    var inputText = enteredText.value;
    var strURL = "/Node/1/SearchName/";
    window.location.assign(strURL + inputText);
};

function redirectPage(numPage) {
    var urlPage = window.location.href;
    var decodedURLPage = decodeURI(urlPage);
    var arrURLPage = decodedURLPage.split('/');
    if (arrURLPage.length == 4) { //для главной страницы, лист 1
        arrURLPage[3] = "Node";
        arrURLPage[4] = "List";
        arrURLPage[5] = numPage;
    }
    else if (arrURLPage.length == 6 && arrURLPage[4] == "List") { //для остальных листов главной страницы
        arrURLPage[5] = numPage;
    }
    else if ((arrURLPage.length == 7 || arrURLPage.length == 8)  && arrURLPage[4] == "List") { //для страниц разделов и категорий
        arrURLPage[5] = numPage;
    }
    else if (arrURLPage.length == 7 && (arrURLPage[5] == "SearchName" || arrURLPage[5] == "Search")) { //для страницы поиска по названию или по тегу
        arrURLPage[4] = numPage;
    }
    else if (arrURLPage.length == 7 &&  arrURLPage[5] == "ProjectSection") { //для страницы проекта
        arrURLPage[4] = numPage;
    }
    var newURLPage = arrURLPage.join('/');
    var encodedURLPage = encodeURI(newURLPage);
    window.location.assign(encodedURLPage);
}