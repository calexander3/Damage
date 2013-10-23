using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Damage.Gadget
{
    public class GadgetPosition
    {
        /// <summary>
        /// Gets or sets the user gadget unique identifier.
        /// </summary>
        /// <value>
        /// The user gadget unique identifier.
        /// </value>
        public int UserGadgetId { get; set; }

        /// <summary>
        /// Gets or sets the display column.
        /// </summary>
        /// <value>
        /// The display column.
        /// </value>
        public int DisplayColumn { get; set; }

        /// <summary>
        /// Gets or sets the display ordinal.
        /// </summary>
        /// <value>
        /// The display ordinal.
        /// </value>
        public int DisplayOrdinal { get; set; }
    }
}