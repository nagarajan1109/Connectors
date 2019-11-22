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
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System.Configuration;

namespace Json_Convertor_Test.Controllers
{
    public class GetAllEntityListController : ApiController
    {
        //private object serviceProxy;
        //private object entityName;
        //private object cbxEntity;

        GetAllEntity allEntity = new GetAllEntity();
        [Route("api/GetAllEntityList/RetrieveAllEntity")]
        [HttpPost]
        public List<string> RetrieveAllEntity(GetAllEntity allEntity)
        {

            List<string> info = new List<string>();
            // EntityCollection GetAllEntity = new EntityCollection();


            //var client = new HttpClient();
            //client.BaseAddress = new Uri(url);
            //client.DefaultRequestHeaders.Accept.Clear();

            string username = allEntity.username;
            string password = allEntity.password;
            string domain = allEntity.domain;
            
            string srclocation = ConfigurationManager.AppSettings["sourcelocation"].ToString();
            string geturl = ConfigurationManager.AppSettings["commonurl"].ToString();
            IOrganizationService service = null;
            try
            {
                ClientCredentials cre = new ClientCredentials();
                cre.Windows.ClientCredential.Domain = domain;
                cre.Windows.ClientCredential.UserName = username;
                cre.Windows.ClientCredential.Password = password;

                Uri serviceUri = new Uri(srclocation);
                OrganizationServiceProxy proxy = new OrganizationServiceProxy(serviceUri, null, cre, null);
                proxy.EnableProxyTypes();
                service = (IOrganizationService)proxy;
                               
                RetrieveAllEntitiesRequest retrieveAllEntityRequest = new RetrieveAllEntitiesRequest
                {
                    RetrieveAsIfPublished = true,
                    EntityFilters = EntityFilters.Attributes
                };
                RetrieveAllEntitiesResponse retrieveAllEntityResponse = (RetrieveAllEntitiesResponse)service.Execute(retrieveAllEntityRequest);

                EntityCollection collection = new EntityCollection();

                var allEntities = retrieveAllEntityResponse.EntityMetadata;
                foreach (EntityMetadata Entity in allEntities)
                {
                  

                    string getresult = geturl+"AttachmentRecord/RetriveRecords?" + "entityname=" + Entity.LogicalName.ToString();
                    info.Add(getresult);
                    

                }

            

            }
            catch (Exception ex)
            {

                
            }

          

          return info;
          
        }
        [Route("api/GetAllEntityList/GetSecurityData")]
        [HttpGet]
        public List<string> GetSecurityData()
        {
            List<string> info2 = new List<string>();
            // Get the CRM connection string and connect to the CRM Organization
            CrmServiceClient crmConn = new CrmServiceClient(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);
            IOrganizationService crmService = crmConn.OrganizationServiceProxy;

            Guid gUserId = ((WhoAmIResponse)crmService.Execute(new WhoAmIRequest())).UserId;

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
            conditionExpression.Values.Add(gUserId);

            userRoles.LinkCriteria = new FilterExpression();
            userRoles.LinkCriteria.Conditions.Add(conditionExpression);

            systemUseRole.LinkEntities.Add(userRoles);
            querysec.LinkEntities.Add(systemUseRole);

            rolePrivileges.LinkEntities.Add(privilege);
            rolePrivileges.LinkEntities.Add(privilegeObjectTypeCodes);
            querysec.LinkEntities.Add(rolePrivileges);


            EntityCollection retUserRoles = crmService.RetrieveMultiple(querysec);

            Console.WriteLine("Retrieved {0} records", retUserRoles.Entities.Count);
            foreach (Entity rur in retUserRoles.Entities)
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



                switch(((AliasedValue)(rur["P.accessright"])).Value.ToString())
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



                switch(((AliasedValue)(rur["RP.privilegedepthmask"])).Value.ToString())
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

                info2.Add("USER NAME:" + " " + UserName + "/ " + "SECURITY ROLE: " + " " +  SecurityRoleName + " / " + "ENTITY NAME:" + " " + EntityName + "/ " + "PRIVILIGE NAME:" + " " + PriviligeName + "/ " + "ACCESS LEVEL:" + " " + AccessLevel + "/ " + "SECURITY LEVEL:" + " " + SecurityLevel);

            }
            return info2;

        }


    }
}
