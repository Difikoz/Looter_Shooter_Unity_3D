using System.Collections;
using UnityEngine;

namespace WinterUniverse
{
    public class PlayerLookComponent : PlayerComponent
    {
        private float _horizontalAngle;
        private float _verticalAngle;

        public float HorizontalRotateSpeed = 20f;
        public float VerticalRotateSpeed = 10f;
        public bool InvertHorizontal;
        public bool InvertVertical;
        [Range(0.1f, 2f)] public float HorizontalSensitivity = 1f;
        [Range(0.1f, 2f)] public float VerticalSensitivity = 1f;
        public float VerticalAngleLimit = 85f;

        public override void HandleComponent()
        {
            base.HandleComponent();
            if (_player.Input.LookInput.x != 0f)
            {
                _horizontalAngle += HorizontalRotateSpeed * _player.Input.LookInput.x * HorizontalSensitivity * (InvertHorizontal ? -1f : 1f) * Time.deltaTime;
                if (_horizontalAngle >= 360f || _horizontalAngle <= -360f)
                {
                    _horizontalAngle = 0f;
                }
            }
            if (_player.Input.LookInput.y != 0f)
            {
                _verticalAngle = Mathf.Clamp(_verticalAngle - (VerticalRotateSpeed * _player.Input.LookInput.y * VerticalSensitivity * (InvertVertical ? -1f : 1f) * Time.deltaTime), -VerticalAngleLimit, VerticalAngleLimit);
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, _horizontalAngle, 0f), HorizontalRotateSpeed * Time.deltaTime);
            _player.Head.localRotation = Quaternion.Slerp(_player.Head.localRotation, Quaternion.Euler(_verticalAngle, 0f, 0f), VerticalRotateSpeed * Time.deltaTime);
        }

        public void AddRecoilForce(float force)
        {
            if (Random.value > 0.5f)
            {
                _horizontalAngle += force / 2f;
            }
            else
            {
                _horizontalAngle -= force / 2f;
            }
            _verticalAngle = Mathf.Clamp(_verticalAngle - force, -VerticalAngleLimit, VerticalAngleLimit);
        }
    }
}