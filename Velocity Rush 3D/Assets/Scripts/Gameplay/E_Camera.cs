using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gameplay
{
    public class E_Camera : MonoBehaviour
    {
        public Transform player;
        public Vector3 offset = new Vector3(0f, 3.79f, -3.16f);         
        public float smoothSpeed = 0.125f;
        public float initialXRotation = 30f;

        private void Start()
        {
            Vector3 currentRotation = transform.eulerAngles;
            currentRotation.x = initialXRotation; 

            transform.rotation = Quaternion.Euler(currentRotation);
        }
        void LateUpdate()
        {
            Vector3 desiredPosition = player.position + offset;
            
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = smoothedPosition;


            


        }
    }
}