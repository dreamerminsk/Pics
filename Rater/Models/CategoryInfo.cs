using LinqToDB.Mapping;

namespace Rater.Models
{
    [Table(Name = "Categories")]
    public class CategoryInfo
    {
        [PrimaryKey, Identity]
        public int ID { get; set; }

        [Column(Name = "Name"), NotNull]
        public string Name { get; set; }
    }
}
