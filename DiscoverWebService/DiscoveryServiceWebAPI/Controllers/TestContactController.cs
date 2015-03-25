using System;
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

            XmlSerializer serializer = new XmlSerializer(typeof(EntityCollection));
            StringBuilder stringBuilder = new StringBuilder();

            XElement xmlElements;
            foreach (Entity test in result.Entities)
            {
                xmlElements = new XElement("Contact", test.Attributes.Select(i => new XElement(i.Key.ToString(), i.Value.ToString())));
                stringBuilder.Append(xmlElements.ToString());

            }
            
            return result;
        }
        public string Put(EntityCollection accounts)
        {
            //return "woo";
            CrmConnection con = new CrmConnection("CRM");
            IOrganizationService service = new OrganizationService(con);



            string testXML = "<Contact> <contactid>49a0e5b9-88df-e311-b8e5-6c3be5a8b200</contactid> <firstname>Adrian</firstname> <lastname>Dumitrascu</lastname> <emailaddress1>Adrian@adventure-works.com</emailaddress1> <address1_line1>249 Alexander Pl.</address1_line1> <address1_stateorprovince>WA</address1_stateorprovince> <address1_postalcode>86372</address1_postalcode> <address1_composite>249 Alexander Pl. WA 86372</address1_composite> </Contact>";
            XmlSerializer serializer = new XmlSerializer(typeof(string));
            //EntityCollection accounts = new EntityCollection();


            /*******************************************************************
             * We need to parse this XML then create an entity with the name and order id
             * Then we need up update the new propery values and save the contact.
             *******************************************************************/
            int position = testXML.IndexOf("<", 0) + 1;
            int valueStart;
            int endPosition;
            string key;
            string value;
            Entity account = new Entity();

            /*
            while (position != 0)
            {

                valueStart = testXML.IndexOf(">", position) + 1;
                endPosition = testXML.IndexOf("<", position);
                key = testXML.Substring(position, valueStart - position);
                value = testXML.Substring(valueStart, endPosition - (valueStart));
                if (key != "Contact")
                {
                    if (key == "contactID")
                    {
                        account = new Entity();
                        account.Id = new Guid(value);
                        account.LogicalName = "contact";
                        accounts.Entities.Add(account);
                    }
                    account.Attributes.Add(new KeyValuePair<String, Object>(key, value));

                }

                position = testXML.IndexOf("<", endPosition) + 1;
            }
             */
            foreach (Entity acc in accounts.Entities)
            {
                service.Update(acc);
            }
            

            return "Success";

        }
    }
}
