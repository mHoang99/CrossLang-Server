using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.DBHelper;
using CrossLang.Library;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.Infrastructure
{
    public class DictionaryWordRepository : BaseRepository<DictionaryWord>, IDictionaryWordRepository
    {
        public DictionaryWordRepository(IDBContext dbContext, SessionData sessionData) : base(dbContext, sessionData)
        {
        }

        public override IEnumerable<IDictionary<string, object>> GetDetailsById(long id)
        {

            var dicts = _dbConnection.Query(
            "SELECT 'vi' AS language, dw.ID, dwtv.* FROM dictionary_word dw JOIN dictionary_word_translate_vi dwtv ON dw.ID = dwtv.DictionaryWordID WHERE dw.ID = @ID UNION SELECT 'en' AS language, dw.ID, dwte.* FROM dictionary_word dw JOIN dictionary_word_translate_en dwte ON dw.ID = dwte.DictionaryWordID WHERE dw.ID = @ID;",
            new
            {
                @ID = id
            }
            ).Cast<IDictionary<string, object>>();

            //.ToDictionary(pair => pair.Key, pair => pair.Value);

            return dicts;
        }

        public List<DictionaryWord> GetSearchResult(string text)
        {

            var res = _dbConnection.Query<DictionaryWord>(
            "Select * FROM (SELECT * FROM dictionary_word dw WHERE dw.Text LIKE @text UNION SELECT * FROM dictionary_word dw WHERE dw.Text LIKE @text1 UNION SELECT * FROM dictionary_word dw WHERE dw.Text LIKE @text2) T LIMIT 20",
            new
            {
                @text = $"{text}",
                @text1 = $"{text}%",
                @text2 = $"%{text}%"
            }
            )?.ToList();

            //.ToDictionary(pair => pair.Key, pair => pair.Value);

            return res ?? new List<DictionaryWord>();
        }
    }
}
