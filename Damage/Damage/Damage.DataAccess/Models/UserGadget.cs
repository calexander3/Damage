using System;
using System.Text;
using System.Collections.Generic;
using NHibernate.Validator.Constraints;


namespace Damage.DataAccess.Models {
    
    public class UserGadget {
        public virtual int UserGadgetId { get; set; }
        public virtual User User { get; set; }
        public virtual Gadget Gadget { get; set; }
        [NotNullNotEmpty()]
        public virtual string GadgetSettings { get; set; }
        [NotNullNotEmpty()]
        public virtual int Column { get; set; }
        [NotNullNotEmpty()]
        public virtual int Ordinal { get; set; }
    }
}
