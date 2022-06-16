using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Enums
{
    /// <summary>
    /// Mã code của hệ thống
    /// </summary>
    /// CREATEDBY: VMHOANG (25/07/2021)
    public enum MISACode
    {
        IsValid = 100,
        NotValid = 900,
        Success = 200,
        DBUnexpectedError = 998,
        UnexpectedError = 999
    }
}
