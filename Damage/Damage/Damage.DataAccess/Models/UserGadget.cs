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
        public virtual int Column { get; set; }
        [Required()]
        public virtual int Ordinal { get; set; }

        public override string CompositeKey
        {
            get { return (this.UserGadgetId.ToString()); }
        }
    }
}
