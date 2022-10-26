class Base{
    constructor(){
        this.initEvent();
        this.fetchData();
    }

    initEvent(){
        sidebarMinimized();
        popUpFormAdd();
        closeForm();
        refreshTable();
    }

    async fetchData(){
        if(window.navigator.onLine){
            try {
                //Get all data
                let data = await apiGetAllData();
                pagingFunction(data);
            } catch (error) {
                displayErrorNotification("Không thể cập nhật được dữ liệu. Vui lòng kiểm tra lại kết nối internet.");
            }
        }
        else{
            displayErrorNotification("Không thể cập nhật được dữ liệu. Vui lòng kiểm tra lại kết nối internet.");
        }
    }
}

/**
 * FUNCTION TO RELOAD TABLE
 * 25/10/2022
 * Nguyen Ba Hai
 */
function refreshTable(){
    $("#refresh-table-btn").unbind('click');
    $("#refresh-table-btn").click(async function(){
        //refresh page
        // let container = $(".content__table");
        // var content = container.innerHTML;
        // container.innerHTML = content;
        let data = await apiGetAllData();
        pagingFunction(data);
        displaySuccessNotification("Làm mới bảng dữ liệu.");
    });
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
 function popUpFormAdd(){
    $("#pop-up-dialog").unbind('click');
    $("#pop-up-dialog").click(function(){
        $("#form-title").text("Thêm thông tin");
        $(".ms-dialog").css("display", "block");
        $("#tab-focus").focus();
        //Ham validate tu them cho tung Trang
        dropdownEvent();
    });
}

/**
 * FUNCTION TO OPEN THE FORM FIX AND FOCUS
 * Use in Duplicate function
 * 24/10/2022
 * Nguyen Ba Hai
 */
 function popUpFormDuplicate(){
    $("#form-title").text("Nhân bản thông tin");
    $(".ms-dialog").css("display", "block");
    $("#tab-focus").focus();
    //Ham validate tu them cho tung Trang
    dropdownEvent();
}

/**
 * FUNCTION TO CLOSE FORM INPUT
 * Use in Init function
 * 22/10/2022
 * Nguyen Ba Hai
 */
 function closeForm(){
    $(".ms-dialog-wrapper__header__exit").click(function(){
        //clear thoong tin form
        $(".ms-dialog-wrapper")[0].reset();
        $(".ms-dialog").css("display", "none");
        fixedDropDownEvent();
    });
    $("#ms-close-form").click(function(){
        //clear thong tin tren form
        $(".ms-dialog-wrapper")[0].reset();
        $(".ms-dialog").css("display", "none");
        fixedDropDownEvent();
    });
    $("#ms-save-form").click(function(){
        //khong clear thong tin
        $(".ms-dialog").css("display", "none");
        fixedDropDownEvent();
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
    mainCheck.unbind('click');
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
 * FUNCTION CALL API DELETE BY ID
 * Nguyen Ba Hai
 * 23/11/2022
 */
async function deleteByID(id){
    let data;
    await apiDeleteById(id, 1);
    data = await apiGetAllData();
    pagingFunction(data);
}

/**
 * DRAWTABLE: FUNCTION ADD EVENT TO FIX DROPDOWN AT EACH TABLE
 * Calles in tableFunctionInitEvent()
 * 23/10/2022
 * Nguyen Ba Hai
 */
function fixedDropDownEvent(){
    let fixedDD = $("#dd-function-list");
    let idRow;
    $(".dd-function-btn").each(function(){ 
        $(this).unbind('click');  
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
    //Apply Duplicate function
    $("#duplicate").unbind('click');
    $("#duplicate").click(function(){
        popUpFormDuplicate();
        //FILL CHO EMPLOYEE
        getFormFill(idRow);
        fixedDD.hide();
    });

    //Apply Delete function
    $("#delete").unbind('click');
    $("#delete").click(function(){
        //Hiển thi dialog gợi ý
        $("#ask-delete-eployee").css("display","flex");
        //Khoi tao su kien cho popup
        dialogPopupClose();
        //Goi API xoa du lieu
        $("#aggree-delete").unbind('click');
        $("#aggree-delete").click(function(){
            deleteByID(idRow);
            $("#ask-delete-eployee").hide();
        });
        fixedDD.hide();
    });

    //Apply Stop function
    $("#stop").unbind('click');
    $("#stop").click(function(){
        console.log($(this).text());
        fixedDD.hide();
    });
}

/**
 * FUNCTION PAGING TABLE
 * This function call the drawTable() each time the page table change
 * Nguyen Ba Hai
 * 23/10/2022
 */
 function pagingFunction(data){
    let records = data.length;
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
    let recordsDisplay = data.slice(numberRecordMax*pageNumber-numberRecordMax,numberRecordMax*pageNumber);
    $("tbody > tr").remove();
    drawTable(recordsDisplay);
    if(records > numberRecordMax){
        $("#go-back").css("display","none");
        $("#dis-go-back").css("display","block");
        $("#dis-go-next").css("display","none");
        $("#go-next").css("display","block");
    } else{
        $("#go-back").css("display","none");
        $("#dis-go-back").css("display","block");
        $("#dis-go-next").css("display","block");
        $("#go-next").css("display","none");
    }
    if(records == 0){
        recordFrom.text("0");
    }
    else{
        recordFrom.text(`${offSet + 1} - ${offSet+recordsDisplay.length}`);
    }
    // recordFrom.text(`${offSet + 1} - ${offSet+recordsDisplay.length}`);
    offSet += recordsDisplay.length;

    //Find number of record max
    eventCatch.each(function(){//Event doi so luong ban ghi 1 trang
        $(this).unbind('click');
        $(this).click(function(){
            $("tbody > tr").remove();//clear data table
            $("#numberOfRecordInPage").val($(this).text());
            $("#numberOfRecordInPage").parent().find(".ms-dropdown__list").css("display","none");//undisplay list
            offSet = 0;
            numberRecordMax = $(this).text();//change max record value
            pageNumber = 1;
            numberOfPage = records%numberRecordMax == 0?records/numberRecordMax:Math.floor(records/numberRecordMax) + 1;
            console.log(`Number of pages:  ${numberOfPage}`);
            console.log(`Number of records in page:  ${numberRecordMax}`);
            console.log(`Page number: ${pageNumber}`);
            recordsDisplay = data.slice(numberRecordMax*pageNumber-numberRecordMax,numberRecordMax*pageNumber);
            if(records > numberRecordMax){
                $("#go-back").css("display","none");
                $("#dis-go-back").css("display","block");
                $("#dis-go-next").css("display","none");
                $("#go-next").css("display","block");
            } else{
                $("#go-back").css("display","none");
                $("#dis-go-back").css("display","block");
                $("#dis-go-next").css("display","block");
                $("#go-next").css("display","none");
            }
            drawTable(recordsDisplay);
            recordFrom.text(`${offSet + 1} - ${offSet+recordsDisplay.length}`);
            offSet += recordsDisplay.length;
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
            $("#go-next").unbind('click');
            $("#go-next").click(function(){
                if(pageNumber < numberOfPage){
                    pageNumber++;
                    console.log(`Page number: ${pageNumber}`);
                    $("tbody > tr").remove();//clear data table
                    recordsDisplay = data.slice(numberRecordMax*pageNumber-numberRecordMax,numberRecordMax*pageNumber);
                    drawTable(recordsDisplay);
                    recordFrom.text(`${offSet + 1} - ${offSet+recordsDisplay.length}`);
                    offSet += recordsDisplay.length;
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
            $("#go-back").unbind('click');
            $("#go-back").click(function(){
                if(pageNumber > 1){
                    offSet -= data.slice(numberRecordMax*pageNumber-numberRecordMax,numberRecordMax*pageNumber).length;
                    pageNumber--;
                    console.log(`Page number: ${pageNumber}`);
                    $("tbody > tr").remove();//clear data table
                    recordsDisplay = data.slice(numberRecordMax*pageNumber-numberRecordMax,numberRecordMax*pageNumber);
                    drawTable(recordsDisplay);
                    recordFrom.text(`${offSet - recordsDisplay.length + 1} - ${offSet}`);
                    // offSet -= recordsDisplay.length;
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
function drawTable(dataSet){
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
                        cellValue = formatDateDDMMYYYY(cellValue);
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
    tableFunctionInitEvent();//Khoi tao su kien cho bang moi
}

/**
 * FUNCTION INIT EVENT FOR TABLE
 * Nguyen Ba Hai
 * 23/10/2022
 */
function tableFunctionInitEvent(){
    dropdownEvent();
    fixedDropDownEvent();
    tableCheckBoxEvent();
}
// Construct Base Object
new Base();