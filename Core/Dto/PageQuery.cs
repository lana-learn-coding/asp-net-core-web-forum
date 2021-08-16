using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Core.Dto
{
    public class PageQuery
    {
        private int _page = 1;
        private int _size = 15;
        private List<string> _sort = new() { "CreatedAt" };

        public int Page
        {
            get => _page;
            set => _page = value > 0 ? value : 1;
        }

        public int Size
        {
            get => _size;
            set => _size = value > 0 ? value : 15;
        }

        public List<string> Sort
        {
            get => _sort;
            set
            {
                if (value == null || value.Count == 0)
                {
                    _sort = new List<string> { "CreatedAt" };
                    return;
                }

                _sort = value
                    .Select(s =>
                    {
                        var direction = "asc";
                        if (s.StartsWith("-"))
                        {
                            s = s[1..];
                            direction = "desc";
                        }

                        s = char.ToUpper(s[0]) + s[1..];
                        return $"{s} {direction}";
                    })
                    .ToList();
            }
        }

        [JsonIgnore]
        public int Skip => _size * (_page - 1);

        [JsonIgnore]
        public int Take => _size;
    }
}