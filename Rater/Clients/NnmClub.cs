using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Rater.Clients
{
    public class NnmClub
    {
        private static readonly string HOST = "http://nnmclub.to";

        private static readonly HtmlWeb htmlWeb = new HtmlWeb();

        public static async Task<List<TorrentInfo>> GetTorrents(int pageNumber = 1)
        {
            var url = HOST;
            if (pageNumber > 1)
            {
                url = "http://nnmclub.to/forum/portal.php?start=" + ((pageNumber - 1) * 20).ToString() + "#pagestart";
            }
            var page = await htmlWeb.LoadFromWebAsync(url);
            var torrents = page.DocumentNode.SelectNodes(".//table[@class='pline']");
            return torrents.Where(x => IsTorentBlock(x)).Select(x =>
            {
                var torrentInfo = new TorrentInfo();
                var cat = x.SelectSingleNode(".//img[@class='picon']");
                if (cat != null)
                {
                    torrentInfo.Category = cat.Attributes["alt"].Value.Trim();
                }
                var title = x.SelectSingleNode(".//td[@class='pcatHead']");
                if (title != null)
                {
                    torrentInfo.Title = title.InnerText.Trim();
                }
                var user = x.SelectSingleNode(".//span[@class='genmed']/b");
                if (user != null)
                {
                    torrentInfo.User = user.InnerText.Trim();
                }
                var published = x.SelectSingleNode(".//span[@class='genmed']");
                if (published != null)
                {
                    var parts = published.InnerText.Split('|');
                    torrentInfo.Published = DateTime.Parse(
                        parts.Last(),
                        CultureInfo.CreateSpecificCulture("ru-RU"));
                }
                var likes = x.SelectSingleNode(".//img[@title='Поблагодарили']/following-sibling::span");
                if (likes != null)
                {
                    int likesCount = int.Parse(likes.InnerText.Trim());
                    torrentInfo.Likes = likesCount;
                }
                return torrentInfo;
            }).ToList();
        }

        private static bool IsTorentBlock(HtmlNode node)
        {
            return node.SelectNodes(".//a[starts-with(@href, 'magnet')]") != null;
        }

    }
}
