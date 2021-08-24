﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC1.Models
{
    public class ResponseModel
    {
        public int status { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public string errorCode { get; set; }
        public object data { get; set; }
        public object error { get; set; }
    }
}
