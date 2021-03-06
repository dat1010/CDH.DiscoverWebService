﻿using System;
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
    public class ContactController : ApiController
    {

        private IOrganizationService Service
        {
            get
            {
                string key = "Cached_Service_Key";
                if (System.Web.HttpContext.Current.Cache.Get(key) == null)
                {
                    return null;
                }
                return (IOrganizationService)System.Web.HttpContext.Current.Cache.Get(key);
            }
            set
            {
                if (value != null)
                {
                    string key = "Cached_Service_Key";
                    System.Web.HttpContext.Current.Cache.Insert(key, value);
                }
            }
        }

        public HttpResponseMessage Post([FromBody]CreateUserLogIn login)
        {
            try
            {
                string connection = string.Format("Url={0}; Username={1}; Password={2};", login.url, login.name, login.password);
                CrmConnection crmConnection = CrmConnection.Parse(connection);
                Microsoft.Xrm.Sdk.IOrganizationService conectionService = new OrganizationService(crmConnection);
                if (conectionService != null)
                {
                    conectionService.Execute(new WhoAmIRequest());
                    Service = conectionService;
                    return Request.CreateResponse<String>(HttpStatusCode.OK, "SUCCESS");
                }
                else
                {
                    return Request.CreateResponse<String>(HttpStatusCode.InternalServerError, "ERROR: could not establish connection service");
                }
            }
            catch(Exception e)
            {
                return Request.CreateResponse<String>(HttpStatusCode.InternalServerError, "ERROR: Exception occured: " + e.Message);
            }
        }

        public HttpResponseMessage Get()
        {
            try
            {
                if (Service == null)
                {
                    return Request.CreateResponse<String>(HttpStatusCode.Forbidden, "ERROR: Not logged in");
                }

                QueryExpression query = new QueryExpression("contact");
                query.ColumnSet = new ColumnSet(new string[] { "contactid", "firstname", "lastname", "emailaddress1", "address1_line1", "address1_stateorprovince", "address1_postalcode" });

                EntityCollection result = Service.RetrieveMultiple(query);
                if (result.TotalRecordCountLimitExceeded)
                {
                    return Request.CreateResponse<String>(HttpStatusCode.InternalServerError, "ERROR: Total Record Count Limit Exceeded");
                }


                return Request.CreateResponse<EntityCollection>(HttpStatusCode.OK, result);
            }
            catch(Exception e)
            {
                return Request.CreateResponse<String>(HttpStatusCode.InternalServerError, "ERROR: Exception occured: " + e.Message);
            }
        }
        public HttpResponseMessage Put(EntityCollection accounts)
        {
            try
            {
                if (accounts == null){
                    return Request.CreateResponse<String>(HttpStatusCode.UnsupportedMediaType, "ERROR: Cannot construct EntityCollection from given XML");
                }

                if (Service == null){
                    return Request.CreateResponse<String>(HttpStatusCode.Forbidden, "ERROR: Not logged in");
                }

                ExecuteMultipleRequest requestWithResults = new ExecuteMultipleRequest() {
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

                ExecuteMultipleResponse responseWithResults = (ExecuteMultipleResponse)Service.Execute(requestWithResults);

                if (responseWithResults.IsFaulted)
                {
                    return Request.CreateResponse<String>(HttpStatusCode.InternalServerError, "ERROR: Fault encountered during upload");
                }
                return Request.CreateResponse<String>(HttpStatusCode.OK, "SUCCESS");
            }
            catch(Exception e)
            {
                return Request.CreateResponse<String>(HttpStatusCode.InternalServerError, "ERROR: Exception occured: " + e.Message);
            }
        }


    }
    public class CreateUserLogIn
    {
        public string url { get; set; }
        public string name { get; set; }
        public string password { get; set; }
    }
}
