using RadiantBot.CrossCutting.DataClasses;

namespace RadiantBot.Logik.Domain.UserFileManagement.Contract
{
    public interface IUserFileManager
    {
        UserFile Get();
        Task Save(UserFile file);
    }
}