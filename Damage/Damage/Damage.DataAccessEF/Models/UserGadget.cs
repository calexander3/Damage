using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Damage.DataAccessEF.Models
{

    public class UserGadget
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserGadgetId { get; set; }
        public UserProfile User { get; set; }
        public Gadget Gadget { get; set; }
        [Required]
        public string GadgetSettings { get; set; }
        [Required]
        public int DisplayColumn { get; set; }
        [Required]
        public int DisplayOrdinal { get; set; }
    }
}
