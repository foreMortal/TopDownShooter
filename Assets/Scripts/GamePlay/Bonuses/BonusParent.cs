using UnityEngine;

public class BonusParent : MonoBehaviour
{
    public BonusType type;
    protected PlayerManager playerManager;
    private bool managerGavered;
    float disappearIn = 5f, disappearTimer;

    private void Update()
    {
        disappearTimer += Time.deltaTime;
        if( disappearTimer >= disappearIn)
        {
            gameObject.SetActive(false);
            disappearTimer = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (!managerGavered)
            {
                playerManager = collision.GetComponent<PlayerManager>();
                managerGavered = true;
            }
            playerManager.GetBonus(this);
        }
    }

    public virtual void AddBonus(PlayerManager manager) 
    {
        gameObject.SetActive(false);
        disappearTimer = 0f;
    }
    public virtual void RemoveBonus(PlayerManager manager) { }
}

public enum BonusType
{
    Player,
    Weapon,
}