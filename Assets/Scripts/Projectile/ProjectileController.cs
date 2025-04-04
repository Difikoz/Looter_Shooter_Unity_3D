using Lean.Pool;
using System.Collections;
using UnityEngine;

namespace WinterUniverse
{
    [RequireComponent(typeof(Rigidbody))]
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private LayerMask _detectableMask;
        [SerializeField] private GameObject _testImpact;

        private PlayerController _player;
        private WeaponController _weapon;
        private DamageTypeConfig _damageType;
        private float _damage;
        private float _range;
        private float _force;

        public void Initialize(WeaponController weapon)
        {
            _weapon = weapon;
            _player = _weapon.Player;
            _damageType = _weapon.DamageType;
            _damage = _weapon.StatHolder.GetStat("Damage").CurrentValue;
            _range = _weapon.StatHolder.GetStat("Fire Range").CurrentValue;
            _force = _weapon.StatHolder.GetStat("Projectile Force").CurrentValue;
            //StartCoroutine(DespawnTimer());
            _rb.linearVelocity = transform.forward * _force;
        }

        private IEnumerator DespawnTimer()
        {
            yield return new WaitForSeconds(_range / _force * 1.1f);
            Despawn();
        }

        private void Despawn()
        {
            _rb.linearVelocity = Vector3.zero;
            LeanPool.Despawn(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1f, _detectableMask))
            {
                LeanPool.Despawn(LeanPool.Spawn(_testImpact, hit.point + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal)), 10f);
            }
            // try get component
            Despawn();
        }
    }
}