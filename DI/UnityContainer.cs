using UnityEngine;

public class UnityContainer : DiContainer
{
    public void ResolveDependenciesForGameObject(GameObject root)
    {
        var components = root.GetComponentsInChildren<MonoBehaviour>(true);
        foreach (var item in components)
        {
            Resolve(item);
        }
    }
}