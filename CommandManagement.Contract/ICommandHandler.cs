
using Discord.WebSocket;

namespace RadiantBot.Logik.Domain.CommandManagement.Contract
{
    public interface ICommandHandler
    {
        Task InstallCommandsAsync(DiscordSocketClient client);
    }
}