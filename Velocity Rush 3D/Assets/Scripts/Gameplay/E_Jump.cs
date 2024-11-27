using UnityEngine;

namespace Gameplay
{
    public class E_Jump : MonoBehaviour
    {
        public float jumpForce = 10.0f;
        public float gravityScale = 1.1f;
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
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

            if (_isGrounded && (_canJump && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))))
            {
                Jump();
            }
        }

        void FixedUpdate()
        {
            _rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
        }

        public void OnSwipe(Vector2 swipeDirection)
        {
            Jump();
        }

        private void Jump()
        {
            if (_isGrounded)
            {
                _rb.velocity = new Vector3(_rb.velocity.x, jumpForce, _rb.velocity.z);
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
