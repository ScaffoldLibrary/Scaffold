using System.Collections;
using UnityEngine;

namespace Scaffold.Builder.Editor.Tabs
{
    [TabOrder(5)]
    public class BuildTab : WindowTab
    {
        public BuildTab(BuilderConfigs config): base(config)
        {

        }

        public override string TabKey => "Building module...";

        public override void OnDraw()
        {

        }

        public override void Draw()
        {

        }


        public override void OnNext()
        {

        }

        public override bool ValidateNext()
        {
            return true;
        }
    }
}