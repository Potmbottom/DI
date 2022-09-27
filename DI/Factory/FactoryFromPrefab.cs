using UnityEditor;
using UnityEngine;

public class FactoryFromPrefab<T> : IFactory<T>
{
    private readonly string _loadPath;

    public FactoryFromPrefab(string path)
    {
        _loadPath = path;
    }

    public virtual T Create()
    {
        var go = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/{_loadPath}.prefab" );
        var prefab = Object.Instantiate(go);
        return prefab.GetComponent<T>();
    }
}