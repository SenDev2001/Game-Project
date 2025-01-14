using UnityEngine;

public class BusMovementTrigger : MonoBehaviour
{
    public GameObject bus;         
    public float moveSpeed = -8f;    
    private bool shouldMove = false;  

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            StartBusMovement();
        }
    }

    void StartBusMovement()
    {
        shouldMove = true; 
    }

    void Update()
    {
        if (shouldMove)
        {
            bus.transform.Translate(0, 0, moveSpeed * Time.deltaTime);
        }
    }
}
