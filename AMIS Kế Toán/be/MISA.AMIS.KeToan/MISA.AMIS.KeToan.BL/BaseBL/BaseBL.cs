using MISA.AMIS.KeToan.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.BL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Field
        private IBaseDL<T> _baseDL;
        #endregion

        #region Constructor
        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }

        #endregion

        /// <summary>
        /// API xóa 1 bản ghi theo ID
        /// Author: Nguyễn Bá Hải
        /// Date:3/11/2022
        /// </summary>
        /// <param name="recordID">ID của bản ghi muốn xóa</param>
        /// <returns></returns>
        public int DeleteRecordByID(Guid recordID)
        {
            return _baseDL.DeleteRecordByID(recordID);
        }

        /// <summary>
        /// API xóa nhiều bản ghi
        /// Author: Nguyễn Bá Hải
        /// Date:3/11/2022
        /// </summary>
        /// <param name="listID"></param>
        /// <returns></returns>
        public void DeleteMany(string[] listID)
        {
            _baseDL.DeleteMany(listID);
        }

        /// <summary>
        /// Lấy danh sách tất cả nhân viên
        /// Author: Nguyễn Bá Hải
        /// Date: 10/11/2022
        /// </summary>
        /// <returns>Danh sách tất cả nhân viên</returns>
        public IEnumerable<T> GetAllRecords(string? keyword)
        {
            return _baseDL.GetAllRecords(keyword);
        }

        /// <summary>
        /// Lấy thông tin 1 nhân viên theo id
        /// Author: Nguyễn Bá Hải
        /// Date: 10/11/2022
        /// </summary>
        /// <param name="employeeID">ID nhân viên muốn lấy</param>
        /// <returns>Thông tin 1 nhân viên</returns>
        public T GetRecordByID(Guid recordID)
        {
            return _baseDL.GetRecordByID(recordID);
        }
    }
}
