﻿using Damage;
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
            var sb = new StringBuilder("<div style='position: relative;' id='" + id + "' ><div class='gmailContainer'><div class='gmailHeader'></div></div><div class='gmailToolbox'><div class='gmailToolboxItem'><img src='/content/images/gmailArchive.png' alt='Archive' onclick='archiveMail" + id + @"()' /></div><div class='gmailToolboxItem'><img src='/content/images/gmailGarbage.png' alt='Delete' onclick='deleteMail" + id + @"()' /></div></div></div>");
            sb.Append("<script type='text/javascript' >");

            sb.Append(@"function openToolbox" + id + @"()
                        {
                            var container = $('#" + id + @"');
                            if(container.find('input:checked').length > 0)
                            {
                                container.children('.gmailToolbox').slideDown('fast');
                            }
                            else
                            {
                                container.children('.gmailToolbox').slideUp('fast');
                            }
                        }");

            sb.Append(@"function makeRead" + id + @"(element)
                        {
                            $(element).parent().parent().addClass('gmailread').children().addClass('gmailread');
                        }");

            sb.Append(@"function deleteMail" + id + @"()
                        {
                            var container = $('#" + id + @"').children('.gmailContainer');
                            var inputs = container.find('input:checked');
                            if(inputs.length > 0)
                            {
                                var messageIds = [];
                                $.each(inputs, function(index, input) {
                                    var internalMessageIds = $(input).attr('data-messageids').split(',');
                                    messageIds = $.merge(messageIds,internalMessageIds);
                                    $(input).parent().parent().remove();
                                    openToolbox" + id + @"();
                                });

                                $.ajax({
                                url: '/gmail/DeleteMail',
                                data: JSON.stringify( { 'MessageIds': messageIds}),
                                type: 'POST',
                                contentType: 'application/json',
                                dataType: 'json',
                                success: function (resultSet) {
                                    if(!resultSet.Result)
                                    {
                                        dialog('Error','There was a problem with deleting your mail messages. Please refresh and try again.');
                                    }
                                }
                                });
                            }
                        }");

            sb.Append(@"function archiveMail" + id + @"()
                        {
                            var container = $('#" + id + @"').children('.gmailContainer');
                            var inputs = container.find('input:checked');
                            if(inputs.length > 0)
                            {
                                var messageIds = [];
                                $.each(inputs, function(index, input) {
                                    var internalMessageIds = $(input).attr('data-messageids').split(',');
                                    messageIds = $.merge(messageIds,internalMessageIds);
                                    $(input).parent().parent().remove();
                                    openToolbox" + id + @"();
                                });

                                $.ajax({
                                url: '/gmail/ArchiveMail',
                                data: JSON.stringify( { 'MessageIds': messageIds}),
                                type: 'POST',
                                contentType: 'application/json',
                                dataType: 'json',
                                success: function (resultSet) {
                                    if(!resultSet.Result)
                                    {
                                        dialog('Error','There was a problem with archiving your mail messages. Please refresh and try again.');
                                    }
                                }
                                });
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
            var container = $('#" + id + @"').children('.gmailContainer');
            container.css('background-image','none');
            if(resultSet.Result)
            {
                var header = container.children('.gmailHeader');
                header.css('display','block');
                header.html('" + settings.FolderName + @"  (' + resultSet.UnreadCount + ')');
                $.each(resultSet.Data, function(index, message) {
                    var unreadCss = (message.Unread ? '' : 'gmailread ');
                    container.append('<div class=""' + unreadCss + 'gmailItem""><div class=""gmailCheckBox""><input type=""checkbox"" data-messageids=""' + message.ThreadMessageIds + '"" onclick=""openToolbox" + id + @"()"" /></div><div class=""gmailDate"">' + message.Date + '</div><div class=""' + unreadCss + 'gmailFrom""><a href=""https://mail.google.com/mail/u/0/#inbox/' + message.ThreadIdHex + '"" target=""_blank"" onclick=""makeRead" + id + @"(this)"" >' + message.From + '</a></div><div class=""gmailSubject"">' + (message.Important ? '<img src=""/content/images/gmailImportant.png"">' : '') + '<a href=""https://mail.google.com/mail/u/0/#inbox/' + message.ThreadIdHex + '"" target=""_blank"" onclick=""makeRead" + id + @"(this)"" >' + message.Subject + '</a></div><div class=""gmailPreview"">' + message.Preview + '</div><div class=""gmailDivider""></div></div>');
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
