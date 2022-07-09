using Scaffold.Core.Editor.Modules;
using System.Collections;
using UnityEngine;

namespace Scaffold.Launcher.Workers
{
    public interface IModuleUpdater
    {
        public void UpdateModule(Module module);

        public void UpdateModuleInfo(Module module);
    }
}