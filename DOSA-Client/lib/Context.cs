using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DOSA_Client.lib
{
    public static class Context
    {
        private static readonly Dictionary<string, object> _store = new();
        public static void Add(string key, object value) => _store[key] = value;

        // A bit kak because it relies on caller discipline
        public static T Get<T>(string key) => (T)_store[key]; 
        public static bool Contains(String key) => _store.ContainsKey(key);
    }
}

