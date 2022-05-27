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
    public class ConvertController : ControllerBase
    {
        private readonly ILogger<ConvertController> _logger;
        private IConvertService _convertService;

        public ConvertController(ILogger<ConvertController> logger, IConvertService convertService)
        {
            _logger = logger;
            _convertService = convertService;
        }

        [HttpPost]
        [Route("ConvertHTMLtoRTF")]
        public IActionResult ConvertHTMLtoRTF(InputRequestModel request)
        {
            IActionResult response = null;

            try
            {
                var result = new { res = _convertService.ConvertHTMLtoRTF(request) };
                response = Wrap.ResponseOK(result);
            }
            catch (Exception ex)
            {
                response = Wrap.ResponseError(null, ex.Message);
            }

            return response;
        }

        [HttpPost]
        [Route("ConvertRTFtoHTML")]
        public IActionResult ConvertRTFtoHTML(InputRequestModel request)
        {
            IActionResult response = null;

            try
            {
                var result = new { res = _convertService.ConvertRTFtoHTML(request) };
                response = Wrap.ResponseOK(result);
            }
            catch (Exception ex)
            {
                response = Wrap.ResponseError(null, ex.Message);
            }

            return response;
        }

        [HttpGet]
        [Route("ShortenURL")]
        public IActionResult ShortenURL(string request)
        {
            IActionResult response = null;
            try
            {
                var result = new { res = _convertService.ShortenURL(request) };
                response = Wrap.ResponseOK(result);
            }
            catch (Exception ex)
            {
                response = Wrap.ResponseError(null, ex.Message);
            }

            return response;
        }

        [HttpGet]
        [Route("PolicyInfo/{docID}/{docRequest}")]
        public IActionResult PolicyInfo(string docID, string docRequest, string docType)
        {
            IActionResult response = null;
            try
            {
                var result = new { };
                response = Wrap.ResponseOK(result);
            }
            catch (Exception ex)
            {
                response = Wrap.ResponseError(null, ex.Message);
            }

            return response;
        }

        [HttpGet]
        [Route("PolicyInfo/{docID}/content")]
        public IActionResult PolicyInfoContent(string docID, string docType)
        {
            IActionResult response = null;
            try
            {
                var result = new { };
                response = Wrap.ResponseOK(result);
            }
            catch (Exception ex)
            {
                response = Wrap.ResponseError(null, ex.Message);
            }

            return response;
        }

        [HttpGet]
        [Route("PolicyInfo/{docID}/documents")]
        public IActionResult PolicyInfoDocuments(string docID)
        {
            IActionResult response = null;
            try
            {
                var result = new { };
                response = Wrap.ResponseOK(result);
            }
            catch (Exception ex)
            {
                response = Wrap.ResponseError(null, ex.Message);
            }

            return response;
        }

        [HttpGet]
        [Route("PolicyInfo/{docID}/deliveryInfo")]
        public IActionResult PolicyInfoDelivery(string docID)
        {
            IActionResult response = null;
            try
            {
                var result = new { };
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
