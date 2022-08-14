using CrossLang.API.Models.Requests;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IService;
using CrossLang.Library;
using CrossLang.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;

namespace CrossLang.API.Controllers
{
    [ApiController]
    public class DictionaryWordController : BaseApiController<DictionaryWord>
    {
        public DictionaryWordController(IBaseService<DictionaryWord> service, SessionData sessionData) : base(service, sessionData)
        {
        }


        /// <summary>
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("search/text")]
        public async Task<IActionResult> GetSearchResults([FromBody] SearchRequest body)
        {
            var res = ((IDictionaryWordService)_service).GetSearchResult(body.Text);

            return Ok(res.ConvertToApiReturn());
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("evaluate/pronunciation/{word}/{fileName}")]
        public async Task<IActionResult> AssessPronunciation(string fileName, string word)
        {
            var res = await ((IDictionaryWordService)_service).PronunciationAssessmentAsync(fileName, word);

            return Ok(res.ConvertToApiReturn());
        }

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns></returns>
        /// CREATED_BY: vmhoang
        [HttpGet("details/{id}")]
        [AllowAnonymous]
        public override async Task<IActionResult> GetDeatails(long id)
        {
            return await base.GetDeatails(id);

        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("pronunciation/{word}")]
        public async Task<HttpResponseMessage> pronunciation(string word)
        {
            var res = await ((IDictionaryWordService)_service).TextToSpeechAsync(word);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(res);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("audio/x-wav");

            return response;
        }


        /// <summary>
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("evaluate/upload")]
        public async Task<IActionResult> UploadPronunciation([FromForm] IFormFileCollection audioData)
        {
            var audioFile = audioData[0];
            var totalSize = audioFile.Length;
            var fileBytes = new byte[audioFile.Length];

            using (var fileStream = audioFile.OpenReadStream())
            {
                var offset = 0;

                while (offset < audioFile.Length)
                {
                    var chunkSize = totalSize - offset < 8192 ? (int)totalSize - offset : 8192;

                    offset += await fileStream.ReadAsync(fileBytes, offset, chunkSize);
                }
            }
            // now save the file on the filesystem
            var newGuid = Guid.NewGuid();

            var fileName = $"{ newGuid.ToString()}.wav";

            System.IO.File.WriteAllBytes($"./Temp/{fileName}", fileBytes);

            return Ok((new ServiceResult
            {
                SuccessState = true,
                Data = fileName
            }).ConvertToApiReturn());
        }
    }
}
