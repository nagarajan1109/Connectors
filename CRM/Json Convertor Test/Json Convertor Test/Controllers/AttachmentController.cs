using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Xrm.Sdk.Query;
//using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk.Organization;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;
using Json_Convertor_Test.Models;



namespace Json_Convertor_Test.Controllers
{
    public class AttachmentController : ApiController
    {
        // GET api/values
        [HttpGet]
        public List<Attachment> RetriveRecords(string Annotationid)
        {
            
            using (CrmServiceClient crmConn = new CrmServiceClient(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString))
            {
                IOrganizationService crmService = crmConn.OrganizationServiceProxy;
                //Guid gUserId = ((WhoAmIResponse)crmService.Execute(new WhoAmIRequest())).UserId;


                QueryExpression query = new QueryExpression
                {
                    EntityName = "annotation",
                    ColumnSet = new ColumnSet("annotationid", "filename", "createdby", "createdon", "filesize", "notetext", "subject", "documentbody"),

                      Criteria = new FilterExpression
                      {

                          Conditions =
                             {

                        new ConditionExpression

                        {

                        AttributeName = "annotationid",

                        Operator = ConditionOperator.Equal,

                       // Values = {getaccid}
                        Values = {Annotationid}


                        }
                            }

                      }
                          
                };
                List<Attachment> info = new List<Attachment>();
                EntityCollection annotationRecord = crmService.RetrieveMultiple(query);
                if (annotationRecord != null && annotationRecord.Entities.Count > 0)
                {
                    Attachment Attachment;
                    for (int i = 0; i < annotationRecord.Entities.Count; i++)
                    {

                        Attachment = new Attachment();
                        //if (annotationRecord[i].Contains("annotationid") && annotationRecord[i]["annotationid"] != null)
                        //    Attachment.annotationid = (Guid)annotationRecord[i]["annotationid"];
                        //if (annotationRecord[i].Contains("filename") && annotationRecord[i]["filename"] != null)
                        //    Attachment.filename = annotationRecord[i]["filename"].ToString();

                                          

                        //if (annotationRecord[i].Contains("filesize") && annotationRecord[i]["filesize"] != null)
                        //    Attachment.filesize = annotationRecord[i]["filesize"].ToString();

                        //if (annotationRecord[i].Contains("notetext") && annotationRecord[i]["notetext"] != null)
                        //    Attachment.notetext = annotationRecord[i]["notetext"].ToString();

                        //if (annotationRecord[i].Contains("subject") && annotationRecord[i]["subject"] != null)
                        //    Attachment.subject = annotationRecord[i]["subject"].ToString();

                        if (annotationRecord[i].Contains("documentbody") && annotationRecord[i]["documentbody"] != null)
                            Attachment.documentbody = annotationRecord[i]["documentbody"].ToString();


                        info.Add(Attachment);
                    }
                }
                return info;
               
            }
           
        }

        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
    }
}
