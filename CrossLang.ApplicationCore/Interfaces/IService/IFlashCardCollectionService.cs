using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.Models;

namespace CrossLang.ApplicationCore.Interfaces.IService
{
	public interface IFlashCardCollectionService : IBaseService<FlashCardCollection>
	{
		public ServiceResult GetListCollection(FlashCardCollection entity, List<FilterObject> filters, string formula, int pageNum, int pageSize);

		public ServiceResult GetListCollectionWithProgress(int pageNum, int pageSize, bool type);

	}
}

