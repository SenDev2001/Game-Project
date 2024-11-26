using UnityEngine;

namespace Gameplay
{
    public class E_Jump : MonoBehaviour
    {
        public float jumpForce = 18.0f;
        public float gravityScale = 1.0f;

        private Rigidbody _rb;
        private bool _isGrounded;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

            if (_isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)))
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
            _rb.velocity = new Vector3(_rb.velocity.x, jumpForce, _rb.velocity.z);
        }

        public bool IsGrounded() => _isGrounded;
    }
}
