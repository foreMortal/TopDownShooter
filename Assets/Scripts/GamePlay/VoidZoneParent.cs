using UnityEngine;

public class VoidZoneParent : MonoBehaviour
{
    private PlayerManager playerManager;
    private bool managerGavered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!managerGavered)
            {
                playerManager = collision.GetComponent<PlayerManager>();
                managerGavered = true;
            }
            AffectPlayer(playerManager);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!managerGavered)
            {
                playerManager = collision.GetComponent<PlayerManager>();
                managerGavered = true;
            }
            RemoveAffect(playerManager);
        }
    }

    protected virtual void AffectPlayer(PlayerManager manager) { }
    protected virtual void RemoveAffect(PlayerManager manager) { }
}
