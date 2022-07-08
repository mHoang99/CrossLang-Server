using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CrossLang.ApplicationCore.Entities;
using CrossLang.Models;

namespace CrossLang.ApplicationCore.Interfaces
{
    /// <summary>
    /// Interface cho Base Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// CREATEDBY: VMHOANG (25/07/2021)
    public interface IBaseRepository<T>
    {
        #region Methods
        /// <summary>
        /// Lấy tất cả dữ liệu
        /// </summary>
        /// <returns>Danh sách tất cả dữ liệu</returns>
        IEnumerable<T> Get();
        IEnumerable<IDictionary<string, object>> GetDetailsById(long id);

        /// <summary>
        /// Lấy tất cả dữ liệu trong view
        /// </summary>
        /// <returns>Danh sách tất cả dữ liệu</returns>
        IEnumerable<Object> GetView();
        /// <summary>
        /// Lấy dữ liệu qua khóa chính
        /// </summary>
        /// <param name="id">Khóa chính</param>
        /// <returns>Bản ghi có khóa chính tương ứng | null</returns>
        T GetEntityById(long id);
        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="entity">Thực thể theo kiểu T</param>
        /// <returns>Số bản ghi được thay đổi</returns>
        long Add(T entity);
        /// <summary>
        /// Thay đổi bản ghi
        /// </summary>
        /// <param name="entity">Thực thể theo kiểu T</param>
        /// <returns>Số bản ghi được thay đổi</returns>
        int Update(List<long> ids, T entity, string whereClause = "WHERE 1 = 1");

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="id">Khóa chính</param>
        /// <returns>Số bản ghi được thay đổi</returns>
        int Delete(T oldEntity, string whereClause = "WHERE 1 = 1");
        /// <summary>
        /// Lấy bản ghi theo tiêu chí
        /// </summary>
        /// <returns>Đối tượng tìm được | null</returns>
        T GetEntityByProperty(T entity, PropertyInfo property);

        List<T> QueryList(T entity, List<FilterObject> filters, string formula, string sortBy, string sortDirection, int pageNum, int pageSize);

        long QueryListCount(T entity, List<FilterObject> filters, string formula);

        List<dynamic> QueryListByView(string viewName, T entity, List<FilterObject> filters, string formula, string sortBy, string sortDirection, int pageNum, int pageSize);

        long QueryListByViewCount(string v, T entity, List<FilterObject> filters, string formula);

        int UpdateFields(List<string> fields, T entity);


        IEnumerable<IDictionary<string, object>> GetPreviewById(long id);

        T GetEntityByColumns(T entity, List<string> columns);
        #endregion


    }
}
