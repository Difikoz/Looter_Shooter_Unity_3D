using UnityEngine;

namespace WinterUniverse
{
    public class PlayerLocomotionComponent : PlayerComponent
    {
        public float Acceleration = 20f;
        public float Deceleration = 40f;
        public float MoveSpeed = 10f;
        public float DashForce = 10f;
        public float JumpForce = 5f;
        public int JumpCount = 1;
        public float FallUpGravityMultiplier = 1f;
        public float FallDownGravityMultiplier = 2f;
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
                GroundVelocity = Vector3.MoveTowards(GroundVelocity, GetMoveDirection() * MoveSpeed, Acceleration * Time.deltaTime);
            }
            else
            {
                GroundVelocity = Vector3.MoveTowards(GroundVelocity, Vector3.zero, Deceleration * Time.deltaTime);
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
                    DashVelocity = Vector3.MoveTowards(DashVelocity, Vector3.zero, DashForce / TimeToDash * Time.deltaTime);
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
            FallVelocity = Vector3.up * Mathf.Sqrt(JumpForce * -Gravity.y);
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
                DashVelocity = GetMoveDirection() * DashForce;
            }
            else
            {
                DashVelocity = transform.forward * DashForce;
            }
        }

        public void PerformJump()
        {
            if (_jumpCount < JumpCount)
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