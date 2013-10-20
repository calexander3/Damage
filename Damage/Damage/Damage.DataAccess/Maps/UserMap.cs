using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Damage.DataAccess.Models;

namespace Damage.DataAccess.Maps {
	
	
	public class UserMap : ClassMap<User> {
		
		public UserMap() {
			Table("Users");
			Id(x => x.UserId).GeneratedBy.Identity().Column("UserId");
			Map(x => x.UserName).Column("UserName").Not.Nullable().Unique();
			HasMany(x => x.UserGadgets).KeyColumn("UserId");
		}
	}
}
