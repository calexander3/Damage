using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Concurrent;

namespace Damage
{
    public static class GlobalConfig
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the gadget types detected by the application.
        /// </summary>
        /// <value>
        /// The gadget types.
        /// </value>
        public static ConcurrentDictionary<string,Type> GadgetTypes { get; set; }
    }
}