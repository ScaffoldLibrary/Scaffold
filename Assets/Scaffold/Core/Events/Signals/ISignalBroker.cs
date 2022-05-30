using System.Collections;
using UnityEngine;
using System;

namespace Scaffold.Core.Events.Signals
{
    public interface ISignalBroker
    {
        public void Register<T>(Type type, Action<T> callback) where T : ISignal;
        public void Register<T>(Type type, Action callback) where T : ISignal;
        public void Unregister<T>(Type type, Action<T> callback) where T : ISignal;
        public void Unregister<T>(Type type, Action callback) where T : ISignal;
        public void UnregisterAll(Type type);
        public void Raise<T>(T signal) where T : ISignal;
    }
}