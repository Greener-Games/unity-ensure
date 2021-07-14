using System.Collections.Generic;
using System.Linq;

namespace GG.UnityEnsure
{
    public class ScopedRegistry
    {
        public string name { get; set; }
        public string url { get; set; }
        public List<string> scopes { get; set; } = new List<string>();
        
        public ScopedRegistry()
        {
        }

        public ScopedRegistry(string name, string url)
        {
            this.name = name;
            this.url = url;
        }


        public override bool Equals(object obj)
        {
            return Equals(obj as ScopedRegistry);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = name != null ? name.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (url != null ? url.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (scopes != null ? scopes.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(ScopedRegistry other)
        {
            if (other == null)
            {
                return false;
            }
            return name.Equals(other.name) && url.Equals(other.url) && scopes.Count == other.scopes.Count && scopes.All(x => other.scopes.Contains(x));
        }
    }
}