
namespace Damage
{
    public interface IGadget
    {
        string RenderHTML();

        Damage.DataAccess.Models.UserGadget UserGadget { get; set; }

    }
}