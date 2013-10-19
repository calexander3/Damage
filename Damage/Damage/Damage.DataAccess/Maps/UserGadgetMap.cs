using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Damage.DataAccess.Models;


namespace Damage.DataAccess.Maps {
    
    
    public class UserGadgetMap : ClassMapping<UserGadget> {
        
        public UserGadgetMap() {
			Table("UserGadgets");
			Id(x => x.UserGadgetId, map => map.Generator(Generators.Identity));
			Property(x => x.GadgetSettings, map => map.NotNullable(true));
			Property(x => x.Column, map => map.NotNullable(true));
			Property(x => x.Ordinal, map => map.NotNullable(true));
			ManyToOne(x => x.User, map => { map.Column("UserId"); map.Cascade(Cascade.None); });

			ManyToOne(x => x.Gadget, map => { map.Column("GadgetId"); map.Cascade(Cascade.None); });

        }
    }
}
