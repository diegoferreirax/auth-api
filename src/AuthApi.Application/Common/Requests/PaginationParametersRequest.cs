namespace AuthApi.Application.Common.Requests;

public record PaginationParametersRequest
{
    private int _pageSize = 10;
    private const int MaxPageSize = 100;

    public int PageNumber { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set
        {
            if (value < 1)
                _pageSize = 1;
            else if (value > MaxPageSize)
                _pageSize = MaxPageSize;
            else
                _pageSize = value;
        }
    }
}
