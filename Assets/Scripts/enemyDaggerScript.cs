using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDaggerScript : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
