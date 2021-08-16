using System.Collections.Generic;

namespace Core.Dto
{
    public class Page<T>
    {
        public List<T> Data { get; init; }
        public PageMeta Meta { get; init; }
    }

    public class PageMeta
    {
        public int CurrentPage { get; init; }
        public int PerPage { get; init; }
        public int TotalPages { get; init; }
        public int TotalItems { get; init; }
    }
}