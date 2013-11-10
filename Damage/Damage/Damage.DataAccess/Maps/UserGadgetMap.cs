using Damage.DataAccess.Models;
using FluentNHibernate.Mapping;

namespace Damage.DataAccess.Maps {
	
	
	public class UserGadgetMap : ClassMap<UserGadget> {
		
		public UserGadgetMap() {
			Table("UserGadgets");
			Id(x => x.UserGadgetId).GeneratedBy.Identity().Column("UserGadgetId");
			References(x => x.User).Column("UserId");
			References(x => x.Gadget).Column("GadgetId");
			Map(x => x.GadgetSettings).Column("GadgetSettings").Not.Nullable();
			Map(x => x.DisplayColumn).Column("DisplayColumn").Not.Nullable();
			Map(x => x.DisplayOrdinal).Column("DisplayOrdinal").Not.Nullable();
		}
	}
}
