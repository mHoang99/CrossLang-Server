using System;
using CrossLang.API.Models.Requests;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IService;
using CrossLang.Library;
using CrossLang.Models;
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
        [HttpPost("LessonList/{isRelatedToUser}")]
        public async Task<IActionResult> GetLessonList([FromBody] FilterRequest<Lesson> filterRequest, bool isRelatedToUser)
        {
            var res = ((ILessonService)_service).GetLessonList(filterRequest.Entity, filterRequest.Filters, filterRequest.Formula, filterRequest.SortBy, filterRequest.SortDirection, filterRequest.PageNum, filterRequest.PageSize, isRelatedToUser) ;

            return Ok(res.ConvertToApiReturn());
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("thumbnail/upload/{fileName}")]
        public async Task<IActionResult> UploadThumbnail([FromForm] IFormFileCollection imageData, string fileName)
        {
            var imageFile = imageData[0];
            var totalSize = imageFile.Length;
            var fileBytes = new byte[imageFile.Length];

            using (var fileStream = imageFile.OpenReadStream())
            {
                var offset = 0;

                while (offset < imageFile.Length)
                {
                    var chunkSize = totalSize - offset < 8192 ? (int)totalSize - offset : 8192;

                    offset += await fileStream.ReadAsync(fileBytes, offset, chunkSize);
                }
            }

            var fileExtension = fileName.Split('.')[fileName.Split('.').Count() - 1];
            var newGuid = Guid.NewGuid();
            fileName = $"{newGuid}.{fileExtension}";

            System.IO.File.WriteAllBytes($"./wwwroot/Images/Thumbnail/{fileName}", fileBytes);
            var newFileUrl = $"{Request.Scheme + "://" + Request.Host + Request.PathBase + "/Images/Thumbnail/" + fileName}";

            return Ok((new ServiceResult
            {
                SuccessState = true,
                Data = newFileUrl
            }).ConvertToApiReturn());
        }
    }
}

