using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaffold.Core.Events.Brokers
{
    public class EventLedger<T> : EventLedger
    {
        private List<T> _list = new List<T>();

        public void Register<T>(T signal)
        {
        }
    }

    public abstract class EventLedger
    {

        public abstract List
    }
}