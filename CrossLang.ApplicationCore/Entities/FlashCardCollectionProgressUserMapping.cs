using System;
using System.ComponentModel;
using CrossLang.Library;

namespace CrossLang.ApplicationCore.Entities
{
	[TableName("flash_card_user_mapping")]
	public class FlashCardUserMapping : BaseEntity
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

