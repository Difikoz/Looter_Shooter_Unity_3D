using UnityEngine;

namespace WinterUniverse
{
    public class ImplantSlot
    {
        private PlayerController _player;

        public ImplantConfig Config { get; private set; }
        public ImplantTypeConfig Type { get; private set; }

        public ImplantSlot(PlayerController player, ImplantConfig baseConfig)
        {
            _player = player;
            Config = baseConfig;
            Type = Config.ImplantType;
            _player.Status.StatHolder.AddStatModifiers(Config.PawnModifiers);
            foreach(WeaponSlot slot in _player.Equipment.WeaponSlots)
            {
                slot.Weapon.StatHolder.AddStatModifiers(Config.WeaponModifiers);
            }
        }

        public void ChangeConfig(ImplantConfig config)
        {
            _player.Status.StatHolder.RemoveStatModifiers(Config.PawnModifiers);
            foreach (WeaponSlot slot in _player.Equipment.WeaponSlots)
            {
                slot.Weapon.StatHolder.RemoveStatModifiers(Config.WeaponModifiers);
            }
            Config = config;
            _player.Status.StatHolder.AddStatModifiers(Config.PawnModifiers);
            foreach (WeaponSlot slot in _player.Equipment.WeaponSlots)
            {
                slot.Weapon.StatHolder.AddStatModifiers(Config.WeaponModifiers);
            }
        }
    }
}