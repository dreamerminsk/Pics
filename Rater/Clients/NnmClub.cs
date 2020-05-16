using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rater.Clients
{
    public class NnmClub
    {
        private static readonly string HOST = "http://nnmclub.to";

        private static readonly HtmlWeb htmlWeb = new HtmlWeb();

        public static async Task<List<TorrentInfo>> GetTorrents()
        {
            var page = await htmlWeb.LoadFromWebAsync(HOST);
            var torrents = page.DocumentNode.SelectNodes(".//table[@class='pline']");
            return torrents.Where(x => IsTorentBlock(x)).Select(x =>
            {
                var torrentInfo = new TorrentInfo();
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
                return torrentInfo;
            }).ToList();
        }

        private static bool IsTorentBlock(HtmlNode node)
        {
            return node.SelectNodes(".//a[starts-with(@href, 'magnet')]") != null;
        }

    }
}
