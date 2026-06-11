using BookstoreApplication.Models;

namespace BookstoreApplication.Utils
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
    }
}
