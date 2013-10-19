using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Damage.DataAccess.Models;


namespace Damage.DataAccess.Maps {
    
    
    public class UserMap : ClassMapping<User> {
        
        public UserMap() {
			Table("Users");
			Id(x => x.UserId, map => map.Generator(Generators.Identity));
			Property(x => x.Username, map => map.NotNullable(true));
			Property(x => x.Password, map => map.NotNullable(true));
			Property(x => x.Salt, map => map.NotNullable(true));
			Bag(x => x.UserGadgets, colmap =>  { colmap.Key(x => x.Column("UserId")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }
    }
}
