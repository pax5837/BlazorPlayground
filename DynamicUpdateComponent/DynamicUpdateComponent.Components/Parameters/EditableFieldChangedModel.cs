namespace DynamicUpdateComponent.Components.Parameters;

public record EditableFieldChangedModel(string FieldId, string FieldValue, ChangeType ChangeType);

public enum ChangeType
{
    Value,
    Increment,
    Decrement,
}