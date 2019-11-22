using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Json_Convertor_Test.Models;

namespace Json_Convertor_Test.Models
{
    public class EntityModel
    {
        //public EntityModel()
        //{
        //    info2 = new List<ContactModel>();
        //}
        //  public Guid entityid { get; set; }
       // public string entityname { get; set; }
        public string name { get; set; }
        public string createdon { get; set; }
       // public string createdbyname { get; set; }
        public string description { get; set; }
        public string address { get; set; }
      //public List<ContactModel> info2 { get; set; }
    }
}