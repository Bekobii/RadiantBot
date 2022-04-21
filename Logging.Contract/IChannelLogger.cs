using Discord;

namespace RadiantBot.CrossCutting.Logging.Contract
{
    public interface IChannelLogger
    {
        Task LogToChannel(IGuild guild, ulong channelId, Embed embed);
    }
}