using System.Collections.Immutable;
using DynamicUpdateComponent.Contracts.ModelUpdate;
using DynamicUpdateComponent.Contracts.ViewModelUpdate;

namespace DynamicUpdateComponent.Backend;

internal interface IComponent
{
    event EventHandler<IImmutableList<ParameterModel>> ModelUpdated;
    string Id { get; }
    void UpdateModel(ParameterViewModel model);
    void Activate();
}