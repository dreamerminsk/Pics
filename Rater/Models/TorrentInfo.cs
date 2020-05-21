using System;

namespace Rater.Models
{
    public class TorrentInfo
    {

        public string Title { get; set; }

        public string Text { get; set; }

        public string Category { get; set; }

        public string Ref { get; set; }

        public string User { get; set; }

        public DateTime Published { get; set; }

        public int Likes { get; set; } = 0;

    }
}
