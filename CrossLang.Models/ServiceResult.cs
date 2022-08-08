using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.Models
{
    /// <summary>
    /// Kết quả trả về của service
    /// </summary>
    /// CREATEDBY: VMHOANG (25/07/2021)
    public class ServiceResult
    {
        #region Properties
        public bool SuccessState { get; set; }
        public object Data { get; set; }
        public string UserMsg { get; set; }
        public string DevMsg { get; set; }
        public int Code { get; set; }
        #endregion
    }
}
