using Scaffold.Launcher.Editor;
using UnityEditor;
using Scaffold.Core.Editor;
using Scaffold.Core.Editor.Modules;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using Scaffold.Launcher.Workers;
using Scaffold.Core.Editor.Manifest;
using Scaffold.Launcher.Library;

namespace Scaffold.Launcher
{
    public class ScaffoldLauncher
    {
        [MenuItem("Scaffold/Launcher/Open Launcher %#L")]
        public static void Launch() 
        {
            ScaffoldManager scaffold = BuildScaffold();
            ScaffoldWindow.OpenLauncher(scaffold);
        }

        public static ScaffoldManager BuildScaffold()
        {
            FileService files = new FileService();
            //Files
            Manifest manifest = files.Read<Manifest>("./Packages/manifest.json");
            ScaffoldLibrary library = ScaffoldLibrary.Load();

            //Operations
            DependencyHandler dependencyValidator = new DependencyHandler(library, manifest);
            IModuleInstaller installer = new ModuleWriterInstaller(manifest, dependencyValidator, files); //install, remove
            IModuleUpdater updater = new ModuleLockUpdater(library, files); //updates

            return new ScaffoldManager(manifest, library, installer, updater, dependencyValidator);
        }
    }
}
