using System;
using System.Collections.Generic;

namespace Osiris.Storage.Implementations
{
    public static class CombatStorage
    {
        public static Dictionary<int, CombatInstance> _dictionary = new Dictionary<int, CombatInstance>();

        public static void StoreInstance(int key, CombatInstance inst)
        {
            if (_dictionary.ContainsKey(key))
            {
                _dictionary[key] = inst;
                return;
            }

            _dictionary.Add(key, inst);
        }

        public static CombatInstance RestoreInstance(int key)
        {
            if(!_dictionary.ContainsKey(key))
                throw new ArgumentException($"The provided key '{key}' wasn't found.");
            return _dictionary[key];
        }

        public static int NumberOfInstances()
        {
            return _dictionary.Count;
        }

    }
}
