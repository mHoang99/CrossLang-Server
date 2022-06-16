using System;
using System.ComponentModel;

namespace CrossLang.ApplicationCore.Entities
{
	[TableName("flash_card_collection_progress_user_mapping")]
	public class FlashCardCollectionProgressUserMapping : BaseEntity
	{
		[DBColumn]
		[DisplayName("User lien quan")]
		public long? UserID { get; set; }
		[DBColumn]
		[DisplayName("Flash card lien quan")]
		public long? FlashCardID { get; set; }
		[DBColumn]
		[DisplayName("Đã hoàn thành")]
		public bool? IsFinished { get; set; }
	}
}

