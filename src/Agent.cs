using System;
using System.Text;
using System.Text.Json;
using WatsonWebsocket;
using server.Models;

namespace server
{
    public class Agent : WatsonWsServer
    {
        private Router _contract;

        public Agent(int port, bool ssl = false) : base(null, port, ssl)
        {
            MessageReceived += InternalRouterOperator;
        }

        public Agent Use(Router contract)
        {
            _contract = contract;
            return this;
        }

        private void InternalRouterOperator(object sender, MessageReceivedEventArgs args)
        {
            if (_contract == null || _contract.Count == 0)
                return;

            var recipient = args.IpPort;
            var payload = Encoding.UTF8.GetString(args.Data);

            try
            {
                var data = JsonSerializer.Deserialize<Message>(payload);
                 _contract.Route(
                    new Envelope
                    {
                        Data = data,
                        SendResponse = (message) => SendAsync(recipient, message)
                    }
                );
            }
            catch (JsonException jexp)
            {
                SendAsync(recipient, jexp.Message);
            }
        }
    }
}