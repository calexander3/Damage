
namespace RSSReader
{
    public class RSSReader: Damage.IGadget
    {
        public string RenderHTML()
        {
            return "RSS";
        }

        public Damage.DataAccess.Models.UserGadget UserGadget { get; set; }

        public string title
        {
            get
            {
                return "Feed Title";
            }
            set{}
        }
    }
}
