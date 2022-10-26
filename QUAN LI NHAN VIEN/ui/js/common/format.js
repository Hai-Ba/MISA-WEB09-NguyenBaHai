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

//FORMAT ID
function formatID(id){
    let idFormatted;
    let first = id.slice(0,2);
    let second = `${id.slice(2)*1 + 1}`;
    switch(second.length){
        case 1:
            idFormatted = first.concat(`00000${second}`);
            break;
        case 2:
            idFormatted = first.concat(`0000${second}`);
            break;
        case 3:
            idFormatted = first.concat(`000${second}`);
            break;
        case 4:
            idFormatted = first.concat(`00${second}`);
            break;
        case 5:
            idFormatted = first.concat(`0${second}`);
            break;
        default:
            idFormatted = first.concat(second);
    }
    return idFormatted;
}