using RadiantBot.CrossCutting.DataClasses.Configs;

namespace RadiantBot.CrossCutting.DataClasses
{
    public class Config
    {
        public ClientConfig ClientConfig { get; set; }

        public RoleConfig RoleConfig { get; set; }

        public List<string> Blackwords { get; set; } = new List<string>();

        public Config(ClientConfig clientConfig, RoleConfig roleConfig)
        {
            ClientConfig = clientConfig;
            RoleConfig = roleConfig;
        }

    }
}