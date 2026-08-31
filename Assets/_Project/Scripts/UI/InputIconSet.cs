using System;
using System.Collections.Generic;
using UnityEngine;
using Hordewood.Input;

namespace Hordewood.UI
{
    [CreateAssetMenu(fileName = "New Icon Set", menuName = "Hordewood/UI/Input Icon Set")]
    public class InputIconSet : ScriptableObject
    {
        [Serializable]
        public struct Entry
        {
            public InputDeviceKind device;
            public Sprite icon;
        }

        [SerializeField] private Entry[] entries;
        [SerializeField] private Sprite fallbackIcon;

        private Dictionary<InputDeviceKind, Sprite> _lookup;

        public Sprite GetIcon(InputDeviceKind device)
        {
            if (_lookup == null)
                BuildLookup();

            return _lookup.TryGetValue(device, out var sprite) ? sprite : fallbackIcon;
        }

        private void BuildLookup()
        {
            _lookup = new Dictionary<InputDeviceKind, Sprite>();
            foreach (var entry in entries)
                _lookup[entry.device] = entry.icon;
        }
    }
}
