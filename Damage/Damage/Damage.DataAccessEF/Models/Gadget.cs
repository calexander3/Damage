using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Damage.DataAccessEF.Models {

	[Table("Gadgets")]
	public class Gadget
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int GadgetId { get; set; }
		[Required]
		public string GadgetName { get; set; }
		[Required]
		public string GadgetTitle { get; set; }
		[Required]
		public string GadgetDescription { get; set; }
		[Required]
		public string GadgetVersion { get; set; }
		[Required]
		public bool RequiresValidGoogleAccessToken { get; set; }
		[Required]
		public bool InBeta { get; set; }
		[Required]
		public bool AssemblyPresent { get; set; }
		[Required]
		public string DefaultSettings { get; set; }
		[Required]
		public string SettingsSchema { get; set; }
		public virtual ICollection<UserGadget> UserGadgets { get; set; }

	}
}
