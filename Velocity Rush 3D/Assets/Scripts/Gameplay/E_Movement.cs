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
        public float forwardSpeed = 10f;

        private int roadCounter = 0;
        public int roadsPerSpeedIncrease = 3;
        public float speedIncreaseFactor = 1.1f;

       

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void Update()
        {
            MoveForward();
            MoveSideToSide();
        }

        private void MoveForward()
        {
            float moveAmount = forwardSpeed * Time.deltaTime;
            transform.Translate(Vector3.forward * moveAmount);
        }

        private void MoveSideToSide()
        {
            float targetX = (_targetLane - 1) * laneDistance;
            _targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
            MoveToTargetPosition();
        }

        private void MoveToTargetPosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, leftrightSpeed * Time.deltaTime);
        }

        public void MoveLeft()
        {
            if (CanMoveLeft())
            {
                _targetLane--;
            }
        }

        public void MoveRight()
        {
            if (CanMoveRight())
            {
                _targetLane++;
            }
        }

        public void MoveToMiddle()
        {
            if (CanMoveToMiddle())
            {
                SetMiddleLane();
            }
        }

        private bool CanMoveLeft()
        {
            return _targetLane > 0;
        }

        private bool CanMoveRight()
        {
            return _targetLane < 2;
        }

        private bool CanMoveToMiddle()
        {
            return _targetLane == 0 || _targetLane == 1;
        }

        private void SetMiddleLane()
        {
            if (_targetLane == 0)
            {
                _targetLane = 1;
            }
            else if (_targetLane == 1)
            {
                _targetLane = 2;
            }
        }

        public void FinishRoad()
        {
            IncrementRoadCounter();
            CheckAndIncreaseSpeed();
        }

        private void IncrementRoadCounter()
        {
            roadCounter++;
        }

        private void CheckAndIncreaseSpeed()
        {
            if (IsSpeedIncreaseNeeded())
            {
                IncreaseSpeed();
                ResetRoadCounter();
            }
        }

        private bool IsSpeedIncreaseNeeded()
        {
            return roadCounter >= roadsPerSpeedIncrease;
        }

        private void IncreaseSpeed()
        {
            if (forwardSpeed < 20f)
            {
                forwardSpeed *= speedIncreaseFactor;
                Debug.Log("Speed increased! New speed: " + forwardSpeed);
            }
            else
            {
                Debug.Log("Max speed reached: " + forwardSpeed);
            }
        }


        private void ResetRoadCounter()
        {
            roadCounter = 0;
        }

        void OnTriggerEnter(Collider other)
        {
            if (IsRoadEnd(other))
            {
                FinishRoad();
            }
        }

        private bool IsRoadEnd(Collider other)
        {
            return other.CompareTag("RoadEnd");
        }

        public int TargetLane => _targetLane;
    }
}