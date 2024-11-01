using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class E_Movement : MonoBehaviour
    {
        private int _targetLane = 1; 
        private Vector3 _targetPosition;
        private Rigidbody _rb;

        public float laneDistance = 3f;
        public float leftrightSpeed = 9.5f;
        public float forwardSpeed = 20f;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

            float targetX = (_targetLane - 1) * laneDistance;
            _targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, leftrightSpeed * Time.deltaTime);
        }

        public void MoveLeft()
        {
            if (_targetLane > 0) _targetLane--;
        }

        public void MoveRight()
        {
            if (_targetLane < 2) _targetLane++;
        }

        public void MoveToMiddle()
        {
            if (_targetLane == 0)
                _targetLane = 1; 
            else if (_targetLane == 1)
                _targetLane = 2; 
        }

        public int TargetLane => _targetLane; 
    }
}
