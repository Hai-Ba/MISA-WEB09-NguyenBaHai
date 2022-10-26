class EmployeePage extends Base{
    constructor() {
        super();
    }

    initEvent(){
        popUpFormAdd(); 
        closeForm();
    }
}

/**
 * FUNCTION TO OPEN THE FORM INPUT AND FOCUS
 * Use in Init function
 * 22/10/2022
 * Nguyen Ba Hai
 * @Overriden
 */
function popUpFormAdd(){
    $("#pop-up-dialog").unbind('click');
    $("#pop-up-dialog").click(function(){
        $("#form-title").text("Thêm thông tin ");
        $(".ms-dialog").css("display", "block");
        $("#tab-focus").focus();
        validateFormAddEmployee(1);
        dropdownEvent();
        autoFillNewID();
    });
}

/**
 * FUNCTION TO OPEN THE FORM FIX AND FOCUS
 * Use in Duplicate function
 * 24/10/2022
 * Nguyen Ba Hai
 * @Overrider
 */
 function popUpFormDuplicate(){
    $(".ms-dialog-wrapper")[0].reset();
    $("#form-title").text("Nhân bản thông tin");
    $(".ms-dialog").css("display", "block");
    $("#tab-focus").focus();
    //Ham validate tu them cho tung Trang
    // validateFormAddEmployee(1);
    dropdownEvent();
}

/**
 * FUNCTION TO OPEN THE FORM INPUT AND FOCUS
 * Use in Init function
 * 24/10/2022
 * Nguyen Ba Hai
 */
async function autoFillNewID(){
    let id = await apiGetNewId();
    $("#tab-focus").val(id);
}

/**
 * VALIDATE CHO THEM MOI NHAN VIEN
 * Nguyen Ba Hai
 * 24/10/2022
 */
//Validate
function validateFormAddEmployee(type){
    $("#ms-add-employee").unbind("click");
    $("#ms-add-employee").click(function(){
        if(!$("#tab-focus").val()|| !$("#employee-name-field").val() || !$("#department-name-field").val()  || !isRightFormat($("#tab-focus").val())){
            if(!$("#tab-focus").val()){
                $("#tab-focus").addClass("ms-input-missing");
                $("#tab-focus ~ .ms-textbox__warning__missing").show();
            }
            else{
                if(!isRightFormat($("#tab-focus").val())){
                    $("#tab-focus").addClass("ms-input-missing");
                    $("#tab-focus ~ .ms-textbox__warning__format").show();
                }
            }
            if(!$("#employee-name-field").val()){
                $("#employee-name-field").addClass("ms-input-missing");
                $("#employee-name-field ~ .ms-textbox__warning__missing").show();
            }
            if(!$("#department-name-field").val()){
                $("#department-name-field").addClass("ms-input-missing");
                $("#department-name-field").parent().siblings(".ms-dropdown__message__missing").show();
            }
            if(!isRightFormat($("#tab-focus").val())){
                showErrorLostInfo("Mã chưa đúng định dạng");
            } else if(!$("#tab-focus").val()){
                showErrorLostInfo("Mã nhân viên không được để trống");
            } else if(!$("#employee-name-field").val()){
                showErrorLostInfo("Tên nhân viên không được để trống");
            } else if(!$("#department-name-field").val()){
                showErrorLostInfo("Tên phòng ban không được để trống");
            }
        }
        else{
            let departName = $("input[name='department']").val();
            let departId;
            //Lay id phong ban
            $("#department-mapping").find("div").each(function(){
                if($(this).text() == departName){
                    departId = $(this).attr("id");
                }
            });
            let employeeData = {}; //Tao obj chua noi dung employeeData
            employeeData.EmployeeCode = $("#tab-focus").val();
            employeeData.EmployeeName = $("#employee-name-field").val();
            employeeData.DepartmentId = departId;
            employeeData.Gender = $("input[name='gender']:checked").val();
            employeeData.DateOfBirth = $("input[name='dob']").val();
            // employeeData.PhoneNumber = $("input[name='phoneNumber']").val();
            // employeeData.Email = $("input[name='email']").val();
            employeeData.Address = $("input[name='address']").val();
            employeeData.IdentityNumber = $("input[name='idNumber']").val();
            // employeeData.IdentityDate = $("input[name='idDate']").val()
            // employeeData.IdentityPlace = $("input[name='idPlace']").val();
            // employeeData.TelephoneNumber = $("input[name='telephoneNumber']").val();
            employeeData.BankAccountNumber = $("input[name='banhAccount']").val();
            employeeData.BankName = $("input[name='bankName']").val();
            employeeData.BankProvinceName = $("input[name='bankProvince']").val();
            // employeeData.PositionId = "05458d48-e7a0-11ec-9b48-00163e06abee";
            $(".ms-dialog-wrapper")[0].reset(); //Reset form
            $(".ms-dialog").css("display", "none");
            //Thuc hien hoi API them moi nhan vien
            addEmployee(employeeData,type);  
        }
    });
    //APPLY EVENT: Duplicate, Delete, Stop
    $("#tab-focus").on("input",function(){
        $("#tab-focus").removeClass("ms-input-missing");
        $("#tab-focus ~ .ms-textbox__warning__missing").hide();
        $("#tab-focus ~ .ms-textbox__warning__format").hide();
    })
    $("#employee-name-field").on("input",function(){
        $("#employee-name-field").removeClass("ms-input-missing");
        $("#employee-name-field ~ .ms-textbox__warning__missing").hide();
    })
    $("#department-name-field").val('').change(function(){
        $("#department-name-field").removeClass("ms-input-missing");
        $("#department-name-field").parent().siblings(".ms-dropdown__message__missing").hide();
    })
}

