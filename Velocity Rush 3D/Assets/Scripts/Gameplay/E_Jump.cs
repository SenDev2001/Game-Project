using UnityEngine;

namespace Gameplay
{
    public class E_Jump : MonoBehaviour
    {
        public float jumpForce = 5.0f;  // Reduced jump force for controlled jump
        public float gravityScale = 3.0f;  // Increased gravity to make the player fall faster
        public float groundCheckDistance = 1.1f;

        private Rigidbody _rb;
        private bool _isGrounded;
        private bool _canJump = true;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            CheckGroundStatus();
            HandleJumpInput();
        }

        void FixedUpdate()
        {
            ApplyCustomGravity();
        }

        public void OnSwipe(Vector2 swipeDirection)
        {
            Jump();
        }

        private void CheckGroundStatus()
        {
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
        }

        private void HandleJumpInput()
        {
            if (_isGrounded && _canJump && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)))
            {
                Jump();
            }
        }

        private void ApplyCustomGravity()
        {
            _rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
        }

        private void Jump()
        {
            if (_isGrounded)
            {
                // Set velocity to jump only vertically, no horizontal movement
                _rb.velocity = new Vector3(0, jumpForce, 0);
                _canJump = false;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.relativeVelocity.magnitude > 0.1f)
            {
                Vector3 normal = collision.contacts[0].normal;
                if (Vector3.Dot(normal, Vector3.up) > 0.5f)
                {
                    _canJump = true;
                }
            }
        }

        public bool IsGrounded() => _isGrounded;
    }
}
