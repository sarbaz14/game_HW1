using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float xOffset = 4f;
    public float smoothSpeed = 8f;
    public float fixedY = 0f;

    private void Start()
    {
        if (target == null && PlayerController.Instance != null)
        {
            target = PlayerController.Instance.transform;
        }
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            if (PlayerController.Instance != null)
                target = PlayerController.Instance.transform;
            return;
        }

        Vector3 targetPos = new Vector3(target.position.x + xOffset, fixedY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}
