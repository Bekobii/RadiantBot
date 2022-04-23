using Discord.WebSocket;

namespace RadiantBot.Logik.Domain.MessageManagement.Contract
{
    public interface IMessageHandler
    {
        Task HandleMessage(SocketMessage arg);
    }
}