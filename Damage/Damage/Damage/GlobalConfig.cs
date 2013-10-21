using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Concurrent;

namespace Damage
{
    public static class GlobalConfig
    {
        public static string ConnectionString { get; set; }
        public static ConcurrentDictionary<string,Type> GadgetTypes { get; set; }
    }
}