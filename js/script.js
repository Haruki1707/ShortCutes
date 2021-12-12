function onload(){
    var markdowntext;
    fetch('emulators.md')
    .then(response => response.text())
    .then(text => {
        markdowntext = text.toString();
        console.log(markdowntext);
        var converter = new showdown.Converter();
        converter.setOption('tables', true);
        document.getElementById("EmulatorsTable").innerHTML = converter.makeHtml(markdowntext);

        rows = document.getElementsByTagName("table")[0].rows;
        for (let i = 0; i < rows.length; i++) {
            cell = rows[i].cells[0];
            links = cell.getElementsByTagName('a');
            for(var j=0; j<links.length; j++)
            {
                links[j].target = "_blank";
                links[j].classList.add("text-white"); 
            }
        }
    });

    ss = document.getElementById("Screenshots");
    cols = ss.getElementsByClassName("col-lg-4");
    for (let i = 0; i < cols.length; i++) {
        rows = cols[i].getElementsByClassName("row");
        for (let j = 0; j < rows.length; j++) {
            rows[j].classList.add("justify-content-center"); 
        }
    }

    if (location.href.indexOf("#") != -1) {
        scrollToE(location.href.slice(location.href.indexOf("#") + 1));
    }
}

var element;
function scrollToE(element, navbartoggler = false){
    window.element = element;

    toggler = document.getElementById("navbar-toggler");
    if (window.getComputedStyle(toggler).display !== "none" && element != "Main" && navbartoggler == false) {
        document.getElementById("navbar-toggler").click();
    }

    window.scrollTo({
        top: findPosition(document.getElementById(window.element)),
        behavior: 'smooth'
    });

    setTimeout(scrollToE2, 700);
}

function scrollToE2(){
    window.scrollTo({
        top: findPosition(document.getElementById(window.element)) - 70,
        behavior: 'smooth'
    });
}

function findPosition(obj) {
    var currenttop = 0;
    if (obj.offsetParent) {
        do {
            currenttop += obj.offsetTop;
        } while ((obj = obj.offsetParent));
        return [currenttop];
    }
}