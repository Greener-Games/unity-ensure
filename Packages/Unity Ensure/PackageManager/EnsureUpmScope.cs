#region

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

#endregion

namespace GG.UnityEnsure
{
    public static class EnsureUpmScope
    {
        const string ManifestPath = "Packages/manifest.json";

        /// <summary>
        ///     Ensure the Define Symbol Exists
        /// </summary>
        public static void EnsurePackageScope(string url, List<string> scopes, string name = "")
        {
            foreach (string scope in scopes)
            {
                CheckAndAdd(url, scope, name);
            }
        }
        
        /// <summary>
        ///     Ensure the Define Symbol Exists
        /// </summary>
        public static void EnsurePackageScope(string url, string scope, string name = "")
        {
            CheckAndAdd(url, scope, name);
        }

        /// <summary>
        /// validate and add the scope to the package manager
        /// </summary>
        /// <param name="url">Registry address</param>
        /// <param name="scope">package scope</param>
        /// <param name="name">Name of Registry</param>
        static void CheckAndAdd(string url, string scope, string name = "")
        {
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                Debug.LogWarning($"Url not valid, skipping {url}");
                return;
            }

            Manifest packageInfo = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText(Path.GetFullPath(ManifestPath)));

            //Check that the scope does not already exist somewhere else
            foreach (ScopedRegistry packageInfoScopedRegistry in packageInfo.scopedRegistries.Where(packageInfoScopedRegistry => packageInfoScopedRegistry.scopes.Any(s => s == scope)))
            {
                if (packageInfoScopedRegistry.url != url)
                {
                    Debug.LogWarning($"Scope already exists in registry {packageInfoScopedRegistry.url}");
                    return;
                }
            }
            
            ScopedRegistry registry = packageInfo.scopedRegistries.FirstOrDefault(x => x.url == url);
            bool modified = false;

            if (registry == null)
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = url;
                }

                registry = new ScopedRegistry(name, url);
                packageInfo.scopedRegistries.Add(registry);
                modified = true;
            }

            if (!registry.scopes.Contains(scope))
            {
                registry.scopes.Add(scope);
                modified = true;
            }

            if (modified)
            {
                string json = JsonConvert.SerializeObject(packageInfo, Formatting.Indented);
                File.WriteAllText(Path.GetFullPath("Packages/manifest.json"), json);
            }
        }
    }
}