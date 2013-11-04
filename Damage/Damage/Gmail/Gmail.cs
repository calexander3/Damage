using Damage;
using Damage.Gadget;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Gmail
{
    public class Gmail : IGadget
    {
        string _output = "";
        string _title = "Gmail";
        public void Initialize()
        {
            _title = "<a href='https://mail.google.com/mail/' target='_blank' >Gmail</a>";
            var settings = JsonConvert.DeserializeObject<GmailOptions>(UserGadget.GadgetSettings);

            var id = ShortGuid.NewGuid().ToString();
            var sb = new StringBuilder("<div id='" + id + "' class='gmailContainer'><div class='gmailToolbox'><img src='' alt='Delete' onclick='deleteMail()' /></div></div>");
            sb.Append("<script type='text/javascript' >");

            sb.Append(@"function openToolbox()
                        {
                            var container = $('#" + id + @"');
                            if(container.find('input:checked').length > 0)
                            {
                                //container.children('.gmailToolbox').css('display','block');
                                container.children('.gmailToolbox').slideDown('fast');
                            }
                            else
                            {
                                //container.children('.gmailToolbox').css('display','none');
                                container.children('.gmailToolbox').slideUp('fast');
                            }
                        }");

            sb.Append(@"function deleteMail()
                        {
                            var container = $('#" + id + @"');
                            var inputs = container.find('input:checked');
                            if(inputs.length > 0)
                            {
                                alert('delete');
                            }
                        }");

            sb.Append("$(document).ready(function () {");

            sb.Append(@" $.ajax({
        url: '/gmail/GetMail',
        data: { timezoneOffset: $('#timeZoneOffset').val(), showUnreadOnly : " + settings.ShowUnreadOnly.ToString().ToLower() + @", folderName : '" + settings.FolderName + @"' },
        type: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        success: function (resultSet) {
            var container = $('#" + id + @"');
            container.css('background-image','none');
            if(resultSet.Result)
            {
                $.each(resultSet.Data, function(index, message) {
                    var unreadCss = (message.Unread ? '' : 'gmailread ');
                    container.append('<div class=""' + unreadCss + 'gmailItem""><div class=""gmailCheckBox""><input type=""checkbox"" data-messageid=""' + message.ImapId + '"" onclick=""openToolbox()"" /></div><div class=""gmailDate"">' + message.Date + '</div><div class=""' + unreadCss + 'gmailFrom""><a href=""https://mail.google.com/mail/u/0/#inbox/' + message.MessageIdHex + '"" target=""_blank"" >' + message.From + '</a></div><div class=""gmailSubject""><a href=""https://mail.google.com/mail/u/0/#inbox/' + message.MessageIdHex + '"" target=""_blank"" >' + message.Subject + '</a></div><div class=""gmailPreview"">' + message.Preview + '</div></div><div class=""gmailDivider""></div>');
                });
            }
            else
            {
                $('#" + id + @"').html('There was a problem retrieving your mail. Please refresh the page');
            }
        }
    });
");

            sb.Append("});");
            sb.Append("</script>");
            _output = sb.ToString();
        }

        public string HTML
        {
            get { return _output; }
        }

        public string Title
        {
            get { return _title; }
        }

        public string Description
        {
            get { return "A smaller version of your Gmail inbox."; }
        }

        public string DefaultSettings
        {
            get { return JsonConvert.SerializeObject(new GmailOptions { FolderName = "Inbox", ShowUnreadOnly = false }); }
        }

        public List<GadgetSettingField> SettingsSchema
        {
            get
            {
                return new List<GadgetSettingField>
                {
                    new GadgetSettingField{FieldName="FolderName", DisplayName = "Folder Name", DataType= SettingDataTypes.Text, Validators= Validators.Required},
                    new GadgetSettingField{FieldName="ShowUnreadOnly", DisplayName = "Show Unread Items Only", DataType= SettingDataTypes.Checkbox, Validators= Validators.None}
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
            get { return true; }
        }
    }
}
