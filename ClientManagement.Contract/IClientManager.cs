using Discord.WebSocket;

namespace RadiantBot.Logik.Domain.ClientManagement.Contract
{
    public interface IClientManager
    {
        DiscordSocketClient Get();
    }
}