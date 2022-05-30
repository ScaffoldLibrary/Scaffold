using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaffold.Core.Events.Signals
{
    public class SignalBus : ISignalBroker
    {
        private Dictionary<Type, ISignalLedger> _ledgers = new Dictionary<Type, ISignalLedger>();

        public void Raise<T>(T signal) where T : ISignal
        {
            Type type = signal.GetType();
            SignalLedger<T> ledger = FetchLedger<T>(type);

            if(ledger == null)
            {
                Debug.Log("No listener registered to this event");
                return;
            }

            var listeners = ledger.GetListeners();
            foreach(Action<T> action in listeners)
            {
                try
                {
                    action.Invoke(signal);
                }
                catch { }
            }
        }

        public void Register<T>(Type type, Action<T> callback) where T : ISignal
        {
            if (!(typeof(ISignal).IsAssignableFrom(typeof(T))))
            {
                return;
            }

            if(type != typeof(T))
            {
                return;
            }

            if (callback == null)
            {
                return;
            }

            SignalLedger<T> ledger = FetchLedger<T>(type);
            if(ledger == null)
            {
                ledger = CreateLedger<T>(type);
            }
            ledger.Register(callback);
        }

        public void Register<T>(Type type, Action callback) where T:ISignal
        {
            if (!(typeof(ISignal).IsAssignableFrom(typeof(T))))
            {
                return;
            }

            if (type != typeof(T))
            {
                return;
            }

            if (callback == null)
            {
                return;
            }

            SignalLedger<T> ledger = FetchLedger<T>(type);
            if (ledger == null)
            {
                ledger = CreateLedger<T>(type);
            }
            ledger.Register(callback);
        }

        public void Unregister<T>(Type type, Action<T> callback) where T : ISignal
        {
            if (!(typeof(ISignal).IsAssignableFrom(typeof(T))))
            {
                return;
            }

            if (type != typeof(T))
            {
                return;
            }

            if (callback == null)
            {
                return;
            }

            SignalLedger<T> ledger = FetchLedger<T>(type);
            if (ledger == null)
            {
                ledger = CreateLedger<T>(type);
            }
            ledger.Unregister(callback);
        }

        public void Unregister<T>(Type type, Action callback) where T : ISignal
        {
            if (!(typeof(ISignal).IsAssignableFrom(typeof(T))))
            {
                return;
            }

            if (type != typeof(T))
            {
                return;
            }

            if (callback == null)
            {
                return;
            }

            SignalLedger<T> ledger = FetchLedger<T>(type);
            if (ledger == null)
            {
                ledger = CreateLedger<T>(type);
            }
            ledger.Unregister(callback);
        }

        public void UnregisterAll(Type type)
        {
            if(_ledgers.TryGetValue(type, out ISignalLedger ledger))
            {
                ledger.ClearLedger();
            }
        }

        private SignalLedger<T> FetchLedger<T>(Type type) where T:ISignal
        {
            if (_ledgers.ContainsKey(type) && _ledgers[type] != null)
            {
                return (SignalLedger<T>)_ledgers[type];
            }

            return null;
        }

        private SignalLedger<T> CreateLedger<T>(Type type)
        {
            SignalLedger<T> ledger = new SignalLedger<T>();
            _ledgers[type] = ledger;
            return ledger;
        }
    }
}