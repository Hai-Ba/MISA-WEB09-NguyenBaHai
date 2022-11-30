using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.KeToan.BL;
using MISA.AMIS.KeToan.Common;
using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.Common.Entities.DTO;
using MISA.AMIS.KeToan.Common.Enums;
using MISA.AMIS.KeToan.DL;
using MySqlConnector;
using System.Data;
using System.Runtime.Versioning;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.IO;

namespace MISA.AMIS.KeToan.API.Controllers
{
    [ApiController]
    public class EmployeesController : BasesController<Employee>
    {
        #region Field
        private IEmployeeBL _employeeBL;
        #endregion

        #region Constructor
        public EmployeesController(IEmployeeBL employeeBL) : base(employeeBL)
        {
            _employeeBL = employeeBL;
        }
        #endregion

        /// <summary>
        /// API lấy danh sách bản ghi theo phân trang
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <param name="keyword">Lọc dữ liệu theo tên, sdt, mã NV</param>
        /// <param name="limit">Giới hạn bản ghi 1 trang</param>
        /// <param name="pageNumber">Trang số</param>
        /// <returns></returns>
        [HttpGet("filter")]
        public IActionResult GetEmployeeByPaging(
            [FromQuery] string? keyword,
            [FromQuery] int limit,
            [FromQuery] int pageNumber
            )
        {
            try
            {
                //Thuc hien goi vao BL
                var employees = _employeeBL.GetEmployeeByPaging(keyword, limit, pageNumber);
                return StatusCode(StatusCodes.Status200OK, employees);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = Exceptions.Exception,
                    DevMsg = Resources.DevMsg_Exception,
                    UserMsg = Resources.UserMsg_Exception,
                    MoreInfo = Resources.MoreInfo_Exception,
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
        }

        /// <summary>
        /// API lấy mã nhân viên lớn nhất cộng thêm 1
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <returns></returns>
        [HttpGet("NewEmployeeCode")]
        public IActionResult GetNewEmployeeCode()
        {
            try
            {
                //Thuc hien goi vao BL
                string employeeCode = _employeeBL.GetNewEmployeeCode();

                if (employeeCode != "")
                {
                    string newEmployeeCode;
                    newEmployeeCode = "NV" + (Int64.Parse(employeeCode.Substring(2)) + 1).ToString();
                    return StatusCode(StatusCodes.Status200OK, newEmployeeCode);
                }
                return StatusCode(StatusCodes.Status200OK, "NV000001");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = Exceptions.Exception,
                    DevMsg = Resources.DevMsg_Exception,
                    UserMsg = Resources.UserMsg_Exception,
                    MoreInfo = Resources.MoreInfo_Exception,
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
        }

        /// <summary>
        /// API thêm mới 1 bản ghi
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <param name="employee">employee truyền vào Body</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InsertNewEmployee([FromBody] Employee employee)
        {
            try
            {
                var blReturn = _employeeBL.InsertNewEmployee(employee);
                if (blReturn.IsSuccess)
                {
                    return StatusCode(StatusCodes.Status201Created, blReturn.Data);
                }
                return StatusCode(StatusCodes.Status400BadRequest, blReturn.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    DevMsg = Resources.DevMsg_Exception,
                    UserMsg = Resources.UserMsg_Exception,
                    MoreInfo = Resources.MoreInfo_Exception,
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
        }

        /// <summary>
        /// API Sửa 1 nhân viên theo ID
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <param name="employee">employee truyền vào body để sửa</param>
        /// <param name="employeeId">Mã Guid id của nhân viên cần sửa</param>
        /// <returns></returns>
        [HttpPut("{employeeID}")]
        public IActionResult UpdateAnEmployee([FromBody] Employee employee, [FromRoute] Guid employeeID)
        {
            try
            {
                var blReturn = _employeeBL.UpdateAnEmployee(employee, employeeID);

                if (blReturn.IsSuccess)
                {
                    return StatusCode(StatusCodes.Status201Created, blReturn.Data);
                }
                return StatusCode(StatusCodes.Status400BadRequest, blReturn.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = Exceptions.Exception,
                    DevMsg = Resources.DevMsg_Exception,
                    UserMsg = Resources.UserMsg_Exception,
                    MoreInfo = Resources.MoreInfo_Exception,
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
        }

        /// <summary>
        /// API xuất file Excel
        /// Author: Nguyễn Bá Hải
        /// Date:22/11/2022
        /// </summary>
        /// <param name="listData"></param>
        /// <returns></returns>
        [HttpGet("ExportExcel")]
        public IActionResult ExportExcel(
            [FromQuery] string? keyword,
            [FromQuery] int limit,
            [FromQuery] int pageNumber
            )
        {
            //Thuc hien goi vao BL
            try
            {
                var employees = _employeeBL.GetEmployeeByPaging(keyword, limit, pageNumber);
                if (employees != null && employees.Count > 0)
                {
                    byte[] content = _employeeBL.ExportExcel(employees, limit);
                    return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "employees.xlsx");
                }
                return StatusCode(StatusCodes.Status404NotFound, new ErrorResult(
                    Exceptions.NoDataInPage, Resources.UserMsg_NoDataInPage, Resources.DevMsg_NoDataInPage
                    ));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = Exceptions.Exception,
                    DevMsg = Resources.DevMsg_Exception,
                    UserMsg = Resources.UserMsg_Exception,
                    MoreInfo = Resources.MoreInfo_Exception,
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
        }
    }
}
