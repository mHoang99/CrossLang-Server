namespace CrossLang.QueueHelper
{
    public class RabbitMQMessage<T>
    {
        public long? UserID { get; set; }
        public T Body { get; set; }
    }
}

