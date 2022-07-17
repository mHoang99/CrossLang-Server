using System;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.ApplicationCore.Interfaces.IService;
using CrossLang.Library;
using Microsoft.AspNetCore.Http;

namespace CrossLang.ApplicationCore.Entities
{
    public class RedeemHistoryService : BaseService<RedeemHistory>, IRedeemHistoryService
    {
        public RedeemHistoryService(IRedeemHistoryRepository repository, IHttpContextAccessor httpContextAccessor, SessionData sessionData) : base(repository, httpContextAccessor, sessionData)
        {
        }
    }
}

