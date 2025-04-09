using System.Collections.Immutable;
using DynamicUpdateComponent.Contracts;
using DynamicUpdateComponent.Contracts.ModelUpdate;
using DynamicUpdateComponent.Contracts.ViewModelUpdate;
using EnumValue = DynamicUpdateComponent.Contracts.ModelUpdate.EnumValue;

namespace DynamicUpdateComponent.Backend;

internal class Component2 : IComponent
{
    public event EventHandler<IImmutableList<ParameterModel>>? ModelUpdated;

    public string Id => nameof(Component2);

    private const string SpeedFieldId = "speed";

    private const string ModeFieldId = "mode";

    private decimal _speedMetersPerSecond;

    private Mode _mode;


    public Component2()
    {
        _speedMetersPerSecond = 44.7m;
        _mode = Mode.Smooth;
    }

    public void UpdateModel(ParameterViewModel model)
    {
        if (!model.Id.ComponentId.Equals(Id, StringComparison.Ordinal))
        {
            return;
        }

        switch (model.Id.ParameterId)
        {
            case SpeedFieldId:
                _speedMetersPerSecond = model.Value.GetDecimalValueOrThrow();
                break;
            case ModeFieldId:
                _mode = Enum.Parse<Mode>(model.Value.GetEnumValueOrThrow());
                break;
            default:
                break;
        }

        PublishModel();
    }

    public void Activate()
    {
        PublishModel();
    }

    private void PublishModel()
    {
        ModelUpdated?.Invoke(
            this,
            [
                new ParameterModel(
                    Id: new ParameterIdentity(Id, SpeedFieldId),
                    LabelText: "Speed [m/s]",
                    ModelValue: new DecimalValue(_speedMetersPerSecond, false)),
                new ParameterModel(
                    Id: new ParameterIdentity(Id, ModeFieldId),
                    LabelText: "Mode",
                    ModelValue: new EnumValue(_mode.ToString(), Enum.GetValues<Mode>().Select(m => m.ToString()).ToImmutableList())),
            ]);
    }

    private enum Mode
    {
        Fast,
        Smooth,
    }
}