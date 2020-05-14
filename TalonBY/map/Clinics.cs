using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalonBY.map
{
    public class Clinics
    {

        private static readonly string REF = "https://talon.by/policlinics";

        private static readonly HtmlWeb web = new HtmlWeb();

        public static async Task<List<Policlinic>> GetPoliclinicsAsync()
        {
            var html = await web.LoadFromWebAsync(REF);
            HtmlNodeCollection nodes = html.DocumentNode.SelectNodes("//div[@class='row']");
            return nodes.Select(node =>
            {
                var clinic = node.SelectSingleNode(".//h5/a");
                return new Policlinic { Name = clinic.InnerText.Trim(), Ref = clinic.Attributes["href"].Value };
            }).ToList();
        }

        public static async Task<Policlinic> GetPoliclinic(Policlinic p)
        {
            var html = await web.LoadFromWebAsync("https://talon.by" + p.Ref);
            p.City = html.DocumentNode.SelectSingleNode("//span[@itemprop='addressLocality']").InnerText.Replace("&nbsp;", " ");
            p.Address = html.DocumentNode.SelectSingleNode("//span[@itemprop='streetAddress']").InnerText.Replace("&nbsp;", " ");
            return p;
        }
    }

}
