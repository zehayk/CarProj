using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player;
    private Rigidbody playerRigidbody;
    private Rigidbody enemyRigidbody;

    public float acceleration = 2f;
    private float topSpeed;

    void Start()
    {
        // Find the player GameObject by tag
        player = GameObject.FindGameObjectWithTag("Player");

        // Try to get the Rigidbody component attached to the player GameObject
        playerRigidbody = player.GetComponent<Rigidbody>();

        // Get the Rigidbody component attached to the enemy GameObject
        enemyRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Change where it is headed
        Vector3 playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(playerPosition);

        // Change speed over time
        Vector3 playerVelocity = playerRigidbody.velocity;

        float currentVelocityMagnitude = playerVelocity.magnitude;
        if (currentVelocityMagnitude > topSpeed)
        {
            topSpeed = currentVelocityMagnitude;
        }

        // Move toward the player using velocity
        Vector3 newVelocity = enemyRigidbody.velocity + (transform.forward * acceleration * Time.deltaTime);
        newVelocity = Vector3.ClampMagnitude(newVelocity, topSpeed);
        enemyRigidbody.velocity = newVelocity;
    }
}
