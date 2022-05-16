using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attributes;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RadiantBot.CrossCutting.DataClasses;
using RadiantBot.CrossCutting.Logging.Contract;
using RadiantBot.Logik.Domain.ChannelManagement.Contract;
using RadiantBot.Logik.Domain.ConfigManagement.Contract;
using RadiantBot.Logik.Domain.WarnManagement.Contract;

namespace RadiantBot.Logik.Domain.CommandManagement.Modules
{
    [Group("mod")]
    public class ModerationModule : ModuleBase<SocketCommandContext>
    {


        private readonly IChannelLogger logger;
        private readonly IChannelManager channelManager;
        private const string logChannelString = "moderation";

        public ModerationModule(IChannelLogger logger, IChannelManager channelManager)
        {
            this.logger = logger;
            this.channelManager = channelManager;

        }


        [Command("delete")]
        [Summary("Deletes the last message in the channel")]
        public async Task DeleteMessageAsync()
        {
            if (Context.Channel != null)
            {
                IGuildChannel channel = Context.Channel as IGuildChannel;

                var messages = await Context.Channel.GetMessagesAsync(Context.Message, Direction.Before, 1).FlattenAsync();



                await (Context.Channel as ITextChannel).DeleteMessagesAsync(messages);

                await Task.Delay(200);

                var builder = new EmbedBuilder();


                var embed = builder
                    .WithAuthor(Context.Message.Author)
                    .WithTitle("Es wurde eine Nachricht gelöscht")
                    .WithColor(Color.Red)
                    .AddField("Channel", $"{MentionUtils.MentionChannel(Context.Channel.Id)}")
                    .AddField("Message", $"{messages.First().ToString()}")
                    .WithCurrentTimestamp()
                    .Build();


                var logChannel = channelManager.GetByName(logChannelString, (IGuildUser)Context.User).Result;
                await logger.LogToChannel(Context.Guild, (ulong)logChannel.Id, embed);
            }

        }

        [RequireUserPermission(GuildPermission.ModerateMembers)]
        [Command("mute")]
        [Summary("Mutes a member for a specific time")]
        public async Task MuteUser(IGuildUser user, int minutes, [Remainder] string reason)
        {


            await user.SetTimeOutAsync(new TimeSpan(0, 1, 0));

            var embed = new EmbedBuilder()
                    .WithAuthor(Context.Message.Author)
                    .WithTitle("Es wurde ein Benutzer gestummt")
                    .WithColor(Color.Red)
                    .AddField("Benutzer", $"{user.Mention}")
                    .AddField("Dauer", $"{minutes} Minuten")
                    .AddField("Grund", $"{reason}")
                    .WithCurrentTimestamp()
                    .Build();

            await Context.Message.DeleteAsync();
            var logChannel = channelManager.GetByName(logChannelString, (IGuildUser)Context.User).Result;
            await logger.LogToChannel(Context.Guild, (ulong)logChannel.Id, embed);
        }

        [Group("warn")]
        public class WarnModule : ModuleBase<SocketCommandContext>
        {
            private readonly IChannelLogger logger;
            private readonly IChannelManager channelManager;
            private readonly IWarnManager warnManager;
            private const string logChannelString = "moderation";

            public WarnModule(IChannelLogger logger, IChannelManager channelManager, IWarnManager warnManager)
            {
                this.logger = logger;
                this.channelManager = channelManager;
                this.warnManager = warnManager;
            }

            [RequireUserPermission(GuildPermission.ModerateMembers)]
            [Command("add")]
            public async Task AddWarn(IGuildUser user, [Remainder] string reason)
            {
                await warnManager.AddWarn(user.Id, reason, (IGuildUser)Context.User);

                await Context.Message.DeleteAsync();
            }

            [RequireRole("High-Team")]
            [Command("remove")]
            public async Task RemoveWarn(IGuildUser user)
            {
                await warnManager.RemoveWarn(user.Id, (IGuildUser)Context.User);


               
                await Context.Message.DeleteAsync();
            }

            

        }

    }
}
