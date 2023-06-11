using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    public float speed = 5f;
    public LayerMask obstacleMask;
    public string enemyTag = "Enemy";

    private Transform target;
    private Rigidbody2D rb;

    void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        Vector2 direction = target.position - transform.position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, direction.magnitude, obstacleMask);
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider.CompareTag(enemyTag)) {
                Vector2 normal = hit.normal;
                direction = Vector2.Reflect(direction, normal);
                break;
            }
        }
        
        //todo присобачить к enemy script чтобы двигаться в этом direction
        transform.Translate(direction.normalized * speed * Time.deltaTime);
            //     rb.velocity = direction.normalized * speed;
    }
    
    
    
    
    
}
