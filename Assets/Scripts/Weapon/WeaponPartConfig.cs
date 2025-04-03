using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Part", menuName = "Winter Universe/Weapon/New Part")]
    public class WeaponPartConfig : BasicInfoConfig
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private WeaponPartType _partType;
        [SerializeField] private RarityConfig _rarity;
        [SerializeField] private List<StatModifierCreator> _weaponModifiers = new();
        [SerializeField] private List<StatModifierCreator> _pawnModifiers = new();

        public GameObject Prefab => _prefab;
        public WeaponPartType PartType => _partType;
        public RarityConfig Rarity => _rarity;
        public List<StatModifierCreator> WeaponModifiers => _weaponModifiers;
        public List<StatModifierCreator> PawnModifiers => _pawnModifiers;
    }
}