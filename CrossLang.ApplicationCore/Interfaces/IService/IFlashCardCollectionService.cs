using System;
using CrossLang.ApplicationCore.Entities;
using CrossLang.Models;

namespace CrossLang.ApplicationCore.Interfaces.IService
{
	public interface IFlashCardCollectionService : IBaseService<FlashCardCollection>
	{
		ServiceResult GetListCollection(FlashCardCollection entity, List<FilterObject> filters, int pageNum, int pageSize);

	}
}

