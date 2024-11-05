using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    public Transform player;          // Reference to the player's position
    public float moveSpeed = 5f;      // Speed at which the vehicle moves toward the player
    public float detectionRadius = 10f; // Radius within which the vehicle will move toward the player
    public float maxDistance = 20f;   // Maximum distance for vehicle to follow the player

    private bool isFollowingPlayer = false;

    void Update()
    {
        // Calculate the distance between the vehicle and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if the player is within the detection radius
        if (distanceToPlayer < detectionRadius)
        {
            isFollowingPlayer = true;
        }
        else
        {
            isFollowingPlayer = false;
        }

        // If within detection range, move the vehicle toward the player
        if (isFollowingPlayer)
        {
            // Move the vehicle towards the player's position
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Optionally, make the vehicle rotate towards the player
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
        }
    }
}
