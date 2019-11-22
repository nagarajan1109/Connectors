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
    public class AllfilterController : ApiController
    {
        [HttpGet]
        public List<EntityModel> RetriveAllFilterRecords(string entityname, int days=0, string createdby="")
        {

            //  var getaccid = Guid.Parse("55DE3D83-BFFB-E911-B805-00155D00090F");
            List<EntityModel> info = new List<EntityModel>();
            
            EntityCollection annotationRecord = null;

            using (CrmServiceClient crmConn = new CrmServiceClient(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString))
            {
                
                    if ((entityname != null) && (days != 0) && (createdby != null))
                    {
                        IOrganizationService crmService = crmConn.OrganizationServiceProxy;

                        QueryExpression query1 = new QueryExpression(entityname);
                        EntityCollection entitycol = crmService.RetrieveMultiple(query1);

                        foreach (Entity en in entitycol.Entities)
                        {

                            QueryExpression query = new QueryExpression
                            {
                                EntityName = entityname,
                                ColumnSet = new ColumnSet("name", "createdby", "createdon", "description", "address1_composite"),

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

                                        AttributeName = "name",

                                        Operator = ConditionOperator.Equal,

                                        Values = {createdby}

                                        }

                                        }

                                }
                            };
                            annotationRecord = crmService.RetrieveMultiple(query);
                        } //for each closing
                          //annotationRecord = crmService.RetrieveMultiple(query);
                        if (annotationRecord != null && annotationRecord.Entities.Count > 0)
                        {
                            EntityModel EntityModel;
                            for (int i = 0; i < annotationRecord.Entities.Count; i++)
                            {

                                EntityModel = new EntityModel();

                                //if (annotationRecord[i].Contains("id") && annotationRecord[i]["id"] != null)
                                //    EntityModel.entityid = (Guid)annotationRecord[i]["id"];

                                if (annotationRecord[i].Contains("name") && annotationRecord[i]["name"] != null)
                                    EntityModel.name = annotationRecord[i]["name"].ToString();

                                //if (annotationRecord[i].Contains("createdby") && annotationRecord[i]["createdby"] != null)
                                //    EntityModel.createdby = annotationRecord[i]["createdby"].ToString();

                                //if (annotationRecord[i].Contains("createdon") && annotationRecord[i]["createdon"] != null)
                                //    EntityModel.createdon = annotationRecord[i]["createdon"].ToString();

                                //if (annotationRecord[i].Contains("createdby") && annotationRecord[i]["createdby"] != null)
                                //    EntityModel.createdbyname = annotationRecord[i]["createdby"].ToString();

                                if (annotationRecord[i].Contains("createdon") && annotationRecord[i]["createdon"] != null)
                                    EntityModel.createdon = annotationRecord[i]["createdon"].ToString();

                                if (annotationRecord[i].Contains("description") && annotationRecord[i]["description"] != null)
                                    EntityModel.description = annotationRecord[i]["description"].ToString();

                                if (annotationRecord[i].Contains("address1_composite") && annotationRecord[i]["address1_composite"] != null)
                                    EntityModel.address = annotationRecord[i]["address1_composite"].ToString();


                                info.Add(EntityModel);
                            }
                        }


                        //}

                    }
                
                return info;
            
                
               

            }
           

        }
    }
}
