using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrossLang.API.Models.Requests
{
    /// <summary>
    /// Request body khi cần gửi nhiều id
    /// </summary>
    /// CREATED_BY: vmhoang
    public class MultipleAffectRequest
    {
        public IEnumerable<string> Ids { get; set; }
    }
}
