using UnityEngine;

namespace WinterUniverse
{
    public class WeaponPartAttachPoint : MonoBehaviour
    {
        [SerializeField] private WeaponPartType _type;

        public WeaponPartType Type => _type;
    }
}