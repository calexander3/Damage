using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Damage.DataAccess.Models {
	
	public class User: BaseModel
	{
		public User()
		{
			UserGadgets = new List<UserGadget>();
		}
		public virtual int UserId { get; set; }
		[Required()]
		public virtual string UserName { get; set; }
		public virtual string CurrentOAuthAccessToken { get; set; }
		public virtual DateTime OAuthAccessTokenExpiration { get; set; }
		public virtual string EmailAddress { get; set; }
		public virtual IList<UserGadget> UserGadgets { get; set; }

		public override string CompositeKey
		{
			get { return (UserId.ToString()); }
		}
	}
}
