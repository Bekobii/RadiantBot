using Discord;
using Discord.WebSocket;
using RadiantBot.CrossCutting.Logging.Contract;
using System;

namespace RadiantBot.CrossCutting.Logging
{
    public class Logger : ILogger
    {

        public Logger(DiscordSocketClient client)
        {
            client.Log += Log;
        }

        public Task Log(LogMessage msg)
        {
            Console.WriteLine($"[LOG:] {msg.ToString()}");
            return Task.CompletedTask;
        }

    }
}