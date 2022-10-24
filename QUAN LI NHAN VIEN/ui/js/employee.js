
import { dropdownEvent } from "./common/dropdown.js"; //Add event for dropdown
import { formatDate, formatGender, formatMoney } from "./common/format.js";


// import {formatDate, formatGender, formatMoney} from "format"
class EmployeePage{
    constructor() {
        let me = this;
        $( window ).on( "load", me.initEvent)
        me.fetchData();
    }
    /**
     * 21/10/2022
     * Nguyen Ba Hai
     * Function to initiate the event(click, minimize,....)
     * Using Jquery
     */
    initEvent(){
        let me = this;
        //Dialog add employee pop-up, focus
        popUpForm();
        //Event close form popup
        closeForm();
        //Sidebar Minimization
        sidebarMinimized();
    }
    fetchData(){
        let me = this;
        //fetch API here
        $.get(api, function(data, status){
            pagingFunction(data.length, data, table);
            // dropdownEvent();
        });
    }
}


/**
 * FUNCTION TO MINIMIZE SIDEBAR
 * Use in Init function
 * 22/10/2022
 * Nguyen Ba Hai
 */
function sidebarMinimized(){
    let sidebarMinimal = $("#sidebar-minimal");
    let sidebarText = $(".item-list__item__text");
    sidebarMinimal.click(function(){
        if(sidebarMinimal.attr("Action") == "maximal"){
            sidebarMinimal.attr("title","Phóng to");
            sidebarMinimal.attr("Action","minimal");
            for (const iterator of sidebarText) {
                iterator.style.display = "none";
                iterator.parentElement.parentElement.style.width = "72px";
                $(".body").css("width",`calc(100% - ${iterator.parentElement.parentElement.style.width})`);
            }
            $("#minimal-sidebar").css("display","none");
            $("#maximal-sidebar").css("display","block");
        } else{
            sidebarMinimal.attr("title","Thu nhỏ");
            sidebarMinimal.attr("Action","maximal");
            for (const iterator of sidebarText) {
                iterator.style.display = "block";
                iterator.parentElement.parentElement.style.width = "200px";
                $(".body").css("width",`calc(100% - ${iterator.parentElement.parentElement.style.width})`);
            }
            $("#minimal-sidebar").css("display","block");
            $("#maximal-sidebar").css("display","none");
        }
    });
}

/**
 * FUNCTION TO OPEN THE FORM INPUT AND FOCUS
 * Use in Init function
 * 22/10/2022
 * Nguyen Ba Hai
 */
function popUpForm(){
    $("#pop-up-dialog").click(function(){
        $(".ms-dialog").css("display", "block");
        $("#tab-focus").focus();
    });
    dropdownEvent();
}

/**
 * FUNCTION TO CLOSE
 * Use in Init function
 * 22/10/2022
 * Nguyen Ba Hai
 */
function closeForm(){
    $(".ms-dialog-wrapper__header__exit").click(function(){
        $(".ms-dialog").css("display", "none");
    });
}


/**
 * DRAW TABLE:FUNCTION TO GENERATE THE CHECKBOX
 * 22/10/2022
 * Nguyen Ba Hai
 * Dang bi loi khong check duoc
 * @param {*} turple 
 * @param {*} tdBody 
 */
 function checkBoxAppend(tdBody){
    //Draw checkbox
    $("<input>").attr({
        type: 'checkbox',
        class: 'row-check mg-top6 ms-checkbox',
    }).appendTo(tdBody);
    tdBody.addClass("ms-table__col pd-left16 pd-right16");
    //initEvent
}

/**
 * DRAW TABLE:FUNCTION GENERATE THE FUNCTION COLUNM
 * 22/10/2022
 * Nguyen Ba Hai
 * @param {*} turple 
 * @param {*} tdBody 
 */
