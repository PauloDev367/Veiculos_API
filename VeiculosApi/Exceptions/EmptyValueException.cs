using System;

namespace VeiculosApi.Exceptions;

public class EmptyValueException : Exception
{
    public EmptyValueException()
    {
    }

    public EmptyValueException(string? message) : base(message)
    {
    }
}
