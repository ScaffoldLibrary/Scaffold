using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Scaffold.Core.Events.Signals
{
    public class SignalLedger<T>: ISignalLedger
    {
        private List<Action<T>> _ledger = new List<Action<T>>();

        private List<Action<T>> _typedListeners = new List<Action<T>>();
        private List<Action> _untypedListeners = new List<Action>();
        private List<Action<T>> _wrappedListeners = new List<Action<T>>();

        public void Register(Action<T> callback)
        {
            if (callback == null)
            {
                return;
            }

            _typedListeners.Add(callback);
            UpdateLedger();
        }

        public void Register(Action callback)
        {
            if(callback == null)
            {
                return;
            }

            _untypedListeners.Add(callback);
            Action<T> wrappedCallback = (t) => callback();
            _wrappedListeners.Add(wrappedCallback);
            UpdateLedger();
        }

        public void Unregister(Action<T> callback)
        {
            if(callback == null)
            {
                return;
            }

            if (!_typedListeners.Contains(callback))
            {
                return;
            }

            _typedListeners.Remove(callback);
            UpdateLedger();
        }

        public void Unregister(Action callback)
        {
            if(callback == null)
            {
                return;
            }

            if (!_untypedListeners.Contains(callback))
            {
                return;
            }

            int index = _untypedListeners.IndexOf(callback);
            _untypedListeners.RemoveAt(index);
            _wrappedListeners.RemoveAt(index);

            UpdateLedger();
        }

        public void ClearLedger()
        {
            _wrappedListeners.Clear();
            _untypedListeners.Clear();
            _typedListeners.Clear();
            _ledger.Clear();
        }

        public List<Action<T>> GetListeners()
        {
            return _ledger;
        }

        private void UpdateLedger()
        {
            _ledger = _typedListeners.Concat(_wrappedListeners).ToList();
            Debug.Log(_ledger.Count);
        }
    }

    public interface ISignalLedger
    {
        public void ClearLedger();
    }
}