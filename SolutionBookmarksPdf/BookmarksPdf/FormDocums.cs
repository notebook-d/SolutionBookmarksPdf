﻿using BookmarksPdf.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BookmarksPdf
{
    public class FormDocums
    {        
        public string DocumentType { get; set; }        
        public string Standard { get; set; }        
        public string FormName { get; set; }
        public DocumentVidEnum DocumentVid { get; set; }

        public FormDocums()
        {
        }

        public Area CehArea { get; set; }

        public override string ToString()
        {
            return $"{DocumentType}|{Standard}|{FormName}|{DocumentVid}";
        }
    }
}
