

using System.Collections.Generic;
using UnityEngine;

public class MetaInstaller : Installer
{
    [SerializeField] private ComponentSizeConfig _componentSize;
    [SerializeField] private PrefabPath _prefabPath;
    
    public override void Install(IBinder context)
    {
        context.BindInterfaces<ItemSizeProvider>();
        context.BindInstance(_componentSize).WithInterfaces();
        context.BindIFactory<BaseControl<IItemModel>>().
            FromPrefab(_prefabPath.RoadmapItem).
            WithPool(8);
        
        //Component factory
        var componentAggregator = new Dictionary<ComponentType, IFactory<BaseControl<IComponentModel>>>();
        
        context.BindIFactory<BaseControl<IComponentModel>>().
            FromPrefab(_prefabPath.Milestone).
            WithPool(8).
            AttachToAggregator(componentAggregator, ComponentType.Milestone);
        context.BindIFactory<BaseControl<IComponentModel>>().
            FromPrefab(_prefabPath.Progress).
            WithPool(8).
            AttachToAggregator(componentAggregator, ComponentType.Progress);
        context.BindIFactory<BaseControl<IComponentModel>>().
            FromPrefab(_prefabPath.CurrencyPlaceholder).
            WithPool(8).
            AttachToAggregator(componentAggregator, ComponentType.CurrencyPlaceholder);
        context.BindIFactory<BaseControl<IComponentModel>>().
            FromPrefab(_prefabPath.LevelPlaceholder).
            WithPool(3).
            AttachToAggregator(componentAggregator, ComponentType.LevelPlaceholder);
        
        context.BindInstance(componentAggregator).WhenInjected<ComponentFactory>();
        context.BindInterfaces<ComponentFactory>();
    }
}