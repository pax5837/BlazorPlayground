using System.Collections.Immutable;
using DynamicUpdateComponent.Contracts.ModelUpdate;
using DynamicUpdateComponent.Contracts.ViewModelUpdate;

namespace DynamicUpdateComponent.Backend;

internal class BackyBackend : IBackyBackend
{
    private readonly IEnumerable<IComponent> _components;
    private string activeComponent;

    public event EventHandler<IImmutableList<ParameterModel>>? ModelUpdated;

    public BackyBackend(IEnumerable<IComponent> components)
    {
        _components = components;
        activeComponent = components.First().Id;
        foreach (var component in components)
        {
            component.ModelUpdated += ComponentOnModelUpdated;
        }
    }

    private void ComponentOnModelUpdated(object? sender, IImmutableList<ParameterModel> e)
    {
        var validModels = e.Where(pm => pm.Id.ComponentId.Equals(activeComponent, StringComparison.Ordinal)).ToImmutableList();
        if (validModels.Any())
        {
            ModelUpdated?.Invoke(this, validModels);
        }
    }

    public void UpdateModel(ParameterViewModel model)
    {
        new TaskFactory().StartNew(() =>
        {
            var targetComponent = _components.FirstOrDefault(c => c.Id == activeComponent);
            if (targetComponent != null)
            {
                targetComponent.UpdateModel(model);
            }
        });
    }

    public IImmutableList<string> GetComponentIds()
    {
        return _components.Select(c => c.Id).ToImmutableList();
    }

    public void SetComponent(string componentId)
    {
        var targetComponent = _components.FirstOrDefault(c => c.Id == componentId);
        if (targetComponent is not null)
        {
            activeComponent = componentId;
            targetComponent.Activate();
        }
    }

}