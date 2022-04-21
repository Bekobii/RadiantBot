using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attributes;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RadiantBot.CrossCutting.Logging.Contract;
using RadiantBot.Infrastruktur.Enums;

namespace RadiantBot.Logik.Domain.CommandManagement.Modules
{
    [Group("mod")]
    public class ModerationModule : ModuleBase<SocketCommandContext>
    {

        private readonly IChannelLogger logger;

        public ModerationModule(IChannelLogger logger)
        {
            this.logger = logger;
        }

        [RequireRoleAttribute("High-Team")]
        [Command("delete")]
        [Summary("Deletes the last message in the channel")]
        public async Task DeleteMessageAsync()
        {
            if(Context.Channel != null)
            {
                IGuildChannel channel = Context.Channel as IGuildChannel;

                var messages = await Context.Channel.GetMessagesAsync(Context.Message, Direction.Before, 1).FlattenAsync();


                await (Context.Channel as ITextChannel).DeleteMessagesAsync(messages);

                await Task.Delay(200);

                await Context.Channel.DeleteMessageAsync(Context.Message);

                var builder = new EmbedBuilder();


                var embed = builder
                    .WithAuthor(Context.Message.Author)
                    .WithTitle("Es wurde eine Nachricht gelöscht")
                    .WithColor(Color.Red)
                    .AddField("Channel", $"{MentionUtils.MentionChannel(Context.Channel.Id)}")
                    .AddField("Message", $"{messages.First().ToString()}")
                    .WithCurrentTimestamp()
                    .Build();

                await logger.LogToChannel(Context.Guild, (ulong)Channel.Entwickler, embed);
            }

        }

        //[RequireUserPermission(GuildPermission.ModerateMembers)]
        [Command("mute")]
        [Summary("Mutes a member for a specific time")]
        public async Task MuteUser(IGuildUser user, string reason, int minutes)
        {
            await user.SetTimeOutAsync(new TimeSpan(0, minutes, 0));

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
            await logger.LogToChannel(Context.Guild, (ulong)Channel.Entwickler, embed);
        }





    }
}
