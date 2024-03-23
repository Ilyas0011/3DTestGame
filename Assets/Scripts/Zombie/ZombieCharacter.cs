using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ZombieCharacter : MonoBehaviour
{
    [SerializeField] ZombieController zombieController;
    [SerializeField] PlayerController playerController;
    [SerializeField] private Slider healthBar;

    private Rigidbody rb;
    private Animator anim;

    private bool isMovementAllowed = true;
    private bool isPlayerDeath = false;
    private bool isBeatPlayer = false;
    private bool isDeath = false;

    private float timeElapsedDamage = 0.8f; //Время прошедшее с последнего дамага по игроку
    private float moveSpeed = 3f;

    private int healthPoints = 100;
    private int damageAmount = 10; //Количество урона наносимого игроку

    public void SetScriptReference(ZombieController script) // Метод для установки ссылки на скрипт
    {
        zombieController = script;
    }

    public void SetPlayerTransform(PlayerController _playerController) // Метод для установки ссылки на скрипт
    {
        playerController = _playerController;
    }

    public void ApplyDamageAmount()
    {
        damageAmount = zombieController.ReturnDamageAmount();
    }

    public void ApplyMoveSpeed()
    {
        moveSpeed = zombieController.ReturnSpeed();
    }

    void Start()
    {
        ApplyDamageAmount();
        ApplyMoveSpeed();
        healthPoints = zombieController.ReturnHealthPoint();
        healthBar.value = healthPoints;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Damage()
    {
        healthPoints -= playerController.ReturnDamage();
        healthBar.value = healthPoints;

        if (healthPoints <= 0) Death();
    }

    public void PlayerDeath()
    {
        isPlayerDeath = true;
        anim.SetBool("isIdle", true);
    }

    private void Death()
    {
        healthPoints = 0;

        isDeath = true;
        isMovementAllowed = false;

        anim.SetBool("isDeath", true);
        Destroy(healthBar);
        Destroy(this.gameObject, 5f);

        zombieController.RemoveObjectAndList(this.gameObject);
    }

    void FixedUpdate()
    {
        if (!isPlayerDeath && !isDeath)
        {
            if (playerController != null && isMovementAllowed) Run();

            if (isBeatPlayer)
            {
                BeatPlayer();
            }
            else
            {
                anim.SetBool("isBeat", false);
             
            }
        }
    }

    private void BeatPlayer()
    {
        isMovementAllowed = false;
        timeElapsedDamage += Time.deltaTime; // Увеличиваем время, прошедшее с последнего обновления

        if (timeElapsedDamage >= 0.8f)
        {
            anim.SetBool("isBeat", true);
            playerController.Damage(damageAmount);
            timeElapsedDamage = 0f;
        }
        
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject.CompareTag("Bullet"))
        {
            Damage();
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            isBeatPlayer = true;
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isBeatPlayer = false;
            StartCoroutine(MoveTrue(0.5f));
        }
    }

    private IEnumerator MoveTrue(float timer) //Когда игрок отходит, зомби не сразу бежит за ним
    {
        yield return new WaitForSeconds(timer);
        isMovementAllowed = true;
    }

    private void Run()
    {
        Transform playerTransform = playerController.ReturnTransform();
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = lookRotation;

        rb.velocity = direction * moveSpeed;
    }
}
