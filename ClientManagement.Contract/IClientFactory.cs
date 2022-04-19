using Discord.WebSocket;

namespace RadiantBot.Logik.Domain.ClientManagement.Contract
{
    public interface IClientFactory
    {
        DiscordSocketClient Create();
        void Initialize(DiscordSocketClient client);
    }
}