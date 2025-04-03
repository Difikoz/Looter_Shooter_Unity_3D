using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Preset", menuName = "Winter Universe/Weapon/New Preset")]
    public class WeaponPresetConfig : BasicInfoConfig
    {
        [SerializeField] private List<WeaponPartConfig> _parts = new();

        public List<WeaponPartConfig> Parts => _parts;
    }
}