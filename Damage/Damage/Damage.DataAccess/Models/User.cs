using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Damage.DataAccess.Models {
    
    public class User {
        public User() {
			UserGadgets = new List<UserGadget>();
        }
        public virtual int UserId { get; set; }
        [Required()]
        public virtual string UserName { get; set; }
        public virtual IList<UserGadget> UserGadgets { get; set; }
    }
}
