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
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

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
    }
}
