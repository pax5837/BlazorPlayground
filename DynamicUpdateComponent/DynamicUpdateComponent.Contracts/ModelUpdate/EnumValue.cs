using System.Collections.Immutable;

namespace DynamicUpdateComponent.Contracts.ModelUpdate;

public record EnumValue(
    int Value,
    IImmutableList<string> EnumValues);