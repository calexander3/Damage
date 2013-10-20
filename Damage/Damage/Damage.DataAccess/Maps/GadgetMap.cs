using Damage.DataAccess.Models;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;


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