function functionAppend(tdBody){
    tdBody.addClass("ms-table__col pd-left16 pd-right16 align-center");
    let ddDiv = $("<div></div>");
    ddDiv.attr({
        class: 'ms-dropdown'
    }).appendTo(tdBody);
    let ddDivModified = $("<div></div>");
    ddDivModified.attr({
        class: 'ms-dropdown__box dropdown__box__modified'
    }).appendTo(ddDiv);
    let button = $("<button></button>");
    button.attr({
        class: 'ms-dropdown__box__icon dropdown-icon-modified dd-function-btn'
    }).appendTo(ddDivModified);
    let buttonDiv = $("<div></div>");
    buttonDiv.attr({
        class: 'sprite-bluearrdown',
    }).appendTo(button);
    $("<input>").attr({
        class: 'ms-dropdown__box__text dropdown-text-modified blue',
        type: 'text',
        placeholder: "Sửa",
        readonly: "readonly"
    }).appendTo(ddDivModified);
}

/**
 * DRAWTABLE: FUNCTION ADD EVENT TO CHECKBOX AT EACH TABLE
 * Calles in tableFunctionInitEvent()
 * 23/10/2022
 * Nguyen Ba Hai
 */
function tableCheckBoxEvent(){
    let mainCheck = $("#main-check");
    let rowCheck = $(".row-check");
    mainCheck.click(function(){
        if(mainCheck.is(':checked') == true){
            rowCheck.each(function(){
                $(this).prop('checked', true);
                $(this).parent().parent().find("td").css("backgroundColor","#F1FFEF");
                $(this).parent().parent().hover(function(){
                    $(this).find("td").css("backgroundColor","#F1FFEF");
                }, function(){
                    $(this).find("td").css("backgroundColor","#F1FFEF");
                });
            });
        }
        else{
            rowCheck.each(function(){
                $(this).prop('checked', false);
                $(this).parent().parent().find("td").css("backgroundColor","#FFFFFF");
                $(this).parent().parent().hover(function(){
                    $(this).find("td").css("backgroundColor","#F8F8F8");
                }, function(){
                    $(this).find("td").css("backgroundColor","#FFFFFF");
                });
            });
        }
    });
    rowCheck.each(function(){
        $(this).change(function(){
            if(mainCheck.is(':checked') == true){
                mainCheck.prop('checked', false);
            }
            if($(this).is(':checked') == false){
                $(this).parent().parent().find("td").css("backgroundColor","#FFFFFF");
                $(this).parent().parent().hover(function(){
                    $(this).find("td").css("backgroundColor","#F8F8F8");
                }, function(){
                    $(this).find("td").css("backgroundColor","#FFFFFF");
                });
            } else{
                $(this).parent().parent().find("td").css("backgroundColor","#F1FFEF");  
                $(this).parent().parent().hover(function(){
                    $(this).find("td").css("backgroundColor","#F1FFEF");
                }, function(){
                    $(this).find("td").css("backgroundColor","#F1FFEF");
                });
            }
        });
    });
}

/**
 * DRAWTABLE: FUNCTION ADD EVENT TO FIX DROPDOWN AT EACH TABLE
 * Calles in tableFunctionInitEvent()
 * 23/10/2022
 * Nguyen Ba Hai
 */
function fixedDropDownEvent(allData){
    let fixedDD = $("#dd-function-list");
    let idRow;
    $(".dd-function-btn").each(function(){   
        $(this).click(function(){
            idRow = $(this).parent().parent().parent().parent().attr("id");
            let top = $(this)[0].getBoundingClientRect().y + 20;
            let left = $(this)[0].getBoundingClientRect().x - fixedDD.width() + 4;
            if(fixedDD.css("display") == "none"){
                fixedDD.css("top",`${top}px`);
                fixedDD.css("left",`${left}px`);
                fixedDD.show();
            } else{
                fixedDD.hide();
            }
        });
    });
    $("#duplicate").unbind('click');
    $("#duplicate").click(function(){
        console.log($(this).text());
        fixedDD.hide();
    });
    $("#delete").unbind('click');
    $("#delete").click(function(){
        console.log(idRow);
        //Goi API xoa du lieu
        deleteByID(idRow,api, allData);
        //Hien thong bao xoa thanh cog
        fixedDD.hide();
    });
    $("#stop").unbind('click');
    $("#stop").click(function(){
        console.log($(this).text());
        fixedDD.hide();
    });
}

