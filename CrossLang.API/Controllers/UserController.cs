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
    public class UserController : BaseApiController<User>
    {

        private IUpgradeCodeService upgradeCodeService;

        public UserController(IUserService service, SessionData sessionData, IUpgradeCodeService ucSV) : base(service, sessionData)
        {
            upgradeCodeService = ucSV;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("avatar/upload/{fileName}")]
        public async Task<IActionResult> UploadAvatar([FromForm] IFormFileCollection imageData, string fileName)
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

            System.IO.File.WriteAllBytes($"./wwwroot/Images/Avatar/{fileName}", fileBytes);
            var newFileUrl = $"{Request.Scheme + "://" + Request.Host + Request.PathBase + "/Images/Avatar/" + fileName}";

            return Ok((new ServiceResult
            {
                SuccessState = true,
                Data = newFileUrl
            }).ConvertToApiReturn());
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("personalImage/upload/{fileName}")]
        public async Task<IActionResult> UploadPersonalImage([FromForm] IFormFileCollection imageData, string fileName)
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

            System.IO.File.WriteAllBytes($"./wwwroot/Images/PersonalImage/{fileName}", fileBytes);
            var newFileUrl = $"{Request.Scheme + "://" + Request.Host + Request.PathBase + "/Images/PersonalImage/" + fileName}";

            return Ok((new ServiceResult
            {
                SuccessState = true,
                Data = newFileUrl
            }).ConvertToApiReturn());
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("upgrade/generate")]
        public async Task<IActionResult> Generate([FromBody] GenerateUpgradeCodeRequest req)
        {
            var res = upgradeCodeService.GenerateCode(req.Package, req.Period, req.IsTrial, req.Price);
            //upgradeCodeService.Add

            return Ok(res.ConvertToApiReturn());
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("upgrade/redeem")]
        public async Task<IActionResult> RedeemCode([FromBody] RedeemUpgradeCodeRequest req)
        {
            var res = upgradeCodeService.RedeemCode(req.Code);
            //upgradeCodeService.Add

            return Ok(res.ConvertToApiReturn());
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("upgrade/validate")]
        public async Task<IActionResult> ValidateCode([FromBody] RedeemUpgradeCodeRequest req)
        {
            var res = upgradeCodeService.ValidateCode(req.Code);
            //upgradeCodeService.Add

            return Ok(res.ConvertToApiReturn());
        }
    }
}
