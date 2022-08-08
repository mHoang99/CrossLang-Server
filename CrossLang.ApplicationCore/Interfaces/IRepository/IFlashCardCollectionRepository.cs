using System;
using CrossLang.ApplicationCore.Entities;

namespace CrossLang.ApplicationCore.Interfaces.IRepository
{
    public interface IFlashCardCollectionRepository : IBaseRepository<FlashCardCollection>
    {
        public Dictionary<string, object> GetCollectionDetails(long id);

        public void RemoveUserProgressOfCollectionByFlashCardIDs(long collectionID, List<long> flashCardIDs);

        public void UpdateIndividualsProgress(long collectionID);

        public (List<dynamic>, long) QueryListWithProgress(long userID, int pageNum, int pageSize, bool type);
    }


}

