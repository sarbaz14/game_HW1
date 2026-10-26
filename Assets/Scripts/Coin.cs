using UnityEngine;

public class Coin : Interactable
{
    public int value = 10;
    public AudioClip collectSound;

    private void Update()
    {
        // Clean up when far behind player
        if (PlayerController.Instance != null && transform.position.x < PlayerController.Instance.transform.position.x - 20f)
        {
            Destroy(gameObject);
        }
    }

    public override void Interact(PlayerController player)
    {
        player.AddScore(value);
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }
        Destroy(gameObject);
    }
}
