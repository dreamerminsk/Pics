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
    }

}
