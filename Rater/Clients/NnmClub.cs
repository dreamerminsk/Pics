using HtmlAgilityPack;
using Rater.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rater.Clients
{
    public static class NnmClub
    {
        private const string FORUM_LIST = "http://nnmclub.to/forum/portal.php?start=";
        private const string HOST = "http://nnmclub.to";

        private static readonly HtmlWeb htmlWeb = new HtmlWeb();

        public static async Task<List<TorrentInfo>> GetTorrents(int pageNumber = 1)
        {
            var url = HOST;
            if (pageNumber > 1)
            {
                url = FORUM_LIST + ((pageNumber - 1) * 20).ToString() + "#pagestart";
            }
            var page = await htmlWeb.LoadFromWebAsync(url).ConfigureAwait(false);
            var torrents = page.DocumentNode.SelectNodes(".//table[@class='pline']");
            return torrents.Where(x => IsTorentBlock(x)).Select(x =>
            {
                var torrentInfo = new TorrentInfo();
                var cat = x.SelectSingleNode(".//img[starts-with(@class, 'picon')]");
                if (cat != null)
                {
                    torrentInfo.Category = cat.Attributes["alt"].Value.Trim();
                }
                var title = x.SelectSingleNode(".//td[@class='pcatHead']");
                if (title != null)
                {
                    torrentInfo.Title = WebUtility.HtmlDecode(title.InnerText.Trim());
                    var rf = title.SelectSingleNode(".//a[@class='pgenmed']");
                    if (rf != null)
                    {
                        torrentInfo.Ref = rf.Attributes["href"].Value;
                    }
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

                var post = x.SelectSingleNode(".//span[@class='portbody']");
                if (post != null)
                {
                    var text = WebUtility.HtmlDecode(post.InnerHtml.Trim())
                    .Replace("<br>", "\r\n").Replace("<b>", "").Replace("</b>", "");
                    while (text.Contains("<a"))
                    {
                        var startPos = text.IndexOf("<a", StringComparison.OrdinalIgnoreCase);
                        var endPos = text.IndexOf("</a>", startPos, StringComparison.OrdinalIgnoreCase);
                        if (startPos > -1 && endPos > -1)
                        {
                            text = text.Replace(text.Substring(startPos, endPos - startPos + 4), "");
                        }
                        else
                        {
                            text = text.Replace("<a", "");
                        }
                    }
                    torrentInfo.Text = text;
                }

                var likes = x.SelectSingleNode(".//img[@title='Поблагодарили']/following-sibling::span");
                if (likes != null)
                {
                    int likesCount = int.Parse(likes.InnerText.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture);
                    torrentInfo.Likes = likesCount;
                }

                var magnet = x.SelectSingleNode(".//a[starts-with(@href, 'magnet')]");
                if (magnet != null)
                {
                    torrentInfo.Magnet = magnet.Attributes["href"].Value;
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
