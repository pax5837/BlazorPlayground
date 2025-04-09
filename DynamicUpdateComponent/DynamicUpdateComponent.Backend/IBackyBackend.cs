using System.Collections.Immutable;
using DynamicUpdateComponent.Contracts.ModelUpdate;
using DynamicUpdateComponent.Contracts.ViewModelUpdate;

namespace DynamicUpdateComponent.Backend;

public interface IBackyBackend
{
    event EventHandler<IImmutableList<ParameterModel>> ModelUpdated;
    void UpdateModel(ParameterViewModel model);
    IImmutableList<string> GetComponentIds();
    void SetComponent(string componentId);
}