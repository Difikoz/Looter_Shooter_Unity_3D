using UnityEngine;

namespace WinterUniverse
{
    [System.Serializable]
    public class StatModifier
    {
        [SerializeField] private StatModifierType _type;
        [SerializeField] private int _value;

        public StatModifierType Type => _type;
        public int Value => _value;

        public StatModifier(StatModifierType type, int value)
        {
            _type = type;
            _value = value;
        }
    }
}