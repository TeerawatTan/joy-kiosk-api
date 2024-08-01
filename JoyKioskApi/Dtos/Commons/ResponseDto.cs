namespace JoyKioskApi.Dtos.Commons
{
    public class ResponseDto
    {
        public class ResultResponse
        {
            public bool IsSuccess { get; set; }
            public object? Data { get; set; }
        }

        public class PageginationResultResponse<TModel>
        {
            public int PageSize { get; set; }
            public int CurrentPage { get; set; }
            public int TotalItems { get; set; }
            public int TotalPages { get; set; }
            public IList<TModel> Items { get; set; } = new List<TModel>();
        }

        public class OrderByDto
        {
            public string? Column { get; set; }
            public string? OrderBy { get; set; }
        }

        public class DropdownListIntResponse
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        public class DropdownListStringResponse
        {
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
        }

        public class DropdownListCodeResponse : DropdownListIntResponse
        {
            public string Code { get; set; } = string.Empty;
        }
    }
}
