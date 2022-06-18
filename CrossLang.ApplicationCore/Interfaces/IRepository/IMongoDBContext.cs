using System;
using MongoDB.Driver;

namespace CrossLang.ApplicationCore.Interfaces.IRepository
{
	public interface IMongoDBContext
	{
		IMongoCollection<T> GetCollection<T>();
	}
}

