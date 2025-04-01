using System.Text.Json.Serialization;

namespace DynamicUpdateComponent.Contracts.ViewModelUpdate;

public class ParameterViewModelValue
{
    [JsonInclude]
    public decimal? DecimalValue { private get; init; }

    [JsonInclude]
    public int? IntegerValue { private get; init; }

    [JsonInclude]
    public EnumValue? EnumValue { private get; init; }

    [JsonIgnore]
    public ParameterType Type { private get; init; }

    [JsonConstructor]
    [Obsolete(message: "This constructor is only here to be used for json deserialization", error: true)]
    public ParameterViewModelValue(
        ParameterType type,
        decimal? decimalValue,
        int? integerValue,
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

    private ParameterViewModelValue(decimal value)
    {
        Type = ParameterType.Decimal;
        DecimalValue = value;
    }

    private ParameterViewModelValue(int value)
    {
        Type = ParameterType.Integer;
        IntegerValue = value;
    }

    private ParameterViewModelValue(EnumValue value)
    {
        Type = ParameterType.Enum;
        EnumValue = value;
    }

    public static implicit operator ParameterViewModelValue(EnumValue value) => new(value);
    public static implicit operator ParameterViewModelValue(int value) => new(value);
    public static implicit operator ParameterViewModelValue(decimal value) => new(value);

    public enum ParameterType
    {
        Decimal = 1,
        Integer = 2,
        Enum = 3,
    }
}