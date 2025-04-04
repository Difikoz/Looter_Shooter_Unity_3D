using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public class PlayerEquipmentComponent : PlayerComponent
    {
        [SerializeField] private List<ImplantConfig> _baseImplants = new();

        public int ActiveWeaponIndex { get; private set; }
        public WeaponSlot ActiveWeaponSlot { get; private set; }
        public List<WeaponSlot> WeaponSlots { get; private set; }
        public Dictionary<string, ImplantSlot> ImplantSlots { get; private set; }
        public ImplantSlot GetImplantSlot(string id) => ImplantSlots[id];

        public override void InitializeComponent()
        {
            base.InitializeComponent();
            WeaponSlots = new();
            ImplantSlots = new();
            WeaponSlot[] weaponsSlots = GetComponentsInChildren<WeaponSlot>();
            foreach (WeaponSlot slot in weaponsSlots)
            {
                WeaponSlots.Add(slot);
                slot.InitializeComponent();
                slot.Weapon.gameObject.SetActive(false);
            }
            foreach (ImplantConfig config in _baseImplants)
            {
                ImplantSlots.Add(config.ImplantType.ID, new(_player, config));
            }
            ActiveWeaponSlot = WeaponSlots[0];
        }

        public override void EnableComponent()
        {
            base.EnableComponent();
            ActiveWeaponSlot.EnableComponent();
        }

        public override void DisableComponent()
        {
            ActiveWeaponSlot.DisableComponent();
            base.DisableComponent();
        }

        public override void HandleComponent()
        {
            base.HandleComponent();
            ActiveWeaponSlot.HandleComponent();
        }

        public void ChangeImplant(ImplantConfig config)
        {
            GetImplantSlot(config.ImplantType.ID).ChangeConfig(config);
        }

        public void ToggleActiveWeaponSlot(int index)
        {
            if (index >= WeaponSlots.Count)
            {
                return;
            }
            if (ActiveWeaponSlot.Weapon.IsPerfomingAction)
            {
                return;
            }
            ActiveWeaponIndex = index;
            ActiveWeaponSlot.DisableComponent();
            ActiveWeaponSlot = WeaponSlots[ActiveWeaponIndex];
            ActiveWeaponSlot.EnableComponent();
        }

        public void NextWeapon()
        {
            if (ActiveWeaponIndex < WeaponSlots.Count - 1)
            {
                ToggleActiveWeaponSlot(ActiveWeaponIndex + 1);
            }
            else
            {
                ToggleActiveWeaponSlot(0);
            }
        }

        public void PreviousWeapon()
        {
            if (ActiveWeaponIndex > 0)
            {
                ToggleActiveWeaponSlot(ActiveWeaponIndex - 1);
            }
            else
            {
                ToggleActiveWeaponSlot(WeaponSlots.Count - 1);
            }
        }
    }
}