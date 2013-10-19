using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Damage.DataAccess.Models;


namespace Damage.DataAccess.Maps {
    
    
    public class GadgetMap : ClassMapping<Gadget> {
        
        public GadgetMap() {
			Table("Gadgets");
			Id(x => x.GadgetId, map => map.Generator(Generators.Identity));
			Property(x => x.GadgetName, map => map.NotNullable(true));
			Property(x => x.GadgetVersion, map => map.NotNullable(true));
			Bag(x => x.UserGadgets, colmap =>  { colmap.Key(x => x.Column("GadgetId")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }
    }
}
