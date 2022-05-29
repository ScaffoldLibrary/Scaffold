using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaffold.Core.Events.Brokers
{
    public class EventBus : IEventBroker
    {
        //private EventLedger _ledger = new EventLedger();

        private Dictionary<Type, List<Action<IEvent>>> dictionary = new Dictionary<Type, List<Action<IEvent>>>();
        public void Raise<T>(T signal) where T : IEvent
        {
            Type type = typeof(T);
            if (dictionary.ContainsKey(type))
            {
                var list = dictionary[type];
                foreach (var listener in list)
                {
                    listener.Invoke(signal);
                }
            }
            else
            {
                Debug.Log("no listener for this event");
            }
        }

        public void Register<T>(Type type, Action<T> callback) where T : IEvent
        {
            if (!(type is IEvent))
            {
                return;
            }

            if (callback == null)
            {
                return;
            }

            Action<IEvent> wrappedAction = (e) => callback((T)e);
            if (dictionary.ContainsKey(type) && dictionary[type] != null)
            {
                dictionary[type].Add(wrappedAction);
            }
            else
            {
                dictionary[type] = new List<Action<IEvent>>() { wrappedAction };
            }
        }

        public void Unregister<T>(Type type, Action<T> callback) where T : IEvent
        {
            if (!(type is IEvent))
            {
                return;
            }

            if (callback == null)
            {
                return;
            }
        }

        public void UnregisterAll(Type type)
        {
            throw new NotImplementedException();
        }
    }
}