using Discord;
using Discord.WebSocket;
using RadiantBot.CrossCutting.DataClasses;
using RadiantBot.CrossCutting.Logging.Contract;
using RadiantBot.Logik.Domain.ChannelManagement.Contract;
using RadiantBot.Logik.Domain.UserFileManagement.Contract;
using RadiantBot.Logik.Domain.UserManagement.Contract;
using RadiantBot.Logik.Domain.WarnManagement.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadiantBot.Logik.Domain.WarnManagement
{
    public class WarnManager : IWarnManager
    {
        private readonly IChannelLogger logger;
        private readonly IUserManager userManager;
        private readonly IChannelManager channelManager;
        private const string logChannelString = "moderation";

        public WarnManager(IChannelLogger logger, IUserManager userManager, IChannelManager channelManager)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.channelManager = channelManager;
        }


        public async Task AddWarn(ulong id, string reason, IGuildUser moderator)
        {
            var guildUser = await moderator.Guild.GetUserAsync(id);

            Warn warn = new Warn();
            warn.Reason = reason;
            warn.Moderator = moderator.Username;
            warn.Date = DateTime.Now;

            var user = userManager.Get(id, moderator.Guild);

            user.warns.Add(warn);

            await userManager.UpdateUser(user);

            if (HasReachedMaxWarns(id, moderator))
            {

                await BanPlayerMaxWarns(guildUser, moderator);
            }


            var embed = new EmbedBuilder()
                    .WithAuthor(moderator)
                    .WithTitle("Ein Benutzer wurde verwarnt")
                    .WithColor(Color.Red)
                    .AddField("Benutzer", $"{guildUser.Mention}")
                    .AddField("Grund", $"{reason}")
                    .AddField("Gesamte Anzahl", $"{GetWarnsCount(guildUser.Id, guildUser.Guild).Result}")
                    .WithCurrentTimestamp()
                    .Build();



            var logChannel = channelManager.GetByName(logChannelString, (IGuildUser)moderator).Result;
            await logger.LogToChannel(moderator.Guild, (ulong)logChannel.Id, embed);


        }

        public async Task RemoveWarn(ulong id, IGuildUser moderator)
        {
            var guildUser = await moderator.Guild.GetUserAsync(id);

            var user = userManager.Get(id, moderator.Guild);

            if(user.warns.Count > 0)
            {
                user.warns.RemoveAt(user.warns.Count - 1);
            }
            

            await userManager.UpdateUser(user);


            var embed = new EmbedBuilder()
                   .WithAuthor(moderator)
                   .WithTitle("Es wurde eine Verwarnung gelöscht")
                   .WithColor(Color.Red)
                   .AddField("Benutzer", $"{guildUser.Mention}")
                   .AddField("Gesamte Anzahl", $"{GetWarnsCount(user.Id, guildUser.Guild).Result}")
                   .WithCurrentTimestamp()
                   .Build();


            var logChannel = channelManager.GetByName(logChannelString, (IGuildUser)moderator).Result;
            await logger.LogToChannel(moderator.Guild, (ulong)logChannel.Id, embed);

        }

        public async Task RemoveAllWarns(ulong id, IGuildUser moderator)
        {
            var user = userManager.Get(id, moderator.Guild);
            user.warns.Clear();
            await userManager.UpdateUser(user);
        }

        public async Task<int> GetWarnsCount(ulong id, IGuild guild)
        {
            var user = userManager.Get(id, guild);

            return user.warns.Count;
        }

        private bool HasReachedMaxWarns(ulong id, IGuildUser moderator)
        {
            var user = userManager.Get(id, moderator.Guild);

            return user.warns.Count >= 3 ? true : false;
        }

        private async Task BanPlayerMaxWarns(IGuildUser user, IGuildUser moderator)
        {
            var embed = new EmbedBuilder()
                .WithAuthor(moderator)
                .WithTitle("Ein Benutzer wurde automatisch gebannt")
                .WithColor(Color.Red)
                .AddField("Grund", "Maximale Anzahl an Verwarnungen überschritten")
                .AddField("Benutzer", $"{user.Mention}")
                .WithCurrentTimestamp()
                .Build();

            await RemoveAllWarns(user.Id, moderator);
            await user.BanAsync(0, "Du hast die maximale Anzahl an Verwarnungen überschritten");

            var logChannel = channelManager.GetByName(logChannelString, (IGuildUser)moderator).Result;
            await logger.LogToChannel(moderator.Guild, (ulong)logChannel.Id, embed);
        }
    }

}