using MISA.AMIS.KeToan.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.Common.Entities.DTO
{
    public class ErrorResult
    {
        #region Property

        /// <summary>
        /// Định danh của mã lỗi nội bộ
        /// </summary>
        public Exceptions ErrorCode { get; set; } = Exceptions.Exception;

        /// <summary>
        /// Thông báo cho user. Không bắt buộc, tùy theo đặc thù từng dịch vụ và client tích hợp
        /// </summary>
        public string? UserMsg { get; set; }

        /// <summary>
        /// Thông báo cho Dev. Thông báo ngắn gọn để thông báo cho Dev biết vấn đề đang gặp phải
        /// </summary>
        public object? DevMsg { get; set; }

        /// <summary>
        /// Thông tin chi tiết hơn về vấn đề. Ví dụ: Đường dẫn mô tả về mã lỗi
        /// </summary>
        public string? MoreInfo { get; set; }

        /// <summary>
        /// Mã để tra cứu thông tin log: ELK, file log,...
        /// </summary>
        public string? TraceId { get; set; }

        #endregion

        #region Constructor

        public ErrorResult()
        {
        }

        public ErrorResult(Exceptions errorCode)
        {
            ErrorCode = errorCode;
        }

        public ErrorResult(Exceptions errorCode, string? userMsg, object? devMsg)
        {
            ErrorCode = errorCode;
            UserMsg = userMsg;
            DevMsg = devMsg;
        }

        public ErrorResult(Exceptions errorCode, string? userMsg, object? devMsg, string? moreInfo, string? traceId)
        {
            ErrorCode = errorCode;
            UserMsg = userMsg;
            DevMsg = devMsg;
            MoreInfo = moreInfo;
            TraceId = traceId;
        }

        #endregion
    }
}
