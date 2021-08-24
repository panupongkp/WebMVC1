using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC1.Models;

namespace WebMVC1.Services
{
    public interface IFileService
    {
        ResponseModel UploadFile();
        ResponseModel DownloadFile();
    }
}
