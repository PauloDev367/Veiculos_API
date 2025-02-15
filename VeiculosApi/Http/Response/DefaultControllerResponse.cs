using System;

namespace VeiculosApi.Http.Response;

public class DefaultControllerResponse<T>
{
    public int Status { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
}
