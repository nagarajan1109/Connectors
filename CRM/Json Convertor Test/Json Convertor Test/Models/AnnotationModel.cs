using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Json_Convertor_Test.Models
{
    public class AnnotationModel
    {
        public Guid Annotationid { get; set; }
        //public Guid entityId { get; set; }
        public string FileName { get; set; }
        public string FileLength { get; set; }
        public string FileExtension { get; set; }
        public string Notetext { get; set; }
        //public string Subject { get; set; }
        public string FileCretionTimeUTC { get; set; }
        public string FileLastWriteTimeUTC { get; set; }
       
        // public string name { get; set; }
        public string FileFullPath { get; set; }
    }
}