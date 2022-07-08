using CrossLang.Models;
using System;
using System.ComponentModel;
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
            if (serviceResult.SuccessState)
            {
                serviceResult.DevMsg = "";
                serviceResult.UserMsg = "";
            }
            else
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


        public static void AddOrUpdate(this IDictionary<string, object> dict, string key, object value)
        {
            if (dict.ContainsKey(key))
            {
                dict.Remove(key);
            }
            dict.Add(key, value);
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            var rng = new Random();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }


        public static IDictionary<string, object> ToDictionary(this object source)
        {
            return source.ToDictionary<object>();
        }

        public static IDictionary<string, T> ToDictionary<T>(this object source)
        {
            if (source == null)
                return null;

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
                AddPropertyToDictionary<T>(property, source, dictionary);
            return dictionary;
        }

        private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
        {
            object value = property.GetValue(source);
            if (IsOfType<T>(value))
                dictionary.Add(property.Name, (T)value);
        }

        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }


        #endregion
    }
}
