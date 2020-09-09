using SQLite;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Text;

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
