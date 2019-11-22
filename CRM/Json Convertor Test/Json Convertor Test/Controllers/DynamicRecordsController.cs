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
    public class DynamicRecordsController : ApiController
    {
        [HttpGet]
        public List<CommonModel> DynamicEntityRecords(string entityname)
        {
            
            List<CommonModel> info = new List<CommonModel>();
            using (CrmServiceClient crmConn = new CrmServiceClient(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString))
            {
                IOrganizationService crmService = crmConn.OrganizationServiceProxy;
                //var columns = new ColumnSet();
                //columns.AllColumns = true;

                //var query = new QueryExpression(entityname);
                //query.ColumnSet = columns;

                // var results = service.RetrieveMultiple(query);
                EntityCollection annotationRecord = null;
                List<string> sColumn = new List<string>();
                List<object> sColumn2 = new List<object>();

                QueryExpression query1 = new QueryExpression(entityname);
                EntityCollection entitycol = crmService.RetrieveMultiple(query1);
                foreach (Entity en in entitycol.Entities)
                {

                    QueryExpression query = new QueryExpression
                    {
                        EntityName = entityname,
                        ColumnSet = new ColumnSet() { AllColumns = true }
                    };

                     annotationRecord = crmService.RetrieveMultiple(query);
                }
                CommonModel CommonModel = new CommonModel();
                if (annotationRecord != null && annotationRecord.Entities.Count > 0)
                {

                    for (int i = 0; i < annotationRecord.Entities.Count; i++)
                    {
                        foreach (string str in annotationRecord.Entities[i].Attributes.Keys)
                        {
                            sColumn.Add(str);

                        }


                        foreach (object str2 in annotationRecord.Entities[i].Attributes.Values)
                        {
                            sColumn2.Add(str2);
                        }

                        //CommonModel = new CommonModel();


                        if (sColumn[0] != null)
                        {
                            CommonModel.field1 = sColumn2[0].ToString();

                        }
                        if (sColumn[1] != null)
                        {
                            CommonModel.field2 = sColumn2[1].ToString();

                        }
                        if (sColumn[2] != null)
                        {
                            CommonModel.field3 = sColumn2[2].ToString();

                        }
                        if (sColumn[3] != null)
                        {
                            CommonModel.field4 = sColumn2[3].ToString();

                        }
                        if (sColumn[4] != null)
                        {
                            CommonModel.field5 = sColumn2[4].ToString();

                        }
                        if (sColumn[5] != null)
                        {
                            CommonModel.field6 = sColumn2[5].ToString();

                        }
                        if (sColumn[6] != null)
                        {
                            CommonModel.field7 = sColumn2[6].ToString();

                        }
                        if (sColumn[7] != null)
                        {
                            CommonModel.field8 = sColumn2[7].ToString();

                        }
                        if (sColumn[8] != null)
                        {
                            CommonModel.field9 = sColumn2[8].ToString();

                        }
                        if (sColumn[9] != null)
                        {
                            CommonModel.field10 = sColumn2[9].ToString();

                        }
                        if (sColumn[10] != null)
                        {
                            CommonModel.field11 = sColumn2[10].ToString();

                        }
                        if (sColumn[11] != null)
                        {
                            CommonModel.field12 = sColumn2[11].ToString();

                        }
                        if (sColumn[12] != null)
                        {
                            CommonModel.field13 = sColumn2[12].ToString();

                        }
                        if (sColumn[13] != null)
                        {
                            CommonModel.field14 = sColumn2[13].ToString();

                        }
                        if (sColumn[14] != null)
                        {
                            CommonModel.field15 = sColumn2[14].ToString();

                        }
                        if (sColumn[15] != null)
                        {
                            CommonModel.field16 = sColumn2[15].ToString();

                        }
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}
                        //if (sColumn[0] != null)
                        //{
                        //    CommonModel.field1 = sColumn2[0].ToString();

                        //}

                        //if (annotationRecord[i]["1"] != null)
                        //    CommonModel.field2 = annotationRecord[i]["1"].ToString();

                        //if (annotationRecord[i]["2"] != null)
                        //    CommonModel.field3 = annotationRecord[i]["2"].ToString();

                        //if (annotationRecord[i].Contains("filename") && annotationRecord[i]["filename"] != null)
                        //    AnnotationModel.filename = annotationRecord[i]["filename"].ToString();





                        info.Add(CommonModel);
                    }
                }
                

            }
            return info;
        }
    }
}