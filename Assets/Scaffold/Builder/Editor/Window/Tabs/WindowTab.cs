using Scaffold.Builder.Utilities;
using Scaffold.Core.Editor.Module;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaffold.Builder.Editor.Tabs
{
    public abstract class WindowTab
    {
        public WindowTab(BuilderConfigs config)
        {
            _configs = config;
        }

        protected BuilderConfigs _configs;

        public abstract string TabKey
        {
            get;
        }

        public abstract void Draw();

        public abstract void OnNext();

        public abstract bool ValidateNext();
    }

    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class TabOrder : System.Attribute
    {
        public TabOrder(int order)
        {
            Order = order;
        }

        public int Order;
    }
}
