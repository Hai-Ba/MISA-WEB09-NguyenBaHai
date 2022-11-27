using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.KeToan.BL;
using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.Common.Enums;
using MISA.AMIS.KeToan.Common;
using MISA.AMIS.KeToan.DL;
using MySqlConnector;
using System.Data;
using MISA.AMIS.KeToan.Common.Entities.DTO;

namespace MISA.AMIS.KeToan.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasesController<T> : ControllerBase
    {
        #region Field
        private IBaseBL<T> _baseBL;
        #endregion

        #region Constructor
        public BasesController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }
        #endregion

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
                //Thuc hien goi sang BL
                var records = _baseBL.GetAllRecords(keyword);

                if (records != null)
                {
                    return StatusCode(StatusCodes.Status200OK, records);//Trả về mã 200 và danh sách
                }
                return StatusCode(StatusCodes.Status200OK, new List<T>());
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
        /// API Lấy ra bản ghi theo ID
        /// Author: Nguyễn Bá Hải
        /// Date: 3/11/2022 
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        [HttpGet("{recordID}")]
        public IActionResult GetRecordByID([FromRoute] Guid recordID)
        {
            try
            {
                //Thuc hien goi sang BL
                var record = _baseBL.GetRecordByID(recordID);

                if (record != null)
                {
                    return StatusCode(StatusCodes.Status200OK, record);
                }
                return StatusCode(StatusCodes.Status404NotFound);
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
        /// API xóa 1 bản ghi theo ID
        /// Author: Nguyễn Bá Hải
        /// Date:3/11/2022
        /// </summary>
        /// <param name="recordID">ID của bản ghi muốn xóa</param>
        /// <returns></returns>
        [HttpDelete("{recordID}")]
        public IActionResult DeleteARecord([FromRoute] Guid recordID)
        {
            try
            {
                //Thuc hien goi BL
                var numberOfAffectedRows = _baseBL.DeleteRecordByID(recordID);

                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, recordID);
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
                    ErrorCode = Exceptions.Exception,
                    DevMsg = Resources.DevMsg_Exception,
                    UserMsg = Resources.UserMsg_Exception,
                    MoreInfo = Resources.MoreInfo_Exception,
                    TraceId = HttpContext.TraceIdentifier,
                });
            }
        }

        /// <summary>
        /// API xóa nhiều bản ghi
        /// Author: Nguyễn Bá Hải
        /// Date:3/11/2022
        /// </summary>
        /// <param name="listID"></param>
        /// <returns></returns>
        [HttpDelete("DeleteMany")]
        public IActionResult DeleteMany([FromBody] string[] listID) 
            {
            try
            {
                _baseBL.DeleteMany(listID);
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
