using Damage.DataAccess.Models;
using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace Damage.DataAccess.Repositories
{
    public class GadgetRepository : BaseRepository<Gadget>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GadgetRepository"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public GadgetRepository(ISession session)
            : base(session)
        {
        }

        /// <summary>
        /// Gets all gadgets registered in database.
        /// </summary>
        /// <returns></returns>
        public IList<Gadget> GetAllGadgets()
        {
            return m_Session.QueryOver<Gadget>().List<Gadget>();
        }

        /// <summary>
        /// Gets all gadgets registered in database that have a corresponding assembly loaded.
        /// </summary>
        /// <returns></returns>
        public IList<Gadget> GetAllAvailableGadgets()
        {
            return m_Session.QueryOver<Gadget>()
                .Where(g => g.AssemblyPresent)
                .OrderBy(g => g.InBeta).Asc
                .OrderBy(g => g.GadgetName).Asc
                .List<Gadget>();
        }

        /// <summary>
        /// Gets the gadget by identifier.
        /// </summary>
        /// <param name="gadgetId">The gadget identifier.</param>
        /// <returns></returns>
        public Gadget GetGadgetById(int gadgetId)
        {
            return m_Session.QueryOver<Gadget>()
                .Where(g => g.AssemblyPresent)
                .And(g => g.GadgetId == gadgetId)
                .SingleOrDefault();
        }


        public List<UserGadget> GetDefaultGadgets()
        {
            var gadgets = GetAllAvailableGadgets();
            var defaultGadgets = new List<UserGadget>();

            var weatherGadget = gadgets.Single(g => g.GadgetName == "Weather");
            defaultGadgets.Add(new UserGadget()
                {
                    DisplayColumn = 3,
                    DisplayOrdinal = 0,
                    Gadget = weatherGadget,
                    GadgetSettings = weatherGadget.DefaultSettings
                });

            var historyGadget = gadgets.Single(g => g.GadgetName == "TodayInHistory");
            defaultGadgets.Add(new UserGadget()
            {
                DisplayColumn = 3,
                DisplayOrdinal = 1,
                Gadget = historyGadget,
                GadgetSettings = historyGadget.DefaultSettings
            });

            var rssGadget = gadgets.Single(g => g.GadgetName == "RSSReader");
            defaultGadgets.Add(new UserGadget()
            {
                DisplayColumn = 1,
                DisplayOrdinal = 0,
                Gadget = rssGadget,
                GadgetSettings = @"{""FeedURL"":""http://hypercritical.co/feeds/main"",""ItemsToDisplay"":""5"",""ExpandItemsByDefault"":false}"
            });

            defaultGadgets.Add(new UserGadget()
            {
                DisplayColumn = 1,
                DisplayOrdinal = 1,
                Gadget = rssGadget,
                GadgetSettings = @"{""FeedURL"":""http://feeds.theonion.com/theonion/daily"",""ItemsToDisplay"":""5"",""ExpandItemsByDefault"":false}"
            });


            defaultGadgets.Add(new UserGadget()
            {
                DisplayColumn = 1,
                DisplayOrdinal = 2,
                Gadget = rssGadget,
                GadgetSettings = @"{""FeedURL"":""http://feeds.feedburner.com/codinghorror?format=xml"",""ItemsToDisplay"":""5"",""ExpandItemsByDefault"":false}"
            });

            defaultGadgets.Add(new UserGadget()
            {
                DisplayColumn = 2,
                DisplayOrdinal = 0,
                Gadget = rssGadget,
                GadgetSettings = @"{""FeedURL"":""https://news.google.com/news/feeds?pz=1&cf=all&ned=us&hl=en&topic=t&output=rss"",""ItemsToDisplay"":""10"",""ExpandItemsByDefault"":false}"
            });

            defaultGadgets.Add(new UserGadget()
            {
                DisplayColumn = 2,
                DisplayOrdinal = 1,
                Gadget = rssGadget,
                GadgetSettings = @"{""FeedURL"":""https://news.google.com/news/feeds?pz=1&cf=all&ned=us&hl=en&topic=b&output=rss"",""ItemsToDisplay"":""10"",""ExpandItemsByDefault"":false}"
            });

            return defaultGadgets;
        }
    }
}
