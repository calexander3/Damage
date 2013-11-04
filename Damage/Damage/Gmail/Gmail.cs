using Damage;
using Damage.Gadget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gmail
{
    public class Gmail : IGadget
    {
        string _output = "";
        string _title = "Gmail";
        public void Initialize()
        {
            _title = "<a href='https://mail.google.com/mail/' target='_blank' >Gmail</a>";
            var id = ShortGuid.NewGuid().ToString();
            var sb = new System.Text.StringBuilder("<div id='" + id + "' class='gmailContainer'></div>");
            sb.Append("<script type='text/javascript' >");
            sb.Append("$(document).ready(function () {");

            sb.Append(@" $.ajax({
        url: '/gmail/GetMail',
        data: { timezoneOffset: $('#timeZoneOffset').val() },
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
                    container.append('<div class=""' + unreadCss + 'gmailItem""><div class=""gmailCheckBox""><input type=""checkbox"" /></div><div class=""gmailDate"">' + message.Date + '</div><div class=""' + unreadCss + 'gmailFrom"">' + message.From + '</div><div class=""gmailSubject"">' + message.Subject + '</div><div class=""gmailPreview"">' + message.Preview + '</div></div><div class=""gmailDivider""></div>');
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
            get { return ""; }
        }

        public List<GadgetSettingField> SettingsSchema
        {
            get { return new List<GadgetSettingField>(); }
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
