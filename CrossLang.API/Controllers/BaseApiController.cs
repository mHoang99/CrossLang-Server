using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossLang.Library;
using CrossLang.API.Models.Requests;

namespace CrossLang.API.Controllers
{

    /// <summary>
    /// Base Api controller 
    /// </summary>
    /// CREATED_BY: vmhoang
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController<T> : ControllerBase where T : BaseEntity
    {
        #region Fields
        /// <summary>
        /// Service xử lý nghiệp vụ
        /// </summary>
        protected IBaseService<T> _service;
        protected SessionData _sessionData;
        #endregion

        #region Constructors
        public BaseApiController(IBaseService<T> service, SessionData sessionData)
        {
            _service = service;
            _sessionData = sessionData;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy tất cả 
        /// </summary>
        /// <returns></returns>
        /// CREATED_BY: vmhoang
        [HttpGet]
        async public Task<IActionResult> Get()
        {
            var res = _service.Get();

            return Ok(res.ConvertToApiReturn());
        }

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns></returns>
        /// CREATED_BY: vmhoang
        [HttpGet("{id}")]
        [Authorize]
        async public Task<IActionResult> Get(long id)
        {
            var res = _service.GetById(id);

            return Ok(res.ConvertToApiReturn());

        }

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns></returns>
        /// CREATED_BY: vmhoang
        [HttpGet("details/{id}")]
        [Authorize]
        async public Task<IActionResult> GetDeatails(long id)
        {
            var res = _service.GetDetailsById(id);

            return Ok(res.ConvertToApiReturn());

        }

        /// <summary>
        /// Thêm bản ghi
        /// </summary>
        /// <param name="entity">Dữ liệu cần thêm mới</param>
        /// <returns></returns>
        /// CREATED_BY: vmhoang
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostAsync([FromBody] T entity)
        {
            var res = _service.Add(entity);

            return Ok(res.ConvertToApiReturn());
        }

        /// <summary>
        /// Sửa Bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <param name="entity">Dữ liệu cần sửa</param>
        /// <returns></returns>
        /// CREATED_BY: vmhoang
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAsync([FromRoute(Name = "id")] long id, [FromBody] T entity)
        {

            var res = _service.Update(id, entity);

            return Ok(res.ConvertToApiReturn());

        }

        /// <summary>
        /// Xóa Bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns></returns>
        /// CREATED_BY: vmhoang
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
        {
            var res = _service.Delete(id);

            return Ok(res.ConvertToApiReturn());

        }

        /// <summary>
        /// Xóa Bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns></returns>
        /// CREATED_BY: vmhoang
        [HttpPost("list")]
        [Authorize]
        public async Task<IActionResult> QueryList([FromBody] FilterRequest<T> filterRequest)
        {
            var res = _service.QueryList(filterRequest.Entity ,filterRequest.Filters, filterRequest.PageNum, filterRequest.PageSize);

            return Ok(res.ConvertToApiReturn());
        }

        /// <summary>
        /// Xóa Bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns></returns>
        /// CREATED_BY: vmhoang
        [HttpPost("massSave")]
        [Authorize]
        public async Task<IActionResult> MassSave([FromBody] List<T> entities)
        {
            var res = _service.MassAdd(entities);

            return Ok(res.ConvertToApiReturn());
        }


        /// <summary>
        /// Xóa Bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns></returns>
        /// CREATED_BY: vmhoang
        [HttpPut("updateByFields")]
        [Authorize]
        public async Task<IActionResult> UpdateByFields([FromBody] UpdateFieldsRequest<T> body)
        {
            var res = _service.UpdateFields(body.Fields, body.Entity);

            return Ok(res.ConvertToApiReturn());
        }


        #endregion

    }
}
