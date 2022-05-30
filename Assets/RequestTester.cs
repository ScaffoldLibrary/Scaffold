using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scaffold.Core.Events.Requests;
using Scaffold.Core.Events;

public class RequestTester : MonoBehaviour
{
    private void Start()
    {
        new Request(new RequestDispatcher());
        Request.Register<int, MyCustomRequest>(new MyCustomRequest(), Answer);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(1);
            var a = Request.Raise<MyCustomRequest, int>(new MyCustomRequest());
            Debug.Log(a);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log(2);
            Do();
        }
    }

    public async void Do()
    {
        var a = await Request.RaiseAsync<int, MyCustomRequest>(new MyCustomRequest());
        Debug.Log(a);
    }

    public int Answer(MyCustomRequest request)
    {
        return Random.Range(0, 10);
    }

    public void AnswerAsync()
    {

    }
}

public class MyCustomRequest: IRequest
{

}
