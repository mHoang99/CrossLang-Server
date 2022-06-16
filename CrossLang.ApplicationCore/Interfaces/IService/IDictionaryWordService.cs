using CrossLang.ApplicationCore.Entities;
using CrossLang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Interfaces.IService
{
    public interface IDictionaryWordService : IBaseService<DictionaryWord>
    {
        public ServiceResult GetSearchResult(string text);

        public Task<ServiceResult> PronunciationAssessmentAsync(string fileName, string word);

    }

}
