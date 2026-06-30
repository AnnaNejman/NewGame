using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float maxSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Transform cameraTransform;
    
        private Rigidbody _rb;
        private Vector3 _moveDirection;
        
        private PlayerControls _controls;
        private Vector2 _moveInput;
    
        public void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _controls = new PlayerControls();
        }
        
        void OnEnable()
        {
            _controls.Player.Enable();
        }

        void OnDisable()
        {
            _controls.Player.Disable();
        }

        public void Update()
        {
            _moveInput = _controls.Player.Move.ReadValue<Vector2>();
        
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            _moveDirection = (camForward * _moveInput.y + camRight * _moveInput.x).normalized;
        }

        public void FixedUpdate()
        {
            Vector3 velocity = _moveDirection * maxSpeed;
            velocity.y = _rb.linearVelocity.y;
            _rb.linearVelocity = velocity;
        
            if (_moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
                _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
            }
        }
    }
}
