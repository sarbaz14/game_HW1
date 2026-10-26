using UnityEngine;

public class ScrollingGround : MonoBehaviour
{
    [Tooltip("Array of ground segment transforms in order")]
    public Transform[] groundPieces;
    public float pieceWidth = 20f;

    private void Update()
    {
        if (PlayerController.Instance == null || groundPieces == null || groundPieces.Length < 2)
            return;

        float playerX = PlayerController.Instance.transform.position.x;

        // Check if any piece is far behind the player and move it forward
        for (int i = 0; i < groundPieces.Length; i++)
        {
            if (groundPieces[i] != null && groundPieces[i].position.x < playerX - pieceWidth)
            {
                // Find the farthest piece
                float maxRight = groundPieces[i].position.x;
                for (int j = 0; j < groundPieces.Length; j++)
                {
                    if (groundPieces[j] != null && groundPieces[j].position.x > maxRight)
                    {
                        maxRight = groundPieces[j].position.x;
                    }
                }
                groundPieces[i].position = new Vector3(maxRight + pieceWidth, groundPieces[i].position.y, groundPieces[i].position.z);
            }
        }
    }
}
