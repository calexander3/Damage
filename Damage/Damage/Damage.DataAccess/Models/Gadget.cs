using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;


namespace Damage.DataAccess.Models {
	
	public class Gadget: BaseModel
	{
		public Gadget() 
		{
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
			UserGadgets = new List<UserGadget>();
		}

		public virtual int GadgetId { get; set; }
		[Required]
		public virtual string GadgetName { get; set; }
		[Required]
		public virtual string GadgetTitle { get; set; }
		[Required]
		public virtual string GadgetDescription { get; set; }
		[Required]
		public virtual string GadgetVersion { get; set; }
		[Required]
		public virtual bool RequiresValidGoogleAccessToken { get; set; }
		[Required]
		public virtual bool InBeta { get; set; }
		[Required]
		public virtual bool AssemblyPresent { get; set; }
		[Required]
		public virtual string DefaultSettings { get; set; }
		[Required]
		public virtual string SettingsSchema { get; set; }
		public virtual IList<UserGadget> UserGadgets { get; set; }

		public override string CompositeKey
		{
			get { return (GadgetId.ToString(CultureInfo.InvariantCulture)); }
		}
	}
}
