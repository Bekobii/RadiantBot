
using Discord;

namespace RadiantBot.Logik.Domain.WarnManagement.Contract
{
    public interface IWarnManager
    {
        Task AddWarn(ulong id, string reason, IGuildUser moderator);
        Task RemoveWarn(ulong id, IGuildUser moderator);
        Task<int> GetWarnsCount(ulong id, IGuild guild);
        Task RemoveAllWarns(ulong id, IGuildUser moderator);
    }
}