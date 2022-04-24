using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadiantBot.CrossCutting.DataClasses
{
    public  class User
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public List<Warn> warns { get; set; } = new List<Warn>();

    }
}
