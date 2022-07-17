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


        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns></returns>
        /// CREATED_BY: vmhoang
        [HttpGet("ExerciseAttemp/tableStruct")]
        [Authorize]
        async public Task<IActionResult> GetAttempHistoryTableStruct()
        {
            var res = ((IExerciseService)_service).GetExerciseAttempTableStruct();

            return Ok(res.ConvertToApiReturn());

        }

        /// <summary>
        /// DS Bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns></returns>
        /// CREATED_BY: vmhoang
        [HttpPost("ExerciseAttemp/list")]
        [Authorize]
        public async Task<IActionResult> QueryAttempHistoryList([FromBody] FilterRequest<ExerciseAttempMongo> filterRequest)
        {
            var res = ((IExerciseService)_service).QueryExerciseAttempList(filterRequest.Entity, filterRequest.Filters, filterRequest.Formula, filterRequest.SortBy, filterRequest.SortDirection, filterRequest.PageNum, filterRequest.PageSize);

            return Ok(res.ConvertToApiReturn());
        }


        [HttpGet("ReportPartitionByAttemps/{exerciseId}")]
        [Authorize]
        public async Task<IActionResult> GetReportPartitionByAttemps(long exerciseId)
        {
            var res = ((IExerciseService)_service).GetReportPartitionByAttemps(exerciseId);

            return Ok(res.ConvertToApiReturn());
        }

        [HttpGet("ReportScroreSpectrum/{exerciseId}")]
        [Authorize]
        public async Task<IActionResult> GetScoreSpectrum(long exerciseId)
        {
            var res = ((IExerciseService)_service).GetScoreSpectrum(exerciseId);

            return Ok(res.ConvertToApiReturn());
        }

        [HttpGet("DoExerciseRatioValue/{exerciseId}")]
        [Authorize]
        public async Task<IActionResult> DoExerciseRatioValue(long exerciseId)
        {
            var res = ((IExerciseService)_service).DoExerciseRatioValue(exerciseId);

            return Ok(res.ConvertToApiReturn());
        }
    }
}

