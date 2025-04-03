using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Stat", menuName = "Winter Universe/Stat/New Stat")]
    public class StatConfig : BasicInfoConfig
    {
        [SerializeField, Range(-999999, 999999)] private int _minValue = -999999;
        [SerializeField, Range(-999999, 999999)] private int _maxValue = 999999;
        [SerializeField] private bool _isPercent;

        public int MinValue => _minValue;
        public int MaxValue => _maxValue;
        public bool IsPercent => _isPercent;
    }
}