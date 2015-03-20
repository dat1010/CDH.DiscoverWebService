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
using System.Xml.Serialization;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml.Linq;  


namespace DiscoverWebService
{
   
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class DiscoverService : System.Web.Services.WebService
    {
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
            query.ColumnSet = new ColumnSet(new string[] { "contactid", "firstname", "lastname", "emailaddress1", "address1_line1", "address1_stateorprovince", "address1_postalcode" });


            EntityCollection result = service.RetrieveMultiple(query);

            XmlSerializer serializer = new XmlSerializer(typeof(EntityCollection));
            StringBuilder stringBuilder = new StringBuilder();

            XElement xmlElements;
            foreach(Entity test in result.Entities)
            {
                xmlElements = new XElement("Contact", test.Attributes.Select(i => new XElement(i.Key.ToString(), i)));
                stringBuilder.Append(xmlElements.ToString());
                stringBuilder.Append("\n\n");
            }

            // return stringBuilder.ToString();
            return result;
        }

        [WebMethod]
        public string UpdateContacts(string xml)
        {
            CrmConnection con = new CrmConnection("CRM");
            IOrganizationService service = new OrganizationService(con);

            string testXML = "<Contact> <contactid>[contactid, 47a0e5b9-88df-e311-b8e5-6c3be5a8b200]</contactid> <firstname>[firstname, Vincent]</firstname> <lastname>[lastname, Lauriant]</lastname> <emailaddress1>[emailaddress1, vlauriant@adatum.com]</emailaddress1> </Contact><Contact> <contactid>[contactid, 49a0e5b9-88df-e311-b8e5-6c3be5a8b200]</contactid> <firstname>[firstname, Adrian]</firstname> <lastname>[lastname, Dumitrascu]</lastname> <emailaddress1>[emailaddress1, Adrian@adventure-works.com]</emailaddress1> <address1_line1>[address1_line1, 249 Alexander Pl.]</address1_line1> <address1_stateorprovince>[address1_stateorprovince, WA]</address1_stateorprovince> <address1_postalcode>[address1_postalcode, 86372]</address1_postalcode> <address1_composite>[address1_composite, 249 Alexander Pl. WA 86372]</address1_composite> </Contact>";
            XmlSerializer serializer = new XmlSerializer(typeof(string));
            EntityCollection accounts = new EntityCollection();
            

            /*******************************************************************
             * We need to parse this XML then create an entity with the name and order id
             * Then we need up update the new propery values and save the contact.
             *******************************************************************/
            int position =  testXML.IndexOf("[", 0) + 1;
            int valueStart;
            int endPosition;
            string key;
            string value;
            Entity account = new Entity();


            while (position != 0)
            {
                
                valueStart = testXML.IndexOf(",", position) + 2;
                endPosition = testXML.IndexOf("]", position);
                key = testXML.Substring(position, valueStart - position - 2);
                value = testXML.Substring(valueStart, endPosition - (valueStart));
                if (key == "contactid"){
                    account = new Entity();
                    account.Id = new Guid(value);
                    account.LogicalName = "contact";
                    accounts.Entities.Add(account);
                    
                } 
                account.Attributes.Add(new KeyValuePair<String,Object>(key, value));

                position = testXML.IndexOf("[", endPosition) + 1;
            }
            foreach(Entity acc in accounts.Entities)
            {
                service.Update(acc);
            }
            // Create a column set to define which attributes should be retrieved.
            //ColumnSet attributes = new ColumnSet(new string[] { "name", "ownerid" });
           
            // Retrieve the account and its name and ownerid attributes.
            //account = service.Retrieve(account.LogicalName, new Guid("47a0e5b9-88df-e311-b8e5-6c3be5a8b200"), attributes);
            
            // Update the postal code attribute.
            // account["address1_postalcode"] = "98052";

            // Update the account.
            //service.Update(account);

            return "Success";
            
        }
    }
}
