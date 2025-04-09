using System.Collections.Immutable;
using DynamicUpdateComponent.Contracts;
using DynamicUpdateComponent.Contracts.ModelUpdate;
using DynamicUpdateComponent.Contracts.ViewModelUpdate;

namespace DynamicUpdateComponent.Backend;

internal class Component1 : IComponent
{

    public event EventHandler<IImmutableList<ParameterModel>>? ModelUpdated;

    private const string decimalValueFieldId = "decimalValue";
    private const string delayBeforeUpdateId = "DelayBeforUpdate";

    private decimal _decimalValue;
    private int _delayBeforeUpdateMilliseconds;

    public string Id => nameof(Component1);

    public Component1()
    {
        _decimalValue = 41841.123m;
        _delayBeforeUpdateMilliseconds = 1000;
    }

    public void UpdateModel(ParameterViewModel model)
    {
        if (!model.Id.ComponentId.Equals(Id, StringComparison.Ordinal))
        {
            return;
        }

        switch (model.Id.ParameterId)
        {
            case decimalValueFieldId:
                _decimalValue = model.Value.GetDecimalValueOrThrow();
                Task.Delay(_delayBeforeUpdateMilliseconds).Wait();
                PublishModel();
                break;
            case delayBeforeUpdateId:
                _delayBeforeUpdateMilliseconds = model.Value.GetIntegerValueOrThrow();
                Task.Delay(_delayBeforeUpdateMilliseconds).Wait();
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
                    Id: new ParameterIdentity(Id, decimalValueFieldId),
                    LabelText: "Decimal Value",
                    ModelValue: new DecimalValue(_decimalValue, false)),
                new ParameterModel(
                    Id: new ParameterIdentity(Id, delayBeforeUpdateId),
                    LabelText: "Delay before update [ms]",
                    ModelValue: new IntegerValue(_delayBeforeUpdateMilliseconds, false)),
            ]);
    }
}