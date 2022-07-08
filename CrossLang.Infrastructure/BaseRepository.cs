using Dapper;
using CrossLang.ApplicationCore.Interfaces;
using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Enums;
using CrossLang.Library;
using CrossLang.Models;

namespace CrossLang.Infrastructure
{
    /// <summary>
    /// Base Repository kết nối database
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// CREATEDBY: VMHOANG
    public class BaseRepository<T> : IBaseRepository<T>, IDisposable where T : BaseEntity
    {
        #region Fields
        /// <summary>
        /// Tên bảng
        /// </summary>
        protected string _tableName;
        /// <summary>
        /// Kết nối tới DB
        /// </summary>
        protected IDbConnection _dbConnection;
        protected SessionData _sessionData;
        #endregion

        #region Constructor
        public BaseRepository(IDBContext dbContext, SessionData sessionData)
        {
            _tableName = typeof(T).GetEntityTableName();
            _dbConnection = dbContext.GetConnection();
            _sessionData = sessionData;
        }
        #endregion

        #region Methods
        public IEnumerable<T> Get()
        {
            var pluralizedName = PluralizeService.Core.PluralizationProvider.Pluralize(_tableName);

            //Khởi tạo commandText
            var entities = _dbConnection.Query<T>($"Proc_Get{pluralizedName}", commandType: CommandType.StoredProcedure);

            return entities;
        }

        public IEnumerable<object> GetView()
        {
            //Khởi tạo commandText
            var query = $"SELECT * FROM View_{_tableName}";

            var entities = _dbConnection.Query<T>(query, commandType: CommandType.Text);
            return entities;
        }

        public virtual T GetEntityById(long id)
        {
            var query = $"SELECT * FROM {_tableName} WHERE ID = {id}";

            //Khởi tạo commandText
            var entity = _dbConnection.QueryFirstOrDefault<T>(
                query,
                commandType: CommandType.Text
                );

            return entity;
        }

