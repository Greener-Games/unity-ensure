using System.Collections.Generic;

namespace GG.UnityEnsure
{
    public class Manifest
    {
        public List<ScopedRegistry> scopedRegistries { get; set; } = new List<ScopedRegistry>();
        public Dictionary<string, string> dependencies { get; set; } = new Dictionary<string, string>();
    }
}