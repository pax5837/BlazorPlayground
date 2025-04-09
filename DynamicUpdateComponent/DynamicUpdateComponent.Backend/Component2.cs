using System.Collections.Immutable;
using DynamicUpdateComponent.Contracts;
using DynamicUpdateComponent.Contracts.ModelUpdate;
using DynamicUpdateComponent.Contracts.ViewModelUpdate;

namespace DynamicUpdateComponent.Backend;

internal class Component2 : IComponent
{
    public event EventHandler<IImmutableList<ParameterModel>>? ModelUpdated;

    public string Id => nameof(Component2);

    private const string SpeedFieldId = "speed";

    private decimal _speedMetersPerSecond;

    public Component2()
    {
        _speedMetersPerSecond = 44.7m;
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
                PublishModel();
                break;
            default:
                break;
        }
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
            ]);
    }
}