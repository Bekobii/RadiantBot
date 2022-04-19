using Discord.WebSocket;

namespace RadiantBot.Logik.Domain.LoginManagement.Contract
{
    public interface ILoginManager
    {

        public Task Login(DiscordSocketClient client, string token);

    }
}