using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using SQLitePCL;

namespace FastDic.Models
{
    public class WordDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, SQLiteOpenFlags.ReadOnly);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;

        public WordDatabase()
        {

        }

        public Task<List<WordString>> GetLimitedWordStrings(int limit)
        {
            return Database.QueryAsync<WordString>($"SELECT [Word] FROM WordDefinitions LIMIT {limit}");
        }

        public Task<List<WordString>> GetDistinctWordsAsync()
        {
            return Database.QueryAsync<WordString>($"SELECT [Word] FROM WordDefinitions asc");
        }

        public Task<List<WordDefinition>> GetWordAsync(string word)
        {
            return Database.QueryAsync<WordDefinition>("SELECT * FROM WordDefinitions WHERE [Word] = ?", word);
        }
    }

    public class WordString
    {
        public string Word { get; set; }
    }
}
