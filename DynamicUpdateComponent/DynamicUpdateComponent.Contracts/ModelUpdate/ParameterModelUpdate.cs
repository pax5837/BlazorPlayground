namespace DynamicUpdateComponent.Contracts.ModelUpdate;

public record ParameterModelUpdate(
    ParameterIdentity Id,
    string LabelText,
    ParameterModelValue ModelValue);