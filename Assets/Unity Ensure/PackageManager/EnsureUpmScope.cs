using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace GG.UnityEnsure
{
    public static class EnsureUpmScope
    {
        const string ManifestPath = "Packages/manifest.json";
        
        /// <summary>
        /// Ensure the Define Symbol Exists
        /// </summary>
        public static void EnsurePackageScope(string url, string scope, string name = "")
        {
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                Debug.LogError($"Url not valid, skipping {url}");
                return;
            }
            
            bool modified = false;
            Manifest packageInfo = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText(Path.GetFullPath(ManifestPath)));
            ScopedRegistry registry = packageInfo.scopedRegistries.FirstOrDefault(x => x.url == url);
            
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