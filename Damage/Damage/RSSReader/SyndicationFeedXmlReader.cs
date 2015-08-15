using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Xml;

namespace RSSReader
{
    public class SyndicationFeedXmlReader : XmlTextReader
    {
        readonly string[] _rss20DateTimeHints = { "pubDate" };
        readonly string[] _atom10DateTimeHints = { "updated", "published", "lastBuildDate" };
        private bool _isRss2DateTime;
        private bool _isAtomDateTime;
        static readonly DateTimeFormatInfo Dtfi = CultureInfo.CurrentCulture.DateTimeFormat;

        public SyndicationFeedXmlReader(Stream stream) : base(stream) { }

        public override bool IsStartElement(string localname, string ns)
        {
            _isRss2DateTime = false;
            _isAtomDateTime = false;

            if (_rss20DateTimeHints.Contains(localname)) _isRss2DateTime = true;
            if (_atom10DateTimeHints.Contains(localname)) _isAtomDateTime = true;

            return base.IsStartElement(localname, ns);
        }

        public override string ReadString()
        {
            var dateVal = base.ReadString();

            try
            {
                if (_isRss2DateTime)
                {
                    var objMethod = typeof(Rss20FeedFormatter).GetMethod("DateFromString",
                                                                                 BindingFlags.NonPublic |
                                                                                 BindingFlags.Static);
                    Debug.Assert(objMethod != null);
                    objMethod.Invoke(null, new object[] { dateVal, this });

                }
                if (_isAtomDateTime)
                {
                    var objMethod = typeof(Atom10FeedFormatter).GetMethod("DateFromString",
                                                                                  BindingFlags.NonPublic |
                                                                                  BindingFlags.Instance);
                    Debug.Assert(objMethod != null);
                    objMethod.Invoke(new Atom10FeedFormatter(), new object[] { dateVal, this });
                }
            }
            catch (TargetInvocationException)
            {
                try
                {
                    return DateTime.Parse(dateVal).ToString(Dtfi.RFC1123Pattern);
                }
                catch (Exception)
                {
                    return DateTimeOffset.UtcNow.ToString(Dtfi.RFC1123Pattern);
                }
            }
            return dateVal;
        }

    }
}
