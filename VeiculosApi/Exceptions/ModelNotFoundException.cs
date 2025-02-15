using System;

namespace VeiculosApi.Exceptions;

public class ModelNotFoundException : Exception
{
    public ModelNotFoundException()
    {
    }

    public ModelNotFoundException(string? message) : base(message)
    {
    }
}
