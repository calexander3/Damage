﻿using Damage.DataAccess.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Damage.DataAccess.Repositories
{
    public class GadgetRepository : BaseRepository
    {
        
        public DbSet<Gadget> Gadgets { get; set; }
        public GadgetRepository(Entities context): base(context)
        {
            Gadgets = Context.Gadgets;
        }

        /// <summary>
        /// Gets all gadgets registered in database that have a corresponding assembly loaded.
        /// </summary>
        /// <returns></returns>
        public IList<Gadget> GetAllAvailableGadgets()
        {
            return Gadgets
                .Where(g => g.AssemblyPresent)
                .OrderBy(g => g.InBeta)
                .ThenByDescending(g => g.RequiresValidGoogleAccessToken)
                .ThenBy(g => g.GadgetName)
                .ToList();
        }

        /// <summary>
        /// Gets the gadget by identifier.
        /// </summary>
        /// <param name="gadgetId">The gadget identifier.</param>
        /// <returns></returns>
        public Gadget GetGadgetById(int gadgetId)
        {
            return Gadgets.SingleOrDefault(g => g.AssemblyPresent && g.GadgetId == gadgetId);
        }


        /// <summary>
        /// Gets the gadget by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Gadget GetGadgetByName(string name)
        {
            return Gadgets.FirstOrDefault(g => g.GadgetName == name);
        }

        public IList<UserGadget> GetDefaultGadgets()
        {
            var gadgets = GetAllAvailableGadgets();

            var defaultGadgets = new List<UserGadget>();

            var weatherGadget = gadgets.Single(g => g.GadgetName == "Weather");
            defaultGadgets.Add(new UserGadget
            {
                DisplayColumn = 3,
                DisplayOrdinal = 0,
                Gadget = weatherGadget,
                GadgetSettings = weatherGadget.DefaultSettings
            });

            var historyGadget = gadgets.Single(g => g.GadgetName == "TodayInHistory");
            defaultGadgets.Add(new UserGadget
            {
                DisplayColumn = 3,
                DisplayOrdinal = 1,
                Gadget = historyGadget,
                GadgetSettings = historyGadget.DefaultSettings
            });

            var rssGadget = gadgets.Single(g => g.GadgetName == "RSSReader");
            defaultGadgets.Add(new UserGadget
            {
                DisplayColumn = 1,
                DisplayOrdinal = 0,
                Gadget = rssGadget,
                GadgetSettings = @"{""FeedURL"":""http://hypercritical.co/feeds/main"",""ItemsToDisplay"":""5"",""ExpandItemsByDefault"":false}"
            });

            defaultGadgets.Add(new UserGadget
            {
                DisplayColumn = 1,
                DisplayOrdinal = 1,
                Gadget = rssGadget,
                GadgetSettings = @"{""FeedURL"":""http://feeds.theonion.com/theonion/daily"",""ItemsToDisplay"":""5"",""ExpandItemsByDefault"":false}"
            });


            defaultGadgets.Add(new UserGadget
            {
                DisplayColumn = 1,
                DisplayOrdinal = 2,
                Gadget = rssGadget,
                GadgetSettings = @"{""FeedURL"":""http://feeds.feedburner.com/codinghorror?format=xml"",""ItemsToDisplay"":""5"",""ExpandItemsByDefault"":false}"
            });

            defaultGadgets.Add(new UserGadget
            {
                DisplayColumn = 2,
                DisplayOrdinal = 0,
                Gadget = rssGadget,
                GadgetSettings = @"{""FeedURL"":""https://news.google.com/news/feeds?pz=1&cf=all&ned=us&hl=en&topic=t&output=rss"",""ItemsToDisplay"":""10"",""ExpandItemsByDefault"":false}"
            });

            defaultGadgets.Add(new UserGadget
            {
                DisplayColumn = 2,
                DisplayOrdinal = 1,
                Gadget = rssGadget,
                GadgetSettings = @"{""FeedURL"":""https://news.google.com/news/feeds?pz=1&cf=all&ned=us&hl=en&topic=b&output=rss"",""ItemsToDisplay"":""10"",""ExpandItemsByDefault"":false}"
            });

            var stockGadget = gadgets.Single(g => g.GadgetName == "StockTicker");
            defaultGadgets.Add(new UserGadget
            {
                DisplayColumn = 1,
                DisplayOrdinal = 3,
                Gadget = stockGadget,
                GadgetSettings = stockGadget.DefaultSettings
            });

            return defaultGadgets;
        }
    }
}