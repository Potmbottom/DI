using UnityEngine;

public class InterfaceA
{
    public InterfaceA(InterfaceB item)
    {
        var complete = item.Get() == 12;
        if (complete)
        {
            Debug.Log("Interface injection success.");
        }
        else
        {
            Debug.LogError("Interface injection failed.");
        }
    }
}

public class InterfaceB : IInterfaceB
{
    private int _value;

    public InterfaceB(int value)
    {
        _value = value;
    }

    public int Get()
    {
        return _value;
    }
}

public interface IInterfaceB
{
    int Get();
}