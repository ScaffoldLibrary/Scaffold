using UnityEngine;

namespace Scaffold.Launcher.Editor
{
    public abstract class WindowTab
    {
        public WindowTab(Vector2 windowSize, ScaffoldManager scaffold)
        {
            WindowSize = windowSize;
            Scaffold = scaffold;
        }

        protected Vector2 WindowSize;
        protected ScaffoldManager Scaffold;

        public abstract string TabName
        {
            get;
        }

        public abstract void Draw(Vector2 windowSize, ScaffoldManager scaffold);
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