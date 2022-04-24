using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadiantBot.CrossCutting.DataClasses
{
    public class UserFile
    {
        public List<User> users { get; set; } = new List<User>();

    }
}
