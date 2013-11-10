﻿using Damage.Gadget;
using System.Collections.Generic;

namespace TodayInHistory
{
    public class TodayInHistory : IGadget
    {
        public void Initialize()
        {

        }

        public string HTML
        {
            get { return "<div style='margin-top:2px;margin-bottom:2px;margin-right:5px;margin-left:5px;'><script type='text/javascript' src='https://rss.brainyhistory.com/link/historyevents.js'></script><small><i>more <a href='https://www.brainyhistory.com/' target='_blank'>History</a></i></small><br/><br/><script type='text/javascript' src='https://rss.brainyhistory.com/link/historybirthdays.js'></script><small><i>more <a href='https://www.brainyhistory.com/' target='_blank'>Birthdays</a></i></small></div>"; }
        }

        public string Title
        {
            get { return "Today In History"; }
        }

        public string Description
        {
            get { return "Expand your knowledge with fun facts from this day in History."; }
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
