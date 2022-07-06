using Scaffold.Launcher.Editor;
using UnityEditor;
using Scaffold.Core.Editor;
using Scaffold.Core.Editor.Modules;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using Scaffold.Launcher.PackageHandler;
using Scaffold.Core.Editor.Manifest;
using Scaffold.Launcher.Objects;

namespace Scaffold.Launcher
{
    public class ScaffoldLauncher
    {
        [MenuItem("Scaffold/Open Launcher")]
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
            DependencyValidator dependencyValidator = new DependencyValidator(library, manifest);
            ModuleInstaller installer = new ModuleInstaller(manifest, dependencyValidator, files); //install, remove
            ModuleUpdater updater = new ModuleUpdater(library, files); //updates

            return new ScaffoldManager(library, installer, updater, dependencyValidator);
        }
    }
}
