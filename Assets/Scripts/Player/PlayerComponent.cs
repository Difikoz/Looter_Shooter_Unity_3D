using UnityEngine;

namespace WinterUniverse
{
    public abstract class PlayerComponent : MonoBehaviour
    {
        protected PlayerController _player;

        public virtual void InitializeComponent()
        {
            _player = GetComponent<PlayerController>();
        }

        public virtual void EnableComponent()
        {

        }
        public virtual void DisableComponent()
        {

        }

        public virtual void HandleComponent()
        {

        }
    }
}