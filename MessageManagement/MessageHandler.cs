using Discord;
using Discord.WebSocket;
using RadiantBot.CrossCutting.Logging.Contract;
using RadiantBot.Logik.Domain.ChannelManagement.Contract;
using RadiantBot.Logik.Domain.ConfigManagement.Contract;
using RadiantBot.Logik.Domain.MessageManagement.Contract;

namespace RadiantBot.Logik.Domain.MessageManagement
{
    public class MessageHandler : IMessageHandler
    {
        private readonly DiscordSocketClient client;
        private readonly IConfigManager configManager;
        private readonly IChannelLogger channelLogger;
        private readonly IChannelManager channelManager;

        public MessageHandler(DiscordSocketClient client, IConfigManager configManager, IChannelLogger channelLogger, IChannelManager channelManager)
        {
            this.client = client;
            this.configManager = configManager;
            this.channelLogger = channelLogger;
            this.channelManager = channelManager;
            client.MessageReceived += HandleMessage;
        }

        public async Task HandleMessage(SocketMessage arg)
        {
            var message = (SocketUserMessage)arg;

            var cfg = configManager.GetConfig();

            var blacklist = cfg.Blackwords;

            foreach (var word in blacklist)
            {
                if (message.Content.Contains(word))
                {
                    await message.DeleteAsync();
                    await LogMessageDeleted(word, (IGuildUser)message.Author);
                    return;
                }
            }


        }

        private Task LogMessageDeleted(string message, IGuildUser user)
        {
            var embed = new EmbedBuilder()
                    .WithAuthor(client.CurrentUser)
                    .WithTitle("Es wurde eine Nachricht automatisch gelöscht")
                    .WithColor(Color.Red)
                    .AddField("Autor", $"{user.Mention}")
                    .AddField("Message", $"{message}")
                    .WithCurrentTimestamp()
                    .Build();

            var logChannel = channelManager.GetByName("team-chat", user).Result;

            channelLogger.LogToChannel(user.Guild, logChannel.Id, embed);

            return Task.CompletedTask;

        }
    }
}