using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;


namespace RadiantBot.Logik.Domain.CommandManagement.Modules
{
    [Group("mod")]
    public class ModerationModule : ModuleBase<SocketCommandContext>
    {
        [Command("clear")]
        public async Task InfoAsync()
        {
            var messages = await Context.Channel.GetMessagesAsync(Context.Message, Direction.Before).FlattenAsync();

            await (Context.Channel as ITextChannel).DeleteMessagesAsync(messages);

            await Task.Delay(200);

            Context.Channel.DeleteMessageAsync(Context.Message);
        }

    }
}
