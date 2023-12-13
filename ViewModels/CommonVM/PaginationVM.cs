using System.Collections;

namespace WebApplication1.ViewModels.CommonVM
{
    public class PaginationVM<T> where T : IEnumerable
    {
        public int TotalCount { get; }
        public int LastPage { get; }
        public int CurrentPage { get; }
        public bool HasPre { get; }
        public bool HasNext { get; }
        public T Items { get; }

        public PaginationVM(int totalCount, int currentPage, int lastPage, T items)
        {
            if (currentPage <= 0)
            {
                throw new ArgumentException();
            }
            TotalCount = totalCount;
            LastPage = lastPage;
            CurrentPage = currentPage;
            Items = items;
            if (currentPage <= lastPage)
            {
                if (currentPage == 1)
                {
                    HasPre = false;
                }
                else
                {
                    HasNext = true;
                    HasPre = true;
                }
                if (currentPage == lastPage)
                {
                    HasNext = false;
                }
                else
                {
                    HasNext = true;
                }
            }
        }
    }
}
