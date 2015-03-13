using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;

// These namespaces are found in the Microsoft.Xrm.Sdk.dll assembly
// located in the SDK\bin folder of the SDK download.
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Client.Services;



// This namespace is found in Microsoft.Crm.Sdk.Proxy.dll assembly
// found in the SDK\bin folder.
using Microsoft.Crm.Sdk.Messages;

using System.Linq;  


namespace DiscoverWebService
{
   
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class DiscoverService : System.Web.Services.WebService
    {
        private OrganizationServiceProxy _serviceProxy;
        private bool IsLoggedIn { get; set; }
        [WebMethod]
        public bool Connect(string url, string userName, string password)
        {
            
            //"Url=https://CDH62CommercialwithCRM.crm.dynamics.com; Username=alans@CDH62CommercialwithCRM.onmicrosoft.com; Password=Vulo5319;"
            try
            {
                string connection = string.Format("Url={0}; Username={1}; Password={2};", url, userName, password);
                CrmConnection crmConnection = CrmConnection.Parse(connection);                         
                Microsoft.Xrm.Sdk.IOrganizationService service = new OrganizationService(crmConnection);
                var response = service.Execute(new WhoAmIRequest());                
                IsLoggedIn = true;
                return true;
            
            }catch
            {
                IsLoggedIn = false;
            }
            return false;

            
        }
        [WebMethod]
        public EntityCollection GetContacts()
        {

            CrmConnection con = new CrmConnection("CRM");
            IOrganizationService service = new OrganizationService(con);
            //Might need to call login fucntion first

        	QueryExpression query = new QueryExpression("contact");
            query.ColumnSet = new ColumnSet(true);

            EntityCollection result = service.RetrieveMultiple(query);
            return result;
        }
    }
}
