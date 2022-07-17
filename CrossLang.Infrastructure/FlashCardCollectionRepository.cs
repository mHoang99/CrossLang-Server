using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.Library;
using System.Data;
using Dapper;
using CrossLang.DBHelper;

namespace CrossLang.Infrastructure
{
    public class FlashCardCollectionRepository : BaseRepository<FlashCardCollection>, IFlashCardCollectionRepository
    {
        public FlashCardCollectionRepository(IDBContext dbContext, SessionData sessionData) : base(dbContext, sessionData)
        {
        }

        public  Dictionary<string, object> GetCollectionDetails(long id)
        {

            var dict = new Dictionary<string, object>();

            using (var multi = _dbConnection.QueryMultiple(
            "Proc_GetFlashCardCollectionDetails",
            new
            {
                @ID = id,
                @UserID = _sessionData.ID
            }, commandType: CommandType.StoredProcedure
            ))
            {
                List<dynamic> list1 = multi.Read<dynamic>().AsList();
                List<dynamic> list2 = multi.Read<dynamic>().AsList();
                List<dynamic> list3 = multi.Read<dynamic>().AsList();
                dict.Add("Infos", list1);
                dict.Add("WordList", list2);
                dict.Add("Translations", list3);

            };

            return dict;
        }

        public void RemoveUserProgressOfCollectionByFlashCardIDs(long collectionID, List<long> flashCardIDs)
        {
            var flashCardIdsStr = string.Join(',', flashCardIDs);

            var query = $"Delete fccpum FROM flash_card_collection_progress_user_mapping fccpum JOIN flash_card fc ON fccpum.FlashCardID = fc.ID WHERE fc.FlashCardCollectionID = {collectionID} AND fc.ID IN ({flashCardIdsStr})";

            _dbConnection.Execute(query);
        }
    }
}

