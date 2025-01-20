using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovementController : MonoBehaviour
    {
        private int _targetLane = 1; // 0: Left, 1: Middle, 2: Right
        private Vector3 _targetPosition;
        private Rigidbody _rb;

        public float laneDistance = 2.39f;
        public float leftrightSpeed = 10f;
        public float forwardSpeed = 12f;
        public float jumpForce = 7f;
        public float minimumSwipeDistance = 40f; // Adjust based on your swipe requirements
        public float gravityScale = 1.2f;
        public float groundCheckDistance = 1.1f;

        private bool _isGrounded;
        private bool _canJump = true;

        private InputAction swipeAction;
        private InputAction keyboardAction;

        private Vector2 swipeDelta;
        private float lastSwipeTime;
        private float swipeCooldown = 0.1f;
        private float lastKeyboardTime; // For keyboard cooldown
        private float keyboardCooldown = 0.1f; // Cooldown time for keyboard control

        private void Awake()
        {
            var inputActions = new PlayerInput();

            // Initialize both swipe and keyboard actions
            swipeAction = inputActions.Move.Swipe;
            keyboardAction = inputActions.Move.Keyboard;

            swipeAction.Enable();
            keyboardAction.Enable();
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void Update()
        {
            DetectSwipe(); // Handle swipe control
            HandleKeyboardInput(); // Handle keyboard control

            MoveForward();
            MoveSideToSide();
            CheckGroundStatus();
            ApplyCustomGravity();
        }

        private void OnDisable()
        {
            swipeAction.Disable();
            keyboardAction.Disable();
        }

        private void DetectSwipe()
        {
            if (Time.time - lastSwipeTime < swipeCooldown) return;

            swipeDelta = swipeAction.ReadValue<Vector2>();
            if (swipeDelta.magnitude >= minimumSwipeDistance)
            {
                ProcessSwipe(swipeDelta);
                swipeDelta = Vector2.zero;
                lastSwipeTime = Time.time;
            }
        }

        private void ProcessSwipe(Vector2 delta)
        {
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) // Horizontal swipe (left-right)
            {
                if (delta.x > 0) MoveRight();
                else MoveLeft();
            }
            else if (delta.y > 0) // Vertical swipe (up)
            {
                Jump();
            }
        }

        private void HandleKeyboardInput()
        {
            // Ensure cooldown between keyboard inputs
            if (Time.time - lastKeyboardTime < keyboardCooldown) return;

            Vector2 input = keyboardAction.ReadValue<Vector2>();

            if (input.y > 0) Jump(); // Jump on up arrow key
            if (input.x < 0) MoveLeft(); // Move left on left arrow key
            if (input.x > 0) MoveRight(); // Move right on right arrow key

            lastKeyboardTime = Time.time; // Update the last keyboard input time
        }

        private void MoveForward()
        {
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
        }

        private void MoveSideToSide()
        {
            float targetX = (_targetLane - 1) * laneDistance;
            _targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, leftrightSpeed * Time.deltaTime);
        }

        private void Jump()
        {
            if (_isGrounded && _canJump)
            {
                _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
                _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                _canJump = false;
            }
        }

        private void CheckGroundStatus()
        {
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
            if (_isGrounded) _canJump = true;
        }

        private void ApplyCustomGravity()
        {
            if (!_isGrounded)
            {
                _rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
            }
        }

        public void MoveLeft()
        {
            if (_targetLane > 0) _targetLane--;
        }

        public void MoveRight()
        {
            if (_targetLane < 2) _targetLane++;
        }
    }
}
