using System.Collections.Generic;

namespace WinterUniverse
{
    public class StatHolder
    {
        public Dictionary<string, Stat> Stats { get; private set; }

        public Stat GetStat(string id) => Stats[id];

        public StatHolder(List<StatConfig> configs)
        {
            Stats = new();
            foreach (StatConfig config in configs)
            {
                Stats.Add(config.ID, new(config));
            }
        }

        public void RecalculateStats()
        {
            foreach (KeyValuePair<string, Stat> kvp in Stats)
            {
                kvp.Value.CalculateCurrentValue();
            }
        }

        public void AddStatModifiers(List<StatModifierCreator> modifiers)
        {
            foreach (StatModifierCreator smc in modifiers)
            {
                AddStatModifier(smc);
            }
            RecalculateStats();
        }

        public void AddStatModifier(StatModifierCreator smc)
        {
            GetStat(smc.Config.ID).AddModifier(smc.Modifier);
        }

        public void RemoveStatModifiers(List<StatModifierCreator> modifiers)
        {
            foreach (StatModifierCreator smc in modifiers)
            {
                RemoveStatModifier(smc);
            }
            RecalculateStats();
        }

        public void RemoveStatModifier(StatModifierCreator smc)
        {
            GetStat(smc.Config.ID).RemoveModifier(smc.Modifier);
        }
    }
}