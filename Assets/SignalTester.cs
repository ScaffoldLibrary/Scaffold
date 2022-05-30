using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Scaffold.Core.Events;

public class SignalTester : MonoBehaviour
{
    public List<Action<int>> actions = new List<Action<int>>();

    private void Start()
    {
        //new Events(new EventBus());
        //Events.Register<MyCustomEvent>(Callback);
        //Events.Register<MyCustomEvent>((s) => { Debug.Log(2); });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Signal.Register<MyCustomEvent>(Callback);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Signal.Raise(new MyCustomEvent());
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Signal.Unregister<MyCustomEvent>(Callback);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Signal.Register<MyCustomEvent>(Callback2);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Signal.Unregister<MyCustomEvent>(Callback2);
        }
    }

    public void Callback(MyCustomEvent signal)
    {
        Debug.Log(1);
    }

    public void Callback2()
    {
        Debug.Log(2);
    }

    public void Raise()
    {
        Signal.Raise<MyCustomEvent>();
    }
}

public class MyCustomEvent : ISignal
{

}