/**
 * FUNCTION CALL API DELETE BY ID
 * Nguyen Ba Hai
 * 23/11/2022
 */
function deleteByID(id, api, allData){
    $.ajax({
        url: `${api}/${id}`,
        type: "DELETE", // <- Change here
        contentType: "application/json",
        success: function() {
            //Ve lai bang
            let newArr = allData.filter(function(value, index, arr){
                return value.EmployeeId !== id;
            });
            console.log(newArr);
            pagingFunction(allData.length-1, newArr, table);
        },
        error: function() {
        }
    });
}

/**
 * FUNCTION PAGING TABLE
 * This function call the drawTable() each time the page table change
 * Nguyen Ba Hai
 * 23/10/2022
 */
function pagingFunction(records, data, table){
    //records: tong so ban ghi, data: day du lieu, table: id bang
    let numberRecordMax = $("#numberOfRecordInPage").val(); //Number of records in a page table, default is 10
    let eventCatch = $("#numberOfRecordInPage + div").find(".ms-dropdown__list__value");
    let totalRecords = $("#total-records");
    let numberOfPage = records%numberRecordMax == 0?records/numberRecordMax:Math.floor(records/numberRecordMax) + 1;//SO luong trang
    let pageNumber = 1;//page number default
    let offSet = 0;//dem ban ghi bat dau tu 1
    let recordFrom = $("#recordFrom");
    totalRecords.text(`${records}`);
    console.log(`Total records: ${records}`);
    console.log(`Number of pages:  ${numberOfPage}`);
    console.log(`Number of records in page:  ${numberRecordMax}`);
    console.log(`Page number: ${pageNumber}`);
    //init table
    let recordPerPage = data.slice(numberRecordMax*pageNumber-numberRecordMax,numberRecordMax*pageNumber);
    $("tbody > tr").remove();
    drawTable(recordPerPage,table,data);
    $("#go-back").css("display","none");
    $("#dis-go-back").css("display","block");
    $("#dis-go-next").css("display","none");
    $("#go-next").css("display","block");
    recordFrom.text(`${offSet + 1} - ${offSet+recordPerPage.length}`);
    offSet += recordPerPage.length;

    //Find number of record max
    eventCatch.each(function(){//Event doi so luong ban ghi 1 trang
        $(this).click(function(){
            $("tbody > tr").remove();//clear data table
            $("#numberOfRecordInPage").val($(this).text());
            $("#numberOfRecordInPage").parent().find(".ms-dropdown__list").css("display","none");//undisplay list
            offSet = 0;
            $("#go-back").css("display","none");
            $("#dis-go-back").css("display","block");
            $("#dis-go-next").css("display","none");
            $("#go-next").css("display","block");
            numberRecordMax = $(this).text();//change max record value
            pageNumber = 1;
            numberOfPage = records%numberRecordMax == 0?records/numberRecordMax:Math.floor(records/numberRecordMax) + 1;
            console.log(`Number of pages:  ${numberOfPage}`);
            console.log(`Number of records in page:  ${numberRecordMax}`);
            console.log(`Page number: ${pageNumber}`);
            recordPerPage = data.slice(numberRecordMax*pageNumber-numberRecordMax,numberRecordMax*pageNumber);
            drawTable(recordPerPage,table,data);
            recordFrom.text(`${offSet + 1} - ${offSet+recordPerPage.length}`);
            offSet += recordPerPage.length;
        });
    });
    if(pageNumber >= 1 && pageNumber <= numberOfPage){//Bat su kien chuyen trang
        if(numberOfPage == 1){
            $("#go-back").css("display","none");
            $("#dis-go-back").css("display","block");
            $("#dis-go-next").css("display","block");
            $("#go-next").css("display","none");
        }
        else{
            $("#go-next").click(function(){
                if(pageNumber < numberOfPage){
                    pageNumber++;
                    console.log(`Page number: ${pageNumber}`);
                    $("tbody > tr").remove();//clear data table
                    recordPerPage = data.slice(numberRecordMax*pageNumber-numberRecordMax,numberRecordMax*pageNumber);
                    drawTable(recordPerPage,table,data);
                    recordFrom.text(`${offSet + 1} - ${offSet+recordPerPage.length}`);
                    offSet += recordPerPage.length;
                    if(pageNumber > 1){
                        $("#go-back").css("display","block");
                        $("#dis-go-back").css("display","none");
                    }
                    if(pageNumber == numberOfPage){
                        $("#dis-go-next").css("display","block");
                        $("#go-next").css("display","none");
                    }
                }
            });
            $("#go-back").click(function(){
                if(pageNumber > 1){
                    offSet -= data.slice(numberRecordMax*pageNumber-numberRecordMax,numberRecordMax*pageNumber).length;
                    pageNumber--;
                    console.log(`Page number: ${pageNumber}`);
                    $("tbody > tr").remove();//clear data table
                    recordPerPage = data.slice(numberRecordMax*pageNumber-numberRecordMax,numberRecordMax*pageNumber);
                    drawTable(recordPerPage,table,data);
                    recordFrom.text(`${offSet - recordPerPage.length + 1} - ${offSet}`);
                    // offSet -= recordPerPage.length;
                    if(pageNumber < numberOfPage){
                        $("#dis-go-next").css("display","none");
                        $("#go-next").css("display","block");
                    }
                    if(pageNumber == 1){
                        $("#go-back").css("display","none");
                        $("#dis-go-back").css("display","block");
                    }
                }
            });
        }    
    }
}

