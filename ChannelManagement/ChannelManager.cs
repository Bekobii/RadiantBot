using Discord;
using Discord.WebSocket;
using RadiantBot.Logik.Domain.ChannelManagement.Contract;

namespace RadiantBot.Logik.Domain.ChannelManagement
{
    public class ChannelManager : IChannelManager
    {
        private readonly DiscordSocketClient client;

        public ChannelManager(DiscordSocketClient client)
        {
            this.client = client;
        }



        public async Task<IGuildChannel> GetByName(string name, IGuildUser user)
        {
            var channels = await user.Guild.GetChannelsAsync();

            return channels.Single(x => x.Name == name);

        }

    }
}