using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Stat Holder", menuName = "Winter Universe/Stat/New Stat Holder")]
    public class StatHolderConfig : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private List<StatConfig> _stats = new();

        public string ID => _id;
        public List<StatConfig> Stats => _stats;
    }
}