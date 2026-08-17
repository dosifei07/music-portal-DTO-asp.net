namespace MusicPortal.Helpers
{
    public static class PaginationHelper
    {
        public static List<int?> BuildPageList(int currentPage, int totalPages, int siblingCount = 1)
        {
            var pages = new List<int?>();
            if (totalPages <= 0) return pages;

            int start = Math.Max(1, currentPage - siblingCount);
            int end = Math.Min(totalPages, currentPage + siblingCount);

            pages.Add(1);
            if (start > 2) pages.Add(null);

            for (int p = Math.Max(2, start); p <= Math.Min(totalPages - 1, end); p++)
            {
                pages.Add(p);
            }

            if (end < totalPages - 1) pages.Add(null);
            if (totalPages > 1) pages.Add(totalPages);

            return pages;
        }
    }
}
