/**
 * 
 * Test code that bypasses login
 * 
 * DO NOT ENABLE IN PRODUCTION
 * 
 * 
 * using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Messages;
namespace DiscoveryServiceWebAPI.Controllers
{
    public class TestContactController : ApiController
    {
        public EntityCollection Get()
        {

            //Might need to call login fucntion first
            CrmConnection con = new CrmConnection("CRM");
            IOrganizationService serviceTest = new OrganizationService(con);


            QueryExpression query = new QueryExpression("contact");
            query.ColumnSet = new ColumnSet(new string[] { "contactid", "firstname", "lastname", "emailaddress1", "address1_line1", "address1_stateorprovince", "address1_postalcode" });
            
            EntityCollection result = serviceTest.RetrieveMultiple(query);


            return result;
        }
        public String Put(EntityCollection accounts)
        {
            if (accounts == null)
            {
                return "Error creating accounts from XML";
            }
            CrmConnection con = new CrmConnection("CRM");
            IOrganizationService service = new OrganizationService(con);

            ExecuteMultipleRequest requestWithResults = new ExecuteMultipleRequest()
            {
                // Assign settings that define execution behavior: continue on error, return responses. 
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = false,
                    ReturnResponses = false
                },
                // Create an empty organization request collection.
                Requests = new OrganizationRequestCollection()
            };

            foreach (Entity acc in accounts.Entities)
            {
                UpdateRequest updateRequest = new UpdateRequest { Target = acc };
                requestWithResults.Requests.Add(updateRequest);
            }

            ExecuteMultipleResponse responseWithResults = (ExecuteMultipleResponse)service.Execute(requestWithResults);

            if (responseWithResults.IsFaulted)
            {
                return "Errors during upload";
            }
            return "Success!";
        }
    }
}
*/