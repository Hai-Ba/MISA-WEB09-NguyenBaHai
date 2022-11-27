using Dapper;
using MISA.AMIS.KeToan.Common;
using MISA.AMIS.KeToan.Common.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.DL
{
    public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
    {
        /// <summary>
        /// Hàm kiểm tra trạng thái đóng mở đưuường truyền vào DB
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <param name="mySqlConnection"></param>
        private void CheckConnectionStatus(MySqlConnection mySqlConnection) 
        {
            if (mySqlConnection != null && mySqlConnection.State != ConnectionState.Open) 
            {
                mySqlConnection.Open();
            }
            else if (mySqlConnection != null && mySqlConnection.State == ConnectionState.Open)
            {
                mySqlConnection.Close();
            }
        }

        /// <summary>
        /// API check mã trùng của nhân viên
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns>Số lượng trùng</returns>
        public int CheckDuplicateCode(string employeeCode)
        {
            //Chuan bi Store Proc
            string storeProcName = String.Format(Procedure.CHECK_DUPLICATE, typeof(Employee).Name);

            //Chuan bi tham so dau vao
            var parameters = new DynamicParameters();
            parameters.Add("@EmployeeCode", employeeCode);

            //Thuc hien goi vao DB
            int numberOfDuplicateRecord = 0;
            using (var mySqlConnection = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                CheckConnectionStatus(mySqlConnection);
                numberOfDuplicateRecord = mySqlConnection.QueryFirstOrDefault<int>(storeProcName, parameters, commandType: CommandType.StoredProcedure);
                CheckConnectionStatus(mySqlConnection);

            }
            return numberOfDuplicateRecord;
        }

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
            

            //Chuan bi Store Proc
            string storeProcName = String.Format(Procedure.GET_PAGING, typeof(Employee).Name);

            //Chuan bi tham so dau vao
            int offset = (pageNumber - 1) * limit;
            var parameters = new DynamicParameters();
            parameters.Add("@Filter", keyword);
            parameters.Add("@Limit", limit);
            parameters.Add("@Offset", offset);


            //Thuc hien goi vao DB
            var employees = new List<Employee>();
            using (var mySqlConnection = new MySqlConnection(DatabaseConfig.ConnectionString)) 
            {
                CheckConnectionStatus(mySqlConnection);
                employees = (List<Employee>)mySqlConnection.Query<Employee>(storeProcName, parameters, commandType: CommandType.StoredProcedure);
                CheckConnectionStatus(mySqlConnection);

            }
            return employees;

        }

        /// <summary>
        /// API lấy mã nhân viên lớn nhất cộng thêm 1
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <returns></returns>
        public string GetNewEmployeeCode()
        {
            //Chuan bi Store Proc
            string storeProcName = String.Format(Procedure.GET_BIGGEST_ID, typeof(Employee).Name);

            //Thuc hien goi vao DB
            string employeeCode = "";

            using (var mySqlConnection = new MySqlConnection(DatabaseConfig.ConnectionString)) 
            {
                CheckConnectionStatus(mySqlConnection);
                employeeCode = mySqlConnection.QueryFirstOrDefault<String>(storeProcName, commandType: CommandType.StoredProcedure);
                CheckConnectionStatus(mySqlConnection);
            }

            return employeeCode;
        }

        /// <summary>
        /// API thêm mới 1 nhân viên
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <param name="employee">nhân viên truyền vào Body</param>
        /// <returns></returns>
        public Guid InsertNewEmployee(Employee employee)
        {

            //Chuan bi Store Proc
            string storeProcName = String.Format(Procedure.INSERT, typeof(Employee).Name);

            //Chuan bi tham so truyen vao
            var newEmployeeID = Guid.NewGuid();
            var parameters = new DynamicParameters();
            parameters.Add("@EmployeeID", newEmployeeID);
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

            int numberOfAffectedRow = 0;
            using (var mySqlConnection = new MySqlConnection(DatabaseConfig.ConnectionString)) 
            {

                CheckConnectionStatus(mySqlConnection);
                using (MySqlTransaction transaction = mySqlConnection.BeginTransaction())
                {
                    try
                    {
                        //Thuc hien goi vao DB
                        numberOfAffectedRow = mySqlConnection.Execute(storeProcName, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception ex2)
                        {
                            Console.WriteLine(ex2.Message);
                        }
                    }
                }
                CheckConnectionStatus(mySqlConnection);
            }
            if (numberOfAffectedRow > 0) 
            {
                return newEmployeeID;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// API Sửa 1 nhân viên theo ID
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022
        /// </summary>
        /// <param name="employee">employee truyền vào body để sửa</param>
        /// <param name="employeeID">Mã Guid id của nhân viên cần sửa</param>
        /// <returns></returns>
        public int UpdateAnEmployee(Employee employee, Guid employeeID)
        {
            //Chuan bi Store Proc
            string storeProcName = String.Format(Procedure.UPDATE, typeof(Employee).Name);

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

            int numberOfAffectedRows = 0;
            using (var mySqlConnection = new MySqlConnection(DatabaseConfig.ConnectionString)) 
            {
                CheckConnectionStatus(mySqlConnection);
                using (MySqlTransaction transaction = mySqlConnection.BeginTransaction())
                {
                    try
                    {
                        //Thuc hien goi vao DB
                        numberOfAffectedRows = mySqlConnection.Execute(storeProcName, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception ex2)
                        {
                            Console.WriteLine(ex2.Message);
                        }
                    }
                }
                CheckConnectionStatus(mySqlConnection);
            }
            return numberOfAffectedRows;
        }
    }
}
