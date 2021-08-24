using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC1.Services;
using WebMVC1.Models;

namespace WebMVC1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<ConvertController> _logger;
        private IFileService _fileService;

        public FileController(ILogger<ConvertController> logger, IFileService fileService)
        {
            _logger = logger;
            _fileService = fileService;
        }

        [HttpGet]
        [Route("UploadFile")]
        public IActionResult UploadFile()
        {
            IActionResult response = null;

            try
            {
                var result = new { res = _fileService.UploadFile() };
                response = Wrap.ResponseOK(result);
            }
            catch (Exception ex)
            {
                response = Wrap.ResponseError(null, ex.Message);
            }

            return response;
        }

        [HttpGet]
        [Route("DownloadFile")]
        public IActionResult DownloadFile()
        {
            IActionResult response = null;

            try
            {
                var result = new { res = _fileService.DownloadFile() };
                response = Wrap.ResponseOK(result);
            }
            catch (Exception ex)
            {
                response = Wrap.ResponseError(null, ex.Message);
            }

            return response;
        }
    }
}
