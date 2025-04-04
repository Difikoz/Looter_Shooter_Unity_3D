using TMPro;
using UnityEngine;

namespace WinterUniverse
{
    public class AmmoBarUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private void OnEnable()
        {
            EventBus.OnAmmoCountChanged += OnAmmoCountChanged;
        }

        private void OnDisable()
        {
            EventBus.OnAmmoCountChanged -= OnAmmoCountChanged;
        }

        private void OnAmmoCountChanged(int current, int max)
        {
            _text.text = $"{current}/{max}";
        }
    }
}