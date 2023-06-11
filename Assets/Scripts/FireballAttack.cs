using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange;
    public int attackDamage;
    public PlayerScript playerScript;
    public float bulletSpeed = 10f;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && playerScript.canAttack && playerScript._fireballCooldownService.cooldown < 1)
        {
            Attack();
            Debug.Log("ATTACK");
        }

    }
    
    void Attack()
    {
        
      
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        if (hitObjects != null)
        {
            foreach (Collider2D obj in hitObjects)
            {
                if (obj.CompareTag("Enemy"))
                {
                    var enemyScript = obj.GetComponent<EnemyScript>();
                        //enemyScript.TakeDamage(attackDamage);
                    playerScript.canAttack = false;
                    Vector2 direction = (obj.transform.position - shootPoint.position).normalized;
                    GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
                    
                    playerScript.GetCooldown("fireball");

                    
                    var fireballScript = bullet.GetComponent<FireballScript>();
                    fireballScript.enemyObject = enemyScript.gameObject;
                    
                    // StartCoroutine(fireballScript.StartAttack(enemyScript, attackPoint.position, obj));
                    bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

                   // targetEnemy = null;
                    playerScript.canAttack = false;
                    
                    
                }
            }

            hitObjects = null;
            
        }
    }


    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
