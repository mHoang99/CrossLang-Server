using System;
using System.Data;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Enums;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.DBHelper;
using CrossLang.Library;
using CrossLang.Models;
using Dapper;

namespace CrossLang.Infrastructure
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDBContext dbContext, SessionData sessionData) : base(dbContext, sessionData)
        {
        }

        public override IEnumerable<IDictionary<string, object>> GetDetailsById(long id)
        {
            var query = "Select u.*, r.RoleName from user u LEFT JOIN role r ON u.RoleID = r.ID WHERE u.ID = @ID;";
            var dicts = (_dbConnection.Query(query,
            new
            {
                @ID = id
            }
            ).Cast<IDictionary<string, object>>()).ToList();

            if (dicts.Count() > 0 && dicts[0].TryGetValue("Package", out var package))
            {
                dicts[0].AddOrUpdate("PackageName", Enum.GetName(typeof(PackageEnum), int.Parse(package.ToString())));
            }

            return dicts;
        }

        public override (List<User>, long) QueryList(User entity, List<FilterObject> filters, string formula, string sortBy, string sortDirection, int pageNum, int pageSize)
        {
            var parameters = new DynamicParameters();

            var filterStr = BuildFilterString(entity, filters, formula);
            
            filters.ForEach(x =>
            {
                MappingDbTypeByField(entity, x.FieldName, ref parameters);
            });

            var res = new List<User>();


            var query = $"SELECT * FROM view_user T WHERE {filterStr} ORDER BY {sortBy} {sortDirection} LIMIT {pageSize} OFFSET {pageSize * (pageNum - 1)};";

            var resp = (_dbConnection.Query<User>(query, parameters, commandType: CommandType.Text))?.ToList() ?? new List<User>();

            var queryCount = $"SELECT COUNT(*) FROM view_user WHERE {filterStr};";

            var total = (_dbConnection.ExecuteScalar<long>(queryCount, parameters, commandType: CommandType.Text));

            return (resp, total);
        }
    }
}

