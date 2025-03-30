using System.Globalization;

namespace DynamicUpdateComponent.Components.Parameters;

internal static class ParameterService
{
    private static readonly IFormatProvider NumberFormatInfo = new NumberFormatInfo
    {
        NumberDecimalSeparator = ".",
    };

    public static bool TryConvertToInteger(string input, out int result)
    {
        return int.TryParse(input, NumberStyles.Integer, NumberFormatInfo, out result);
    }

    public static bool TryConvertToDecimal(string input, out decimal result)
    {
        return decimal.TryParse(input, NumberStyles.Float, NumberFormatInfo, out result);
    }

    public static string? CalculateDecimalIncrementedValue(string originalValue, string incrementValue, ChangeType changeType)
    {
        var couldParseOriginalValue = TryConvertToDecimal(originalValue, out var originalVal);
        if (!couldParseOriginalValue)
        {
            return null;
        }

        var couldParseIncrementValue = TryConvertToDecimal(incrementValue, out var incrementVal);
        if (!couldParseIncrementValue)
        {
            return null;
        }

        var incrementedValue = changeType switch
        {
            ChangeType.Increment => originalVal + incrementVal,
            ChangeType.Decrement => originalVal - incrementVal,
            _ => throw new InvalidOperationException($"can not handle {changeType}"),
        };

        return incrementedValue.ToString(NumberFormatInfo);
    }

    public static string? CalculateIntegerIncrementedValue(string originalValue, string incrementValue, ChangeType changeType)
    {
        var couldParseOriginalValue = TryConvertToInteger(originalValue, out var originalVal);
        if (!couldParseOriginalValue)
        {
            return null;
        }

        var couldParseIncrementValue = TryConvertToInteger(incrementValue, out var incrementVal);
        if (!couldParseIncrementValue)
        {
            return null;
        }

        var incrementedValue = changeType switch
        {
            ChangeType.Increment => originalVal + incrementVal,
            ChangeType.Decrement => originalVal - incrementVal,
            _ => throw new InvalidOperationException($"can not handle {changeType}"),
        };

        return incrementedValue.ToString();
    }
}