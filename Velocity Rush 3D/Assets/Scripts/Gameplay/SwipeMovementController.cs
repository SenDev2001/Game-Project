using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class SwipeMovementController : MonoBehaviour
    {
        private int _targetLane = 1; // 0: Left, 1: Middle, 2: Right
        private Vector3 _targetPosition;
        private Rigidbody _rb;

        public float laneDistance = 2.39f;
        public float leftrightSpeed = 10f;
        public float forwardSpeed = 12f;
        public float jumpForce = 6f;
        public float minimumSwipeDistance = 40f;
        private bool _isGrounded;
        private bool _canJump = true;

        private InputAction swipeAction;
        private Vector2 swipeDelta;
        private float lastSwipeTime;
        private float swipeCooldown = 0.1f; // Prevents multiple quick swipes

        public float gravityScale = 1.5f;
        public float groundCheckDistance = 1.1f;

        private void Awake()
        {
            var inputActions = new PlayerInput();
            swipeAction = inputActions.Move.Swipe;
            swipeAction.Enable();
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void Update()
        {
            DetectSwipe();
            MoveForward();
            MoveSideToSide();
            CheckGroundStatus();
            ApplyCustomGravity();
        }

        private void OnDisable()
        {
            swipeAction.Disable();
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