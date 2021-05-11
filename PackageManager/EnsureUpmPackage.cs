using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace GG.UnityEnsure
{
    public static class EnsureUpmPackage
    {
        const string ManifestPath = "Packages/manifest.json";

        /// <summary>
        ///     Ensure the Define Symbol Exists
        /// </summary>
        public static void EnsurePackageDependency(string package, string version)
        {
            CheckAndAdd(package, version);
        }

        /// <summary>
        /// validate and add the scope to the package manager
        /// </summary>
        /// <param name="package">Registry address</param>
        /// <param name="version">package scope</param>
        static void CheckAndAdd(string package, string version)
        {
            Manifest packageInfo = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText(Path.GetFullPath(ManifestPath)));

            //Check that the scope does not already exist somewhere else
            foreach (KeyValuePair<string, string> dependency in packageInfo.dependencies)
            {
                if (dependency.Key == package)
                {
                    Debug.LogWarning($"Scope already exists in registry {dependency.Key}");

                    if (dependency.Value != version)
                    {
                        //todo add a check here that can check a version and offer to update before just canceling
                        return;
                    }
                    else
                    {
                        return;
                    }
                }

            }
            
            packageInfo.dependencies.Add(package, version);
            string json = JsonConvert.SerializeObject(packageInfo, Formatting.Indented);
            File.WriteAllText(Path.GetFullPath("Packages/manifest.json"), json);
        }
    }
}