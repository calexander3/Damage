
namespace Damage
{
    public interface IGadget
    {
        string RenderHTML();

        string title { get; set; }

        Damage.DataAccess.Models.UserGadget UserGadget { get; }

    }
}