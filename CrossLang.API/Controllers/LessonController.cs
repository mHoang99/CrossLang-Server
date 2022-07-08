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
    public class LessonController : BaseApiController<Lesson>
    {
        IBaseService<UserLessonStatus> _userLessonStatusService;

        public LessonController(ILessonService service, IBaseService<UserLessonStatus> userLessonStatusService, SessionData sessionData) : base(service, sessionData)
        {
            _userLessonStatusService = userLessonStatusService;
        }

        [HttpPost("finishLesson")]
        [Authorize]
        public async Task<IActionResult> FinishLesson([FromBody] Lesson entity)
        {
            var res = ((ILessonService)_service).FinishLesson(entity);
            return Ok(res.ConvertToApiReturn());
        }


        [HttpGet("PreviewById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetPreviewById(long id)
        {
            var res = _service.GetPreviewById(id);
            return Ok(res.ConvertToApiReturn());
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("AttempHistory/{id}")]
        public async Task<IActionResult> GetAttempHistory(long id)
        {
            var res = ((ILessonService)_service).GetAttempHistory(id);

            return Ok(res.ConvertToApiReturn());
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("LessonList")]
        public async Task<IActionResult> GetLessonList([FromBody] FilterRequest<Lesson> filterRequest)
        {
            var res = ((ILessonService)_service).GetLessonList(filterRequest.Entity, filterRequest.Filters, filterRequest.Formula, filterRequest.SortBy, filterRequest.SortDirection, filterRequest.PageNum, filterRequest.PageSize) ;

            return Ok(res.ConvertToApiReturn());
        }
    }
}

