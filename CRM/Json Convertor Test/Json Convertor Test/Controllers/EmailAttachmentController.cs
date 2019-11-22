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
    public class EmailAttachmentController : ApiController
    {

        [HttpGet]
        public List<EmailAttachmentModel> EmailAttachmentRecords(string mimetype)
        {

            using (CrmServiceClient crmConn = new CrmServiceClient(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString))
            {
               // var emailid = Guid.Parse("84B6587A-DFFB-E911-B805-00155D00090F");
              
                IOrganizationService crmService = crmConn.OrganizationServiceProxy;
               //Entity ActivityMimeAttachment = new Entity();
               // ActivityMimeAttachment.LogicalName = "ActivityMimeAttachment";
               // Entity Email = new Entity();
               // Email.LogicalName = "email";
                QueryExpression query = new QueryExpression
                {


                    EntityName = "activitymimeattachment",


                    ColumnSet = new ColumnSet("activitymimeattachmentid","filename", "filesize", "activitysubject", "body"),

                    Criteria = new FilterExpression
                    {
                        Conditions =
                       {
                           //new ConditionExpression
                           //{
                           //    AttributeName = "objectid",
                           //    Operator = ConditionOperator.Equal,
                           //    Values = { emailid }
                           //},
                           new ConditionExpression
                           {
                               AttributeName = "mimetype",
                               Operator = ConditionOperator.Equal,
                               Values = {mimetype}
                           }
                       }
                    }


                };
                List<EmailAttachmentModel> info = new List<EmailAttachmentModel>();
                EntityCollection annotationRecord = crmService.RetrieveMultiple(query);
               // DataCollection<Entity> annotationRecord = crmService.RetrieveMultiple(query).Entities;
                if (annotationRecord != null && annotationRecord.Entities.Count > 0)
                {
                    EmailAttachmentModel AnnotationModel;
                    for (int i = 0; i < annotationRecord.Entities.Count; i++)
                    {

                        AnnotationModel = new EmailAttachmentModel();
                        if (annotationRecord[i].Contains("activitymimeattachmentid") && annotationRecord[i]["activitymimeattachmentid"] != null)
                            AnnotationModel.attachmentid = (Guid)annotationRecord[i]["activitymimeattachmentid"];

                        if (annotationRecord[i].Contains("filename") && annotationRecord[i]["filename"] != null)
                            AnnotationModel.filename = annotationRecord[i]["filename"].ToString();

                        //if (annotationRecord[i].Contains("createdby") && annotationRecord[i]["createdby"] != null)
                        //    AnnotationModel.createdby = annotationRecord[i]["createdby"].ToString();

                      
                        if (annotationRecord[i].Contains("filesize") && annotationRecord[i]["filesize"] != null)
                            AnnotationModel.filesize = annotationRecord[i]["filesize"].ToString();

                        
                        if (annotationRecord[i].Contains("activitysubject") && annotationRecord[i]["activitysubject"] != null)
                            AnnotationModel.activitysubject = annotationRecord[i]["activitysubject"].ToString();

                        if (annotationRecord[i].Contains("body") && annotationRecord[i]["body"] != null)
                            AnnotationModel.body = annotationRecord[i]["body"].ToString();


                        info.Add(AnnotationModel);
                    }
                }
                return info;

            }

        }
    }
}
