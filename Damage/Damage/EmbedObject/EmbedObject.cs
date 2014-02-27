using System.Linq;
using Damage.Gadget;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EmbedObject
{
    public class EmbedObject : IGadget
    {
        private string _title = "Embed Object";
        private string _html;
        public void Initialize()
        {
            var settings = JsonConvert.DeserializeObject<EmbedObjectOptions>(UserGadget.GadgetSettings);
            _title = settings.Title;
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(settings.Markup);
            var errors = htmlDoc.ParseErrors.ToList();
            if (errors.Any())
            {
                var errorHtml = new System.Text.StringBuilder("<div>Errors were present in your markup:</div>");
                foreach (var error in errors)
                {
                    errorHtml.Append("<div>" + System.Web.HttpUtility.HtmlEncode(error.Code)+ ": " + System.Web.HttpUtility.HtmlEncode(error.Reason) + "</div>");
                }
                _html = errorHtml.ToString();
            }
            else
            {
                _html = settings.Markup;
            }
        }

        public string HTML
        {
            get
            {
                return _html;
            }
        }

        public string Title
        {
            get { return _title; }
        }

        public string Description
        {
            get { return "Embed any HTML markup object into your Damage page. Warning: Insecure content might be blocked by certain web browsers."; }
        }

        public string DefaultSettings
        {
            get { return JsonConvert.SerializeObject(new EmbedObjectOptions { Title = "Embed Object", Markup = "<p>Use the settings dialog to change the content of this gadget.</p>" }); }
        }

        public List<GadgetSettingField> SettingsSchema
        {
            get
            {
                return new List<GadgetSettingField>
                {
                    new GadgetSettingField{FieldName="Title", DisplayName = "Title", DataType= SettingDataTypes.Text, Validators= Validators.Required},
                    new GadgetSettingField{FieldName="Markup", DisplayName = "Markup", DataType= SettingDataTypes.TextArea, Validators= Validators.None}
                };
            }
        }

        public Damage.DataAccess.Models.UserGadget UserGadget { get; set; }

        public bool InBeta
        {
            get { return false; }
        }

        public bool RequiresValidGoogleAccessToken
        {
            get { return false; }
        }
    }
}
