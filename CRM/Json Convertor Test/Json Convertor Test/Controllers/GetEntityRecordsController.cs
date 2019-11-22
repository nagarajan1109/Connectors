using System;
using System.Collections.Generic;
//using System.Linq;
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
    public class GetEntityRecordsController : ApiController
    {
        [HttpGet]
        //List<myEntity> entityList = myEntityCollection.ToList();
        public List<CommonEntity> RetriveAllEntityRecords(string entityname)
        //EntityCollection RetriveAllEntityRecords(string entityname)
        {

            //  var getaccid = Guid.Parse("55DE3D83-BFFB-E911-B805-00155D00090F");
            List<CommonEntity> info = new List<CommonEntity>();
            var contactFinalEntityCollection = new EntityCollection();
            EntityCollection contactEntityCollection = new EntityCollection();
            EntityCollection annotationRecord = null;
            int pageNumber = 1;
            bool moreRecords = true;

            using (CrmServiceClient crmConn = new CrmServiceClient(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString))
            {

                IOrganizationService crmService = crmConn.OrganizationServiceProxy;
                while (moreRecords)
                {
                    var queryExpression = new QueryExpression()
                    {
                        Distinct = false,
                        EntityName = entityname,
                        ColumnSet = new ColumnSet(true),
                        Criteria = new FilterExpression
                        {
                            FilterOperator = LogicalOperator.And,
                            Conditions =
                             {
                             new ConditionExpression("mobilephone", ConditionOperator.NotNull)
                             }
                        },
                        Orders =
                             {
                             new OrderExpression { AttributeName = "createdon", OrderType = OrderType.Descending }
                             },
                        PageInfo =
                             {
                             Count = 5000,
                             PagingCookie = (pageNumber == 1) ? null : contactEntityCollection.PagingCookie,
                             PageNumber = pageNumber++
                             }
                    };

                    contactEntityCollection = crmService.RetrieveMultiple(queryExpression);


                    if (contactEntityCollection.Entities.Count > 0)
                    {
                        contactFinalEntityCollection.Entities.AddRange(contactEntityCollection.Entities);
                    }



                    if (contactEntityCollection != null && contactEntityCollection.Entities.Count > 0)
                    {
                        CommonEntity CommonEntity = new CommonEntity();

                        for (int i = 0; i < contactEntityCollection.Entities.Count; i++)
                        {

                            //if (contactEntityCollection[0].Contains("fullname") && contactEntityCollection[0]["fullname"] != null)
                            CommonEntity.fullname = contactEntityCollection[i]["fullname"].ToString();

                            //if (contactEntityCollection[0].Contains("createdon") && contactEntityCollection[0]["createdon"] != null)
                            CommonEntity.createdon = contactEntityCollection[i]["createdon"].ToString();


                            info.Add(CommonEntity);
                           // entityList.Add(CommonEntity);
                        }

                    }
                    moreRecords = contactEntityCollection.MoreRecords;
                }
               
                return info;
                // return contactFinalEntityCollection;

                // contactFinalEntityCollection.Entities.ToArray();

                // List<CommonEntity> entityList = new List<CommonEntity>(contactFinalEntityCollection);

              
            }


        }


    }
}
    

