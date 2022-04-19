using Discord.WebSocket;
using RadiantBot.Logik.Domain.LoginManagement.Contract;

namespace RadiantBot.Logik.Domain.LoginManagement
{
    public class LoginManager : ILoginManager
    {
        public async Task Login(DiscordSocketClient client, string token)
        {
            await client.LoginAsync(Discord.TokenType.Bot, token);
        }

        public async Task Start(DiscordSocketClient client)
        {
            await client.StartAsync();
        }
    }
}