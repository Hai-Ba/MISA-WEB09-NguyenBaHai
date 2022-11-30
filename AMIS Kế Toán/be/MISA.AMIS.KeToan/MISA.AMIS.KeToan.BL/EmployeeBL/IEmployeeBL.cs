using ClosedXML.Excel;
using MISA.AMIS.KeToan.Common;
using MISA.AMIS.KeToan.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.BL
{
    public interface IEmployeeBL : IBaseBL<Employee>
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
        /// API thêm mới 1 bản ghi
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <param name="employee">employee truyền vào Body</param>
        /// <returns></returns>
        public ResponseService InsertNewEmployee(Employee employee);

        /// <summary>
        /// API Sửa 1 nhân viên theo ID
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <param name="employee">employee truyền vào body để sửa</param>
        /// <param name="employeeID">Mã Guid id của nhân viên cần sửa</param>
        /// <returns></returns>
        public ResponseService UpdateAnEmployee(Employee employee, Guid employeeID);

        public byte[] ExportExcel(List<Employee> employees, int limit);
    }
}
