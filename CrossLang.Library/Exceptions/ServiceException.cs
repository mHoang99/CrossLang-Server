using CrossLang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.Library.Exceptions
{
    /// <summary>
    /// Exception cho service
    /// </summary>
    /// CREATEDBY: VMHOANG (05/08/2021)
    public class ServiceException : Exception
    {
        #region Properties
        public ServiceResult ServiceResult { get; }
        #endregion

        #region Constructors
        public ServiceException(string msg, ServiceResult res) : base(msg)
        {
            ServiceResult = res;
        }
        #endregion
    }
}
