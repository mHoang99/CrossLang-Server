using CrossLang.ApplicationCore.Enums;
using CrossLang.Library;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Entities
{
    [TableName("")]
    public class BaseEntity
    {
        #region Properties
        [JsonIgnore]
        [BsonIgnore]
        public EntityState EntityState { get; set; } = EntityState.GET;
        /// <summary>
        /// Ngày tạo
        /// </summary>
        [DisplayName("Khóa chính")]
        [DBColumn]
        [PrimaryKey]
        public long ID { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        [DisplayName("Ngày tạo")]
        [DBColumn]
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
        [DisplayName("Người tạo")]
        [DBColumn]
        public string? CreatedBy { get; set; }
        /// <summary>
        /// Ngày thay đổi gần nhất
        /// </summary>
        [DisplayName("Ngày thay đổi gần nhất")]
        [DBColumn]
        public DateTime? ModifiedDate { get; set; }
        /// <summary>
        /// Người thay đổi gần nhất
        /// </summary>
        [DisplayName("Người thay đổi")]
        [DBColumn]
        public string? ModifiedBy { get; set; }
        #endregion

        #region Methods
        public static string BuildAddQueryStatic()
        {
            var tableName = MethodBase.GetCurrentMethod()?
                .DeclaringType.GetTableName() ?? "";
            var columns = GetDBColumnsStatic();

            var columnsString = string.Join(",", columns);
            var columnsVarString = string.Join(",", columns.Select(x => $"@{x}"));

            return $"INSERT INTO {tableName} ({columnsString}) VALUES ({columnsVarString});";
        }

        public static string BuildUpdateQueryStatic(List<string> columnsToUpdate = null, string whereClause = "")
        {
            var tableName = MethodBase.GetCurrentMethod()?
                .DeclaringType.GetTableName() ?? "";

            if (columnsToUpdate == null)
            {
                columnsToUpdate = GetDBColumnsStatic();
            }

            string updateQuery = $"UPDATE {tableName} SET";

            var i = 0;
            columnsToUpdate.ForEach((x) =>
            {
                updateQuery += $" {x} = @{x}";

                if (i < columnsToUpdate.Count())
                {
                    updateQuery += ",";
                }

                i++;
            });

            return $"{updateQuery} {whereClause};";
        }

        public static string BuildDeleteQueryStatic(string whereClause = "1 = 1")
        {
            var tableName = MethodBase.GetCurrentMethod()?
                .DeclaringType.GetTableName() ?? "";
            return $"DELETE FROM {tableName} WHERE {whereClause}";
        }

        public static List<string> GetDBColumnsStatic()
        {
            var properties = MethodBase.GetCurrentMethod()?
                .DeclaringType?.GetProperties();

            var columns = new List<string>();

            if (properties == null)
            {
                return columns;
            }

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                if (property.IsDefined(typeof(DBColumn), false))
                {
                    columns.Add(propertyName);
                }
            }

            return columns;
        }

        public string BuildAddQuery()
        {
            var tableName = GetTableName();
            var columns = GetDBColumns();

            var columnsString = string.Join(",", columns);
            var columnsVarString = string.Join(",", columns.Select(x => $"@{x}"));

            return $"INSERT INTO {tableName} ({columnsString}) VALUES ({columnsVarString});SELECT LAST_INSERT_ID();";
        }

        public string BuildUpdateQuery(List<string> columnsToUpdate = null, string whereClause = "")
        {
            var tableName = GetTableName();

            if (columnsToUpdate == null)
            {
                columnsToUpdate = GetDBColumns();
            }

            string updateQuery = $"UPDATE {tableName} SET";

            var i = 0;
            columnsToUpdate.ForEach((x) =>
            {
                updateQuery += $" {x} = @{x}";

                if (i < columnsToUpdate.Count())
                {
                    updateQuery += ",";
                }

                i++;
            });

            return $"{updateQuery} {whereClause};";
        }

        public string BuildDeleteQuery(string whereClause = "1 = 1")
        {
            var tableName = GetTableName();
            return $"DELETE FROM {tableName} WHERE {whereClause};";
        }

        public List<string> GetDBColumns()
        {
            var properties = this.GetType().GetProperties();

            var columns = new List<string>();

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                if (property.IsDefined(typeof(DBColumn), false))
                {
                    columns.Add(propertyName);
                }
            }

            return columns;
        }

        public string GetTableName()
        {
            var tableNameAttr = this.GetType().GetCustomAttribute(typeof(TableName), false);

            if (tableNameAttr == null || (TableName)tableNameAttr == null)
            {
                return "";
            }

            return ((TableName)tableNameAttr).Value;
        }
        #endregion
    }

    public static class EntityExtensionMethod
    {
        public static string GetEntityTableName(this Type o)
        {
            var tableNameAttr = o.GetCustomAttribute(typeof(TableName), false);

            if (tableNameAttr == null || (TableName)tableNameAttr == null)
            {
                return "";
            }

            return ((TableName)tableNameAttr).Value;
        }
    }
}
