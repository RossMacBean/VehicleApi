namespace Vehicles.Application.Queries;

internal class PageQuery(int pageNumber, int pageSize)
{
    private readonly int _initialPageNumber = pageNumber;
    private readonly int _initialPageSize = pageSize;
    
    internal int PageNumber { get; private set; } = pageNumber;
    internal int PageSize { get; private set; } = pageSize;

    public PageQuery WithPageNumber(int? pageNumber)
    {
        PageNumber = pageNumber ?? _initialPageNumber;
        return this;
    }

    public PageQuery WithPageSize(int? pageSize)
    {
        PageSize = pageSize ?? _initialPageSize;
        return this;
    }
}