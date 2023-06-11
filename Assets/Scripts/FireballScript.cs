using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    public int damage = 1;
    public GameObject enemyObject;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == enemyObject)  
        {
            collision.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public IEnumerator StartAttack(EnemyScript enemyScript, Vector3 attackPointPosition, Collider2D collider2D1)
    {
        bool isFlying = true;
        while (isFlying)
        {
            var direction = (attackPointPosition - transform.position);

            if (direction.magnitude < 1f)
            {
                isFlying = false;
            }
            
            transform.Translate(direction.normalized * 11f);
            yield return null;
        }
        
        enemyScript.TakeDamage(damage);
        Destroy(gameObject);
    }
}
