using System.IO;
using Newtonsoft.Json;
using RadiantBot.CrossCutting.DataClasses;
using RadiantBot.CrossCutting.DataClasses.Configs;
using RadiantBot.Logik.Domain.ConfigManagement.Contract;

namespace RadiantBot.Logik.Domain.ConfigManagement
{
    public class ConfigManager : IConfigManager
    {

        private readonly string path = $@"{AppDomain.CurrentDomain.BaseDirectory}\config\config.json";
        private readonly ClientConfig clientConfig;
        private readonly RoleConfig roleConfig;
        private Config cfg;

        public ConfigManager(ClientConfig clientConfig, RoleConfig roleConfig)
        {
            this.clientConfig = clientConfig;
            this.roleConfig = roleConfig;
        }

        public Config GetConfig()
        {
            cfg = new Config(clientConfig, roleConfig);
            var file = new FileInfo(path);

            if (!DoesConfigExist())
            {
                file.Directory.Create();

                using (file.Create())
                {

                }

                InitiateConfig(cfg);
            }
            else
            {
                string jsonText = File.ReadAllText(path);

                cfg = JsonConvert.DeserializeObject<Config>(jsonText);
            }


            return cfg;
        }

        private bool DoesConfigExist()
        {
            return File.Exists(path);
        }

        private void InitiateConfig(Config cfg)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;

            string jsonText = JsonConvert.SerializeObject(cfg, settings);

            File.WriteAllText(path, jsonText);
        }

        public async Task SaveConfig()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;

            string jsonText = JsonConvert.SerializeObject(cfg, settings);

            File.WriteAllText(path, jsonText);
        }

    }
}