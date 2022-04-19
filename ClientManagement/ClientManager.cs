using Discord.WebSocket;
using RadiantBot.CrossCutting.Logging;
using RadiantBot.Logik.Domain.ClientManagement.Contract;

namespace RadiantBot.Logik.Domain.ClientManagement
{
    public class ClientManager : IClientManager
    {
        private readonly IClientFactory factory;
        private readonly ILogger logger;

        public ClientManager(IClientFactory factory, ILogger logger)
        {
            this.factory = factory;
            this.logger = logger;
        }

        public DiscordSocketClient Get()
        {
            var client = factory.Create();
            return client;
        }



    }
}