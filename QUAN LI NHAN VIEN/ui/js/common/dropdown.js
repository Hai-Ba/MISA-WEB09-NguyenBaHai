export function dropdownEvent(){
    // let dropDown = $(".ms-dropdown");
    // let dropDownBox = $(".ms-dropdown__box");
    let ddBox = $(".ms-dropdown__box");
    ddBox.each(function(){
        var input = $(this).find("input");
        var btn = $(this).find("button");
        var list = $(this).children("div");

        btn.unbind('click');//To come across action happen twice
        btn.click(function(){
            if(list.css("display") == "none"){
                list.show();
            }
            else{
                list.hide();
            }
        });
    });
}



