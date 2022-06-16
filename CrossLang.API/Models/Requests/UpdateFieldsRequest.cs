using System;
namespace CrossLang.API.Models.Requests
{
    public class UpdateFieldsRequest<T>
    {
        public List<string> Fields { get; set; }
        public T Entity { get; set; }
    }
}

