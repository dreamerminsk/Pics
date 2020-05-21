using LinqToDB.Mapping;

namespace Rater.Models
{
    [Table(Name = "Users")]
    public class UserInfo
    {

        [PrimaryKey, Identity]
        public int ID { get; set; }

        [Column(Name = "Name"), NotNull]
        public string Name { get; set; }

    }
}
