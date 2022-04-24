using Newtonsoft.Json;
using RadiantBot.CrossCutting.DataClasses;
using RadiantBot.Logik.Domain.UserFileManagement.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadiantBot.Logik.Domain.UserFileManagement
{
    public class UserFileManager : IUserFileManager
    {

        private readonly string path = $@"{AppDomain.CurrentDomain.BaseDirectory}\users\users.json";

        public UserFile Get()
        {

            var userFile = new UserFile();

            var file = new FileInfo(path);

            if (!file.Exists)
            {
                file.Directory.Create();

                using (file.Create())
                {

                }

                Initiate(userFile);
            }
            else
            {
                string jsonText = File.ReadAllText(file.FullName);

                userFile = JsonConvert.DeserializeObject<UserFile>(jsonText);
            }


            return userFile;
        }

        private void Initiate(UserFile file)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;

            string jsonText = JsonConvert.SerializeObject(file, settings);

            File.WriteAllText(path, jsonText);
        }

        public async Task Save(UserFile file)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;

            string jsonText = JsonConvert.SerializeObject(file, settings);

            File.WriteAllText(path, jsonText);
        }


    }
}
