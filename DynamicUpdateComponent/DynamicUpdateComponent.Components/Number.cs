namespace DynamicUpdateComponent.Components;

public class Number
{
    private readonly int? valueInt;

    private readonly decimal? valueDecimal;

    private readonly ValueType valueType;

    public Number(int val)
    {
        valueInt = val;
        this.valueType = ValueType.Integer;
    }

    public Number(decimal val)
    {
        valueDecimal = val;
        this.valueType = ValueType.Decimal;
    }

    public T Switch<T>(Func<int, T> whenInteger, Func<decimal, T> whenDecimal)
    {
        return valueType switch
        {
            ValueType.Integer => whenInteger(valueInt!.Value),
            ValueType.Decimal => whenDecimal(valueDecimal!.Value),
            _ => throw new InvalidOperationException($"Unknown kind: {valueType}")
        };
    }

    public void Match(Action<int> whenInteger, Action<decimal> whenDecimal)
    {
        switch (valueType)
        {
            case ValueType.Integer:
                whenInteger(valueInt!.Value);
                break;
            case ValueType.Decimal:
                whenDecimal(valueDecimal!.Value);
                break;
            default:
                throw new InvalidOperationException($"Unknown kind: {valueType}");
        }
    }

    public static implicit operator Number(int value) => new(value);
    public static implicit operator Number(decimal value) => new(value);

    public static Number operator +(Number a, Number b)
    {
        if (a.valueType != b.valueType)
        {
            throw new InvalidOperationException($"Can not add to number sof different types");
        }

        if (a.valueType == ValueType.Decimal)
        {
            return new Number(a.valueDecimal!.Value + b.valueDecimal!.Value);
        }

        return new Number(a.valueInt!.Value + b.valueInt!.Value);
    }

    public static Number operator -(Number a, Number b)
    {
        if (a.valueType != b.valueType)
        {
            throw new InvalidOperationException($"Can not add to number sof different types");
        }

        if (a.valueType == ValueType.Decimal)
        {
            return new Number(a.valueDecimal!.Value - b.valueDecimal!.Value);
        }

        return new Number(a.valueInt!.Value - b.valueInt!.Value);
    }

    private enum ValueType
    {
        Integer = 1,
        Decimal = 2,
    }
}