using Attributes;
using Discord;
using Discord.Commands;
using Discord.Rest;
using RadiantBot.CrossCutting.Logging.Contract;
using RadiantBot.Logik.Domain.ChannelManagement.Contract;
using RadiantBot.Logik.Domain.ConfigManagement.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadiantBot.Logik.Domain.CommandManagement.Modules
{


    [Group("admin")]
    public class AdministrationModule : ModuleBase<SocketCommandContext>
    {
        private readonly IChannelLogger logger;
        private readonly IChannelManager channelManager;
        private const string logChannelString = "team-chat";

        public AdministrationModule(IChannelLogger logger, IConfigManager configManager, IChannelManager channelManager)
        {
            this.logger = logger;
            this.channelManager = channelManager;
        }

        [RequireRole("High-Team")]
        [Command("clean")]
        [Summary("Cleans a whole channel")]
        public async Task Clean()
        {
            ITextChannel currentChannel = Context.Channel as ITextChannel;
           

            ITextChannel newChannel = await Context.Guild.CreateTextChannelAsync(Context.Channel.Name, (TextChannelProperties prop) =>
            {
                prop.CategoryId = currentChannel.CategoryId;
                prop.Position = currentChannel.Position;
                prop.Name = currentChannel.Name;
                
            });

            

            await newChannel.SyncPermissionsAsync();
            await currentChannel.DeleteAsync();

            var embed = new EmbedBuilder()
                    .WithAuthor(Context.Message.Author)
                    .WithTitle("Es wurde ein Channel gesäubert")
                    .WithColor(Color.Red)
                    .AddField("Channel", $"{Context.Channel.Name}")
                    .WithCurrentTimestamp()
                    .Build();

            var logChannel = channelManager.GetByName(logChannelString, (IGuildUser)Context.User).Result;
            await logger.LogToChannel(Context.Guild, (ulong)logChannel.Id, embed);
        }

       

    }
}
