using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC1.Models;

namespace WebMVC1.Services
{
    public interface IConvertService
    {
        ResponseModel ConvertHTMLtoRTF(InputRequestModel html);
        ResponseModel ConvertRTFtoHTML(InputRequestModel rtf);
        ResponseModel ShortenURL(string url);
    }
}
