/**DIALOG POP UP JS */
document.getElementById("pop-up-dialog").addEventListener("click",function(){
    document.querySelector(".ms-dialog").style.display = "block";
});

document.querySelector(".ms-dialog-wrapper__header__exit").addEventListener("click",function(){
    document.querySelector(".ms-dialog").style.display = "none";
});

/**FORM FOCUS */
document.querySelector(".ms-dialog-wrapper").addEventListener("click", function(){
    document.getElementById("tab-focus").focus();
});

/**SIDEBAR MINIMIZATION */
const sidebarMinimal  = document.getElementById("sidebar-minimal");
const sidebarText = document.querySelectorAll(".item-list__item__text");
sidebarMinimal.addEventListener("click",function(){
    if(sidebarMinimal.getAttribute("Action") == "maximal"){
        sidebarMinimal.setAttribute("title","Phóng to");
        sidebarMinimal.setAttribute("Action","minimal");
        sidebarText.forEach(x => {
            x.style.display = "none";
            x.parentElement.parentElement.style.width = "80px";
            document.querySelector(".body").style.width = `calc(100% - ${x.parentElement.parentElement.style.width})`
        });
        document.getElementById("minimal-sidebar").setAttribute("style","display: none;");
        document.getElementById("maximal-sidebar").setAttribute("style","display: block;");
        // alert(document.querySelector(".item-list").style.width);
        return;
    }
    if(sidebarMinimal.getAttribute("Action") == "minimal"){
        sidebarMinimal.setAttribute("title","Thu nhỏ");
        sidebarMinimal.setAttribute("Action","maximal");
        // sidebarText.style.display = "block";
        // alert("minimal");
        sidebarText.forEach(x => {
            // console.log(x);
            x.style.display = "block";
            x.parentElement.parentElement.style.width = "200px";
            document.querySelector(".body").style.width = `calc(100% - ${x.parentElement.parentElement.style.width})`
        });
        document.getElementById("minimal-sidebar").setAttribute("style","display: block;");
        document.getElementById("maximal-sidebar").setAttribute("style","display: none;");
        return;
    }
});

/**TABLE CHECKBOX CHECK */