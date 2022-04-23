using Discord;

namespace RadiantBot.Logik.Domain.ChannelManagement.Contract
{
    public interface IChannelManager
    {
        Task<IGuildChannel> GetByName(string name, IGuildUser user);
    }
}