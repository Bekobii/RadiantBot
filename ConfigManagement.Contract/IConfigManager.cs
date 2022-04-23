using RadiantBot.CrossCutting.DataClasses;

namespace RadiantBot.Logik.Domain.ConfigManagement.Contract

{
    public interface IConfigManager
    {
        public Config GetConfig();
        public Task SaveConfig();
    }
}