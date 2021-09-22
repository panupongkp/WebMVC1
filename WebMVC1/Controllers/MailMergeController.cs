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
    public class MailMergeController : ControllerBase
    {
        private readonly ILogger<ConvertController> _logger;
        private IMailMergeService _mailMergeService;

        public MailMergeController(ILogger<ConvertController> logger, IMailMergeService mailMergeService)
        {
            _logger = logger;
            _mailMergeService = mailMergeService;
        }
        
        [HttpGet]
        [Route("GetMailMerge")]
        public IActionResult GetMailMerge()
        {
            IActionResult response = null;

            try
            {
                var result = new { res = _mailMergeService.GetMailMerge() };
                response = Wrap.ResponseOK(result);
            }
            catch (Exception ex)
            {
                response = Wrap.ResponseError(null, ex.Message);
            }

            return response;
        }

        [HttpGet]
        [Route("GetMailMergeFile")]
        public IActionResult GetMailMergeFile()
        {
            IActionResult response = null;

            try
            {
                var result = new { res = _mailMergeService.GetMailMergeFile() };
                response = Wrap.ResponseOK(result);
            }
            catch (Exception ex)
            {
                response = Wrap.ResponseError(null, ex.Message);
            }

            return response;
        }

        [HttpGet]
        [Route("GetMailMergePowerTools")]
        public IActionResult GetMailMergePowerTools()
        {
            IActionResult response = null;

            try
            {
                var result = new { res = _mailMergeService.GetMailMergePowerTools() };
                response = Wrap.ResponseOK(result);
            }
            catch (Exception ex)
            {
                response = Wrap.ResponseError(null, ex.Message);
            }

            return response;
        }

        [HttpGet]
        [Route("GetMailMergeText")]
        public IActionResult GetMailMergeText()
        {
            IActionResult response = null;

            try
            {
                var result = new { res = _mailMergeService.GetMailMergeText() };
                response = Wrap.ResponseOK(result);
            }
            catch (Exception ex)
            {
                response = Wrap.ResponseError(null, ex.Message);
            }

            return response;
        }

        [HttpGet]
        [Route("GetMailMergeReplace")]
        public IActionResult GetMailMergeReplace()
        {
            IActionResult response = null;

            try
            {
                var result = new { res = _mailMergeService.GetMailMergeReplace() };
                response = Wrap.ResponseOK(result);
            }
            catch (Exception ex)
            {
                response = Wrap.ResponseError(null, ex.Message);
            }

            return response;
        }

        [HttpGet]
        [Route("UploadMailMerge")]
        public IActionResult UploadMailMerge()
        {
            IActionResult response = null;

            try
            {
                var result = new { res = _mailMergeService.UploadMailMerge() };
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
