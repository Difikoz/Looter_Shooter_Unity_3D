using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public class WeaponSlot : PlayerComponent
    {
        [SerializeField] private float _aimSpeed = 4f;
        [SerializeField] private float _swaySpeed = 4f;
        [SerializeField] private float _swayMultiplier = 10f;

        private Vector3 _swayRotation;

        public WeaponController Weapon { get; private set; }

        public override void InitializeComponent()
        {
            _player = GetComponentInParent<PlayerController>();
            Weapon = GetComponentInChildren<WeaponController>();
            Weapon.Initialize(this);
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
            Weapon.Enable();
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
            Weapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            Weapon.gameObject.SetActive(false);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            Weapon.CancelAim();
        }

        public override void HandleComponent()
        {
            HandleRecoil();
            HandleSway();
            HandleAiming();
        }

        private void HandleRecoil()
        {
            Weapon.transform.localPosition = Vector3.Lerp(Weapon.transform.localPosition, Vector3.zero, Weapon.StatHolder.GetStat("Recoil Recovery").CurrentValue / 100f * Time.deltaTime);
            if (!Weapon.IsPerfomingAction)
            {
                Weapon.transform.localRotation = Quaternion.Lerp(Weapon.transform.localRotation, Quaternion.identity, Weapon.StatHolder.GetStat("Recoil Recovery").CurrentValue / 100f * Time.deltaTime);
            }
        }

        private void HandleSway()
        {
            if (Weapon.IsAiming)
            {
                _swayRotation = Vector3.zero;
            }
            else
            {
                _swayRotation.x = -Mathf.Clamp(_player.Input.LookInput.y, -1f, 1f);
                _swayRotation.y = Mathf.Clamp(_player.Input.LookInput.x, -1f, 1f);
                _swayRotation.z = -Mathf.Clamp(_player.Input.MoveInput.x, -1f, 1f);
            }
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(_swayRotation * _swayMultiplier), _swaySpeed * Time.deltaTime);
        }

        private void HandleAiming()
        {
            if (Weapon.IsAiming && !Weapon.IsReloading)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, -Weapon.AimPosition, _aimSpeed * Time.deltaTime);
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, _aimSpeed * Time.deltaTime);
            }
        }
    }
}