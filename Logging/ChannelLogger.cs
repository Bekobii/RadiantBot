using Discord;
using RadiantBot.CrossCutting.Logging.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadiantBot.CrossCutting.Logging
{
    public class ChannelLogger : IChannelLogger
    {

        public async Task LogToChannel(IGuild guild, ulong channelId, Embed embed)
        {
            ITextChannel channel = await guild.GetTextChannelAsync(channelId);

            await channel.SendMessageAsync("", false, embed);
        }

    }
}
