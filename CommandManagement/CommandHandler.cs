using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RadiantBot.CrossCutting.Logging.Contract;
using RadiantBot.Logik.Domain.CommandManagement.Contract;
using RadiantBot.Logik.Domain.CommandManagement.Modules;
using System.Reflection;

namespace RadiantBot.Logik.Domain.CommandManagement
{
    public class CommandHandler : ICommandHandler
    {
        private readonly CommandService service;
        private readonly ILogger logger;
        private DiscordSocketClient? client;

        public CommandHandler(CommandService service, ILogger logger)
        {
            this.service = service;
            this.logger = logger;
        }            

        public async Task InstallCommandsAsync(DiscordSocketClient client)
        {
            this.client = client;
            client.MessageReceived += HandleCommandAsync;


            await service.AddModuleAsync(typeof(ModerationModule), null);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = (SocketUserMessage)arg;


            if (message == null)
            {
                return;
            }

         
            int argPos = 0;

            var IsValid = message.HasCharPrefix('!', ref argPos) ||
                message.HasMentionPrefix(client?.CurrentUser, ref argPos) ||
                message.Author.IsBot;

      
            if (!IsValid)
            {
                return;
            }


            var context = new SocketCommandContext(client, message);

            await service.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null);
        }
    }
}