/**
 * 
 * FUNCTION DRAWTABLE
 * @param {*} data 
 * @param {*} table 
 * Nguyen Ba Hai
 * 23/10/2022
 */
function drawTable(dataSet,table,allData){
    // console.log(dataSet);
    for (const data of dataSet) {
        let trBody = $("<tr></tr>");
        trBody.attr("id",`${data.EmployeeId}`);
        trBody.addClass("ms-table__row");
        table.find("th").each(function() {
            let tdBody = $("<td></td>");
            tdBody.addClass("pd-left16 pd-right16 ms-table__col");
            if($(this).attr("tableTurple") == "CheckBox"){
                checkBoxAppend(tdBody);
            } else if($(this).attr("tableTurple") == "Function"){
                functionAppend(tdBody);
            } else{
                let cellValue = data[$(this).attr("tableTurple")];
                if(cellValue != null){
                    if($(this).attr("tableTurple") == "DateOfBirth"){
                        tdBody.addClass("align-center");
                        cellValue = new Date(cellValue);
                        cellValue = formatDate(cellValue);
                    } 
                    if($(this).attr("tableTurple") == "Gender"){
                        cellValue = formatGender(cellValue);
                    } 
                    if($(this).attr("tableTurple") == "Salary"){
                        cellValue = formatMoney(cellValue);
                    } 
                    tdBody.append(cellValue);
                }
            }
            // functionAppend($(this), tdBody);
            trBody.append(tdBody);
        });
        $("tbody").append(trBody);
    }
    $("tbody tr").hover(function(){
        $(this).find("td").css("backgroundColor","#F8F8F8");
    }, function(){
        $(this).find("td").css("backgroundColor","#FFFFFF");
    });
    tableFunctionInitEvent(allData);//Khoi tao su kien cho bang moi
}

/**
 * FUNCTION INIT EVENT FOR TABLE
 * Nguyen Ba Hai
 * 23/10/2022
 */
function tableFunctionInitEvent(allData){
    dropdownEvent();
    fixedDropDownEvent(allData);
    tableCheckBoxEvent();
}

const table = $("#tableID");
const api = table.attr("api");
new EmployeePage();
// console.log(totalRecords);
