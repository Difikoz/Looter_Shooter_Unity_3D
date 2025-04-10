using UnityEngine;

namespace WinterUniverse
{
    public class PlayerLocomotionComponent : PlayerComponent
    {
        public float FallUpGravityMultiplier = 2f;
        public float FallDownGravityMultiplier = 4f;
        public float MaxFallSpeed = 40f;
        [Range(0.1f, 1f)] public float TimeToDash = 0.5f;
        [Range(0.1f, 1f)] public float TimeToJump = 0.25f;
        [Range(0.1f, 1f)] public float TimeToFall = 0.25f;
        public LayerMask ObstacleMask;

        private RaycastHit _groundHit;
        private int _jumpCount;
        private float _jumpTime;
        private float _groundedTime;

        public Vector3 MoveVelocity { get; private set; }
        public Vector3 GroundVelocity { get; private set; }
        public Vector3 DashVelocity { get; private set; }
        public Vector3 FallVelocity { get; private set; }
        public Vector3 Gravity { get; private set; }
        public bool IsGrounded { get; private set; }
        public bool IsDashing { get; private set; }

        public override void InitializeComponent()
        {
            base.InitializeComponent();
            Gravity = new(0f, -10f, 0f);// test
        }

        public override void HandleComponent()
        {
            base.HandleComponent();
            HandleJumping();
            HandleGravity();
            HandleMovement();
            HandleDashing();
            MoveVelocity = GroundVelocity + DashVelocity + FallVelocity;
            _player.CC.Move(MoveVelocity * Time.deltaTime);
        }

        private void HandleJumping()
        {
            if (_jumpTime > 0f)
            {
                if (_groundedTime > 0f)
                {
                    ApplyJumpForce();
                }
                else
                {
                    _jumpTime -= Time.deltaTime;
                }
            }
        }

        private void HandleGravity()
        {
            if (IsGrounded)
            {
                if (FallVelocity.y > 0f || !StayOnGround())
                {
                    IsGrounded = false;
                }
            }
            else
            {
                if (FallVelocity.y <= 0f && StayOnGround())
                {
                    IsGrounded = true;
                    FallVelocity = Gravity / 10f;
                    _groundedTime = TimeToFall;
                    _jumpCount = 0;
                }
                else
                {
                    if (FallVelocity.y > 0f)
                    {
                        FallVelocity += Gravity * FallUpGravityMultiplier * Time.deltaTime;
                    }
                    else if (FallVelocity.y > -MaxFallSpeed)
                    {
                        FallVelocity += Gravity * FallDownGravityMultiplier * Time.deltaTime;
                    }
                    if (_groundedTime > 0f)
                    {
                        _groundedTime -= Time.deltaTime;
                    }
                }
            }
        }

        private void HandleMovement()
        {
            if (_player.Input.MoveInput != Vector2.zero)
            {
                GroundVelocity = Vector3.MoveTowards(GroundVelocity, GetMoveDirection() * _player.Status.StatHolder.GetStat("Move Speed").CurrentValue / 10f, _player.Status.StatHolder.GetStat("Acceleration").CurrentValue / 10f * Time.deltaTime);
            }
            else
            {
                GroundVelocity = Vector3.MoveTowards(GroundVelocity, Vector3.zero, _player.Status.StatHolder.GetStat("Deceleration").CurrentValue / 10f * Time.deltaTime);
            }
        }

        private void HandleDashing()
        {
            if (IsDashing)
            {
                if (DashVelocity == Vector3.zero)
                {
                    IsDashing = false;
                }
                else
                {
                    DashVelocity = Vector3.MoveTowards(DashVelocity, Vector3.zero, _player.Status.StatHolder.GetStat("Dash Force").CurrentValue / 10f / TimeToDash * Time.deltaTime);
                }
            }
            else
            {
                if (DashVelocity != Vector3.zero)
                {
                    IsDashing = true;
                    GroundVelocity = Vector3.zero;
                    FallVelocity = Vector3.zero;
                }
            }
        }

        private void ApplyJumpForce()
        {
            _jumpCount++;
            _jumpTime = 0f;
            _groundedTime = 0f;
            FallVelocity = Vector3.up * Mathf.Sqrt(_player.Status.StatHolder.GetStat("Jump Force").CurrentValue / 10f * -Gravity.y);
        }

        private bool StayOnGround()
        {
            return Physics.SphereCast(transform.position, _player.CC.radius, Vector3.down, out _groundHit, _player.CC.height / 2f - _player.CC.radius * 0.75f, ObstacleMask);
        }

        private Vector3 GetMoveDirection()
        {
            return transform.right * _player.Input.MoveInput.x + transform.forward * _player.Input.MoveInput.y;
        }

        public void PerformDash()
        {
            if (IsDashing)
            {
                return;
            }
            if (_player.Input.MoveInput != Vector2.zero)
            {
                DashVelocity = GetMoveDirection() * _player.Status.StatHolder.GetStat("Dash Force").CurrentValue / 10f;
            }
            else
            {
                DashVelocity = transform.forward * _player.Status.StatHolder.GetStat("Dash Force").CurrentValue / 10f;
            }
        }

        public void PerformJump()
        {
            if (_jumpCount < _player.Status.StatHolder.GetStat("Jump Count").CurrentValue)
            {
                ApplyJumpForce();
            }
            else
            {
                _jumpTime = TimeToJump;
            }
        }

        public void CancelJump()
        {
            if (FallVelocity.y > 0f)
            {
                FallVelocity /= 2f;
            }
        }
    }
}