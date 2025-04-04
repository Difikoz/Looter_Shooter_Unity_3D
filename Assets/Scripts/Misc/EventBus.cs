using System;
using UnityEngine;

namespace WinterUniverse
{
    public static class EventBus
    {
        public static Action<int, int> OnAmmoCountChanged;

        public static void AmmoCountChanged(int current, int max)
        {
            OnAmmoCountChanged?.Invoke(current, max);
        }
    }
}