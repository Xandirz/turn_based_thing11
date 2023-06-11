using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDaggerScript : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
