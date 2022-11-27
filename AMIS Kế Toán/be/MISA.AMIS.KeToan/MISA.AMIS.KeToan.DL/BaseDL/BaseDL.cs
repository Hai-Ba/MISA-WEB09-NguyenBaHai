using Dapper;
using MISA.AMIS.KeToan.Common;
using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.DL
{
    public class BaseDL<T> : IBaseDL<T>
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
        /// API xóa nhiều bản ghi
        /// Author: Nguyễn Bá Hải
        /// Date:3/11/2022
        /// </summary>
        /// <param name="listID"></param>
        /// <returns></returns>
        public void DeleteMany(string[] listID)
        {
            //Chuan bi Store Proc
            string storeProcName = String.Format(Procedure.DELETE_MANY, typeof(T).Name);

            //Chuan bi tham so
            var parameters = new DynamicParameters();
            List<String> formatListString = new List<String>();
            if (listID != null)
            {
                foreach (string id in listID)
                {
                    formatListString.Add($"\"{id}\"");
                }
            }
            string list = "";
            if (formatListString.Count > 0)
            {
                list = $"{string.Join(", ", formatListString)}";
            }
            parameters.Add("@ArrayID", list);

            //Thực hiện gọi vào DB 
            //int numberOfAffectedRows = 0;
            using (var mySqlConnection = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                //numberOfAffectedRows = mySqlConnection.Execute(storeProcName, parameters, commandType: CommandType.StoredProcedure);
                CheckConnectionStatus(mySqlConnection);
                using (MySqlTransaction transaction = mySqlConnection.BeginTransaction())
                {
                    try
                    {
                        mySqlConnection.Execute(storeProcName, parameters, commandType: System.Data.CommandType.StoredProcedure, transaction: transaction);
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
                //numberOfAffectedRows = mySqlConnection.Execute(storeProcName, parameters, commandType: CommandType.StoredProcedure);
            }
            //return numberOfAffectedRows;
        }

        /// <summary>
        /// API xóa 1 bản ghi theo ID
        /// Author: Nguyễn Bá Hải
        /// Date:3/11/2022
        /// </summary>
        /// <param name="recordID">ID của bản ghi muốn xóa</param>
        /// <returns></returns>
        public int DeleteRecordByID(Guid recordID)
        {
            //Chuan bi Store Proc
            string storeProcName = String.Format(Procedure.DELETE, typeof(T).Name);

            //Chuan bi tham so
            var parameters = new DynamicParameters();
            parameters.Add($"{typeof(T).Name}ID", recordID);

            //Thực hiện gọi vào DB muộn nhất và hủy gọi vào sớm nhất
            int numberOfAffectedRows = 0;
            using (var mySqlConnection = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                CheckConnectionStatus(mySqlConnection);
                using (MySqlTransaction transaction = mySqlConnection.BeginTransaction())
                {
                    try
                    {
                        numberOfAffectedRows = mySqlConnection.Execute(storeProcName, parameters, commandType: System.Data.CommandType.StoredProcedure, transaction: transaction);
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

        /// <summary>
        /// Lấy danh sách tất cả bản ghi
        /// Author: Nguyễn Bá Hải
        /// Date: 10/11/2022
        /// </summary>
        /// <returns>Danh sách tất cả bản ghi</returns>
        public IEnumerable<T> GetAllRecords(string? keyword)
        {
            //Chuan bi Store Proc
            string storeProcName = String.Format(Procedure.GET_ALL, typeof(T).Name);

            //Chuan bi tham so
            var parameters = new DynamicParameters();
            parameters.Add("@Filter", keyword);

            var records = new List<T>();
            //Thực hiện gọi vào DB muộn nhất và hủy gọi vào sớm nhất
            using (var mySqlConnection = new MySqlConnection(DatabaseConfig.ConnectionString)) 
            {
                CheckConnectionStatus(mySqlConnection);
                records = (List<T>)mySqlConnection.Query<T>(storeProcName, parameters, commandType: CommandType.StoredProcedure);
                CheckConnectionStatus(mySqlConnection);
            }
            return records;
        }

        /// <summary>
        /// Lấy thông tin 1 bản ghi theo id
        /// Author: Nguyễn Bá Hải
        /// Date: 10/11/2022
        /// </summary>
        /// <param name="recordID">ID bản ghi muốn lấy</param>
        /// <returns>Thông tin 1 bản ghi</returns>
        public T GetRecordByID(Guid recordID)
        {
            //Chuan bi Store Proc
            string storeProcName = String.Format(Procedure.GET_BY_ID, typeof(T).Name);

            //Chuan bi tham so
            var parameters = new DynamicParameters();
            parameters.Add($"{typeof(T).Name}ID", recordID);

            //Thực hiện gọi vào DB muộn nhất và hủy gọi vào sớm nhất
            using (var mySqlConnection = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                CheckConnectionStatus(mySqlConnection);
                T records = mySqlConnection.QueryFirstOrDefault<T>(storeProcName, parameters, commandType: CommandType.StoredProcedure);
                CheckConnectionStatus(mySqlConnection);
                return records;
            }
        }
    }
}
