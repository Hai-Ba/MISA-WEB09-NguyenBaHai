using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.Common.Enums
{
    public enum Exceptions
    {
        //TRường hợp xảy ra ngoại lệ, thông báo cho Misa
        Exception = 1,

        //Trường hợp trùng mã
        DuplicateCode = 2,

        //Trường hợp dữ liệu không hợp lệ
        InvalidData = 3,

        //TRường hợp không tìm thấy dữ liệu
        DataNotFound = 4,

        //Trường dữ liệu chưa được điền
        MissingField = 5,
    }
}
