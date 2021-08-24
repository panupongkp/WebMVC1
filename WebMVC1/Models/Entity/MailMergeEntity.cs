using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebMVC1.Models
{
    public class MailMergeEntity
    {
        public Guid? id { get; set; }
        public string fileName { get; set; }
        public byte[] fileData { get; set; }
        public string fileExtension { get; set; }
        public string filePath { get; set; }
    }
}
