using Damage.DataAccess.Models;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;


namespace Damage.DataAccess.Maps {
    
    
    public class UserMap : ClassMapping<User> {
        
        public UserMap() {
			Table("Users");
			Id(x => x.UserId, map => map.Generator(Generators.Identity));
			Property(x => x.UserName, map => map.NotNullable(true));
			Bag(x => x.UserGadgets, colmap =>  { colmap.Key(x => x.Column("UserId")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }
    }
}
