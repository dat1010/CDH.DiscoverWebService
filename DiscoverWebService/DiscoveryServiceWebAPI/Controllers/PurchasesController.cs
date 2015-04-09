using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk;
using System.Xml.Linq;
using System.Text;
namespace DiscoveryServiceWebAPI.Controllers
{
    public class PurchasesController : ApiController
    {
        public EntityCollection Get()
        {
            CrmConnection con = new CrmConnection("CRM");
            IOrganizationService serviceTest = new OrganizationService(con);

            QueryExpression query = new QueryExpression("contact");
            query.ColumnSet.AllColumns = true;
            
            EntityCollection result = serviceTest.RetrieveMultiple(query);

            StringBuilder stringBuilder = new StringBuilder();
            XElement xmlElements;
            
            foreach (Entity test in result.Entities)
            {
                xmlElements = new XElement("Contact", test.Attributes.Select(i => new XElement(i.Key.ToString(), i.Value.ToString())));
                stringBuilder.Append(xmlElements.ToString());

            }
            return result;
        }
       
    }
}
