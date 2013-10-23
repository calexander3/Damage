
using System.Collections.Generic;

namespace Damage.Gadget
{
    public interface IGadget
    {
        /// <summary>
        /// Creates the html/javascript for the gadget to send to the browser.
        /// </summary>
        /// <returns></returns>
        void Initialize();

        /// <summary>
        /// Send the html/javascript for the gadget to send to the browser.
        /// </summary>
        /// <returns></returns>
        string HTML { get; }

        /// <summary>
        /// Gets the title that appears at the top of the gadget.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string Title { get; }

        /// <summary>
        /// Gets an empty settings object to be used during adding a gadget.
        /// </summary>
        /// <value>
        /// The default settings.
        /// </value>
        string DefaultSettings { get; }

        /// <summary>
        /// Gets the settings schema. This is used to build the settings form.
        /// </summary>
        /// <value>
        /// The settings schema.
        /// </value>
        List<GadgetSettingField> SettingsSchema { get; }


        /// <summary>
        /// Gets or sets the information about this instance of the gadget.
        /// </summary>
        /// <value>
        /// The user gadget.
        /// </value>
        Damage.DataAccess.Models.UserGadget UserGadget { get; set; }

    }
}