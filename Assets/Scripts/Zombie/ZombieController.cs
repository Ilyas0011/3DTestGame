using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public List<GameObject> zombieList = new List<GameObject>();

    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform[] spawnerTransform = new Transform[8];
    [SerializeField] private GameObject zombiePrefab;

    private float moveSpeed = 3f;

    private int healthPoint = 100;
    private int damageAmount = 10;

    private bool isSpawned = true;
    private bool playerDeath = false;

    private void Start()
    {
        ApplyHealthSettings();
        damageAmount = PlayerPrefs.GetInt("DamageZombieAmount", 10);
        moveSpeed = PlayerPrefs.GetFloat("SpeedZombie", 3f);
    }

    public int ReturnDamageAmount()
    {
        damageAmount = PlayerPrefs.GetInt("DamageZombieAmount", 10);
        return damageAmount;
    }

    public float ReturnSpeed()
    {
        moveSpeed = PlayerPrefs.GetFloat("SpeedZombie", 3f);
        return moveSpeed;
    }

    public void ApplyZombiesSettings()
    {
        damageAmount = PlayerPrefs.GetInt("DamageZombieAmount", 10);
        moveSpeed = PlayerPrefs.GetFloat("SpeedZombie", 3f);
        foreach (GameObject zombie in zombieList)
        {
            zombie.GetComponent<ZombieCharacter>().ApplyDamageAmount();
            zombie.GetComponent<ZombieCharacter>().ApplyMoveSpeed();
        }
    }


    public int ReturnHealthPoint()
    {
        return healthPoint;
    }

    public void ApplyHealthSettings()
    {
        healthPoint = PlayerPrefs.GetInt("ZombieHealth", 100);
    }

    void FixedUpdate()
    {
        if(isSpawned && !playerDeath)
           StartCoroutine(ZombieSpawn(Random.Range(0.5f, 3f)));
    }
    public void RemoveObjectAndList(GameObject zombie)
    {
        zombieList.Remove(zombie);

        playerController.RemoveNarestZombie();
    }

    public void StopZombies()
    {
        isSpawned = false;
        playerDeath = true;
        foreach (GameObject zombie in zombieList)
        {
            zombie.GetComponent<ZombieCharacter>().PlayerDeath();
        }
    }

    private IEnumerator ZombieSpawn(float timer)
    {
        isSpawned = false;
        yield return new WaitForSeconds(timer);
        Vector3 spawnPosition = spawnerTransform[Random.Range(0,8)].position;

        GameObject newZombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        zombieList.Add(newZombie);
        ZombieCharacter newZombieScript = newZombie.GetComponent<ZombieCharacter>();
        newZombieScript.SetScriptReference(this);
        newZombieScript.SetPlayerTransform(playerController);
        isSpawned = true;
    }
        
}
