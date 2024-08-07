using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerManager : MonoBehaviour
{
    private PlayerShooting playerShooting;
    private PlayerMovement playerMovement;
    private Health health;

    private List<PlayerBonus> bonuses = new();

    public PlayerMovement PlayerMovement { get { return playerMovement; } }
    public PlayerShooting PlayerShooting { get {  return playerShooting; } }
    public Health Health { get { return health; } }

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponent<PlayerShooting>();
        health = GetComponent<Health>();
    }

    private void Update()
    {
        foreach(var b in bonuses)
        {
            b.SelfUpdate(Time.deltaTime);
        }
    }

    public void GetBonus(BonusParent bonus)
    {
        if (bonus.type == BonusType.Player)
            GetPlayerBonus((PlayerBonus)bonus);
        else
            GetWeaponBonus(bonus);
    }

    private void GetWeaponBonus(BonusParent bonus)
    {
        bonus.AddBonus(this);
    }

    private void GetPlayerBonus(PlayerBonus bonus)
    {
        bonus.AddBonus(this);
        bonuses.Add(bonus);
    }
}
