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
                return StatusCode(StatusCodes.Status500InternalServerError, new
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
                    using (var workbook = new XLWorkbook())
                    {
                        IXLWorksheet workSheet = workbook.Worksheets.Add("Employees");//Name Worksheet
                        var startRowData = 3;//Start from row 3
                        int count = 0;//Number of row data
                        var endRowData = startRowData + limit;//End at row 3 + limit
                        #region Tiêu đề cho file excel
                        workSheet.Range("A1:I1").Row(1).Merge();
                        workSheet.Cell("A1").Value = "DANH SÁCH NHÂN VIÊN";
                        workSheet.Range("A1").Style.Font.FontSize = 16;
                        workSheet.Range("A1").Style.Font.SetFontName("Arial");
                        workSheet.Range("A1").Style.Font.Bold = true;
                        workSheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        workSheet.Range("A2:I2").Row(1).Merge();
                        workSheet.Row(1).Height = 20.25;
                        workSheet.Row(2).Height = 21.75;
                        workSheet.Range("A3:I3").Style.Fill.SetBackgroundColor(XLColor.LightGray);
                        workSheet.Range("A3:I3").Style.Font.SetFontName("Arial");
                        workSheet.Range("A3:I3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        workSheet.Range("A3:I3").Style.Font.Bold = true;
                        workSheet.Range($"A3:I{endRowData}").Style.Alignment.WrapText = true;
                        workSheet.Range($"A3:I{endRowData}").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                        workSheet.Range($"A3:I{endRowData}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        #endregion
                        //Set column width
                        workSheet.Column("A").Width = 4.3;
                        workSheet.Column("B").Width = 15.3;
                        workSheet.Column("C").Width = 26;
                        workSheet.Column("D").Width = 12;
                        workSheet.Column("E").Width = 15.3;
                        workSheet.Column("F").Width = 26;
                        workSheet.Column("G").Width = 25;
                        workSheet.Column("H").Width = 15.3;
                        workSheet.Column("I").Width = 26;

                        workSheet.Row(startRowData).Height = 15;
                        workSheet.Row(startRowData).Style.Font.FontSize = 10;
                        workSheet.Cell(startRowData, 1).Value = "STT";
                        workSheet.Cell(startRowData, 2).Value = "Mã nhân viên";
                        workSheet.Cell(startRowData, 3).Value = "Tên nhân viên";
                        workSheet.Cell(startRowData, 4).Value = "Giới tính";
                        workSheet.Cell(startRowData, 5).Value = "Ngày sinh";
                        workSheet.Cell(startRowData, 6).Value = "Chức danh";
                        workSheet.Cell(startRowData, 7).Value = "Tên đơn vị";
                        workSheet.Cell(startRowData, 8).Value = "Số tài khoản";
                        workSheet.Cell(startRowData, 9).Value = "Tên ngân hàng";
                        foreach (var employee in employees)
                        {
                            startRowData++;
                            count++;
                            workSheet.Row(startRowData).Height = 15;
                            workSheet.Row(startRowData).Style.Font.SetFontName("Times New Roman");
                            workSheet.Cell(startRowData, 1).Value = count;
                            workSheet.Cell(startRowData, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            workSheet.Cell(startRowData, 2).Value = employee.EmployeeCode;
                            workSheet.Cell(startRowData, 3).Value = employee.EmployeeName;
                            workSheet.Cell(startRowData, 4).Value = employee.Gender;
                            if (employee.DateOfBirth != null)
                            {
                                workSheet.Cell(startRowData, 5).Value = ((DateTime)employee.DateOfBirth).ToString("dd/MM/yyyy");
                            }
                            else 
                            {
                                workSheet.Cell(startRowData, 5).Value = employee.DateOfBirth;
                            }
                            workSheet.Cell(startRowData, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            workSheet.Cell(startRowData, 6).Value = employee.PositionName;
                            workSheet.Cell(startRowData, 7).Value = employee.DepartmentName;
                            workSheet.Cell(startRowData, 8).Value = employee.BankAccountNumber;
                            workSheet.Cell(startRowData, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            workSheet.Cell(startRowData, 9).Value = employee.BankName;
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();

                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "employees.xlsx");
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK);
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
