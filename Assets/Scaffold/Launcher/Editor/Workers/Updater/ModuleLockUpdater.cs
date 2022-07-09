using System.Collections.Generic;
using Scaffold.Core.Editor.Modules;
using Scaffold.Launcher.Objects;
using Scaffold.Core.Editor;
using Scaffold.Core.Editor.ManifestLock;
using System.Linq;

namespace Scaffold.Launcher.Workers
{
    public class ModuleLockUpdater: IModuleUpdater
    {
        public ModuleLockUpdater(ScaffoldLibrary library, FileService files)
        {
            _library = library;
            _files = files;
            _lock = files.Read<ManifestLock>("./Packages/packages-lock.json");
        }

        private ScaffoldLibrary _library;
        private FileService _files;
        private ManifestLock _lock;

        public void UpdateModule(Module module)
        {
            _lock.Remove(module.name);
            _files.Save(_lock);
        }

        public void UpdateModuleInfo(Module module)
        {
            int index = _library.Modules.FindIndex(m => m.name == module.name);
            _library.Modules[index] = module;
        }
    }
}