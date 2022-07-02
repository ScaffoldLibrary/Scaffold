using System.Collections;
using UnityEngine;
using Scaffold.Core.Editor;

namespace Scaffold.Core.Editor.Module
{
    public static class ModuleExtensions
    {
        public static bool Validate(this Module module)
        {
            //check if all things are there
            //check for version
            //check if git is real?
            //check if all lsits are true
            return true;
        }

        public static string GetPascalName(this Module module)
        {
            return NameFormatter.KeyToPascal(module.name);
        }

        public static string GetDefine(this Module module)
        {
            return NameFormatter.KeyToDefine(module.name);
        }
    }
}