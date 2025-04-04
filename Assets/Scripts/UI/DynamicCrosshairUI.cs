using UnityEngine;
using UnityEngine.UI;

namespace WinterUniverse
{
    public class DynamicCrosshairUI : Singleton<DynamicCrosshairUI>
    {
        [SerializeField] private Image _crosshairImage;

        public void SetPosition(Vector3 worldPosition)
        {
            worldPosition = Camera.main.WorldToScreenPoint(worldPosition);
            worldPosition.z = 0f;
            _crosshairImage.rectTransform.anchoredPosition = worldPosition;
        }
    }
}