        public virtual long Add(T entity)
        {
            long newID = 0;

            //_dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    var parameters = MappingDbType(entity);
                    newID = _dbConnection.ExecuteScalar<long>(entity.BuildAddQuery(), parameters, commandType: CommandType.Text, transaction: transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return newID;
        }

        public virtual int Update(List<long> ids, T entity, string whereClause = "WHERE 1 = 1")
        {
            var rowsAffected = 0;

            var idsString = string.Join(",", ids);

            whereClause += $" AND ID IN ({idsString})";

            //_dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    var parameters = MappingDbType(entity);
                    rowsAffected = _dbConnection.Execute(entity.BuildUpdateQuery(whereClause: whereClause), parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return rowsAffected;
        }

        public virtual int Delete(T oldEntity, string whereClause = "WHERE 1 = 1")
        {
            var rowsAffected = 0;

            if (oldEntity == null)
            {
                try
                {
                    oldEntity = Activator.CreateInstance(typeof(T), new object[] { }) as T;
                }
                catch (Exception)
                {

                }
            }

            var query = oldEntity?.BuildDeleteQuery(whereClause);

            //_dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    //Thực thi commandText
                    rowsAffected = _dbConnection.Execute(query, commandType: CommandType.Text, transaction: transaction);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return rowsAffected;
        }

        public T GetEntityByProperty(T entity, PropertyInfo property)
        {
            var propertyName = property.Name;
            var propertyValue = property.GetValue(entity);
            var keyValue = entity.ID;
            var tableName = entity.GetTableName();

            var query = string.Empty;

            if (entity.EntityState == EntityState.ADD || entity.EntityState == EntityState.GET)
            {
                query = $"SELECT * FROM {tableName} WHERE {propertyName} = '{propertyValue}'";
            }
            else if (entity.EntityState == EntityState.UPDATE)
            {
                query = $"SELECT * FROM {tableName} WHERE {propertyName} = '{propertyValue}' AND ID <> '{keyValue}'";
            }
            else
            {
                return null;
            }

            var res = _dbConnection.QueryFirstOrDefault<T>(query, commandType: CommandType.Text);
            return res;
        }

        /// <summary>
        /// Map dữ liệu của 1 entity sang thành dynamic parameters dùng cho truy vấn SQL
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns>dynamic parameters đã được format đúng</returns>
        protected DynamicParameters MappingDbType<TEntity>(TEntity entity)
        {
            var properties = entity?.GetType().GetProperties();
            var parameters = new DynamicParameters();

            if (properties == null)
            {
                return parameters;
            }

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(entity);
                var propertyType = property.PropertyType;
                if (property.GetCustomAttribute(typeof(DBColumn), false) != null)
                {
                    if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                    {
                        parameters.Add($"@{propertyName}", propertyValue, DbType.String);
                    }
                    else
                    {
                        parameters.Add($"@{propertyName}", propertyValue);
                    }
                }
            }

            return parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="parameters"></param>
        protected void MappingDbTypeByField(T entity, string fieldName, ref DynamicParameters parameters)
        {
            var propertyInfo = entity.GetType().GetProperty(fieldName);

            if (propertyInfo == null)
            {
                return;
            }


            var propertyName = propertyInfo.Name;

            var propertyValue = propertyInfo.GetValue(entity);

            var propertyType = propertyInfo.PropertyType;

            //if (propertyInfo.GetCustomAttribute(typeof(DBColumn), false) != null)
            //{
                if (propertyType == typeof(Guid) || propertyType == typeof(Guid?) || propertyType == typeof(string))
                {
                    parameters.Add($"@{propertyName}", propertyValue, DbType.String);
                }
                else
                {
                    parameters.Add($"@{propertyName}", propertyValue);
                }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (_dbConnection.State == ConnectionState.Open)
            {
                _dbConnection.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual IEnumerable<IDictionary<string, object>> GetDetailsById(long id)
        {
            return new List<Dictionary<string, object>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="filters"></param>
        /// <param name="filterStr"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<T> QueryList(T entity, List<FilterObject> filters, string formula, string sortBy, string sortDirection, int pageNum, int pageSize)
        {
            var parameters = new DynamicParameters();

            var filterStr = BuildFilterString(entity, filters, formula);

            filters.ForEach(x =>
            {
                MappingDbTypeByField(entity, x.FieldName, ref parameters);
            });

            var res = new List<T>();


            var query = $"SELECT * FROM {_tableName} WHERE {filterStr} ORDER BY {sortBy} {sortDirection} LIMIT {pageSize} OFFSET {pageSize * (pageNum - 1)};";

            var resp = (_dbConnection.Query<T>(query, parameters, commandType: CommandType.Text))?.ToList() ?? new List<T>();

            return resp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="filters"></param>
        /// <param name="filterStr"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<dynamic> QueryListByView(string viewName, T entity, List<FilterObject> filters, string formula, string sortBy, string sortDirection, int pageNum, int pageSize)
        {
            var parameters = new DynamicParameters();

            var filterStr = BuildFilterString(entity, filters, formula);

            filters.ForEach(x =>
            {
                MappingDbTypeByField(entity, x.FieldName, ref parameters);
            });

            var res = new List<T>();


            var query = $"SELECT * FROM {viewName} WHERE {filterStr} ORDER BY {sortBy} {sortDirection} LIMIT {pageSize} OFFSET {pageSize * (pageNum - 1)};";

            var resp = (_dbConnection.Query<dynamic>(query, parameters, commandType: CommandType.Text))?.ToList() ?? new List<dynamic>();

            return resp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="filters"></param>
        /// <param name="filterStr"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public long QueryListCount(T entity, List<FilterObject> filters, string formula)
        {
            var parameters = new DynamicParameters();

            var filterStr = BuildFilterString(entity, filters, formula);

            filters.ForEach(x =>
            {
                MappingDbTypeByField(entity, x.FieldName, ref parameters);
            });

            var res = new List<T>();


            var query = $"SELECT COUNT(*) FROM {_tableName} WHERE {filterStr};";

            var resp = (_dbConnection.ExecuteScalar<long>(query, parameters, commandType: CommandType.Text));

            return resp;
        }

        public long QueryListByViewCount(string viewName, T entity, List<FilterObject> filters, string formula)
        {
            var parameters = new DynamicParameters();

            var filterStr = BuildFilterString(entity, filters, formula);

            filters.ForEach(x =>
            {
                MappingDbTypeByField(entity, x.FieldName, ref parameters);
            });

            var res = new List<T>();


            var query = $"SELECT COUNT(*) FROM {viewName} WHERE {filterStr};";

            var resp = (_dbConnection.ExecuteScalar<long>(query, parameters, commandType: CommandType.Text));

            return resp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public string BuildFilterString(T entity, List<FilterObject> filters, string formula = null)
        {
            string str = " ";


            if (string.IsNullOrEmpty(formula))
            {
                filters.ForEach(x =>
                {
                    PropertyInfo? prop = entity.GetType().GetProperty(x.FieldName);

                    if (prop == null)
                    {
                        return;
                    }

                    var propType = prop.PropertyType;
                    switch (x.Operator)
                    {
                        case (int)Operator.EQUALS:
                            str += $@"{x.FieldName} = @{x.FieldName}";
                            break;
                        case (int)Operator.NOT_EQUALS:
                            str += $@"{x.FieldName} <> @{x.FieldName}";
                            break;
                        case (int)Operator.LIKE:
                            str += $@"{x.FieldName} LIKE @{x.FieldName}";
                            break;
                        case (int)Operator.NOT_LIKE:
                            str += $@"{x.FieldName} NOT LIKE @{x.FieldName}";
                            break;
                        case (int)Operator.GREATER_THAN:
                            str += $@"{x.FieldName} > @{x.FieldName}";
                            break;
                        case (int)Operator.GREATER_EQUAL:
                            str += $@"{x.FieldName} >= @{x.FieldName}";
                            break;
                        case (int)Operator.SMALLER_THAN:
                            str += $@"{x.FieldName} < @{x.FieldName}";
                            break;
                        case (int)Operator.SMALLER_EQUAL:
                            str += $@"{x.FieldName} <= @{x.FieldName}";
                            break;
                        case (int)Operator.IS_NOT:
                            str += $@"{x.FieldName} IS NOT @{x.FieldName}";
                            break;
                        case (int)Operator.IS:
                            str += $@"{x.FieldName} IS @{x.FieldName}";
                            break;
                        case (int)Operator.IS_NULL:
                            str += $@"{x.FieldName} IS NULL";
                            break;
                        case (int)Operator.IS_NOT_NULL:
                            str += $@"{x.FieldName} IS NOT NULL";
                            break;
                        case (int)Operator.IN:
                            if (x.ContainedValues.Any())
                            {
                                if (propType.ToString() == "string")
                                {
                                    str += $@"{x.FieldName} IN ('{string.Join("','", x.ContainedValues)}')";
                                }
                                else
                                {
                                    str += $@"{x.FieldName} IN ({string.Join(",", x.ContainedValues)})";
                                }
                            }
                            break;
                        default:
                            str += "1=1";
                            break;
                    }

                    str += " AND ";

                });
                str += "1 = 1";
            }
            else
            {
                str = formula;

                for (int i = 0; i < filters.Count(); ++i)
                {
                    var x = filters[i];
                    PropertyInfo? prop = entity.GetType().GetProperty(x.FieldName);

                    if (prop == null)
                    {
                        continue;
                    }

                    var fieldStr = "";

                    var propType = prop.PropertyType;
                    switch (x.Operator)
                    {
                        case (int)Operator.EQUALS:
                            fieldStr = $@"{x.FieldName} = @{x.FieldName}";
                            break;
                        case (int)Operator.NOT_EQUALS:
                            fieldStr = $@"{x.FieldName} <> @{x.FieldName}";
                            break;
                        case (int)Operator.LIKE:
                            fieldStr = $@"{x.FieldName} LIKE @{x.FieldName}";
                            break;
                        case (int)Operator.NOT_LIKE:
                            fieldStr = $@"{x.FieldName} NOT LIKE @{x.FieldName}";
                            break;
                        case (int)Operator.GREATER_THAN:
                            fieldStr = $@"{x.FieldName} > @{x.FieldName}";
                            break;
                        case (int)Operator.GREATER_EQUAL:
                            fieldStr = $@"{x.FieldName} >= @{x.FieldName}";
                            break;
                        case (int)Operator.SMALLER_THAN:
                            fieldStr = $@"{x.FieldName} < @{x.FieldName}";
                            break;
                        case (int)Operator.SMALLER_EQUAL:
                            fieldStr = $@"{x.FieldName} <= @{x.FieldName}";
                            break;
                        case (int)Operator.IS_NOT:
                            fieldStr = $@"{x.FieldName} IS NOT @{x.FieldName}";
                            break;
                        case (int)Operator.IS:
                            fieldStr = $@"{x.FieldName} IS @{x.FieldName}";
                            break;
                        case (int)Operator.IS_NULL:
                            fieldStr = $@"{x.FieldName} IS NULL";
                            break;
                        case (int)Operator.IS_NOT_NULL:
                            fieldStr = $@"{x.FieldName} IS NOT NULL";
                            break;
                        case (int)Operator.IN:
                            if (x.ContainedValues.Any())
                            {
                                if (propType.ToString() == "string")
                                {
                                    fieldStr = $@"{x.FieldName} IN ('{string.Join("','", x.ContainedValues)}')";
                                }
                                else
                                {
                                    fieldStr = $@"{x.FieldName} IN ({string.Join(",", x.ContainedValues)})";
                                }
                            }
                            break;
                        default:
                            fieldStr = "1=1";
                            break;
                    }
                    str = str.Replace($"({i + 1})", fieldStr);
                }
            }

            return str;
        }

        public int UpdateFields(List<string> fields, T entity)
        {
            var rowsAffected = 0;

            var parameters = new DynamicParameters();

            var paramStr = "";

            var entityDBColumns = entity.GetDBColumns();

            fields.ForEach(x =>
            {
                if (entityDBColumns.Exists(col => col == x))
                {
                    MappingDbTypeByField(entity, x, ref parameters);
                    paramStr = $@"{paramStr}{x} = @{x},";
                }
            });

            paramStr = paramStr.Remove(paramStr.Length - 1);



            var query = $"UPDATE {entity.GetTableName()} SET {paramStr} WHERE ID = {entity.ID}";

            //_dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    rowsAffected = _dbConnection.Execute(query, parameters, commandType: CommandType.Text, transaction: transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return rowsAffected;
        }

        public virtual IEnumerable<IDictionary<string, object>> GetPreviewById(long id)
        {
            return new List<Dictionary<string, object>>();
        }

        public T GetEntityByColumns(T entity, List<string> columns)
        {
            var dynamicParameters = new DynamicParameters();

            var wherePhrase = "";

            foreach (var column in columns)
            {
                wherePhrase = $"{wherePhrase} {column} = @{column} AND ";
                dynamicParameters.Add(column, entity.GetType().GetProperty(column)?.GetValue(entity));
            }

            wherePhrase = $"{wherePhrase} 1 = 1";

            var tableName = typeof(T).GetTableName();

            var query = $"SELECT * FROM {tableName} WHERE {wherePhrase};";

            var res = _dbConnection.QueryFirstOrDefault<T>(query, dynamicParameters);

            return res;
        }


        #endregion
    }
}