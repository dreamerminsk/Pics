using LinqToDB.Mapping;
using System;

namespace Rater.Models
{
    [Table(Name = "Torrents")]
    public class TorrentInfo
    {

        [PrimaryKey]
        public int ID { get; set; }

        [Column(Name = "Title"), NotNull]
        public string Title { get; set; }

        public string Text { get; set; }

        [Column(Name = "Category")]
        public string Category { get; set; }

        public string Ref { get; set; }

        [Column(Name = "User")]
        public string User { get; set; }

        [Column(Name = "Published")]
        public DateTime Published { get; set; }

        [Column(Name ="Magnet")]
        public string Magnet { get; set; }

        [Column(Name = "Likes")]
        public int Likes { get; set; } = 0;

    }
}
