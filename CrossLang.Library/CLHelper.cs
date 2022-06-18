using CrossLang.Models;
using System;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace CrossLang.Library
{
    /// <summary>
    /// Class chứa các hàm dùng chung
    /// </summary>
    public static class CLHelper
    {
        #region Methods
        /// <summary>
        /// Chuyển từ ServiceResult sang ApiErrorReturn
        /// </summary>
        /// <param name="serviceResult"></param>
        /// <returns>instance của ApiErrorReturn</returns>
        public static ApiReturn ConvertToApiReturn(this ServiceResult serviceResult)
        {
            if(serviceResult.SuccessState)
            {
                serviceResult.DevMsg = "";
                serviceResult.UserMsg = "";
            } else
            {
                serviceResult.Data = null;
            }

            var aer = new ApiReturn
            {
                Success = serviceResult.SuccessState,
                Data = serviceResult.Data,
                DevMsg = serviceResult.DevMsg,
                UserMsg = serviceResult.UserMsg,
                ErrorCode = $""
            };

            return aer;
        }


        public static T CastObject<T>(object input)
        {
            return (T)input;
        }

        public static T ConvertObject<T>(object input)
        {
            return (T)Convert.ChangeType(input, typeof(T));
        }

        public static string GetCollectionName(this Type type)
        {
            var collectionNameAttr = type.GetCustomAttributes(typeof(CollectionName), false);

            if (collectionNameAttr == null || (CollectionName)collectionNameAttr[0] == null)
            {
                return "";
            }

            return ((CollectionName)collectionNameAttr[0]).Value;
        }

        public static string GetTableName(this Type type)
        {
            var tableNameAttr = type.GetCustomAttributes(typeof(TableName), false);

            if (tableNameAttr == null || (TableName)tableNameAttr[0] == null)
            {
                return "";
            }

            return ((TableName)tableNameAttr[0])?.Value ?? "";
        }
        #endregion
    }
}
