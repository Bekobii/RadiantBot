using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadiantBot.CrossCutting.DataClasses.Configs
{
    public class RoleConfig
    {

        public List<string> modRoles { get; set; } = new List<string>();
        public List<string> adminRoles { get; set; } = new List<string>();

    }
}
