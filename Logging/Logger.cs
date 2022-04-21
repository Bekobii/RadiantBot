using Discord;
using RadiantBot.CrossCutting.Logging.Contract;
using System;

namespace RadiantBot.CrossCutting.Logging
{
    public class Logger : ILogger
    {

        public Task Log(LogMessage msg)
        {
            Console.WriteLine($"[LOG:] {msg.ToString()}");
            return Task.CompletedTask;
        }

    }
}