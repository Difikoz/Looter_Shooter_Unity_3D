using UnityEngine;

namespace WinterUniverse
{
    [System.Serializable]
    public class DamageType
    {
        [SerializeField, Range(1f, 1000f)] private float _damage = 10f;
        [SerializeField] private DamageTypeConfig _type;

        public float Damage => _damage;
        public DamageTypeConfig Type => _type;
    }
}