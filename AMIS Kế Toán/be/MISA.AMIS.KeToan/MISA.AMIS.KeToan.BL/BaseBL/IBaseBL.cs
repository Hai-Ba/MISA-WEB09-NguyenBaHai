using MISA.AMIS.KeToan.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.BL
{
    public interface IBaseBL<T>
    {
        /// <summary>
        /// Lấy danh sách tất cả bản ghi
        /// Author: Nguyễn Bá Hải
        /// Date: 10/11/2022
        /// </summary>
        /// <returns>Danh sách tất cả bản ghi</returns>
        public IEnumerable<T> GetAllRecords(string? keyword);

        /// <summary>
        /// Lấy thông tin 1 bản ghi theo id
        /// Author: Nguyễn Bá Hải
        /// Date: 10/11/2022
        /// </summary>
        /// <param name="recordID">ID bản ghi muốn lấy</param>
        /// <returns>Thông tin 1 bản ghi</returns>
        public T GetRecordByID(Guid recordID);

        /// <summary>
        /// API xóa 1 bản ghi theo ID
        /// Author: Nguyễn Bá Hải
        /// Date:3/11/2022
        /// </summary>
        /// <param name="recordID">ID của bản ghi muốn xóa</param>
        /// <returns></returns>
        public int DeleteRecordByID(Guid recordID);

        /// <summary>
        /// API xóa nhiều bản ghi
        /// Author: Nguyễn Bá Hải
        /// Date:3/11/2022
        /// </summary>
        /// <param name="listID"></param>
        /// <returns></returns>
        public void DeleteMany(string[] listID);
    }
}
