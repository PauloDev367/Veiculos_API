using System;

namespace VeiculosApi.Http.Response;

public class PageResultResponse<T>
{
    public int TotalRecords { get; set; }
    public IEnumerable<T> Data { get; set; }
    public int PageSize { get; set; }
    public int Page { get; set; }
}
