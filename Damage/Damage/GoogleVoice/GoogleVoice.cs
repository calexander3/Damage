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

function parseVoiceMessages" + id + @"(data)
{
var container=$('#GoogleVoiceMessageContainer" + id + @"');
               var result = $.parseJSON(resultSet);
               container.append('<div>' + result.messageList.length + '</div>');
}


//var container=$('#GoogleVoiceMessageContainer" + id + @"');
//if($.support.cors)
//{
//        $.ajax({
//            url: 'https://www.google.com/voice/request/messages',
//            type: 'GET',
//            contentType: 'text/plain',
//            xhrFields: {
//                  withCredentials: true
//               },
//            success: function (resultSet) 
//            {
//                var result = $.parseJSON(resultSet);
//                container.append('<div>' + result.messageList.length + '</div>');
//            }
//        });
//}
//else
//{
//    container.append('<div>Your browser must support CORS</div>');
//}

</script>
<script type='application/javascript'
        src='https://www.google.com/voice/request/messages?callback=parseVoiceMessages" + id + @"'>
</script>
");


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
