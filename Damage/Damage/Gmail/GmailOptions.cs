using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmail
{
    internal class GmailOptions
    {
        public string FolderName { get; set; }
        public bool ShowUnreadOnly { get; set; }
        public bool? ShowPreview { get; set; }
    }
}
