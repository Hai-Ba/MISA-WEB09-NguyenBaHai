using MISA.AMIS.KeToan.Common.CustomAttributes;
using MISA.AMIS.KeToan.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace MISA.AMIS.KeToan.Common.Entities
{
    /// <summary>
    /// Nhân viên
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// ID nhân viên
        /// </summary>
        [MyAttributes(KeyProp = true)]
        public Guid EmployeeID   { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        //[Required(ErrorMessage = "e004")]
        [MyAttributes(RequiredProp = true, ValidCode = true)]
        public string? EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        //[Required(ErrorMessage = "e005")]
        [MyAttributes(RequiredProp = true)]
        public string? EmployeeName { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        [MyAttributes(ValidAge = true)]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// Số Chứng minh nhân dân
        /// </summary>
        [MyAttributes(ValidLengthNumber15 = true)]
        public string? IdentityNumber { get; set; }

        /// <summary>
        /// Ngày cấp
        /// </summary>
        [MyAttributes(ValidDate = true)]
        public DateTime? IdentityDate { get; set; }

        /// <summary>
        /// Nơi cấp
        /// </summary>
        public string? IdentityPlace { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        //[MaxLength(20, ErrorMessage = "e007")]
        //[Phone(ErrorMessage = "e008")]
        [MyAttributes(ValidPhone = true)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Số điện thoại cố định
        /// </summary>
        //[MaxLength(20, ErrorMessage = "e007")]
        //[Phone(ErrorMessage = "e008")]
        [MyAttributes(ValidPhone = true)]
        public string? TelephoneNumber { get; set; }

        /// <summary>
        /// Mail cá nhân
        /// </summary>
        //[EmailAddress(ErrorMessage = "e009")]
        [MyAttributes(ValidEmail = true)]
        public string? Email { get; set; }

        /// <summary>
        /// Số tài khoản ngân hàng
        /// </summary>
        [MyAttributes(ValidLengthNumber15 = true)]
        public string? BankAccountNumber { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string? BankName { get; set; }

        /// <summary>
        /// Chi nhánh ngân hàng
        /// </summary>
        public string? BankProvinceName { get; set; }

        /// <summary>
        /// ID phòng ban(Khóa ngoại)
        /// </summary>
        //[Required(ErrorMessage = "e006")]
        [MyAttributes(RequiredProp = true)]
        public Guid? DepartmentID { get; set; }

        /// <summary>
        /// Ten phòng ban
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// Tên vị trí
        /// </summary>
        public string? PositionName { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        public string? ModifiedBy { get; set; }


    }
}
