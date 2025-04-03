using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Color", menuName = "Winter Universe/Misc/New Color")]
    public class ColorConfig : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private Color _color = Color.white;

        public string ID => _id;
        public Color Color => _color;
    }
}