using UnityEngine;

namespace WinterUniverse
{
    public class PlayerStatusComponent : PlayerComponent
    {
        [SerializeField] private StatHolderConfig _statHolder;

        public StatHolder StatHolder { get; private set; }

        public override void InitializeComponent()
        {
            base.InitializeComponent();
            StatHolder = new(_statHolder.Stats);
        }
    }
}