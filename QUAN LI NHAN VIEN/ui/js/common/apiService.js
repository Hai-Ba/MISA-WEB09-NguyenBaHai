//https://amis.manhnv.net/api/v1/Employees
//https://amis.manhnv.net/api/v1/Departments
//https://amis.manhnv.net/api/v1/Positions
const table = $("#tableID");
const api = table.attr("api");
let apiEmployee = "https://amis.manhnv.net/api/v1/Employees";
let apiDepartment = "https://amis.manhnv.net/api/v1/Departments";
let apiPosition = "https://amis.manhnv.net/api/v1/Positions";
let getNewId;

if(api == apiEmployee){
    getNewId = "/NewEmployeeCode";//Get the new ID for Employee
}


//API get all data: api
async function apiGetAllData(){
    try {
        let record;
    await $.ajax({
        url: `${api}`,
        type: "GET", // <- Change here
        success: function(res) {
            record = res;
        },
        error: function() {
            //Loi
            if(api == apiEmployee){
                displayWarningNotification("Lấy thông tin tất cả nhân viên thất bại.");
            } else if(api == apiDepartment){
                displayWarningNotification("Lấy thông tin tất cả phòng ban thất bại.");
            } else if(api == apiPosition){
                displayWarningNotification("Lấy thông tin tất cả chức vụ thất bại.");
            }
        }
    });
    return record;
    } catch (error) {
        console.log(error);
    }
}

//API get data by id: api/{id}
async function apiGetDataById(id){
    try {
        let data;
        await $.ajax({
            url: `${api}/${id}`,
            type: "GET", // <- Change here
            contentType: "application/json",
            success: function(res) {
                data = res;
            },
            error: function() {
                //Loi
                if(api == apiEmployee){
                    displayWarningNotification("Lấy thông tin nhân viên từ id thất bại.");
                } else if(api == apiDepartment){
                    displayWarningNotification("Lấy thông tin phòng ban từ id thất bại.");
                } else if(api == apiPosition){
                    displayWarningNotification("Lấy thông tin chức vụ từ id thất bại.");
                }
            }
        });
        return data;
    } catch (error) {
        console.log(error);
    }
}

//API get new id: api/getNewId
async function apiGetNewId(){
    try {
        let id;
        await $.ajax({
            url: `${api}${getNewId}`,
            type: "GET", // <- Change here
            contentType: "application/json",
            success: function(res) {
                //Format new id
                if(res != undefined){
                    id = formatID(res);
                } else{
                    if(api == apiEmployee){
                        id = "NV000001";
                    } else if(api == apiDepartment){
                        id = "DE000001";
                    } else if(api == apiPosition){
                        id = "PO000001";
                    }
                }
            },
            error: function() {
                //Loi
                if(api == apiEmployee){
                    displayWarningNotification("Lấy mã nhân viên thất bại.");
                } else if(api == apiDepartment){
                    displayWarningNotification("Lấy mã phòng ban thất bại.");
                } else if(api == apiPosition){
                    displayWarningNotification("Lấy mã chức vụ thất bại.");
                }
            }
        });
        return id;
    } catch (error) {
        console.log("No data is displayed!");
    }
}

//API delete by id
async function apiDeleteById(id, type){
    try {
        await $.ajax({
            url: `${api}/${id}`,
            type: "DELETE", // <- Change here
            contentType: "application/json",
            success: function() {
                if(type == 1){
                    //Type 1 la co thong bao
                    //Popup da xoa thanh cong
                    if(api == apiEmployee){
                        displaySuccessNotification("Xóa nhân viên thành công.");
                    } else if(api == apiDepartment) {
                        displaySuccessNotification("Xóa phòng ban thành công.");
                    } else if(api == apiPosition){
                        displaySuccessNotification("Xóa chức vụ thành công.");
                    }
                }
            },
            error: function(e) {
                displayWarningNotification("Xóa dữ liệu thất bại.");
            }
        });
    } catch (error) {
        console.log(error);
    }
}

//API post new employee
async function apiPostData(data, type){
    //type 2: Change Data and inform Change Success
    //type 1: Post Data and inform Add Success
    //type 0: Post Data and inform Dupliecate Success
    try {
        await $.ajax({
            type: "POST",
            url: api,
            data: JSON.stringify(data),
            dataType: 'json',
            contentType: "application/json",
            success: function(){
                //Popup them nv thanh cong
                if(type == 1){
                    if(api == apiEmployee){
                        displaySuccessNotification("Thêm mới nhân viên thành công.");
                    } else if(api == apiDepartment) {
                        displaySuccessNotification("Thêm mới phòng ban thành công.");
                    } else if(api == apiPosition){
                        displaySuccessNotification("Thêm mới chức vụ thành công.");
                    }
                } else if(type == 0){ //type = 0 la nhan ban
                    if(api == apiEmployee){
                        displaySuccessNotification("Nhân bản thông tin nhân viên thành công.");
                    } else if(api == apiDepartment) {
                        displaySuccessNotification("Nhân bản thông tin phòng ban thành công.");
                    } else if(api == apiPosition){
                        displaySuccessNotification("Nhân bản thông tin chức vụ thành công.");
                    }
                } else{
                    if(api == apiEmployee){
                        displaySuccessNotification("Chỉnh sửa thông tin nhân viên thành công.");
                    } else if(api == apiDepartment) {
                        displaySuccessNotification("Chỉnh sửa thông tin phòng ban thành công.");
                    } else if(api == apiPosition){
                        displaySuccessNotification("Chỉnh sửa thông tin chức vụ thành công.");
                    }
                }
            },
            error: function(e){
                //Popup ma nvien da ton tai
                if(e.status == 400){
                    if(api == apiEmployee){
                        showWarningExist("Mã nhân viên đã tồn tại! Thêm thất bại");
                    }else if(api == apiDepartment){
                        showWarningExist("Mã phòng ban đã tồn tại! Thêm thất bại");
                    } else if(apt == apiPosition){
                        showWarningExist("Mã vị trí đã tồn tại! Thêm thất bại");
                    }
                }
            }
        });
    } catch (error) {
        console.log(error);
    }
}
