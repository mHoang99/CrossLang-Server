using CrossLang.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Interfaces.IRepository
{
    public interface IDictionaryWordRepository : IBaseRepository<DictionaryWord>
    {
        public List<DictionaryWord> GetSearchResult(string text);
    }
}
