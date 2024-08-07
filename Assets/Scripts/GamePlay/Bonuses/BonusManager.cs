using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    [SerializeField] private GameObject[] bonusWeapons;
    [SerializeField] private GameObject[] playerBonuses;
    [SerializeField] private PlayerShooting playerShooting;
    private GameObject activeBonus;

    private List<string> weapons = new List<string>()
    {
        "Pistol", "GranadeLauncher", "AssaultRifle", "Shotgun"
    };
    private string excludedWeapon;
    private float spawnBonusWeaponIn = 10f, spawnPlayerBonuIn = 27f, weaponTimer, playerBonusTimer, height, width;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        height = Screen.height;
        width = Screen.width;

        for(int i = 0; i < bonusWeapons.Length; i++)
        {
            bonusWeapons[i] = Instantiate(bonusWeapons[i], Vector3.zero, Quaternion.identity);
            bonusWeapons[i].SetActive(false);
        }
        for (int i = 0; i < playerBonuses.Length; i++)
        {
            playerBonuses[i] = Instantiate(playerBonuses[i], Vector3.zero, Quaternion.identity);
            playerBonuses[i].SetActive(false);
        }
    }

    private void Update()
    {
        weaponTimer += Time.deltaTime;
        playerBonusTimer += Time.deltaTime;

        if(weaponTimer >= spawnBonusWeaponIn)
        {//исключить оружие в руках у игрока, а потом вернуть его в список
            excludedWeapon = playerShooting.WeaponName;
            weapons.Remove(excludedWeapon);
            SpawnWeapon(weapons[Random.Range(0, weapons.Count)]);
            weapons.Add(excludedWeapon);
            weaponTimer = 0f;
        }
        if(playerBonusTimer >= spawnPlayerBonuIn)
        {
            playerBonusTimer = 0;
            SpawnBonus();
        }
    }

    private void SpawnWeapon(string name)
    {
        switch (name)//выдать игроку новое оружие
        {
            case "Pistol":
                bonusWeapons[0].transform.SetPositionAndRotation(PickAPos(), Quaternion.identity);
                bonusWeapons[0].SetActive(true);
                break;
            case "GranadeLauncher":
                bonusWeapons[1].transform.SetPositionAndRotation(PickAPos(), Quaternion.identity);
                bonusWeapons[1].SetActive(true);
                break;
            case "AssaultRifle":
                bonusWeapons[2].transform.SetPositionAndRotation(PickAPos(), Quaternion.identity);
                bonusWeapons[2].SetActive(true);
                break;
            case "Shotgun":
                bonusWeapons[3].transform.SetPositionAndRotation(PickAPos(), Quaternion.identity);
                bonusWeapons[3].SetActive(true);
                break;
        }
    }

    private void SpawnBonus()
    {
        activeBonus = playerBonuses[Random.Range(0, playerBonuses.Length)];
        activeBonus.transform.SetPositionAndRotation(PickAPos(), Quaternion.identity);
        activeBonus.SetActive(true);
    }

    private Vector3 PickAPos()
    {
        Vector3 t = cam.ScreenToWorldPoint(new Vector3(Random.Range(0 + 5, width - 5), Random.Range(0 + 5, height - 5)));
        t.z = 0f;
        return t;
    }
}
