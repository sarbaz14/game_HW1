using UnityEngine;

public class GroundObstacle : Obstacle
{
    private void Update()
    {
        // Clean up when far behind player
        if (PlayerController.Instance != null && transform.position.x < PlayerController.Instance.transform.position.x - 20f)
        {
            Destroy(gameObject);
        }
    }
}
