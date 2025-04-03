using UnityEngine;
using UnityEngine.Localization;

namespace WinterUniverse
{
    public abstract class BasicInfoConfig : ScriptableObject
    {
        [SerializeField] protected string _id;
        [SerializeField] protected LocalizedString _displayName;
        [SerializeField] protected LocalizedString _description;
        [SerializeField] protected ColorConfig _color;
        [SerializeField] protected Sprite _icon;

        public string ID => _id;
        public LocalizedString DisplayName => _displayName;
        public LocalizedString Description => _description;
        public ColorConfig Color => _color;
        public Sprite Icon => _icon;
    }
}