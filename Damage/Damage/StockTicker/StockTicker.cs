using Damage.Gadget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;

namespace StockTicker
{
    public class StockTicker : IGadget
    {
        string _output = "";
        public void Initialize()
        {
            var htmlBuilder = new System.Text.StringBuilder("<div style='margin:3px;'><table style='margin:0px;'>");
            var settings = JsonConvert.DeserializeObject<StockTickerOptions>(UserGadget.GadgetSettings);
            string tickerData;
            if (HttpContext.Current.Cache[settings.StockSymbols.Trim()] != null)
            {
                tickerData = (string)HttpContext.Current.Cache[settings.StockSymbols];
            }
            else
            {
                var request = (HttpWebRequest)WebRequest.Create("http://finance.google.com/finance/info?client=ig&q=" + HttpContext.Current.Server.UrlEncode(settings.StockSymbols.Trim()));
                using (var response = request.GetResponse())
                {
                    tickerData = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd().Substring(4);
                    HttpContext.Current.Cache.Insert(settings.StockSymbols.Trim(), tickerData, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }

            var results = JsonConvert.DeserializeObject<List<StockResult>>(tickerData);

            foreach (var result in results)
            {
                var color = "red";
                if (double.Parse(result.c) >= 0)
                {
                    color = "green";
                }
                htmlBuilder.Append("<tr><td style='padding:0px;'><div style='margin-right:30px;'><a href='https://www.google.com/finance?q=" + result.e + "%3a" + result.t + "' target='_blank' >" + result.t + "</a></div></td><td style='text-align:right;padding:0px;'><div style='margin-right:10px;font-weight:bold;'>" + result.l + "</div></td><td style='color:" + color + ";padding:0px;text-align:right;font-weight:bold;'>" + result.c + "</td><td style='padding:0px;color:" + color + ";'><div style='margin-left:3px;'>(" + Math.Abs(double.Parse(result.cp)).ToString("0.00") + "&#37)</div></td></tr>");
            }

            htmlBuilder.Append("</table></div>");
            _output = htmlBuilder.ToString();
        }

        public string HTML
        {
            get { return _output; }
        }

        public string Title
        {
            get { return "Stock Ticker"; }
        }

        public string Description
        {
            get { return "Track your important stocks and keep an eye on your portfolio."; }
        }

        public string DefaultSettings
        {
            get { return JsonConvert.SerializeObject(new StockTickerOptions { StockSymbols = "GOOG,MSFT,AAPL,YHOO" }); }
        }

        public List<GadgetSettingField> SettingsSchema
        {
            get { return new List<GadgetSettingField> { new GadgetSettingField { FieldName = "StockSymbols", DisplayName = "Stock Symbols (Comma-Separated)", DataType = SettingDataTypes.Text, Validators = Validators.Required } }; }
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


        public class StockResult
        {
            public string id { get; set; }
            public string t { get; set; }
            public string e { get; set; }
            public string l { get; set; }
            public string l_fix { get; set; }
            public string l_cur { get; set; }
            public string s { get; set; }
            public string ltt { get; set; }
            public string lt { get; set; }
            public string c { get; set; }
            public string cp { get; set; }
            public string ccol { get; set; }
            public string el { get; set; }
            public string el_fix { get; set; }
            public string el_cur { get; set; }
            public string elt { get; set; }
            public string ec { get; set; }
            public string ecp { get; set; }
            public string eccol { get; set; }
            public string div { get; set; }
            public string yld { get; set; }
        }
    }
}
