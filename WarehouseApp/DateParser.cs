namespace DateParser;

using System;
using System.Globalization;
public static class DateParser
{
    public static DateOnly ParseDateOrThrow(string input, string label)
    {
        if (!DateOnly.TryParseExact(input, "dd.MM.yyyy", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out DateOnly result))
        {
            throw new ArgumentException($"Invalid field format \"{label}\". Use \"dd.MM.yyyy\" instead");
        }

        return result;
    }
}