using System;
using CrossLang.ApplicationCore.Entities;

namespace CrossLang.ApplicationCore.Interfaces.IRepository
{
    public interface IFlashCardCollectionRepository : IBaseRepository<FlashCardCollection>
    {
        public Dictionary<string, object> GetCollectionDetails(long id);
    }
}

