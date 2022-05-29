using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaffold.Core.Events
{
    public class Events
    {
        public Events(IEventBroker eventBroker)
        {
            _broker = eventBroker;
        }

        private static IEventBroker broker
        {
            get
            {
                if(_broker == null)
                {
                    Debug.Log("Events not initialized, pass a valid EventBroker on the Events constructor");
                }
                return _broker;
            }
        }

        private static IEventBroker _broker;

        public void Register<T>(T signal, Action<T> callback) where T : IEvent
        {
            broker?.Register(signal, callback);
        }

        public void Register<T>(T signal, Action callback) where T: IEvent
        {
            Action<T> wrappedCallback = (e) => callback();
            Register<T>(signal, wrappedCallback);
        }

        public void Register<T>(Action callback) where T : IEvent, new()
        {
            Register<T>(new T(), callback);
        }
        public void Register<T>(Action<T> callback) where T : IEvent, new()
        {
            Register<T>(new T(), callback);
        }

        public void Unregister<T>(T signal, Action<T> callback) where T: IEvent
        {
            broker?.Unregister<T>(signal, callback);
        }
        public void Unregister<T>(T signal, Action callback) where T : IEvent
        {
            Action<T> wrappedCallback = (e) => callback();
            Unregister(signal, wrappedCallback);
        }

        public void Unregister<T>(Action callback) where T : IEvent, new()
        {
            Unregister(new T(), callback);
        }

        public void Unregister<T>(Action<T> callback) where T : IEvent, new()
        {
            Unregister(new T(), callback);
        }

        public void Raise(IEvent signal)
        {
            broker?.Raise(signal);
        }

        public void Raise<T>() where T : IEvent, new()
        {
            Raise(new T());
        }
    }
}