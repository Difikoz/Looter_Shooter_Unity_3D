using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private StatHolderConfig _statHolder;
        [SerializeField] private WeaponPresetConfig _preset;
        public GameObject ProjectileController;

        private Coroutine _fireCoroutine;
        private Coroutine _reloadCoroutine;
        private float _recoil;

        public PlayerController Player { get; private set; }
        public Dictionary<string, WeaponPart> WeaponParts { get; private set; }
        public List<WeaponPartAttachPoint> AttachPoints { get; private set; }
        public ShootPoint ShootPoint { get; private set; }
        public StatHolder StatHolder { get; private set; }
        public DamageTypeConfig DamageType { get; private set; }
        public bool SingleShot { get; private set; }
        public bool IsFiring { get; private set; }
        public int AmmoInMag { get; private set; }

        public WeaponPart GetWeaponPart(string id) => WeaponParts[id];
        public bool IsPerfomingAction => _fireCoroutine != null || _reloadCoroutine != null;

        public void Initialize()
        {
            Player = GetComponentInParent<PlayerController>();
            WeaponParts = new();
            AttachPoints = new();
            StatHolder = new(_statHolder.Stats);
            foreach (WeaponPartConfig config in _preset.Parts)
            {
                WeaponParts.Add(config.PartType.ID, new(this, config));
            }
            UpdateAttachPoints();
            foreach (KeyValuePair<string, WeaponPart> kvp in WeaponParts)
            {
                kvp.Value.ApplyAttachTransform();
            }
        }

        public void ChangePart(WeaponPartConfig config)
        {
            GetWeaponPart(config.PartType.ID).ChangeConfig(config);
        }

        public void UpdateAttachPoints()
        {
            AttachPoints.Clear();
            WeaponPartAttachPoint[] points = GetComponentsInChildren<WeaponPartAttachPoint>();
            foreach (WeaponPartAttachPoint point in points)
            {
                AttachPoints.Add(point);
            }
            ShootPoint = GetComponentInChildren<ShootPoint>();
        }

        public void PerformFire()
        {
            IsFiring = true;
            if (IsPerfomingAction)
            {
                return;
            }
            if (AmmoInMag == 0)
            {
                return;
            }
            _fireCoroutine = StartCoroutine(FireCoroutine());
        }

        public void CancelFire()
        {
            IsFiring = false;
        }

        private IEnumerator FireCoroutine()
        {
            while (IsFiring && AmmoInMag > 0)
            {
                AmmoInMag--;
                for (int i = 0; i < StatHolder.GetStat("Projectiles Per Shot").CurrentValue; i++)
                {
                    LeanPool.Spawn(ProjectileController, ShootPoint.transform.position, Quaternion.Euler(CalculateShootDirection())).GetComponent<ProjectileController>().Initialize(this);
                }
                AddRecoilForce(StatHolder.GetStat("Recoil").CurrentValue / 100f);
                yield return new WaitForSeconds(60f / StatHolder.GetStat("Fire Rate").CurrentValue);
                if (SingleShot)
                {
                    break;
                }
            }
            _fireCoroutine = null;
        }

        private Vector3 CalculateShootDirection()
        {
            return ShootPoint.transform.eulerAngles + CalculateSpread();
        }

        private Vector3 CalculateSpread()
        {
            return new Vector3(GetRandomSpread(), GetRandomSpread(), GetRandomSpread()) / 100f;
        }

        private float GetRandomSpread()
        {
            return Random.Range(StatHolder.GetStat("Spread").CurrentValue, -StatHolder.GetStat("Spread").CurrentValue);
        }

        private void AddRecoilForce(float force)
        {
            force /= 2f;
            Player.Look.AddRecoilForce(force);
            //_recoil = transform.localRotation.x;
            //_recoil -= force;
            //_recoil = Mathf.Clamp(_recoil, -30f, 0f);
            //transform.localRotation = Quaternion.Euler(_recoil, 0f, 0f);
            if (transform.localPosition.z > -1f)
            {
                transform.localPosition += Vector3.back * force / 10f;
            }
        }

        public void PerformReload()
        {
            if (IsPerfomingAction)
            {
                return;
            }
            if (AmmoInMag == StatHolder.GetStat("Mag Size").CurrentValue)
            {
                return;
            }
            _reloadCoroutine = StartCoroutine(ReloadCoroutine());
        }

        public void CancelReload()
        {
            if (_reloadCoroutine != null)
            {
                StopCoroutine(_reloadCoroutine);
            }
        }

        private IEnumerator ReloadCoroutine()
        {
            yield return new WaitForSeconds(600f / StatHolder.GetStat("Reload Speed").CurrentValue);
            int ammoDif = StatHolder.GetStat("Mag Size").CurrentValue - AmmoInMag;
            AmmoInMag += ammoDif;
            _reloadCoroutine = null;
        }
    }
}