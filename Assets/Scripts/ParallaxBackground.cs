using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Tooltip("Movement factor relative to camera (0 = moves with camera, 1 = static in world)")]
    public float parallaxEffect = 0.5f;

    private Transform camTransform;
    private Vector3 lastCamPos;
    private Material mat;
    public bool useTextureOffset = false;
    public float scrollSpeed = 0.2f;

    private void Start()
    {
        if (Camera.main != null)
        {
            camTransform = Camera.main.transform;
            lastCamPos = camTransform.position;
        }

        var rend = GetComponent<Renderer>();
        if (rend != null)
        {
            mat = rend.material;
        }
    }

    private void LateUpdate()
    {
        if (useTextureOffset && mat != null)
        {
            mat.mainTextureOffset += new Vector2(scrollSpeed * Time.deltaTime, 0);
            return;
        }

        if (camTransform == null)
        {
            if (Camera.main != null)
            {
                camTransform = Camera.main.transform;
                lastCamPos = camTransform.position;
            }
            return;
        }

        Vector3 delta = camTransform.position - lastCamPos;
        transform.position += new Vector3(delta.x * (1f - parallaxEffect), 0, 0);
        lastCamPos = camTransform.position;
    }
}
