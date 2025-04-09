using DynamicUpdateComponent.Contracts.ModelUpdate;

namespace DynamicUpdateComponent.Components.Parameters.EditableField;

public class NumberValue
{
    private IntegerValue? IntValue { get; set; }

    private DecimalValue? DecimalValue { get; set; }

    public NumberType Type { get; private set; }

    public NumberValue(IntegerValue value)
    {
        Type = NumberType.Integer;
        IntValue = value;
    }

    public NumberValue(DecimalValue value)
    {
        Type = NumberType.Decimal;
        DecimalValue = value;
    }

    public T Switch<T>(Func<IntegerValue, T> whenInteger, Func<DecimalValue, T> whenDecimal)
    {
        return Type switch
        {
            NumberType.Integer => whenInteger(IntValue!),
            NumberType.Decimal => whenDecimal(DecimalValue!),
            _ => throw new InvalidOperationException($"Unsupported number type {Type}"),
        };
    }

    public static implicit operator NumberValue(IntegerValue value) => new(value);
    public static implicit operator NumberValue(DecimalValue value) => new(value);

    public int GetWrappedIntValueOrThrow() => IntValue?.Value ?? throw new InvalidOperationException();
    public decimal GetWrappedDecimalValueOrThrow() => DecimalValue?.Value ?? throw new InvalidOperationException();

    public enum NumberType
    {
        Integer = 1,
        Decimal = 2,
    }
}