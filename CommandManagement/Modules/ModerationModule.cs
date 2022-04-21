using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attributes;
using Discord;
using Discord.Commands;
using RadiantBot.CrossCutting.Logging.Contract;

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


        //[RequireRoleAttribute("High-Team")]
        [Command("clear")]
        [Summary("Deletes a specific amount of messages in the current channel")]
        public async Task InfoAsync([Summary("The amount of messages to be deleted")]int num)
        {


            var messages = await Context.Channel.GetMessagesAsync(Context.Message, Direction.Before, num).FlattenAsync();

            await (Context.Channel as ITextChannel).DeleteMessagesAsync(messages);

            await Task.Delay(200);

            await Context.Channel.DeleteMessageAsync(Context.Message);

            var builder = new EmbedBuilder();

            var embed = builder
                .WithAuthor(Context.Client.CurrentUser)
                .WithTitle("Es wurden Nachrichten gelöscht")
                .WithColor(Color.Red)
                .WithDescription($"{Context.User.Mention} hat {num} Nachrichten gelöscht")
                .Build();

            await logger.LogToChannel(Context.Guild, 960936244848783412, embed);

        }

    }
}
