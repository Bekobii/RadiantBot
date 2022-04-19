using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RadiantBot.CrossCutting.Logging;
using RadiantBot.Logik.Domain.ClientManagement.Contract;

namespace RadiantBot.Logik.Domain.ClientManagement
{
    public class ClientFactory : IClientFactory
    {
        private readonly ILogger logger;

        public ClientFactory(ILogger logger)
        {
            this.logger = logger;
        }

        public DiscordSocketClient Create()
        {
            var client = new DiscordSocketClient();
            Initialize(client);

            return client;
        }

        public void Initialize(DiscordSocketClient client)
        {
            client.Log += logger.Log;
        }


    }
}
