using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.Models
{
    /// <summary>
    /// Thông tin lỗi trả về của API
    /// </summary>
    /// CREATEDBY: VMHOANG (28/07/2021)

    public class ApiReturn
    {
        #region Properties
        public bool Success { get; set; }
        public dynamic Data { get; set; }
        public string DevMsg { get; set; }
        public string UserMsg { get; set; }
        public string ErrorCode { get; set; }
        public string MoreInfo { get; set; }
        public string TraceId { get; set; }
        #endregion
    }
}
