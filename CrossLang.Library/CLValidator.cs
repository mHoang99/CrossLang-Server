using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.Library
{
    public static class CLValidator
    {
        #region Methods
        /// <summary>
        /// Validate email
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>true: hợp lệ | false: không hợp lệ</returns>
        public static bool IsValidEmail(string email)
        {
            if (!MailAddress.TryCreate(email, out _))
                return false;
            return true;
        }
        #endregion
    }

}
