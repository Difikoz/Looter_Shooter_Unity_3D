using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public class WeaponSlot : PlayerComponent
    {
        public WeaponController Weapon { get; private set; }

        public override void InitializeComponent()
        {
            _player = GetComponentInParent<PlayerController>();
            Weapon = GetComponentInChildren<WeaponController>();
            Weapon.Initialize();
        }

        public override void EnableComponent()
        {
            foreach (KeyValuePair<string, WeaponPart> kvp in Weapon.WeaponParts)
            {
                if (kvp.Value.Config == null)
                {
                    continue;
                }
                _player.Status.StatHolder.AddStatModifiers(kvp.Value.Config.PawnModifiers);
            }
            Weapon.gameObject.SetActive(true);
        }

        public override void DisableComponent()
        {
            Weapon.CancelReload();
            foreach (KeyValuePair<string, WeaponPart> kvp in Weapon.WeaponParts)
            {
                if (kvp.Value.Config == null)
                {
                    continue;
                }
                _player.Status.StatHolder.RemoveStatModifiers(kvp.Value.Config.PawnModifiers);
            }
            Weapon.gameObject.SetActive(false);
        }

        public override void HandleComponent()
        {
            Weapon.transform.localPosition = Vector3.Lerp(Weapon.transform.localPosition, Vector3.zero, Weapon.StatHolder.GetStat("Recoil Recovery").CurrentValue / 100f * Time.deltaTime);
            if (!Weapon.IsPerfomingAction)
            {
                Weapon.transform.localRotation = Quaternion.RotateTowards(Weapon.transform.localRotation, Quaternion.identity, Weapon.StatHolder.GetStat("Recoil Recovery").CurrentValue / 100f * Time.deltaTime);
            }
        }
    }
}