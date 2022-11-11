using Dapper;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.KeToan.API.Entities;
using MySqlConnector;
using System.Data;

namespace MISA.AMIS.KeToan.API.Controllers
{
    [Route("api/v1/[controller]")] //Attribute
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        /// <summary>
        /// API Lấy danh sách tất cả bản ghi
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllEmployees([FromQuery] string? keyword)
        {
            try
            {
                //Khoi tao ket noi toi DB
                string connectionString = "Server=localhost;Port=3306;Database=misaamisketoan.nbhhai.gpbl_development;Uid=root;Pwd=hai12122001;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Chuan bi Store Proc
                string storeProcName = "Proc_employee_GetAllEmployees";

                //Chuan bi tham so
                var parameters = new DynamicParameters();
                parameters.Add("@Filter", keyword);

                //Thuc hien goi vao DB
                var employees = mySqlConnection.Query(storeProcName,parameters,commandType: CommandType.StoredProcedure);

                //Xu li ket qua tra ve
                if (employees != null)
                {
                    return StatusCode(StatusCodes.Status200OK, employees);//Trả về mã 200 và danh sách nhân viên
                }
                return StatusCode(StatusCodes.Status200OK, new List<Employee>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Catched an exception.",
                    UserMsg = "Có lỗi xảy ra! Vui lòng liên hệ với MISA.",
                    MoreInfo = "SMT HERE",
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
        }

        /// <summary>
        /// API Lấy ra bản ghi theo ID
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022 
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        [HttpGet("{employeeID}")]
        public IActionResult GetEmployeeByID([FromRoute] Guid employeeID) 
        {
            try
            {
                //Khoi tao ket noi toi DB
                string connectionString = "Server=localhost;Port=3306;Database=misaamisketoan.nbhhai.gpbl_development;Uid=root;Pwd=hai12122001;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Chuan bi Store Proc
                string storeProcName = "Proc_employee_GetEmployeeByID";

                //Chuan bi tham so dau vao
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employeeID);

                //Thuc hien goi vao DB
                var employee = mySqlConnection.QueryFirstOrDefault<Employee>(storeProcName, parameters, commandType: CommandType.StoredProcedure);

                if (employee != null)
                {
                    return StatusCode(StatusCodes.Status200OK, employee);
                }
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Catched an exception.",
                    UserMsg = "Có lỗi xảy ra! Vui lòng liên hệ với MISA.",
                    MoreInfo = "SMT HERE",
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
        }

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
                //Khoi tao ket noi toi DB
                string connectionString = "Server=localhost;Port=3306;Database=misaamisketoan.nbhhai.gpbl_development;Uid=root;Pwd=hai12122001;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Chuan bi Store Proc
                string storeProcName = "Proc_employee_GetByFiltering";

                //Chuan bi tham so dau vao
                int offset = (pageNumber - 1) * limit;
                var parameters = new DynamicParameters();
                parameters.Add("@Filter", keyword);
                parameters.Add("@Limit", limit);
                parameters.Add("@Offset", offset);

                //Thuc hien goi vao DB
                var employees = mySqlConnection.Query(storeProcName, parameters, commandType: CommandType.StoredProcedure);

                if (employees != null)
                {
                    return StatusCode(StatusCodes.Status200OK, employees);
                }
                return StatusCode(StatusCodes.Status200OK, new List<Employee>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Catched an exception.",
                    UserMsg = "Có lỗi xảy ra! Vui lòng liên hệ với MISA.",
                    MoreInfo = "SMT HERE",
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
                //Khoi tao ket noi toi DB
                string connectionString = "Server=localhost;Port=3306;Database=misaamisketoan.nbhhai.gpbl_development;Uid=root;Pwd=hai12122001;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Chuan bi Store Proc
                string storeProcName = "Proc_employee_GetBiggestID";

                //Thuc hien goi vao DB
                string employeeCode = mySqlConnection.QueryFirstOrDefault<String>(storeProcName, commandType: CommandType.StoredProcedure);

                if (employeeCode != "")
                {
                    string newEmployeeCode;
                    newEmployeeCode = "NV" + (Int64.Parse(employeeCode.Substring(2)) + 1).ToString();
                    return StatusCode(StatusCodes.Status200OK, newEmployeeCode);
                }
                return StatusCode(StatusCodes.Status200OK, "NV001");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Catched an exception.",
                    UserMsg = "Có lỗi xảy ra! Vui lòng liên hệ với MISA.",
                    MoreInfo = "SMT HERE",
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
        public IActionResult PostNewEmployee([FromBody] Employee employee) 
        {
            try
            {
                //Khoi tao ket noi toi DB
                string connectionString = "Server=localhost;Port=3306;Database=misaamisketoan.nbhhai.gpbl_development;Uid=root;Pwd=hai12122001;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Chuan bi Store Proc
                string storeProcName = "Proc_employee_PostEmployee";

                //Chuan bi tham so truyen vao
                var newEmployeeID = Guid.NewGuid();
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeCode", employee.EmployeeCode);
                parameters.Add("@EmployeeName", employee.EmployeeName);
                parameters.Add("@DateOfBirth", employee.DateOfBirth);
                parameters.Add("@Gender", employee.Gender);
                parameters.Add("@IdentityNumber", employee.IdentityNumber);
                parameters.Add("@IdentityDate", employee.IdentityDate);
                parameters.Add("@IdentityPlace", employee.IdentityPlace);
                parameters.Add("@Address", employee.Address);
                parameters.Add("@PhoneNumber", employee.PhoneNumber);
                parameters.Add("@TelephoneNumber", employee.TelephoneNumber);
                parameters.Add("@Email", employee.Email);
                parameters.Add("@BankAccountNumber", employee.BankAccountNumber);
                parameters.Add("@BankName", employee.BankName);
                parameters.Add("@BankProvinceName", employee.BankProvinceName);
                parameters.Add("@DepartmentID", employee.DepartmentID);
                parameters.Add("@PositionName", employee.PositionName);

                //Thuc hien goi vao DB
                var postedEmployee = mySqlConnection.QueryFirstOrDefault<Employee>(storeProcName, parameters, commandType: CommandType.StoredProcedure);

                if (postedEmployee != null)
                {
                    return StatusCode(StatusCodes.Status201Created, postedEmployee.EmployeeID);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 2,
                    DevMsg = "Database insert failed.",
                    UserMsg = "Thêm mới nhân viên thất bại.",
                    MoreInfo = "SMT HERE",
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Catched an exception.",
                    UserMsg = "Có lỗi xảy ra! Vui lòng liên hệ với MISA.",
                    MoreInfo = "SMT HERE",
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
        public IActionResult ModifyAnEmployee([FromBody] Employee employee, [FromRoute] Guid employeeID) 
        {
            try
            {
                //Khoi tao ket noi toi DB
                string connectionString = "Server=localhost;Port=3306;Database=misaamisketoan.nbhhai.gpbl_development;Uid=root;Pwd=hai12122001;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Chuan bi Store Proc
                string storeProcName = "Proc_employee_ModifyEmployee";

                //Chuan bi tham so truyen vao
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employeeID);
                parameters.Add("@EmployeeCode", employee.EmployeeCode);
                parameters.Add("@EmployeeName", employee.EmployeeName);
                parameters.Add("@DateOfBirth", employee.DateOfBirth);
                parameters.Add("@Gender", employee.Gender);
                parameters.Add("@IdentityNumber", employee.IdentityNumber);
                parameters.Add("@IdentityDate", employee.IdentityDate);
                parameters.Add("@IdentityPlace", employee.IdentityPlace);
                parameters.Add("@Address", employee.Address);
                parameters.Add("@PhoneNumber", employee.PhoneNumber);
                parameters.Add("@TelephoneNumber", employee.TelephoneNumber);
                parameters.Add("@Email", employee.Email);
                parameters.Add("@BankAccountNumber", employee.BankAccountNumber);
                parameters.Add("@BankName", employee.BankName);
                parameters.Add("@BankProvinceName", employee.BankProvinceName);
                parameters.Add("@DepartmentID", employee.DepartmentID);
                parameters.Add("@PositionName", employee.PositionName);

                //Thuc hien goi vao DB
                var numberOfAffectedRows = mySqlConnection.Execute(storeProcName, parameters, commandType: CommandType.StoredProcedure);

                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status201Created, employeeID);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 3,
                    DevMsg = "Database modified failed.",
                    UserMsg = "Sửa mới nhân viên thất bại.",
                    MoreInfo = "SMT HERE",
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Catched an exception.",
                    UserMsg = "Có lỗi xảy ra! Vui lòng liên hệ với MISA.",
                    MoreInfo = "SMT HERE",
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
        }

        /// <summary>
        /// API xóa 1 nhân viên theo ID
        /// Author: Nguyễn Bá Hải
        /// Date:3/11/2022
        /// </summary>
        /// <param name="employeeID">ID của nhân viên muốn xóa</param>
        /// <returns></returns>
        [HttpDelete("{employeeID}")]
        public IActionResult DeleteAnEmployee([FromRoute] Guid employeeID) 
        {
            try
            {
                //Khoi tao ket noi toi DB
                string connectionString = "Server=localhost;Port=3306;Database=misaamisketoan.nbhhai.gpbl_development;Uid=root;Pwd=hai12122001;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Chuan bi Store Proc
                string storeProcName = "Proc_employee_DeleteEmployeeByID";

                //Chuan bi tham so truyen vao
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employeeID);

                //Thuc hien goi vao DB
                var numberOfAffectedRows = mySqlConnection.Execute(storeProcName, parameters, commandType: CommandType.StoredProcedure);

                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, employeeID);
                }
                return StatusCode(StatusCodes.Status404NotFound, new
                {
                    ErrorCode = 4,
                    StatusCode = 404,
                    DevMsg = "Employee ID not exist.",
                    UserMsg = "Nhân viên không tồn tại, không thể xóa.",
                    MoreInfo = "SMT HERE",
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Catched an exception.",
                    UserMsg = "Có lỗi xảy ra! Vui lòng liên hệ với MISA.",
                    MoreInfo = "SMT HERE",
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
        }
    }
}
