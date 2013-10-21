
namespace Damage
{
    public interface IGadget
    {
        string RenderHTML();

        string title { get; }

        Damage.DataAccess.Models.UserGadget UserGadget { get; set; }

    }
}