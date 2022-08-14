using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.Library;
using CrossLang.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrossLang.API.Controllers
{
    [Route("api/common")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private IBaseService<LessonCategory> _lessonCategoryService;

        public CommonController(IBaseService<LessonCategory> lessonCategoryService)
        {
            _lessonCategoryService = lessonCategoryService;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet("initData")]
        public async Task<IActionResult> initData()
        {
            var res = new Dictionary<string, object>();

            var categories = _lessonCategoryService.Get().Data;

            res.AddOrUpdate("Categories", categories);

            var sr = new ServiceResult
            {
                SuccessState = true,
                Data = res
            };

            return Ok(sr.ConvertToApiReturn());
        }


    }
}

