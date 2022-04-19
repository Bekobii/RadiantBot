using Discord;

namespace RadiantBot.CrossCutting.Logging
{
    public class Logger
    {

        private Task Log(LogMessage msg)
        {
            Console.WriteLine($"[LOG:] {msg.ToString()}");
            return Task.CompletedTask;
        }

    }
}