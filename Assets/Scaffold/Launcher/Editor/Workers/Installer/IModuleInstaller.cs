using Scaffold.Core.Editor.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaffold.Launcher.Workers
{
    public interface IModuleInstaller
    {
        public void Install(Module module);

        public void Install(List<Module> module);

        public void Uninstall(Module module);

        public void Uninstall(List<Module> module);
    }
}