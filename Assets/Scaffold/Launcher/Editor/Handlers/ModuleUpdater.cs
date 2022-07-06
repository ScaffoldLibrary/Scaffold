using System.Collections.Generic;
using Scaffold.Core.Editor.Modules;
using Scaffold.Launcher.Objects;
using Scaffold.Core.Editor;

namespace Scaffold.Launcher.PackageHandler
{
    public class ModuleUpdater
    {
        public ModuleUpdater(ScaffoldLibrary library, FileService files)
        {
            _library = library;
        }

        private ScaffoldLibrary _library;

        public void Update(Module module)
        {
            //update package.lock / client.add
        }

        public void UpdateInfo(Module module)
        {
            //swap data on library
        }
    }
}