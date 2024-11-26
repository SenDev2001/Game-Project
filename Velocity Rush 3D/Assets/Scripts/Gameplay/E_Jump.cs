using UnityEngine;

namespace Gameplay
{
    public class E_Jump : MonoBehaviour
    {
        public float jumpForce = 10.0f;
        public float gravityScale = 1.1f;

        private Rigidbody _rb;
        private bool _isGrounded;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            // Check if the player is grounded
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

            // Allow jumping with space or W key when grounded
            if (_isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)))
            {
                Jump();
            }
        }

        void FixedUpdate()
        {
            // Apply custom gravity
            _rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
        }

        public void OnSwipe(Vector2 swipeDirection)
        {
            // Check if swipe is upwards and the player is grounded
            if (_isGrounded && swipeDirection.y > 0)
            {
                Jump();
            }
        }

        private void Jump()
        {
            // Set vertical velocity to the jump force while maintaining the current horizontal velocity
            _rb.velocity = new Vector3(_rb.velocity.x, jumpForce, _rb.velocity.z);
        }

        public bool IsGrounded() => _isGrounded;
    }
}
