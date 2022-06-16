using System;
using CrossLang.Models;

namespace CrossLang.API.Models.Requests
{
    public class FilterRequest<T>
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public T Entity { get; set; }
        public List<FilterObject> Filters { get; set; }

    }
}
