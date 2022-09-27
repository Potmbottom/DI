using UnityEngine;
public class SimpleA
{
    public SimpleA(SimpleB item)
    {
        var complete = item.Get() == 12;
        if (complete)
        {
            Debug.Log("Simple injection success.");
        }
        else
        {
            Debug.LogError("Simple injection failed.");
        }
    }
}

public class SimpleB
{
    private int _value;

    public SimpleB(int value)
    {
        _value = value;
    }

    public int Get()
    {
        return _value;
    }
}
