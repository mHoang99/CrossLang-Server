using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IService;
using CrossLang.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrossLang.API.Controllers
{
    public class ExerciseController : BaseApiController<Exercise>
    {
        public ExerciseController(IExerciseService service, SessionData sessionData) : base(service, sessionData)
        {
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromBody] ExerciseAttempMongo entity)
        {
            var res = ((IExerciseService)_service).Submit(entity);

            return Ok(res.ConvertToApiReturn());
        }


        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("AttempHistory/{id}")]
        public async Task<IActionResult> GetAttempHistory(long id)
        {
            var res = ((IExerciseService)_service).GetAttempHistory(id);

            return Ok(res.ConvertToApiReturn());
        }
    }
}

