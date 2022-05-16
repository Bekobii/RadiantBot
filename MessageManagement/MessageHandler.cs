using Discord;
using Discord.WebSocket;
using RadiantBot.CrossCutting.Logging.Contract;
using RadiantBot.Logik.Domain.ChannelManagement.Contract;
using RadiantBot.Logik.Domain.ConfigManagement.Contract;
using RadiantBot.Logik.Domain.MessageManagement.Contract;
using RadiantBot.Logik.Domain.WarnManagement.Contract;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace RadiantBot.Logik.Domain.MessageManagement
{
    public class MessageHandler : IMessageHandler
    {
        private readonly DiscordSocketClient client;
        private readonly IConfigManager configManager;
        private readonly IChannelLogger channelLogger;
        private readonly IChannelManager channelManager;
        private readonly IWarnManager warnManager;
        private readonly Regex linkRegex;

        public MessageHandler(DiscordSocketClient client, IConfigManager configManager, IChannelLogger channelLogger, IChannelManager channelManager, IWarnManager warnManager)
        {
            this.client = client;
            this.configManager = configManager;
            this.channelLogger = channelLogger;
            this.channelManager = channelManager;
            this.warnManager = warnManager;
            this.linkRegex = new Regex(@"https:\/\/discord.*");

            client.MessageReceived += HandleMessage;
            client.MessageDeleted += HandleDelete;
           
        }

        private Task HandleDelete(Cacheable<IMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel)
        {

            if(message.HasValue)
            {
                var embed = new EmbedBuilder()
                    .WithAuthor(client.CurrentUser.Username, client.CurrentUser.GetAvatarUrl())
                    .WithTitle("Es wurde eine Nachricht gelöscht")
                    .WithColor(Color.Red)
                    .AddField("Autor", $"{message.Value.Author.Mention}")
                    .AddField("Message", $"{message.Value.Content}")
                    .WithCurrentTimestamp()
                    .Build();

                var logChannel = channelManager.GetByName("gelöschte-nachrichten", (IGuildUser)message.Value.Author).Result;

                var guild = ((IGuildChannel)message.Value.Channel).Guild;

                channelLogger.LogToChannel(guild, logChannel.Id, embed);

                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }

        public async Task HandleMessage(SocketMessage arg)
        {
            if(arg is SocketSystemMessage)
            {
                return;
            }

            var message = (SocketUserMessage)arg;

            if (DoesMessageContainLink(message.Content))
            {
                await WarnPlayer((IGuildUser)message.Author, "Senden eines Discord-Links");
                await message.DeleteAsync();
            }


            var cfg = configManager.GetConfig();

            var blacklist = cfg.Blackwords;

            foreach (var word in blacklist)
            {
                if (message.Content.Contains(word, StringComparison.OrdinalIgnoreCase))
                {
                    await message.DeleteAsync();
                    await LogMessageDeleted(word, (IGuildUser)message.Author);
                    await WarnPlayer((IGuildUser)message.Author, "Aussprechen eines Blackwords");
                    return;
                }
            }

        }

        private Task LogMessageDeleted(string message, IGuildUser user)
        {
            var embed = new EmbedBuilder()
                    .WithAuthor(client.CurrentUser.Username, client.CurrentUser.GetAvatarUrl())
                    .WithTitle("Es wurde eine Nachricht automatisch gelöscht")
                    .WithColor(Color.Red)
                    .AddField("Autor", $"{user.Mention}")
                    .AddField("Message", $"{message}")
                    .WithCurrentTimestamp()
                    .Build();

            var logChannel = channelManager.GetByName("moderation", user).Result;

            channelLogger.LogToChannel(user.Guild, logChannel.Id, embed);

            return Task.CompletedTask;

        }

        private async Task WarnPlayer(IGuildUser user, string reason)
        {
            await warnManager.AddWarn(user.Id, reason, await user.Guild.GetCurrentUserAsync());


        }

        private bool DoesMessageContainLink(string message)
        {
            if(linkRegex.Match(message).Success)
            {
                return true;
            }

            return false;
        }
    }
}