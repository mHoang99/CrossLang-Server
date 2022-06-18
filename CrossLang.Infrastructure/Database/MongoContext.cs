
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.Library;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace CrossLang.Infrastructure.Database
{
	public class MongoContext : IMongoDBContext
	{
		private string connectionString = "";
		private string dbName = "";

		public MongoContext(IConfiguration configuration)
		{
			connectionString = configuration.GetConnectionString("MongoDB");
			dbName = configuration.GetConnectionString("MongoDBName");
		}

		public IMongoCollection<T> GetCollection<T>()
		{
			var client = new MongoClient(connectionString);
			var db = client.GetDatabase(dbName);
			var collectionName = typeof(T).GetCollectionName();
			return db.GetCollection<T>(collectionName);
		}
	}
}

