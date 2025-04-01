namespace DynamicUpdateComponent.Contracts.ModelUpdate;

public record ParameterModel(
    ParameterIdentity Id,
    string LabelText,
    ParameterModelValue ModelValue);