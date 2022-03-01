using System;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace ServiceBusAAD_latest
{
    
    class Program
    {

        static void Main(string[] args)
        {
       
            // ManagementClientCredentials().GetAwaiter().GetResult();
            //UserFlow().GetAwaiter().GetResult();
            ManagedIdentity().GetAwaiter().GetResult();


        }

        static async Task ManagementClientCredentials()
        {
            TokenCredential credential = new ClientSecretCredential("{{TenantId}}", "{{clientId}}", "{{clientSecret}}");

           // var _administrationClient = new ServiceBusAdministrationClient("sbus.msazurelabs.com", credential);
            var options = new ServiceBusClientOptions()
            {
                EnableCrossEntityTransactions = true,
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
            var _queueclient = new ServiceBusClient("azbuslab.servicebus.windows.net", credential, options);

            var sender = _queueclient.CreateSender("test");
            ServiceBusMessage message = new ServiceBusMessage("Test");
            while (true)
            {
                await sender.SendMessageAsync(message);
                Console.WriteLine(message.Body);
            }

        }

        static async Task UserFlow()
        {
            var options = new InteractiveBrowserCredentialOptions();
            options.TenantId = "{{tenantId}}";
          
            var credential = new InteractiveBrowserCredential(options);

            var _queueclient = new ServiceBusClient("{{namespace}}.servicebus.windows.net", credential);
            var sender = _queueclient.CreateSender("test");
            ServiceBusMessage message = new ServiceBusMessage("Test");
            while (true)
            {
                await sender.SendMessageAsync(message);
                Console.WriteLine(message.Body);
            }

        }

        static async Task ManagedIdentity()
        {
            var credential = new ManagedIdentityCredential();
            var _queueclient = new ServiceBusClient("{{namespace}}.servicebus.windows.net",credential);
            var sender = _queueclient.CreateSender("test");
            ServiceBusMessage message = new ServiceBusMessage("Test");


            while (true)
            {
                await sender.SendMessageAsync(message);
                Console.WriteLine(message.Body);
            }
        }

     
    }
}
