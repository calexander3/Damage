using System.ComponentModel.DataAnnotations;


namespace Damage.DataAccess.Models {
    
    public class UserGadget: BaseModel
    {
        public virtual int UserGadgetId { get; set; }
        public virtual User User { get; set; }
        public virtual Gadget Gadget { get; set; }
        [Required()]
        public virtual string GadgetSettings { get; set; }
        [Required()]
        public virtual int DisplayColumn { get; set; }
        [Required()]
        public virtual int DisplayOrdinal { get; set; }

        public override string CompositeKey
        {
            get { return (UserGadgetId.ToString()); }
        }
    }
}
