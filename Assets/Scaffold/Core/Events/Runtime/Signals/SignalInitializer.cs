using UnityEngine;

namespace Scaffold.Core.Events.Signals
{
    internal class SignalInitializer
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            Debug.Log("Signals Initialized");
            new Signal(new SignalBus());
        }
    }
}