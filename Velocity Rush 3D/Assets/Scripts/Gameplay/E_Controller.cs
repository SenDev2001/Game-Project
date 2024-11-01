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
            if (_jump.IsGrounded())
            {
                // Keyboard controls for left/right movement
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {
                    _movement.MoveLeft();
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    _movement.MoveRight();
                }

                // Touch input for swipe detection
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
                                    // Horizontal swipe
                                    if (swipeDirection.x > 0)
                                    {
                                        // Move right if in left lane
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
                                    // Vertical swipe
                                    _jump.OnSwipe(swipeDirection.normalized);
                                }
                            }
                            break;
                    }
                }
            }
        }

        private void DetectSwipe()
        {
            if (_swipeDetected) return;

            float swipeDistance = (_currentTouchPosition - _startTouchPosition).magnitude;
            if (swipeDistance > 50) 
            {
                _swipeDetected = true;
            }
        }
    }
}
