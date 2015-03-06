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
            OrganizationService service;
            //"Url=https://CDH62CommercialwithCRM.crm.dynamics.com; Username=alans@CDH62CommercialwithCRM.onmicrosoft.com; Password=Vulo5319;"
            try
            {
                string connection = string.Format("Url={0}; Username={1}; Password={2};", url, userName, password);
                CrmConnection crmConnection = CrmConnection.Parse(connection);
                service = new OrganizationService(crmConnection);
                IsLoggedIn = true;
                return true;
                
            }catch
            {
                IsLoggedIn = false;
            }
            return false;

            
        }
        [WebMethod]
        public string GetContactsByName(string accountName)
        {
            CrmConnection srm = new CrmConnection("CRM");
            using (ServiceContext svcContext = new ServiceContext(_serviceProxy))
            {

                var query_where3 = from c in svcContext.ContactSet
                                   join a in svcContext.AccountSet
                                   on c.ContactId equals a.PrimaryContactId.Id
                                   where a.Name.Contains(accountName)

                                   select new
                                   {
                                       account_name = a.Name,
                                       contact_name = c.LastName
                                   };

                foreach (var c in query_where3)
                {
                    System.Console.WriteLine("acct: " +
                     c.account_name +
                     "\t\t\t" +
                     "contact: " +
                     c.contact_name);
                }
            }

            if (IsLoggedIn)
            {
                QualifyLeadRequest rs = new QualifyLeadRequest();


            }
            else
            {
                return "Please log in";
            }


            return accountName;
        }
    }
}
