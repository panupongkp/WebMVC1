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
    public class TextEditorController : ControllerBase
    {
        private readonly ILogger<ConvertController> _logger;
        private ITextEditorService _textEditorService;

        public TextEditorController(ILogger<ConvertController> logger, ITextEditorService textEditorService)
        {
            _logger = logger;
            _textEditorService = textEditorService;
        }

        [HttpPost]
        [Route("SaveEditorData")]
        public async Task<IActionResult> SaveEditorData([FromBody]RequestTextEditorModel requestTextEditorModel)
        {
            IActionResult response = null;

            try
            {
                var result = new { res = _textEditorService.SaveEditorData(requestTextEditorModel) };
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
