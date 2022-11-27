using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.Common
{
    public class ResponseService
    {
        #region Properties
        /// <summary>
        /// Trạng thái của kết quả trả về
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        public object? Data { get; set; }
        #endregion

        #region Constructors
        public ResponseService(bool isSuccess, object? data)
        {
            IsSuccess = isSuccess;
            Data = data;
        }
        #endregion
    }
}
