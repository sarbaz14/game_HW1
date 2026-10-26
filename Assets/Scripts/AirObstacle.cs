using UnityEngine;

public class AirObstacle : Obstacle
{
    public float amplitude = 0.6f;
    public float frequency = 2f;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float y = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x, startPos.y + y, transform.position.z);

        // Clean up when far behind player
        if (PlayerController.Instance != null && transform.position.x < PlayerController.Instance.transform.position.x - 20f)
        {
            Destroy(gameObject);
        }
    }
}
