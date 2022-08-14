using System;
namespace CrossLang.Models
{
    public class NotificationMessage
    {
        public string HtmlTemplate  { get; set; }
        public string? RelatedEndpoint { get; set; }
        public NotificationType Type { get; set; }
        public List<long>? UserIDs { get; set; }
        public long SenderUserID { get; set; }
        public string? Data { get; set; }
        public bool? SaveToDB { get; set; }
    }
}

