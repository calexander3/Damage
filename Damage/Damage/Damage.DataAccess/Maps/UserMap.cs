using Damage.DataAccess.Models;
using FluentNHibernate.Mapping;

namespace Damage.DataAccess.Maps {
	
	
	public class UserMap : ClassMap<User> {
		
		public UserMap() {
			Table("Users");
			Id(x => x.UserId).GeneratedBy.Identity().Column("UserId");
			Map(x => x.UserName).Column("UserName").Not.Nullable().Unique();
			Map(x => x.EmailAddress).Column("EmailAddress").Not.Nullable();
			Map(x => x.LastLoginTime).Column("LastLoginTime").Not.Nullable();
			Map(x => x.CurrentOAuthAccessToken).Column("CurrentOAuthAccessToken").Not.Nullable();
			Map(x => x.OAuthAccessTokenExpiration).Column("OAuthAccessTokenExpiration").Not.Nullable();
			HasMany(x => x.UserGadgets).KeyColumn("UserId");
		}
	}
}
