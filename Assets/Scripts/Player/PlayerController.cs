using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerEquipmentComponent))]
    [RequireComponent(typeof(PlayerInputComponent))]
    [RequireComponent(typeof(PlayerLocomotionComponent))]
    [RequireComponent(typeof(PlayerLookComponent))]
    [RequireComponent(typeof(PlayerStatusComponent))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform _head;

        private List<PlayerComponent> _components;

        public CharacterController CC { get; private set; }
        public PlayerEquipmentComponent Equipment { get; private set; }
        public PlayerInputComponent Input { get; private set; }
        public PlayerLocomotionComponent Locomotion { get; private set; }
        public PlayerLookComponent Look { get; private set; }
        public PlayerStatusComponent Status { get; private set; }
        public Transform Head => _head;

        private void Awake()
        {
            _components = new();
            CC = GetComponent<CharacterController>();
            Equipment = GetComponent<PlayerEquipmentComponent>();
            Input = GetComponent<PlayerInputComponent>();
            Locomotion = GetComponent<PlayerLocomotionComponent>();
            Look = GetComponent<PlayerLookComponent>();
            Status = GetComponent<PlayerStatusComponent>();
            _components.Add(Status);
            _components.Add(Equipment);
            _components.Add(Input);
            _components.Add(Locomotion);
            _components.Add(Look);
            foreach (PlayerComponent component in _components)
            {
                component.InitializeComponent();
            }
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnEnable()
        {
            foreach (PlayerComponent component in _components)
            {
                component.EnableComponent();
            }
        }

        private void OnDisable()
        {
            foreach (PlayerComponent component in _components)
            {
                component.DisableComponent();
            }
        }

        private void Update()
        {
            foreach (PlayerComponent component in _components)
            {
                component.HandleComponent();
            }
        }
    }
}