using Discord;
using RadiantBot.CrossCutting.DataClasses;

namespace RadiantBot.Logik.Domain.UserManagement.Contract
{
    public interface IUserManager
    {
        Task Add(User user);
        bool DoesExist(ulong id);
        User Get(ulong id, IGuild guild);
        List<User> GetList();
        Task Remove(User user);
        Task UpdateList(List<User> userList);
        Task UpdateUser(User user);
    }
}