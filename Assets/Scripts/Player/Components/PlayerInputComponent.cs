using UnityEngine;

namespace WinterUniverse
{
    public class PlayerInputComponent : PlayerComponent
    {
        public bool HoldToAim;

        private PlayerInputActions _inputActions;
        private bool _dashPerfomed;
        private bool _jumpPerfomed;
        private bool _jumpCanceled;
        private bool _interactPerfomed;
        private bool _attackPerfomed;
        private bool _attackCanceled;
        private bool _firePerfomed;
        private bool _fireCanceled;
        private bool _aimPerfomed;
        private bool _aimCanceled;
        private bool _reloadPerfomed;
        private bool _previousWeaponPerfomed;
        private bool _nextWeaponPerfomed;

        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }

        public override void InitializeComponent()
        {
            base.InitializeComponent();
            _inputActions = new();
        }

        public override void EnableComponent()
        {
            base.EnableComponent();
            _inputActions.Enable();
            _inputActions.Player.Dash.performed += ctx => _dashPerfomed = true;
            _inputActions.Player.Jump.performed += ctx => _jumpPerfomed = true;
            _inputActions.Player.Jump.canceled += ctx => _jumpCanceled = true;
            _inputActions.Player.Interact.performed += ctx => _interactPerfomed = true;
            _inputActions.Weapon.Attack.performed += ctx => _attackPerfomed = true;
            _inputActions.Weapon.Attack.canceled += ctx => _attackCanceled = true;
            _inputActions.Weapon.Fire.performed += ctx => _firePerfomed = true;
            _inputActions.Weapon.Fire.canceled += ctx => _fireCanceled = true;
            _inputActions.Weapon.Aim.performed += ctx => _aimPerfomed = true;
            _inputActions.Weapon.Aim.canceled += ctx => _aimCanceled = true;
            _inputActions.Weapon.Reload.performed += ctx => _reloadPerfomed = true;
            _inputActions.Weapon.PreviousWeapon.performed += ctx => _previousWeaponPerfomed = true;
            _inputActions.Weapon.NextWeapon.performed += ctx => _nextWeaponPerfomed = true;
        }

        public override void DisableComponent()
        {
            _inputActions.Player.Dash.performed -= ctx => _dashPerfomed = true;
            _inputActions.Player.Jump.performed -= ctx => _jumpPerfomed = true;
            _inputActions.Player.Jump.canceled -= ctx => _jumpCanceled = true;
            _inputActions.Player.Interact.performed -= ctx => _interactPerfomed = true;
            _inputActions.Weapon.Attack.performed -= ctx => _attackPerfomed = true;
            _inputActions.Weapon.Attack.canceled -= ctx => _attackCanceled = true;
            _inputActions.Weapon.Fire.performed -= ctx => _firePerfomed = true;
            _inputActions.Weapon.Fire.canceled -= ctx => _fireCanceled = true;
            _inputActions.Weapon.Aim.performed -= ctx => _aimPerfomed = true;
            _inputActions.Weapon.Aim.canceled -= ctx => _aimCanceled = true;
            _inputActions.Weapon.Reload.performed -= ctx => _reloadPerfomed = true;
            _inputActions.Weapon.PreviousWeapon.performed -= ctx => _previousWeaponPerfomed = true;
            _inputActions.Weapon.NextWeapon.performed -= ctx => _nextWeaponPerfomed = true;
            _inputActions.Disable();
            base.DisableComponent();
        }

        public override void HandleComponent()
        {
            base.HandleComponent();
            MoveInput = _inputActions.Player.Move.ReadValue<Vector2>();
            LookInput = _inputActions.Player.Look.ReadValue<Vector2>();
            if (_dashPerfomed)
            {
                _dashPerfomed = false;
                _player.Locomotion.PerformDash();
            }
            if (_jumpPerfomed)
            {
                _jumpPerfomed = false;
                _player.Locomotion.PerformJump();
            }
            else if (_jumpCanceled)
            {
                _jumpCanceled = false;
                _player.Locomotion.CancelJump();
            }
            if (_interactPerfomed)
            {
                _interactPerfomed = false;
                //...
            }
            if (_attackPerfomed)
            {
                _attackPerfomed = false;
                //...
            }
            else if (_attackCanceled)
            {
                _attackCanceled = false;
                //...
            }
            if (_firePerfomed)
            {
                _firePerfomed = false;
                _player.Equipment.ActiveWeaponSlot.Weapon.PerformFire();
            }
            else if (_fireCanceled)
            {
                _fireCanceled = false;
                _player.Equipment.ActiveWeaponSlot.Weapon.CancelFire();
            }
            if (_aimPerfomed)
            {
                _aimPerfomed = false;
                if (HoldToAim)
                {
                    // start aim
                }
                else
                {
                    // toggle aim
                }
            }
            else if (_aimCanceled)
            {
                _aimCanceled = false;
                if (HoldToAim)
                {
                    // stop aim
                }
            }
            if (_reloadPerfomed)
            {
                _reloadPerfomed = false;
                _player.Equipment.ActiveWeaponSlot.Weapon.PerformReload();
            }
            if (_previousWeaponPerfomed)
            {
                _previousWeaponPerfomed = false;
                _nextWeaponPerfomed = false;
                _player.Equipment.PreviousWeapon();
            }
            else if (_nextWeaponPerfomed)
            {
                _previousWeaponPerfomed = false;
                _nextWeaponPerfomed = false;
                _player.Equipment.NextWeapon();
            }
        }
    }
}