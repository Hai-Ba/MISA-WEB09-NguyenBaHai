/**
 * FUNCTION CHECK THE EMPLOYEECODE IS RIGHT FORMAT
 * Nguyen Ba Hai
 * 24/10/2022
 * @param {*} id 
 * @returns 
 */
function isRightFormat(id){
    if(id[0] == 'N' && id[1] == 'V' && isNumber(id.slice(2))){
        return true;
    }
    return false;
}

/**
 * FUNCTION CHECK IF STRING IS ALL DIGIT
 * Nguyen Ba Hai
 * 24/10/2022
 * @param {} n 
 * @returns 
 */
function isNumber(n) { return !isNaN(parseFloat(n)) && !isNaN(n - 0) }
