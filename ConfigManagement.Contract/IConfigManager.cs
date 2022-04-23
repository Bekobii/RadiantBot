using RadiantBot.CrossCutting.DataClasses;

namespace RadiantBot.Logik.Domain.ConfigManagement.Contract

{
    public interface IConfigManager
    {
        public Task<Config> GetConfig();
        public Task SaveConfig();
    }
}