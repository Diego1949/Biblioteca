using Biblioteca.Domain.Exceptions;

namespace Biblioteca.Domain.Common;

internal static class Guard
{
    public static string AgainstNullOrWhiteSpace(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException($"'{parameterName}' is required.");
        }

        return value.Trim();
    }

    public static int AgainstNonPositive(int value, string parameterName)
    {
        if (value <= 0)
        {
            throw new DomainException($"'{parameterName}' must be greater than zero.");
        }

        return value;
    }
}
