using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebMVC1.Models
{
    public class TextEditorEntity
    {
        public Guid? id { get; set; }
        public string EditorData { get; set; }
        public string EditorType { get; set; }
    }
}
