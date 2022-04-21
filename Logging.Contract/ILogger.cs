using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadiantBot.CrossCutting.Logging.Contract
{
    public interface ILogger
    {
        public Task Log(LogMessage msg);
    }
}
