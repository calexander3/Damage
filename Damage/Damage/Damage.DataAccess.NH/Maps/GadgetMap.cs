using Damage.DataAccess.Models;
using FluentNHibernate.Mapping;

namespace Damage.DataAccess.Maps {
	
	
	public class GadgetMap : ClassMap<Gadget> {
		
		public GadgetMap() {
			Table("Gadgets");
			Id(x => x.GadgetId).GeneratedBy.Identity().Column("GadgetId");
			Map(x => x.GadgetName).Column("GadgetName").Not.Nullable();
			Map(x => x.GadgetTitle).Column("GadgetTitle").Not.Nullable();
			Map(x => x.GadgetDescription).Column("GadgetDescription").Not.Nullable();
			Map(x => x.GadgetVersion).Column("GadgetVersion").Not.Nullable();
			Map(x => x.DefaultSettings).Column("DefaultSettings").Not.Nullable();
			Map(x => x.SettingsSchema).Column("SettingsSchema").Not.Nullable();
			Map(x => x.RequiresValidGoogleAccessToken).Column("RequiresValidGoogleAccessToken").Not.Nullable();
			Map(x => x.InBeta).Column("InBeta").Not.Nullable();
			Map(x => x.AssemblyPresent).Column("AssemblyPresent").Not.Nullable();
			HasMany(x => x.UserGadgets).KeyColumn("GadgetId");
		}
	}
}
