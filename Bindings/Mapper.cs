﻿
using Ninject;

using RadiantBot.CrossCutting.Logging;
using RadiantBot.Logik.Domain.ClientManagement;
using RadiantBot.Logik.Domain.ClientManagement.Contract;
using RadiantBot.Logik.Domain.CommandManagement;
using RadiantBot.Logik.Domain.CommandManagement.Contract;
using RadiantBot.Logik.Domain.LoginManagement;
using RadiantBot.Logik.Domain.LoginManagement.Contract;
using RadiantBot.CrossCutting.Logging.Contract;
using RadiantBot.Logik.Domain.CommandManagement.Modules;
using Microsoft.Extensions.DependencyInjection;
using Discord.Commands;
using Discord.WebSocket;
using RadiantBot.Logik.Domain.ConfigManagement;
using RadiantBot.Logik.Domain.ConfigManagement.Contract;
using RadiantBot.CrossCutting.DataClasses;
using RadiantBot.CrossCutting.DataClasses.Configs;

namespace RadiantBot.Infrastruktur.Bindings
{
    public class Mapper
    {
        private readonly CommandService service;
        private readonly DiscordSocketClient client;
        private readonly RoleConfig roleConfig;
        private readonly ClientConfig clientConfig;

        public Mapper(CommandService service = null, DiscordSocketClient client = null, RoleConfig roleConfig = null, ClientConfig clientConfig = null)
        {
            this.service = service ?? new CommandService();
            this.client = client ?? new DiscordSocketClient();
            this.roleConfig = roleConfig ?? new RoleConfig();
            this.clientConfig = clientConfig ?? new ClientConfig();
        }


        public IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddScoped<ILogger, Logger>()
                .AddScoped<IChannelLogger, ChannelLogger>()
                .AddScoped<ICommandHandler, CommandHandler>()
                .AddScoped<IClientFactory, ClientFactory>()
                .AddScoped<IClientManager, ClientManager>()
                .AddScoped<ILoginManager, LoginManager>()
                .AddScoped<IConfigManager, ConfigManager>()
                .AddSingleton<ModerationModule>()
                .AddSingleton(service)
                .AddSingleton(client)
                .AddSingleton(roleConfig)
                .AddSingleton(clientConfig)
                .BuildServiceProvider();

        }


      
    }
}