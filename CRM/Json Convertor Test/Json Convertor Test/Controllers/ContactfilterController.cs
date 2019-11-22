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
    public class ContactfilterController : ApiController
    {
        [HttpGet]
        public List<ContactModel> RetriveContactRecords(string entityname, int days = 0, string createdby = "")
        {

            //  var getaccid = Guid.Parse("55DE3D83-BFFB-E911-B805-00155D00090F");
            List<ContactModel> info = new List<ContactModel>();

            EntityCollection annotationRecord = null;

            using (CrmServiceClient crmConn = new CrmServiceClient(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString))
            {
               
                 if ((entityname != null) && (days != 0) && (createdby != null))
                    {
                        IOrganizationService crmService = crmConn.OrganizationServiceProxy;

                        QueryExpression query2 = new QueryExpression(entityname);
                        EntityCollection entitycol = crmService.RetrieveMultiple(query2);

                        foreach (Entity en in entitycol.Entities)
                        {

                            QueryExpression querycontact = new QueryExpression
                            {
                                EntityName = entityname,
                                ColumnSet = new ColumnSet("fullname", "createdon", "description"),

                                Criteria = new FilterExpression
                                {

                                    Conditions =
                                    {

                                    new ConditionExpression

                                    {

                                        AttributeName = "createdon",

                                        Operator = ConditionOperator.LastXDays,

                                       // Values = {getaccid}
                                        Values = {days}


                                    },


                                    new ConditionExpression

                                    {

                                    AttributeName = "fullname",

                                    Operator = ConditionOperator.Like,

                                    Values = {"%"+createdby+"%"}

                                    }

                                    }

                                }
                            };
                            annotationRecord = crmService.RetrieveMultiple(querycontact);
                        } //for each closing
                          //annotationRecord = crmService.RetrieveMultiple(query);
                        if (annotationRecord != null && annotationRecord.Entities.Count > 0)
                        {
                          
                            for (int i = 0; i < annotationRecord.Entities.Count; i++)
                            {

                                ContactModel ContactModel = new ContactModel();

                                //if (annotationRecord[i].Contains("id") && annotationRecord[i]["id"] != null)
                                //    ContactModel.entityid = (Guid)annotationRecord[i]["id"];

                                if (annotationRecord[i].Contains("fullname") && annotationRecord[i]["fullname"] != null)
                                    ContactModel.fullname = annotationRecord[i]["fullname"].ToString();

                                //if (annotationRecord[i].Contains("createdby") && annotationRecord[i]["createdby"] != null)
                                //    ContactModel.createdby = annotationRecord[i]["createdby"].ToString();

                                //if (annotationRecord[i].Contains("createdon") && annotationRecord[i]["createdon"] != null)
                                //    ContactModel.createdon = annotationRecord[i]["createdon"].ToString();

                                //if (annotationRecord[i].Contains("createdby") && annotationRecord[i]["createdby"] != null)
                                //    ContactModel.createdbyname = annotationRecord[i]["createdby"].ToString();

                                if (annotationRecord[i].Contains("createdon") && annotationRecord[i]["createdon"] != null)
                                    ContactModel.createdon = annotationRecord[i]["createdon"].ToString();

                                if (annotationRecord[i].Contains("description") && annotationRecord[i]["description"] != null)
                                    ContactModel.description = annotationRecord[i]["description"].ToString();


                               // ContactModel.info2.Add(ContactModel);

                                info.Add(ContactModel);
                            }
                        }


                        //}

                    }

                
                return info;

            }


        }
    }
}
