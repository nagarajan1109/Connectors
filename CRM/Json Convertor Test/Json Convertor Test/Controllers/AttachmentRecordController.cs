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
using System.Configuration;
using System.IO;
using System.Globalization;
using System.DirectoryServices.AccountManagement;

namespace Json_Convertor_Test.Controllers
{
    public class AttachmentRecordController : ApiController
    {
        public string strentityname = null;
        //public string Get(string id)
        //{
        //    //return "value";
        //    return id;
        //}
        [Route("api/AttachmentRecord/RetriveRecords")]
        [HttpGet]
        public List<AnnotationModel> RetriveRecords(string entityname)
        {

             //strentityname = System.Web.HttpContext.Current.Session.ToString();
            //  var getaccid = Guid.Parse("55DE3D83-BFFB-E911-B805-00155D00090F");
            List <AnnotationModel> info = new List<AnnotationModel>();
            EntityCollection annotationRecord = null;
            using (CrmServiceClient crmConn = new CrmServiceClient(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString))
            {

                IOrganizationService crmService = crmConn.OrganizationServiceProxy;

                QueryExpression query1 = new QueryExpression(entityname);
                EntityCollection entitycol = crmService.RetrieveMultiple(query1);

                foreach (Entity en in entitycol.Entities)
                {

                    QueryExpression query = new QueryExpression
                    {
                        EntityName = "annotation",
                        ColumnSet = new ColumnSet("annotationid", "filename", "filesize", "notetext", "subject", "createdon","modifiedon", "mimetype", "documentbody"),
                        Criteria = new FilterExpression
                        {

                            Conditions =
                        {

                        new ConditionExpression

                        {

                        AttributeName = "objectid",

                        Operator = ConditionOperator.Equal,

                       // Values = {getaccid}
                        Values = {en.Id}


                        },


                        new ConditionExpression

                        {

                        AttributeName = "isdocument",

                        Operator = ConditionOperator.Equal,

                        Values = {true}

                        }

                        }

                        }
                    };
                   
                    annotationRecord = crmService.RetrieveMultiple(query);
                    string geturl = ConfigurationManager.AppSettings["commonurl"].ToString();
                    if (annotationRecord != null && annotationRecord.Entities.Count > 0)
                    {
                        AnnotationModel AnnotationModel;
                        for (int i = 0; i < annotationRecord.Entities.Count; i++)
                        {

                            AnnotationModel = new AnnotationModel();

                            if (annotationRecord[i].Contains("annotationid") && annotationRecord[i]["annotationid"] != null)
                                AnnotationModel.Annotationid = (Guid)annotationRecord[i]["annotationid"];

                            if (annotationRecord[i].Contains("filename") && annotationRecord[i]["filename"] != null)
                                AnnotationModel.FileName = annotationRecord[i]["filename"].ToString();

                            if (annotationRecord[i].Contains("filesize") && annotationRecord[i]["filesize"] != null)
                                AnnotationModel.FileLength = annotationRecord[i]["filesize"].ToString();

                            if (annotationRecord[i].Contains("notetext") && annotationRecord[i]["notetext"] != null)
                                AnnotationModel.Notetext = annotationRecord[i]["notetext"].ToString();

                            //if (annotationRecord[i].Contains("subject") && annotationRecord[i]["subject"] != null)
                            //    AnnotationModel.Subject = annotationRecord[i]["subject"].ToString();

                            if (annotationRecord[i].Contains("createdon") && annotationRecord[i]["createdon"] != null)
                            {
                                DateTime create = Convert.ToDateTime(annotationRecord[i]["createdon"].ToString());
                                AnnotationModel.FileCretionTimeUTC = create.ToString("yyyy-MM-dd HH:mm:ss");
                            }

                            if (annotationRecord[i].Contains("modifiedon") && annotationRecord[i]["modifiedon"] != null)
                            {
                                DateTime create2 = Convert.ToDateTime(annotationRecord[i]["modifiedon"].ToString());
                                AnnotationModel.FileLastWriteTimeUTC = create2.ToString("yyyy-MM-dd HH:mm:ss");
                            }

                          

                            if (annotationRecord[i].Contains("filename") && annotationRecord[i]["filename"] != null)
                                AnnotationModel.FileExtension = Path.GetExtension(annotationRecord[i]["filename"].ToString());

                            if (annotationRecord[i].Contains("documentbody") && annotationRecord[i]["documentbody"] != null)
                                AnnotationModel.FileFullPath = geturl+"Attachment?annotationid=" + annotationRecord[i]["annotationid"].ToString();

                           

                            info.Add(AnnotationModel);
                        }
                    }
                    

                }
                return info;
            }

        }
        