//Close form
function closeForm(){
    $(".ms-dialog-wrapper__header__exit").click(function(){
        //clear thoong tin form
        $("#tab-focus").removeClass("ms-input-missing");
        $("#tab-focus ~ .ms-textbox__warning__missing").hide();
        $("#tab-focus ~ .ms-textbox__warning__format").hide();
        $("#employee-name-field").removeClass("ms-input-missing");
        $("#employee-name-field ~ .ms-textbox__warning__missing").hide();
        $("#department-name-field").removeClass("ms-input-missing");
        $("#department-name-field").parent().siblings(".ms-dropdown__message__missing").hide();
        $(".ms-dialog-wrapper")[0].reset();
        $(".ms-dialog").css("display", "none");
        fixedDropDownEvent();
    });
    $("#ms-close-form").click(function(){
        //clear thong tin tren form
        $("#tab-focus").removeClass("ms-input-missing");
        $("#tab-focus ~ .ms-textbox__warning__missing").hide();
        $("#tab-focus ~ .ms-textbox__warning__format").hide();
        $("#employee-name-field").removeClass("ms-input-missing");
        $("#employee-name-field ~ .ms-textbox__warning__missing").hide();
        $("#department-name-field").removeClass("ms-input-missing");
        $("#department-name-field").parent().siblings(".ms-dropdown__message__missing").hide();
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

//Add employee
async function addEmployee(employeeData,type){
    await apiPostData(employeeData,type);
    let data = await apiGetAllData();
    pagingFunction(data);
}

//Duplicate handle
async function getFormFill(id){
    try {
        //Lay data tu id
        let data = await apiGetDataById(id);

        //Validate va them lai
        await validateFormAddEmployee(0);
        //Fill vao form
        let departName;
        $("#department-mapping").find("div").each(function(){
            if(data.DepartmentId == $(this).attr("id")){
                departName = $(this).text();
            }
        });
        //Lay 3 gia tri quan trong
        $("#tab-focus").val(data.EmployeeCode);
        $("#employee-name-field").val(data.EmployeeName);
        $("#department-name-field").val(departName);
        // $("input[name='dob']").val(new Date(data.DateOfBirth).toLocaleDateString());
        $("input[name='idNumber']").val(data.IdentityNumber);
        $("input[name='banhAccount']").val(data.BankAccountNumber);
        $("input[name='bankName']").val(data.BankName);
        $("input[name='bankProvince']").val(data.BankProvinceName);
        $("input[name='address']").val(data.Address);

        //Xoa du lieu ban dau
        await apiDeleteById(id,0);

    } catch (error) {
        displayErrorNotification("Thông tin nhân viên này đã bị xóa! Không thể nhân bản");
    }
    
}

new EmployeePage();
