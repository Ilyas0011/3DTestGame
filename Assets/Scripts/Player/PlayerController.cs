using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private ShootingController shootingController;
    [SerializeField] private ZombieController zombieController;
    [SerializeField] private GameObject nearestZombie; //Ближайший зомби
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Slider healthBar;


    private Rigidbody rb;
    private Animator anim;

    private float moveSpeed;

    private int healthPoint = 100;
    private int damagePower = 20;

    private bool isSearchingZombies = true;
    private bool deathPlayer = false;
    void Start()
    {
        ApplyHealthSettings();
        ApplyDamageSettings();
        ApplyMoveSpeed();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    public IEnumerator RestartScene(float timer)
    {
        yield return new WaitForSeconds(timer);
        SceneManager.LoadScene(0);
    }

    public int ReturnDamage()
    {
        ApplyDamageSettings();
        return damagePower;
    }

    public void ApplyDamageSettings()
    {
        damagePower = PlayerPrefs.GetInt("DamagePowerPlayer", 20);
    }

    public void PauseActivate()
    {
        Time.timeScale = 0;
    }

    public void ApplyMoveSpeed()
    {
        moveSpeed = PlayerPrefs.GetInt("moveSpeedPlayer", 9);
    }

    public void ApplyHealthSettings()
    {
        healthPoint = PlayerPrefs.GetInt("HP", 100);

        if(healthPoint < 0)
            healthPoint = 1;

        healthBar.value = healthPoint;
    }

    public void RemoveNarestZombie()
    {
        nearestZombie = null;
        shootingController.ShootingOff();
    }

    public Transform ReturnTransform()
    {
        return transform;
    }

    public void Damage(int damageAmount)
    {
        healthPoint -= damageAmount;

        if (healthPoint <= 0)
            PlayerDeath();

        healthBar.value = healthPoint;
    }

    private void PlayerDeath()
    {
        deathPlayer = true;
        healthPoint = 0;
        moveSpeed = 0;
        anim.SetBool("isDeath", true);
        zombieController.StopZombies();
        shootingController.ShootingOff();
        StartCoroutine(RestartScene(1f));
    }

    void FixedUpdate()
    {
        if (!deathPlayer)
        {
            MovePlayer();

            if (isSearchingZombies) StartCoroutine(FindNearestZombie(0.5f));
        }
    }

    private void MovePlayer()
    {
        float horizontalMovement = joystick.Horizontal * moveSpeed;
        float verticalMovement = joystick.Vertical * moveSpeed;

        rb.velocity = new Vector3(horizontalMovement, rb.velocity.y, verticalMovement);


        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            if (nearestZombie != null)
            {
                RotateTowardsZombie();
                anim.SetBool("isRunning", true);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(rb.velocity);
                anim.SetBool("isRunning", true);
            }
        }
        else
        {
            if (nearestZombie != null)
                RotateTowardsZombie();

            anim.SetBool("isRunning", false);
        }
    }

    private void RotateTowardsZombie()
    {
        Vector3 direction = (nearestZombie.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
        
        if(nearestZombie != null) shootingController.ShootingOn();
        else shootingController.ShootingOff();
    }

    private IEnumerator FindNearestZombie(float timer)
    {
        isSearchingZombies = false;
        yield return new WaitForSeconds(timer);
        nearestZombie = null; 
        float nearestDistance = 10f; //Расстояние с которого начинается поиск
        Vector3 characterPosition = transform.position;

        foreach (var zombie in zombieController.zombieList)
        {
            float distance = Vector3.Distance(characterPosition, zombie.transform.position);

            // Если расстояние до текущего зомби меньше предыдущего ближайшего,
            // обновляем ближайшего зомби и расстояние до него
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestZombie = zombie;
            }
        }

        isSearchingZombies = true;

    }
}

