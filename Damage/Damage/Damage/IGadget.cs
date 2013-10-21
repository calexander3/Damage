
namespace Damage
{
    public interface IGadget
    {
        /// <summary>
        /// Creates the html/javascript for the gadget to send to the browser.
        /// </summary>
        /// <returns></returns>
        string RenderHTML();

        /// <summary>
        /// Gets the title that appears at the top of the gadget.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string title { get; }

        /// <summary>
        /// Gets or sets the information about this instance of the gadget.
        /// </summary>
        /// <value>
        /// The user gadget.
        /// </value>
        Damage.DataAccess.Models.UserGadget UserGadget { get; set; }

    }
}