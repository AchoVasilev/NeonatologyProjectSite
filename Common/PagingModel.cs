namespace Common
{
    using System;

    public class PagingModel
    {
        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < PagesCount;

        public int PreviousPageNumber => PageNumber - 1;

        public int NextPageNumber => PageNumber + 1;

        public int PageNumber { get; set; }

        public int PagesCount => (int)Math.Ceiling((double)ItemCount / ItemsPerPage);

        public int ItemCount { get; set; }

        public int ItemsPerPage { get; set; }
    }
}
