using Newtonsoft.Json;
using SautinSoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebMVC1.Models;
using System.IO;
using HtmlToOpenXml;
using OpenXmlHelpers.Word;
using System.Xml.Linq;
using WebMVC1.DBContext;

namespace WebMVC1.Services
{
    public class TextEditorService : ITextEditorService
    {
        private readonly FWContext _textEditorContext;

        public TextEditorService(FWContext textEditorContext)
        {
            _textEditorContext = textEditorContext;
        }

        public ResponseModel SaveEditorData(RequestTextEditorModel requestTextEditorModel)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                var textEditorEntity = new TextEditorEntity
                {
                    id = Guid.NewGuid(),
                    EditorData = requestTextEditorModel.data,
                    EditorType = requestTextEditorModel.type
                };

                _textEditorContext.textEditorEntities.Add(textEditorEntity);
                _textEditorContext.SaveChanges();

                response.status = 200;
                response.success = true;
                response.data = "Save Success";
                response.message = "Success";
            }
            catch(Exception ex)
            {
                response.status = 500;
                response.success = false;
                response.data = "SaveEditorData Error: " + ex.Message + " - " + ex?.InnerException?.Message + " " + ex.ToString();
                response.message = "Failed";
            }

            return response;
        }
    }
}