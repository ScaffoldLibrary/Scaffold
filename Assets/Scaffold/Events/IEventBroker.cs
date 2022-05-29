using System.Collections;
using UnityEngine;
using System;

namespace Scaffold.Core.Events
{
    public interface IEventBroker
    {
        public void Register<T>(Type type, Action<T> callback) where T : IEvent;
        public void Unregister<T>(Type type, Action<T> callback) where T : IEvent;
        public void UnregisterAll(Type type);
        public void Raise<T>(T signal) where T: IEvent;
    }
}