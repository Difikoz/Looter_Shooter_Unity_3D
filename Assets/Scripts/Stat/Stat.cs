using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    [System.Serializable]
    public class Stat
    {
        public StatConfig Config { get; private set; }
        public int CurrentValue { get; private set; }
        public List<int> FlatModifiers { get; private set; }
        public List<int> MultiplierModifiers { get; private set; }

        public Stat(StatConfig config)
        {
            Config = config;
            CurrentValue = 0;
            FlatModifiers = new();
            MultiplierModifiers = new();
        }

        public void AddModifier(StatModifier modifier)
        {
            if (modifier.Type == StatModifierType.Flat)
            {
                FlatModifiers.Add(modifier.Value);
            }
            else if (modifier.Type == StatModifierType.Multiplier)
            {
                MultiplierModifiers.Add(modifier.Value);
            }
            CalculateCurrentValue();
        }

        public void RemoveModifier(StatModifier modifier)
        {
            if (modifier.Type == StatModifierType.Flat && FlatModifiers.Contains(modifier.Value))
            {
                FlatModifiers.Remove(modifier.Value);
            }
            else if (modifier.Type == StatModifierType.Multiplier && MultiplierModifiers.Contains(modifier.Value))
            {
                MultiplierModifiers.Remove(modifier.Value);
            }
            CalculateCurrentValue();
        }

        public void CalculateCurrentValue()
        {
            int value = 0;
            foreach (int i in FlatModifiers)
            {
                value += i;
            }
            int multiplierValue = 0;
            foreach (int i in MultiplierModifiers)
            {
                multiplierValue += i;
            }
            if (multiplierValue != 0f)
            {
                multiplierValue *= value;
                multiplierValue /= 100;
                value += multiplierValue;
            }
            CurrentValue = Mathf.Clamp(value, Config.MinValue, Config.MaxValue);
        }
    }
}