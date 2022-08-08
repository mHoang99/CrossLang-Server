using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Entities;
using CrossLang.Models;
using CrossLang.Library;
using CrossLang.Library.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
using System.Web;
using Microsoft.AspNetCore.Http;
using CrossLang.ApplicationCore.Enums;

namespace CrossLang.ApplicationCore
{
    /// <summary>
    /// Base Service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// CREATEDBY: VMHOANG (25/02/2022)
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        #region Fields
        /// <summary>
        /// repository kết nối DB
        /// </summary>
        protected IBaseRepository<T> _repository;
        protected IHttpContextAccessor _httpContextAccessor;
        protected SessionData _sessionData;


        #endregion

        #region Properties
        protected ServiceResult serviceResult;

        #endregion

        #region Constructor
        public BaseService(IBaseRepository<T> repository,
            IHttpContextAccessor httpContextAccessor,
            SessionData sessionData)
        {
            try
            {
                _repository = repository;
                _httpContextAccessor = httpContextAccessor;
                _sessionData = sessionData;
                serviceResult = new ServiceResult
                {
                    SuccessState = true,
                };
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    ex.Message,
                    UnexpectedErrorResponse(ex.Message)
                );
            }
        }
        #endregion

        #region Methods

        public void CreateTask(Action action)
        {
            var taskName = (new Guid()).ToString();

            Task.Run(() =>
            {
                Thread.CurrentThread.Name = taskName;
                action();
            })
            .ContinueWith((task) =>
            {
                _sessionData.CurrentPrincipalDispose();
            });
        }



        public ServiceResult Get()
        {
            try
            {
                serviceResult.SuccessState = true;
                serviceResult.Data = _repository.Get();
                return serviceResult;
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    ex.Message,
                    UnexpectedErrorResponse(ex.Message)
                );
            }
        }

        public ServiceResult GetById(long id)
        {
            try
            {
                //Kiểm tra kiểu dữ liệu Guid
                serviceResult.SuccessState = true;
                serviceResult.Data = _repository.GetEntityById(id);


                return serviceResult;
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    ex.Message,
                    UnexpectedErrorResponse(ex.Message)
                );
            }

        }

        public ServiceResult Add(T entity)
        {
            try
            {
                entity.EntityState = Enums.EntityState.ADD;

                //validate entity
                var isValid = Validate(entity);
                if (!isValid)
                {
                    return serviceResult;
                }

                BeforeAdd(ref entity);

                serviceResult.SuccessState = true;
                var newID = _repository.Add(entity);
                serviceResult.Data = newID;

                entity.ID = newID;

                AfterAdd(ref entity);

                AsyncAfterAdd(entity, _httpContextAccessor.HttpContext);

                //Không tác động được bản ghi
                if (int.Parse(serviceResult.Data.ToString()) <= 0)
                {
                    serviceResult = RowAffectingUnexpectedFailureResponse();
                }

                return serviceResult;
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    ex.Message,
                    UnexpectedErrorResponse(ex.Message)
                );
            }

        }

        protected virtual void AsyncAfterAdd(T entity, HttpContext httpContext)
        {

        }

        protected virtual void AfterAdd(ref T entity)
        {
        }

        protected virtual void BeforeAdd(ref T entity)
        {
            entity.CreatedDate = DateTime.Now;
            entity.ModifiedDate = DateTime.Now;
            entity.CreatedBy = _sessionData.Username;
            entity.ModifiedBy = _sessionData.Username;
        }

        public ServiceResult Update(long id, T entity)
        {
            try
            {
                entity.EntityState = Enums.EntityState.UPDATE;

                //set id của entity thành parsedId
                entity.ID = id;

                //Lấy dữ liệu từ repository
                var oldEntity = _repository.GetEntityById(id);

                if (oldEntity == null)
                {
                    serviceResult.SuccessState = false;
                    serviceResult.DevMsg = string.Format(Properties.Resources.CrossLang_ResponseMessage_RecordIdNotExists, id);
                    serviceResult.UserMsg = string.Format(Properties.Resources.CrossLang_ResponseMessage_RecordIdNotExists, id);
                    return serviceResult;
                }



                //validate entity
                var isValid = Validate(entity);

                if (!isValid)
                {
                    return serviceResult;
                }

                BeforeUpdate(ref entity, oldEntity);

                serviceResult.SuccessState = true;
                serviceResult.Data = _repository.Update(new List<long>() { id }, entity);

                AfterUpdate(entity, oldEntity);

                AsyncAfterUpdate(entity, oldEntity);

                //Không tác động được bản ghi
                if (int.Parse(serviceResult.Data?.ToString() ?? "-1") <= 0)
                {
                    serviceResult = RowAffectingUnexpectedFailureResponse();
                }

                return serviceResult;
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    ex.Message,
                    UnexpectedErrorResponse(ex.Message)
                );
            }
        }

        protected virtual void AsyncAfterUpdate(T entity, T oldEntity)
        {
        }

        protected virtual void AfterUpdate(T entity, T oldEntity)
        {
        }

        protected virtual void BeforeUpdate(ref T entity, T oldEntity)
        {
            entity.ModifiedDate = DateTime.Now;
        }

        public ServiceResult Delete(long id)
        {
            try
            {
                //lấy enity từ repository
                var oldEntity = _repository.GetEntityById(id);

                if (oldEntity == null)
                {
                    serviceResult.SuccessState = false;
                    serviceResult.DevMsg = string.Format(Properties.Resources.CrossLang_ResponseMessage_RecordIdNotExists, id);
                    serviceResult.UserMsg = string.Format(Properties.Resources.CrossLang_ResponseMessage_RecordIdNotExists, id);
                    return serviceResult;
                }

                if (!ValidateBeforeDelete(oldEntity))
                {
                    serviceResult.SuccessState = false;
                    serviceResult.DevMsg = string.Format(Properties.Resources.CrossLang_ResponseMessage_NotValid, id);
                    serviceResult.UserMsg = string.Format(Properties.Resources.CrossLang_ResponseMessage_NotValid, id);
                    return serviceResult;
                }


                BeforeDelete(oldEntity);

                serviceResult.SuccessState = true;
                oldEntity.EntityState = Enums.EntityState.REMOVE;

                var whereClause = $"ID IN ({oldEntity.ID})";

                serviceResult.Data = _repository.Delete(oldEntity, whereClause);

                AfterDelete(oldEntity);

                AsyncAfterDelete(oldEntity);

                if (int.Parse(serviceResult.Data?.ToString() ?? "-1") <= 0)
                {
                    serviceResult = RowAffectingUnexpectedFailureResponse();
                }

                return serviceResult;
            }
            catch (Exception ex)
            {
                throw new ServiceException(
                    ex.Message,
                    UnexpectedErrorResponse(ex.Message)
                );
            }
        }

        protected virtual bool ValidateBeforeDelete(T oldEntity)
        {
            return true;
        }

        protected virtual void AsyncAfterDelete(T oldEntity)
        {

        }

        protected virtual void AfterDelete(T oldEntity)
        {
        }

        protected virtual void BeforeDelete(T oldEntity)
        {
        }

        /// <summary>
        /// Validate dữ liệu
        /// </summary>
        /// <param name="entity">đối tượng cần validate</param>
        /// <returns>true: hợp lệ | false: không hợp lệ</returns>
        private bool Validate(T entity, List<string>? fields = null)
        {
            var isValid = true;

            var properties = entity.GetType().GetProperties();

            //Duyệt qua tất cả các properties của entity
            foreach (var property in properties)
            {
                if (property.IsDefined(typeof(DBColumn), false))
                {
                    var propertyValue = property.GetValue(entity);
                    var propertyName = property.Name;

                    if (fields != null && !fields.Exists(x => x.Equals(propertyName)))
                    {
                        continue;
                    }

                    var propertyDisplayName = property.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() as DisplayNameAttribute;

                    //Kiểm tra property có bắt buộc không
                    if (property.IsDefined(typeof(Required), true))
                    {
                        if (propertyValue == null && entity.EntityState != Enums.EntityState.ADD)
                        {
                            isValid = false;
                            serviceResult.SuccessState = false;
                            serviceResult.UserMsg = string.Format(Properties.Resources.CrossLang_ResponseMessage_RecordIdNotExists, propertyDisplayName?.DisplayName, propertyValue);
                            serviceResult.DevMsg = string.Format(Properties.Resources.CrossLang_ResponseMessage_RecordIdNotExists, propertyName, propertyValue);
                            break;
                        }
                    }

                    //Kiểm tra property có cần duy nhất không
                    if (property.IsDefined(typeof(Unique), true))
                    {
                        var entityNotUnique = _repository.GetEntityByProperty(entity, property);
                        if (entityNotUnique != null)
                        {
                            isValid = false;
                            serviceResult.SuccessState = false;
                            serviceResult.UserMsg = string.Format(Properties.Resources.CrossLang_ResponseMessage_Duplicated, propertyDisplayName.DisplayName, propertyValue.ToString());
                            serviceResult.DevMsg = string.Format(Properties.Resources.CrossLang_ResponseMessage_Duplicated, propertyName, propertyValue.ToString());
                            break;
                        }
                    }

                    //Kiểm tra độ dài
                    if (property.IsDefined(typeof(MaxLength), false))
                    {
                        var attr = property.GetCustomAttributes(typeof(MaxLength), false).FirstOrDefault() as MaxLength;

                        var maxLength = attr?.Value;

                        if (property.PropertyType == typeof(string))
                        {
                            if (((propertyValue?.ToString()) ?? "").Length > maxLength)
                            {
                                isValid = false;
                                serviceResult.SuccessState = false;
                                serviceResult.UserMsg = string.Format(
                                    Properties.Resources.CrossLang_ResponseMessage_MaxLengthExceeded,
                                    propertyDisplayName?.DisplayName ?? propertyName,
                                    maxLength
                                    );
                                serviceResult.DevMsg = string.Format(Properties.Resources.CrossLang_ResponseMessage_MaxLengthExceeded, propertyName, maxLength);
                                break;
                            }
                        }

                    }

                    //Kiểm tra email
                    if (property.IsDefined(typeof(Email), false))
                    {
                        var testPropertyValue = propertyValue;
                        if (testPropertyValue == null)
                        {
                            testPropertyValue = "";
                        }

                        if (property.PropertyType == typeof(string))
                        {
                            if (!CLValidator.IsValidEmail((string)testPropertyValue))
                            {
                                isValid = false;
                                serviceResult.SuccessState = false;
                                serviceResult.UserMsg = string.Format(Properties.Resources.CrossLang_ResponseMessage_InvalidEmail, propertyDisplayName?.DisplayName ?? propertyName);
                                serviceResult.DevMsg = string.Format(Properties.Resources.CrossLang_ResponseMessage_InvalidEmail, propertyName);
                                break;
                            }
                        }
                    }

                    //Kiểm tra edit role
                    if (property.IsDefined(typeof(EditPrivilege), false) && entity.EntityState == EntityState.UPDATE)
                    {
                        var attr = property.GetCustomAttributes(typeof(EditPrivilege), false).FirstOrDefault() as EditPrivilege;

                        var roleID = attr?.Value;

                        if ((roleID ?? 0) < _sessionData.RoleID)
                        {
                            isValid = false;
                            serviceResult.SuccessState = false;
                            serviceResult.UserMsg = string.Format(Properties.Resources.CrossLang_ResponseMessage_NoPermission, propertyDisplayName?.DisplayName ?? propertyName);
                            serviceResult.DevMsg = string.Format(Properties.Resources.CrossLang_ResponseMessage_NoPermission, propertyName);
                            break;
                        }
                    }
                }
            }
            return isValid && CustomValidate(entity, fields);
        }

        /// <summary>
        /// Hàm validate tùy chọn
        /// </summary>
        /// <returns>true | false</returns>
        protected virtual bool CustomValidate(T entity, List<string>? fields)
        {
            return true;
        }

        /// <summary>
        /// Kiểm tra string là guid
        /// </summary>
        /// <param name="fieldName">tên trường</param>
        /// <param name="toCheck">string cần kiểm tra</param>
        /// <returns>true | false</returns>
        protected bool CheckGuid(string fieldName, string toCheck)
        {
            var check = Guid.TryParse(toCheck, out _);

            if (!check)
            {
                serviceResult.SuccessState = false;
                serviceResult.DevMsg = string.Format(Properties.Resources.CrossLang_ResponseMessage_NotUUID, fieldName);
                serviceResult.UserMsg = string.Format(Properties.Resources.CrossLang_ResponseMessage_NotUUID, fieldName);
            }

            return check;
        }

        /// <summary>
        /// Kết quả của service khi không thể thay đổi bản ghi trong CSDL không rõ nguyên nhân
        /// </summary>
        /// <returns>Kết quả của service</returns>
        protected ServiceResult RowAffectingUnexpectedFailureResponse()
        {
            serviceResult.SuccessState = false;
            serviceResult.UserMsg = Properties.Resources.CrossLang_ResponseMessage_RowAffectingUnexpectedFailure;
            serviceResult.DevMsg = Properties.Resources.CrossLang_ResponseMessage_RowAffectingUnexpectedFailure;
            return serviceResult;
        }

        /// <summary>
        /// Kết quả của service khi gặp lỗi không xác định
        /// </summary>
        /// <param name="errorMessage">Message lỗi</param>
        /// <returns>Kết quả của service</returns>
        protected ServiceResult UnexpectedErrorResponse(string errorMessage)
        {
            serviceResult.SuccessState = false;
            serviceResult.DevMsg = errorMessage;
            serviceResult.UserMsg = Properties.Resources.CrossLang_ResponseMessage_Default;
            return serviceResult;
        }

        public virtual ServiceResult GetDetailsById(long id)
        {
            serviceResult.Data = _repository.GetDetailsById(id);
            serviceResult.SuccessState = true;

            return serviceResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual ServiceResult QueryList(T entity, List<FilterObject> filters, string formula, string sortBy = "ModifiedDate", string sortDirection = "desc", int pageNum = 1, int pageSize = 10)
        {
            List<T> list = this._repository.QueryList(entity, filters, formula, sortBy, sortDirection, pageNum, pageSize);

            long dbCount = this._repository.QueryListCount(entity, filters, formula);

            serviceResult.SuccessState = true;
            serviceResult.Data = new
            {
                Data = list,
                Summary = new
                {
                    Count = dbCount
                }
            };

            return serviceResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ServiceResult QueryListByView(string viewName, T entity, List<FilterObject> filters, string formula, string sortBy = "ModifiedDate", string sortDirection = "desc", int pageNum = 1, int pageSize = 10)
        {
            List<dynamic> list = this._repository.QueryListByView(viewName, entity, filters, formula, sortBy, sortDirection, pageNum, pageSize);

            long dbCount = this._repository.QueryListCount(entity, filters, formula);

            serviceResult.SuccessState = true;
            serviceResult.Data = new
            {
                Data = list,
                Summary = new
                {
                    Count = dbCount
                }
            };

            return serviceResult;
        }

        public ServiceResult MassAdd(List<T> entities)
        {
            var data = new List<dynamic>();

            BeforeMassAdd(ref entities);

            foreach (var entity in entities)
            {
                var res = Add(entity);
                data.Add((long)res.Data);
            }

            AfterMassAdd(ref entities);
            AsyncAfterMassAdd(entities);

            serviceResult.SuccessState = true;
            serviceResult.Data = data;

            return serviceResult;
        }

        protected virtual void AsyncAfterMassAdd(List<T> entities)
        {

        }

        protected virtual void AfterMassAdd(ref List<T> entities)
        {
        }

        protected virtual void BeforeMassAdd(ref List<T> entities)
        {
            
        }



        public ServiceResult UpdateFields(List<string> fields, T entity)
        {
            entity.EntityState = EntityState.UPDATE;
            if (!Validate(entity, fields))
            {
                return serviceResult;
            }

            if(!fields.Exists(x => { return x.Equals("ModifiedDate"); }))
            {
                fields.Add("ModifiedDate");
                entity.ModifiedDate = DateTime.Now;
                fields.Add("ModifiedBy");
                entity.ModifiedBy = _sessionData.Username;
            }

            var data = _repository.UpdateFields(fields, entity);

            serviceResult.SuccessState = true;
            serviceResult.Data = data;

            return serviceResult;
        }

        public ServiceResult GetPreviewById(long id)
        {
            var data = _repository.GetPreviewById(id);

            serviceResult.SuccessState = true;
            serviceResult.Data = data;

            return serviceResult;
        }


        public ServiceResult GetTableStruct()
        {
            var properties = typeof(T).GetProperties();

            var res = new List<Dictionary<string, string>>();

            foreach (var property in properties)
            {
                var propertyDisplayName = property.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() as DisplayNameAttribute;
                if (property.IsDefined(typeof(DisplayColumn), false))
                {
                    var x = new Dictionary<string, string>();
                    x.Add("FieldName", property.Name);
                    x.Add("DisplayName", propertyDisplayName?.DisplayName ?? "");

                    var typeName = property.PropertyType.Name;
                    if (property.PropertyType == typeof(DateTime?))
                    {
                        typeName = "DateTime";
                    }
                    if (property.PropertyType == typeof(int?))
                    {
                        typeName = "int";
                    }
                    if (property.PropertyType == typeof(long?))
                    {
                        typeName = "long";
                    }
                    x.Add("Type", typeName);
                    res.Add(x);
                }
            }

            serviceResult.SuccessState = true;
            serviceResult.Data = res;

            return serviceResult;
        }


        public bool CheckPackagePermission(int package)
        {
            return _sessionData?.Package >= package;
        }

        #endregion
    }
}