        [Route("api/AttachmentRecord/GetSecurityData")]
        [HttpGet]
        public List<string> GetSecurityData()
        {
            
            List<string> info2 = new List<string>();
            
            // Get the CRM connection string and connect to the CRM Organization
            CrmServiceClient crmConn = new CrmServiceClient(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);
            IOrganizationService crmService = crmConn.OrganizationServiceProxy;

                Guid gUserId = ((WhoAmIResponse)crmService.Execute(new WhoAmIRequest())).UserId;
           
                string ranjithid = "A107FE21-5101-EA11-B805-00155D00090F";
                     
              QueryExpression querysec = new QueryExpression();
            querysec.EntityName = "role";
            querysec.ColumnSet = new ColumnSet("name");

            LinkEntity systemUseRole = new LinkEntity();
            systemUseRole.LinkFromEntityName = "role";
            systemUseRole.LinkFromAttributeName = "roleid";
            systemUseRole.LinkToEntityName = "systemuserroles";
            systemUseRole.LinkToAttributeName = "roleid";
            systemUseRole.JoinOperator = JoinOperator.Inner;
            systemUseRole.EntityAlias = "SUR";

            LinkEntity userRoles = new LinkEntity();
            userRoles.LinkFromEntityName = "systemuserroles";
            userRoles.LinkFromAttributeName = "systemuserid";
            userRoles.LinkToEntityName = "systemuser";
            userRoles.LinkToAttributeName = "systemuserid";
            userRoles.JoinOperator = JoinOperator.Inner;
            userRoles.EntityAlias = "SU";
            userRoles.Columns = new ColumnSet("fullname");

            LinkEntity rolePrivileges = new LinkEntity();
            rolePrivileges.LinkFromEntityName = "role";
            rolePrivileges.LinkFromAttributeName = "roleid";
            rolePrivileges.LinkToEntityName = "roleprivileges";
            rolePrivileges.LinkToAttributeName = "roleid";
            rolePrivileges.JoinOperator = JoinOperator.Inner;
            rolePrivileges.EntityAlias = "RP";
            rolePrivileges.Columns = new ColumnSet("privilegedepthmask");

            LinkEntity privilege = new LinkEntity();
            privilege.LinkFromEntityName = "roleprivileges";
            privilege.LinkFromAttributeName = "privilegeid";
            privilege.LinkToEntityName = "privilege";
            privilege.LinkToAttributeName = "privilegeid";
            privilege.JoinOperator = JoinOperator.Inner;
            privilege.EntityAlias = "P";
            privilege.Columns = new ColumnSet("name", "accessright");

            LinkEntity privilegeObjectTypeCodes = new LinkEntity();
            privilegeObjectTypeCodes.LinkFromEntityName = "roleprivileges";
            privilegeObjectTypeCodes.LinkFromAttributeName = "privilegeid";
            privilegeObjectTypeCodes.LinkToEntityName = "privilegeobjecttypecodes";
            privilegeObjectTypeCodes.LinkToAttributeName = "privilegeid";
            privilegeObjectTypeCodes.JoinOperator = JoinOperator.Inner;
            privilegeObjectTypeCodes.EntityAlias = "POTC";
            privilegeObjectTypeCodes.Columns = new ColumnSet("objecttypecode");

            ConditionExpression conditionExpression = new ConditionExpression();
            conditionExpression.AttributeName = "systemuserid";
            conditionExpression.Operator = ConditionOperator.Equal;
            conditionExpression.Values.Add(ranjithid);
            //conditionExpression.Values.Add(gUserId);
            userRoles.LinkCriteria = new FilterExpression();
            userRoles.LinkCriteria.Conditions.Add(conditionExpression);

            systemUseRole.LinkEntities.Add(userRoles);
            querysec.LinkEntities.Add(systemUseRole);

            rolePrivileges.LinkEntities.Add(privilege);
            rolePrivileges.LinkEntities.Add(privilegeObjectTypeCodes);
            querysec.LinkEntities.Add(rolePrivileges);


            EntityCollection retUserRoles = crmService.RetrieveMultiple(querysec);

            Console.WriteLine("Retrieved {0} records", retUserRoles.Entities.Count);

            List<string> GlobalStrings = new List<string>();
            foreach (Entity rur in retUserRoles.Entities)
            {

                if (((AliasedValue)(rur["POTC.objecttypecode"])).Value.ToString() == "account")
                 { 

                string UserName = String.Empty;
                string SecurityRoleName = String.Empty;
                string PriviligeName = String.Empty;
                string AccessLevel = String.Empty;
                string SecurityLevel = String.Empty;
                string EntityName = String.Empty;

                UserName = ((AliasedValue)(rur["SU.fullname"])).Value.ToString();
                SecurityRoleName = (string)rur["name"];
                EntityName = ((AliasedValue)(rur["POTC.objecttypecode"])).Value.ToString();
                PriviligeName = ((AliasedValue)(rur["P.name"])).Value.ToString();



                switch (((AliasedValue)(rur["P.accessright"])).Value.ToString())
                {
                    case "1":
                        AccessLevel = "READ";
                        break;

                    case "2":
                        AccessLevel = "WRITE";
                        break;

                    case "4":
                        AccessLevel = "APPEND";
                        break;

                    case "16":
                        AccessLevel = "APPENDTO";
                        break;

                    case "32":
                        AccessLevel = "CREATE";
                        break;

                    case "65536":
                        AccessLevel = "DELETE";
                        break;

                    case "262144":
                        AccessLevel = "SHARE";
                        break;

                    case "524288":
                        AccessLevel = "ASSIGN";
                        break;

                    default:
                        AccessLevel = "";
                        break;
                }



                switch (((AliasedValue)(rur["RP.privilegedepthmask"])).Value.ToString())
                {
                    case "1":
                        SecurityLevel = "User";
                        break;

                    case "2":
                        SecurityLevel = "Business Unit";
                        break;

                    case "4":
                        SecurityLevel = "Parent: Child Business Unit";
                        break;

                    case "8":
                        SecurityLevel = "Organisation";
                        break;

                    default:
                        SecurityLevel = "";
                        break;
                }


                //Console.WriteLine("User name:" + rur["SU.fullname"]);
                //Console.WriteLine("Security Role name:" + rur["name"]);
                //Console.WriteLine("Privilige name:" + rur["P.name"]);
                //Console.WriteLine("Access Right :" + rur["P.accessright"]);
                //Console.WriteLine("Security Level:" + rur["RP.privilegedepthmask"]);

                info2.Add("USER NAME:" + " " + UserName + "/ " + "SECURITY ROLE: " + " " + SecurityRoleName + " / " + "ENTITY NAME:" + " " + EntityName + "/ " + "PRIVILIGE NAME:" + " " + PriviligeName + "/ " + "ACCESS LEVEL:" + " " + AccessLevel + "/ " + "SECURITY LEVEL:" + " " + SecurityLevel);
            }
            }
            return info2;

        }
    }
}
