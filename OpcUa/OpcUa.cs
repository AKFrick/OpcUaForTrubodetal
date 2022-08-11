using System;
using System.Collections.Generic;
using System.Text;
using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;

namespace OpcUaForTrubodetal
{
    public class OpcUa
    {
        public string URI { get; set; } = "opc.tcp://10.10.10.98:4840/";
        ApplicationInstance application;
        ApplicationConfiguration configuration;
        SessionReconnectHandler reconnectHandler;
        Browser browser;
        Session session;

        private string sendTaskID = "ns=3;s=\"OpcUaMethodSendNewTask\"";
        private string sendTaskMethodID = "ns=3;s=\"OpcUaMethodSendNewTask\".Method";
        private NodeId sendTaskNodeID;
        private NodeId sendTaskMethodNodeID;

        public OpcUa()
        {
            application = new ApplicationInstance()
            {
                ApplicationName = "OPUA",
                ApplicationType = ApplicationType.Client,
                ConfigSectionName = "Client"
            };
            sendTaskNodeID = new NodeId(sendTaskID);
            sendTaskMethodNodeID = new NodeId(sendTaskMethodID);
        }

        public async void SendTask(ProductionTaskExtended task)
        {
            configuration = await application.LoadApplicationConfiguration(false);
            var selectedEndpoint = CoreClientUtils.SelectEndpoint(URI, false, 15000);
            var endpointConfiguration = EndpointConfiguration.Create(configuration);
            var endpoint = new ConfiguredEndpoint(null, selectedEndpoint, endpointConfiguration);

            using (session = await Session.Create(configuration,
                endpoint,
                false,
                "OPC UA Client",
                60000,
                new UserIdentity(new AnonymousIdentityToken()),
                null))
            {
                object[] inputs = new object[10]
                {
                    (Int32)(task.Task.ID),
                    (string)(task.Task.TaskNumber ?? "БЕЗ НОМЕРА"),
                    (Int16)(task.Task.Diameter ?? 0),
                    (Int16)(task.Task.Thickness ?? 0),
                    (float)(task.Task.PieceLength1 ?? 0),
                    (Int16)(task.Task.PieceQuantity1 ?? 0),
                    (Int16)(task.serialLabel.StartSerial),
                    (string)task.Task.Labeling1Piece1 ?? "",
                    (string)task.Task.Labeling2Piece1 ?? "",
                    (string)task.serialLabel.EndLabel
                };

                CallMethod(sendTaskNodeID, sendTaskMethodNodeID, inputs);
            }
        }
        public void CallMethod(NodeId nodeId, NodeId methodId, object[] inputArgument)
        {
            session.Call(nodeId, methodId, inputArgument);
            //CallMethodRequest request = new CallMethodRequest();

            //request.ObjectId = nodeId;
            //request.MethodId = methodId;
            //request.InputArguments = inputArgument;

            //CallMethodRequestCollection requests = new CallMethodRequestCollection();
            //requests.Add(request);

            //CallMethodResultCollection results;
            //DiagnosticInfoCollection diagnosticInfos;

            //ResponseHeader responseHeader = session.Call(
            //    null,
            //    requests,
            //    out results,
            //    out diagnosticInfos);            
            
        }   
    }
}
