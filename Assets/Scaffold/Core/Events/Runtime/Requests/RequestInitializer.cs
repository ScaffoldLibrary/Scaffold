using UnityEngine;

namespace Scaffold.Core.Events.Requests
{
    public class RequestInitializer
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            Debug.Log("Requests initialized");
            new Request(new RequestDispatcher());
        }
    }
}