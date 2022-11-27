using MISA.AMIS.KeToan.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.DL
{
    public interface IEmployeeDL : IBaseDL<Employee>
    {
        /// <summary>
        /// API lấy danh sách bản ghi theo phân trang
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <param name="keyword">Lọc dữ liệu theo tên, sdt, mã NV</param>
        /// <param name="limit">Giới hạn bản ghi 1 trang</param>
        /// <param name="pageNumber">Trang số</param>
        public List<Employee> GetEmployeeByPaging(string? keyword, int limit, int pageNumber);

        /// <summary>
        /// API lấy mã nhân viên lớn nhất cộng thêm 1
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <returns></returns>
        public string GetNewEmployeeCode();

        /// <summary>
        /// API thêm mới 1 nhân viên
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <param name="employee">nhân viên truyền vào Body</param>
        /// <returns></returns>
        public Guid InsertNewEmployee(Employee employee);

        /// <summary>
        /// API Sửa 1 nhân viên theo ID
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <param name="employee">employee truyền vào body để sửa</param>
        /// <param name="employeeID">Mã Guid id của nhân viên cần sửa</param>
        /// <returns></returns>
        public int UpdateAnEmployee(Employee employee, Guid employeeID);

        /// <summary>
        /// API check mã trùng của nhân viên
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns>Số lượng trùng</returns>
        public int CheckDuplicateCode(string employeeCode);
    }
}
