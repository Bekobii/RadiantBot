using Discord;
using RadiantBot.CrossCutting.DataClasses;
using RadiantBot.Logik.Domain.UserFileManagement.Contract;
using RadiantBot.Logik.Domain.UserManagement.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadiantBot.Logik.Domain.UserManagement
{
    public class UserManager : IUserManager
    {
        private readonly IUserFileManager userFileManager;
        private UserFile userFile;

        public UserManager(IUserFileManager userFileManager)
        {
            this.userFileManager = userFileManager;
            userFile = userFileManager.Get();
        }

        public User Get(ulong id, IGuild guild)
        {

            User? user = null;

            if (DoesExist(id))
            {
                user = userFile.users.Single(x => x.Id == id);
            }
            else
            {
                user = new User();
                user.Id = id;
                user.Name = guild.GetUserAsync(id).Result.Username;

                Add(user);
            }

            return user;
        }

        public List<User> GetList()
        {
            return userFile.users.ToList();
        }

        public bool DoesExist(ulong id)
        {
            return userFile.users.Any(x => x.Id == id);
        }

        public async Task Add(User user)
        {
            userFile.users.Add(user);
            await userFileManager.Save(userFile);
        }

        public async Task Remove(User user)
        {
            //userFile.users.Remove(user);
            //await userFileManager.Save();

            throw new NotImplementedException();
        }

        public async Task UpdateList(List<User> userList)
        {
            userFile.users = userList;
            await userFileManager.Save(userFile);
        }

        public async Task UpdateUser(User user)
        {
            var listUser = userFile.users.Single(x => x.Id == user.Id);
            listUser = user;
            await userFileManager.Save(userFile);
        }



    }
}
