using Lean.Pool;
using UnityEngine;

namespace WinterUniverse
{
    public class WeaponPart
    {
        private WeaponController _weapon;

        public WeaponPartConfig Config { get; private set; }
        public WeaponPartType Type { get; private set; }
        public GameObject Model { get; private set; }

        public WeaponPart(WeaponController weapon, WeaponPartConfig baseConfig)
        {
            _weapon = weapon;
            Config = baseConfig;
            Type = Config.PartType;
            _weapon.StatHolder.AddStatModifiers(Config.WeaponModifiers);
            Model = LeanPool.Spawn(Config.Prefab, _weapon.transform);
        }

        public void ChangeConfig(WeaponPartConfig config)
        {
            LeanPool.Despawn(Model);
            _weapon.StatHolder.RemoveStatModifiers(Config.WeaponModifiers);
            Config = config;
            _weapon.StatHolder.AddStatModifiers(Config.WeaponModifiers);
            Model = LeanPool.Spawn(Config.Prefab, _weapon.transform);
            _weapon.UpdateAttachPoints();
            ApplyAttachTransform();
        }

        public void ApplyAttachTransform()
        {
            foreach (WeaponPartAttachPoint attachPoint in _weapon.AttachPoints)
            {
                if (attachPoint.Type == Type)
                {
                    Model.transform.SetPositionAndRotation(attachPoint.transform.position, attachPoint.transform.rotation);
                    break;
                }
            }
        }
    }
}