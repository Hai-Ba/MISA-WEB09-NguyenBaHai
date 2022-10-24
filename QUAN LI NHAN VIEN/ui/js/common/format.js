//FORMAT DATE THEO DINH DANG DD/MM/YYYY
function formatDate(value){
    value = `${value.getDate()}/${value.getMonth() + 1}/${value.getFullYear()}`;
    return value;
}

//FORMAT GIOI TINH 0:NAM  , 1: NU,  2: KHAC
function formatGender(value){
    // console.log(value)
    switch(value) {
        case 0:
            value = "Nam";
            break;
        case 1:
            value = "Nữ";
            break;
        case 2:
            value = "Khác";
            break;
        default:
            value = "";
    }
    return value;
}

//FORMAT TIEN CO DAU NGAN CACH
function formatMoney(value){
    return value;
}

export {formatDate, formatGender, formatMoney};
