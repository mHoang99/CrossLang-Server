using System;
using CrossLang.API.Models.Requests;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IService;
using CrossLang.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrossLang.API.Controllers
{
    public class FlashCardCollectionController : BaseApiController<FlashCardCollection>
    {
        public FlashCardCollectionController(IFlashCardCollectionService service, SessionData sessionData) : base(service, sessionData)
        {
        }


        /// <summary>
        /// Xóa Bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns></returns>
        /// CREATED_BY: vmhoang
        [HttpPost("detailList")]
        [Authorize]
        public async Task<IActionResult> GetListCollection([FromBody] FilterRequest<FlashCardCollection> filterRequest)
        {
            var res = ((IFlashCardCollectionService)_service).GetListCollection(filterRequest.Entity, filterRequest.Filters, filterRequest.Formula, filterRequest.PageNum, filterRequest.PageSize);

            return Ok(res.ConvertToApiReturn());
        }

        [HttpGet("progressList/")]
        [Authorize]
        public async Task<IActionResult> GetProgressListCollection([FromQuery] int pageNum, [FromQuery] int pageSize, [FromQuery] bool type)
        {
            var res = ((IFlashCardCollectionService)_service).GetListCollectionWithProgress(pageNum,pageSize, type);

            return Ok(res.ConvertToApiReturn());
        }


    }
}

