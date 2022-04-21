using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Ninject;
using RadiantBot.CrossCutting.Logging;
using RadiantBot.Infrastruktur.Bindings;
using RadiantBot.Logik.Domain.ClientManagement.Contract;
using RadiantBot.Logik.Domain.CommandManagement.Contract;
using RadiantBot.Logik.Domain.CommandManagement.Modules;
using RadiantBot.Logik.Domain.LoginManagement.Contract;

namespace RadiantBot.UI.Start
{
    public class Program
    {
        ILoginManager? loginManager;
        IClientManager? clientManager;
        ICommandHandler? commandHandler;
        IServiceProvider serviceProvider;

        private readonly string token = "NzYzODI4Mzk1MTMzMzcwNDU4.X39YoA.OOBe0lpiZrB8LGXsyrIm0WBNHoM";

        public static Task Main(string[] args) => new Program().MainAsync();

        public async Task MainAsync()
        {

            var kernel = new Mapper().Initialize();
            

            loginManager = kernel.Get<ILoginManager>();
            clientManager = kernel.Get<IClientManager>();
            commandHandler = kernel.Get<ICommandHandler>();
            var client = clientManager.Get();
            await loginManager.Login(client, token);
            await loginManager.Start(client);
            await commandHandler.InstallCommandsAsync(client);

            await Task.Delay(-1);
        }

    
        

      
       
    }
}



