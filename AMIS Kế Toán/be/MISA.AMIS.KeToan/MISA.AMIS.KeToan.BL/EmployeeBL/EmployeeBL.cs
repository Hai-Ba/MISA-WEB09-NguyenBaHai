using Microsoft.AspNetCore.Http;
using MISA.AMIS.KeToan.Common;
using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.Common.Entities.DTO;
using MISA.AMIS.KeToan.Common.Enums;
using MISA.AMIS.KeToan.DL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MISA.AMIS.KeToan.Common.CustomAttributes;
using System.Text.RegularExpressions;
using System.Numerics;

namespace MISA.AMIS.KeToan.BL
{
    public class EmployeeBL : BaseBL<Employee>, IEmployeeBL
    {
        #region Fields
        private IEmployeeDL _employeeDL;
        #endregion

        #region Constructors
        public EmployeeBL(IEmployeeDL employeeDL) : base(employeeDL)
        {
            _employeeDL = employeeDL;
        }

        #endregion

        #region Methods
        /// <summary>
        /// API lấy danh sách nhân viên theo phân trang
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <param name="keyword">Lọc dữ liệu theo tên, sdt, mã NV</param>
        /// <param name="limit">Giới hạn bản ghi 1 trang</param>
        /// <param name="pageNumber">Trang số</param>
        public List<Employee> GetEmployeeByPaging(string? keyword, int limit, int pageNumber)
        {
            return _employeeDL.GetEmployeeByPaging(keyword, limit, pageNumber);
        }

        /// <summary>
        /// API lấy mã nhân viên lớn nhất cộng thêm 1
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <returns></returns>
        public string GetNewEmployeeCode()
        {
            return _employeeDL.GetNewEmployeeCode();
        }

        /// <summary>
        /// API thêm mới 1 bản ghi
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <param name="employee">employee truyền vào Body</param>
        /// <returns></returns>
        public ResponseService InsertNewEmployee(Employee employee)
        {
            //Validate Duplicate code
            if (employee.EmployeeCode != null && _employeeDL.CheckDuplicateCode(employee.EmployeeCode) != 0)// Ma bi trung
            {
                return new ResponseService(false, new ErrorResult
                {
                    ErrorCode = Exceptions.DuplicateCode,
                    DevMsg = Resources.DevMsg_DuplicateCode,
                    UserMsg = Resources.UserMsg_DuplicateCode,
                    MoreInfo = Resources.MoreInfo_DuplicateCode,
                    //TraceId = HttpContext.TraceIdentifier,
                });
            }
            else
            {
                ResponseService employeeValidated = ValidateData(employee);
                if (employeeValidated.IsSuccess)
                {
                    Guid idEmployee = _employeeDL.InsertNewEmployee(employee);
                    if (idEmployee != Guid.Empty) 
                    {
                        return new ResponseService(true, idEmployee);
                    }
                    //Trường hợp thêm không thành công sinh ra lỗi Ngoaij le
                    return new ResponseService(false,
                        new ErrorResult(Exceptions.Exception, Resources.UserMsg_Exception, Resources.DevMsg_Exception));
                }
                return employeeValidated;
            }
        }

        /// <summary>
        /// API Sửa 1 nhân viên theo ID
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <param name="employee">employee truyền vào body để sửa</param>
        /// <param name="employeeID">Mã Guid id của nhân viên cần sửa</param>
        /// <returns></returns>
        public ResponseService UpdateAnEmployee(Employee employee, Guid employeeID)
        {
            ResponseService employeeValidated = ValidateData(employee);
            if (employeeValidated.IsSuccess)
            {
                int rowAffected = _employeeDL.UpdateAnEmployee(employee, employeeID);
                if (rowAffected > 0)
                {
                    return new ResponseService(true, rowAffected);
                }
                //Trường hợp thêm không thành công sinh ra lỗi Ngoaij le
                return new ResponseService(false,
                    new ErrorResult(Exceptions.DuplicateCode, Resources.UserMsg_DuplicateCode, Resources.DevMsg_DuplicateCode));
            }
            return employeeValidated;
            
        }

