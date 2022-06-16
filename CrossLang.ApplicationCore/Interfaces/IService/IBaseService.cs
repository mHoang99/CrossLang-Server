using CrossLang.ApplicationCore.Entities;
using CrossLang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Interfaces
{
    /// <summary>
    /// Interface cho Base Service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// CREATEDBY: VMHOANG (25/07/2021)
    public interface IBaseService<T>
    {
        #region Methods

        /// <summary>
        /// Lấy tất cả dữ liệu
        /// </summary>
        /// <returns>Danh sách tất cả dữ liệu</returns>
        ServiceResult Get();
        /// <summary>
        /// Lấy dữ liệu qua khóa chính
        /// </summary>
        /// <param name="id">Khóa chính</param>
        /// <returns>Bản ghi có khóa chính tương ứng | null</returns>
        ServiceResult GetById(long id);

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="entity">Thực thể theo kiểu T</param>
        /// <returns>Thông tin trả về</returns>
        ServiceResult Add(T entity);
        /// <summary>
        /// Thay đổi bản ghi
        /// </summary>
        /// <param name="entity">Thực thể theo kiểu T</param>
        /// <param name="id">Khóa chính</param>
        /// <returns>Thông tin trả vềL</returns>
        ServiceResult Update(long id, T entity);
        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="id">Khóa chính</param>
        /// <returns>Thông tin trả về</returns>
        ServiceResult Delete(long id);

        ServiceResult GetDetailsById(long id);

        ServiceResult QueryList(T entity, List<FilterObject> filters, int pageNum, int pageSize);

        ServiceResult QueryListByView(string viewName, T entity, List<FilterObject> filters, int pageNum, int pageSize);
        ServiceResult MassAdd(List<T> entities);

        ServiceResult UpdateFields(List<string> fields, T entity);
        #endregion
    }
}
