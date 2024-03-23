using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime = 0.5f;
    //public float distance;
    //public int damage;
    //public LayerMask whatIsSolid;

    void Start()
    {
        DestroyBullet();


    }

    private void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject, lifetime);
    }

}
