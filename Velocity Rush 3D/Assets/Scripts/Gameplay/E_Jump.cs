using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class E_Jump : MonoBehaviour
    {
        public float jumpForce = 15.0f; // Force applied when jumping
        public float gravityScale = 1.0f; // Scale for additional gravity

        private Rigidbody _rb;
        private bool _isGrounded;
        public LayerMask groundLayer; // Layer for ground objects

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            // Check if the player is grounded
            _isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundLayer);

            // Jump input
            if (_isGrounded && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
            {
                Jump();
            }
        }

        void FixedUpdate()
        {
            // Apply additional gravity for smoother jumps
            _rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
        }

        void Jump()
        {
            // Apply an upward force to the Rigidbody
            _rb.velocity = new Vector3(_rb.velocity.x, jumpForce, _rb.velocity.z);
        }
    }
}


