using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(E_Movement))]
    [RequireComponent(typeof(E_Jump))]
    public class E_Controller : MonoBehaviour
    {
        private E_Movement _movement;
        private E_Jump _jump;

        private Vector2 _startTouchPosition;
        private Vector2 _currentTouchPosition;
        private bool _swipeDetected;

        private void Start()
        {
            _movement = GetComponent<E_Movement>();
            _jump = GetComponent<E_Jump>();
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                HandleTouchInput(Input.GetTouch(0));
            }
        }

        private void HandleTouchInput(Touch touch)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnTouchBegan(touch);
                    break;

                case TouchPhase.Moved:
                    OnTouchMoved(touch);
                    break;

                case TouchPhase.Ended:
                    OnTouchEnded();
                    break;
            }
        }

        private void OnTouchBegan(Touch touch)
        {
            _startTouchPosition = touch.position;
            _swipeDetected = false;
        }

        private void OnTouchMoved(Touch touch)
        {
            _currentTouchPosition = touch.position;
            DetectSwipe();
        }

        private void OnTouchEnded()
        {
            if (_swipeDetected)
            {
                ProcessSwipe();
            }
        }

        private void DetectSwipe()
        {
            if (_swipeDetected) return;

            float swipeDistance = (_currentTouchPosition - _startTouchPosition).magnitude;
            const float swipeThreshold = 50f; // Minimum distance for a swipe to be considered

            if (swipeDistance > swipeThreshold)
            {
                _swipeDetected = true;
            }
        }

        private void ProcessSwipe()
        {
            Vector2 swipeDirection = _currentTouchPosition - _startTouchPosition;

            if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
            {
                ProcessHorizontalSwipe(swipeDirection.x);
            }
            else
            {
                ProcessVerticalSwipe(swipeDirection.normalized);
            }
        }

        private void ProcessHorizontalSwipe(float swipeDirectionX)
        {
            if (swipeDirectionX > 0)
            {
                _movement.MoveRight();
            }
            else
            {
                _movement.MoveLeft();
            }
        }

        private void ProcessVerticalSwipe(Vector2 swipeDirection)
        {
            _jump.OnSwipe(swipeDirection);
        }
    }
}
