using Discord.WebSocket;
using Ninject;


using RadiantBot.Infrastruktur.Bindings;
using RadiantBot.Logik.Domain.LoginManagement.Contract;

namespace RadiantBot.UI.Start
{
    public class Program
    {
        ILoginManager manager;

        private readonly string token = "NzYzODI4Mzk1MTMzMzcwNDU4.X39YoA.OOBe0lpiZrB8LGXsyrIm0WBNHoM";

        public static Task Main(string[] args) => new Program().MainAsync();

        public async Task MainAsync()
        {
            var kernel = new Mapper().Initialize();
            manager = kernel.Get<ILoginManager>();

            var client = new DiscordSocketClient();
            
            await manager.Login(client, token);
            await client.StartAsync();


            await Task.Delay(-1);
        }
    }
}



