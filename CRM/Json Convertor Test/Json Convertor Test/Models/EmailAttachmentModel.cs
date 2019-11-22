using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Json_Convertor_Test.Models
{
    public class EmailAttachmentModel
    {
        public Guid attachmentid { get; set; }
        public string filename { get; set; }
        // public string createdby { get; set; }
        public string filesize { get; set; }
       
        public string activitysubject { get; set; }
        public string body { get; set; }
    }
}