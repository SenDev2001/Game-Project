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
        public float forwardSpeed = 12f; // Initial speed
        public float jumpForce = 7f;
        public float minimumSwipeDistance = 40f;
        public float gravityScale = 1.2f;
        public float groundCheckDistance = 1.1f;

        private bool _isGrounded;
        private bool _canJump = true;

        private InputAction swipeAction;
        private InputAction keyboardAction;

        private Vector2 swipeDelta;
        private float lastSwipeTime;
        private float swipeCooldown = 0.1f;
        private float lastKeyboardTime;
        private float keyboardCooldown = 0.1f;

        private void Awake()
        {
            var inputActions = new PlayerInput();

            swipeAction = inputActions.Move.Swipe;
            keyboardAction = inputActions.Move.Keyboard;
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.constraints = RigidbodyConstraints.FreezeRotation;

            // swipe and keyboard are enabled/disabled based on platform
            SwitchInputMethod();
        }

        private void Update()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                DetectSwipe();
                HandleKeyboardInput(false);
            }
            else if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                DetectSwipe(false);
                HandleKeyboardInput(true);
            }
            else
            {
                DetectSwipe(true);
                HandleKeyboardInput(true);
            }

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

        private void DetectSwipe(bool enable = true)
        {
            if (!enable)
            {
                swipeAction.Disable();
                return;
            }
            swipeAction.Enable();

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
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                if (delta.x > 0) MoveRight();
                else MoveLeft();
            }
            else if (delta.y > 0)
            {
                Jump();
            }
        }

        private void HandleKeyboardInput(bool enable = true)
        {
            if (!enable)
            {
                keyboardAction.Disable();
                return;
            }

            keyboardAction.Enable();

            if (Time.time - lastKeyboardTime < keyboardCooldown) return;

            Vector2 input = keyboardAction.ReadValue<Vector2>();

            if (input.y > 0) Jump();
            if (input.x < 0) MoveLeft();
            if (input.x > 0) MoveRight();

            lastKeyboardTime = Time.time;
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

        private void SwitchInputMethod()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                swipeAction.Enable();
                keyboardAction.Disable();
            }
            else if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                swipeAction.Disable();
                keyboardAction.Enable();
            }
            else
            {
                swipeAction.Enable();
                keyboardAction.Enable();
            }
        }

        // Trigger handler for road end event
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("RoadEnd")) // Assuming the road end object is tagged "RoadEnd"
            {
                IncreaseSpeed();
            }
        }

        // Method to increase speed
        private void IncreaseSpeed()
        {
            if (forwardSpeed < 20)
            {
                forwardSpeed += 1; // Increase speed by 1
            }
        }
    }
}
