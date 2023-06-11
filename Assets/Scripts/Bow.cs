using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;

    private GameObject targetEnemy;
    public PlayerScript playerScript;
    public float attackRadius = 5f; 
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && playerScript.canAttack  && playerScript._bowCooldownService.cooldown < 1)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                // проверяем расстояние между игроком и врагом
                float distance = Vector2.Distance(transform.position, hit.collider.transform.position);
                if (distance <= attackRadius)
                {
                    targetEnemy = hit.collider.gameObject;
                }
            }
        }

        if (targetEnemy != null)
        {
            Vector2 direction = (targetEnemy.transform.position - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            playerScript.GetCooldown("bow");
            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            targetEnemy = null;
            playerScript.canAttack = false;
        }
    }
    
    
    void OnDrawGizmosSelected()
    {
     
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
