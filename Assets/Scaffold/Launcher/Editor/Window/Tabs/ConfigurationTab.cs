using System.Collections;
using UnityEngine;

namespace Scaffold.Launcher.Editor
{
    [TabOrder(1)]
    public class ConfigurationTab : WindowTab
    {
        public ConfigurationTab(Vector2 windowSize, ScaffoldManager scaffold) : base(windowSize, scaffold) { }

        public override string TabName => "Configs";

        public override void Draw(Vector2 windowSize, ScaffoldManager scaffold)
        {

        }
    }
}