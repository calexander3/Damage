using Damage.Gadget;
using System.Collections.Generic;
using Damage;


namespace GoogleVoice
{
    public class GoogleVoice : IGadget
    {
        string _output = "";
        public void Initialize()
        {
            var id = ShortGuid.NewGuid().ToString();
            var outputBuilder = new System.Text.StringBuilder("<div id='GoogleVoiceMessageContainer" + id +"' style='width:100%;max-height:500px;overflow-x:hidden;overflow-y:auto;'></div>");

            outputBuilder.Append(@"<script type='text/javascript' >
        $.ajax({
            url: 'https://www.google.com/voice/request/messages',
            type: 'GET',
            dataType: 'jsonp',
            success: function (resultSet) 
            {
                var container=$('#GoogleVoiceMessageContainer" + id + @"');
                container.append('<div>' + resultSet.messageList.length + '</div>');
            }
        });
</script>");


            _output = outputBuilder.ToString();
        }

        public string HTML
        {
            get { return _output; }
        }

        public string Title
        {
            get { return "Google Voice"; }
        }

        public string Description
        {
            get { return "View your recent Google Voice messages."; }
        }

        public string DefaultSettings
        {
            get { return ""; }
        }

        public List<GadgetSettingField> SettingsSchema
        {
            get { return new List<GadgetSettingField>(); }
        }

        public Damage.DataAccess.Models.UserGadget UserGadget { get; set; }

        public bool InBeta
        {
            get { return true; }
        }

        public bool RequiresValidGoogleAccessToken
        {
            get { return true; }
        }
    }
}
