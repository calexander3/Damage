
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
        /// Gets the description of the gadget.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        string Description { get; }

        /// <summary>
        /// Gets a value indicating whether [in beta].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [in beta]; otherwise, <c>false</c>.
        /// </value>
        bool InBeta { get; }

        /// <summary>
        /// Gets a value indicating whether [requires valid google access token]. 
        /// If this flag is set to true, the application to get a new access token if the current one is expired.
        /// </summary>
        /// <value>
        /// <c>true</c> if [requires valid google access token]; otherwise, <c>false</c>.
        /// </value>
        bool RequiresValidGoogleAccessToken { get; }

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