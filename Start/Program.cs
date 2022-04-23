
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Ninject;
using RadiantBot.CrossCutting.DataClasses;
using RadiantBot.CrossCutting.Logging.Contract;
using RadiantBot.Infrastruktur.Bindings;
using RadiantBot.Logik.Domain.ClientManagement.Contract;
using RadiantBot.Logik.Domain.CommandManagement.Contract;
using RadiantBot.Logik.Domain.ConfigManagement.Contract;
using RadiantBot.Logik.Domain.LoginManagement.Contract;
using RadiantBot.Logik.Domain.MessageManagement.Contract;

namespace RadiantBot.UI.Start
{
    public class Program
    {
        ILoginManager loginManager;
        ICommandHandler commandHandler;
        IServiceProvider serviceProvider;
        DiscordSocketClient client;
        ILogger logger;
        IConfigManager configManager;
        IMessageHandler messageHandler;

        private string token;

        public static Task Main(string[] args) => new Program().MainAsync();

        public async Task MainAsync()
        {

            serviceProvider = new Mapper().ConfigureServices();


            

            loginManager = serviceProvider.GetService<ILoginManager>();
            commandHandler = serviceProvider.GetService<ICommandHandler>();
            client = serviceProvider.GetService<DiscordSocketClient>();
            logger = serviceProvider.GetService<ILogger>();
            configManager = serviceProvider.GetService<IConfigManager>();
            messageHandler = serviceProvider.GetService<IMessageHandler>();
            

            Config cfg = configManager.GetConfig();

            token = cfg.ClientConfig.token;

            await loginManager.Login(client, token);
            await loginManager.Start(client);

            await commandHandler.InstallCommandsAsync();

            await Task.Delay(-1);
        }






    }
}



