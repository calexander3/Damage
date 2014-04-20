using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Damage.DataAccessEF.Models
{

    public class UserGadget
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserGadgetId { get; set; }
        public int GadgetId { get; set; }
        public virtual UserProfile User { get; set; }
        public int UserId { get; set; }
        public virtual Gadget Gadget { get; set; }
        [Required]
        public string GadgetSettings { get; set; }
        [Required]
        public int DisplayColumn { get; set; }
        [Required]
        public int DisplayOrdinal { get; set; }
    }
}
