using SQLite;

namespace FastDic.Models
{
    [Table("WordDefinitions")]
    public class WordDefinition
    {
        [Indexed, Collation("NOCASE")]
        public string Word { get; set; }

        public string Definitions { get; set; }
    }
}