        /// <summary>
        /// Hàm validate backend
        /// Nguyễn Bá Hải
        /// 20/11/2022
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private ResponseService ValidateData(Employee employee)
        {
            var validateProperties = employee.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(MyAttributes)));
            foreach (PropertyInfo prop in validateProperties)
            {
                //Lấy giá trị prop truyền lên
                var propVal = prop.GetValue(employee);
                var propName = prop.Name;
                var attr = prop.GetCustomAttributes(typeof(MyAttributes), true).FirstOrDefault();
                if ((attr as MyAttributes).KeyProp)
                {
                    //Validate cho key prop
                }
                if ((attr as MyAttributes).RequiredProp)
                {
                    //Validate cho require prop
                    if (propVal == null || propVal.ToString().Trim() == "")
                    {
                        return new ResponseService(false, 
                            new ErrorResult(Exceptions.MissingField, Resources.UserMsg_MissingInput, Resources.DevMsg_MissingInput));
                    }
                }
                if ((attr as MyAttributes).ValidCode)
                {
                    //Validate cho Age
                    if (propVal != null)
                    {
                        if (CodeValidation(propVal.ToString()) == false)
                        {
                            return new ResponseService(false,
                            new ErrorResult(Exceptions.InvalidData, Resources.UserMsg_WrongInput, Resources.DevMsg_WrongInput));
                        }
                    }
                }
                if ((attr as MyAttributes).ValidAge)
                {
                    //Validate cho Age
                    if (propVal != null) 
                    {
                        if (AgeValidation(propVal) == false) 
                        {
                            return new ResponseService(false,
                            new ErrorResult(Exceptions.InvalidData, Resources.UserMsg_WrongInput, Resources.DevMsg_WrongInput));
                        }
                    }
                }
                if ((attr as MyAttributes).ValidDate)
                {
                    //Validate cho Date
                    if (propVal != null)
                    {
                        if (DateValidation(propVal) == false)
                        {
                            return new ResponseService(false,
                            new ErrorResult(Exceptions.InvalidData, Resources.UserMsg_WrongInput, Resources.DevMsg_WrongInput));
                        }
                    }
                }
                if ((attr as MyAttributes).ValidEmail)
                {
                    //Validate cho Email
                    if (propVal != null)
                    {
                        if (EmailValidation(propVal.ToString()) == false)
                        {
                            return new ResponseService(false,
                            new ErrorResult(Exceptions.InvalidData, Resources.UserMsg_WrongInput, Resources.DevMsg_WrongInput));
                        }
                    }
                }
                if ((attr as MyAttributes).ValidPhone)
                {
                    //Validate cho Phone Number
                    if (propVal != null)
                    {
                        if (PhoneValidation(propVal.ToString()) == false)
                        {
                            return new ResponseService(false,
                            new ErrorResult(Exceptions.InvalidData, Resources.UserMsg_WrongInput, Resources.DevMsg_WrongInput));
                        }
                    }
                }
                if ((attr as MyAttributes).ValidLengthNumber15)
                {
                    //Validate cho Max 15 Number
                    if (propVal != null)
                    {
                        if (Max15NumberValidation(propVal.ToString()) == false)
                        {
                            return new ResponseService(false,
                            new ErrorResult(Exceptions.InvalidData, Resources.UserMsg_WrongInput, Resources.DevMsg_WrongInput));
                        }
                    }
                }
            }
            return new ResponseService(true, employee);
        }

        /// <summary>
        /// Hàm check mã code
        /// Nguyễn Bá Hải-21/11/2022
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool CodeValidation(string? code) 
        {
            string pattern = @"(^[N][V]\d{5,6}$)";
            Regex codeRG = new Regex(pattern);
            if (code != null) 
            {
                return codeRG.IsMatch(code);
            }
            return false;
        }

        /// <summary>
        /// Hàm check tuổi khi input ngày
        /// Nguyễn Bá Hải-21/11/2022
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool AgeValidation(object? date)
        {
            if (date != null) 
            {
                var age = DateTime.Today.Year - Convert.ToDateTime(date).Year;
                return (age <= 50 && age >= 18);
            }
            return true;
        }

        /// <summary>
        /// Hàm check mã ngày ko lớn hơn ngày hiện tại
        /// Nguyễn Bá Hải-21/11/2022
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool DateValidation(object? date)
        {
            if (date != null)
            {
                return Convert.ToDateTime(date) <= DateTime.Now; ;
            }
            return true;
        }

        /// <summary>
        /// Hàm check mã email
        /// Nguyễn Bá Hải-21/11/2022
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool EmailValidation(string? email)
        {
            Regex codeRG = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
            if (email != null) 
            {
                return codeRG.IsMatch(email);
            }
            return true;
        }

        /// <summary>
        /// Hàm check sdt
        /// Nguyễn Bá Hải-21/11/2022
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public bool PhoneValidation(string? phone)
        {
            Regex codeRG = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
            if (phone != null)
            {
                return codeRG.IsMatch(phone);
            }
            return true;
        }

        /// <summary>
        /// Hàm check Số in put là ko lớn hơn 15
        /// Nguyễn Bá Hải-21/11/2022
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool Max15NumberValidation(string? number)
        {
            Regex codeRG = new Regex("^\\d{7,15}$");
            if (number != null)
            {
                return codeRG.IsMatch(number);
            }
            return true;
        }

        #endregion
    }
}
