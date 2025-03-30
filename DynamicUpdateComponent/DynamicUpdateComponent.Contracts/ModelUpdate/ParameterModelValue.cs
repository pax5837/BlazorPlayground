using System.Text.Json.Serialization;

namespace DynamicUpdateComponent.Contracts.ModelUpdate;

public class ParameterModelValue
{
    [JsonInclude]
    public DecimalValue? DecimalValue { private get; init; }

    [JsonInclude]
    public IntegerValue? IntegerValue { private get; init; }

    [JsonInclude]
    public EnumValue? EnumValue { private get; init; }

    [JsonIgnore]
    public ParameterType Type { private get; init; }

    [JsonConstructor]
    [Obsolete(message: "This constructor is only here to be used for json deserialization", error: true)]
    public ParameterModelValue(
        ParameterType type,
        DecimalValue? decimalValue,
        IntegerValue? integerValue,
        EnumValue? enumValue)
    {
        switch (type)
        {
            case ParameterType.Enum:
                Type = type;
                EnumValue = enumValue ?? throw new ArgumentNullException(nameof(enumValue));
                break;
            case ParameterType.Integer:
                Type = type;
                IntegerValue = integerValue ?? throw new ArgumentNullException(nameof(integerValue));
                break;
            case ParameterType.Decimal:
                Type = type;
                DecimalValue = decimalValue ?? throw new ArgumentNullException(nameof(decimalValue));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        };
    }

    private ParameterModelValue(DecimalValue value)
    {
        if (value is null) // if it is used in the context of an project not using nullable reference types.
        {
            throw new ArgumentNullException(nameof(value));
        }

        Type = ParameterType.Decimal;
        DecimalValue = value;
    }

    private ParameterModelValue(IntegerValue value)
    {
        if (value is null) // if it is used in the context of an project not using nullable reference types.
        {
            throw new ArgumentNullException(nameof(value));
        }

        Type = ParameterType.Integer;
        IntegerValue = value;
    }

    private ParameterModelValue(EnumValue value)
    {
        if (value is null) // if it is used in the context of an project not using nullable reference types.
        {
            throw new ArgumentNullException(nameof(value));
        }

        Type = ParameterType.Enum;
        EnumValue = value;
    }

    public static implicit operator ParameterModelValue(EnumValue value) => new(value);
    public static implicit operator ParameterModelValue(IntegerValue value) => new(value);
    public static implicit operator ParameterModelValue(DecimalValue value) => new(value);

    public enum ParameterType
    {
        Decimal = 1,
        Integer = 2,
        Enum = 3,
    }
}