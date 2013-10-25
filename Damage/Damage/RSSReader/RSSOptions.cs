using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSSReader
{
    internal class RSSOptions
    {
        public string FeedURL { get; set; }
        public int ItemsToDisplay { get; set; }
        public bool ExpandItemsByDefault { get; set; }
    }
}
