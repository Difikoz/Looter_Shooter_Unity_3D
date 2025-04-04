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
        [SerializeField] private LayerMask _detectableMask;
        public GameObject ProjectileController;

        private AudioSource _fireAudioSource;
        private AudioSource _reloadAudioSource;
        private Coroutine _fireCoroutine;
        private Coroutine _reloadCoroutine;
        private bool _isFiring;

        public PlayerController Player { get; private set; }
        public WeaponSlot WeaponSlot { get; private set; }
        public Dictionary<string, WeaponPart> WeaponParts { get; private set; }
        public List<WeaponPartAttachPoint> AttachPoints { get; private set; }
        public AimPoint AimPoint { get; private set; }
        public Vector3 AimPosition { get; private set; }
        public ShootPoint ShootPoint { get; private set; }
        public StatHolder StatHolder { get; private set; }
        public DamageTypeConfig DamageType { get; private set; }
        public bool SingleShot { get; private set; }
        public int AmmoInMag { get; private set; }
        public bool IsAiming { get; private set; }

        public WeaponPart GetWeaponPart(string id) => WeaponParts[id];
        public bool IsPerfomingAction => IsFiring || IsReloading;
        public bool IsFiring => _fireCoroutine != null;
        public bool IsReloading => _reloadCoroutine != null;

        public void Initialize(WeaponSlot slot)
        {
            Player = GetComponentInParent<PlayerController>();
            WeaponSlot = slot;
            _reloadAudioSource = GetComponent<AudioSource>();
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

        public void Enable()
        {
            EventBus.AmmoCountChanged(AmmoInMag, StatHolder.GetStat("Mag Size").CurrentValue);
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
            AimPoint = GetComponentInChildren<AimPoint>();
            AimPosition = Camera.main.transform.InverseTransformPoint(AimPoint.transform.position) + AimPoint.transform.localPosition;
            ShootPoint = GetComponentInChildren<ShootPoint>();
            _fireAudioSource = ShootPoint.GetComponent<AudioSource>();
        }

        public void PerformFire()
        {
            _isFiring = true;
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
            _isFiring = false;
        }

        private IEnumerator FireCoroutine()
        {
            while (_isFiring && AmmoInMag > 0)
            {
                AmmoInMag--;
                _fireAudioSource.Play();
                for (int i = 0; i < StatHolder.GetStat("Projectiles Per Shot").CurrentValue; i++)
                {
                    LeanPool.Spawn(ProjectileController, ShootPoint.transform.position, Quaternion.Euler(CalculateShootDirection())).GetComponent<ProjectileController>().Initialize(this);
                }
                AddRecoilForce(StatHolder.GetStat("Recoil").CurrentValue / 100f);
                EventBus.AmmoCountChanged(AmmoInMag, StatHolder.GetStat("Mag Size").CurrentValue);
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
            return new Vector3(GetRandomSpread(), GetRandomSpread(), GetRandomSpread()) / 50f;
        }

        private float GetRandomSpread()
        {
            return Random.Range(StatHolder.GetStat("Spread").CurrentValue, -StatHolder.GetStat("Spread").CurrentValue);
        }

        private void AddRecoilForce(float force)
        {
            if (IsAiming)
            {
                force /= 4f;
            }
            else
            {
                force /= 2f;
            }
            Player.Look.AddRecoilForce(force);
            if (transform.localRotation.x > -0.1f)
            {
                transform.localRotation *= Quaternion.Euler(-force, 0f, 0f);
            }
            transform.localPosition += Vector3.back * force * Mathf.InverseLerp(-0.1f, 0f, transform.localPosition.z) / 25f;
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
                _reloadCoroutine = null;
            }
        }

        private IEnumerator ReloadCoroutine()
        {
            _reloadAudioSource.Play();
            yield return new WaitForSeconds(600f / StatHolder.GetStat("Reload Speed").CurrentValue);
            int ammoDif = StatHolder.GetStat("Mag Size").CurrentValue - AmmoInMag;
            AmmoInMag += ammoDif;
            EventBus.AmmoCountChanged(AmmoInMag, StatHolder.GetStat("Mag Size").CurrentValue);
            _reloadCoroutine = null;
        }

        public void ToggleAim()
        {
            IsAiming = !IsAiming;
        }

        public void PerformAim()
        {
            IsAiming = true;
        }

        public void CancelAim()
        {
            IsAiming = false;
        }

        public Vector3 GetHitPoint()
        {
            if (Physics.Raycast(ShootPoint.transform.position, ShootPoint.transform.forward, out RaycastHit hit, float.MaxValue, _detectableMask))
            {
                return hit.point;
            }
            else
            {
                return ShootPoint.transform.position + ShootPoint.transform.forward * 1000f;
            }
        }
    }
}