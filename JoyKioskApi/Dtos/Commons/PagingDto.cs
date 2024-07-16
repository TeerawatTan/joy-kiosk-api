namespace JoyKioskApi.Dtos.Commons
{
    public class PagingRequestDto
    {
        public class PageRequestModel
        {
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 10;
        }

        public class PagedModel<TModel>
        {
            public int PageSize { get; set; }
            public int CurrentPage { get; set; }
            public int TotalItems { get; set; }
            public int TotalPages { get; set; }
            public IList<TModel> Items { get; set; } = new List<TModel>();
        }
    }
}
