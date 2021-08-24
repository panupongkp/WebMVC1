using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC1.Models
{
    public class MailMergeModel
    {        
        public string innerText { get; set; }
        public string innerXML { get; set; }
        public string outerXML { get; set; }
        public byte[] bytesdata { get; set; }
    }
}
