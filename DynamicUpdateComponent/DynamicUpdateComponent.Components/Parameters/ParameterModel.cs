using System.Collections.Immutable;

namespace DynamicUpdateComponent.Components.Parameters;

public class ParameterModel
{
    public ParameterModel(
        string ParameterId,
        string DisplayText,
        string Value,
        ParameterType Type,
        Action<EditableFieldChangedModel> OnParameterChange,
        bool ShowIncrementButtons = false,
        IImmutableList<string>? EnumValues = null,
        Func<string, string?>? Validator = null)
    {
        this.ParameterId = ParameterId;
        this.DisplayText = DisplayText;
        this.Value = Value;
        this.Type = Type;
        this.OnParameterChange = OnParameterChange;
        this.ShowIncrementButtons = ShowIncrementButtons;
        this.EnumValues = EnumValues;
        this.Validator = Validator;
    }

    public string ParameterId { get; init; }
    public string DisplayText { get; init; }
    public string Value { get; set; }
    public ParameterType Type { get; init; }
    public Action<EditableFieldChangedModel> OnParameterChange { get; init; }
    public bool ShowIncrementButtons { get; init; }
    public IImmutableList<string>? EnumValues { get; set; }
    public Func<string, string?>? Validator { get; init; }
}