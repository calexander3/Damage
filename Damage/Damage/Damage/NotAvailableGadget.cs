using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Damage
{
    public class NotAvailableGadget : IGadget
    {
        public string RenderHTML()
        {
            return @"<div>Gadget Is Unavaliable</div>";
        }

        public DataAccess.Models.UserGadget UserGadget { get; set; }


        public string Title
        {
            get
            { 
                return UserGadget.Gadget.GadgetName; 
            }
        }


        public string DefaultSettings
        {
            get { return ""; }
        }
    }
}