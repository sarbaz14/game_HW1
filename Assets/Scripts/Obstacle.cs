using UnityEngine;

public abstract class Obstacle : Interactable
{
    public int damage = 1;

    public override void Interact(PlayerController player)
    {
        player.TakeDamage();
    }
}
