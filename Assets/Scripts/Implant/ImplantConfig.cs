using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Implant", menuName = "Winter Universe/Implant/New Implant")]
    public class ImplantConfig : BasicInfoConfig
    {
        [SerializeField] private ImplantTypeConfig _implantType;
        [SerializeField] private RarityConfig _rarity;
        [SerializeField] private List<StatModifierCreator> _pawnModifiers = new();
        [SerializeField] private List<StatModifierCreator> _weaponModifiers = new();

        public ImplantTypeConfig ImplantType => _implantType;
        public RarityConfig Rarity => _rarity;
        public List<StatModifierCreator> PawnModifiers => _pawnModifiers;
        public List<StatModifierCreator> WeaponModifiers => _weaponModifiers;
    }
}