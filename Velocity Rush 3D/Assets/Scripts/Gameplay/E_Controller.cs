using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{

    [RequireComponent(typeof(E_Movement))]
    [RequireComponent(typeof(E_Jump))]
    [RequireComponent(typeof(E_Slide))]
    public class E_Controller : MonoBehaviour
    {
        private E_Movement _movement;
        private E_Jump _jump;
        private E_Slide _slide;

        private void Start()
        {
            _movement = GetComponent<E_Movement>();
            _jump = GetComponent<E_Jump>();
            _slide = GetComponent<E_Slide>();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) _movement.MoveLeft();
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) _movement.MoveRight();
          
        }
    }
}

