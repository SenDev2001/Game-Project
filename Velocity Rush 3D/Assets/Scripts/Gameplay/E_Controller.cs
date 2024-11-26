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
            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _startTouchPosition = touch.position;
                        _swipeDetected = false;
                        break;

                    case TouchPhase.Moved:
                        _currentTouchPosition = touch.position;
                        DetectSwipe();
                        break;

                    case TouchPhase.Ended:
                        if (_swipeDetected)
                        {
                            Vector2 swipeDirection = _currentTouchPosition - _startTouchPosition;
                            if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                            {
                                if (swipeDirection.x > 0)
                                {
                                    if (_movement.TargetLane == 0)
                                    {
                                        _movement.MoveToMiddle();
                                    }
                                    else
                                    {
                                        _movement.MoveRight();
                                    }
                                }
                                else
                                {
                                    _movement.MoveLeft();
                                }
                            }
                            else
                            {
                                _jump.OnSwipe(swipeDirection.normalized);
                            }
                        }
                        break;
                }
            }
        }

        private void DetectSwipe()
        {
            if (_swipeDetected) return;

            float swipeDestance = (_currentTouchPosition - _startTouchPosition).magnitude;
            float swipeThreshold = 50f;

            if (swipeDestance > swipeThreshold)
            {
                _swipeDetected = true;
            }
        }
    }
}
