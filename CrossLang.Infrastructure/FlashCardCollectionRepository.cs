using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.Library;
using System.Data;
using Dapper;
using CrossLang.DBHelper;
using CrossLang.Models;

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

            var query = $"Delete fcum FROM flash_card_user_mapping fcum JOIN flash_card fc ON fcum.FlashCardID = fc.ID WHERE fc.FlashCardCollectionID = {collectionID} AND fc.ID IN ({flashCardIdsStr})";

            _dbConnection.Execute(query);
        }


        public void UpdateIndividualsProgress(long collectionID)
        {
            _dbConnection.Execute("Proc_UpdateProgessOfCollection", new { v_FlashCardCollectionID  = collectionID }, commandType: CommandType.StoredProcedure);
        }

        public (List<dynamic>, long) QueryListWithProgress(long userID, int pageNum, int pageSize, bool type)
        {

            var multi = (_dbConnection.QueryMultiple("Proc_GetProgressingCollectionByUser", new {
                v_UserID = userID,
                v_Limit = pageSize,
                v_Offset = (pageNum - 1) * pageSize,
                v_Type = type
            },
            commandType: CommandType.StoredProcedure));

            var listRes = multi.Read<dynamic>().AsList();
            var total = multi.Read<long>().AsList().FirstOrDefault();


            return (listRes, total);
        }
    }
}

