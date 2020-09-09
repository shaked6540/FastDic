using System;
using System.IO;

namespace FastDic.Models
{
    public class Constants
    {
        public static readonly string DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "db.sqlite");
    }
}
