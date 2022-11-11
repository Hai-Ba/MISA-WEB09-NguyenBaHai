/**
 * EVENT CLOSE DIALOG POPUP
 * Ngyuen Ba Hai
 * 25/10/2022
 */
function dialogPopupClose(){
    $(".close-form").each(function(){
        $(this).click(function(){
            $(this).parent().parent().hide();
        });
    });
}

/**
 * EVENT SHOW DIALOG IS EXIST
 * Nguyen Ba Hai
 * 25/10/2022
 * @param {} text
 *  
 */
function showWarningExist(text){
    $("#existance-code").text(text);
    $("#employee-code-exist").css("display","flex");
    dialogPopupClose();
}

/**
 * EVENT SHOW LOST INFO FIELD
 * Nguyen Ba Hai
 * 25/10/2022
 * @param {*} text 
 */
function showErrorLostInfo(text){
    $("#error-info-text").text(text);
    $("#must-fullfill").css("display","flex");
    dialogPopupClose();
}

/**
 * EVENT SHOW DIALOG INFORM
 * Nguyen Ba Hai
 * 26/10/2022
 * @param {*} text 
 */
 function showInformDialog(){
    // $("#error-info-text").text(text);
    $("#change-data-inform").css("display","flex");
    // dialogPopupClose();
}