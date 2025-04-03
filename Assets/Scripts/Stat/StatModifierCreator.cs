using UnityEngine;

namespace WinterUniverse
{
    [System.Serializable]
    public class StatModifierCreator
    {
        [SerializeField] private StatConfig _config;
        [SerializeField] private StatModifier _modifier;
        [SerializeField] private ColorConfig _color;

        public StatConfig Config => _config;
        public StatModifier Modifier => _modifier;
        public ColorConfig Color => _color;

        public StatModifierCreator(StatConfig config, StatModifier modifier, ColorConfig color)
        {
            _config = config;
            _modifier = modifier;
            _color = color;
        }
    }
}