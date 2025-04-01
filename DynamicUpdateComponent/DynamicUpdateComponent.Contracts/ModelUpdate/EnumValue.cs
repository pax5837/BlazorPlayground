using System.Collections.Immutable;

namespace DynamicUpdateComponent.Contracts.ModelUpdate;

public record EnumValue(
    string Value,
    IImmutableList<string> EnumValues);