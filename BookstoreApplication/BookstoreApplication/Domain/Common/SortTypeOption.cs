using BookstoreApplication.Domain.Enums;

namespace BookstoreApplication.Domain.Common
{
    public class SortTypeOption
    {
        public int Key { get; set; }
        public string Name { get; set; }=String.Empty;

        public SortTypeOption(PublisherSortType sortType)
        {
            Key = (int)sortType;
            Name = sortType.ToString();
        }

        public SortTypeOption(BookSortType sortType)
        {
            Key = (int)sortType;
            Name = sortType.ToString();
        }
    }
}
