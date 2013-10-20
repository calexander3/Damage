
namespace RSSReader
{
    public class RSSReader: Damage.IGadget
    {
        public string RenderHTML()
        {
            return "RSS";
        }


        private Damage.DataAccess.Models.UserGadget _userGadget = null;
        public Damage.DataAccess.Models.UserGadget UserGadget
        {
            get
            {
                return _userGadget;
            }
            set
            {
                _userGadget = value;
            }
        }
    }
}
