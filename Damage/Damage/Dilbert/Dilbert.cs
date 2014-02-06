using Damage.Gadget;
using System.Collections.Generic;
namespace Dilbert
{
    public class Dilbert : IGadget
    {
        private string _title = "Dilbert Comic Reader";
        public void Initialize()
        {
            _title = "<a href='http://www.dilbert.com/' target='_blank'>Dilbert Comic Reader</a>";
        }

        public string HTML
        {
            get
            {
                return @"<!--[if IE]><object width='400' height='300' type='application/x-shockwave-flash' quality='high' id='W478cf2052d7472a1'><param value='http://widget.dilbert.com/o/4782b1ae641c3eb6/478cf2052d7472a1/4782b1ae641c3eb6/74b9dd60' name='movie'/><![endif]-->
<!--[if !IE]><!-->
<object width='400' height='300' type='application/x-shockwave-flash' id='W478cf2052d7472a1' data='http://widget.dilbert.com/o/4782b1ae641c3eb6/478cf2052d7472a1/4782b1ae641c3eb6/74b9dd60'><!--<![endif]--><param name='wmode' value='transparent'><param name='allowScriptAccess' value='always'><param name='allowNetworking' value='all'></object>
<!--<![endif]-->
<param name='wmode' value='transparent'>
<param name='allowScriptAccess' value='always'>
<param name='allowNetworking' value='all'>
<object width='400' height='300' type='application/x-shockwave-flash' id='W478cf2052d7472a1' data='http://widget.dilbert.com/o/4782b1ae641c3eb6/478cf2052d7472a1/4782b1ae641c3eb6/74b9dd60'><!--<![endif]--><param name='wmode' value='transparent'><param name='allowScriptAccess' value='always'><param name='allowNetworking' value='all'></object>";
            }
        }

        public string Title
        {
            get { return _title; }
        }

        public string Description
        {
            get { return "Enjoy all your favorite Dilbert strips from 1989 to today."; }
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
            get { return false; }
        }
    }
}
